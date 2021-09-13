using System.Collections.Generic;

namespace zeptolib
{

    public enum Slot
    {
        One,
        Two,
        Three,
        Four,
        Five,
        Cycle,
    }

    public enum ZeptoOp
    {
        NONE,
        INCREMENT,
        DECREMENT,
        ASSIGN,
    }
    public class Action
    {
        public List<Action> subActions = new List<Action>();
        private string attrib;
        private ZeptoOp op;
        private int val;

        public void Setup()
        {
            subActions.Clear();
        }
        public void AddSubAction(Action next)
        {
            subActions.Add(next);
        }

        public void SetAttribChanger(string attrib, ZeptoOp op, int val)
        {
            this.attrib = attrib;
            this.op = op;
            this.val = val;
        }

        public bool PawnPerform(Pawn obj)
        {
            bool consumed = false;
            foreach(Action sub in subActions)
            {
                consumed |= sub.PawnPerform(obj);
            }
            switch(this.op)
            {
                case ZeptoOp.ASSIGN:
                    obj.SetAttrib(this.attrib, this.val);
                    consumed = true;
                    break;
                case ZeptoOp.INCREMENT:
                    obj.ModifyAttrib(this.attrib, this.val);
                    consumed = true;
                    break;
                case ZeptoOp.DECREMENT:
                    obj.ModifyAttrib(this.attrib, -this.val);
                    consumed = true;
                    break;
                default:
                    break;
            }
            return consumed;
            
            
        }
    }
}