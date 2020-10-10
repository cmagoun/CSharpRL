using NumberCruncher.Screens.MainMap;
using SadSharp.Game;
using System;
using Button = SadSharp.Game.Button;
namespace NumberCruncher.Modes.Menu
{
    public class MenuConsole : GameConsole
    {
        public override string MyKey => "MENU";

        public MenuConsole() : base(Program.GameWidth, Program.GameHeight, 0, 0)
        {
            PrintTitle();
            CreateButtons();
        }

        private void CreateButtons()
        {
            var bParams = new ButtonParams { Height = 3, Width = 20 };

            var startButton = new Button("START GAME", 28, 14, bParams)
                .WithClickEvent((s, a) => SwitchModes(new MainLoopMode()));

            Children.Add(startButton);

        }

        private void PrintTitle()
        {
            Print(30, 10, "Number  Cruncher");
        }

        public override void Update(TimeSpan timeElapsed)
        {


        }
    }
}
