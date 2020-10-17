using CsEcs;
using NumberCruncher.Components;
using NumberCruncher.Systems;
using System;
using System.Collections.Generic;
using System.Text;

namespace NumberCruncher.Behaviors
{
    public class PickupTrigger : ITrigger
    {
        public MoveResult Activate(string moverId, string triggerId, object data, MoveResult currentResult = null)
        {
            return moverId != Program.Player 
                ? MoveResult.Done() 
                : ItemSystem.Pickup(moverId, triggerId, (Ecs)data, currentResult);
        }
    }
}
