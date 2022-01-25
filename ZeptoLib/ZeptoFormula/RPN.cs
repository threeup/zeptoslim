
namespace ZeptoFormula;

public static class RPN
{

  public static void InfixToRPN(List<FormulaElement> input, ref Stack<FormulaElement> stack, ref List<FormulaElement> outList, out bool isConstFormula)
  {
    for (int i = 0; i < input.Count; i++)
    {
      FormulaElementType inputType = input[i].elementType;
      if (HasPrecendence(inputType) || inputType == FormulaElementType.LB || inputType == FormulaElementType.RB)
      {
        if (i != 0 && input[i - 1].elementType != FormulaElementType.NONE)
          input.Insert(i, FormulaElement.BlankElement);
        if (i != input.Count - 1 && input[i + 1].elementType != FormulaElementType.NONE)
          input.Insert(i + 1, FormulaElement.BlankElement);
      }
    }

    isConstFormula = true;
    for (int i = 0; i < input.Count; ++i)
    {
      FormulaElement element = input[i];
      FormulaElementType token = element.elementType;
      if (token == FormulaElementType.NONE)
        continue;

      if (HasPrecendence(token))
      {
        while (stack.Count != 0 && HasPrecendence(stack.Peek().elementType))
        {
          if (ShouldPopStack(token, stack.Peek()))
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
        while (stack.Count != 0 && stack.Peek().elementType != FormulaElementType.LB)
        {
          outList.Add(stack.Pop());
        }
        stack.Pop();
      }
      else
      {
        if (isConstFormula && IsRegisterType(token))
        {
          isConstFormula = false;
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

  public static bool HasPrecendence(FormulaElementType elementType)
  {
    return IsAssignType(elementType) || IsOperatorType(elementType);
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
    if (!HasPrecendence(token))
      throw new System.ArgumentException("Invalid token: " + token);

    if (RPNConsts.Precedence[token][1] == type)
      return true;

    return false;
  }

  private static int ComparePrecedence(FormulaElementType token1, FormulaElementType token2)
  {
    if (!HasPrecendence(token1) || !HasPrecendence(token2))
      throw new System.ArgumentException("Invalid token: " + token1 + " " + token2);

    return RPNConsts.Precedence[token1][0] - RPNConsts.Precedence[token2][0];
  }

  public static bool ShouldPopStack(FormulaElementType token, FormulaElement peek)
  {
    if (IsAssociative(token, RPNConsts.LEFT_ASSOC) && ComparePrecedence(token, peek.elementType) <= 0)
    {
      return true;
    }
    if (IsAssociative(token, RPNConsts.RIGHT_ASSOC) && ComparePrecedence(token, peek.elementType) < 0)
    {
      return true;
    }
    return false;

  }

  public static int DoOperation(FormulaElementType op, int val1, int val2)
  {
    switch (op)
    {
      case FormulaElementType.ADD:
        return val1 + val2;
      case FormulaElementType.SUBTRACT:
        return val1 - val2;
      case FormulaElementType.MULTIPLY:
        return val1 * val2;
      case FormulaElementType.DIVIDE:
        return val1 / val2;
      case FormulaElementType.MODULO:
        return val1 % val2;
      case FormulaElementType.MIN:
        return (val1 < val2) ? val1 : val2;
      case FormulaElementType.MAX:
        return (val1 > val2) ? val1 : val2;
      case FormulaElementType.EQUALTO:
        return (val1 == val2) ? 1 : 0;
      case FormulaElementType.NOTEQUALTO:
        return (val1 != val2) ? 1 : 0;
      case FormulaElementType.GREATERTHAN:
        return (val1 > val2) ? 1 : 0;
      case FormulaElementType.GREATERTHANEQUALTO:
        return (val1 >= val2) ? 1 : 0;
      case FormulaElementType.LESSTHAN:
        return (val1 < val2) ? 1 : 0;
      case FormulaElementType.LESSTHANEQUALTO:
        return (val1 <= val2) ? 1 : 0;
      case FormulaElementType.LOGICALAND:
        return (val1 > 0 && val2 > 0) ? 1 : 0;
      case FormulaElementType.LOGICALOR:
        return (val1 > 0 || val2 > 0) ? 1 : 0;
      case FormulaElementType.BITWISEOR:
        return val1 | val2;
      case FormulaElementType.BITWISEAND:
        return val1 & val2;
      case FormulaElementType.BITWISEMATCHANY:
        return (val1 & val2) > 0 ? 1 : 0;
      case FormulaElementType.BITWISEMATCHALL:
        return (val1 & val2) == val2 ? 1 : 0;
      case FormulaElementType.BITWISEEXCLUDEANY:
        return (val1 & val2) == 0 ? 1 : 0;
      default:
        return 0;
    }
  }


  private static float DoFloatOperation(float val1, float val2, FormulaElementType op)
  {
    const float epsilon = 0.0001f;
    switch (op)
    {
      case FormulaElementType.ADD:
        return val1 + val2;
      case FormulaElementType.SUBTRACT:
        return val1 - val2;
      case FormulaElementType.MULTIPLY:
        return val1 * val2;
      case FormulaElementType.DIVIDE:
        return val1 / val2;
      case FormulaElementType.MODULO:
        return (int)val1 % (int)val2;
      case FormulaElementType.MIN:
        return (val1 < val2) ? val1 : val2;
      case FormulaElementType.MAX:
        return (val1 > val2) ? val1 : val2;
      case FormulaElementType.EQUALTO:
        return System.Math.Abs(val1 - val2) < epsilon ? 1.0f : 0.0f;
      case FormulaElementType.NOTEQUALTO:
        return System.Math.Abs(val1 - val2) > epsilon ? 1.0f : 0.0f;
      case FormulaElementType.GREATERTHAN:
        return val1 > (val2 + epsilon) ? 1.0f : 0.0f;
      case FormulaElementType.GREATERTHANEQUALTO:
        return val1 > (val2 - epsilon) ? 1.0f : 0.0f;
      case FormulaElementType.LESSTHAN:
        return val1 < (val2 - epsilon) ? 1.0f : 0.0f;
      case FormulaElementType.LESSTHANEQUALTO:
        return val1 < (val2 + epsilon) ? 1.0f : 0.0f;
      case FormulaElementType.LOGICALAND:
        return (val1 > epsilon && val2 > epsilon) ? 1.0f : 0.0f;
      case FormulaElementType.LOGICALOR:
        return (val1 > epsilon || val2 > epsilon) ? 1.0f : 0.0f;
      default:
        return 0;
    }
  }

}
