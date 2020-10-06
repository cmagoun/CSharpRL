using CsEcs;
using NumberCruncher.Components;
using System.Linq;

namespace NumberCruncher.Systems
{
    public static class SchedulingSystem
    {
        public static string FindNext(Ecs ecs)
        {
            //returns the guy with the highest number of AP remaining
            return ecs.GetComponents<ActionPointsComponent>()
                .Where(ap => ap.ActionPoints >= 1.0)
                .OrderByDescending(ap => ap.ActionPoints)
                .FirstOrDefault()
                ?.EntityId;
        } 
    }
}
