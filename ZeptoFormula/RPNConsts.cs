using System.Collections.Generic;

namespace ZeptoFormula
{

    public enum ElementType
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

    public static class RPNConsts
    {
        /// <summary>
        /// Constant for left association symbols
        /// </summary>
        public static readonly int LEFT_ASSOC = 0;

        /// <summary>
        /// Constant for right association symbols
        /// </summary>
        public static readonly int RIGHT_ASSOC = 1;


        public static readonly Dictionary<string, ElementType> AssignStrings = new Dictionary<string, ElementType> {
            {"SET",ElementType.SET},
            {"=",ElementType.SET},
            {"+=",ElementType.INCREMENT},
            {"-=",ElementType.DECREMENT},
        };

        public static readonly Dictionary<string, ElementType> OperatorStrings = new Dictionary<string, ElementType> {
            {"(",ElementType.LB},
            {")",ElementType.RB},
            {"+",ElementType.ADD},
            {"-",ElementType.SUBTRACT},
            {"*",ElementType.MULTIPLY},
            {"/",ElementType.DIVIDE},
            {"%",ElementType.MODULO},
            {"MIN",ElementType.MIN},
            {"MAX",ElementType.MAX},
            {"==",ElementType.EQUALTO},
            {"!=",ElementType.NOTEQUALTO},
            {">",ElementType.GREATERTHAN},
            {">=",ElementType.GREATERTHANEQUALTO},
            {"<",ElementType.LESSTHAN},
            {"<=",ElementType.LESSTHANEQUALTO},
            {"AND",ElementType.LOGICALAND},
            {"&&",ElementType.LOGICALAND},
            {"OR",ElementType.LOGICALOR},
            {"||",ElementType.LOGICALOR},
            {"&",ElementType.BITWISEAND},
            {",",ElementType.BITWISEOR},
            {"|",ElementType.BITWISEOR},
            {"$",ElementType.BITWISEMATCHALL},
            {"MATCH",ElementType.BITWISEMATCHALL},
            {"ALL",ElementType.BITWISEMATCHALL},
            {"#",ElementType.BITWISEMATCHANY},
            {"ANY",ElementType.BITWISEMATCHANY},
            {"~",ElementType.BITWISEEXCLUDEANY},
            {"NOT",ElementType.BITWISEEXCLUDEANY},
        };

        public static readonly Dictionary<ElementType, int[]> Precedence = new Dictionary<ElementType, int[]> {


            {ElementType.SET, new int[] { -2, LEFT_ASSOC }},
            {ElementType.INCREMENT, new int[] { -2, LEFT_ASSOC }},
            {ElementType.DECREMENT, new int[] { -2, LEFT_ASSOC }},

            {ElementType.ADD, new int[] { 8, LEFT_ASSOC }},
            {ElementType.SUBTRACT, new int[] { 8, LEFT_ASSOC }},
            {ElementType.MULTIPLY, new int[] { 9, LEFT_ASSOC }},
            {ElementType.DIVIDE, new int[] { 9, LEFT_ASSOC }},
            {ElementType.MODULO, new int[] { 9, LEFT_ASSOC }},
            {ElementType.MIN, new int[] { -1, LEFT_ASSOC }},
            {ElementType.MAX, new int[] { -1, LEFT_ASSOC }},
            {ElementType.EQUALTO, new int[] { 5, LEFT_ASSOC }},
            {ElementType.NOTEQUALTO, new int[] { 5, LEFT_ASSOC }},
            {ElementType.GREATERTHAN, new int[] { 6, LEFT_ASSOC }},
            {ElementType.GREATERTHANEQUALTO, new int[] { 6, LEFT_ASSOC }},
            {ElementType.LESSTHAN, new int[] { 6, LEFT_ASSOC }},
            {ElementType.LESSTHANEQUALTO, new int[] { 6, LEFT_ASSOC }},

            {ElementType.LOGICALAND, new int[] { 1, LEFT_ASSOC }},
            {ElementType.LOGICALOR, new int[] { 0, LEFT_ASSOC }},
            {ElementType.BITWISEAND, new int[] { 4, LEFT_ASSOC }},
            {ElementType.BITWISEOR, new int[] { 2, LEFT_ASSOC }},
            {ElementType.BITWISEMATCHALL, new int[] { 7, LEFT_ASSOC }},
            {ElementType.BITWISEMATCHANY, new int[] { 7, LEFT_ASSOC }},
            {ElementType.BITWISEEXCLUDEANY, new int[] { 7, LEFT_ASSOC }},
        };
    }
}