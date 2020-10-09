using CsEcs;
using CsEcs.SimpleEdits;
using System;

namespace NumberCruncher.Components
{
    public class AttachedToComponent : Component<NoEdit>
    {
        public override Type MyType => typeof(AttachedToComponent);

        public string ParentEntity { get; }

        public AttachedToComponent(string parentEntity)
        {
            ParentEntity = parentEntity;
        }

        public override IComponent Copy()
        {
            return new AttachedToComponent(ParentEntity);
        }
    }
}
