//#define CONSOLE_DEBUG_MAKE
//#define CONSOLE_DEBUG_EXEC

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
        AppendTree(instrList, span, 0, root);
    }

    public void AppendTree(in List<Instruction> instrList, ChunkSpan span, int curDepth, TreeNode parent)
    {
        if (span.start > span.end)
        {
            return;
        }

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
                if (!instr.IsConditional() && parent.isDecider && parent.parent != null)
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
                int innerSpanEnd = idxInstruction;
                for (int idxInner = idxInstruction; idxInner < span.end; ++idxInner)
                {
                    if (instrList[idxInner].depth <= curDepth)
                    {
                        innerSpanEnd = idxInner - 1;
                        //idxInstruction should be idxInner next step but it will be incremented by for loop
                        idxInstruction = idxInner - 1;
                        break;
                    }
                }
                ChunkSpan nextSpan = new ChunkSpan(innerSpanStart, innerSpanEnd);
                AppendTree(instrList, nextSpan, instr.depth, prev);

            }
            else if (instr.depth < curDepth)
            {
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



    public bool ConsumeNode(TreeNode current)
    {
#if CONSOLE_DEBUG_EXEC

#endif
        Instruction? instr = current.payload;
        bool result = false;
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
                result = val != null && val > 0;
            }
            else if (instr.IsElseConditional())
            {
                result = true;
            }
        }
        else
        {
            result = true;
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

        if (result && current.children != null)
        {
            for (int i = 0; i < current.children.Count; ++i)
            {
                bool childResult = ConsumeNode(current.children[i]);
                if (current.isDecider)
                {
                    if (childResult)
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
        }
        return result;
    }
}
