using CsEcs;
using NumberCruncher.Components;
using System;
using System.Diagnostics;

namespace NumberCruncher.Systems
{
    public static class AttackSystem
    {
        public static MoveResult Attack(string attackerId, string defenderId, Ecs ecs)
        {
            Debug.WriteLine($"{attackerId} attacks {defenderId}");

            return attackerId == Program.Player
                ? PlayerBattle(defenderId, ecs)
                : defenderId == Program.Player
                    ? PlayerBattle(attackerId, ecs)
                    : throw new NotImplementedException("Two computer entities cannot fight yet");
        }

        private static MoveResult PlayerBattle(string enemyId, Ecs ecs)
        {
            var player = ecs.Get<StrengthComponent>(Program.Player);
            var enemy = ecs.Get<StrengthComponent>(enemyId);

            if(player.Strength >= enemy.Strength)
            {
                //If player >= enemy ==> player takes no damage, enemy is destroyed
                Kill(enemyId, ecs);     
            }
            else
            {
                var damage = enemy.Strength - player.Strength;
                var hp = ecs.Get<HitPointComponent>(Program.Player);

                var currentHits = hp.CurrentHits - damage;
                if(currentHits < 1)
                {
                    //YOU ARE DEAD
                    throw new Exception("YOU ARE DEAD");

                } else
                {
                    //If player < enemy ==> player takes difference, enemy is reduced by player str
                    hp.DoEdit(new HitPointEdit(currentHits));
                    StrengthSystem.ChangeStrength(enemyId, enemy.Strength - player.Strength, ecs);
                }

                
            }

            ecs.AddComponent(Program.Player, new StrengthUsedComponent());
            
            return MoveResult.Done(MoveSystem.BaseCost);
        }

        private static void Kill(string enemyId, Ecs ecs)
        {
            ecs.AddComponent(enemyId, new DeadComponent());
            ecs.RemoveComponent(enemyId, "BehaviorComponent");
            ecs.RemoveComponent(enemyId, "BumpTriggerComponent");
            ecs.RemoveComponent(enemyId, "ActionPointsComponent");

            var strength = ecs.Get<StrengthComponent>(enemyId);
            var score = ecs.Get<ScoreComponent>(Program.Player);

            score.UpdateScore(strength.OriginalStrength);
        }
    }
}
