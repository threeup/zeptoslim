using System.IO;

namespace zeptolib
{
    public static class RuleLib
    {
        
        public static string StringToAttrib(string val)
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

        public static void LoadStart(Pawn pawn, string filePath)
        {
            using (StreamReader file = new StreamReader(filePath))
            {
                string ln;

                while ((ln = file.ReadLine()) != null)
                {
                    Action action = ActionParser.MakeAction(ln);
                    action.PawnPerform(pawn);
                }
                file.Close();
            }
        }

        
        public static void LoadTile(Pawn pawn, string filePath)
        {
            using (StreamReader file = new StreamReader(filePath))
            {
                string ln;

                while ((ln = file.ReadLine()) != null)
                {
                    //Condition condition = ConditionParser.MakeAction(ln);
                    //Action action = ActionParser.MakeAction(ln);
                    //action.PawnPerform(pawn);
                }
                file.Close();
            }
        }
        
        public static void LoadEnd(Pawn pawn, string filePath)
        {
            using (StreamReader file = new StreamReader(filePath))
            {
                string ln;

                while ((ln = file.ReadLine()) != null)
                {
                    //Condition cond = ConditionParser.MakeCondition(ln);
                }
                file.Close();
            }
        }
    }
}