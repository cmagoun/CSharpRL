using CsEcs;
using NumberCruncher.Components;
using NumberCruncher.Systems;

namespace NumberCruncher.Behaviors
{
    public class AttackTrigger : ITrigger
    {
        public MoveResult Activate(string moverId, string triggerId, object data)
        {
            var ecs = (Ecs)data;
            var enemyComps = ecs.GetComponents<EnemyComponent>(moverId, triggerId);

            //if enemy comps == 1 then one of the two actors is an enemy and they fight
            //otherwise the two actors are on the same side and just block each other
            return enemyComps.Count == 1
                ? AttackSystem.Attack(moverId, triggerId, ecs)
                : MoveResult.Blocked;
        }
    }
}
