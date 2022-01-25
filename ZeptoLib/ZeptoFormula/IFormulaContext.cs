
namespace ZeptoFormula;

public interface IFormulaContext
{
  void AddVariableName(string varName);
  void AddVariableNameList(List<string> varNameList);
  bool ContainsVariableName(string varName);
  int GetVariableIndex(string varName);
  int GetVariableValue(int idx);
  int DoAssign(FormulaElementType assignType, int elementIndex, int val);
  
  void CopyVariableData(IFormulaContext other);
  string ToLongString();
}
