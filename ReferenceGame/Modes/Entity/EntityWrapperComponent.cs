
using CsEcs;
using Microsoft.Xna.Framework;
using SadConsole.Components;
using SadSharp.Game;
using SadSharp.Helpers;
using System;

namespace ReferenceGame.Components
{
    //This is an experiment to see if we can wrap SadConsole.Entity
    public class EntityWrapperComponent : Component<EntityWrapperEdit>
    {
        public override Type MyType => typeof(EntityWrapperComponent);
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

        private int _offset;

        public EntityWrapperComponent(GameConsole con, int x, int y, int glyph, Color? fg = null, Color? bg = null)
        {
            _offset = con.IsBordered
                ? 16
                : 0;

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
            SadEntity.Position = new Point( (int)(DrawX * 16) + _offset, (int)(DrawY * 16) + _offset);

            SadEntity.Components.Add(new EntityViewSyncComponent());

            con.Children.Add(SadEntity);
        }

        public override void DoEdit(EntityWrapperEdit values)
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

            SadEntity.Position = new Point((int)(DrawX * 16) + _offset, (int)(DrawY * 16) + _offset);
            SadEntity.SetGlyph(0, 0, DrawGlyphIndex);
            SadEntity.SetForeground(0, 0, DrawFColor);
            SadEntity.SetBackground(0, 0, DrawBColor); 
        }

        public override IComponent Copy()
        {
            return this.DeepClone();
        }

        public Point ToXnaPoint(int dx = 0, int dy = 0)
        {
            return new Point(X + dx, Y + dy);
        }
    }

    public class EntityWrapperEdit
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

        public static EntityWrapperEdit AnimatePosition(double x, double y)
        {
            return new EntityWrapperEdit { DrawX = x, DrawY = y };
        }

        public static EntityWrapperEdit ChangePosition(int x, int y)
        {
            return new EntityWrapperEdit { X = x, DrawX = x, Y = y, DrawY = y };
        }

        public static EntityWrapperEdit ChangePositionPendingAnimation(int x, int y)
        {
            return new EntityWrapperEdit { X = x, Y = y };
        }

        public static EntityWrapperEdit AnimateColor(Color fg, Color? bg = null)
        {
            return new EntityWrapperEdit { DrawFColor = fg, DrawBColor = bg };
        }

        public static EntityWrapperEdit ChangeColor(Color fg, Color? bg = null)
        {
            return new EntityWrapperEdit { FColor = fg, DrawFColor = fg, BColor = bg, DrawBColor = bg };
        }

        public static EntityWrapperEdit AnimateGlyph(int index, Color? fg = null, Color? bg = null)
        {
            return new EntityWrapperEdit { DrawGlyphIndex = index, DrawFColor = fg, DrawBColor = bg };
        }

        public static EntityWrapperEdit ChangeGlyph(int index, Color? fg = null, Color? bg = null)
        {
            return new EntityWrapperEdit
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
