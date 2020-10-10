

using Microsoft.Xna.Framework;
using RogueSharp;
using SadSharp.Game;
using SadSharp.Helpers;
using SadSharp.MapCreators;
using SharpDX.WIC;
using System;

namespace NumberCruncher.Screens.MainMap
{
    public class MainMapConsole : GameConsole
    {
        public override string MyKey => "MAIN_MAP";

        public MainMapConsole() : base(Program.MapWidth, Program.MapHeight, 0, 0)
        {
        }

        public override void Draw(TimeSpan timeElapsed)
        {
            var mode = (MainLoopMode)GameMode;
            DrawMap(mode.Terrain);

            base.Draw(timeElapsed);
        }

        private void DrawMap(Map<RogueCell> terrain)
        {
            foreach (var cell in terrain.GetAllCells())
            {
                if (!cell.IsWalkable)
                {
                    SetGlyph(cell.X, cell.Y, Glyphs.Filled, Color.White);
                }
                else
                {
                    SetGlyph(cell.X, cell.Y, Glyphs.DotCenter, Color.DarkGray.Dim());
                }
            }
        }


    }
}
