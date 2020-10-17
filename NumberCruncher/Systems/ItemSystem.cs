using CsEcs;
using NumberCruncher.Components;
using System.Collections.Generic;

namespace NumberCruncher.Systems
{
    public static class ItemSystem
    {
        public static Dictionary<string, InventoryItem> AllItems = new Dictionary<string, InventoryItem>
        {
            { "B", new InventoryItem("B", "Blink", "B) Blink") },
            { "L", new InventoryItem("L", "Laser", "L) Laser") },
            { "M", new InventoryItem("M", "Bomb", "M) Bomb") },
            { "S", new InventoryItem("S", "Shadow", "S) Shadow") }
        };

        public static MoveResult Pickup(string moverId, string itemId, Ecs ecs, MoveResult currentMoveResult = null)
        {
            var inventory = ecs.Get<InventoryComponent>(moverId);
            var itemComp = ecs.Get<ItemComponent>(itemId);

            if (itemComp == null || inventory == null) return MoveResult.Done();

            if(inventory.Items.ContainsKey(itemComp.Item.Key))
            {
                //you have one, just heal 2 points
                var hpComp = ecs.Get<HitPointComponent>(moverId);
                hpComp.UpdateHits(2);
            }
            else 
            {
                //you don't have one, pick it up
                inventory.Items.Add(itemComp.Item.Key, itemComp.Item);
            }

            ecs.DestroyEntity(itemId);
            return MoveResult.Done();
        }
    }

    public class InventoryItem
    {
        public string Name { get; }
        public string Display { get; }
        public string Key { get; } //what to press to use

        public InventoryItem(string key, string name, string display)
        {
            Key = key;
            Name = name;
            Display = display;
        }

        //when pressed, we fire teh keyed power?
    }
}
