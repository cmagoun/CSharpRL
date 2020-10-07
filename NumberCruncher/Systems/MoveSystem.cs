using CsEcs;
using NumberCruncher.Animation;
using NumberCruncher.Components;
using RogueSharp;
using SadSharp.Helpers;
using SadSharp.MapCreators;
using System.Collections.Generic;
using System.Linq;
using Point = Microsoft.Xna.Framework.Point;

namespace NumberCruncher.Systems
{
    public enum MoveStatus    {Blocked, Done}; //Continue is NULL

    public static class MoveSystem
    {
        public const int BaseCost = 1;

        public static MoveResult TryMove(string entityId, Point from, Point to, Ecs ecs, Map<RogueCell> terrain)
        {
            var onSpace = ecs.EntitiesInIndex(Program.SadWrapper, to.ToKey());

            var result = CheckForBlockedSpace(to, terrain)
                ?? CheckForBumpTrigger(entityId, onSpace, ecs);

            if (result != MoveResult.Continue) return result;

            result = DoMove(entityId, from, to, ecs);

            return result;
        }

        private static MoveResult CheckForBlockedSpace(Point to, Map<RogueCell>terrain)
        {
            var walkable = terrain.GetCell(to.X, to.Y).IsWalkable;

            return !walkable
                ? MoveResult.Blocked
                : MoveResult.Continue;
        }

        private static MoveResult CheckForBumpTrigger(string entity, List<string>onSpace, Ecs ecs )
        {
            //only take the first bumper based on order?
            var bumper = ecs
                .GetComponents<BumpTriggerComponent>(onSpace.ToArray())
                .OrderBy(b => b.Order)
                .FirstOrDefault();

            return bumper?.Interaction?.Activate(entity, bumper.EntityId, ecs);
        }

        public static MoveResult DoMove(string entityId, Point from, Point to, Ecs ecs)
        {
            ecs.AddComponent(
                entityId,
                Animations.Slide(entityId, from, to, .1f));

            return MoveResult.Done(BaseCost);
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
        public static MoveResult Continue => null;
        public static MoveResult Done(int cost = MoveSystem.BaseCost, string triggerId = "") => new MoveResult(MoveStatus.Done, cost, triggerId);
    }
}
