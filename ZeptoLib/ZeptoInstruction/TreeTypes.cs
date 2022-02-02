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


public class TreeNode
{
    public TreeNode? parent;
    public List<TreeNode>? children;
    public Instruction? payload;

    public TreeNode(TreeNode? parent, Instruction? payload)
    {
        this.parent = parent;
        children = null;
        this.payload = payload;
    }

    public bool IsIfConditional()
    {
        return payload != null && payload.IsIfConditional();
    }

    public bool IsElseConditional()
    {
        return payload != null && payload.IsElseConditional();
    }
    
    public override string ToString()
    {
        return "@"+payload?.ToSourceString();
    }

}   
