

using ZeptoFormula;
using ZeptoBehave;

namespace ZeptoInstruction;

public static class UnitTest
{
    public static void Run()
    {
        FlatTestZero();
        FlatTestOne();
        FlatTestTwo();
        FlatTestThree();
        RunZeptoTest("test\\branchtest0.zeptest");
        RunZeptoTest("test\\branchtest1.zeptest");
        Console.WriteLine("Instruction Test Passed");
    }

    public static void FlatTestZero()
    {
        string varContents = "HP, , ";
        string methodContents = " ";
        string[] prepContents = new string[] { "" };
        string[] bodyContents = new string[] { "4" };
        int[] answers = new int[] { 4 };

        IInstructionContext ictx = InstructionFactory.MakeContext(varContents, methodContents, prepContents);
        List<Instruction> instrList = new List<Instruction>();
        List<string> buffer = new List<string>();
        InstructionFactory.MakeList(ictx, bodyContents, ref instrList, ref buffer);
        CheckAnswers(ictx, ref instrList, ref answers);

    }
    public static void FlatTestOne()
    {
        string varContents = "HP,ENERGY";
        string methodContents = "explode";
        string[] prepContents = new string[] { "HP=1", "ENERGY=1" };
        string[] bodyContents = new string[] { "if HP > 1", "  HP = 3", "ENERGY = explode(3)" };
        int[] answers = new int[] { 0, 3, 6 };

        IInstructionContext ictx = InstructionFactory.MakeContext(varContents, methodContents, prepContents);
        ictx.SetMethodPtr("explode", Context.Double);
        List<Instruction> instrList = new List<Instruction>();
        List<string> buffer = new List<string>();
        InstructionFactory.MakeList(ictx, bodyContents, ref instrList, ref buffer);
        CheckAnswers(ictx, ref instrList, ref answers);

    }
    public static void FlatTestTwo()
    {
        string varContents = "HP,ENERGY";
        string methodContents = " ";
        string[] prepContents = new string[] { "HP=1", "ENERGY=1" };
        string[] bodyContents = new string[] { "(3+ENERGY) - (2*HP)" };
        int[] answers = new int[] { 2 };


        IInstructionContext ictx = InstructionFactory.MakeContext(varContents, methodContents, prepContents);

        List<Instruction> instrList = new List<Instruction>();
        List<string> buffer = new List<string>();
        InstructionFactory.MakeList(ictx, bodyContents, ref instrList, ref buffer);

        CheckAnswers(ictx, ref instrList, ref answers);
    }
    public static void FlatTestThree()
    {
        string varContents = "HP,ENERGY";
        string methodContents = "explode,laser";
        string[] prepContents = new string[] { "HP=1", "ENERGY=1" };
        string[] bodyContents = new string[] { "HP+3*ENERGY", "ENERGY = 3*HP", "HP = explode(2) + laser(1)", "explode(3*HP) + ENERGY" };

        int[] answers = new int[] { 4, 3, 5, 33 };

        IInstructionContext ictx = InstructionFactory.MakeContext(varContents, methodContents, prepContents);
        ictx.SetMethodPtr("explode", Context.Double);
        List<Instruction> instrList = new List<Instruction>();
        List<string> buffer = new List<string>();
        InstructionFactory.MakeList(ictx, bodyContents, ref instrList, ref buffer);
        CheckAnswers(ictx, ref instrList, ref answers);
    }


    public static void RunZeptoTest(string filename)
    {
        Console.WriteLine("Testing " + filename);

        ZeptoTest zt = ParseFile(filename);

        Consumer c = new Consumer();
        c.SetContext(zt.varContents, zt.methodContents);
        if (zt.prepContents != null)
        {
            c.ConsumeFormulaList(zt.prepContents);
        }
        List<Instruction> instrList = new List<Instruction>();
        List<string> buffer = new List<string>();
        if (zt.bodyContents != null)
        {
            InstructionFactory.MakeList(c.ctx, zt.bodyContents, ref instrList, ref buffer);
        }
        c.MakeTree(instrList);
        c.ConsumeStart();
        if (zt.answers != null)
        {
            CheckContext(c.ctx, ref zt.answers);
        }
    }

    public static ZeptoTest ParseFile(string testPath)
    {
        ZeptoTest zt = new ZeptoTest();
        using (StreamReader file = new StreamReader(testPath))
        {
            List<string> buffer = new List<string>();
            string section = "";
            string? curLine;
            while (file != null && (curLine = file.ReadLine()) != null)
            {
                if (curLine.StartsWith('#'))
                {
                    if (section.StartsWith("#VAR"))
                    {
                        zt.varContents = buffer.ToArray();
                    }
                    else if (curLine.StartsWith("#METHOD"))
                    {
                        zt.methodContents = buffer.ToArray();
                    }
                    else if (curLine.StartsWith("#PREP"))
                    {
                        zt.prepContents = buffer.ToArray();
                    }
                    else if (curLine.StartsWith("#BODY"))
                    {
                        zt.bodyContents = buffer.ToArray();
                    }
                    buffer.Clear();
                    section = curLine;
                }
                else
                {
                    buffer.Add(curLine);
                }
            }
            if (file != null)
            {
                file.Close();
            }
        }
        return zt;
    }

    private static void CheckAnswers(IInstructionContext ictx, ref List<Instruction> instrList, ref int[] answers)
    {
        IFormulaContext? fctx = ictx as IFormulaContext;
        for (int i = 0; i < instrList.Count; ++i)
        {
            Expression? ex = instrList[i].expression;
            if (ex != null && fctx != null)
            {
                int val = ex.Calculate(fctx, ictx);
                if (val != answers[i])
                {
                    //throw new Exception("Test fail " + val + " Expected:" + answers[i]);
                    Console.WriteLine("Test fail " + val + " Expected:" + answers[i]);
                }
            }
        }
    }

    private static void CheckContext(IInstructionContext ictx, ref int[] answers)
    {
        IFormulaContext? fctx = ictx as IFormulaContext;
        for (int i = 0; i < answers.Length; ++i)
        {
            if (fctx != null)
            {
                int val = fctx.GetVariableValue(i);
                if (val != answers[i])
                {
                    //throw new Exception("Test fail " + val + " Expected:" + answers[i]);
                    Console.WriteLine("Test fail " + val + " Expected:" + answers[i]);
                }
            }
        }

    }
}
