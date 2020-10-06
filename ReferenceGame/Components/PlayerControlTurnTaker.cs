using Microsoft.Xna.Framework.Input;
using ReferenceGame.Systems;
using SadSharp.Modes.Map;

namespace ReferenceGame.Components
{
    public class PlayerControlTurnTaker:ITurnTaker
    {
        public WhoControls Who => WhoControls.Player;
        public MoveResult TakeTurn(string entityId, MapMode mm)
        {
            var ppos = mm.Ecs.Get<PositionComponent>(mm.CurrentActor);
            var mresult = new MoveResult(MoveStatus.Blocked);
            var kb = mm.KeyboardState;
            var actor = mm.CurrentActor;

            if (kb.IsKeyPressed(Keys.NumPad8)) mresult = MoveSystem.TryMove(actor, ppos.ToXnaPoint(), ppos.ToXnaPoint(0, -1), mm);
            if (kb.IsKeyPressed(Keys.NumPad6)) mresult = MoveSystem.TryMove(actor, ppos.ToXnaPoint(), ppos.ToXnaPoint(1, 0), mm);
            if (kb.IsKeyPressed(Keys.NumPad2)) mresult = MoveSystem.TryMove(actor, ppos.ToXnaPoint(), ppos.ToXnaPoint(0, 1), mm);
            if (kb.IsKeyPressed(Keys.NumPad4)) mresult = MoveSystem.TryMove(actor, ppos.ToXnaPoint(), ppos.ToXnaPoint(-1, 0), mm);

            if (kb.IsKeyPressed(Keys.NumPad7)) mresult = MoveSystem.TryMove(actor, ppos.ToXnaPoint(), ppos.ToXnaPoint(-1, -1), mm);
            if (kb.IsKeyPressed(Keys.NumPad9)) mresult = MoveSystem.TryMove(actor, ppos.ToXnaPoint(), ppos.ToXnaPoint(1, -1), mm);
            if (kb.IsKeyPressed(Keys.NumPad3)) mresult = MoveSystem.TryMove(actor, ppos.ToXnaPoint(), ppos.ToXnaPoint(1, 1), mm);
            if (kb.IsKeyPressed(Keys.NumPad1)) mresult = MoveSystem.TryMove(actor, ppos.ToXnaPoint(), ppos.ToXnaPoint(-1, 1), mm);


            if (mresult.Status == MoveStatus.Done)
            {
                kb.Clear();
                mm.WaitingOnPlayer = false;
                //Ticker.MeasureHereToHere(); //measures the time between the player moves       
            }
            else
            {
                mm.WaitingOnPlayer = true;
            }
 
            return mresult;
        }
    }
}
