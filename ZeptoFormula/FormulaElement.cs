
namespace ZeptoFormula
{
    public enum FormulaElementType
    {
        NONE,
        SET,
        INCREMENT,
        DECREMENT,
    
        CONST,
        LB,
        RB,

        ADD,
        SUBTRACT,
        MULTIPLY,
        DIVIDE,
        MODULO,
        MIN,
        MAX,

        EQUALTO,
        NOTEQUALTO,
        GREATERTHAN,
        GREATERTHANEQUALTO,
        LESSTHAN,
        LESSTHANEQUALTO,

        LOGICALAND,
        LOGICALOR,

        BITWISEAND,
        BITWISEOR,
        BITWISEMATCHALL,
        BITWISEMATCHANY,
        BITWISEEXCLUDEANY,

        REG1,
        REG2,
        REG3,
        REG4,
        REG5,
        REG6,
        REG7,
        REG8,
        REG9,
    }

    public struct FormulaElement
    {
        public FormulaElementType elementType;
        public int val;
        public FormulaElement(FormulaElementType elementType)
        {
            this.elementType = elementType;
            this.val = 0;
        }
        public FormulaElement(int val)
        {
            this.elementType = FormulaElementType.CONST;
            this.val = val;
        }
        public static FormulaElement BlankElement = new FormulaElement(FormulaElementType.NONE);

        public override string ToString()
        {
            if (val != 0 || elementType == FormulaElementType.CONST) 
            {
                return elementType.ToString()+"="+val;
            }
            else 
            {
                return elementType.ToString();
            }
        }
    }
}