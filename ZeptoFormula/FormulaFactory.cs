
using System;
using System.Collections.Generic;
using ZeptoCommon;

namespace ZeptoFormula
{
    public static class FormulaFactory
    {
        public static IFormulaContext MakeContext(string varContents, string[] fileContents)
        {
            IFormulaContext ctx = new ZeptoCommon.Context();
            List<string> varChunks = Parser.CommaSeparatedIntoChunks(varContents);
            ctx.AddVariableNameList(varChunks);

            Parser.StripComments(fileContents);
            List<string> buffer = new List<string>();
            for(int i=0; i<fileContents.Length; ++i)
            {
                Parser.StringIntoChunks(fileContents[i], ref buffer);
                Formula f = FormulaFactory.Make(ctx, buffer);
                f.Calculate(ctx);
            }
            return ctx;
        }

        public static Formula Make(IFormulaContext ctx, List<string> stringChunks)
        {
            Formula resultFormula;
            List<FormulaElement> elementList = new List<FormulaElement>();
            resultFormula = new Formula();
            for (int i = 0; i < stringChunks.Count; ++i)
            {
                string str = stringChunks[i];        
                if (str.StartsWith("\""))
                {
                    continue;
                }
                str = str.ToUpper();
                if (RPNConsts.AssignStrings.ContainsKey(str))
                {
                    FormulaElementType etype = RPNConsts.AssignStrings[str];
                    elementList.Add(new FormulaElement(etype));
                }
                else if (RPNConsts.OperatorStrings.ContainsKey(str))
                {
                    FormulaElementType etype = RPNConsts.OperatorStrings[str];
                    elementList.Add(new FormulaElement(etype));
                }
                else if(ctx.ContainsVariableName(str))
                {
                    int varIndex = ctx.GetVariableIndex(str);
                    int registerPlace = resultFormula.AddBinding(varIndex);
                    FormulaElementType elType = Formula.RegisterFromInt(registerPlace);
                    elementList.Add(new FormulaElement(elType));
                }
                else
                {
                    int number = 0;
                    bool success = int.TryParse(str, out number);
                    if (success)
                    {
                        elementList.Add(new FormulaElement(number));
                    }
                    else 
                    {
                        throw new Exception("What is this, "+str);
                    }
                }
            }

            resultFormula.AddElementList(elementList);
            if(resultFormula.IsMismatched())
            {
                throw new Exception("Mismatched, "+resultFormula.ToLongString(ctx));
            }
            return resultFormula;
        }

    }
}