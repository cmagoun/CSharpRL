using Microsoft.Xna.Framework.Input;
using NumberCruncher.Components;
using NumberCruncher.Screens.MainMap;
using NumberCruncher.Systems;
using SadSharp.Helpers;

namespace NumberCruncher.Behaviors
{
    public class PlayerBehavior : IBehavior
    {
        public MoveResult TakeAction(string entityId, IGameData data)
        {
            var sad = data.Ecs.Get<SadWrapperComponent>(entityId);
            var mresult = MoveResult.Blocked;
            var kb = data.KeyboardState;

            var from = sad.ToXnaPoint();

            if (kb.IsKeyPressed(Keys.NumPad8)) mresult = MoveSystem.TryMove(entityId, from, from.North(), data.Ecs, data.Terrain);
            if (kb.IsKeyPressed(Keys.NumPad2)) mresult = MoveSystem.TryMove(entityId, from, from.South(), data.Ecs, data.Terrain);
            if (kb.IsKeyPressed(Keys.NumPad6)) mresult = MoveSystem.TryMove(entityId, from, from.East(), data.Ecs, data.Terrain);
            if (kb.IsKeyPressed(Keys.NumPad4)) mresult = MoveSystem.TryMove(entityId, from, from.West(), data.Ecs, data.Terrain);
            if (kb.IsKeyPressed(Keys.NumPad9)) mresult = MoveSystem.TryMove(entityId, from, from.NorthEast(), data.Ecs, data.Terrain);
            if (kb.IsKeyPressed(Keys.NumPad3)) mresult = MoveSystem.TryMove(entityId, from, from.SouthEast(), data.Ecs, data.Terrain);
            if (kb.IsKeyPressed(Keys.NumPad1)) mresult = MoveSystem.TryMove(entityId, from, from.SouthWest(), data.Ecs, data.Terrain);
            if (kb.IsKeyPressed(Keys.NumPad7)) mresult = MoveSystem.TryMove(entityId, from, from.NorthWest(), data.Ecs, data.Terrain);

            if (kb.IsKeyPressed(Keys.D1)) StrengthSystem.ChangeStrength(Program.Player, 1, data.Ecs);
            if (kb.IsKeyPressed(Keys.D2)) StrengthSystem.ChangeStrength(Program.Player, 2, data.Ecs);
            if (kb.IsKeyPressed(Keys.D3)) StrengthSystem.ChangeStrength(Program.Player, 3, data.Ecs);
            if (kb.IsKeyPressed(Keys.D4)) StrengthSystem.ChangeStrength(Program.Player, 4, data.Ecs);
            if (kb.IsKeyPressed(Keys.D5)) StrengthSystem.ChangeStrength(Program.Player, 5, data.Ecs);
            if (kb.IsKeyPressed(Keys.D6)) StrengthSystem.ChangeStrength(Program.Player, 6, data.Ecs);
            if (kb.IsKeyPressed(Keys.D7)) StrengthSystem.ChangeStrength(Program.Player, 7, data.Ecs);
            if (kb.IsKeyPressed(Keys.D8)) StrengthSystem.ChangeStrength(Program.Player, 8, data.Ecs);
            if (kb.IsKeyPressed(Keys.D9)) StrengthSystem.ChangeStrength(Program.Player, 9, data.Ecs);

            kb.Clear();
            return mresult;
        }
    }

}
