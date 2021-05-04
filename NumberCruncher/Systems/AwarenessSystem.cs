using CsEcs.SimpleEdits;
using NumberCruncher.Components;
using NumberCruncher.Screens.MainMap;
using RogueSharp;
using SadSharp.Helpers;
using SadSharp.MapCreators;
using System.Collections.Generic;
using System.Data.Common;

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
        public static int MakeUnawareChance = 8;

        public static AwarenessResult CheckForAwareness(string entityId, IGameData data)
        {
            var epos = data.Ecs.Get<SadWrapperComponent>(entityId);
            var eaware = data.Ecs.Get<AwarenessComponent>(entityId);

            var isInFov = data.CurrentFov.IsInFov(epos.X, epos.Y);

            if(isInFov)
            {          
                if (eaware.Aware) return AwarenessResult.Aware;

                var ppos = data.Ecs.Get<SadWrapperComponent>(Program.Player);
                var distance = ppos.ToXnaPoint().MDistance(epos.ToXnaPoint());

                if (distance > 20) return AwarenessResult.Unaware;
                var chance = Chances[distance];
                var roll = Roller.NextD100;

                if (roll <= chance)
                {
                    eaware.DoEdit(new BoolEdit(true));
                    return AwarenessResult.MadeAware;
                }
                else
                {
                    return AwarenessResult.Unaware;
                }
            } 
            else //NOT in FOV
            {
                if (!eaware.Aware) return AwarenessResult.Unaware;

                var roll = Roller.NextD100;
                if(roll < MakeUnawareChance)
                {
                    eaware.DoEdit(new BoolEdit(false));
                    return AwarenessResult.MadeUnaware;
                } 
                else
                {
                    return AwarenessResult.Aware;
                }
            }
        }
    }
}
