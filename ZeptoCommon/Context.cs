using System;
using System.Collections.Generic;
using ZeptoFormula;
using ZeptoInstruction;
namespace ZeptoCommon
{
    public class Context : IFormulaContext, IInstructionContext
    {

        public Dictionary<string, Func<int, bool>> VerbActionDict = new Dictionary<string, Func<int, bool>>();
        public Dictionary<string, int> VariableNameDict = new Dictionary<string, int>();
        private Dictionary<int, int> VariableValues = new Dictionary<int, int>();


        private static bool Noop(int something)
        {
            return false;
        }
        public void AddVerbName(string verbName)
        {
            Func<int, bool> f = Noop;
            VerbActionDict.Add(verbName, f);
        }
        public void AddVerbNameList(List<string> nameList)
        {
            nameList.ForEach(x => AddVerbName(x));
        }
        public bool ContainsVerbName(string verbName)
        {
            return VerbActionDict.ContainsKey(verbName);
        }
        public Func<int, bool> GetVerbAction(string verbName)
        {
            return VerbActionDict[verbName];
        }

        public void AddVariableName(string varName)
        {
            int idx = VariableNameDict.Keys.Count;
            VariableNameDict.Add(varName, idx);
            VariableValues.Add(idx, 0);
        }
        
        public void AddVariableNameList(List<string> nameList)
        {
            nameList.ForEach(x => AddVariableName(x));
        }
        public bool ContainsVariableName(string varName)
        {
            return VariableNameDict.ContainsKey(varName);
        }

        public int GetVariableIndex(string varName)
        {
            return VariableNameDict[varName];
        }

        public void SetVariableValue(string varName, int val)
        {
            int idx = GetVariableIndex(varName);
            VariableValues[idx] = val;
        }

        public int GetVariableValue(int idx)
        {
            return VariableValues[idx];
        }

        public int DoAssign(FormulaElementType assignType, int elementIndex, int val)
        {
            switch (assignType)
            {
                case FormulaElementType.SET:
                    VariableValues[elementIndex] = val;
                    break;
                case FormulaElementType.INCREMENT:
                    VariableValues[elementIndex] += val;
                    break;
                case FormulaElementType.DECREMENT:
                    VariableValues[elementIndex] -= val;
                    break;
                default:
                    break;
            }
            return VariableValues[elementIndex];
        }

        public string ToLongString()
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.AppendLine("Ctx:");
            foreach (KeyValuePair<string, int> kvp in VariableNameDict)
            {
                sb.Append(kvp.Key);
                sb.Append('#');
                sb.Append(kvp.Value);
                sb.Append('=');
                sb.AppendLine(VariableValues[kvp.Value].ToString());
            }
            foreach (KeyValuePair<string, Func<int, bool>> kvp in VerbActionDict)
            {
                sb.Append(kvp.Key);
                sb.Append('*');
                
            }
            return sb.ToString();
        }
    }
};