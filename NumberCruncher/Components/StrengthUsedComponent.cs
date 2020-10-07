using CsEcs;
using CsEcs.SimpleEdits;
using System;

namespace NumberCruncher.Components
{
    public class StrengthUsedComponent : Component<NoEdit>
    {
        public override Type MyType => typeof(StrengthUsedComponent);

        public override IComponent Copy()
        {
            return new StrengthUsedComponent();
        }
    }
}
