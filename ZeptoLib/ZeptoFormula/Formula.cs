
namespace ZeptoFormula;

public class Formula
{

  public bool isConstFormula;
  public List<int> bindings = new List<int>();

  private List<FormulaElement> rpnFormula = new List<FormulaElement>();

  Stack<FormulaElement> stackBuffer = new Stack<FormulaElement>();


  public Formula()
  {
    isConstFormula = true;
  }

  public void AddElementList(List<FormulaElement> elements)
  {
    stackBuffer.Clear();
    rpnFormula.Clear();
    RPN.InfixToRPN(elements, ref stackBuffer, ref rpnFormula, out isConstFormula);
  }

  public int AddBinding(int varIndex)
  {
    if (bindings == null)
    {
      bindings = new List<int>();
    }
    else
    {
      int found = bindings.IndexOf(varIndex);
      if (found >= 0)
      {
        return found;
      }
    }
    int registerPlace = bindings.Count;
    bindings.Add(varIndex);
    return registerPlace;
  }


  public bool IsMismatched()
  {
    if (isConstFormula)
    {
      return false;
    }
    int assignCount = 0;
    int operatorCount = 0;
    int registerCount = 0;
    int elementCount = 0;
    for (int i = 0; i < rpnFormula.Count; ++i)
    {
      FormulaElementType token = rpnFormula[i].elementType;
      if (token == FormulaElementType.LB || token == FormulaElementType.RB)
      {
        //dontcare
      }
      else if (RPN.IsAssignType(token))
      {
        assignCount++;
      }
      else if (RPN.IsOperatorType(token))
      {
        operatorCount++;
      }
      else if (RPN.IsRegisterType(token))
      {
        registerCount++;
      }
      else
      {
        elementCount++;
      }
    }
    int shouldbeZero = 1 + assignCount + operatorCount - registerCount - elementCount;
    return shouldbeZero != 0;
  }

  public static FormulaElementType RegisterFromInt(int i)
  {
    return (FormulaElementType)(i + (int)FormulaElementType.REG1);
  }

  public static int RegisterToInt(FormulaElementType elementType)
  {
    return (int)elementType - (int)FormulaElementType.REG1;
  }


  private int GetVal(IFormulaContext ctx, FormulaElement el)
  {
    if (el.elementType == FormulaElementType.CONST)
    {
      return el.val;
    }
    int boundIndex = bindings[RegisterToInt(el.elementType)];
    return ctx.GetVariableValue(boundIndex);
  }


  /// <summary>
  /// Method use to calculate the value of the formula.
  /// </summary>
  public int Calculate(IFormulaContext ctx)
  {
    if (rpnFormula.Count == 0)
    {
      return 0;
    }
    stackBuffer.Clear();
    FormulaElement element = FormulaElement.BlankElement;
    for (int i = 0; i < rpnFormula.Count; ++i)
    {
      element = rpnFormula[i];
      FormulaElementType elementType = element.elementType;
      if (RPN.IsAssignType(elementType))
      {
        if (stackBuffer.Count >= 2)
        {
          FormulaElement elOne = stackBuffer.Pop();
          FormulaElement elTwo = stackBuffer.Pop();
          int boundIndex = bindings[RegisterToInt(elTwo.elementType)];
          int result = ctx.DoAssign(elementType, boundIndex, elOne.val);
          stackBuffer.Push(new FormulaElement(result));
        }
        else
        {
          throw new System.Exception("Incorrect number of values");
        }
      }
      else if (RPN.IsOperatorType(elementType))
      {
        if (stackBuffer.Count >= 2)
        {

          FormulaElement elOne = stackBuffer.Pop();
          FormulaElement elTwo = stackBuffer.Pop();
          int valOne = GetVal(ctx, elOne);
          int valTwo = GetVal(ctx, elTwo);
          int result = RPN.DoOperation(elementType, valTwo, valOne);
          stackBuffer.Push(new FormulaElement(result));
        }
        else
        {
          throw new System.Exception("Incorrect number of values");
        }
      }
      else if (RPN.IsRegisterType(elementType))
      {
        stackBuffer.Push(new FormulaElement(elementType));
      }
      else
      {
        stackBuffer.Push(new FormulaElement(element.val));
      }
    }

    if (stackBuffer.Count != 1)
      throw new System.Exception("Cannot calculate formula." + stackBuffer.Count + " of " + rpnFormula.Count + " " + ToLongString(ctx));

    FormulaElement last = stackBuffer.Pop();
    return last.val;
  }



  public string ToLongString(IFormulaContext ctx)
  {
    System.Text.StringBuilder sb = new System.Text.StringBuilder(isConstFormula ? "CONST: " : "RPN: ");
    FormulaElement element = FormulaElement.BlankElement;
    for (int i = 0; i < rpnFormula.Count; ++i)
    {
      element = rpnFormula[i];
      sb.Append(element);
      if (RPN.IsRegisterType(element.elementType))
      {
        int val = GetVal(ctx, element);
        sb.Append('=');
        sb.Append(val);
      }
      sb.Append(" ");
    }
    return sb.ToString();
  }
}
