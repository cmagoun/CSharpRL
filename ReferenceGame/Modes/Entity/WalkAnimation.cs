using Microsoft.Xna.Framework;
using ReferenceGame.Components;
using System;

namespace ReferenceGame.Modes.Entity
{
    public class WalkAnimation : IAnimation
    {
        private SlideAnimaion _slide;
        private HopAnimation _hop;

        public bool IsRunning { get; set; }

        public WalkAnimation(string entityId, Point from, Point to, float speed)
        {
            _slide = new SlideAnimaion(entityId, from, to, speed);
            _hop = new HopAnimation(10, 2, 0.35);
        }

        public bool IsComplete(EntityWrapperComponent comp)
        {
            return _slide.IsComplete(comp);
        }

        public void OnEnd(EntityWrapperComponent comp)
        {
            _slide.OnEnd(comp);
        }

        public void OnStart(GameTime time, EntityWrapperComponent comp)
        {
            _slide.OnStart(time, comp);
        }

        public void Update(GameTime time, EntityWrapperComponent comp)
        {
            _slide.Update(time, comp);
            _hop.Update(time, comp);
        }
    }
}
