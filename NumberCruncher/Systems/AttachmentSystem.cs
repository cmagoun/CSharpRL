using CsEcs;
using NumberCruncher.Components;
using System.Linq;

namespace NumberCruncher.Systems
{
    public static class AttachmentSystem
    {
        public static void MoveAttachedEntities(string entityId, double oldX, double oldY, double newX, double newY, Ecs ecs)
        {
            var toMove = ecs
                .GetComponents<AttachedToComponent, SadWrapperComponent>()
                .Where(tup => tup.Item1.ParentEntity == entityId)
                .Select(tup => tup.Item2)
                .ToList();

            if (!toMove.Any()) return;
            
            var dx = newX - oldX;
            var dy = newY - oldY;

            foreach(var sad in toMove)
            {
                sad.AnimatePosition(sad.DrawX + dx, sad.DrawY + dy);
            }
        }

        internal static void RemoveAttachedEntities(string entityId, Ecs ecs)
        {
            var toRemove = ecs
                .GetComponents<AttachedToComponent>()
                .Where(c => c.ParentEntity == entityId)
                .ToList();

            if (!toRemove.Any()) return;

            foreach(var comp in toRemove)
            {
                ecs.AddComponent(comp.EntityId, new DeleteComponent());
            }

        }
    }
}
