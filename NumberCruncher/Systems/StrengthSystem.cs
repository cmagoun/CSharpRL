using CsEcs;
using CsEcs.SimpleEdits;
using NumberCruncher.Components;

namespace NumberCruncher.Systems
{
    //So... how do I know to call this instead of strengthComponent.DoEdit???
    //We need to consider what that all means for the API
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
                sad.ChangeGlyph(Glyphs.Digit(newStrength));
            }
        }

        public static void UnreadySlots(Ecs ecs)
        {
            var entities = ecs.EntitiesWith("StrengthUsedComponent");

            foreach(var entityId in entities)
            {
                var slots = ecs.Get<StrengthSlotsComponent>(entityId);
                var strength = ecs.Get<StrengthComponent>(entityId);

                if (slots != null && strength.Strength > 1)
                {
                    slots.MakeUsed(strength.Strength);
                    ChangeStrength(entityId, 1, ecs);
                }

                ecs.RemoveComponent(entityId, "StrengthUsedComponent");
            }
        }
    }
}
