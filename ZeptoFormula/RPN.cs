using System.Collections.Generic;

namespace ZeptoFormula
{
    public static class RPN
    {

        public static void InfixToRPN(List<Element> input, ref Stack<Element> stack, ref List<Element> outList, out bool isConstFormula)
        {
            for (int i = 0; i < input.Count; i++)
            {
                ElementType inputType = input[i].elementType;
                if (HasPrecendence(inputType) || inputType == ElementType.LB || inputType == ElementType.RB)
                {
                    if (i != 0 && input[i - 1].elementType != ElementType.NONE)
                        input.Insert(i, Element.BlankElement);
                    if (i != input.Count - 1 && input[i + 1].elementType != ElementType.NONE)
                        input.Insert(i + 1, Element.BlankElement);
                }
            }

            isConstFormula = true;
            for (int i = 0; i < input.Count; ++i)
            {
                Element element = input[i];
                ElementType token = element.elementType;
                if (token == ElementType.NONE)
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
                else if (token == ElementType.LB)
                {
                    stack.Push(element);
                }
                else if (token == ElementType.RB)
                {
                    while (stack.Count != 0 && stack.Peek().elementType != ElementType.LB)
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

        public static bool IsAssignType(ElementType elementType)
        {
            return elementType >= ElementType.SET && elementType <= ElementType.DECREMENT;
        }
        public static bool IsOperatorType(ElementType elementType)
        {
            return elementType >= ElementType.ADD && elementType <= ElementType.BITWISEEXCLUDEANY;
        }

        public static bool HasPrecendence(ElementType elementType)
        {
            return IsAssignType(elementType) || IsOperatorType(elementType);
        }

        public static bool IsRegisterType(ElementType elementType)
        {
            return elementType >= ElementType.REG1 && elementType <= ElementType.REG9;
        }

        public static bool RequiresBitWise(ElementType elementType)
        {
            return elementType >= ElementType.BITWISEMATCHALL && elementType <= ElementType.BITWISEEXCLUDEANY;
        }

        private static bool IsAssociative(ElementType token, int type)
        {
            if (!HasPrecendence(token))
                throw new System.ArgumentException("Invalid token: " + token);

            if (RPNConsts.Precedence[token][1] == type)
                return true;

            return false;
        }

        private static int ComparePrecedence(ElementType token1, ElementType token2)
        {
            if (!HasPrecendence(token1) || !HasPrecendence(token2))
                throw new System.ArgumentException("Invalid token: " + token1 + " " + token2);

            return RPNConsts.Precedence[token1][0] - RPNConsts.Precedence[token2][0];
        }

        public static bool ShouldPopStack(ElementType token, Element peek)
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

        public static int DoOperation(int val1, int val2, ElementType op)
        {
            switch (op)
            {
                case ElementType.ADD:
                    return val1 + val2;
                case ElementType.SUBTRACT:
                    return val1 - val2;
                case ElementType.MULTIPLY:
                    return val1 * val2;
                case ElementType.DIVIDE:
                    return val1 / val2;
                case ElementType.MODULO:
                    return val1 % val2;
                case ElementType.MIN:
                    return (val1 < val2) ? val1 : val2;
                case ElementType.MAX:
                    return (val1 > val2) ? val1 : val2;
                case ElementType.EQUALTO:
                    return (val1 == val2) ? 1 : 0;
                case ElementType.NOTEQUALTO:
                    return (val1 != val2) ? 1 : 0;
                case ElementType.GREATERTHAN:
                    return (val1 > val2) ? 1 : 0;
                case ElementType.GREATERTHANEQUALTO:
                    return (val1 >= val2) ? 1 : 0;
                case ElementType.LESSTHAN:
                    return (val1 < val2) ? 1 : 0;
                case ElementType.LESSTHANEQUALTO:
                    return (val1 <= val2) ? 1 : 0;
                case ElementType.LOGICALAND:
                    return (val1 > 0 && val2 > 0) ? 1 : 0;
                case ElementType.LOGICALOR:
                    return (val1 > 0 || val2 > 0) ? 1 : 0;
                case ElementType.BITWISEOR:
                    return val1 | val2;
                case ElementType.BITWISEAND:
                    return val1 & val2;
                case ElementType.BITWISEMATCHANY:
                    return (val1 & val2) > 0 ? 1 : 0;
                case ElementType.BITWISEMATCHALL:
                    return (val1 & val2) == val2 ? 1 : 0;
                case ElementType.BITWISEEXCLUDEANY:
                    return (val1 & val2) == 0 ? 1 : 0;
                default:
                    return 0;
            }
        }


        private static float DoFloatOperation(float val1, float val2, ElementType op)
        {
            const float epsilon = 0.0001f;
            switch (op)
            {
                case ElementType.ADD:
                    return val1 + val2;
                case ElementType.SUBTRACT:
                    return val1 - val2;
                case ElementType.MULTIPLY:
                    return val1 * val2;
                case ElementType.DIVIDE:
                    return val1 / val2;
                case ElementType.MODULO:
                    return (int)val1 % (int)val2;
                case ElementType.MIN:
                    return (val1 < val2) ? val1 : val2;
                case ElementType.MAX:
                    return (val1 > val2) ? val1 : val2;
                case ElementType.EQUALTO:
                    return System.Math.Abs(val1 - val2) < epsilon ? 1.0f : 0.0f;
                case ElementType.NOTEQUALTO:
                    return System.Math.Abs(val1 - val2) > epsilon ? 1.0f : 0.0f;
                case ElementType.GREATERTHAN:
                    return val1 > (val2 + epsilon) ? 1.0f : 0.0f;
                case ElementType.GREATERTHANEQUALTO:
                    return val1 > (val2 - epsilon) ? 1.0f : 0.0f;
                case ElementType.LESSTHAN:
                    return val1 < (val2 - epsilon) ? 1.0f : 0.0f;
                case ElementType.LESSTHANEQUALTO:
                    return val1 < (val2 + epsilon) ? 1.0f : 0.0f;
                case ElementType.LOGICALAND:
                    return (val1 > epsilon && val2 > epsilon) ? 1.0f : 0.0f;
                case ElementType.LOGICALOR:
                    return (val1 > epsilon || val2 > epsilon) ? 1.0f : 0.0f;
                default:
                    return 0;
            }
        }

    }
}