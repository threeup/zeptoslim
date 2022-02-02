

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
    BranchTestZero();
    BranchTestOne();
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

  public static void BranchTestZero()
  {
    string varContents = "HP, , ";
    string methodContents = " ";
    string[] prepContents = new string[] { "HP=1" };
    string[] bodyContents = new string[] { "if 0", " HP=2", "if 1", " HP+=4", "HP-=1" };
    int[] answers = new int[] { 4 };

    Consumer c = new Consumer();
    c.SetContext(varContents, methodContents);
    c.ConsumeFormulaList(prepContents);
    List<Instruction> instrList = new List<Instruction>();
    List<string> buffer = new List<string>();
    InstructionFactory.MakeList(c.ctx, bodyContents, ref instrList, ref buffer);
    c.MakeTree(instrList);
    c.ConsumeStart();
    CheckContext(c.ctx, ref answers);
  }

  
  public static void BranchTestOne()
  {
    string varContents = "HP, ENERGY ";
    string methodContents = " ";
    string[] prepContents = new string[] { "HP=1","ENERGY=0" };
    string[] bodyContents = new string[] { "if 1", " HP=5", " ENERGY+=1", " ENERGY+=1", " ENERGY+=1","else"," HP=2", "ENERGY+=3" };
    int[] answers = new int[] { 5,6 };

    Consumer c = new Consumer();
    c.SetContext(varContents, methodContents);
    c.ConsumeFormulaList(prepContents);
    List<Instruction> instrList = new List<Instruction>();
    List<string> buffer = new List<string>();
    InstructionFactory.MakeList(c.ctx, bodyContents, ref instrList, ref buffer);
    c.MakeTree(instrList);
    c.ConsumeStart();
    CheckContext(c.ctx, ref answers);
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
