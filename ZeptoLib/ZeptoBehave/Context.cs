
using ZeptoFormula;
using ZeptoInstruction;
namespace ZeptoBehave;

public class Context : IFormulaContext, IInstructionContext
{

  private Dictionary<string, Func<int, int>> methodPtrMap = new Dictionary<string, Func<int, int>>();
  private Dictionary<string, int> varNameMap = new Dictionary<string, int>();
  private Dictionary<int, int> varVals = new Dictionary<int, int>();


  private static int Noop(int something)
  {
    return something;
  }
  public static int Double(int val)
  {
    return val * 2;
  }
  private static string SanitizeString(string s)
  {
    return s.ToUpper();
  }
  public void AddMethodName(string methodNameRaw)
  {
    string methodName = SanitizeString(methodNameRaw);
    Func<int, int> f = Noop;
    methodPtrMap.Add(methodName, f);
  }
  public bool ContainsMethodName(string methodName)
  {
    return methodPtrMap.ContainsKey(methodName);
  }
  public void SetMethodPtr(string methodNameRaw, Func<int, int> f)
  {
    string methodName = SanitizeString(methodNameRaw);
    if (ContainsMethodName(methodName))
    {
      methodPtrMap[methodName] = f;
    }
    else
    {
      methodPtrMap.Add(methodName, f);
    }
  }

  public void AddMethodNameList(List<string> nameListRaw)
  {
    nameListRaw.ForEach(x => AddMethodName(x));
  }
  public Func<int, int> GetMethodPtr(string methodName)
  {
    return methodPtrMap[methodName];
  }

  public void AddVariableName(string varNameRaw)
  {
    string varName = SanitizeString(varNameRaw);
    int idx = varNameMap.Keys.Count;
    varNameMap.Add(varName, idx);
    varVals.Add(idx, 0);
  }

  public void AddVariableNameList(List<string> nameListRaw)
  {
    nameListRaw.ForEach(x => AddVariableName(x));
  }
  public bool ContainsVariableName(string varName)
  {
    return varNameMap.ContainsKey(varName);
  }

  public int GetVariableIndex(string varName)
  {
    return varNameMap[varName];
  }

  public void SetVariableValue(string varName, int val)
  {
    int idx = GetVariableIndex(varName);
    varVals[idx] = val;
  }

  public int GetVariableValue(int idx)
  {
    return varVals[idx];
  }

  public int DoAssign(FormulaElementType assignType, int elementIndex, int val)
  {
    switch (assignType)
    {
      case FormulaElementType.SET:
        varVals[elementIndex] = val;
        break;
      case FormulaElementType.INCREMENT:
        varVals[elementIndex] += val;
        break;
      case FormulaElementType.DECREMENT:
        varVals[elementIndex] -= val;
        break;
      default:
        break;
    }
    return varVals[elementIndex];
  }

  public string ToLongString()
  {
    System.Text.StringBuilder sb = new System.Text.StringBuilder();
    sb.AppendLine("Ctx:");
    foreach (KeyValuePair<string, int> kvp in varNameMap)
    {
      sb.Append(kvp.Key);
      sb.Append('#');
      sb.Append(kvp.Value);
      sb.Append('=');
      sb.AppendLine(varVals[kvp.Value].ToString());
    }
    foreach (KeyValuePair<string, Func<int, int>> kvp in methodPtrMap)
    {
      sb.Append(kvp.Key);
      sb.Append('*');

    }
    return sb.ToString();
  }
}
