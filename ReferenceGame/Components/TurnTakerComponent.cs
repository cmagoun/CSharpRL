using CsEcs;
using SadSharp.Helpers;
using System;

namespace ReferenceGame.Components
{
    public class TurnTakerComponent:Component<TurnTakerEdit>
    {
        public override Type MyType => typeof(TurnTakerComponent);

        public int NextTurn { get; private set; }
        public ITurnTaker TurnTaker { get; private set; }

        public TurnTakerComponent(ITurnTaker turnTaker, int startTurn)
        {
            TurnTaker = turnTaker;
            NextTurn = startTurn;
        }

        public override void DoEdit(TurnTakerEdit values)
        {
            base.DoEdit(values);
            NextTurn = values.NextTurn ?? NextTurn;
            TurnTaker = values.TurnTaker ?? TurnTaker;
        }

        public override IComponent Copy()
        {
            return new TurnTakerComponent(TurnTaker.DeepClone(), NextTurn);
        }

    }

    public class TurnTakerEdit
    {
        public int? NextTurn { get; set; }
        public ITurnTaker TurnTaker { get; set; }

        public TurnTakerEdit(int? nextTurn = null, ITurnTaker newTurnTaker = null)
        {
            NextTurn = nextTurn;
            TurnTaker = newTurnTaker;
        }

        public TurnTakerEdit(ITurnTaker newTurnTaker)
        {
            NextTurn = null;
            TurnTaker = newTurnTaker;
        }
    }
}
