using System;
namespace zeptolib
{
    public static class ActionParser
    {
        private static string IncrementString = "+=";
        private static string[] IncrementStringSep = new string[] { IncrementString };
        private static string DecrementString = "-=";
        private static string[] DecrementStringSep = new string[] { DecrementString };
        private static string AssignmentString = "=";
        private static string[] AssignmentStringSep = new string[] { AssignmentString };
        public static Action MakeAction(string line)
        {
            Action action = new Action();
            action.Setup();
            if(line.Contains(","))
            {
                string[] chunks = line.Split(',');
                
                foreach(string chunk in chunks)
                {
                    action.AddSubAction(MakeAction(chunk));
                }
                return action;
            }
            else
            {

                if (line.Contains(IncrementString))
                {
                    String[] chunks = line.Split(IncrementStringSep, StringSplitOptions.None);
                    ZeptoVar var = StringToVar(chunks[0]);
                    int val = int.Parse(chunks[1]);
                    action.SetVarChanger(var, ZeptoOp.INCREMENT, val);
                }
                if (line.Contains(DecrementString))
                {
                    string[] chunks = line.Split(DecrementStringSep, StringSplitOptions.None);
                    ZeptoVar var = StringToVar(chunks[0]);
                    int val = int.Parse(chunks[1]);
                    action.SetVarChanger(var, ZeptoOp.DECREMENT, val);
                }
                if (line.Contains(AssignmentString))
                {
                    string[] chunks = line.Split(AssignmentStringSep, StringSplitOptions.None);
                    ZeptoVar var = StringToVar(chunks[0]);
                    int val = int.Parse(chunks[1]);
                    action.SetVarChanger(var, ZeptoOp.ASSIGN, val);
                }

                return action;
            }
        }

        private static ZeptoVar StringToVar(string val)
        {
            if(val == "HP")
            {
                return ZeptoVar.HP;
            }
            if(val == "NRG")
            {
                return ZeptoVar.NRG;
            }
            if(val == "SPD")
            {
                return ZeptoVar.SPD;
            }
            return ZeptoVar.NONE;
        }
    }
}