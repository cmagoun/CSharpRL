using CsEcs;
using NumberCruncher.Animation;
using NumberCruncher.Components;
using NumberCruncher.Screens.MainMap;
using RogueSharp;
using SadSharp.Helpers;
using SadSharp.MapCreators;
using SharpDX;
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
            //Debug.WriteLine($"Moving: {from.X}/{from.Y} -> {to.X}/{to.Y}");
            var onSpace = ecs.EntitiesInIndex(Program.SadWrapper, to.ToKey());

            var result = CheckForBlockedSpace(to, terrain)
                ?? CheckForBumpTrigger(entityId, onSpace, ecs);

            if (result != MoveResult.Continue) return result;

            result = DoMove(entityId, from, to, ecs);

            result = CheckForStepTrigger(entityId, onSpace, result, ecs);

            return result;
        }

        public static MoveResult TryTeleport(string entityId, Point from, Point to, Ecs ecs, Map<RogueCell>terrain)
        {
            //This is the same as try move, we can DRY this up in the future
            var onSpace = ecs.EntitiesInIndex(Program.SadWrapper, to.ToKey());

            var result = CheckForBlockedSpace(to, terrain)
                ?? CheckForBumpTrigger(entityId, onSpace, ecs);

            if (result != MoveResult.Continue) return result;

            result = DoTeleport(entityId, from, to, ecs);

            result = CheckForStepTrigger(entityId, onSpace, result, ecs);

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
            if (!onSpace.Any()) return MoveResult.Continue;

            //only take the first bumper based on order?
            var bumper = ecs
                .GetComponents<BumpTriggerComponent>(onSpace.ToArray())
                .OrderBy(b => b.Order)
                .FirstOrDefault();

            return bumper?.Interaction?.Activate(entity, bumper.EntityId, ecs);
        }

        private static MoveResult CheckForStepTrigger(string entity, List<string>onSpace, MoveResult currentResult, Ecs ecs)
        {
            if (!onSpace.Any()) return currentResult;

            var trigger = ecs
                .GetComponents<StepTriggerComponent>(onSpace.ToArray())
                .OrderBy(b => b.Order)
                .FirstOrDefault();

            return trigger?.Interaction?.Activate(entity, trigger.EntityId, ecs, currentResult) ?? currentResult;
        }

        public static MoveResult DoMove(string entityId, Point from, Point to, Ecs ecs)
        {
            ecs.Get<SadWrapperComponent>(entityId)
                .ChangePositionPendingAnimation(to.X, to.Y);

            ecs.AddComponent(
                entityId,
                Animations.Slide(entityId, from, to, .1f));

            return MoveResult.Done(BaseCost);
        }

        public static MoveResult DoTeleport(string entityId, Point from, Point to, Ecs ecs)
        {
            //for now instant and no animation
            ecs.Get<SadWrapperComponent>(entityId)
                .ChangePosition(to.X, to.Y);

            return MoveResult.Done(BaseCost);
        }

        public static MoveResult TakeRandomMove(string entityId, IGameData game)
        {
            var pos = game.Ecs.Get<SadWrapperComponent>(entityId);
            var mresult = MoveResult.Blocked;

            while (mresult.Status != MoveStatus.Done)
            {
                var dx = Roller.NextD3 - 2;
                var dy = Roller.NextD3 - 2;

                if (dx == 0 && dy == 0)
                {
                    mresult = MoveResult.Blocked;
                }
                else
                {
                    mresult = MoveSystem.TryMove(
                        entityId,
                        pos.ToXnaPoint(),
                        pos.ToXnaPoint(dx, dy),
                        game.Ecs,
                        game.Terrain);
                }
            }

            return mresult;
        }

        public static MoveResult TakeStalkerMove(string entityId, IGameData data)
        {
            //Tracks the player unerringly and follows him
            var map = data.Terrain;
            var ecs = data.Ecs;

            var myPos = ecs.Get<SadWrapperComponent>(entityId);
            var from = myPos.ToXnaPoint();

            var path = GetPath(myPos, map, ecs);
            var nextStep = path.StepForward();

            var to = new Point(nextStep.X, nextStep.Y);

            var mresult = MoveSystem.TryMove(entityId, from, to, data.Ecs, data.Terrain);

            //If I cannot move, just skip me
            return mresult.Status == MoveStatus.Blocked ? MoveResult.Done() : mresult;
        }

        private static Path GetPath(SadWrapperComponent myPos, Map<RogueCell> map, Ecs ecs)
        {
            var ppos = ecs.Get<SadWrapperComponent>(Program.Player);

            var pf = new GoalMap<RogueCell>(map, true);
            pf.AddGoal(ppos.X, ppos.Y, 1);

            //var blocksMove = ecs.GetComponents<BlocksMoveComponent, PositionComponent>();
            //pf.AddObstacles(blocksMove.Select(bm => bm.Item2.ToRogueSharpPoint()));

            var notDoors = ecs
                .GetComponents<BumpTriggerComponent, SadWrapperComponent>()
                .Where(c =>
                    c.Item2.EntityId != myPos.EntityId &&
                    c.Item2.EntityId != Program.Player)
                .ToList();

            pf.AddObstacles(notDoors.Select(nd => nd.Item2.ToSharpPoint()));

            return pf.FindPath(myPos.X, myPos.Y);
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
