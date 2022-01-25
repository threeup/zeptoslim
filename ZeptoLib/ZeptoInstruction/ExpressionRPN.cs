
using ZeptoFormula;

namespace ZeptoInstruction;

public static class ExpressionRPN
{

  public static void InfixToRPN(List<ExpressionElement> input, ref Stack<ExpressionElement> stack, ref List<ExpressionElement> outList, out bool isConst)
  {

    isConst = true;
    for (int i = 0; i < input.Count; ++i)
    {
      ExpressionElement element = input[i];
      FormulaElementType? token = element.formulaElementType;

      if (CheckPrecendence(element))
      {
        while (stack.Count != 0 && CheckPrecendence(stack.Peek()))
        {
          if (ShouldPopStack(element, stack.Peek()))
          {
            outList.Add(stack.Pop());
            continue;
          }
          break;
        }
        stack.Push(element);
      }
      else if (token == FormulaElementType.LB)
      {
        stack.Push(element);
      }
      else if (token == FormulaElementType.RB)
      {
        while (stack.Count != 0 && stack.Peek().formulaElementType != FormulaElementType.LB)
        {
          outList.Add(stack.Pop());
        }
        stack.Pop();
      }
      else
      {
        if (isConst && IsRegisterType(token ?? FormulaElementType.NONE))
        {
          isConst = false;
        }
        outList.Add(element);
      }
    }

    while (stack.Count != 0)
      outList.Add(stack.Pop());

  }

  public static bool IsAssignType(FormulaElementType elementType)
  {
    return elementType >= FormulaElementType.SET && elementType <= FormulaElementType.DECREMENT;
  }
  public static bool IsOperatorType(FormulaElementType elementType)
  {
    return elementType >= FormulaElementType.ADD && elementType <= FormulaElementType.BITWISEEXCLUDEANY;
  }

  public static bool CheckPrecendence(ExpressionElement element)
  {
    string? method = element.methodName;
    if (method != null)
    {
      return true;
    }
    FormulaElementType token = element.formulaElementType ?? FormulaElementType.NONE;
    return IsAssignType(token) || IsOperatorType(token);
  }

  public static bool IsRegisterType(FormulaElementType elementType)
  {
    return elementType >= FormulaElementType.REG1 && elementType <= FormulaElementType.REG9;
  }

  public static bool RequiresBitWise(FormulaElementType elementType)
  {
    return elementType >= FormulaElementType.BITWISEMATCHALL && elementType <= FormulaElementType.BITWISEEXCLUDEANY;
  }

  private static bool IsAssociative(FormulaElementType token, int type)
  {
    if (RPNConsts.Precedence[token][1] == type)
      return true;

    return false;
  }


  public static bool ShouldPopStack(ExpressionElement current, ExpressionElement peek)
  {
    int priCurrent = -10;
    int priPeek = -10;
    if (current.formulaElementType.HasValue)
    {
      FormulaElementType token = current.formulaElementType.GetValueOrDefault();
      priCurrent = RPNConsts.Precedence[token][0];
    }
    else if(current.methodName != null)
    {
      priCurrent = 20;
    }
    
    if (peek.formulaElementType.HasValue)
    {
      FormulaElementType peekToken = peek.formulaElementType.GetValueOrDefault();
      priPeek = RPNConsts.Precedence[peekToken][0];
    }
    else if(peek.methodName != null)
    {
      priPeek = 20;
    }
    return (priCurrent <= priPeek);
  }

}
