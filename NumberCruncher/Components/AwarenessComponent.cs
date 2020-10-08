using CsEcs;
using CsEcs.SimpleEdits;
using System;

namespace NumberCruncher.Components
{
    public class AwarenessComponent : Component<BoolEdit>
    {
        public override Type MyType => typeof(AwarenessComponent);
        public bool Aware { get; private set; }

        public AwarenessComponent(bool aware = false)
        {
            Aware = aware;
        }

        public override void DoEdit(BoolEdit values)
        {
            base.DoEdit(values);
            Aware = values?.NewValue ?? Aware;
        }

        public override IComponent Copy()
        {
           return new AwarenessComponent(Aware);
        }
    }
}
