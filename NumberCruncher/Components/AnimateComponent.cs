using CsEcs;
using CsEcs.SimpleEdits;
using EasyComponents;
using Microsoft.Xna.Framework;
using SadSharp.Helpers;
using System;
using System.Collections.Generic;

namespace NumberCruncher.Components
{
    public class AnimateComponent : Component<NoEdit>, IMergable
    {
        public override Type MyType => typeof(AnimateComponent);
        public List<IAnimation> Animations { get; private set; }
        public Action<AnimateComponent> Callback { get; private set; }

        public AnimateComponent(List<IAnimation> animations)
        {
            Animations = animations;
        }

        public AnimateComponent(IAnimation animation, Action<AnimateComponent> callback = null)
        {
            Animations = new List<IAnimation> { animation };
            Callback = callback;
        }

        public override IComponent Copy()
        {
            return this.DeepClone();
        }

        public IComponent Merge(IComponent newComponent)
        {
            var newAnimateComponent = newComponent as AnimateComponent;
            foreach(var animation in newAnimateComponent.Animations)
            {
                Animations.Add(animation);
            }

            return this;
        }
    }

    public interface IAnimation
    {
        bool IsRunning { get; set; }
        void OnStart(GameTime time, SadWrapperComponent comp);
        void OnEnd(SadWrapperComponent comp);
        void Update(GameTime time, SadWrapperComponent comp);
        bool IsComplete(SadWrapperComponent comp);

    }
}
