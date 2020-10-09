using CsEcs;
using CsEcs.SimpleEdits;
using NumberCruncher.Systems;
using System;
using System.Collections.Generic;
using System.Text;

namespace NumberCruncher.Components
{
    public class ScoreComponent : Component<NoEdit>
    {
        public override Type MyType => typeof(ScoreComponent);
        public int Score { get; private set; }
        public int Refresh { get; private set; }

        public ScoreComponent(int score = 0, int refresh = 0)
        {
            Score = score;
            Refresh = refresh;
        }

        public void UpdateScore(int points)
        {
            Score += points;
            Refresh += points;

            var refreshNumber = Refresh % 10;
            
            var slots = MyEcs.Get<StrengthSlotsComponent>(EntityId);
            if(!slots.IsReady(refreshNumber))
            {
                slots.MakeReady(refreshNumber);
                Refresh = 0;
            }
        }

        public override IComponent Copy()
        {
            return new ScoreComponent(Score, Refresh);
        }
    }
}
