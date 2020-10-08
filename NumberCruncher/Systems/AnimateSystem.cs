using CsEcs;
using Microsoft.Xna.Framework;
using NumberCruncher.Animation;
using NumberCruncher.Components;
using System;
using System.Linq;


namespace ReferenceGame.Modes.Entity
{
    public static class AnimateSystem
    {
        public static void AnimateDeadEnemies(Ecs ecs)
        {
            var entities = ecs.EntitiesWith("DeadComponent");
            foreach (var entityId in entities)
            {
                ecs.AddComponent(entityId, Animations.Death());
                ecs.RemoveComponent(entityId, "BumpTriggerComponent");
                ecs.RemoveComponent(entityId, "DeadComponent");
            }
        }

        public static void Update(GameTime time, Ecs ecs)
        {
            var toAnimate = ecs.GetComponents<AnimateComponent, SadWrapperComponent>();
            foreach((var anim, var comp) in toAnimate)
            {
                var animation = anim.Animations.First();
                if(!animation.IsRunning)
                {
                    animation.OnStart(time, comp);
                    animation.IsRunning = true;
                }

                animation.Update(time, comp);

                if (animation.IsComplete(comp))
                {
                    animation.OnEnd(comp);
                    anim.Animations.RemoveAt(0);

                    if(!anim.Animations.Any())
                    {
                        anim?.Callback?.Invoke(anim);
                        ecs.RemoveComponent(anim.EntityId, "AnimateComponent");
                    }
                }
            }
        }
    }
}
