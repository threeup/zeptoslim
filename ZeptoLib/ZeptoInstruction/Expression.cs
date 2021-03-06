
using ZeptoFormula;

namespace ZeptoInstruction;

public class Expression
{

  public bool isConst = true;
  public List<int>? varBindings = null;
  public List<int>? methodBindings = null;

  private List<ExpressionElement> rpnFormula = new List<ExpressionElement>();

  Stack<ExpressionElement> stackBuffer = new Stack<ExpressionElement>();


  public Expression()
  {
    isConst = true;
  }

  public void AddElementList(List<ExpressionElement> elements)
  {
    stackBuffer.Clear();
    rpnFormula.Clear();
    ExpressionRPN.InfixToRPN(elements, ref stackBuffer, ref rpnFormula, out isConst);
  }

  public int AddVarBinding(int varIndex)
  {
    if (varBindings == null)
    {
      varBindings = new List<int>();
    }
    else
    {
      int found = varBindings.IndexOf(varIndex);
      if (found >= 0)
      {
        return found;
      }
    }
    int registerPlace = varBindings.Count;
    varBindings.Add(varIndex);
    return registerPlace;
  }


  public bool IsMismatched()
  {
    if (isConst)
    {
      return false;
    }
    int assignCount = 0;
    int operatorCount = 0;
    int registerCount = 0;
    int elementCount = 0;
    for (int i = 0; i < rpnFormula.Count; ++i)
    {
      if (rpnFormula[i].formulaElementType.HasValue)
      {
        FormulaElementType token = rpnFormula[i].formulaElementType ?? FormulaElementType.CONST;
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

    }
    int shouldbeZero = 1 + assignCount + operatorCount - registerCount - elementCount;
    return shouldbeZero != 0;
  }

  public static FormulaElementType VarRegisterFromInt(int i)
  {
    return (FormulaElementType)(i + (int)FormulaElementType.REG1);
  }

  public static int VarRegisterToInt(FormulaElementType elementType)
  {
    return (int)elementType - (int)FormulaElementType.REG1;
  }


  private int GetVal(IFormulaContext fctx, FormulaElementType? token, int val)
  {
    if (token == FormulaElementType.CONST)
    {
      return val;
    }
    if(varBindings != null && token.HasValue)
    {
      int boundIndex = varBindings[VarRegisterToInt(token.GetValueOrDefault())];
      return fctx.GetVariableValue(boundIndex);
    }
    return 0;
  }


  /// <summary>
  /// Method use to calculate the value of the formula.
  /// </summary>
  public int Calculate(IFormulaContext fctx, IInstructionContext ictx)
  {
    if (rpnFormula.Count == 0)
    {
      return 0;
    }
    stackBuffer.Clear();
    ExpressionElement el = ExpressionElement.BlankElement;
    for (int i = 0; i < rpnFormula.Count; ++i)
    {
      el = rpnFormula[i];
      //Console.WriteLine("  "+el.ToString());
      if(el.methodName != null)
      {
        if (stackBuffer.Count >= 1)
        {
          ExpressionElement elOne = stackBuffer.Pop();
          int valOne = GetVal(fctx, elOne.formulaElementType, elOne.val);
          var func = ictx.GetMethodPtr(el.methodName);
          int result = func(valOne);
          stackBuffer.Push(new ExpressionElement(result));
        }
        else
        {
          throw new System.Exception("Incorrect number of values");
        }
      }
      else if(el.formulaElementType.HasValue)
      {
        FormulaElementType elementType = el.formulaElementType.GetValueOrDefault();
        if (RPN.IsAssignType(elementType))
        {
          if (stackBuffer.Count >= 2 && varBindings != null)
          {
            ExpressionElement elOne = stackBuffer.Pop();
            ExpressionElement elTwo = stackBuffer.Pop();
            int boundIndex = varBindings[VarRegisterToInt(elTwo.formulaElementType.GetValueOrDefault())];
            int result = fctx.DoAssign(elementType, boundIndex, elOne.val);
            stackBuffer.Push(new ExpressionElement(result));
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
            ExpressionElement elOne = stackBuffer.Pop();
            ExpressionElement elTwo = stackBuffer.Pop();
            int valOne = GetVal(fctx, elOne.formulaElementType, elOne.val);
            int valTwo = GetVal(fctx, elTwo.formulaElementType, elTwo.val);
            int result = RPN.DoOperation(elementType, valTwo, valOne);
            stackBuffer.Push(new ExpressionElement(result));
          }
          else
          {
            throw new System.Exception("Incorrect number of values");
          }
        }
        else if (RPN.IsRegisterType(elementType))
        {
          stackBuffer.Push(new ExpressionElement(elementType));
        }
        else
        {
          stackBuffer.Push(new ExpressionElement(el.val));
        }
      }
    }

    if (stackBuffer.Count != 1)
      throw new System.Exception("Cannot calculate formula." + stackBuffer.Count + " of " + rpnFormula.Count + " " + ToLongString(fctx, ictx));

    ExpressionElement last = stackBuffer.Pop();
    return last.val;
  }



  public string ToLongString(IFormulaContext fctx, IInstructionContext ictx)
  {
    System.Text.StringBuilder sb = new System.Text.StringBuilder(isConst ? "CONST: " : "RPN: ");
    ExpressionElement element = ExpressionElement.BlankElement;
    for (int i = 0; i < rpnFormula.Count; ++i)
    {
      element = rpnFormula[i];
      sb.Append(element);
      FormulaElementType token = element.formulaElementType.GetValueOrDefault();
      if (ExpressionRPN.IsRegisterType(token))
      {
        int val = GetVal(fctx, token, element.val);
        sb.Append('=');
        sb.Append(val);
      }
      sb.Append(" ");
    }
    return sb.ToString();
  }
}
