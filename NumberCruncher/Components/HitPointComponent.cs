using CsEcs;
using CsEcs.SimpleEdits;
using System;

namespace NumberCruncher.Components
{
    public class HitPointComponent : Component<NoEdit>
    {
        public override Type MyType => typeof(HitPointComponent);
        public int MaxHits { get; private set; }
        public int CurrentHits { get; private set; }

        public HitPointComponent(int max, int? current = null)
        {
            MaxHits = max;
            CurrentHits = current ?? max;
        }

        public void UpdateHits(int change)
        {
            CurrentHits = Math.Min(MaxHits, CurrentHits + change);
        }

        public void SetHits(int newValue)
        {
            CurrentHits = Math.Min(MaxHits, newValue);
        }


        public override IComponent Copy()
        {
            return new HitPointComponent(MaxHits, CurrentHits);
        }
    }

    public class HitPointEdit
    {
        public int? MaxHits { get; set; }
        public int? CurrentHits { get; set; }

        public HitPointEdit(int current, int? max = null)
        {
            CurrentHits = current;
            MaxHits = max ?? MaxHits;
        }
    }
}
