
using System;
using System.Collections.Generic;
using ZeptoFormula;
using ZeptoCommon;

namespace ZeptoInstruction
{
    public static class InstructionFactory
    {

        public static IInstructionContext MakeContext(string varContents, string verbFileContents, string[] fileContents)
        {
            ZeptoCommon.Context ctx = new ZeptoCommon.Context();
            List<string> varChunks = Parser.CommaSeparatedIntoChunks(varContents);
            ctx.AddVariableNameList(varChunks);
            List<string> verbChunks = Parser.CommaSeparatedIntoChunks(verbFileContents);
            ctx.AddVerbNameList(verbChunks);

            Parser.StripComments(fileContents);
            List<string> buffer = new List<string>();
            for (int i = 0; i < fileContents.Length; ++i)
            {
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
                int depth = Parser.GetDepth(contents[i]);
                Parser.StringIntoChunks(contents[i], ref buffer);
                Instruction instr = new Instruction(depth);
                InstructionFactory.Make(instr, fctx, ctx, buffer, 0);
                list.Add(instr);
                Console.WriteLine(instr.ToLongString());
            }
        }


        public static void Make(Instruction instr, IFormulaContext fctx, IInstructionContext ictx, List<string> stringChunks, int chunkOffset)
        {
            for (int i = chunkOffset; i < stringChunks.Count; ++i)
            {
                string str = stringChunks[i];
                if (LogicConsts.ConditionStrings.ContainsKey(str))
                {
                    Instruction subInstruction = new Instruction(instr.depth, ZeptoInstruction.LogicConsts.ConditionStrings[str]);
                    Make(subInstruction, fctx, ictx, stringChunks, i+1);
                    instr.AddSubInstruction(subInstruction);
                    break;
                }
                if (ictx.ContainsVerbName(str))
                {
                    Instruction subInstruction = new Instruction(instr.depth, str);
                    Make(subInstruction, fctx, ictx, stringChunks, i+1);
                    instr.AddSubInstruction(subInstruction);
                    break;
                }
                //instr.AddFormula(FormulaFactory.Make(fctx, stringChunks));
            }
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