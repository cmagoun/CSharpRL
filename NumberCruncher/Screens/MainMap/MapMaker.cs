using CsEcs;
using Microsoft.Xna.Framework;
using NumberCruncher.Components;
using RogueSharp;
using SadSharp.Game;
using SadSharp.Helpers;
using SadSharp.MapCreators;
using System;
using System.Linq;
using Point = Microsoft.Xna.Framework.Point;

namespace NumberCruncher.Screens.MainMap
{
    public static class MapMaker
    {
        public static Map<RogueCell> CreateArenaMap(int level, Ecs ecs, GameConsole mapConsole)
        {
            var terrain = CreateArena(ecs);
            //CreateObstacles();
            if(level == 1) CreatePlayer(ecs, mapConsole, terrain);
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

        private static void CreatePlayer(Ecs ecs, GameConsole mapConsole, Map<RogueCell> terrain)
        {
            var startSpace = terrain.GetAllCells().Where(c => c.IsWalkable).PickRandom();
            Entities.Player(startSpace.X, startSpace.Y, mapConsole, ecs);
        }

    }
}
