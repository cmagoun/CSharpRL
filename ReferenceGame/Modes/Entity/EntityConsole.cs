using CsEcs;
using Microsoft.Xna.Framework;
using ReferenceGame.Components;
using SadSharp;
using SadSharp.Game;
using SadSharp.MapCreators;
using SadSharp.Modes.Map;
using System;

namespace ReferenceGame.Modes.Entity
{
    public class EntityConsole : GameConsole
    {
        public override string MyKey => "ENTITY_CONSOLE";
        public EntityConsole() : base(MapConsole.MyWidth, MapConsole.MyHeight, 0, 0)
        {
        }

        public override void Draw(TimeSpan timeElapsed)
        {
            //I am taking a naive approach to drawing the map.
            //While I am using SadConsole, I would like directish control
            //over rendering. Thus, I am restricting myself to using
            //SetGlyph to explicitly draw the map each cycle.
            var mapMode = (MapMode)GameMode;

            DrawMap(mapMode.Map);
            //DrawEntities(mapMode.Ecs);

            base.Draw(timeElapsed);
        }

        private void DrawMap(RogueSharp.Map<RogueCell> map)
        {
            foreach (var cell in map.GetAllCells())
            {
                if (!cell.IsWalkable)
                {
                    SetGlyph(cell.X, cell.Y, Glyphs.Filled, Color.White);
                }
            }
        }

    }
}
