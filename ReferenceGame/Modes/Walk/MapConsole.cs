using CsEcs;
using Microsoft.Xna.Framework;
using ReferenceGame.Components;
using SadSharp.Game;
using SadSharp.MapCreators;
using System;
using System.Linq;
using C = SadSharp.Constants;

namespace SadSharp.Modes.Map
{
    public class MapConsole : GameConsole
    {
        public static int MyHeight = 38;
        public static int MyWidth = 78;
        public static int PosX = 0;
        public static int PosY = 0;
        public static string Key = "MAP";
        public override string MyKey => Key;

        public MapConsole() : base(MyWidth, MyHeight, PosX, PosY)
        {
        }

        public override void Draw(TimeSpan timeElapsed)
        {
            //I am taking a naive approach to drawing the map.
            //While I am using SadConsole, I would like directish control
            //over rendering. Thus, I am restricting myself to using
            //SetGlyph to explicitly draw the map each cycle.
            var mapMode = (MapMode) GameMode;

            Clear();
            DrawMap(mapMode.Map);
            DrawEntities(mapMode.Ecs);

            base.Draw(timeElapsed);
        }

        private void DrawEntities(Ecs ecs)
        {
            var toDraw = ecs
                .GetComponents<PositionComponent, GlyphComponent>()
                .OrderBy(x => x.Item2.ZIndex)
                .ToList();

            foreach ((var pos, var glyph) in toDraw)
            {
                SetGlyph(pos.X, pos.Y, glyph.Index, glyph.FColor, glyph.BColor);
            }
        }

        private void DrawMap(RogueSharp.Map<RogueCell>map)
        {
            foreach(var cell in map.GetAllCells())
            {
                if(!cell.IsWalkable)
                {
                    SetGlyph(cell.X, cell.Y, Glyphs.Filled, Color.White);
                }
            }
        }
    }
}
