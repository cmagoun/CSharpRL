using Microsoft.Xna.Framework;
using NumberCruncher.Modes.Menu;
using SadConsole;
using SadSharp.Game;

namespace NumberCruncher
{
    public static class Program
    {
        public const int GameWidth = 80;
        public const int GameHeight = 50;

        public const int MapWidth = 60;
        public const int MapHeight = 35;

        public const string Player = "PLAYER";
        public const string SadWrapper = "SadWrapperComponent";
        public const int FontSize = 16;

        public static RGame Game;
        private static string Font_C64 = "Fonts/C64.font";

        static void Main(string[] args)
        {
            SadConsole.Game.Create(Font_C64, GameWidth, GameHeight);

            SadConsole.Game.OnInitialize = Init;
            SadConsole.Game.OnUpdate = Update;

            SadConsole.Game.Instance.Run();
            SadConsole.Game.Instance.Dispose();
        }

        private static void Init()
        {
            Game = new RGame(new ConsoleManager(GameWidth, GameHeight));
            Game.SwitchModes(new MenuMode());
        }

        private static void Update(GameTime time)
        {
            Game.Update(Global.KeyboardState, Global.MouseState, time);
        }
    }
}
