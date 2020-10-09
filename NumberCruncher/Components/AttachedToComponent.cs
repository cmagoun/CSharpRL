using CsEcs;
using CsEcs.SimpleEdits;
using System;

namespace NumberCruncher.Components
{
    public class AttachedToComponent : Component<NoEdit>
    {
        public override Type MyType => typeof(AttachedToComponent);

        public string ParentEntity { get; }
        public double DeltaX { get; }
        public double DeltaY { get; }

        public AttachedToComponent(string parentEntity, double dx, double dy)
        {
            ParentEntity = parentEntity;
            DeltaX = dx;
            DeltaY = dy;
        }

        public override IComponent Copy()
        {
            return new AttachedToComponent(ParentEntity, DeltaX, DeltaY);
        }
    }
}
