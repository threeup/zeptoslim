
using ZeptoFormula;
using ZeptoCommon;
using ZeptoBehave;

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
public static class InstructionFactory
{

    public static IInstructionContext MakeContext(string varContents, string verbContents, string[] contents)
    {
        Context ctx = new Context();
        List<string> varChunks = Parser.CommaSeparatedIntoChunks(varContents.ToUpper());
        ctx.AddVariableNameList(varChunks);
        List<string> verbChunks = Parser.CommaSeparatedIntoChunks(verbContents.ToUpper());
        ctx.AddVerbNameList(verbChunks);

        List<string> buffer = new List<string>();
        for (int i = 0; i < contents.Length; ++i)
        {
            int depth;
            string line = Parser.Sanitize(contents[i], out depth);
            Parser.StringIntoChunks(line, ref buffer);
            Formula f = FormulaFactory.Make(ctx, buffer);
            f.Calculate(ctx);
        }
        return ctx as IInstructionContext;
    }

    public static void MakeList(IInstructionContext ctx, string[] contents,
        ref List<Instruction> list, ref List<string> buffer)
    {
        IFormulaContext? fctx = ctx as IFormulaContext;
        if (fctx != null)
        {
            for (int i = 0; i < contents.Length; ++i)
            {
                int depth;
                string line = Parser.Sanitize(contents[i], out depth);
                Parser.StringIntoChunks(line, ref buffer);
                Instruction instr = new Instruction(depth);
                InstructionFactory.Make(instr, fctx, ctx, buffer, new ChunkSpan(-1, 999));
                list.Add(instr);
            }
        }
    }


    public static void Make(Instruction instr, IFormulaContext fctx, IInstructionContext ictx,
        List<string> stringChunks, ChunkSpan cspan)
    {
        int start = System.Math.Max(cspan.start, 0);
        int end = System.Math.Min(cspan.end, stringChunks.Count - 1);
        for (int i = start; i <= end; ++i)
        {
            string str = stringChunks[i];
            if (LogicConsts.ConditionStrings.ContainsKey(str))
            {
                Instruction subInstruction = new Instruction(instr.depth, ZeptoInstruction.LogicConsts.ConditionStrings[str]);
                ChunkSpan gutSpan = new ChunkSpan(i + 1, end);
                Make(subInstruction, fctx, ictx, stringChunks, gutSpan);
                subInstruction.AddComments(GutsToString(stringChunks, gutSpan));
                instr.AddSubInstruction(subInstruction);
                break;
            }

            // bar(5+foo(20+1))
            if (ictx.ContainsVerbName(str))
            {
                Instruction subInstruction = new Instruction(instr.depth, str);
                ChunkSpan gutSpan;
                FindVerbGuts(stringChunks, new ChunkSpan(i + 1, end), out gutSpan);
                Make(subInstruction, fctx, ictx, stringChunks, gutSpan);
                subInstruction.AddComments(GutsToString(stringChunks, gutSpan));

                instr.AddSubInstruction(subInstruction);
                i = gutSpan.end + 1;
            }
            else
            {
                ChunkSpan gutSpan = new ChunkSpan(i, end);
                Formula f = new Formula();
                instr.AddFormula(f);
                instr.AddComments(GutsToString(stringChunks, gutSpan));
                i = gutSpan.end + 1;
            }
        }
    }

    public static string GutsToString(List<string> stringChunks, ChunkSpan cspan)
    {
        int start = System.Math.Max(cspan.start, 0);
        int end = System.Math.Min(cspan.end, stringChunks.Count - 1);
        System.Text.StringBuilder sb = new System.Text.StringBuilder();
        for (int i = start; i <= end; ++i)
        {
            sb.Append(stringChunks[i]);
        }
        return sb.ToString();
    }

    public static void FindVerbGuts(List<string> stringChunks, ChunkSpan cspan, out ChunkSpan inside)
    {
        inside = new ChunkSpan(-1, 999);
        bool foundBrace = false;
        for (int i = cspan.start; i < cspan.end; ++i)
        {
            string str = stringChunks[i];
            if (str == "(")
            {
                inside.start = i + 1;
                foundBrace = true;
                break;
            }
        }
        if (!foundBrace)
        {
            return;
        }

        for (int i = cspan.end; i >= inside.start; --i)
        {
            string str = stringChunks[i];
            if (str == ")")
            {
                inside.end = i - 1;
                break;
            }
        }

        return;
    }


    public static void FindFormulaGuts(List<string> stringChunks, ChunkSpan cspan, out ChunkSpan inside)
    {
        inside = new ChunkSpan(-1, 999);
        for (int i = cspan.start; i < cspan.end; ++i)
        {
            string str = stringChunks[i];
            if (str == "(")
            {
                inside.start = i + 1;
                break;
            }
        }
        for (int i = cspan.end; i >= cspan.start; --i)
        {
            string str = stringChunks[i];
            if (str == ")")
            {
                inside.end = i - 1;
                break;
            }
        }
        return;
    }


    // public static GumboInstruction InstructionFromChunks(List<string> stringChunks, int depth)
    // {
    //     GumboInstruction result;
    //     if (stringChunks.Count == 0)
    //     {
    //         result = GumboInstruction.NoopInstruction;
    //         return result;
    //     }

    //     bool isIf = false;
    //     bool isElse = false;
    //     GumboVerb verb = GumboVerb.NOOP;

    //     int chunkIdx = 0;
    //     if (stringChunks[chunkIdx].Equals("?") ||
    //         stringChunks[chunkIdx].Equals(GumboEnum<GumboSyntax>.GetName(GumboSyntax.IF)))
    //     {
    //         isIf = true;
    //         chunkIdx++;
    //     }
    //     else if (stringChunks[chunkIdx].Equals(":?") ||
    //         stringChunks[chunkIdx].Equals(GumboEnum<GumboSyntax>.GetName(GumboSyntax.ELSEIF)))
    //     {
    //         isIf = true;
    //         isElse = true;
    //         chunkIdx++;
    //     }
    //     else if (stringChunks[chunkIdx].Equals(":") ||
    //         stringChunks[chunkIdx].Equals(GumboEnum<GumboSyntax>.GetName(GumboSyntax.ELSE)))
    //     {
    //         isElse = true;
    //         chunkIdx++;
    //     }

    //     if (chunkIdx < stringChunks.Count)
    //     {
    //         string str = stringChunks[chunkIdx];
    //         if (!GumboEnum<GumboVerb>.IsDefined(str))
    //         {
    //             logGumbo.Error("Invalid verb " + string.Join(",", stringChunks.ToArray()));
    //             verb = GumboVerb.INVALID;
    //         }
    //         else
    //         {
    //             verb = GumboEnum<GumboVerb>.Parse(str);
    //         }
    //         chunkIdx++;
    //     }

    //     stringChunks.RemoveRange(0, chunkIdx);

    //     result = new GumboInstruction(depth, isIf, isElse, verb);
    //     return result;
    // }

}

