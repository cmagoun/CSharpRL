using CsEcs;
using CsEcs.SimpleEdits;
using System;
using System.Collections.Generic;
using System.Text;

namespace NumberCruncher.Components
{
    class MadeAwareComponent : Component<NoEdit>
    {
        public override Type MyType => typeof(MadeAwareComponent);

        public override IComponent Copy()
        {
            return new MadeAwareComponent();
        }
    }
}
