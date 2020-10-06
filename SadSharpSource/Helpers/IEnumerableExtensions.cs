using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace SadSharp.Helpers
{
    public static class IEnumerableExtensions
    {
        public static T PickRandom<T>(this IEnumerable<T> list)
        {
            var roll = Roller.Next(list.Count());
            return list.Skip(roll).First();
        }

        public static T Pop<T>(this List<T> list)
        {
            var result = list.First();
            list.RemoveAt(0);
            return result;
        }
    }
}
