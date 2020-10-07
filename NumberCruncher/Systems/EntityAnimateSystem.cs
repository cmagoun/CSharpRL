using CsEcs;
using Microsoft.Xna.Framework;
using NumberCruncher.Components;
using System;
using System.Linq;


namespace ReferenceGame.Modes.Entity
{
    public static class AnimateSystem
    {
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
                        ecs.RemoveComponent(anim.EntityId, "AnimateComponent");
                    }
                }
            }
        }
    }
}
