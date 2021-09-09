namespace zeptolib
{
    public class Human : Controller
    {
        public int? direction = null;

        private int moveX = 0;
        private int moveY = 0;

        public void Move(int moveX, int moveY)
        {
            this.moveX = moveX;
            this.moveY = moveY;
        }

        public override void Tick()
        {
            if (pawn == null)
            {
                return;
            }
            if(moveX != 0 || moveY != 0)
            {
                pawn.Move(moveX, moveY);
                moveX = 0;
                moveY = 0;
                if(cards.Count > 0)
                {
                    Card card = cards[0] as Card;
                    card.Activate(pawn);
                    if(card.Count <= 0) {
                        cards.RemoveAt(0);
                    }
                }
            }
        }

    }
}