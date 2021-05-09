namespace zeptolib
{
    public class Card : Item
    {
        public delegate bool CardPawnAction(Pawn obj);

        public CardPawnAction performAction = null;

        public Card(string p_name, CardPawnAction p_action, int p_count = 1 ) :
            base(p_name, p_count)
        {
            performAction = p_action;
        }

        public void Activate(Pawn obj)
        {
            if (performAction(obj))
            {
                Count--;
            }
        }
    }
}