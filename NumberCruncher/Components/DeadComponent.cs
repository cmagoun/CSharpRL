using CsEcs;
using CsEcs.SimpleEdits;
using System;

namespace NumberCruncher.Components
{
    public class DeadComponent : Component<NoEdit>
    {
        public override Type MyType => typeof(DeadComponent);

        public override IComponent Copy()
        {
            return new DeadComponent();
        }
    }
}
