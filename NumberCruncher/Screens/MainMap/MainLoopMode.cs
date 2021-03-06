﻿using CsEcs;
using CsEcs.SimpleEdits;
using Microsoft.Xna.Framework;
using NumberCruncher.Components;
using NumberCruncher.Systems;
using ReferenceGame.Modes.Entity;
using RogueSharp;
using SadSharp.Game;
using SadSharp.Helpers;
using SadSharp.MapCreators;
using System.Linq;
using Keyboard = SadConsole.Input.Keyboard;
using Mouse = SadConsole.Input.Mouse;

namespace NumberCruncher.Screens.MainMap
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
        public FieldOfView<RogueCell> CurrentFov { get; private set; }

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

            //includes side-effects of altering the ECS and putting Entities
            //on the MapConsole :(
            Terrain = MapMaker.CreateArenaMap(Level, Ecs, MapConsole);

            var strConsole = new StrengthConsole(Ecs).Under(MapConsole, 1);
            var scoreConsole = new ScoreConsole(this).RightOf(MapConsole, 0);

            //this is a little wonky because we are adding border after calculating position
            //Should really be 0 under, but has to be 2 because of the borders
            var inventoryConsole = new InventoryConsole(this).Under(scoreConsole, 2);

            Game.SetConsoles(
                MapConsole, 
                strConsole, 
                scoreConsole.WithBorder(Color.Green), 
                inventoryConsole.WithBorder(Color.Green));
        }

        public void ChangeState(InternalState newState)
        {
            _state = newState;
        }

        public void ChangeStrength(int newStrength)
        {
            var comp = Ecs.Get<StrengthComponent>(Program.Player);
            var slots = Ecs.Get<StrengthSlotsComponent>(Program.Player);

            if (slots.IsReady(newStrength))
            {
                comp.DoEdit(new IntEdit(newStrength));
                var sad = Ecs.Get<SadWrapperComponent>(Program.Player);
                sad.ChangeGlyph(Glyphs.Digit(newStrength));
            }
        }

        //probably will move all these out
    
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
                    if (Ecs.Not<AnimateComponent>(Program.Player))
                    {
                        var playerMoveResult = ResolveTurn();
                        if (playerMoveResult.Status == MoveStatus.Done) CurrentFov = FovSystem.UpdatePlayerFov(Ecs, Terrain);
                    }
                    break;
                case InternalState.EndOfTurn:
                    kb.Clear();
                    EndOfTurn();
                    break;
            }

            AnimateSystem.StartPendingAnimations(Ecs, MapConsole);
            AnimateSystem.Update(time, Ecs);
            AttachmentSystem.MoveAttachedEntities(Ecs);
            CleanUpSystem.RemoveDeletedEntities(Ecs);

            //This might be map specific later, for now
            //every map is "kill all"
            CheckForCompletedLevel();
        }

        public void CheckForCompletedLevel()
        {
            var enemies = Ecs.GetComponents<EnemyComponent>();
            if(!enemies.Any())
            {
                Level++;
                Turn++;
                Terrain = MapMaker.CreateArenaMap(Level, Ecs, MapConsole);

                var playerAp = Ecs.Get<ActionPointsComponent>(Program.Player);
                playerAp.DoEdit(new DoubleEdit(1.1));
            }
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
            ap?.DoEdit(new DoubleEdit(ap.ActionPoints - mresult.Cost));

            ChangeState(InternalState.FindNext);

            return mresult;
        }

        public void EndOfTurn()
        {
            StrengthSystem.UnreadySlots(Ecs);

            Turn++;
            var ap = Ecs.GetComponents<ActionPointsComponent>();
            foreach (var comp in ap)
            {
                comp.DoEdit(new DoubleEdit(comp.ActionPoints + 1.0));
            }

            //other stuff???
            ChangeState(InternalState.FindNext);
        }
    }

    public interface IGameData
    {
        public int Turn { get; }
        public int Score { get; }
        public int Level { get; }
        public int RefreshScore { get; }
        public Ecs Ecs { get; }
        public Map<RogueCell> Terrain { get; }
        public string CurrentActor { get; }
        public Keyboard KeyboardState { get; }
        public Mouse MouseState { get; }

        public FieldOfView<RogueCell> CurrentFov { get; }
    }
}
