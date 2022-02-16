// #define CONSOLE_DEBUG_MAKE
// #define CONSOLE_DEBUG_EXEC

using ZeptoFormula;
using ZeptoCommon;
using ZeptoBehave;

namespace ZeptoInstruction;

public class Consumer
{
    public Context ctx = new Context();
    private TreeNode? root = null;
    public void SetContext(string[]? varNames, string[]? methodNames)
    {
        List<string> varChunks = new List<string>();
        if (varNames != null)
        {
            varChunks.AddRange(varNames);
        }
        ctx.AddVariableNameList(varChunks);
        List<string> verbChunks = new List<string>();
        if (methodNames != null)
        {
            verbChunks.AddRange(methodNames);
        }
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
        root = new TreeNode(null, null, false);
        ChunkSpan span = new ChunkSpan(0, instrList.Count - 1);
#if CONSOLE_DEBUG_MAKE
        for (int i = span.start; i <= span.end; ++i)
        {
            Instruction instr = instrList[i];
            Console.WriteLine("[" + i + "] D:" + instr.depth + " " + instr.ToSourceString());
        }
#endif
        AppendTree(instrList, span, 0, root);
    }

    public void AppendTree(in List<Instruction> instrList, ChunkSpan span, int curDepth, TreeNode parent)
    {
        if (span.start > span.end)
        {
            return;
        }
#if CONSOLE_DEBUG_MAKE
        Console.WriteLine(curDepth+" ->"+span.ToString());
#endif
        TreeNode? prev = null;
        for (int idxInstruction = span.start; idxInstruction <= span.end; ++idxInstruction)
        {
            Instruction instr = instrList[idxInstruction];
            if (instr.depth == curDepth)
            {
                if (instr.IsIfConditional() && !parent.isDecider)
                {
                    TreeNode decider = new TreeNode(parent, null, true);
#if CONSOLE_DEBUG_MAKE
                    Console.WriteLine(parent.ToShortString(true)+".."+decider.ToString());
#endif
                    if (parent.children == null)
                    {
                        parent.children = new List<TreeNode>();
                    }
                    parent.children.Add(decider);
                    parent = decider;
                }
                else if (!instr.IsElseConditional() && parent.isDecider && parent.parent != null)
                {
                    parent = parent.parent;
                }
                TreeNode node = new TreeNode(parent, instr, false);

#if CONSOLE_DEBUG_MAKE
                Console.WriteLine(parent.ToShortString(true)+"..."+node.ToString());
#endif
                if (parent.children == null)
                {
                    parent.children = new List<TreeNode>();
                }
                parent.children.Add(node);
                prev = node;
            }
            else if (prev != null && instr.depth > curDepth)
            {
                int innerSpanStart = idxInstruction;
                int innerSpanEnd = span.end;
                int innerDepth = instr.depth;
                for (int idxInner = idxInstruction; idxInner <= span.end; ++idxInner)
                {
                    if (instrList[idxInner].depth < innerDepth)
                    {
                        innerSpanEnd = idxInner - 1;
                        break;
                    }
                }
                ChunkSpan nextSpan = new ChunkSpan(innerSpanStart, innerSpanEnd);
                AppendTree(instrList, nextSpan, innerDepth, prev);
                idxInstruction = innerSpanEnd;
            }
            else if (instr.depth < curDepth)
            {
                //something wrong
                break;
            }
        }
    }

    public void ConsumeStart()
    {
        if (root == null)
        {
            return;
        }
        ConsumeNode(root);
    }


    public TreeNodeExecution ConsumeNode(TreeNode current)
    {
        Instruction? instr = current.payload;
        TreeNodeExecution result = TreeNodeExecution.NORMAL;
        int? val = null;
        if (instr != null)
        {
            Expression? expr = instr.expression;
            if (expr != null)
            {
                val = expr.Calculate(ctx, ctx);
            }
            if (instr.IsIfConditional())
            {
                result = val != null && val > 0 ? TreeNodeExecution.CONDITION_PASS : TreeNodeExecution.CONDITION_FAIL;
            }
            else if (instr.IsElseConditional())
            {
                result = TreeNodeExecution.CONDITION_PASS;
            }
        }
#if CONSOLE_DEBUG_EXEC
        if (val != null)
        {
            Console.WriteLine(current.ToAncestryString() + val + " = " + current.ToString());
        }
        else
        {
            Console.WriteLine(current.ToAncestryString() + current.ToString());
        }
#endif      

        if (result != TreeNodeExecution.CONDITION_FAIL && current.children != null)
        {
            for (int i = 0; i < current.children.Count; ++i)
            {
                TreeNodeExecution childResult = ConsumeNode(current.children[i]);
                if (current.isDecider && childResult != TreeNodeExecution.CONDITION_FAIL)
                {
                    while (++i < current.children.Count)
                    {
#if CONSOLE_DEBUG_EXEC
                        Console.WriteLine(current.children[i].ToAncestryString() + current.children[i].ToString() + " SKIP");
#endif
                    }
                }
            }
        }
        return result;
    }
}
