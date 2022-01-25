
using ZeptoFormula;
using ZeptoCommon;
using ZeptoBehave;

namespace ZeptoInstruction;

public class ZeptoInstructionNode
{

}

public class Consumer
{
    public Context ctx = new Context();
    private TreeNode? root = null;
    public void SetContext(string varContents, string verbContents)
    {
        List<string> varChunks = Parser.CommaSeparatedIntoChunks(varContents);
        ctx.AddVariableNameList(varChunks);
        List<string> verbChunks = Parser.CommaSeparatedIntoChunks(verbContents);
        ctx.AddMethodNameList(verbChunks);
    }
    public void ConsumeFormulaList(string[] formulaList)
    {
        List<string> buffer = new List<string>();
        for (int i = 0; i < formulaList.Length; ++i)
        {
            int depth;
            string line = Parser.Sanitize(formulaList[i], out depth);
            Parser.StringIntoChunks(line, ref buffer);
            Formula f = FormulaFactory.Make(ctx, buffer);
            f.Calculate(ctx);
        }
    }
    public void CopyContext(Context ctx)
    {
        // todo
        this.ctx.CopyMethodData(ctx);
        this.ctx.CopyVariableData(ctx);
    }

    public void MakeTree(in List<Instruction> instrList)
    {
        root = new TreeNode(null, null);
        AppendTree(instrList, new ChunkSpan(0, instrList.Count), root);
    }

    public void AppendTree(in List<Instruction> instrList, ChunkSpan span, TreeNode parent)
    {
        if (span.start == span.end)
        {
            return;
        }

        int first = span.start;
        int listDepth = instrList[first].depth;

        TreeNode? prev = null;
        for (int i = span.start; i < span.end; ++i)
        {
            Instruction instr = instrList[i];
            if (instr.depth == listDepth)
            {
                TreeNode node = new TreeNode(parent, instr);
                if (parent.children == null)
                {
                    parent.children = new List<TreeNode>();
                }
                parent.children.Add(node);
                prev = node;
            }
            else if (prev != null && instr.depth > listDepth)
            {
                int innerSpanStart = i;
                int innerSpanEnd = i;
                for (int j = i; j < span.end; ++j)
                {
                    if (instrList[j].depth <= listDepth)
                    {
                        innerSpanEnd = j - 1;
                    }
                }
                AppendTree(instrList, new ChunkSpan(innerSpanStart, innerSpanEnd), prev);
            }
            else if (instr.depth < listDepth)
            {
                break;
            }
        }
    }

    public void ConsumeRoot()
    {
        if (root == null)
        {
            return;
        }
        bool skipConditional;
        ConsumeNode(root, out skipConditional);
    }



    public void ConsumeNode(TreeNode current, out bool parentShouldSkip)
    {
        parentShouldSkip = false;
        Instruction? instr = current.payload;
        bool doChildren = current.children != null;
        if (instr != null)
        {
            Expression? expr = instr.expression;
            if (expr != null)
            {
                int val = expr.Calculate(ctx, ctx);
                if (instr.condition == Condition.IF || instr.condition == Condition.ELSEIF)
                {
                    doChildren = current.children != null && val > 0;
                    parentShouldSkip = true;
                }

            }
        }
        if (doChildren && current.children != null)
        {
            bool skipConditional = false;
            for (int i = 0; i < current.children.Count; ++i)
            {
                bool skip = false;
                if (skipConditional)
                {
                    if (current.children[i].IsConditional())
                    {
                        skip = true;
                    }
                    else
                    {
                        skipConditional = false;
                    }
                }
                if (!skip)
                {
                    bool childWantsSkip;
                    ConsumeNode(current.children[i], out childWantsSkip);
                    skipConditional = childWantsSkip;
                }
            }
        }
    }
}
