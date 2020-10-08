using CsEcs;
using NumberCruncher.Animation;
using NumberCruncher.Components;
using NumberCruncher.Modes.MainMap;
using NumberCruncher.Systems;
using SadSharp.Helpers;
using System;

namespace NumberCruncher.Behaviors
{
    public class EnemyBehavior : IBehavior
    {
        private IBehavior _subBehavior;

        public EnemyBehavior()
        {
            _subBehavior = new RandomWalkBehavior();
        }

        public MoveResult TakeAction(string entityId, IGameData data)
        {
            var aware = AwarenessSystem.CheckForAwareness(entityId, data);

            if(aware == AwarenessResult.MadeAware)
            {
                //game.Ecs.AddComponent(entityId, Animations.MadeAware());
                _subBehavior = new StalkerBehavior();
            } else if(aware == AwarenessResult.MadeUnaware)
            {
                //game.Ecs.AddComponent(entityId, Animations.MadeAware());
                _subBehavior = new RandomWalkBehavior();
            }

            return _subBehavior.TakeAction(entityId, data);
            
        }

 
    }
}
