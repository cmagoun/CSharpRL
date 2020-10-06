using Microsoft.Xna.Framework;
using SadConsole;
using SadSharp.Game;
using SadSharp.Modes.Menu;

namespace SadSharp
{
    class Program
    {
        public static RGame Game;

        private static string Font_Cheep = "Fonts/Cheepicus12.font";
        private static string Font_C64 = "Fonts/C64.font";

        static void Main(string[] args)
        {
            SadConsole.Game.Create(Font_C64, Constants.GAME_WIDTH, Constants.GAME_HEIGHT);
            SadConsole.Game.OnInitialize = Init;
            SadConsole.Game.OnUpdate = Update;


            // Start the game.
            SadConsole.Game.Instance.Run();

            // Code here will not run until the game window closes.

            SadConsole.Game.Instance.Dispose();
        }

        private static void Init()
        {
            //Ticker.Init();
            Game = new RGame(new ConsoleManager(Constants.GAME_WIDTH, Constants.GAME_HEIGHT));
            Game.SwitchModes(new MenuMode());
        }

        private static void Update(GameTime time)
        {
            Game.Update(Global.KeyboardState, Global.MouseState, time);
        }
    }
}
