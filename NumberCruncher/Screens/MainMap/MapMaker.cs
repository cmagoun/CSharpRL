using CsEcs;
using NumberCruncher.Components;
using NumberCruncher.Systems;
using RogueSharp;
using SadSharp.Game;
using SadSharp.Helpers;
using SadSharp.MapCreators;
using System;
using System.Collections.Generic;
using System.Linq;
using Point = Microsoft.Xna.Framework.Point;

namespace NumberCruncher.Screens.MainMap
{
    public static class MapMaker
    {
        public static Map<RogueCell> CreateArenaMap(int level, Ecs ecs, GameConsole mapConsole)
        {
            var terrain = CreateArena(ecs);
            if(level == 1) CreatePlayer(ecs, mapConsole, terrain);

            CreateObstacles(ecs, terrain);
            //TODO: Do we need to check for unreachable space?

            CreateItems(ecs, mapConsole, terrain);

            CreateEnemies(level, ecs, mapConsole, terrain);

            return terrain;
        }

        private static Map<RogueCell> CreateArena(Ecs ecs)
        {
            var param = new MapParameters { Width = Program.MapWidth, Height = Program.MapHeight };
            var mapInfo = BasicMapCreator.CreateArena("MAP", param, ecs);
            return mapInfo.Map;
        }

        private static void CreateEnemies(int level, Ecs ecs, GameConsole mapConsole, Map<RogueCell>terrain)
        {
            var numEnemies = Roller.Next($"3d4+{Math.Min(level - 1, 20)}");
            for (var index = 0; index < numEnemies; index++)
            {
                CreateEnemy(ecs, mapConsole, terrain);
            }
        }

        private static void CreateEnemy(Ecs ecs, GameConsole mapConsole, Map<RogueCell> terrain)
        {
            var player = ecs.Get<SadWrapperComponent>(Program.Player);
            var possible = terrain.GetAllCells().Where(c => c.IsWalkable);

            var startPoint = new Point(player.X, player.Y);

            while (startPoint.MDistance(player.ToXnaPoint()) < 4
                || ecs.EntitiesInIndex(Program.SadWrapper, startPoint.ToKey()).Any())
            {
                var startSpace = possible.PickRandom();
                startPoint = new Point(startSpace.X, startSpace.Y);
            }

            var strength = Roller.Next("1d9");
            Entities.Enemy(startPoint.X, startPoint.Y, strength, mapConsole, ecs);
        }

        private static void CreateItems(Ecs ecs, GameConsole mapConsole, Map<RogueCell>terrain)
        {
            var numItems = Roller.Next("2d4-2");
            for(var index = 0; index < numItems; index++)
            {
                CreateItem(ecs, mapConsole, terrain);
            }
        }

        private static void CreateItem(Ecs ecs, GameConsole mapConsole, Map<RogueCell>terrain)
        {
            var player = ecs.Get<SadWrapperComponent>(Program.Player);
            var possible = terrain.GetAllCells().Where(c => c.IsWalkable);

            var startPoint = new Point(player.X, player.Y);

            while (startPoint.MDistance(player.ToXnaPoint()) < 4
                || ecs.EntitiesInIndex(Program.SadWrapper, startPoint.ToKey()).Any())
            {
                var startSpace = possible.PickRandom();
                startPoint = new Point(startSpace.X, startSpace.Y);
            }

            var item = ItemSystem.AllItems.PickRandom();
            Entities.Item(startPoint.X, startPoint.Y, item.Value, mapConsole, ecs);
            
        }

        private static void CreateObstacles(Ecs ecs, Map<RogueCell>terrain)
        {
            var player = ecs.Get<SadWrapperComponent>(Program.Player);
            var num = Roller.Next("2d10-2");

            for(var index = 0; index < num; index++)
            {
                bool inRec = true;
                var rec = new List<RogueCell>();

                while (inRec)
                {
                    var rw = Roller.Next(3, 10);
                    var rh = Roller.Next(3, 10);

                    var x = Roller.Next(2, Program.MapWidth - (rw + 1));
                    var y = Roller.Next(2, Program.MapHeight - (rh + 1));

                    rec = terrain.GetCellsInRectangle(y, x, rw, rh).ToList();

                    inRec = rec.Any(p => p.X == player.X && p.Y == player.Y);
                }

                rec.ForEach(p => p.SetWall());
            }
        }

        private static void CreatePlayer(Ecs ecs, GameConsole mapConsole, Map<RogueCell> terrain)
        {
            var startSpace = terrain.GetAllCells().Where(c => c.IsWalkable).PickRandom();
            Entities.Player(startSpace.X, startSpace.Y, mapConsole, ecs);
        }

    }
}
