using System.Collections.Generic;

namespace ZeptoFormula
{
    public class Context
    {
        public Dictionary<string,int> VariableNameDict = new Dictionary<string, int>();
        private Dictionary<int, int> VariableValues = new Dictionary<int, int>();
        

        public void AddVariableName(string varName)
        {
            int idx = VariableNameDict.Keys.Count;
            VariableNameDict.Add(varName, idx);
            VariableValues.Add(idx, 0);
        }

        public void SetVariableValue(string varName, int val)
        {
            int idx = VariableNameDict[varName];
            VariableValues[idx] = val;
        }

        public int GetVariableValue(int idx)
        {
            return VariableValues[idx];
        }

        public int DoAssign(ElementType assignType, int elementIndex, int val)
        {
            switch(assignType)
            {
                case ElementType.SET:
                    VariableValues[elementIndex] = val;
                    break;
                case ElementType.INCREMENT:
                    VariableValues[elementIndex] += val;
                    break;
                case ElementType.DECREMENT:
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
            foreach(KeyValuePair<string, int> kvp in VariableNameDict)
            {
                sb.Append(kvp.Key);
                sb.Append('=');
                sb.AppendLine(VariableValues[kvp.Value].ToString());
            }
            return sb.ToString();
        }
    }
}