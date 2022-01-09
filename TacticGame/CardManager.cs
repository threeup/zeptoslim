
using ZeptoBehave;

namespace TacticGame;
public class CardManager
{
  public static CardManager? Instance = null;
  public Queue<Card> Deck = new Queue<Card>();
  private string cardPath = "";
  public void Populate(string? dir, string? fileName)
  {
    if (dir != null && fileName != null)
    {
      cardPath = Path.Combine(dir, fileName);
    }
    Instance = this;
    RuleLib rl = new RuleLib();
    using (StreamReader file = new StreamReader(cardPath))
    {
      string? ln;
      while (file != null && (ln = file.ReadLine()) != null)
      {
        PawnAction action = PawnActionParser.MakeAction(ln, rl);
        Card card = new Card(ln, action, 1);
        Deck.Enqueue(card);
      }
      if (file != null)
      {
        file.Close();
      }
    }
  }

  public Card[] Grab(int count)
  {
    Card[] result = new Card[count];

    for (int i = 0; i < count; ++i)
    {
      if (Deck.Count == 0)
      {
        Populate(null, null);
      }
      result[i] = Deck.Dequeue();
    }
    return result;
  }

}