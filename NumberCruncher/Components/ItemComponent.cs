using CsEcs;
using CsEcs.SimpleEdits;
using NumberCruncher.Systems;
using System;

namespace NumberCruncher.Components
{
    public class ItemComponent : Component<NoEdit>
    {
        public override Type MyType => typeof(ItemComponent);
        public InventoryItem Item { get; private set; }

        public ItemComponent(InventoryItem item)
        {
            Item = item;
        }

        public override IComponent Copy()
        {
            return new ItemComponent(Item);
        }
    }
}
