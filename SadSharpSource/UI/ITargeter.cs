using SadSharp.Game;
using System.Collections.Generic;

namespace SadSharp.Powers
{
    public interface ITargeter
    {
        List<object> SelectedTargets { get; }
    }

    public abstract class TargeterConsole : GameConsole, ITargeter
    {
        protected IGameMode Mode;
        protected object Data;
        public List<object> SelectedTargets { get; }

        protected TargeterConsole(int width, int height, IGameMode mode, object data) : base(width, height, 0, 0)
        {
            Data = data;
            Mode = mode;
            SelectedTargets = new List<object>();
        }
    }
}
