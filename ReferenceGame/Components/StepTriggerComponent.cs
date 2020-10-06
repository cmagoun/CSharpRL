using System;
using CsEcs;
using CsEcs.SimpleEdits;
using ReferenceGame.Systems;
using SadSharp.Modes.Map;

namespace ReferenceGame.Components
{
    public class StepTriggerComponent : Component<NoEdit>
    {
        public override Type MyType => typeof(StepTriggerComponent);

        //Func<actorId, triggerId, currentMoveResult, gamestate, result>
        public Func<string, string, MoveResult, MapMode, MoveResult> Interaction { get; }
        public int Order { get; }

        public StepTriggerComponent(Func<string, string, MoveResult, MapMode, MoveResult> action, int order = 0)
        {
            Interaction = action;
            Order = order;
        }

        public override IComponent Copy()
        {
            return new StepTriggerComponent(Interaction, Order);
        }
    }
}
