using CsEcs;
using NumberCruncher.Components;
using NumberCruncher.Screens.MainMap;
using System.Collections.Generic;

namespace NumberCruncher.Systems
{
    public static class ItemSystem
    {
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


}
