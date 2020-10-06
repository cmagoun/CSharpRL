using System;
using System.Collections.Generic;

namespace CsEcs
{
    public static class ForEachExtension
    {
        public static void ForEach<T>(this IEnumerable<T> list, Action<T> action)
        {
            foreach (var item in list)
            {
                action.Invoke(item);
            }
        }

        public static void ForEach<T>(this IEnumerable<T> list, Action<T, int> action)
        {
            int i = 0;
            foreach (var item in list)
            {
                action.Invoke(item, i);
                i++;
            }
        }
    }
}
