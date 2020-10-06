using Microsoft.Xna.Framework;
using SadConsole;

namespace SadSharp.Game
{
    public interface IGameConsole
    {
        void SetParentMode(IGameMode mode);
    }

    public abstract class GameConsole : SadConsole.Console, IGameConsole
    {
        protected IGameMode GameMode;
        public abstract string MyKey { get; }
        public int X => Position.X;
        public int Y => Position.Y;
        public virtual bool IsBordered => false;
 
        protected GameConsole(int width, int height, int posX, int posY) : base(width, height)
        {
            Position = new Point(posX, posY);
        }

        public virtual void SetParentMode(IGameMode mode)
        {
            GameMode = mode;
        }

        public void SwitchModes(IGameMode mode)
        {
            GameMode.Game.SwitchModes(mode);
        }
    }


}
