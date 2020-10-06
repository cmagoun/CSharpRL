using CsEcs;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using ReferenceGame.Components;
using ReferenceGame.Systems;
using RogueSharp;
using SadSharp.Game;
using SadSharp.MapCreators;
using SadSharp.Modes.Menu;

using Keyboard = SadConsole.Input.Keyboard;
using Mouse = SadConsole.Input.Mouse;

namespace SadSharp.Modes.Map
{
    public enum InternalState {
        FindNext,
        PlayerTurn,
        WaitingForMove,
        WaitingForTargeter,
        PowerActivating,
        Menu
    }

    public class MapMode:IGameMode
    {
        public int CurrentTime { get; protected set; }
        public string CurrentActor { get; private set; }
        //public string CurrentMap { get; set; }
        public bool WaitingOnPlayer { get; set; }

        //public Dictionary<string, Map<RogueCell>> Maps { get; }
        //public Dictionary<string, Ecs> EntityLists { get; }
        //public Ecs Ecs => Maps.ContainsKey(CurrentMap) ? EntityLists[CurrentMap] : null;

        //public Map<RogueCell> Map => Maps.ContainsKey(CurrentMap) ? Maps[CurrentMap] : null;

        public Map<RogueCell> Map;
        public Ecs Ecs;

        private InternalState _state;
        //private IPower _powerWaitingTargets;
        //private object _powerTargets;

        public RGame Game { get; set; }
        public Mouse MouseState { get; private set; }
        public Keyboard KeyboardState { get; set; }

        public MapMode()
        {
        }

        //If I could pass in a map(s), maybe I would not need to virtualize this?
        public virtual void Initialize()
        {   
            CurrentTime = 1;

            var mp = new MapParameters
            {
                Width = MapConsole.MyWidth,
                Height = MapConsole.MyHeight,
                MaxRooms = 40,
                MaxRoomSize = 20,
                MinRoomSize = 4
            };

            var mapInfo = BasicMapCreator.CreateArena("WALK", mp);
            Map = mapInfo.Map;
            Ecs = mapInfo.Ecs;

            Ecs.New("PLAYER")
                .Add(new PositionComponent(5, 5))
                .Add(new GlyphComponent(Glyphs.HappyFace, Color.White))
                .Add(new TurnTakerComponent(new PlayerControlTurnTaker(), 1));

            var map = new MapConsole().WithBorder(Color.Red);
            Game.SetConsoles(map);
        }

        public virtual void Update(Keyboard kb, Mouse mouse, GameTime time)
        {
            MouseState = mouse;
            KeyboardState = kb;

            if(kb.IsKeyReleased(Keys.Escape)) Game.SwitchModes(new MenuMode());
            //if(kb.IsKeyReleased(Keys.OemTilde)) Ticker.Report();

            switch (_state)
            {
                case InternalState.FindNext:
                    FindNextActor();
                    break;

                case InternalState.PlayerTurn:
                    ResolveTurn();
                    break;

                //case InternalState.WaitingForTargeter:
                    //break;

                //case InternalState.PowerActivating:
                //    PowerActivating();
                //    break;
            }
        }


        public void ChangeState(InternalState newState)
        {
            _state = newState;
        }

        public void FindNextActor()
        {
            //I find this loop clunky, but it was thrown together at 1:15AM
            //I will rewrite this later to make more sense
            TurnTakerComponent current;
            do
            {
                current = SchedulingSystem.NextTurn(this);
                CurrentActor = current.EntityId;
                CurrentTime = current.NextTurn;

                if (current.TurnTaker.Who == WhoControls.Player) continue;

                var mresult = MoveResult.Continue;
                while (mresult?.Status != MoveStatus.Done)
                {
                    mresult = ResolveTurn();
                }
            } while (current.TurnTaker.Who != WhoControls.Player);

            ChangeState(InternalState.PlayerTurn);
        }

        public MoveResult ResolveTurn()
        {
            var tt = Ecs.Get<TurnTakerComponent>(CurrentActor);
            var mresult = tt.TurnTaker.TakeTurn(CurrentActor, this);

            if (mresult.Status != MoveStatus.Done) return mresult;

            tt.DoEdit(new TurnTakerEdit(CurrentTime + mresult.Cost));
            ChangeState(InternalState.FindNext);

            return mresult;
        }

        //public void RequestPowerActivation(string actorId, IPower power)
        //{
        //    _powerWaitingTargets = power;
        //    _powerTargets = actorId;

        //    if (power.TargetUi == null)
        //    {
        //        PowerActivating();
        //        return;
        //    }

        //    ChangeState(InternalState.WaitingForTargeter);
        //    var targetConsole = power.TargetUi(this);

        //    Game.OverlayConsole(MapConsole.Key, targetConsole);
        //}

        //public void CancelTargeter()
        //{
        //    _powerWaitingTargets = null;
        //    _powerTargets = null;
        //    RemoveTargeter();
        //    ChangeState(InternalState.PlayerTurn);
        //}

        //public void TargetsChosen(object targets)
        //{
        //    _powerTargets = targets;
        //    RemoveTargeter();
        //    ChangeState(InternalState.PowerActivating);
        //}

        //private void RemoveTargeter()
        //{
        //    var targeter = Game.PopConsole();
        //    targeter = null;
        //}

        //public void PowerActivating()
        //{
        //    var mresult = _powerWaitingTargets.EffectOnActivate(_powerTargets, this);
        //    var tt = Ecs.Get<TurnTakerComponent>(CurrentActor);

        //    _powerTargets = null;
        //    _powerWaitingTargets = null;

        //    if (mresult.Status != MoveStatus.Done) return;

        //    tt.DoEdit(new TurnTakerEdit(CurrentTime + mresult.Cost));
        //    ChangeState(InternalState.FindNext);
        //}
    }
}
