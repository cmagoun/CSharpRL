using CsEcs;
using Microsoft.Xna.Framework;
using NumberCruncher;
using NumberCruncher.Animation;
using NumberCruncher.Components;
using SadSharp.Game;
using SadSharp.Helpers;
using System;
using System.Linq;


namespace ReferenceGame.Modes.Entity
{
    public static class AnimateSystem
    {
        public static void StartPendingAnimations(Ecs ecs, GameConsole console)
        {
            AnimateDeadEnemies(ecs);
            AnimateMakeAware(ecs, console);
            AnimateMakeUnaware(ecs, console);
        }

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

        public static void AnimateMakeAware(Ecs ecs, GameConsole console)
        {
            var entities = ecs.EntitiesWith("MadeAwareComponent");
            foreach(var entityId in entities)
            {
                var entity = ecs.Get<SadWrapperComponent>(entityId);
                entity.ChangeColor(Color.Red);

                ecs.AddComponent(entity.EntityId, Animations.Hop());

                var exclaim = ecs.New()
                    .Add(new SadWrapperComponent(console, entity.X, entity.Y - 1, Glyphs.Exclaim, Color.Red.Bright(), Color.Transparent))
                    .Add(Animations.FadeToBlack())
                    .Add(new AttachedToComponent(entityId, 0, -1));

                ecs.RemoveComponent(entityId, "MadeAwareComponent");
            }
        }

        public static void AnimateMakeUnaware(Ecs ecs, GameConsole console)
        {
            var entities = ecs.EntitiesWith("MadeAwareComponent");
            foreach (var entityId in entities)
            {
                var entity = ecs.Get<SadWrapperComponent>(entityId);

                var question = ecs.New()
                    .Add(new SadWrapperComponent(console, entity.X, entity.Y - 1, Glyphs.Question, Color.Green, Color.Transparent))
                    .Add(Animations.FadeToBlack());

                ecs.RemoveComponent(entityId, "MadeAwareComponent");
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
