using CsEcs;
using CsEcs.SimpleEdits;
using Microsoft.Xna.Framework;
using NumberCruncher.Behaviors;
using NumberCruncher.Components;
using NumberCruncher.Systems;
using ReferenceGame.Modes.Entity;
using RogueSharp;
using SadConsole;
using SadSharp.Game;
using SadSharp.Helpers;
using SadSharp.MapCreators;
using System;
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

    public class MainLoopMode : IGameMode, IGameData
    {
        public RGame Game { get; set; }
        public int Turn { get; private set; }
        public int Score { get; private set; }
        public int Level { get; private set; }
        public int RefreshScore { get; private set; }
        public Map<RogueCell> Terrain { get; private set; }
        public Ecs Ecs { get; private set; }
        public GameConsole MapConsole;
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
            Ecs.AddIndex(Program.SadWrapper);
            
            //This order is clunky because I need a reference to the console
            //upon which to place the entities
            MapConsole = new MainMapConsole();
 
            CreateArenaMap(Level);

            var strConsole = new StrengthConsole(Ecs).Under(MapConsole, 1);
            Game.SetConsoles(MapConsole, strConsole);
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
                sad.ChangeGlyph(Glyphs.Digit(newStrength)));
            }
        }

        //probably will move all these out
        public void CreateArenaMap(int level)
        {
            CreateArena();
            //CreateObstacles();
            CreatePlayer();
            CreateEnemies(level);
        }

        private void CreateArena()
        {
            var param = new MapParameters { Width = Program.MapWidth, Height = Program.MapHeight };
            var mapInfo = BasicMapCreator.CreateArena("MAP", param, Ecs);
            Terrain = mapInfo.Map;
        }

        private void CreateEnemies(int level)
        {
            var numEnemies = 1;
            //var numEnemies = Roller.Next($"3d4+{Math.Min(level-1, 20)}");
            for(var index = 0; index < numEnemies; index ++)
            {
                CreateEnemy();
            }
        }

        private void CreateEnemy()
        {
            var player = Ecs.Get<SadWrapperComponent>(Program.Player);
            var possible = Terrain.GetAllCells().Where(c => c.IsWalkable);

            var startPoint = new Point(player.X, player.Y);

            while (startPoint.MDistance(player.ToXnaPoint()) < 4
                || Ecs.EntitiesInIndex(Program.SadWrapper, startPoint.ToKey()).Any())
            {
                var startSpace = possible.PickRandom();
                startPoint = new Point(startSpace.X, startSpace.Y);
            }

            var strength = Roller.Next("1d9");
            Entities.Enemy(startPoint.X, startPoint.Y, strength, MapConsole, Ecs);
        }

        private void CreatePlayer()
        {
            var startSpace = Terrain.GetAllCells().Where(c => c.IsWalkable).PickRandom();
            Entities.Player(startSpace.X, startSpace.Y, MapConsole, Ecs);
        }

        public void Update(Keyboard kb, Mouse mouse, GameTime time)
        {
            
            KeyboardState = kb;
            MouseState = mouse;

            switch (_state)
            {
                case InternalState.FindNext:
                    kb.Clear();
                    FindNextActor();
                    break;
                case InternalState.PlayerTurn:
                    if(Ecs.Not<AnimateComponent>(Program.Player)) ResolveTurn();
                    break;
                case InternalState.EndOfTurn:
                    kb.Clear();
                    EndOfTurn();
                    break;
            }

            AnimateSystem.StartPendingAnimations(Ecs, MapConsole);
            AnimateSystem.Update(time, Ecs);
            CleanUpSystem.RemoveDeletedEntities(Ecs);

        }

        public void FindNextActor()
        {
            //I find this loop clunky, but it was thrown together at 1:15AM
            //I will rewrite this later to make more sense
            do
            {
                CurrentActor = SchedulingSystem.FindNext(Ecs);

                //no one has points, so end of turn
                if (CurrentActor == null)
                {
                    ChangeState(InternalState.EndOfTurn);
                    return;
                }

                //player, so change state to wait for input
                if (CurrentActor == Program.Player) continue;

                //Do all enemy turns
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
            StrengthSystem.UnreadySlots(Ecs);

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

    public interface IGameData
    {
        public int Turn { get;  }
        public int Score { get; }
        public int Level { get;  }
        public int RefreshScore { get;  }
        public Ecs Ecs { get; }
        public Map<RogueCell> Terrain { get; }
        public string CurrentActor { get; }
        public Keyboard KeyboardState { get; }
        public Mouse MouseState { get; }
    }
}
