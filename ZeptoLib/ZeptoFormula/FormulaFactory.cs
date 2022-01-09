
using ZeptoCommon;
using ZeptoBehave;

namespace ZeptoFormula;

public static class FormulaFactory
{
  public static IFormulaContext MakeContext(string varContents, string[] contents)
  {
    IFormulaContext ctx = new Context();
    List<string> varChunks = Parser.CommaSeparatedIntoChunks(varContents);
    ctx.AddVariableNameList(varChunks);

    List<string> buffer = new List<string>();
    for (int i = 0; i < contents.Length; ++i)
    {
      int depth;
      string line = Parser.Sanitize(contents[i], out depth);
      Parser.StringIntoChunks(line, ref buffer);
      Formula f = FormulaFactory.Make(ctx, buffer);
      f.Calculate(ctx);
    }
    return ctx;
  }

  public static Formula Make(IFormulaContext ctx, List<string> stringChunks, int chunkStart = 0)
  {
    Formula resultFormula;
    List<FormulaElement> elementList = new List<FormulaElement>();
    resultFormula = new Formula();
    for (int i = chunkStart; i < stringChunks.Count; ++i)
    {
      string str = stringChunks[i];
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
      else if (ctx.ContainsVariableName(str))
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
          throw new Exception("What is this, " + str);
        }
      }
    }

    resultFormula.AddElementList(elementList);
    if (resultFormula.IsMismatched())
    {
      throw new Exception("Mismatched, " + resultFormula.ToLongString(ctx));
    }
    return resultFormula;
  }

}
