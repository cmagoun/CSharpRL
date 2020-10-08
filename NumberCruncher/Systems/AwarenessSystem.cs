using CsEcs;
using NumberCruncher.Components;
using NumberCruncher.Modes.MainMap;
using SadSharp.Game;
using SadSharp.Helpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace NumberCruncher.Systems
{
    public enum AwarenessResult { MadeAware, MadeUnaware, Aware, Unaware }

    public static class AwarenessSystem
    {
        public static Dictionary<int, int> Chances = new Dictionary<int, int>
        {
            {1, 80},
            {2, 40},
            {3, 20},
            {4, 10},
            {5, 10},
            {6, 10},
            {7, 05},
            {8, 05},
            {9, 05},
            {10, 02},
            {11, 02},
            {12, 02},
            {13, 02},
            {14, 02},
            {15, 02},
            {16, 01},
            {17, 01},
            {18, 01},
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
                return AwarenessResult.MadeAware;
            } 
            else
            {
                return AwarenessResult.Unaware;
            }
        }
    }
}
