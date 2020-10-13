using NumberCruncher.Components;
using NumberCruncher.Screens.MainMap;
using NumberCruncher.Systems;

namespace NumberCruncher.Behaviors
{
    public class EnemyBehavior : IBehavior
    {
        private IBehavior _subBehavior;
        private int _stumbleChance;

        public EnemyBehavior(int stumbleChance = 8)
        {
            _subBehavior = new RandomWalkBehavior();
            _stumbleChance = stumbleChance;
        }

        public MoveResult TakeAction(string entityId, IGameData data)
        {
            var aware = AwarenessSystem.CheckForAwareness(entityId, data);

            if(aware == AwarenessResult.MadeAware)
            {
                data.Ecs.AddComponent(entityId, new MadeAwareComponent());
                _subBehavior = new StalkerBehavior();
            } 
            else if(aware == AwarenessResult.MadeUnaware)
            {
                data.Ecs.AddComponent(entityId, new MadeUnawareComponent());
                _subBehavior = new RandomWalkBehavior();
            }


            return _subBehavior.TakeAction(entityId, data);
            
        }

 
    }
}
