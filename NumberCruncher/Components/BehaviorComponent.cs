using CsEcs;
using CsEcs.SimpleEdits;
using NumberCruncher.Screens.MainMap;
using NumberCruncher.Systems;
using SadSharp.Helpers;
using System;

namespace NumberCruncher.Components
{
    public class BehaviorComponent : Component<NoEdit>
    {
        private IBehavior _behavior;
        public override Type MyType => typeof(BehaviorComponent);

        public BehaviorComponent(IBehavior behavior)
        {
            _behavior = behavior;
        }

        public MoveResult TakeAction(string entityId, MainLoopMode game)
        {
            return _behavior.TakeAction(entityId, game);
        }


        public override IComponent Copy()
        {
            return this.DeepClone();
        }
    }

    public interface IBehavior
    {
        MoveResult TakeAction(string entityId, IGameData data);
    }
}
