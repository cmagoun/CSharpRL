using CsEcs;
using CsEcs.SimpleEdits;
using System;

namespace NumberCruncher.Components
{
    public class MadeUnawareComponent : Component<NoEdit>
    {
        public override Type MyType => typeof(MadeUnawareComponent);

        public override IComponent Copy()
        {
            return new MadeUnawareComponent();
        }
    }
}
