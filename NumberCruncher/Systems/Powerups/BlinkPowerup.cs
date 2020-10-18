using NumberCruncher.Screens.MainMap;
using SadSharp.Powers;
using System;

namespace NumberCruncher.Systems.Powerups
{
    public class BlinkPowerup : Powerup
    {
        public BlinkPowerup() : base("B", "Blink", "B) Blink") {}

        public override Func<IGameData, ITargeter> Targeter => null;

        public override MoveResult Activate(string activator, IGameData gameData)
        {
            throw new NotImplementedException();
        }
    }
}
