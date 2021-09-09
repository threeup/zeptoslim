using System.Collections.Generic;
using System.IO;

namespace zeptolib
{
    public class CardManager
    {
        public static CardManager Instance = null;
        public Queue<Card> Deck = new Queue<Card>();
        public void Populate()
        {
            Instance = this;
            using(StreamReader file = new StreamReader("actions.csv")) {  
                string ln;  
                
                while ((ln = file.ReadLine()) != null) {  
                    Action action = ActionParser.MakeAction(ln);
                    Card card = new Card(ln, action, 1);
                    Deck.Enqueue(card);
                }  
                file.Close();  
            } 
        }

        public Card[] Grab(int count)
        {
            Card[] result = new Card[count];
            
            for(int i=0; i<count; ++i)
            {
                if(Deck.Count == 0)
                {
                    Populate();
                }
                result[i] = Deck.Dequeue();
            }
            return result;
        }

    }
}