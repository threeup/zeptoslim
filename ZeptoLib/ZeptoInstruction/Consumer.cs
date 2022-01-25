
using ZeptoFormula;
using ZeptoCommon;
using ZeptoBehave;

namespace ZeptoInstruction;

public class Consumer
{
  private List<Instruction> instrList = new List<Instruction>();
  public Context ctx = new Context();
  public void SetContext(string varContents, string verbContents)
  {
    List<string> varChunks = Parser.CommaSeparatedIntoChunks(varContents);
    ctx.AddVariableNameList(varChunks);
    List<string> verbChunks = Parser.CommaSeparatedIntoChunks(verbContents);
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
  public void AddInstructions(List<Instruction> instrList)
  {
    this.instrList.AddRange(instrList);
  }

  public void Do()
  {
    for (int i = 0; i < instrList.Count;)
    {
      Instruction instr = instrList[i];
      i += 1;
      Expression? expr = instr.expression;
      if (expr != null)
      {
        int val = expr.Calculate(ctx, ctx);
        bool jumpCondition = instr.condition == Condition.IF || instr.condition == Condition.ELSEIF;
        if (jumpCondition && val <= 0)
        {
          i = JumpOut(ref instrList, instr.depth, i);
        }
        else
        {
          i = JumpNext(ref instrList, instr.depth, i);
        }
      }
    }
  }

  public static int JumpOut(ref List<Instruction> instrList, int depth, int start)
  {
    // if false
    //   foo
    // else
    //   bar

    // something
    // if blah
    //   if foo
    //      bar
    //   something
    for (int i = start; i < instrList.Count; ++i)
    {
      Instruction instr = instrList[i];
      if(instr.depth <= depth)
      {
        return i;
      }
    }
    return instrList.Count;
  }
  public static int JumpNext(ref List<Instruction> instrList, int depth, int start)
  {
    Instruction instr = instrList[start];
    bool isFalseBranch = instr.condition == Condition.ELSE || instr.condition == Condition.ELSEIF;
    if(isFalseBranch)
    {
      return JumpOut(ref instrList, depth, start);
    }
    return start;
  }
}
