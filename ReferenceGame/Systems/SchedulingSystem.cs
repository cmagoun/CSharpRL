using ReferenceGame.Components;
using SadSharp.Modes.Map;
using System.Linq;

namespace ReferenceGame.Systems
{
    //How many turntakers will this support with any speed?
    //I could keep an ordered list of TT, but that would be much more complicated
    public static class SchedulingSystem 
    {
        public static TurnTakerComponent NextTurn(MapMode mm)
        {
            return mm.Ecs
                .GetComponents<TurnTakerComponent>()
                .OrderBy(x => x.NextTurn)
                .FirstOrDefault();
        }
    }
}
