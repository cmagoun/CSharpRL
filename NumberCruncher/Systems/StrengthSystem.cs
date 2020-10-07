using CsEcs;
using CsEcs.SimpleEdits;
using NumberCruncher.Components;
using ReferenceGame.Components;

namespace NumberCruncher.Systems
{
    public static class StrengthSystem
    {
        public static void ChangeStrength(string entityId, int newStrength, Ecs ecs)
        {
            var strComp = ecs.Get<StrengthComponent>(entityId);
            var slots = ecs.Get<StrengthSlotsComponent>(entityId);

            if(slots == null || slots.IsReady(newStrength))
            {
                strComp.DoEdit(new IntEdit(newStrength));
                var sad = ecs.Get<SadWrapperComponent>(entityId);
                sad.DoEdit(SadWrapperEdit.ChangeGlyph(Glyphs.Digit(newStrength)));
            }
        }
    }
}
