using System;
using Microsoft.Xna.Framework;
using SadConsole;
using SadSharp.Game.Controls;
using SadSharp.Helpers;


namespace SadSharp.Game
{
    //Currently, the button will only handle a single-line of text
    public class Button : RControl
    {
        public string Text;
        public Color FColor;
        public Color BColor;
        public EventHandler OnClick;
        private bool _mouseOver;

        public Button(string text, int x, int y, ButtonParams param = null) 
            : base(param?.Width ?? text.Length + 2, param?.Height ?? 1)
        {
            if(param == null) param = new ButtonParams();

            Text = text;
            Position = new Point(x, y);
            FColor = param.FColor;
            BColor = param.BColor;
            if (param.Font != null) Font = param.Font;

            MouseButtonClicked += ButtonClicked;
            MouseEnter += (sender, args) => _mouseOver = true;
            MouseExit += (sender, args) => _mouseOver = false;
        }

        protected virtual void ButtonClicked(object sender, SadConsole.Input.MouseEventArgs e)
        {
            OnClick?.Invoke(sender, e);
        }

        public Button(string text, ButtonParams param = null):this(text, 0, 0, param){}


        public override void Draw(TimeSpan timeElapsed)
        {
            foreach (var cell in Cells)
            {
                cell.Background = _mouseOver
                    ? BColor.Bright()
                    : BColor;
            }

            Print((Width - Text.Length)/2, (Height - 1)/2, Text, FColor);

            base.Draw(timeElapsed);
        }

    }


    //Primarily used to set properties that you are going to
    //reuse between many similar buttons on a screen
    public class ButtonParams
    {
        public int Height = 1;
        public int? Width = null;
        public Color FColor = Color.White;
        public Color BColor = Color.DarkGray;
        public Font Font = null;

        public ButtonParams() { }

        public ButtonParams(int width, int height, Color? fcolor = null, Color? bcolor = null, Font font = null)
        {
            Width = width;
            Height = height;
            FColor = fcolor ?? FColor;
            BColor = bcolor ?? BColor;
            Font = font;
        }

        public ButtonParams(int height, Color? fcolor = null, Color? bcolor = null, Font font = null)
        {
            Height = height;
            FColor = fcolor ?? FColor;
            BColor = bcolor ?? BColor;
            Font = font;
        }

        public ButtonParams(Color? fcolor = null, Color? bcolor = null, Font font = null)
        {
            FColor = fcolor ?? FColor;
            BColor = bcolor ?? BColor;
            Font = font;
        }
    }
}
