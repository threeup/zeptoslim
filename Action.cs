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

    public enum ZeptoVar
    {
        NONE,
        HP,
        NRG,
        SPD
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
        private ZeptoVar var;
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

        public void SetVarChanger(ZeptoVar var, ZeptoOp op, int val)
        {
            this.var = var;
            this.op = op;
            this.val = val;
        }

        public bool PawnPerform(Pawn obj)
        {
            return true;
        }
    }
}