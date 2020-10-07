using Microsoft.Xna.Framework.Input;
using NumberCruncher.Components;
using NumberCruncher.Modes.MainMap;
using NumberCruncher.Systems;
using SadSharp.Helpers;

namespace NumberCruncher.Behaviors
{
    public class PlayerBehavior : IBehavior
    {
        public MoveResult TakeAction(string entityId, MainLoopMode game)
        {
            var sad = game.Ecs.Get<SadWrapperComponent>(entityId);
            var mresult = MoveResult.Blocked;
            var kb = game.KeyboardState;

            var from = sad.ToXnaPoint();

            if (kb.IsKeyPressed(Keys.NumPad8)) mresult = MoveSystem.TryMove(entityId, from, from.North(), game.Ecs, game.Terrain);
            if (kb.IsKeyPressed(Keys.NumPad2)) mresult = MoveSystem.TryMove(entityId, from, from.South(), game.Ecs, game.Terrain);
            if (kb.IsKeyPressed(Keys.NumPad6)) mresult = MoveSystem.TryMove(entityId, from, from.East(), game.Ecs, game.Terrain);
            if (kb.IsKeyPressed(Keys.NumPad4)) mresult = MoveSystem.TryMove(entityId, from, from.West(), game.Ecs, game.Terrain);
            if (kb.IsKeyPressed(Keys.NumPad9)) mresult = MoveSystem.TryMove(entityId, from, from.NorthEast(), game.Ecs, game.Terrain);
            if (kb.IsKeyPressed(Keys.NumPad3)) mresult = MoveSystem.TryMove(entityId, from, from.SouthEast(), game.Ecs, game.Terrain);
            if (kb.IsKeyPressed(Keys.NumPad1)) mresult = MoveSystem.TryMove(entityId, from, from.SouthWest(), game.Ecs, game.Terrain);
            if (kb.IsKeyPressed(Keys.NumPad7)) mresult = MoveSystem.TryMove(entityId, from, from.NorthWest(), game.Ecs, game.Terrain);

            if (kb.IsKeyPressed(Keys.D1)) StrengthSystem.ChangeStrength(Program.Player, 1, game.Ecs);
            if (kb.IsKeyPressed(Keys.D2)) StrengthSystem.ChangeStrength(Program.Player, 2, game.Ecs);
            if (kb.IsKeyPressed(Keys.D3)) StrengthSystem.ChangeStrength(Program.Player, 3, game.Ecs);
            if (kb.IsKeyPressed(Keys.D4)) StrengthSystem.ChangeStrength(Program.Player, 4, game.Ecs);
            if (kb.IsKeyPressed(Keys.D5)) StrengthSystem.ChangeStrength(Program.Player, 5, game.Ecs);
            if (kb.IsKeyPressed(Keys.D6)) StrengthSystem.ChangeStrength(Program.Player, 6, game.Ecs);
            if (kb.IsKeyPressed(Keys.D7)) StrengthSystem.ChangeStrength(Program.Player, 7, game.Ecs);
            if (kb.IsKeyPressed(Keys.D8)) StrengthSystem.ChangeStrength(Program.Player, 8, game.Ecs);
            if (kb.IsKeyPressed(Keys.D9)) StrengthSystem.ChangeStrength(Program.Player, 9, game.Ecs);

            kb.Clear();
            return mresult;
        }
    }

}
