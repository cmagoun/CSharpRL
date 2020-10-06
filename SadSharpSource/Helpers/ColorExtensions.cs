
using Microsoft.Xna.Framework;

namespace SadSharp.Helpers
{
    public static class ColorExtensions
    {
        public static Color Bright(this Color c)
        {
            var result = Color.Multiply(c, 1.2F);
            return result;
        }

        public static Color Dim(this Color c)
        {
            var result = Color.Multiply(c, .90F);
            return result;
        }
    }
}
