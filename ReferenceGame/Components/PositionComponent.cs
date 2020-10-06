using System;
using CsEcs;
using CsEcs.SimpleEdits;
using Microsoft.Xna.Framework;

namespace ReferenceGame.Components
{
    public class PositionComponent : Component<PosEdit>, IIndexable
    {
        public override Type MyType => typeof(PositionComponent);

        public int X { get; private set; }
        public int Y { get; private set; }


        public PositionComponent(int x, int y)
        {
            X = x;
            Y = y;
        }

        public override void DoEdit(PosEdit values)
        {
            base.DoEdit(values);
            X = values.X ?? X;
            Y = values.Y ?? Y;
        }

        public override IComponent Copy()
        {
            return new PositionComponent(X, Y);
        }

        public Microsoft.Xna.Framework.Point ToXnaPoint(int dx = 0, int dy = 0)
        {
            return new Microsoft.Xna.Framework.Point(X + dx, Y + dy);
        }

        public RogueSharp.Point ToRogueSharpPoint(int dx = 0, int dy = 0)
        {
            return new RogueSharp.Point(X = dx, Y + dy);
        }

        public Vector2 ToVector2(float dx = 0, float dy = 0)
        {
            return new Vector2(X + dx, Y + dy);
        }

        public string IndexKey => $"{X}/{Y}";
    }
}
