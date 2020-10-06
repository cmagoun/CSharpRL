using CsEcs;
using RogueSharp;
using RogueSharp.MapCreation;

namespace SadSharp.MapCreators
{
    public static class TestMapCreator
    {

        public static MapInfo CreateFilledMap(string name, int width, int height)
        {
            var map = new Map<RogueCell>(width, height);
            return new MapInfo(name, map, new Ecs(name), new MapParameters() );
        }
    }
}
