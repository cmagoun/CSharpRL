using CsEcs;
using Microsoft.Xna.Framework;
using ReferenceGame.Components;
using RogueSharp;
using SadConsole;
using SadConsole.Input;
using SadSharp;
using SadSharp.Game;
using SadSharp.Helpers;
using SadSharp.MapCreators;
using SadSharp.Modes.Map;
using System.Linq;

namespace ReferenceGame.Modes.Entity
{
    public class EntityMode:MapMode
    {
        public override void Initialize()
        {
            CurrentTime = 1;

            var mp = new MapParameters
            {
                Width = MapConsole.MyWidth,
                Height = MapConsole.MyHeight,
                MaxRooms = 40,
                MaxRoomSize = 20,
                MinRoomSize = 4
            };

            var mapInfo = BasicMapCreator.CreateArena("WALK", mp);
            Map = mapInfo.Map;
            Ecs = mapInfo.Ecs;


            var console = new EntityConsole().WithBorder(Color.Blue);

            Ecs.New("PLAYER")
                .Add(new EntityWrapperComponent(console, 5, 5, Glyphs.HappyFace))
                .Add(new TurnTakerComponent(new EntityTurnTaker(), 1));

            DropWalkers(20, Ecs, Map, console);

            Game.SetConsoles(console);
        }

        public override void Update(Keyboard kb, Mouse mouse, GameTime time)
        {
            base.Update(kb, mouse, time);

            EntityAnimateSystem.Update(time, this.Ecs);
        }

        private void DropWalkers(int num, Ecs ecs, Map<RogueCell> map, GameConsole console)
        {
            var walkable = map.GetAllCells().Where(c => c.IsWalkable).ToList();

            for (int x = 0; x < num; x++)
            {
                var cell = walkable.PickRandom();

                ecs.New()
                    .Add(new EntityWrapperComponent(console, cell.X, cell.Y, Glyphs.W, Color.Red))
                    .Add(new TurnTakerComponent(new RandomWalkTurnTaker(), 2));
            }
        }
    }
}
