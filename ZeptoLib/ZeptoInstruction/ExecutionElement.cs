using ZeptoFormula;
namespace ZeptoInstruction;



public struct ExecutionElement
{
  public FormulaElementType? formulaElementType;
  public string? verbName;

  public int val;

  public ExecutionElement(FormulaElementType elementType)
  {
    this.formulaElementType = elementType;
    this.verbName = null;
    this.val = 0;
  }
  public ExecutionElement(string verbName)
  {
    this.formulaElementType = null;
    this.verbName = verbName;
    this.val = 0;
  }
  public ExecutionElement(int val)
  {
    this.formulaElementType = FormulaElementType.CONST;
    this.verbName = null;
    this.val = val;
  }
  public static ExecutionElement BlankElement = new ExecutionElement(FormulaElementType.NONE);

  public override string ToString()
  {
    if (verbName != null)
    {
      return verbName;
    }
    FormulaElementType token = formulaElementType ?? FormulaElementType.NONE;
    if (val != 0 || token == FormulaElementType.CONST)
    {
      return token.ToString() + "=" + val;
    }
    else
    {
      return token.ToString();
    }
  }
}
