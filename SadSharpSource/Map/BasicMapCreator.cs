using CsEcs;
using RogueSharp;
using RogueSharp.MapCreation;
using System;

namespace SadSharp.MapCreators
{
    public enum DoorCheck
    {
        Horizontal,
        Vertical,
        None
    };

    //This is bad because I am creating the ECS in here... not good
    //We are going to inject the ecs... which will break our Reference game?
    public static class BasicMapCreator
    {
        public static MapInfo CreateArena(string name, MapParameters param, Ecs ecs)
        {
            var mapName = string.IsNullOrEmpty(name) ? Guid.NewGuid().ToString() : name;

            var strategy = new BorderOnlyMapCreationStrategy<Map<RogueCell>, RogueCell>(param.Width, param.Height);
            var map = RogueSharp.Map.Create(strategy);

            var mapInfo = new MapInfo(mapName, map, ecs, param);

            return mapInfo;
        }

    }

    public class MapParameters
    {
        public int Height { get; set; }
        public int Width { get; set; }
        public int MaxRooms { get; set; }
        public int MaxRoomSize { get; set; }
        public int MinRoomSize { get; set; }
    }

    public class MapInfo
    {        
        //contains the map that R# produces, in addition to the various entity objects that we will add
        public Map<RogueCell> Map { get; private set; }
        public Ecs Ecs { get; private set; }

        public MapInfo(string name, Map<RogueCell> map, Ecs ecs, MapParameters p)
        {
            Map = map;
            Ecs = ecs;
            //name and p? Do I need these anymore
        }
        
    }
}
