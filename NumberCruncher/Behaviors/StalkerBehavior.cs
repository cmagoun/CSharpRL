using CsEcs;
using NumberCruncher.Components;
using NumberCruncher.Screens.MainMap;
using NumberCruncher.Systems;
using RogueSharp;
using SadSharp.MapCreators;
using System.Linq;
using Point = Microsoft.Xna.Framework.Point;

namespace NumberCruncher.Behaviors
{
    public class StalkerBehavior : IBehavior
    {
        public MoveResult TakeAction(string entityId, IGameData data)
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

        private Path GetPath(SadWrapperComponent myPos, Map<RogueCell> map, Ecs ecs)
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
}
