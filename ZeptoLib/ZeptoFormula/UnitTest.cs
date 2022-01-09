
using ZeptoCommon;

namespace ZeptoFormula;

public static class UnitTest
{
  public static void Run()
  {
    TestZero();
    TestOne();
    TestTwo();
    TestThree();
    Console.WriteLine("Formula Test Passed");
  }

  public static void TestZero()
  {
    string varContents = "HP, , ";
    string[] prepContents = new string[] { "" };
    string testLine = "4";

    IFormulaContext ctx = FormulaFactory.MakeContext(varContents, prepContents);
    List<string> buffer = new List<string>();
    Parser.StringIntoChunks(testLine, ref buffer);
    Formula f = FormulaFactory.Make(ctx, buffer, 0);
    int val = f.Calculate(ctx);

    if (val != 4)
    {
      throw new Exception("TestZero fail");
    }
  }
  public static void TestOne()
  {
    string varContents = "HP";
    string[] prepContents = new string[] { "HP=5" };
    string testLine = "HP+3";

    IFormulaContext ctx = FormulaFactory.MakeContext(varContents, prepContents);
    List<string> buffer = new List<string>();
    Parser.StringIntoChunks(testLine, ref buffer);
    Formula f = FormulaFactory.Make(ctx, buffer, 0);
    int val = f.Calculate(ctx);

    if (val != 8)
    {
      throw new Exception("TestOne fail");
    }
  }
  public static void TestTwo()
  {
    string varContents = "HP, ENERGY, ";
    string[] prepContents = new string[] { "HP=5", "ENERGY=2" };
    string testLine = "HP += 3*ENERGY";


    IFormulaContext ctx = FormulaFactory.MakeContext(varContents, prepContents);
    List<string> buffer = new List<string>();
    Parser.StringIntoChunks(testLine, ref buffer);
    Formula f = FormulaFactory.Make(ctx, buffer, 0);
    int val = f.Calculate(ctx);

    if (val != 11)
    {
      throw new Exception("TestTwo fail" + val);
    }

  }
  public static void TestThree()
  {
    string varContents = "HP, ENERGY, ";
    string[] prepContents = new string[] { "HP=15", "ENERGY=2" };
    string testLine = "HP = ENERGY+";


    IFormulaContext ctx = FormulaFactory.MakeContext(varContents, prepContents);
    List<string> buffer = new List<string>();
    Parser.StringIntoChunks(testLine, ref buffer);
    try
    {
      Formula f = FormulaFactory.Make(ctx, buffer, 0);
    }
    catch (Exception)
    {
      return;
    }
    throw new Exception("TestThree Failed to throw exception");
  }
}
