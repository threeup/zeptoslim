namespace zeptolib
{
    public class Human : Controller
    {
        Action.Slot? currentSlot = null;

        public void DoAction(Action.Slot nextSlot)
        {
            currentSlot = nextSlot;
        }

        public override void Tick()
        {
            if (pawn == null)
            {
                return;
            }
            switch (currentSlot)
            {
                case Action.Slot.One: TryCard(0); break;
                case Action.Slot.Two: TryCard(1); break;
                case Action.Slot.Three: TryCard(2); break;
                case Action.Slot.Four: TryCard(3); break;
                case Action.Slot.Five: TryCard(4); break;
                case Action.Slot.Cycle:
                    items.Clear();
                    items.AddRange(CardManager.Grab(5));
                    break;

            }
            currentSlot = null;
        }

        void TryCard(int idx)
        {
            if (items.Count <= idx)
            {
                return;
            }
            Card card = items[idx] as Card;
            card.Activate(pawn);
            if (card.Count == 0)
            {
                items.RemoveAt(idx);
            }
        }
    }
}