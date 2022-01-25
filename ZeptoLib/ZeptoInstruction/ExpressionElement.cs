using ZeptoFormula;

namespace ZeptoInstruction;

public struct ExpressionElement
{
  public FormulaElementType? formulaElementType;
  public string? methodName;

  public int val;

  public ExpressionElement(FormulaElementType elementType)
  {
    this.formulaElementType = elementType;
    this.methodName = null;
    this.val = 0;
  }
  public ExpressionElement(string verbName)
  {
    this.formulaElementType = null;
    this.methodName = verbName;
    this.val = 0;
  }
  public ExpressionElement(int val)
  {
    this.formulaElementType = FormulaElementType.CONST;
    this.methodName = null;
    this.val = val;
  }
  public static ExpressionElement BlankElement = new ExpressionElement(FormulaElementType.NONE);

  public override string ToString()
  {
    if (methodName != null)
    {
      return methodName;
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
