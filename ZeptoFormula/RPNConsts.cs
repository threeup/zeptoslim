using System.Collections.Generic;

namespace ZeptoFormula
{

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


        public static readonly Dictionary<string, FormulaElementType> AssignStrings = new Dictionary<string, FormulaElementType> {
            {"SET",FormulaElementType.SET},
            {"=",FormulaElementType.SET},
            {"+=",FormulaElementType.INCREMENT},
            {"-=",FormulaElementType.DECREMENT},
        };

        public static readonly Dictionary<string, FormulaElementType> OperatorStrings = new Dictionary<string, FormulaElementType> {
            {"(",FormulaElementType.LB},
            {")",FormulaElementType.RB},
            {"+",FormulaElementType.ADD},
            {"-",FormulaElementType.SUBTRACT},
            {"*",FormulaElementType.MULTIPLY},
            {"/",FormulaElementType.DIVIDE},
            {"%",FormulaElementType.MODULO},
            {"MIN",FormulaElementType.MIN},
            {"MAX",FormulaElementType.MAX},
            {"==",FormulaElementType.EQUALTO},
            {"!=",FormulaElementType.NOTEQUALTO},
            {">",FormulaElementType.GREATERTHAN},
            {">=",FormulaElementType.GREATERTHANEQUALTO},
            {"<",FormulaElementType.LESSTHAN},
            {"<=",FormulaElementType.LESSTHANEQUALTO},
            {"AND",FormulaElementType.LOGICALAND},
            {"&&",FormulaElementType.LOGICALAND},
            {"OR",FormulaElementType.LOGICALOR},
            {"||",FormulaElementType.LOGICALOR},
            {"&",FormulaElementType.BITWISEAND},
            {",",FormulaElementType.BITWISEOR},
            {"|",FormulaElementType.BITWISEOR},
            {"$",FormulaElementType.BITWISEMATCHALL},
            {"MATCH",FormulaElementType.BITWISEMATCHALL},
            {"ALL",FormulaElementType.BITWISEMATCHALL},
            {"#",FormulaElementType.BITWISEMATCHANY},
            {"ANY",FormulaElementType.BITWISEMATCHANY},
            {"~",FormulaElementType.BITWISEEXCLUDEANY},
            {"NOT",FormulaElementType.BITWISEEXCLUDEANY},
        };

        public static readonly Dictionary<FormulaElementType, int[]> Precedence = new Dictionary<FormulaElementType, int[]> {


            {FormulaElementType.SET, new int[] { -2, LEFT_ASSOC }},
            {FormulaElementType.INCREMENT, new int[] { -2, LEFT_ASSOC }},
            {FormulaElementType.DECREMENT, new int[] { -2, LEFT_ASSOC }},

            {FormulaElementType.ADD, new int[] { 8, LEFT_ASSOC }},
            {FormulaElementType.SUBTRACT, new int[] { 8, LEFT_ASSOC }},
            {FormulaElementType.MULTIPLY, new int[] { 9, LEFT_ASSOC }},
            {FormulaElementType.DIVIDE, new int[] { 9, LEFT_ASSOC }},
            {FormulaElementType.MODULO, new int[] { 9, LEFT_ASSOC }},
            {FormulaElementType.MIN, new int[] { -1, LEFT_ASSOC }},
            {FormulaElementType.MAX, new int[] { -1, LEFT_ASSOC }},
            {FormulaElementType.EQUALTO, new int[] { 5, LEFT_ASSOC }},
            {FormulaElementType.NOTEQUALTO, new int[] { 5, LEFT_ASSOC }},
            {FormulaElementType.GREATERTHAN, new int[] { 6, LEFT_ASSOC }},
            {FormulaElementType.GREATERTHANEQUALTO, new int[] { 6, LEFT_ASSOC }},
            {FormulaElementType.LESSTHAN, new int[] { 6, LEFT_ASSOC }},
            {FormulaElementType.LESSTHANEQUALTO, new int[] { 6, LEFT_ASSOC }},

            {FormulaElementType.LOGICALAND, new int[] { 1, LEFT_ASSOC }},
            {FormulaElementType.LOGICALOR, new int[] { 0, LEFT_ASSOC }},
            {FormulaElementType.BITWISEAND, new int[] { 4, LEFT_ASSOC }},
            {FormulaElementType.BITWISEOR, new int[] { 2, LEFT_ASSOC }},
            {FormulaElementType.BITWISEMATCHALL, new int[] { 7, LEFT_ASSOC }},
            {FormulaElementType.BITWISEMATCHANY, new int[] { 7, LEFT_ASSOC }},
            {FormulaElementType.BITWISEEXCLUDEANY, new int[] { 7, LEFT_ASSOC }},
        };
    }
}