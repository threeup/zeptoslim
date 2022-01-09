

namespace ZeptoInstruction;

public enum Syntax
{
  INVALID,
  FALSE,
  TRUE,
  ZERO,
  ONE,
}
public static class Utils
{

  public static int SyntaxToInt(int keyword)
  {
    switch (keyword)
    {
      case (int)Syntax.ZERO:
      case (int)Syntax.FALSE:
        return 0;
      case (int)Syntax.ONE:
      case (int)Syntax.TRUE:
        return 1;
    }
    return -1;
  }
}
