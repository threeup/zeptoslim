using System.Collections.Generic;

namespace zeptolib
{
    public enum ZeptoComp
    {
        NONE,
        LESS_THAN,
        LESS_THAN_EQUAL,
        GREATER_THAN,
        GREATER_THAN_EQUAL,
        EQUAL,
    }
    public enum ZeptoGate
    {
        NONE,
        AND,
        OR,
    }
    public class Condition
    {
        public List<Condition> subConditions = new List<Condition>();
        public ZeptoGate gate = ZeptoGate.NONE;
        private string attrib;
        private ZeptoComp comp;
        private int val;


        public void Setup()
        {
            subConditions.Clear();
        }
        public void AddSubCondition(Condition next)
        {
            subConditions.Add(next);
        }

        public void SetAttribComparer(string attrib, ZeptoComp comp, int val)
        {
            this.attrib = attrib;
            this.comp = comp;
            this.val = val;
        }

         public bool PawnEvaluate(Pawn obj)
        {
            bool evaluation = false;
            if(gate == ZeptoGate.AND)
            {
                evaluation = true;
                foreach(Condition con in subConditions)
                {
                    evaluation &= con.PawnEvaluate(obj);
                }
            }
            else if (gate == ZeptoGate.OR)
            {
                evaluation = false;
                foreach(Condition con in subConditions)
                {
                    evaluation &= con.PawnEvaluate(obj);
                }
            }
            else
            {
                int objval = obj.GetAttrib(this.attrib);
                switch(this.comp)
                {
                    case ZeptoComp.LESS_THAN:
                        evaluation = val < objval;
                        break;
                    case ZeptoComp.LESS_THAN_EQUAL:
                        evaluation = val <= objval;
                        break;
                    case ZeptoComp.GREATER_THAN:
                        evaluation = val > objval;
                        break;
                    case ZeptoComp.GREATER_THAN_EQUAL:
                        evaluation = val >= objval;
                        break;
                    case ZeptoComp.EQUAL:
                        evaluation = val == objval;
                        break;
                    default:
                        break;
                }
            }
            return evaluation;            
        }
    }
}