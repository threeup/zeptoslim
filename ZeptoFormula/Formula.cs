using System.Collections.Generic;

namespace ZeptoFormula
{
    public class Formula
    {

        public bool isConstFormula;
        public List<int> bindings;

        private List<Element> rpnFormula = new List<Element>();

        Stack<Element> stackBuffer = new Stack<Element>();


        public Formula()
        {
            isConstFormula = true;
        }

        public void AddElementList(List<Element> elements)
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
                ElementType token = rpnFormula[i].elementType;
                if (token == ElementType.LB || token == ElementType.RB)
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

        public static ElementType RegisterFromInt(int i)
        {
            return (ElementType)(i + (int)ElementType.REG1);
        }

        public static int RegisterToInt(ElementType elementType)
        {
            return (int)elementType - (int)ElementType.REG1;
        }


        public int Calculate(Context ctx)
        {

            return EvaluateRPN(ctx);
        }

        private int GetVal(Context ctx, Element el)
        {
            if(el.elementType == ElementType.CONST)
            {
                return el.val;
            }
            return ctx.GetVariableValue(RegisterToInt(el.elementType));
        }


        /// <summary>
        /// Method use to calculate the value of the formula.
        /// </summary>
        private int EvaluateRPN(Context ctx)
        {
            stackBuffer.Clear();
            Element element = Element.BlankElement;
            for (int i = 0; i < rpnFormula.Count; ++i)
            {
                element = rpnFormula[i];
                ElementType elementType = element.elementType;
                if (RPN.IsAssignType(elementType))
                {
                    if (stackBuffer.Count >= 2)
                    {
                        Element elOne = stackBuffer.Pop();
                        Element elTwo = stackBuffer.Pop();
                        int result = ctx.DoAssign(elementType, RegisterToInt(elTwo.elementType), elOne.val);
                        stackBuffer.Push(new Element(result));
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
                        
                        Element elOne = stackBuffer.Pop();
                        Element elTwo = stackBuffer.Pop();
                        int valOne = GetVal(ctx, elOne);
                        int valTwo = GetVal(ctx, elTwo);
                        int result = RPN.DoOperation(valTwo, valOne, elementType);
                        stackBuffer.Push(new Element(result));
                    }
                    else
                    {
                        throw new System.Exception("Incorrect number of values");
                    }
                }
                else if (RPN.IsRegisterType(elementType))
                {
                    stackBuffer.Push(new Element(elementType));
                }
                else
                {
                    stackBuffer.Push(new Element(element.val));
                }
            }

            if (stackBuffer.Count != 1)
                throw new System.Exception("Cannot calculate formula." + stackBuffer.Count + " of " + rpnFormula.Count + " " + ToLongString(ctx));

            Element last = stackBuffer.Pop();
            return last.val;
        }



        public string ToLongString(Context ctx)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder(isConstFormula ? "CONST: " : "RPN: ");
            Element element = Element.BlankElement;
            for (int i = 0; i < rpnFormula.Count; ++i)
            {
                element = rpnFormula[i];
                sb.Append(element);
                if (RPN.IsRegisterType(element.elementType))
                {
                    int val = ctx.GetVariableValue(RegisterToInt(element.elementType));
                    sb.Append('=');
                    sb.Append(val);
                }
                sb.Append(" ");
            }
            return sb.ToString();
        }
    }
}