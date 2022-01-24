namespace ZeptoInstruction;

public class Instruction
{
    public int depth = 0;
    public Condition condition;
    public string verb;

    private List<Instruction>? subInstructions = null;
    private string comments = "";
    private ZeptoFormula.Formula? formula = null;

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

    public void AddSubInstruction(Instruction sub)
    {
        if (subInstructions == null)
        {
            subInstructions = new List<Instruction>();
        }
        subInstructions.Add(sub);
    }

    public void AddFormula(ZeptoFormula.Formula formula)
    {
        this.formula = formula;
    }
    public void AddComments(string comments)
    {
        this.comments = comments;
    }

    public string ToLongString(int shift = 0)
    {
        System.Text.StringBuilder sb = new System.Text.StringBuilder();
        for (int i = 0; i < shift; ++i)
        {
            sb.Append(' ');
        }
        int subCount = subInstructions != null ? subInstructions.Count : 0;
        sb.AppendLine("Inst " + verb + "." + this.condition + " Depth:" + depth + " sub:" + subCount + " //" + comments);
        if (subInstructions != null)
        {
            foreach (Instruction instr in subInstructions)
            {
                sb.AppendLine(instr.ToLongString(shift + 3));
            }
        }
        return sb.ToString();
    }
}
