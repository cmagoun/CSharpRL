using CsEcs;
using Microsoft.Xna.Framework;
using NumberCruncher.Components;
using ReferenceGame.Modes.Entity;
using SharpDX.MediaFoundation.DirectX;
using System.Collections.Generic;

namespace NumberCruncher.Animation
{
    public static class Animations
    {
        public static AnimateComponent Slide(string entityId, Point from, Point to, float speed)
        {
            return new AnimateComponent(
                new SlideAnimaion(entityId, from, to, speed));
        }

        public static AnimateComponent Death()
        {
            var animations = new List<IAnimation>
            {
                new MakeBackgroundTransparentAnimtation(1000),
                new FramesAnimation(new[] { Glyphs.Circle, Glyphs.Donut, Glyphs.Asterisk, Glyphs.DotCenter }, 250),
                new JiggleAnimation(1000, .1)
            };

            return new AnimateComponent(
                new CompoundAnimation(
                    animations,
                    new List<int> { 1 },
                    new List<int> { 0, 1 },
                    new List<int> { 0, 1 }),
                MarkEntityForDeletion);
        }

        public static AnimateComponent FadeToBlack()
        {
            return new AnimateComponent(
                new FadeAnimation(60),
                MarkEntityForDeletion);
        }

        public static void MarkEntityForDeletion(AnimateComponent anim)
        {
            var ecs = anim.MyEcs;
            ecs.AddComponent(anim.EntityId, new DeleteComponent());
        }
    }
}
