using CsEcs;
using NumberCruncher.Animation;
using NumberCruncher.Behaviors;

namespace NumberCruncher.Systems
{
    public static class CleanUpSystem
    {
        public static void RemoveDeletedEntities(Ecs ecs)
        {
            var entities = ecs.EntitiesWith("DeleteComponent");
            foreach(var entityId in entities)
            {
                AttachmentSystem.RemoveAttachedEntities(entityId, ecs);
                ecs.DestroyEntity(entityId);
            }
        }
    }
}
