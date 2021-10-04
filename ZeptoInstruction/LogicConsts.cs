
using System.Collections.Generic;

namespace ZeptoInstruction
{


    public enum Condition
    {
        NONE,
        IF,
        ELSE,
        ELSEIF,
    }
    public static class LogicConsts
    {

        public static readonly Dictionary<string, Condition> ConditionStrings = new Dictionary<string, Condition> {
            {"IF",Condition.IF},
            {"?",Condition.IF},
            {"ELSE",Condition.ELSE},
            {":",Condition.ELSE},
            {"ELSEIF",Condition.ELSEIF},
            {":?",Condition.ELSEIF},
        };

    }

}