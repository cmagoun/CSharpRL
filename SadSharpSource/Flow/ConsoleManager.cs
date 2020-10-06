using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using SadConsole;

namespace SadSharp.Game
{
    public class ConsoleManager
    {
        private readonly MainConsole _main;
        private RGame _game;
        private readonly Dictionary<string, GameConsole> _consoles;

        public ConsoleManager(int maxWidth, int maxHeight)
        {
            _consoles = new Dictionary<string, GameConsole>();
            _main = new MainConsole(maxWidth, maxHeight);
            Global.CurrentScreen = _main;
        }

        public void SetGame(RGame game)
        {
            _game = game;
        }

        public void SetConsoles(params GameConsole[] consoles)
        {
            _main.Children.Clear();
            _consoles.Clear();
            
            foreach (var console in consoles)
            {
                console.SetParentMode(_game.CurrentGameMode);
                _main.Children.Add(console);
                _consoles.Add(console.MyKey, console);
            }
        }

        public void Overlay(string parentKey, GameConsole console)
        {
            var parent = _consoles[parentKey];
            console.Position = parent.IsBordered
                ? new Point(parent.X + 1, parent.Y + 1)
                : new Point(parent.X, parent.Y);

            _main.Children.Add(console);
            _consoles.Add(console.MyKey, console);
        }

        public void Push(GameConsole console)
        {
            _main.Children.Add(console);
            _consoles.Add(console.MyKey, console);
        }

        public GameConsole Pop()
        {
            var console = (GameConsole)_main.Children.Last();
            _main.Children.Remove(console);
            _consoles.Remove(console.MyKey);
            return console;
        }
    }
}
