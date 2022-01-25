namespace ZeptoInstruction;

public class Instruction
{
  public int depth = 0;
  public Condition condition;
  public Execution? execution = null;
  public string verb;


    public Instruction(int depth)
    {
        this.condition = Condition.NONE;
        this.verb = "";
        this.depth = depth;
    }

    public Instruction(int depth, Condition condition)
    {
        this.condition = condition;
        this.verb = "";
        this.depth = depth;
    }
    public Instruction(int depth, string verb)
    {
        this.condition = Condition.NONE;
        this.verb = verb;
        this.depth = depth;
    }



  public string ToLongString()
  {        
    System.Text.StringBuilder sb = new System.Text.StringBuilder();
    for (int i = 0; i < shift; ++i)
    {
        sb.Append(' ');
    }
    if(execution != null)
    {
      return "Inst Depth:" + depth + this.condition + " exec:" + execution?.ToString() ??"";
    }
    return "Inst Depth:" + depth + this.condition;
  }
}
