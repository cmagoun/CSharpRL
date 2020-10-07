using CsEcs;
using CsEcs.SimpleEdits;
using System;


namespace NumberCruncher.Components
{
    public class EnemyComponent : Component<NoEdit>
    {
        public override Type MyType => typeof(EnemyComponent);

        public override IComponent Copy()
        {
            return new EnemyComponent();
        }
    }
}
