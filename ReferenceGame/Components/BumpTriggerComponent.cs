using CsEcs;
using CsEcs.SimpleEdits;
using ReferenceGame.Systems;
using SadSharp.Modes.Map;
using System;

namespace ReferenceGame.Components
{
    public class BumpTriggerComponent : Component<NoEdit>
    {
        public override Type MyType => typeof(BumpTriggerComponent);

        //Func<actorId, triggerId, gamestate, result>
        public Func<string, string, MapMode, MoveResult> Interaction { get; }
        public int Order { get; }

        public BumpTriggerComponent(Func<string, string, MapMode, MoveResult> action, int order = 0)
        {
            Interaction = action;
            Order = order;
        }

        public override IComponent Copy()
        {
            return new BumpTriggerComponent(Interaction, Order);
        }
    }

}
