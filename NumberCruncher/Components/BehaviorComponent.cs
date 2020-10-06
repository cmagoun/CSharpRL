using CsEcs;
using CsEcs.SimpleEdits;
using NumberCruncher.Modes.MainMap;
using NumberCruncher.Systems;
using SadSharp.MapCreators;
using System;
using System.Collections.Generic;
using System.Text;

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
            throw new NotImplementedException();
        }
    }

    public interface IBehavior
    {
        MoveResult TakeAction(string entityId, MainLoopMode game);
    }
}
