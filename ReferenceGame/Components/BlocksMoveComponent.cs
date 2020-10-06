using System;
using CsEcs;
using CsEcs.SimpleEdits;

namespace ReferenceGame.Components
{
    public class BlocksMoveComponent : Component<NoEdit>
    {
        public override Type MyType => typeof(BlocksMoveComponent);

        public override IComponent Copy()
        {
            return new BlocksMoveComponent();
        }
    }
}
