

using ZeptoFormula;
using ZeptoBehave;
using ZeptoCommon;

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
        RunZeptoTest("test\\branchtest2.zeptest");
        // RunZeptoTest("test\\branchtest3.zeptest");
        // RunZeptoTest("test\\branchtest4.zeptest");
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
        List<Instruction> instrList = new();
        List<string> buffer = new();
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
        List<Instruction> instrList = new();
        List<string> buffer = new();
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

        List<Instruction> instrList = new();
        List<string> buffer = new();
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
        List<Instruction> instrList = new();
        List<string> buffer = new();
        InstructionFactory.MakeList(ictx, bodyContents, ref instrList, ref buffer);
        CheckAnswers(ictx, ref instrList, ref answers);
    }


    public static void RunZeptoTest(string filename)
    {
        Console.WriteLine("Testing " + filename);

        ZeptoTest zt = ParseFile(filename);

        Consumer c = new();
        c.SetContext(zt.varNames, zt.methodNames);
        if (zt.prepContents != null)
        {
            c.ConsumeFormulaList(zt.prepContents);
        }
        List<Instruction> instrList = new();
        List<string> buffer = new();
        if (zt.bodyContents != null)
        {
            InstructionFactory.MakeList(c.ctx, zt.bodyContents, ref instrList, ref buffer);
        }
        c.MakeTree(instrList);
        c.ConsumeStart();
        if (zt.answers != null)
        {
            bool success = CheckContext(c.ctx, ref zt.answers);
            Console.WriteLine(success ? "Pass" : "Failed");
        }
    }

    private static void ParseFileAssign(ZeptoTest zt, string lastSection, ref List<string> buffer)
    {
        if (lastSection.StartsWith("#VAR"))
        {
            List<string> varChunks = new();
            for (int i = 0; i < buffer.Count; ++i)
            {
                varChunks.AddRange(Parser.CommaSeparatedIntoChunks(buffer[i]));
            }
            zt.varNames = varChunks.ToArray();
        }
        else if (lastSection.StartsWith("#METHOD"))
        {
            List<string> methodChunks = new();
            for (int i = 0; i < buffer.Count; ++i)
            {
                methodChunks.AddRange(Parser.CommaSeparatedIntoChunks(buffer[i]));
            }
            zt.methodNames = methodChunks.ToArray();
        }
        else if (lastSection.StartsWith("#PREP"))
        {
            zt.prepContents = buffer.ToArray();
        }
        else if (lastSection.StartsWith("#BODY"))
        {
            zt.bodyContents = buffer.ToArray();
        }
        else if (lastSection.StartsWith("#ANSWERS"))
        {
            List<int> answerChunks = new();
            for (int i = 0; i < buffer.Count; ++i)
            {
                answerChunks.AddRange(Parser.CommaSeparatedIntoNumbers(buffer[i]));
            }
            zt.answers= answerChunks.ToArray();;
        }
    }

    public static ZeptoTest ParseFile(string testPath)
    {
        ZeptoTest zt = new();
        using (StreamReader file = new StreamReader(testPath))
        {
            List<string> buffer = new();
            string section = "";
            string? curLine;
            while (file != null && (curLine = file.ReadLine()) != null)
            {
                if (curLine.StartsWith('#'))
                {
                    if(buffer.Count > 0)
                    {
                        ParseFileAssign(zt, section, ref buffer);
                        buffer.Clear();
                    }
                    section = curLine;
                }
                else
                {
                    buffer.Add(curLine);
                }
            }
            ParseFileAssign(zt, section, ref buffer);
            if (file != null)
            {
                file.Close();
            }
        }
        return zt;
    }

    private static bool CheckAnswers(IInstructionContext ictx, ref List<Instruction> instrList, ref int[] answers)
    {
        IFormulaContext? fctx = ictx as IFormulaContext;
        bool success = true;
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
                    success = false;
                }
            }
        }
        return success;
    }

    private static bool CheckContext(IInstructionContext ictx, ref int[] answers)
    {
        IFormulaContext? fctx = ictx as IFormulaContext;
        bool success = true;
        for (int i = 0; i < answers.Length; ++i)
        {
            if (fctx != null)
            {
                int val = fctx.GetVariableValue(i);
                if (val != answers[i])
                {
                    //throw new Exception("Test fail " + val + " Expected:" + answers[i]);
                    Console.WriteLine("Test fail " + val + " Expected:" + answers[i]);
                    success = false;
                }
            }
        }
        return success;
        

    }
}
