
using ReferenceGame.Components;
using ReferenceGame.Systems;
using SadSharp.Helpers;
using SadSharp.Modes.Map;
using System.Diagnostics;

namespace ReferenceGame.Modes.Entity
{
    public class RandomWalkTurnTaker : ITurnTaker
    {
        public WhoControls Who => WhoControls.Computer;

        public MoveResult TakeTurn(string entityId, MapMode mm)
        {
            var pos = mm.Ecs.Get<EntityWrapperComponent>(entityId);
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
                    mresult = EntityMoveSystem.TryMove(entityId, pos.ToXnaPoint(), pos.ToXnaPoint(dx, dy), mm);
                }            
            }

            return mresult;
        }

    }
}
