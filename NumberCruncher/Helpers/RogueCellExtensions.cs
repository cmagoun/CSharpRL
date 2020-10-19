using CsEcs;
using Microsoft.Xna.Framework;
using SadSharp.Helpers;
using SadSharp.MapCreators;
using System.Collections.Generic;
using System.Linq;

namespace NumberCruncher.Helpers
{
    public static class RogueCellExtensions
    {
        public static IEnumerable<RogueCell> WhereWalkable(this IEnumerable<RogueCell>cells)
        {
            return cells.Where(c => c.IsWalkable);
        }

        public static IEnumerable<RogueCell> WithMinDistance(this IEnumerable<RogueCell>cells, Point other, int distance)
        {
            return cells.Where(c => c.ToXnaPoint().MDistance(other) >= distance);
        }

        public static IEnumerable<RogueCell> WithNoneOnSpace(this IEnumerable<RogueCell>cells, Ecs ecs)
        {
            return cells.Where(c => !ecs.EntitiesInIndex(Program.SadWrapper, $"{c.X}/{c.Y}").Any());
        }
    }
}
