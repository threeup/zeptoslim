
using System;
using System.Collections.Generic;
using ZeptoCommon;

namespace ZeptoFormula
{
    public static class UnitTest
    {
        public static void Run()
        {
            TestZero();
            TestOne();
            TestTwo();
            TestThree();
            Console.WriteLine("Test Passed");
        }

        public static void TestZero()
        {
            string varFileContents = "HP, , ";
            string[] prepFileContents = new string[]{""};
            string testLine = "4";
            
            IFormulaContext ctx = FormulaFactory.MakeContext(varFileContents, prepFileContents);
            List<string> buffer = new List<string>();
            Parser.StringIntoChunks(testLine, ref buffer);
            Formula f = FormulaFactory.Make(ctx, buffer);
            int val = f.Calculate(ctx);
            
            if(val != 4)
            {
                throw new Exception("TestZero fail");
            }
        }
        public static void TestOne()
        {
            string varFileContents = "HP";
            string[] prepFileContents = new string[]{"HP=5"};
            string testLine = "HP+3";

            IFormulaContext ctx = FormulaFactory.MakeContext(varFileContents, prepFileContents);
            List<string> buffer = new List<string>();
            Parser.StringIntoChunks(testLine, ref buffer);
            Formula f = FormulaFactory.Make(ctx, buffer);
            int val = f.Calculate(ctx);

            if(val != 8)
            {
                throw new Exception("TestOne fail");
            }
        }
        public static void TestTwo()
        {
            string varFileContents = "HP, ENERGY, ";
            string[] prepFileContents = new string[]{"HP=5","ENERGY=2"};
            string testLine = "HP += 3*ENERGY";


            IFormulaContext ctx = FormulaFactory.MakeContext(varFileContents, prepFileContents);
            List<string> buffer = new List<string>();
            Parser.StringIntoChunks(testLine, ref buffer);
            Formula f = FormulaFactory.Make(ctx, buffer);
            int val = f.Calculate(ctx);
            
            if(val != 11)
            {
                throw new Exception("TestTwo fail"+val);
            }

        }
        public static void TestThree()
        {
            string varFileContents = "HP, ENERGY, ";
            string[] prepFileContents = new string[]{"HP=15","ENERGY=2"};
            string testLine = "HP = ENERGY+";

           
            IFormulaContext ctx = FormulaFactory.MakeContext(varFileContents, prepFileContents);
            List<string> buffer = new List<string>();
            Parser.StringIntoChunks(testLine, ref buffer);
            try
            {
                Formula f = FormulaFactory.Make(ctx, buffer);
            }
            catch(Exception)
            {
                return;
            }
            throw new Exception("TestThree Failed to throw exception");
        }
    }
}