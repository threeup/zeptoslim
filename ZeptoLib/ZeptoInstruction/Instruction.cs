namespace ZeptoInstruction;

public class Instruction
{
    public int depth = 0;
    public Condition condition;
    public Expression? expression = null;

    public String source;


    public Instruction(int depth, string line)
    {
        this.condition = Condition.NONE;
        this.depth = depth;
        this.source = line;
    }

    public bool IsIfConditional()
    {
        return condition == Condition.IF || condition == Condition.ELSEIF;
    }


    public bool IsElseConditional()
    {
        return condition == Condition.ELSEIF || condition == Condition.ELSE;
    }

    public string ToLongString()
    {
        System.Text.StringBuilder sb = new System.Text.StringBuilder();
        for (int i = 0; i < depth; ++i)
        {
            sb.Append(' ');
        }
        if (this.condition != Condition.NONE)
        {
            sb.Append(this.condition.ToString());
        }
        if (expression != null)
        {
            sb.Append(" expr:");
            sb.Append(expression?.ToString() ?? "");
        }
        return sb.ToString();
    }

    public string ToSourceString()
    {
        return source;
    }
}
