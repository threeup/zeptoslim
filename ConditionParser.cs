using System;
namespace zeptolib
{
    public static class ConditionParser
    {
        private static string LessThanString = "<";
        private static string[] LessThanStringSep = new string[] { LessThanString };
        private static string GreaterThanString = ">";
        private static string[] GreaterThanStringSep = new string[] { GreaterThanString };
        private static string EqualString = "==";
        private static string[] EqualStringSep = new string[] { EqualString };
        public static Condition MakeCondition(string line)
        {
            Condition cond = new Condition();
            cond.Setup();
            if(line.Contains(","))
            {
                string[] chunks = line.Split(',');
                
                foreach(string chunk in chunks)
                {
                    cond.AddSubCondition(MakeCondition(chunk));
                }
                return cond;
            }
            else
            {

                if (line.Contains(LessThanString))
                {
                    String[] chunks = line.Split(LessThanStringSep, StringSplitOptions.None);
                    string attrib = RuleLib.StringToAttrib(chunks[0]);
                    int val = int.Parse(chunks[1]);
                    cond.SetAttribComparer(attrib, ZeptoComp.LESS_THAN, val);
                }
                else if (line.Contains(GreaterThanString))
                {
                    String[] chunks = line.Split(GreaterThanStringSep, StringSplitOptions.None);
                    string attrib = RuleLib.StringToAttrib(chunks[0]);
                    int val = int.Parse(chunks[1]);
                    cond.SetAttribComparer(attrib, ZeptoComp.GREATER_THAN, val);
                }
                else if (line.Contains(EqualString))
                {
                    String[] chunks = line.Split(EqualStringSep, StringSplitOptions.None);
                    string attrib = RuleLib.StringToAttrib(chunks[0]);
                    int val = int.Parse(chunks[1]);
                    cond.SetAttribComparer(attrib, ZeptoComp.EQUAL, val);
                }

                return cond;
            }
        }

    }
}