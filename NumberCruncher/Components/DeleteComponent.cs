using CsEcs;
using CsEcs.SimpleEdits;
using System;

namespace NumberCruncher.Components
{
    public class DeleteComponent : Component<NoEdit>
    {
        public override Type MyType => typeof(DeleteComponent);

        public override IComponent Copy()
        {
            return new DeleteComponent();
        }
    }
}
