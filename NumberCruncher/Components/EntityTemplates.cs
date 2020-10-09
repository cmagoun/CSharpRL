using CsEcs;
using Microsoft.Xna.Framework;
using NumberCruncher.Behaviors;
using SadSharp.Game;
using SadSharp.Helpers;

namespace NumberCruncher.Components
{
    public static class Entities
    {
        public static EntityBuilder Player(int x, int y, GameConsole console, Ecs ecs)
        {
            return ecs.New(Program.Player)
                .Add(new StrengthComponent(1))
                .Add(new StrengthSlotsComponent())
                .Add(new SadWrapperComponent(console, x, y, Glyphs.Digit(1), Color.CornflowerBlue, Color.White))
                .Add(new ActionPointsComponent(1.1)) //this is to ensure the player goes first
                .Add(new BehaviorComponent(new PlayerBehavior()))
                .Add(new BumpTriggerComponent(new AttackTrigger()))
                .Add(new HitPointComponent(20))
                .Add(new ScoreComponent());
        }

        public static EntityBuilder Enemy(int x, int y, int strength, GameConsole console, Ecs ecs)
        {
            return ecs.New()
                .Add(new EnemyComponent())
                .Add(new SadWrapperComponent(console, x, y, Glyphs.Digit(strength), Color.Green.Bright(), Color.Black))
                .Add(new StrengthComponent(strength))
                .Add(new BumpTriggerComponent(new AttackTrigger()))
                .Add(new ActionPointsComponent(1.0))
                .Add(new BehaviorComponent(new EnemyBehavior()))
                .Add(new AwarenessComponent());
        }
    }
}
