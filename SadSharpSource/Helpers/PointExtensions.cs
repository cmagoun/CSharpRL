using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace SadSharp.Helpers
{
    public static class PointExtensions
    {
        public static int MDistance(this Point orig, Point other)
        {
            return Math.Max(Math.Abs(orig.X - other.X), Math.Abs(orig.Y - other.Y));
        }
    }
}
