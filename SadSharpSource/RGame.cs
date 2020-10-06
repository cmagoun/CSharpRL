
using Microsoft.Xna.Framework;
using SadConsole.Input;
using SadSharp.Helpers;

namespace SadSharp.Game
{
    public class RGame
    {
        private readonly ConsoleManager _consoleManager;
        public IGameMode CurrentGameMode { get; private set; }

        public RGame(ConsoleManager cmgr)
        {
            _consoleManager = cmgr;
            _consoleManager.SetGame(this);
            Roller.Create();
        }

        public void Update(Keyboard kb, Mouse mouse, GameTime time)
        {
            CurrentGameMode?.Update(kb, mouse, time);
        }

        public void SwitchModes(IGameMode mode)
        {
            //So... there is the question of initializing the game modes -- do we do it each time? Only the first time?
            //Also, is there data shared between game modes? Where is this data stored?
            mode.Game = this;
            CurrentGameMode = mode;
            mode.Initialize(); 
        }

        public void SetConsoles(params GameConsole[] consoles)
        {
            _consoleManager.SetConsoles(consoles);
        }

        public void OverlayConsole(string key, GameConsole console)
        {
            _consoleManager.Overlay(key, console);
        }

        public void PushConsole(GameConsole console)
        {
            _consoleManager.Push(console);
        }

        public GameConsole PopConsole()
        {
            return _consoleManager.Pop();
        }
    }
}
