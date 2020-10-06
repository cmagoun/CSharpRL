using CsEcs;
using CsEcs.SimpleEdits;
using Microsoft.Xna.Framework;
using ReferenceGame.Components;
using System;
using System.Collections.Generic;
using System.Text;

namespace ReferenceGame.Modes.Entity
{
    public class EntityAnimateComponent : Component<NoEdit>
    {
        public override Type MyType => typeof(EntityAnimateComponent);
        public List<IAnimation> Animations { get; private set; }

        public EntityAnimateComponent(List<IAnimation> animations)
        {
            Animations = animations;
        }

        public EntityAnimateComponent(IAnimation animation)
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
        void OnStart(GameTime time, EntityWrapperComponent comp);
        void OnEnd(EntityWrapperComponent comp);
        void Update(GameTime time, EntityWrapperComponent comp);
        bool IsComplete(EntityWrapperComponent comp);

    }
}
