
using CsEcs;
using Microsoft.Xna.Framework;
using SadConsole.Components;
using SadSharp.Game;
using SadSharp.Helpers;
using System;
using System.Linq;

namespace NumberCruncher.Components
{
    //This is an experiment to see if we can wrap SadConsole.Entity
    public class SadWrapperComponent : Component<SadWrapperEdit>, IIndexable
    {
        public override Type MyType => typeof(SadWrapperComponent);
        public int X { get; private set; }
        public int Y { get; private set; }
        public double DrawX { get; private set; }
        public double DrawY { get; private set; }
        public int GlyphIndex { get; private set; }
        public int DrawGlyphIndex { get; private set; }
        public Color FColor { get; private set; }
        public Color DrawFColor { get; private set; }
        public Color BColor { get; private set; }
        public Color DrawBColor { get; private set; }
        public SadConsole.Entities.Entity SadEntity { get; private set; }

        public string IndexKey => $"{X}/{Y}";

        private int _offset;
        private GameConsole _console;


        public SadWrapperComponent(GameConsole con, int x, int y, int glyph, Color? fg = null, Color? bg = null)
        {
            _offset = con.IsBordered
                ? 16
                : 0;

            _console = con;

            var fcolor = fg ?? Color.White;
            var bcolor = bg ?? Color.Transparent;

            X = x;
            DrawX = x;

            Y = y;
            DrawY = y;

            GlyphIndex = glyph;
            DrawGlyphIndex = glyph;

            FColor = fcolor;
            DrawFColor = fcolor;

            BColor = bcolor;
            DrawBColor = bcolor;

            SadEntity = new SadConsole.Entities.Entity(FColor, BColor, GlyphIndex);
            SadEntity.UsePixelPositioning = true;
            SadEntity.Position = new Point((int)(DrawX * 16) + _offset, (int)(DrawY * 16) + _offset);

            //SadEntity.Components.Add(new EntityViewSyncComponent());

        }

        public override void OnAdd()
        {
            _console.Children.Add(SadEntity);
        }

        public override void OnDelete()
        {
            _console.Children.Remove(SadEntity);
        }

        public override void DoEdit(SadWrapperEdit values)
        {
            base.DoEdit(values);

            X = values?.X ?? X;
            Y = values?.Y ?? Y;
            DrawX = values?.DrawX ?? DrawX;
            DrawY = values?.DrawY ?? DrawY;
            FColor = values?.FColor ?? FColor;
            BColor = values?.BColor ?? BColor;
            DrawFColor = values?.DrawFColor ?? DrawFColor;
            DrawBColor = values?.DrawBColor ?? DrawBColor;
            GlyphIndex = values?.GlyphIndex ?? GlyphIndex;
            DrawGlyphIndex = values?.DrawGlyphIndex ?? DrawGlyphIndex;

            if (values?.DrawX != null || values?.DrawY != null)
            {
                SadEntity.Position = new Point((int)(DrawX * 16) + _offset, (int)(DrawY * 16) + _offset);
                SadEntity.Animation.IsDirty = true;
            }

            if (values?.DrawGlyphIndex != null)
            {
                SadEntity.Animation.CurrentFrame[0].Glyph = DrawGlyphIndex;
                SadEntity.Animation.IsDirty = true;
            }

            if (values?.DrawFColor != null)
            {
                SadEntity.Animation.CurrentFrame[0].Foreground = DrawFColor;
                SadEntity.Animation.IsDirty = true;
            }

            if (values?.DrawBColor != null)
            {
                SadEntity.Animation.CurrentFrame[0].Background = DrawBColor;
                SadEntity.Animation.IsDirty = true;
            }

        }

        public override IComponent Copy()
        {
            return this.DeepClone();
        }

        public Point ToXnaPoint(int dx = 0, int dy = 0)
        {
            return new Point(X + dx, Y + dy);
        }

        public Vector2 ToVector(float dx = 0, float dy = 0)
        {
            return new Vector2((float)X + dx, (float)Y + dy);
        }

        public RogueSharp.Point ToSharpPoint(int dx = 0, int dy = 0)
        {
            return new RogueSharp.Point(X + dx, Y + dy);
        }
    }

    public class SadWrapperEdit : IIndexable
    {
        public int? X { get; set; }
        public int? Y { get; set; }
        public double? DrawX { get; set; }
        public double? DrawY { get; set; }

        public int? GlyphIndex { get; set; }
        public int? DrawGlyphIndex { get; set; }

        public Color? FColor { get; set; }
        public Color? DrawFColor { get; set; }
        public Color? BColor { get; set; }
        public Color? DrawBColor { get; set; }

        public string IndexKey => (X == null || Y == null) ? null : $"{X}/{Y}";

        public static SadWrapperEdit AnimatePosition(double x, double y)
        {
            return new SadWrapperEdit { DrawX = x, DrawY = y };
        }

        public static SadWrapperEdit ChangePosition(int x, int y)
        {
            return new SadWrapperEdit { X = x, DrawX = x, Y = y, DrawY = y };
        }

        public static SadWrapperEdit ChangePositionPendingAnimation(int x, int y)
        {
            return new SadWrapperEdit { X = x, Y = y };
        }

        public static SadWrapperEdit AnimateColor(Color fg, Color? bg = null)
        {
            return new SadWrapperEdit { DrawFColor = fg, DrawBColor = bg };
        }

        public static SadWrapperEdit ChangeColor(Color fg, Color? bg = null)
        {
            return new SadWrapperEdit { FColor = fg, DrawFColor = fg, BColor = bg, DrawBColor = bg };
        }

        public static SadWrapperEdit AnimateGlyph(int index, Color? fg = null, Color? bg = null)
        {
            return new SadWrapperEdit { DrawGlyphIndex = index, DrawFColor = fg, DrawBColor = bg };
        }

        public static SadWrapperEdit ChangeGlyph(int index, Color? fg = null, Color? bg = null)
        {
            return new SadWrapperEdit
            {
                GlyphIndex = index,
                DrawGlyphIndex = index,
                FColor = fg,
                DrawFColor = fg,
                BColor = bg,
                DrawBColor = bg
            };
        }

    }
}
