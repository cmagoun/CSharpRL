using CsEcs;
using CsEcs.SimpleEdits;
using NumberCruncher.Systems;
using SadSharp.Game;
using System;

namespace NumberCruncher.Components
{
    public class BumpTriggerComponent : Component<NoEdit>
    {
        public override Type MyType => typeof(BumpTriggerComponent);

        //Func<actorId, triggerId, gamestate, result>
        public ITrigger Interaction { get; }
        public int Order { get; }

        public BumpTriggerComponent(ITrigger action, int order = 0)
        {
            Interaction = action;
            Order = order;
        }

        public override IComponent Copy()
        {
            return new BumpTriggerComponent(Interaction, Order);
        }
    }

    public interface ITrigger
    {
        MoveResult Activate(string moverId, string triggerId, object data);
    }

}
