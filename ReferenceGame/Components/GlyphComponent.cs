using CsEcs;
using System;
using Color = Microsoft.Xna.Framework.Color;

namespace ReferenceGame.Components
{
    public class GlyphComponent:Component<GlyphEdit>
    {
        public override Type MyType => typeof(GlyphComponent);

        public int Index { get; private set; }
        public Color BColor { get; private set; }
        public Color FColor { get; private set; }
        public int ZIndex { get; private set; }

        public GlyphComponent(int index, Color? fg = null, Color? bg = null, int zindex = 1)
        {
            Index = index;
            FColor = fg ?? Color.White;
            BColor = bg ?? Color.Transparent;
            ZIndex = zindex;
        }

        public override void DoEdit(GlyphEdit values)
        {
            base.DoEdit(values);
            Index = values.Index ?? Index;
            FColor = values.FColor ?? FColor;
            BColor = values.BColor ?? BColor;
            ZIndex = values.ZIndex ?? ZIndex;
        }

        public override IComponent Copy()
        {
            return new GlyphComponent(Index, BColor, FColor, ZIndex);
        }
    }

    public class GlyphEdit
    {
        public int? Index { get; }
        public Color? BColor { get; }
        public Color? FColor { get; }
        public int? ZIndex { get; }

        public GlyphEdit(int? index = null, Color? fg = null, Color? bg = null, int? zIndex = null)
        {
            Index = index;
            BColor = bg;
            FColor = fg;
            ZIndex = zIndex;
        }

        public GlyphEdit(int? index)
        {
            Index = index;
            BColor = null;
            FColor = null;
            ZIndex = null;
        }

        public GlyphEdit(Color? fg = null, Color? bg = null)
        {
            Index = null;
            FColor = fg;
            BColor = bg;
            ZIndex = null;
        }


    }
}
