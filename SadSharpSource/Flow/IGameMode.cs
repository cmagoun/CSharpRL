using CsEcs;
using Microsoft.Xna.Framework;
using SadConsole.Input;

namespace SadSharp.Game
{
    public interface IGameMode
    {
        RGame Game { get; set; }
        void Initialize();
        void Update(Keyboard kb, Mouse mouse, GameTime time);
    }
}
