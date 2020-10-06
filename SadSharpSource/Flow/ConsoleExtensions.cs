using Microsoft.Xna.Framework;

namespace SadSharp.Game
{
    public static class ConsoleExtensions
    {
        public static GameConsole WithBorder(this GameConsole c, Color? borderColor = null)
        {
            return new BorderConsole(c);
        }

        public static GameConsole RightOf(this GameConsole c, GameConsole other, int dx, int? dy = null)
        {
            c.Position = new Point(other.X + other.Width + dx, other.Y + dy??0);
            return c;
        }

        public static GameConsole Under(this GameConsole c, GameConsole other, int dy, int? dx = null)
        {
            c.Position = new Point(other.X + dx??0, other.Y + other.Height + dy);
            return c;
        }
    }
}
