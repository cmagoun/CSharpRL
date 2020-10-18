using CsEcs;
using CsEcs.SimpleEdits;
using NumberCruncher.Systems;
using System;

namespace NumberCruncher.Components
{
    public class ItemComponent : Component<NoEdit>
    {
        public override Type MyType => typeof(ItemComponent);
        public Powerup Item { get; private set; }

        public ItemComponent(Powerup item)
        {
            Item = item;
        }

        public override IComponent Copy()
        {
            return new ItemComponent(Item);
        }
    }
}
