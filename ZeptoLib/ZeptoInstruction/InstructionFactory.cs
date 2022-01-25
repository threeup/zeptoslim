
using ZeptoFormula;
using ZeptoCommon;
using ZeptoBehave;

namespace ZeptoInstruction;


public static class InstructionFactory
{

  public static IInstructionContext MakeContext(string varContents, string verbContents, string[] contents)
  {
    Context ctx = new Context();
    List<string> varChunks = Parser.CommaSeparatedIntoChunks(varContents);
    ctx.AddVariableNameList(varChunks);
    List<string> verbChunks = Parser.CommaSeparatedIntoChunks(verbContents);
    ctx.AddMethodNameList(verbChunks);

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
        InstructionFactory.Make(instr, fctx, ctx, buffer);
        list.Add(instr);
      }
    }
  }


  public static void Make(Instruction instr, IFormulaContext fctx, IInstructionContext ictx,
      List<string> stringChunks)
  {
    if (stringChunks.Count > 0)
    {
      string first = stringChunks[0];
      int chunkStart = 0;
      if (LogicConsts.ConditionStrings.ContainsKey(first))
      {
        instr.condition = ZeptoInstruction.LogicConsts.ConditionStrings[first];
        chunkStart = 1;
      }
      instr.expression = MakeExecution(fctx, ictx, stringChunks, chunkStart);
    }
  }


  public static Expression MakeExecution(IFormulaContext fctx, IInstructionContext ictx, List<string> stringChunks, int chunkStart = 0)
  {
    Expression resultExecution = new Expression();
    List<ExpressionElement> elementList = new List<ExpressionElement>();

    for (int i = chunkStart; i < stringChunks.Count; ++i)
    {
      string str = stringChunks[i];
      str = str.ToUpper();
      if (RPNConsts.AssignStrings.ContainsKey(str))
      {
        FormulaElementType etype = RPNConsts.AssignStrings[str];
        elementList.Add(new ExpressionElement(etype));
      }
      else if (RPNConsts.OperatorStrings.ContainsKey(str))
      {
        FormulaElementType etype = RPNConsts.OperatorStrings[str];
        elementList.Add(new ExpressionElement(etype));
      }
      else if (fctx.ContainsVariableName(str))
      {
        int varIndex = fctx.GetVariableIndex(str);
        int registerPlace = resultExecution.AddVarBinding(varIndex);
        FormulaElementType elType = Formula.RegisterFromInt(registerPlace);
        elementList.Add(new ExpressionElement(elType));
      }
      else if (ictx.ContainsMethodName(str))
      {
        elementList.Add(new ExpressionElement(str));
      }
      else
      {
        int number = 0;
        bool success = int.TryParse(str, out number);
        if (success)
        {
          elementList.Add(new ExpressionElement(number));
        }
        else
        {
          throw new Exception("What is this, " + str);
        }
      }
    }

    resultExecution.AddElementList(elementList);
    if (resultExecution.IsMismatched())
    {
      throw new Exception("Mismatched, " + resultExecution.ToLongString(fctx, ictx));
    }
    return resultExecution;
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
}

