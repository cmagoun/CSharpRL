
using Microsoft.Xna.Framework;

namespace SadSharp.Helpers
{
    public static class ColorExtensions
    {
        public static Color Bright(this Color c, float mult = 1.2F)
        {
            var result = Color.Multiply(c, mult);
            return result;
        }

        public static Color Dim(this Color c, float mult = .90F)
        {
            var result = Color.Multiply(c, mult);
            return result;
        }
    }
}
