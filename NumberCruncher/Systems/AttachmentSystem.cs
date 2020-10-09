using CsEcs;
using NumberCruncher.Components;
using System.Linq;

namespace NumberCruncher.Systems
{
    public static class AttachmentSystem
    {
        public static void MoveAttachedEntities(Ecs ecs)
        {
            var toMove = ecs.GetComponents<AttachedToComponent, SadWrapperComponent>();
            foreach(var tup in toMove)
            {
                var attach = tup.Item1;
                var parent = ecs.Get<SadWrapperComponent>(attach.ParentEntity);

                var sad = tup.Item2;
                sad.ChangePositionPendingAnimation(parent.X + (int)attach.DeltaX, parent.Y + (int)attach.DeltaY);
                sad.AnimatePosition(parent.DrawX + attach.DeltaX, parent.DrawY + attach.DeltaY);
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
