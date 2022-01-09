
using ZeptoBehave;

namespace TacticGame;
public class RuleLib : IZeptoRuleLib
{

  public string StringToAttrib(string val)
  {
    return InternalStringToAttrib(val);
  }
  private static string InternalStringToAttrib(string val)
  {
    if (val == "X")
    {
      return Attribs.X;
    }
    if (val == "Y")
    {
      return Attribs.Y;
    }
    if (val == "HP")
    {
      return Attribs.HP;
    }
    return val;
  }

  public void LoadStart(Pawn pawn, string dir, string fileName)
  {
    string filePath = Path.Combine(dir, fileName);
    using (StreamReader file = new StreamReader(filePath))
    {
      string? ln;
      while (true)
      {
        ln = file?.ReadLine();
        if (ln == null)
        {
          break;
        }
        PawnAction action = PawnActionParser.MakeAction(ln, this);
        action.PawnPerform(pawn);
      }

      file?.Close();
    }
  }


  public void LoadTile(Pawn pawn, string dir, string fileName)
  {
    string filePath = Path.Combine(dir, fileName);
    using (StreamReader file = new StreamReader(filePath))
    {
      string? ln;
      while (true)
      {
        ln = file?.ReadLine();
        if (ln == null)
        {
          break;
        }
        //Condition condition = ConditionParser.MakeAction(ln);
        //Action action = ActionParser.MakeAction(ln);
        //action.PawnPerform(pawn);
      }
      file?.Close();
    }
  }

  public void LoadEnd(Pawn pawn, string dir, string fileName)
  {
    string filePath = Path.Combine(dir, fileName);
    using (StreamReader file = new StreamReader(filePath))
    {
      string? ln;
      while (true)
      {
        ln = file?.ReadLine();
        if (ln == null)
        {
          break;
        }
        //Condition cond = ConditionParser.MakeCondition(ln);
      }
      file?.Close();
    }
  }
}