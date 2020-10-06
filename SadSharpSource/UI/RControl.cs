using System;
using Microsoft.Xna.Framework;

namespace SadSharp.Game.Controls
{
    public class RControl:SadConsole.Console
    {
        public object Data { get; set; }

        public RControl(int width, int height) : base(width, height){}

        public RControl Under(SadConsole.Console other, int dy)
        {
            Position = new Point(other.Position.X, other.Position.Y + other.Height + dy);
            return this;
        }

        public RControl RightOf(SadConsole.Console other, int dx)
        {
            Position = new Point(other.Position.X + other.Width + dx, other.Position.Y);
            return this;
        }
        public RControl WithClickEvent(Action<object, EventArgs> action)
        {
            MouseButtonClicked += (sender, args) => action(sender, args);
            return this;
        }
    }
}
