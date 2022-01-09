
namespace ZeptoBehave;
public static class PawnActionParser
{
  private static string IncrementString = "+=";
  private static string[] IncrementStringSep = new string[] { IncrementString };
  private static string DecrementString = "-=";
  private static string[] DecrementStringSep = new string[] { DecrementString };
  private static string AssignmentString = "=";
  private static string[] AssignmentStringSep = new string[] { AssignmentString };
  public static PawnAction MakeAction(string line, IZeptoRuleLib rulelib)
  {
    PawnAction action = new PawnAction();
    action.Setup();
    if (line.Contains(","))
    {
      string[] chunks = line.Split(',');

      foreach (string chunk in chunks)
      {
        action.AddSubAction(MakeAction(chunk, rulelib));
      }
      return action;
    }
    else
    {

      if (line.Contains(IncrementString))
      {
        String[] chunks = line.Split(IncrementStringSep, StringSplitOptions.None);
        string attrib = rulelib.StringToAttrib(chunks[0]);
        int val = int.Parse(chunks[1]);
        action.SetAttribChanger(attrib, ZeptoOp.INCREMENT, val);
      }
      else if (line.Contains(DecrementString))
      {
        string[] chunks = line.Split(DecrementStringSep, StringSplitOptions.None);
        string attrib = rulelib.StringToAttrib(chunks[0]);
        int val = int.Parse(chunks[1]);
        action.SetAttribChanger(attrib, ZeptoOp.DECREMENT, val);
      }
      else if (line.Contains(AssignmentString))
      {
        string[] chunks = line.Split(AssignmentStringSep, StringSplitOptions.None);
        string attrib = rulelib.StringToAttrib(chunks[0]);
        int val = int.Parse(chunks[1]);
        action.SetAttribChanger(attrib, ZeptoOp.ASSIGN, val);
      }

      return action;
    }
  }

}
