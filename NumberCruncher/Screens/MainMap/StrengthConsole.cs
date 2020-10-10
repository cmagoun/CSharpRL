using CsEcs;
using Microsoft.Xna.Framework;
using NumberCruncher.Components;
using NumberCruncher.Systems;
using SadConsole.Input;
using SadSharp.Game;
using System;

namespace NumberCruncher.Screens.MainMap
{
    public class StrengthConsole : GameConsole
    {
        public override string MyKey => "STRENGTH_CONSOLE";
        public StrengthConsole(Ecs ecs) : base(60, 3, 0, 0)
        {
            for (var strength = 1; strength < 10; strength++)
            {
                var button = new StrengthButton(strength, ecs);
                button.OnClick += Button_OnClick;

                var x = (strength - 1) * (button.Width + 1) + 3;

                button.Position = new Point(x, 0);
                Children.Add(button);
            }
        }

        private void Button_OnClick(int strengthValue)
        {
            var mode = (MainLoopMode)GameMode;
            StrengthSystem.ChangeStrength(Program.Player, strengthValue, mode.Ecs);
        }
    }

    public class StrengthButton : Button
    {
        int _strength;
        private Ecs _ecs;

        public delegate void StrengthClicked(int strengthValue);
        public new event StrengthClicked OnClick;

        public StrengthButton(int strength, Ecs ecs)
            : base(strength.ToString(), new ButtonParams { Height = 3, Width = 5, BColor = Color.LightGreen, FColor = Color.Black })
        {
            _strength = strength;
            _ecs = ecs;
        }

        protected override void ButtonClicked(object sender, MouseEventArgs e)
        {
            OnClick?.Invoke(_strength);
        }

        public override void Draw(TimeSpan timeElapsed)
        {
            BColor = _ecs.Get<StrengthSlotsComponent>(Program.Player).IsReady(_strength)
                ? Color.LightGreen
                : Color.OrangeRed;

            base.Draw(timeElapsed);
        }


    }
}
