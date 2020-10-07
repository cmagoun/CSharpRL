using CsEcs;
using CsEcs.SimpleEdits;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using NumberCruncher.Behaviors;
using NumberCruncher.Components;
using NumberCruncher.Systems;
using ReferenceGame.Components;
using ReferenceGame.Modes.Entity;
using RogueSharp;
using SadConsole.Input;
using SadSharp.Game;
using SadSharp.Helpers;
using SadSharp.MapCreators;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Keyboard = SadConsole.Input.Keyboard;
using Mouse = SadConsole.Input.Mouse;
using Point = Microsoft.Xna.Framework.Point;

namespace NumberCruncher.Modes.MainMap
{
    public enum InternalState
    {
        FindNext,
        PlayerTurn,
        EndOfTurn,
        WaitingForMove,
        WaitingForTargeter,
        PowerActivating,
        Menu
    }

    public class MainLoopMode : IGameMode
    {
        public RGame Game { get; set; }
        public int Turn { get; private set; }
        public int Score { get; private set; }
        public int Level { get; private set; }
        public int RefreshScore { get; private set; }
        public Map<RogueCell> Terrain;
        public Ecs Ecs;
        public Mouse MouseState { get; set; }
        public Keyboard KeyboardState { get; set; }
        private InternalState _state;
        public string CurrentActor { get; set; }

        public void Initialize()
        {
            Roller.Create();

            Turn = 1;
            Score = 0;
            RefreshScore = 0;
            Level = 1;
            CurrentActor = "";

            Ecs = new Ecs("NumberCruncher");
            Ecs.AddIndex("SadWrapperComponent");
            
            //This order is clunky because I need a reference to the console
            //upon which to place the entities
            var console = new MainMapConsole();
 
            CreateArenaMap(Level, console);

            var strConsole = new StrengthConsole(Ecs).Under(console, 1);
            Game.SetConsoles(console, strConsole);
        }

        public void ChangeState(InternalState newState)
        {
            _state = newState;
        }

        public void ChangeStrength(int newStrength)
        {
            var comp = Ecs.Get<StrengthComponent>(Program.Player);
            var slots = Ecs.Get<StrengthSlotsComponent>(Program.Player);
            
            if(slots.IsReady(newStrength))
            {
                comp.DoEdit(new IntEdit(newStrength));
                var sad = Ecs.Get<SadWrapperComponent>(Program.Player);
                sad.DoEdit(SadWrapperEdit.ChangeGlyph(Glyphs.Digit(newStrength)));
            }
        }

        public void CreateArenaMap(int level, GameConsole console)
        {
            CreateArena();
            //CreateObstacles();
            CreatePlayer(console);
            CreateEnemies(level, console);
        }

        private void CreateArena()
        {
            var mapInfo = BasicMapCreator.CreateArena("MAP", new MapParameters { Width = 60, Height = 30 }, Ecs);
            Terrain = mapInfo.Map;
        }

        private void CreateEnemies(int level, GameConsole console)
        {
            var numEnemies = Roller.Next($"2d4+{Math.Min(level-1, 20)}");
            for(var index = 0; index < numEnemies; index ++)
            {
                CreateEnemy(console);
            }
        }

        private void CreateEnemy(GameConsole console)
        {
            var player = Ecs.Get<SadWrapperComponent>("PLAYER");
            var possible = Terrain.GetAllCells().Where(c => c.IsWalkable);

            var startPoint = new Point(player.X, player.Y);

            while (startPoint.MDistance(player.ToXnaPoint()) < 4
                || Ecs.EntitiesInIndex("SadWrapperComponent", startPoint.ToKey()).Any())
            {
                var startSpace = possible.PickRandom();
                startPoint = new Point(startSpace.X, startSpace.Y);          
            }

            var strength = Roller.Next("2d5-1");

            Ecs.New()
                .Add(new SadWrapperComponent(console, startPoint.X, startPoint.Y, Glyphs.Digit(strength), Color.Red, Color.Black))
                .Add(new StrengthComponent(strength))
                .Add(new ActionPointsComponent(1.0))
                .Add(new BehaviorComponent(new RandomWalkBehavior()));
        }

        private void CreatePlayer(GameConsole console)
        {
            var startSpace = Terrain.GetAllCells().Where(c => c.IsWalkable).PickRandom();
            Ecs.New(Program.Player)
                .Add(new StrengthComponent(1))
                .Add(new StrengthSlotsComponent())
                .Add(new SadWrapperComponent(console, startSpace.X, startSpace.Y, Glyphs.Digit(1), Color.CornflowerBlue, Color.White))
                .Add(new ActionPointsComponent(1.1)) //this is to ensure the player goes first
                .Add(new BehaviorComponent(new PlayerBehavior()));

        }

        public void Update(Keyboard kb, Mouse mouse, GameTime time)
        {
            KeyboardState = kb;
            MouseState = mouse;

            Debug.WriteLine(string.Join(',', KeyboardState.KeysPressed.Select(k => k.ToString())));

            switch (_state)
            {
                case InternalState.FindNext:
                    FindNextActor();
                    break;
                case InternalState.PlayerTurn:
                    ResolveTurn();
                    break;
                case InternalState.EndOfTurn:
                    EndOfTurn();
                    break;

            }

            AnimateSystem.Update(time, Ecs);
        }

        public void FindNextActor()
        {
            //I find this loop clunky, but it was thrown together at 1:15AM
            //I will rewrite this later to make more sense
            do
            {
                CurrentActor = SchedulingSystem.FindNext(Ecs);
                if (CurrentActor == null)
                {
                    ChangeState(InternalState.EndOfTurn);
                    return;
                }

                if (CurrentActor == Program.Player) continue;

                var mresult = MoveResult.Continue;
                while (mresult?.Status != MoveStatus.Done)
                {
                    mresult = ResolveTurn();
                }
            } while (CurrentActor != Program.Player);

            ChangeState(InternalState.PlayerTurn);
        }

        public MoveResult ResolveTurn()
        {
            var behavior = Ecs.Get<BehaviorComponent>(CurrentActor);
            if (behavior == null) return MoveResult.Done();

            var mresult = behavior.TakeAction(CurrentActor, this);

            if (mresult.Status != MoveStatus.Done) return mresult;

            var ap = Ecs.Get<ActionPointsComponent>(CurrentActor);
            ap.DoEdit(new DoubleEdit(ap.ActionPoints - mresult.Cost));

            ChangeState(InternalState.FindNext);

            return mresult;
        }

        public void EndOfTurn()
        {
            Turn++;
            var ap = Ecs.GetComponents<ActionPointsComponent>();
            foreach(var comp in ap)
            {
                comp.DoEdit(new DoubleEdit(comp.ActionPoints + 1.0));
            }

            //other stuff???
            ChangeState(InternalState.FindNext);
        }
    }
}
