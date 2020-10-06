using Microsoft.Xna.Framework;
using NumberCruncher.Components;
using ReferenceGame.Modes.Entity;

namespace NumberCruncher.Animation
{
    public static class Animations
    {
        public static AnimateComponent Slide(string entityId, Point from, Point to, float speed)
        {
            return new AnimateComponent(
                new SlideAnimaion(entityId, from, to, speed));
        }
    }
}
