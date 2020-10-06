using ReferenceGame.Modes.Entity;
using SadSharp.Game;
using SadSharp.Modes.Map;
using System;
using Button = SadSharp.Game.Button;
using C = SadSharp.Constants;

namespace SadSharp.Modes.Menu
{
    public class MenuConsole : GameConsole
    {
        public override string MyKey => "MENU";

        public MenuConsole() : base(C.GAME_WIDTH, C.GAME_HEIGHT, 0, 0)
        {
            PrintTitle();
            CreateButtons();
        }

        private void CreateButtons()
        {
            var bParams = new ButtonParams { Height = 3, Width = 20 };

            var walkButton = new Button("WALK TEST", 48, 24, bParams)
                .WithClickEvent((s, a) => SwitchModes(new MapMode()));

            var testButton = new Button("ANIMATIONS", bParams)
                .WithClickEvent((s, a) => SwitchModes(new EntityMode()))
                .Under(walkButton, 2);

            Children.Add(walkButton);
            Children.Add(testButton);
        }

        private void PrintTitle()
        {
            //Font = Global.FontDefault.Master.GetFont(Font.FontSizes.Two);
            Print(20, 10, "Hello World!");
            Print(20, 12, "This is an example of how to put all the pieces together to make a game.");
            Print(20, 14, "I need to work a little more to get stuff into a proper library, but for now...");
            Print(20, 16, "Click a button to continue.");
        }

        public override void Update(TimeSpan timeElapsed)
        {
            

        }
    }
}
