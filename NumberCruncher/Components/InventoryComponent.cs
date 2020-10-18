using CsEcs;
using CsEcs.SimpleEdits;
using NumberCruncher.Systems;
using System;
using System.Collections.Generic;
using System.Text;

namespace NumberCruncher.Components
{
    public class InventoryComponent : Component<NoEdit>
    {
        public override Type MyType => typeof(InventoryComponent);
        public Dictionary<string, Powerup> Items { get; private set; }

        public InventoryComponent()
        {
            Items = new Dictionary<string, Powerup>();
        }

        public override IComponent Copy()
        {
            throw new NotImplementedException();
        }
    }



}
