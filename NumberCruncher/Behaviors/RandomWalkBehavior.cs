
using NumberCruncher.Components;
using NumberCruncher.Modes.MainMap;
using NumberCruncher.Systems;
using SadSharp.Helpers;

namespace NumberCruncher.Behaviors
{
    public class RandomWalkBehavior : IBehavior
    {
        public MoveResult TakeAction(string entityId, MainLoopMode game)
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
    }
}
