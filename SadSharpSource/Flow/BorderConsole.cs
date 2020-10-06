using System;
using Microsoft.Xna.Framework;
using SadConsole;

namespace SadSharp.Game
{
    class BorderConsole:GameConsole
    {
        private readonly Color _borderColor;
        private readonly GameConsole _child;
        public override string MyKey => _child.MyKey;
        public override bool IsBordered => true;

        public BorderConsole(GameConsole console, Color? borderColor = null) : base(console.Width+2, console.Height+2, console.Position.X, console.Position.Y)
        {
            _borderColor = borderColor ?? Color.Red;
            _child = console;

            console.Position  = new Point(1, 1);
            Children.Add(console);
        }

        public override void Draw(TimeSpan timeElapsed)
        {
            DrawBorder();
            base.Draw(timeElapsed);
        }


        private void DrawBorder()
        {
            DrawBox(new Rectangle(0, 0, Width, Height), new Cell(_borderColor), null, ConnectedLineThin);
        }

        public override void SetParentMode(IGameMode game)
        {
            _child.SetParentMode(game);
            base.SetParentMode(game);
        }
    }
}
