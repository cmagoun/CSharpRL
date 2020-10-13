using NumberCruncher.Components;
using NumberCruncher.Screens.MainMap;
using NumberCruncher.Systems;
using SadSharp.Helpers;

namespace NumberCruncher.Behaviors
{
    public class EnemyBehavior : IBehavior
    {
        private int _stumbleChance;

        public EnemyBehavior(int stumbleChance = 8)
        {
            _stumbleChance = stumbleChance;
        }

        public MoveResult TakeAction(string entityId, IGameData data)
        {
            var aware = AwarenessSystem.CheckForAwareness(entityId, data);
            var stumbleRoll = Roller.NextD100;

            switch(aware)
            {
                case AwarenessResult.Aware:
                    if(stumbleRoll <= _stumbleChance)
                    {
                        return MoveSystem.TakeRandomMove(entityId, data);
                    } 
                    else
                    {
                        return MoveSystem.TakeStalkerMove(entityId, data);
                    }

                case AwarenessResult.MadeAware:
                    data.Ecs.AddComponent(entityId, new MadeAwareComponent());
                    return MoveSystem.TakeStalkerMove(entityId, data);

                case AwarenessResult.MadeUnaware:
                    data.Ecs.AddComponent(entityId, new MadeUnawareComponent());
                    return MoveSystem.TakeRandomMove(entityId, data);

                case AwarenessResult.Unaware:
                    return MoveSystem.TakeRandomMove(entityId, data);

                default:
                    return MoveSystem.TakeRandomMove(entityId, data);
            }
        }

 
    }
}
