using CsEcs;
using CsEcs.SimpleEdits;
using NumberCruncher.Components;
using System;

namespace NumberCruncher.Systems
{
    public static class AttackSystem
    {
        public static MoveResult Attack(string attackerId, string defenderId, Ecs ecs)
        { 
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
                ecs.AddComponent(enemyId, new DeadComponent());      
            }
            else
            {
                var damage = enemy.Strength - player.Strength;
                var hp = ecs.Get<HitPointComponent>(Program.Player);

                var currentHits = hp.CurrentHits - damage;
                if(currentHits < 1)
                {
                    //YOU ARE DEAD

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
    }
}
