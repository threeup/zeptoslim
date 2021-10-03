
using System;
using System.Collections.Generic;

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
            Context ctx = new Context();
            ctx.AddVariableName("HP");

            string line = "4";
            List<string> chunks = Parser.StringIntoChunks(line);
            Formula f = Parser.ChunksIntoFormula(ctx, chunks);
            //Console.WriteLine(f.ToLongString(ctx));
            int val = f.Calculate(ctx);
            //Console.WriteLine(ctx.ToLongString());
            if(val != 4)
            {
                throw new Exception("TestZero fail");
            }
        }
        public static void TestOne()
        {
            Context ctx = new Context();
            ctx.AddVariableName("HP");
            ctx.SetVariableValue("HP",5);
            
            string line = "HP+3";
            List<string> chunks = Parser.StringIntoChunks(line);
            Formula f = Parser.ChunksIntoFormula(ctx, chunks);
            //Console.WriteLine(f.ToLongString(ctx));
            int val = f.Calculate(ctx);
            //Console.WriteLine(ctx.ToLongString());
            if(val != 8)
            {
                throw new Exception("TestOne fail");
            }
        }
        public static void TestTwo()
        {
            Context ctx = new Context();
            ctx.AddVariableName("HP");
            ctx.AddVariableName("ENERGY");
            ctx.SetVariableValue("HP",5);
            ctx.SetVariableValue("ENERGY",2);

            string line = "HP += 3*ENERGY";
            List<string> chunks = Parser.StringIntoChunks(line);
            Formula f = Parser.ChunksIntoFormula(ctx, chunks);
            //Console.WriteLine(f.ToLongString(ctx));
            int val = f.Calculate(ctx);
            //Console.WriteLine(ctx.ToLongString());
            if(val != 11)
            {
                throw new Exception("TestTwo fail");
            }

        }
        public static void TestThree()
        {
            Context ctx = new Context();
            ctx.AddVariableName("HP");
            ctx.AddVariableName("ENERGY");
            ctx.SetVariableValue("HP",5);
            ctx.SetVariableValue("ENERGY",2);

            string line = "HP = ENERGY+";
            List<string> chunks = Parser.StringIntoChunks(line);
            try
            {
                Formula f = Parser.ChunksIntoFormula(ctx, chunks);
                //Console.WriteLine(f.ToLongString(ctx));
            }
            catch(Exception)
            {
                return;
            }
            throw new Exception("TestThree Failed to throw exception");
        }
    }
}