using Microsoft.Xna.Framework.Input;
using NumberCruncher.Components;
using NumberCruncher.Modes.MainMap;
using NumberCruncher.Systems;
using ReferenceGame.Components;
using System;
using System.Collections.Generic;
using System.Text;

namespace NumberCruncher.Behaviors
{
    public class PlayerBehavior : IBehavior
    {
        public MoveResult TakeAction(string entityId, MainLoopMode game)
        {
            var sad = game.Ecs.Get<SadWrapperComponent>(entityId);
            var mresult = MoveResult.Blocked;
            var kb = game.KeyboardState;

            if (kb.IsKeyPressed(Keys.NumPad5))
            {
                return MoveResult.Done();
            }



            return mresult;
        }
    }


    //public MoveResult TakeTurn(string entityId, MapMode mm)
    //{

    //    var ppos = mm.Ecs.Get<EntityWrapperComponent>(mm.CurrentActor);
    //    var mresult = new MoveResult(MoveStatus.Blocked);
    //    var kb = mm.KeyboardState;
    //    var actor = mm.CurrentActor;

    //    if (kb.IsKeyPressed(Keys.Enter)) return MoveResult.Done();

    //    if (kb.IsKeyPressed(Keys.NumPad8)) mresult = EntityMoveSystem.TryMove(actor, ppos.ToXnaPoint(), ppos.ToXnaPoint(0, -1), mm);
    //    if (kb.IsKeyPressed(Keys.NumPad6)) mresult = EntityMoveSystem.TryMove(actor, ppos.ToXnaPoint(), ppos.ToXnaPoint(1, 0), mm);
    //    if (kb.IsKeyPressed(Keys.NumPad2)) mresult = EntityMoveSystem.TryMove(actor, ppos.ToXnaPoint(), ppos.ToXnaPoint(0, 1), mm);
    //    if (kb.IsKeyPressed(Keys.NumPad4)) mresult = EntityMoveSystem.TryMove(actor, ppos.ToXnaPoint(), ppos.ToXnaPoint(-1, 0), mm);

    //    if (kb.IsKeyPressed(Keys.NumPad7)) mresult = EntityMoveSystem.TryMove(actor, ppos.ToXnaPoint(), ppos.ToXnaPoint(-1, -1), mm);
    //    if (kb.IsKeyPressed(Keys.NumPad9)) mresult = EntityMoveSystem.TryMove(actor, ppos.ToXnaPoint(), ppos.ToXnaPoint(1, -1), mm);
    //    if (kb.IsKeyPressed(Keys.NumPad3)) mresult = EntityMoveSystem.TryMove(actor, ppos.ToXnaPoint(), ppos.ToXnaPoint(1, 1), mm);
    //    if (kb.IsKeyPressed(Keys.NumPad1)) mresult = EntityMoveSystem.TryMove(actor, ppos.ToXnaPoint(), ppos.ToXnaPoint(-1, 1), mm);

    //    return mresult;
    //}
}
