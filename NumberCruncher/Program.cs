using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NumberCruncher.Modes.Menu;
using SadConsole;
using SadSharp.Game;

namespace NumberCruncher
{
    public static class Program
    {
        public const int Width = 80;
        public const int Height = 40;
        public const string Player = "PLAYER";
        public static RGame Game;
        private static string Font_C64 = "Fonts/C64.font";

        static void Main(string[] args)
        {
            // Setup the engine and create the main window.
            SadConsole.Game.Create(Font_C64, Width, Height);

            // Hook the start event so we can add consoles to the system.
            SadConsole.Game.OnInitialize = Init;

            SadConsole.Game.OnUpdate = Update;

            // Start the game.
            SadConsole.Game.Instance.Run();
            SadConsole.Game.Instance.Dispose();
        }

        private static void Init()
        {
            Game = new RGame(new ConsoleManager(Width, Height));
            Game.SwitchModes(new MenuMode());
        }

        private static void Update(GameTime time)
        {
            Game.Update(Global.KeyboardState, Global.MouseState, time);
        }
    }
}
