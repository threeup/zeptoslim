

using ZeptoFormula;
using ZeptoBehave;

namespace ZeptoInstruction;

public static class UnitTest
{
  public static void Run()
  {
    TestZero();
    TestOne();
    TestTwo();
    TestThree();
    Console.WriteLine("Instruction Test Passed");
  }

  public static void TestZero()
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
  public static void TestOne()
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
  public static void TestTwo()
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
  public static void TestThree()
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

  private static void CheckAnswers(IInstructionContext ictx, ref List<Instruction> instrList, ref int[] answers)
  {
    IFormulaContext? fctx = ictx as IFormulaContext;
    for (int i = 0; i < instrList.Count; ++i)
    {
      Expression? ex = instrList[i].execution;
      if (ex != null && fctx != null)
      {
        int val = ex.Calculate(fctx, ictx);
        if (val != answers[i])
        {
          throw new Exception("Test fail " + val + " Expected:" + answers[i]);
        }
      }
    }
  }
}
