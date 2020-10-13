using CsEcs.SimpleEdits;
using NumberCruncher.Components;
using NumberCruncher.Screens.MainMap;
using SadSharp.Helpers;
using System.Collections.Generic;

namespace NumberCruncher.Systems
{
    public enum AwarenessResult { MadeAware, MadeUnaware, Aware, Unaware }

    public static class AwarenessSystem
    {
        public static Dictionary<int, int> Chances = new Dictionary<int, int>
        {
            {1, 90},
            {2, 80},
            {3, 50},
            {4, 25},
            {5, 12},
            {6, 12},
            {7, 06},
            {8, 06},
            {9, 06},
            {10, 03},
            {11, 03},
            {12, 03},
            {13, 03},
            {14, 03},
            {15, 03},
            {16, 02},
            {17, 02},
            {18, 02},
            {19, 01},
            {20, 01},
        };

        public static AwarenessResult CheckForAwareness(string entityId, IGameData game)
        {
            var eaware = game.Ecs.Get<AwarenessComponent>(entityId);
            if (eaware.Aware) return AwarenessResult.Aware;

            var ppos = game.Ecs.Get<SadWrapperComponent>(Program.Player);
            var epos = game.Ecs.Get<SadWrapperComponent>(entityId);

            var distance = ppos.ToXnaPoint().MDistance(epos.ToXnaPoint());

            if (distance > 20) return AwarenessResult.Unaware;
            var chance = Chances[distance];
            var roll = Roller.NextD100;

            if(roll <= chance)
            {
                eaware.DoEdit(new BoolEdit(true));
                return AwarenessResult.MadeAware;
            } 
            else
            {
                return AwarenessResult.Unaware;
            }
        }
    }
}
