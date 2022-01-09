
namespace ZeptoFormula;

public interface IFormulaContext
{
  void AddVariableName(string varName);
  void AddVariableNameList(List<string> varNameList);
  bool ContainsVariableName(string varName);
  int GetVariableIndex(string varName);
  void SetVariableValue(string varName, int val);
  int GetVariableValue(int idx);
  int DoAssign(FormulaElementType assignType, int elementIndex, int val);
  string ToLongString();
}
