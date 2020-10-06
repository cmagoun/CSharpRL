using CsEcs;
using CsEcs.SimpleEdits;
using Microsoft.Xna.Framework;
using ReferenceGame.Components;
using System;
using System.Collections.Generic;

namespace NumberCruncher.Components
{
    public class AnimateComponent : Component<NoEdit>
    {
        public override Type MyType => typeof(AnimateComponent);
        public List<IAnimation> Animations { get; private set; }

        public AnimateComponent(List<IAnimation> animations)
        {
            Animations = animations;
        }

        public AnimateComponent(IAnimation animation)
        {
            Animations = new List<IAnimation> { animation };
        }

        public override IComponent Copy()
        {
            return null;
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
