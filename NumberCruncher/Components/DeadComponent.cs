using CsEcs;
using CsEcs.SimpleEdits;
using EasyComponents;
using System;

namespace NumberCruncher.Components
{
    public class DeadComponent : Component<NoEdit>, IMergeable
    {
        public override Type MyType => typeof(DeadComponent);

        public override IComponent Copy()
        {
            return new DeadComponent();
        }

        public IComponent Merge(IComponent newComponent)
        {
            return this;
        }
    }
}
