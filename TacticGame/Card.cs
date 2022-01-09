
using ZeptoBehave;


namespace TacticGame;
public class Card : Resource
{
  public delegate bool CardPawnAction(Pawn obj);

  public PawnAction? action = null;

  public Card(string name, PawnAction action, int count = 1) :
      base(name, count)
  {
    this.action = action;
  }

  public void Activate(Pawn obj)
  {
    if (action != null && action.PawnPerform(obj))
    {
      Count--;
    }
  }
}