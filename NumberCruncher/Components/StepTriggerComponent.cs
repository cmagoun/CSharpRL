using CsEcs;
using CsEcs.SimpleEdits;
using System;

namespace NumberCruncher.Components
{
    public class StepTriggerComponent : Component<NoEdit>
    {
        public override Type MyType => typeof(StepTriggerComponent);
        public ITrigger Interaction { get; }
        public int Order { get; }

        public StepTriggerComponent(ITrigger action, int order = 0)
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
