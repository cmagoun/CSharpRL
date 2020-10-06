using System;
using System.Linq;

namespace SadSharp.Helpers
{
    public static class BoolArray
    {
        //takes an array of bool (ex. TffTTfTf) and returns an integer representing the
        //value of the byte array (10011010 in this case)
        public static int ToInt(this bool[] b)
        {
            if(b.Count() != 8) throw new ArgumentException("You need an array of size 8 for this function");

            double result = 0;

            for (var pos = 7; pos >= 0; pos--)
            {
                if (b[pos]) result += Math.Pow(2, 7 - pos);
            }

            return (int) result;
        }
    }
}
