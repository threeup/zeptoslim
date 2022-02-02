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
    public void SetContext(string[]? varContents, string[]? verbContents)
    {
        List<string> varChunks = new List<string>();
        if(varContents != null)
        {
            for(int i=0; i<varContents.Length; ++i)
            {
                varChunks.AddRange(Parser.CommaSeparatedIntoChunks(varContents[i]));
            }
        }
        ctx.AddVariableNameList(varChunks);
        List<string> verbChunks = new List<string>();
        if(verbContents != null)
        {
            for(int i=0; i<verbContents.Length; ++i)
            {
                verbChunks.AddRange(Parser.CommaSeparatedIntoChunks(verbContents[i]));
            }
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
        root = new TreeNode(null, null);
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

        TreeNode? prev = null;
        for (int idxInstruction = span.start; idxInstruction <= span.end; ++idxInstruction)
        {
            Instruction instr = instrList[idxInstruction];
            if (instr.depth == curDepth)
            {
                TreeNode node = new TreeNode(parent, instr);

#if CONSOLE_DEBUG_MAKE
                Console.WriteLine("[" + idxInstruction + "] to " + parent.ToString());
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
#if CONSOLE_DEBUG_MAKE
                Console.WriteLine(prev + " nextSpan" + nextSpan + " at D:" + instr.depth);
#endif
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
        bool skipConditional;

        ConsumeNode(root, out skipConditional);
    }



    public void ConsumeNode(TreeNode current, out bool drilledIn)
    {
        drilledIn = false;
        Instruction? instr = current.payload;
        bool doChildren = current.children != null;
        if (instr != null)
        {
            Expression? expr = instr.expression;
            if (expr != null)
            {
                int val = expr.Calculate(ctx, ctx);
#if CONSOLE_DEBUG_EXEC
                Console.WriteLine(val + " = " + current.ToString() + (current.children != null ? " ~" + current.children.Count : "."));
#endif                
                if (instr.IsIfConditional())
                {
                    doChildren = current.children != null && val > 0;
                    drilledIn = true;
                }

            }
        }
        else
        {
#if CONSOLE_DEBUG_EXEC
            Console.WriteLine(current.ToString() + (current.children != null ? " ~" + current.children.Count : "."));
#endif                  
        }

        if (doChildren && current.children != null)
        {
            bool skipElse = false;
            for (int i = 0; i < current.children.Count; ++i)
            {
                bool skip = false;
                if (skipElse)
                {
                    if (current.children[i].IsElseConditional())
                    {
                        skip = true;
                    }
                    else
                    {
                        skipElse = false;
                    }
                }


                if (!skip)
                {
                    bool childDrilledIn;
                    ConsumeNode(current.children[i], out childDrilledIn);
                    skipElse = childDrilledIn;
                }
                else
                {
#if CONSOLE_DEBUG_EXEC
                    Console.WriteLine("Skip:" + current.children[i].ToString());
#endif                    
                }
            }
        }
    }
}
