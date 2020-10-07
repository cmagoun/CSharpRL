using CsEcs;
using NumberCruncher.Animation;

namespace NumberCruncher.Systems
{
    public static class CleanUpSystem
    {
        public static void AnimateDeadEnemies(Ecs ecs)
        {
            var entities = ecs.EntitiesWith("DeadComponent");
            foreach(var entityId in entities)
            {
                ecs.AddComponent(entityId, Animations.Death());
            }
        }

        public static void CleanUpDeadEnemies(Ecs ecs)
        {
            var entities = ecs.EntitiesWith("DeleteComponent");
            foreach(var entityId in entities)
            {
                ecs.DestroyEntity(entityId);
            }
        }
    }
}
