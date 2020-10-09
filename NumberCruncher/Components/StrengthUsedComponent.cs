using CsEcs;
using CsEcs.SimpleEdits;
using EasyComponents;
using System;

namespace NumberCruncher.Components
{
    public class StrengthUsedComponent : Component<NoEdit>, IMergable
    {
        public override Type MyType => typeof(StrengthUsedComponent);

        public override IComponent Copy()
        {
            return new StrengthUsedComponent();
        }

        public IComponent Merge(IComponent newComponent)
        {
            return this;
        }
    }
}
