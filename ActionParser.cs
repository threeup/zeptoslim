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
                    string attrib = StringToAttrib(chunks[0]);
                    int val = int.Parse(chunks[1]);
                    action.SetAttribChanger(attrib, ZeptoOp.INCREMENT, val);
                }
                else if (line.Contains(DecrementString))
                {
                    string[] chunks = line.Split(DecrementStringSep, StringSplitOptions.None);
                    string attrib = StringToAttrib(chunks[0]);
                    int val = int.Parse(chunks[1]);
                    action.SetAttribChanger(attrib, ZeptoOp.DECREMENT, val);
                }
                else if (line.Contains(AssignmentString))
                {
                    string[] chunks = line.Split(AssignmentStringSep, StringSplitOptions.None);
                    string attrib = StringToAttrib(chunks[0]);
                    int val = int.Parse(chunks[1]);
                    action.SetAttribChanger(attrib, ZeptoOp.ASSIGN, val);
                }

                return action;
            }
        }

        private static string StringToAttrib(string val)
        {
            if(val == "X")
            {
                return Attribs.X;
            }
            if(val == "Y")
            {
                return Attribs.Y;
            }
            if(val == "HP")
            {
                return Attribs.HP;
            }
            return val;
        }
    }
}