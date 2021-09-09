namespace zeptolib
{
    public class Card : Resource
    {
        public delegate bool CardPawnAction(Pawn obj);

        public Action action = null;

        public Card(string name, Action action, int count = 1 ) :
            base(name, count)
        {
            this.action = action;
        }

        public void Activate(Pawn obj)
        {
            if (action.PawnPerform(obj))
            {
                Count--;
            }
        }
    }
}