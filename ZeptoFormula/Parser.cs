
using System;
using System.Collections.Generic;

namespace ZeptoFormula
{
    public static class Parser
    {

        public static void StripComments(string[] lines)
        {
            for (int i = 0; i < lines.Length; ++i)
            {
                string line = lines[i];
                int indexOfComment = line.IndexOf("//");
                if (indexOfComment != -1)
                {
                    line = line.Substring(0, indexOfComment);
                }
                line = line.TrimEnd();
                lines[i] = line;
            }
        }

        public static List<string> StringIntoChunks(string line)
        {
            List<string> stringChunks = new List<string>();
            int indexOfComment = line.IndexOf("//");
            if (indexOfComment != -1)
            {
                line = line.Substring(0, indexOfComment);
            }
            line = line.Trim();
            if (line.Length == 0)
            {
                return stringChunks;
            }


            string chunk = string.Empty;
            char[] charArray = line.ToCharArray();
            bool isLetter = Char.IsLetterOrDigit(charArray[0]);
            bool isBracket = false;
            bool isOpenQuote = false;
            int wordStart = 0;
            int wordEnd = 1;
            for (; wordEnd < charArray.Length; ++wordEnd)
            {
                char nextChar = charArray[wordEnd];
                bool nextIsBracket = nextChar == '(' || nextChar == ')';
                bool nextIsLetter = Char.IsLetterOrDigit(nextChar) || nextChar == '_';
                bool nextIsQuote = nextChar == '\"';
                bool nextIsSpace = nextChar == ' ';
                if (nextIsQuote)
                {
                    isOpenQuote = !isOpenQuote;
                }
                if (!isOpenQuote)
                {
                    bool letterSwitch = isLetter != nextIsLetter;
                    bool nonletterSpace = !isLetter && nextIsSpace;
                    if (isBracket || nextIsBracket || letterSwitch || nonletterSpace)
                    {
                        isLetter = nextIsLetter;
                        chunk = line.Substring(wordStart, wordEnd - wordStart).Trim();
                        if (!string.IsNullOrEmpty(chunk))
                        {
                            stringChunks.Add(chunk);
                        }
                        wordStart = wordEnd;
                    }
                }
                isBracket = nextIsBracket;
            }
            chunk = line.Substring(wordStart, wordEnd - wordStart).Trim();
            if (!string.IsNullOrEmpty(chunk))
            {
                stringChunks.Add(chunk);
            }
            return stringChunks;
        }


        public static Formula ChunksIntoFormula(Context ctx, List<string> stringChunks)
        {
            Formula resultFormula;
            List<Element> elementList = new List<Element>();
            resultFormula = new Formula();
            for (int i = 0; i < stringChunks.Count; ++i)
            {
                string str = stringChunks[i];        
                if (str.StartsWith("\""))
                {
                    continue;
                }
                str = str.ToUpper();
                if (RPNConsts.AssignStrings.ContainsKey(str))
                {
                    ElementType etype = RPNConsts.AssignStrings[str];
                    elementList.Add(new Element(etype));
                }
                else if (RPNConsts.OperatorStrings.ContainsKey(str))
                {
                    ElementType etype = RPNConsts.OperatorStrings[str];
                    elementList.Add(new Element(etype));
                }
                else if(ctx.VariableNameDict.ContainsKey(str))
                {
                    int varIndex = ctx.VariableNameDict[str];
                    int registerPlace = registerPlace = resultFormula.AddBinding(varIndex);
                    elementList.Add(new Element(Formula.RegisterFromInt(registerPlace)));
                }
                else
                {
                    int number = 0;
                    bool success = int.TryParse(str, out number);
                    if (success)
                    {
                        elementList.Add(new Element(number));
                    }
                    else 
                    {
                        throw new Exception("What is this, "+str);
                    }
                }
            }

            resultFormula.AddElementList(elementList);
            if(resultFormula.IsMismatched())
            {
                throw new Exception("Mismatched, "+resultFormula.ToLongString(ctx));
            }
            return resultFormula;
        }


    }
}