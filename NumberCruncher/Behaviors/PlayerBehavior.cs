using Microsoft.Xna.Framework.Input;
using NumberCruncher.Components;
using NumberCruncher.Screens.MainMap;
using NumberCruncher.Systems;
using SadSharp.Helpers;
using System.Data;
using System.Linq;

namespace NumberCruncher.Behaviors
{
    public class PlayerBehavior : IBehavior
    {
        //At some point, I need to find a way to limit the keystrokes/time because
        //sometimes, I seem to get extra inputs where I don't want to
        public MoveResult TakeAction(string entityId, IGameData data)
        {
            var mresult = CheckForMove(entityId, data)
                ?? CheckForStrengthChange(entityId, data)
                ?? CheckForPowerActivation(entityId, data);
        
            //kb.Clear();
            return mresult ?? MoveResult.Blocked;
        }

        public MoveResult CheckForMove(string entityId, IGameData data)
        {
            var sad = data.Ecs.Get<SadWrapperComponent>(entityId);
            var from = sad.ToXnaPoint();
            var kb = data.KeyboardState;

            //maybe IsKeyReleased is better? Choppier for sure, but there are no inadvertent double moves
            if (kb.IsKeyReleased(Keys.NumPad8)) return MoveSystem.TryMove(entityId, from, from.North(), data.Ecs, data.Terrain);
            if (kb.IsKeyReleased(Keys.NumPad2)) return MoveSystem.TryMove(entityId, from, from.South(), data.Ecs, data.Terrain);
            if (kb.IsKeyReleased(Keys.NumPad6)) return MoveSystem.TryMove(entityId, from, from.East(), data.Ecs, data.Terrain);
            if (kb.IsKeyReleased(Keys.NumPad4)) return MoveSystem.TryMove(entityId, from, from.West(), data.Ecs, data.Terrain);
            if (kb.IsKeyReleased(Keys.NumPad9)) return MoveSystem.TryMove(entityId, from, from.NorthEast(), data.Ecs, data.Terrain);
            if (kb.IsKeyReleased(Keys.NumPad3)) return MoveSystem.TryMove(entityId, from, from.SouthEast(), data.Ecs, data.Terrain);
            if (kb.IsKeyReleased(Keys.NumPad1)) return MoveSystem.TryMove(entityId, from, from.SouthWest(), data.Ecs, data.Terrain);
            if (kb.IsKeyReleased(Keys.NumPad7)) return MoveSystem.TryMove(entityId, from, from.NorthWest(), data.Ecs, data.Terrain);

            return MoveResult.Continue;

        }

        public MoveResult CheckForStrengthChange(string entityId, IGameData data)
        {
            var kb = data.KeyboardState;
            if (kb.IsKeyReleased(Keys.D1)) StrengthSystem.ChangeStrength(Program.Player, 1, data.Ecs);
            if (kb.IsKeyReleased(Keys.D2)) StrengthSystem.ChangeStrength(Program.Player, 2, data.Ecs);
            if (kb.IsKeyReleased(Keys.D3)) StrengthSystem.ChangeStrength(Program.Player, 3, data.Ecs);
            if (kb.IsKeyReleased(Keys.D4)) StrengthSystem.ChangeStrength(Program.Player, 4, data.Ecs);
            if (kb.IsKeyReleased(Keys.D5)) StrengthSystem.ChangeStrength(Program.Player, 5, data.Ecs);
            if (kb.IsKeyReleased(Keys.D6)) StrengthSystem.ChangeStrength(Program.Player, 6, data.Ecs);
            if (kb.IsKeyReleased(Keys.D7)) StrengthSystem.ChangeStrength(Program.Player, 7, data.Ecs);
            if (kb.IsKeyReleased(Keys.D8)) StrengthSystem.ChangeStrength(Program.Player, 8, data.Ecs);
            if (kb.IsKeyReleased(Keys.D9)) StrengthSystem.ChangeStrength(Program.Player, 9, data.Ecs);

            //change strength never ends turn
            return MoveResult.Continue;
        }

        public MoveResult CheckForPowerActivation(string entityId, IGameData data)
        {
            var kb = data.KeyboardState;
            if (!kb.KeysReleased.Any()) return MoveResult.Continue;

            var key = kb.KeysReleased.Last();
            var inv = data.Ecs.Get<InventoryComponent>(entityId);

            if (inv.Items.TryGetValue(key.Character.ToString().ToUpper(), out var power))
            {
                return PowerSystem.ActivatePower(entityId, power, data);
            }
            else
            {
                return MoveResult.Continue;
            }
        }
    }

}
