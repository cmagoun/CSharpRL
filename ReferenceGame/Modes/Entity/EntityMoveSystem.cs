using Microsoft.Xna.Framework;
using ReferenceGame.Components;
using ReferenceGame.Systems;
using SadSharp.Modes.Map;
using System;
using System.Collections.Generic;
using System.Text;

namespace ReferenceGame.Modes.Entity
{
    public static class EntityMoveSystem
    {
        public static MoveResult TryMove(string entityId, Point from, Point to, MapMode mm)
        {
            //var onSpace = mm.Ecs.EntitiesInIndex("GamePieceComponent", $"{to.X}/{to.Y}").ToArray();

            var result = CheckForBlockedSpace(to, mm);
      
            if (result != MoveResult.Continue) return result;

            result = DoMove(entityId, from, to, mm);
   
            return result;
        }

        public static MoveResult CheckForBlockedSpace(Point to, MapMode mode)
        {
            var walkable = mode.Map.GetCell(to.X, to.Y).IsWalkable;

            return !walkable
                ? MoveResult.Blocked
                : MoveResult.Continue;
        }
        public static MoveResult DoMove(string entityId, Point from, Point to, MapMode mm)
        {
            mm.Ecs.AddComponent(
                entityId,
                new EntityAnimateComponent(
                    new WalkAnimation(entityId, from, to, .1f)));

            return MoveResult.Done(120);
        }
    }
}
