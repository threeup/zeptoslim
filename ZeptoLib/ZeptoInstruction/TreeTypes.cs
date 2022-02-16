namespace ZeptoInstruction;

public struct ChunkSpan
{
    public int start;
    public int end;
    public ChunkSpan(int start, int end)
    {
        this.start = start;
        this.end = end;
    }
    public override string ToString()
    {
        return "[" + start + "_" + end + "]";
    }
}

public class ZeptoTest
{
    public string[]? varNames = null;
    public string[]? methodNames = null;
    public string[]? prepContents = null;
    public string[]? bodyContents = null;
    public int[]? answers = null;

    public ZeptoTest()
    {
    }
}


public class TreeNode
{
    public static int NodeID = 100;
    public static int GetNextNode() { return NodeID++; }
    public int id;
    public TreeNode? parent;
    public List<TreeNode>? children;
    public Instruction? payload;
    public bool isDecider;

    public TreeNode(TreeNode? parent, Instruction? payload, bool isDecider)
    {
        this.id = GetNextNode();
        this.parent = parent;
        children = null;
        this.payload = payload;
        this.isDecider = isDecider;
    }

    public bool IsIfConditional()
    {
        return payload != null && payload.IsIfConditional();
    }

    public bool IsElseConditional()
    {
        return payload != null && payload.IsElseConditional();
    }

    public int Ancestry()
    {
        if (parent != null)
        {
            return 1 + parent.Ancestry();
        }
        return 1;
    }

     public string ToAncestryString()
    {
        return new String(' ', Ancestry());
    }

    public string ToShortString(bool withAncestry)
    {
        if (withAncestry)
        {
            return ToAncestryString() + id.ToString();
        }
        return id.ToString();
    }

    public override string ToString()
    {
        if (isDecider)
        {
            return "<" + id + ">";
        }
        return "@" + id + " " + payload?.ToSourceString();
    }

}
