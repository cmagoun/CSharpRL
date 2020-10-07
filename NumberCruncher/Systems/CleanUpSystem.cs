using CsEcs;

namespace NumberCruncher.Systems
{
    public static class CleanUpSystem
    {
        public static void CleanUpDeadEnemies(Ecs ecs)
        {
            var entities = ecs.EntitiesWith("DeadComponent");
            foreach(var entityId in entities)
            {
                ecs.DestroyEntity(entityId);
            }
        }
    }
}
