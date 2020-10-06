using Microsoft.Xna.Framework;
using SadSharp.Game.Controls;
using System;

namespace SadSharp.Game
{
    public class Label : RControl
    {
        public string Text;
        public Color FColor;
        public Color BColor;

        public Label(string text, int x, int y, ButtonParams param = null)
            : base(param?.Width ?? text.Length + 2, param?.Height ?? 1)
        {
            if (param == null) param = new ButtonParams(null, Color.Transparent);

            Text = text;
            Position = new Point(x, y);
            FColor = param.FColor;
            BColor = param.BColor;
            if (param.Font != null) Font = param.Font;
        }

        public Label(string text, ButtonParams param = null) : this(text, 0, 0, param)
        {
        }

        //There is a lot of overlap here with Button. We will refactor later.
        public override void Draw(TimeSpan timeElapsed)
        {
            foreach (var cell in Cells)
            {
                cell.Background = BColor;
            }

            Print((Width - Text.Length) / 2, (Height - 1) / 2, Text, FColor);

            base.Draw(timeElapsed);
        }

    }
}
