using CsEcs;
using Microsoft.Xna.Framework;
using ReferenceGame.Components;
using System;
using System.Linq;


namespace ReferenceGame.Modes.Entity
{
    public static class EntityAnimateSystem
    {
        public static void Update(GameTime time, Ecs ecs)
        {
            var toAnimate = ecs.GetComponents<EntityAnimateComponent, EntityWrapperComponent>();
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
                        ecs.RemoveComponent(anim.EntityId, "EntityAnimateComponent");
                    }
                }
            }
        }
    }
}
