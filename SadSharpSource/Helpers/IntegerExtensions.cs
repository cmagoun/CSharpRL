using System;
using System.Linq;

namespace SadSharp.Helpers
{
    public static class IntegerExtensions
    {
        public static string Pad(this int i, int places, string padStr = " ")
        {
            if (places < 2 || places > 9) throw new ArgumentException("Places should be between 2 and 9");
            var asString = i.ToString();

            var numPad = Math.Max(0, places - asString.Length);
            var pad = String.Join("", Enumerable.Range(0, numPad).Select(x => padStr));

            return $"{pad}{i}";
        }
    }
}
