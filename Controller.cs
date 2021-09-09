using System.Collections.Generic;

namespace zeptolib
{
    public class Controller
    {
        public Pawn pawn;
        public List<Resource> resources = new List<Resource>();
        public List<Card> cards = new List<Card>();

        public void Possess(Pawn p_pawn) {
            if(pawn != null) {
                pawn.controlled = false;
            }
            if(p_pawn != null) {
                pawn = p_pawn;
                pawn.controlled = true;
            }
        }

        public void Setup()
        {
            cards.Clear();
            cards.AddRange(CardManager.Instance.Grab(5));
        }
        
        public void SelectSlot(Slot nextSlot)
        {
            
            switch (nextSlot)
            {
                case Slot.One: PickCard(0); break;
                case Slot.Two: PickCard(1); break;
                case Slot.Three: PickCard(2); break;
                case Slot.Four: PickCard(3); break;
                case Slot.Five: PickCard(4); break;
                case Slot.Cycle:
                    cards.Clear();
                    cards.AddRange(CardManager.Instance.Grab(5));
                    break;
                default: 
                    break;

            }
        }
        
        private void PickCard(int idx)
        {
            if (cards.Count <= idx)
            {
                return;
            }
            Card selected = cards[idx];
            cards.Remove(selected);
            cards.Insert(0, selected);
        }

        public virtual void Tick()
        {
            if (pawn != null) {
                
                pawn.Tick();
            }
        }
    }
}