using CsEcs;
using Microsoft.Xna.Framework;
using SadSharp.Game;
using Keyboard = SadConsole.Input.Keyboard;
using Mouse = SadConsole.Input.Mouse;

namespace SadSharp.Modes.Menu
{
    public class MenuMode:IGameMode
    {
        public RGame Game { get; set; }

        public void Initialize()
        {
            Game.SetConsoles(new MenuConsole());
        }

        public void Update(Keyboard kb, Mouse mouse, GameTime time)
        {
        }

        public Ecs Ecs => null;
        public object Data => null;
    }
}
