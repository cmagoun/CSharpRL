using CsEcs;
using CsEcs.SimpleEdits;
using System;

namespace NumberCruncher.Components
{
    public class StrengthComponent : Component<IntEdit>
    {
        public override Type MyType => typeof(StrengthComponent);

        public int Strength { get; private set; }
        public int OriginalStrength { get; private set; }

        public StrengthComponent(int startStrength)
        {
            Strength = startStrength;
            OriginalStrength = startStrength;
        }

        public override void DoEdit(IntEdit values)
        {
            base.DoEdit(values);
            Strength = values?.NewValue ?? Strength;
        }

        public override IComponent Copy()
        {
            return new StrengthComponent(Strength);
        }
    }
}
