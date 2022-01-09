namespace TacticGame;
  public class Human : Controller
  {


    private bool dirtyMove = false;

    public void Move(int moveX, int moveY)
    {
      if (pawn == null)
      {
        return;
      }
      pawn.ModifyAttrib(Attribs.X, moveX);
      pawn.ModifyAttrib(Attribs.Y, moveY);
      dirtyMove = true;
    }

    public override void Tick()
    {
      if (pawn == null)
      {
        return;
      }
      if (dirtyMove)
      {
        dirtyMove = false;
        if (cards.Count > 0)
        {
          Card card = cards[0] as Card;
          card.Activate(pawn);
          if (card.Count <= 0)
          {
            cards.RemoveAt(0);
          }
        }
        pawn.Move();
      }
    }

  }