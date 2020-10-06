using CsEcs;
using CsEcs.SimpleEdits;
using System;

namespace NumberCruncher.Components
{
    public class ActionPointsComponent : Component<DoubleEdit>
    {
        public override Type MyType => typeof(ActionPointsComponent);
        public double ActionPoints { get; private set; }

        public ActionPointsComponent(double startingPoints)
        {
            ActionPoints = startingPoints;
        }

        public override void DoEdit(DoubleEdit values)
        {
            base.DoEdit(values);
            ActionPoints = values?.NewValue ?? ActionPoints;
        }

        public override IComponent Copy()
        {
            return new ActionPointsComponent(ActionPoints);
        }
    }
}
