
using System;
using System.Collections.Generic;
using ZeptoFormula;
using ZeptoCommon;

namespace ZeptoInstruction
{
    public static class InstructionFactory
    {

        public static void Make(ZeptoInstruction.Instruction instr, IFormulaContext fctx, IInstructionContext ictx, List<string> stringChunks)
        {
            //if explode(3) > laser(HP):
            //    SET HP = ENERGY*3=
            for (int i = instr.chunkIndex; i < stringChunks.Count; ++i)
            {
                string str = stringChunks[i];        
                if (str.StartsWith("\""))
                {
                    continue;
                }
                str = str.ToUpper();
                if (LogicConsts.ConditionStrings.ContainsKey(str))
                {
                    Instruction subInstruction = new Instruction(ZeptoInstruction.LogicConsts.ConditionStrings[str], i+1);
                    Make(subInstruction, fctx, ictx, stringChunks);
                    instr.AddSubInstruction(subInstruction);
                    break;
                }
                if (ictx.ContainsVerbName(str))
                {
                    Instruction subInstruction = new Instruction(ZeptoInstruction.LogicConsts.ConditionStrings[str], i+1);
                    Make(subInstruction, fctx, ictx, stringChunks);
                    instr.AddSubInstruction(subInstruction);
                    break;
                }
                instr.AddFormula(FormulaFactory.Make(fctx, stringChunks));
                break;
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