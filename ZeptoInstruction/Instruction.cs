
using System.Collections.Generic;

namespace ZeptoInstruction
{
    public class Instruction
    {
        public Condition condition;
        public string verb;
        public int chunkIndex = 0;

        private List<Instruction> subInstructions = null;
        private ZeptoFormula.Formula formula = null;

        public Instruction(int chunkIndex)
        {
            condition = Condition.NONE;
            verb = "";
            this.chunkIndex = chunkIndex;
        }

        public Instruction(Condition condition, int chunkIndex)
        {
            this.condition = condition;
            verb = "";
            this.chunkIndex = chunkIndex;
        }
        public Instruction(string verb, int chunkIndex)
        {
            condition = Condition.NONE;
            this.verb = verb;
            this.chunkIndex = chunkIndex;
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
    }
}