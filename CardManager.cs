 
namespace zeptolib
{
    public class CardManager
    {
        public static Card[] Grab(int count)
        {
            Card[] result = new Card[count];
            for(int i=0; i<count; ++i)
            {
                if(i<2)
                {
                    result[i] = GrabBackward();
                }
                else
                {
                    result[i] = GrabForward();
                }
            }
            return result;
        }

        public static Card GrabForward()
        {
            return new Card("F,Forward", Action.PawnForward);
        }
        
        public static Card GrabBackward()
        {
            return new Card("B,Backward", Action.PawnBackward);
        }
        
        public static Card GrabLeft()
        {
            return new Card("L,Left", Action.PawnLeft);
        }
        
        public static Card GrabRight()
        {
            return new Card("R,Right", Action.PawnRight);
        }
    }
}