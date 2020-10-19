using NumberCruncher.Components;
using NumberCruncher.Helpers;
using NumberCruncher.Screens.MainMap;
using SadSharp.Helpers;
using SadSharp.Powers;
using System;
using System.Linq;

namespace NumberCruncher.Systems.Powerups
{
    public class BlinkPowerup : Powerup
    {
        public BlinkPowerup() : base("B", "Blink", "B) Blink") {}

        public override Func<IGameData, ITargeter> Targeter => null;

        public override MoveResult Activate(string activator, IGameData gameData)
        {
            var ecs = gameData.Ecs;
            var terrain = gameData.Terrain;
            var pos = ecs.Get<SadWrapperComponent>(activator);

            var possible = terrain
                .GetAllCells()
                .WhereWalkable()
                .WithMinDistance(pos.ToXnaPoint(), 5)
                .WithNoneOnSpace(ecs)
                .ToList();

            var chosen = possible.PickRandom();

            return MoveSystem.TryTeleport(activator, pos.ToXnaPoint(), chosen.ToXnaPoint(), ecs, terrain);
        }
    }
}
