using Microsoft.Xna.Framework;
using NumberCruncher.Components;
using ReferenceGame.Modes.Entity;
using SharpDX.MediaFoundation.DirectX;

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
            return new AnimateComponent(
                new FramesAnimation(
                    new[] { Glyphs.Circle, Glyphs.Donut, Glyphs.Asterisk, Glyphs.DotCenter }, 200),
                    MarkEntityForDeletion);
        }

        public static void MarkEntityForDeletion(AnimateComponent anim)
        {
            var ecs = anim.MyEcs;
            ecs.AddComponent(anim.EntityId, new DeleteComponent());
        }
    }
}
