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

        public static Point East(this Point p) => new Point(p.X + 1, p.Y);
        public static Point North(this Point p) => new Point(p.X, p.Y - 1);
        public static Point West(this Point p) => new Point(p.X + - 1, p.Y);
        public static Point South(this Point p) => new Point(p.X, p.Y + 1);
        public static Point NorthEast(this Point p) => new Point(p.X + 1, p.Y - 1);
        public static Point NorthWest(this Point p) => new Point(p.X - 1, p.Y - 1);
        public static Point SouthWest(this Point p) => new Point(p.X - 1, p.Y + 1);
        public static Point SouthEast(this Point p) => new Point(p.X + 1, p.Y + 1);

        public static string ToKey(this Point p) => $"{p.X}/{p.Y}";
    }
}
