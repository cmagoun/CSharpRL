using CsEcs.SimpleEdits;
using Microsoft.Xna.Framework;
using ReferenceGame.Components;
using SadSharp.Modes.Map;
using System.Linq;

namespace ReferenceGame.Systems
{
    public enum MoveStatus
    {
        Blocked,
        //Continue,
        Done
    };

    public static class MoveSystem
    {
        public const int BaseCost = 120;

        public static MoveResult TryMove(string entity, Point from, Point to, MapMode mode)
        {
            var onSpace = mode.Ecs.EntitiesInIndex("PositionComponent", $"{to.X}/{to.Y}").ToArray();

            var result = CheckForBumpTriggers(entity, onSpace, mode)
                         ?? CheckForBlockedSpace(to, mode)
                         ?? CheckForMoveBlocker(onSpace, mode);

            if (result != MoveResult.Continue) return result;

            result = DoMove(entity, from, to, mode);
            result = CheckForStepTriggers(result, entity, onSpace, mode);

            return result;
        }

        public static MoveResult CheckForBumpTriggers(string entity, string[] onSpace, MapMode mode)
        {
            //only take the first bumper based on order?
            var bumper = mode.Ecs
                .GetComponents<BumpTriggerComponent>(onSpace)
                .OrderBy(b => b.Order)
                .FirstOrDefault();

            return bumper?.Interaction?.Invoke(entity, bumper.EntityId, mode);
        }

        public static MoveResult CheckForBlockedSpace(Point to, MapMode mode)
        {
            return !mode.Map.GetCell(to.X, to.Y).IsWalkable
                ? MoveResult.Blocked
                : MoveResult.Continue;
        }

        public static MoveResult CheckForMoveBlocker(string[] onSpace, MapMode mode)
        {
            var blocker = mode.Ecs
                .GetComponents<BlocksMoveComponent>(onSpace)
                .FirstOrDefault();

            return blocker != null
                ? MoveResult.Blocked
                : MoveResult.Continue;
        }

        public static MoveResult DoMove(string entity, Point from, Point to, MapMode mode)
        {
            var pos = mode.Ecs.Get<PositionComponent>(entity);
            pos.DoEdit(new PosEdit(to.X, to.Y));
            return MoveResult.Done(BaseCost);
        }

        public static MoveResult CheckForStepTriggers(MoveResult currentResult, string entity, string[] onSpace,
            MapMode mode)
        {

            //Once you've moved, did you step on anything?
            var stepTrigger = mode.Ecs
                .GetComponents<StepTriggerComponent>(onSpace)
                .OrderBy(b => b.Order)
                .FirstOrDefault(); //for now let's just do one

            return stepTrigger?.Interaction?.Invoke(entity, stepTrigger.EntityId, currentResult, mode) ?? currentResult;
        }
    }

    public class MoveResult
    {
        public MoveStatus Status { get; set; }
        public int Cost { get; set; }
        public string TriggerId { get; set; }

        public MoveResult(MoveStatus status, int cost = 0, string triggerId = "")
        {
            Status = status;
            Cost = cost;
            TriggerId = triggerId;
        }

        public static MoveResult Blocked => new MoveResult(MoveStatus.Blocked);
        public static MoveResult Continue => null; //new MoveResult(MoveStatus.Continue);
        public static MoveResult Done(int cost = MoveSystem.BaseCost, string triggerId = "") => new MoveResult(MoveStatus.Done, cost, triggerId);
    }
}
