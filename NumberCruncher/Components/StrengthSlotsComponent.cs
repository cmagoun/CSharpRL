using CsEcs;
using CsEcs.SimpleEdits;
using System;

namespace NumberCruncher.Components
{
    public class StrengthSlotsComponent : Component<NoEdit>
    {
        public override Type MyType => typeof(StrengthSlotsComponent);
        public bool[] Slots { get; private set; }
            
        public StrengthSlotsComponent()
        {
            Slots = new[] { true, true, true, true, true, true, true, true, true, true };
        }

        public StrengthSlotsComponent(bool[] slots)
        {
            Slots = slots;
        }

        public bool IsReady(int slot)
        {
            return Slots[slot];
        }

        public void MakeReady(int slot)
        {
            Slots[slot] = true;
        }

        public void MakeUsed(int slot)
        {
            Slots[slot] = false;
        }


        public override IComponent Copy()
        {
            return new StrengthSlotsComponent(Slots);
        }
    }
}
