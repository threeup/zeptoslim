
using System;
using System.Collections.Generic;
using ZeptoFormula;
using ZeptoCommon;

namespace ZeptoInstruction
{
    public static class InstructionFactory
    {

        public static IInstructionContext MakeContext(string varContents, string verbContents, string[] contents)
        {
            ZeptoCommon.Context ctx = new ZeptoCommon.Context();
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
            IFormulaContext fctx = ctx as IFormulaContext;
            for (int i = 0; i < contents.Length; ++i)
            {
                int depth;
                string line = Parser.Sanitize(contents[i], out depth);
                Parser.StringIntoChunks(line, ref buffer);
                Instruction instr = new Instruction(depth);
                InstructionFactory.Make(instr, fctx, ctx, buffer);
                list.Add(instr);
            }
        }


        public static void Make(Instruction instr, IFormulaContext fctx, IInstructionContext ictx,
            List<string> stringChunks, int chunkStart = 0, int chunkEnd = -1)
        {
            int end = chunkEnd < 0 ? stringChunks.Count : chunkEnd;
            for (int i = chunkStart; i < end; ++i)
            {
                string str = stringChunks[i];
                if (LogicConsts.ConditionStrings.ContainsKey(str))
                {
                    Instruction subInstruction = new Instruction(instr.depth, ZeptoInstruction.LogicConsts.ConditionStrings[str]);
                    Make(subInstruction, fctx, ictx, stringChunks, i + 1);
                    instr.AddSubInstruction(subInstruction);
                    break;
                }
                if (ictx.ContainsVerbName(str))
                {
                    Instruction subInstruction = new Instruction(instr.depth, str);
                    int afterLeft, beforeRight;
                    FindGuts(stringChunks, i+1, out afterLeft, out beforeRight);
                    Make(subInstruction, fctx, ictx, stringChunks, afterLeft, beforeRight);
                    instr.AddSubInstruction(subInstruction);
                    i = beforeRight + 2;
                }
                else
                {
                    //instr.AddFormula(FormulaFactory.Make(fctx, stringChunks, i));
                }
            }
        }

        public static void FindGuts(List<string> stringChunks, int startIdx, out int afterLeft, out int beforeRight)
        {
            int braceCount = 0;
            afterLeft = startIdx + 1;
            beforeRight = -1;
            for (int i = startIdx; i < stringChunks.Count; ++i)
            {
                string str = stringChunks[i];
                if (str == "(")
                {
                    braceCount++;
                }
                else if (str == ")")
                {
                    braceCount--;
                }
                if (braceCount == 0)
                {
                    beforeRight = i - 1;
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

}