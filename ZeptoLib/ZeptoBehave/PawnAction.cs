
using ZeptoCommon;

namespace ZeptoBehave;

public enum ZeptoOp
{
  NONE,
  INCREMENT,
  DECREMENT,
  ASSIGN,
}
public class PawnAction
{
  public List<PawnAction> subActions = new List<PawnAction>();
  private string attrib = string.Empty;
  private ZeptoOp op;
  private int val;

  public void Setup()
  {
    subActions.Clear();
  }
  public void AddSubAction(PawnAction next)
  {
    subActions.Add(next);
  }

  public void SetAttribChanger(string attrib, ZeptoOp op, int val)
  {
    this.attrib = attrib;
    this.op = op;
    this.val = val;
  }

  public bool PawnPerform(IZeptoPawn obj)
  {
    bool consumed = false;
    foreach (PawnAction sub in subActions)
    {
      consumed |= sub.PawnPerform(obj);
    }
    switch (this.op)
    {
      case ZeptoOp.ASSIGN:
        obj.SetAttrib(this.attrib, this.val);
        consumed = true;
        break;
      case ZeptoOp.INCREMENT:
        obj.ModifyAttrib(this.attrib, this.val);
        consumed = true;
        break;
      case ZeptoOp.DECREMENT:
        obj.ModifyAttrib(this.attrib, -this.val);
        consumed = true;
        break;
      default:
        break;
    }
    return consumed;


  }
}
