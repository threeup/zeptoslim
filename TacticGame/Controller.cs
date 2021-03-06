
namespace TacticGame;
  public class Controller
  {
    public Pawn? pawn = null;
    public List<Card> cards = new List<Card>();

    private System.Text.StringBuilder sb = new System.Text.StringBuilder();

    public void Possess(Pawn p_pawn)
    {
      if (pawn != null)
      {
        pawn.controlled = false;
      }
      if (p_pawn != null)
      {
        pawn = p_pawn;
        pawn.controlled = true;
      }
    }

    public void Setup()
    {
      cards.Clear();
      if (CardManager.Instance != null)
      {
        cards.AddRange(CardManager.Instance.Grab(5));
      }
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
          if (CardManager.Instance != null)
          {
            cards.AddRange(CardManager.Instance.Grab(5));
          }
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
      if (pawn != null)
      {

        pawn.Tick();
      }
    }

    public string GetAttributesString()
    {
      sb.Clear();
      if (pawn != null)
      {
        foreach (KeyValuePair<string, int> kvp in pawn.attributes)
        {
          sb.Append(kvp.Key);
          sb.Append(":");
          sb.Append(kvp.Value);
          sb.Append(", ");
        }
      }
      return sb.ToString();
    }
  }