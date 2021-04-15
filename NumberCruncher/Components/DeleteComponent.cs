using CsEcs;
using CsEcs.SimpleEdits;
using EasyComponents;
using System;

namespace NumberCruncher.Components
{
    public class DeleteComponent : Component<NoEdit>, IMergeable
    {
        public override Type MyType => typeof(DeleteComponent);

        public override IComponent Copy()
        {
            return new DeleteComponent();
        }

        public IComponent Merge(IComponent newComponent)
        {
            return this;
        }
    }
}
