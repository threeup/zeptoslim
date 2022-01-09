
using ZeptoCommon;
using ZeptoFormula;

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
    string verbContents = " ";
    string[] prepContents = new string[] { "" };
    string[] bodyContents = new string[] { "4" };

    IInstructionContext ctx = InstructionFactory.MakeContext(varContents, verbContents, prepContents);
    List<Instruction> instrList = new List<Instruction>();
    List<string> buffer = new List<string>();
    InstructionFactory.MakeList(ctx, bodyContents, ref instrList, ref buffer);

  }
  public static void TestOne()
  {
    string varContents = "HP,ENERGY";
    string verbContents = "explode";
    string[] prepContents = new string[] { "HP=1", "ENERGY=1" };
    string[] bodyContents = new string[] { "if HP > 1", "  HP = 3", "ENERGY = 3" };

    IInstructionContext ctx = InstructionFactory.MakeContext(varContents, verbContents, prepContents);

    List<Instruction> instrList = new List<Instruction>();
    List<string> buffer = new List<string>();
    InstructionFactory.MakeList(ctx, bodyContents, ref instrList, ref buffer);

    //if HP > 1:
    //    HP = 3
    //    ENERGY = 1

    //if explode(3) > laser(HP):
    //    HP = explode(1) + laser(2)
  }
  public static void TestTwo()
  {
    string varContents = "HP,ENERGY";
    string verbContents = "explode,laser";
    string[] prepContents = new string[] { "HP=1", "ENERGY=1" };
    string[] bodyContents = new string[] { "if explode(3) > laser(HP)", "  HP = explode(1) + laser(2)", "ENERGY = 3" };


    IInstructionContext ctx = InstructionFactory.MakeContext(varContents, verbContents, prepContents);
    List<Instruction> instrList = new List<Instruction>();
    List<string> buffer = new List<string>();
    InstructionFactory.MakeList(ctx, bodyContents, ref instrList, ref buffer);
  }
  public static void TestThree()
  {
  }
}
