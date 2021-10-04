
using System.Collections.Generic;

namespace ZeptoInstruction
{
    public class Instruction
    {
        public int depth = 0;
        public Condition condition;
        public string verb;

        private List<Instruction> subInstructions = null;
        private ZeptoFormula.Formula formula = null;

        public Instruction(int depth)
        {
            this.condition = Condition.NONE;
            this.verb = "";
            this.depth = depth;
        }

        public Instruction(int depth, Condition condition)
        {
            this.condition = condition;
            this.verb = "";
            this.depth = depth;
        }
        public Instruction(int depth, string verb)
        {
            this.condition = Condition.NONE;
            this.verb = verb;
            this.depth = depth;
        }

        public void AddSubInstruction(Instruction sub)
        {
            if(subInstructions == null)
            {
                subInstructions = new List<Instruction>();
            }
            subInstructions.Add(sub);
        }

        public void AddFormula(ZeptoFormula.Formula formula)
        {
            this.formula = formula;
        }

        public string ToLongString()
        {
            int subCount = subInstructions != null ? subInstructions.Count: 0;
            return "Inst Depth:"+depth+" sub:"+subCount;
        }
    }
}