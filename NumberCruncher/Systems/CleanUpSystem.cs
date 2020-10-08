using CsEcs;
using NumberCruncher.Animation;

namespace NumberCruncher.Systems
{
    public static class CleanUpSystem
    {


        public static void RemoveDeletedEntities(Ecs ecs)
        {
            var entities = ecs.EntitiesWith("DeleteComponent");
            foreach(var entityId in entities)
            {
                ecs.DestroyEntity(entityId);
            }
        }
    }
}
