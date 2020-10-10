using CsEcs;
using System;

namespace NumberCruncher.Components
{
    public class HitPointComponent : Component<HitPointEdit>
    {
        public override Type MyType => typeof(HitPointComponent);
        public int MaxHits { get; private set; }
        public int CurrentHits { get; private set; }

        public HitPointComponent(int max, int? current = null)
        {
            MaxHits = max;
            CurrentHits = current ?? max;
        }

        public override void DoEdit(HitPointEdit values)
        {
            base.DoEdit(values);
            CurrentHits = values.CurrentHits ?? CurrentHits;
            MaxHits = values.MaxHits ?? MaxHits;
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
