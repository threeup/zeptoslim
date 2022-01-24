

namespace ZeptoCommon;

    public static class Parser
    {
        
        public static string Sanitize(string line, out int depth)
        {
            int indexOfComment = line.IndexOf("//");
            if (indexOfComment != -1)
            {
                line = line.Substring(0, indexOfComment);
            }
            line = line.TrimEnd();
            int fullLen = line.Length;
            string trimmedLine = line.ToUpper().TrimStart();
            depth = fullLen - trimmedLine.Length;
            return trimmedLine;
        }
        

        public static List<string> CommaSeparatedIntoChunks(string line)
        {
            List<string> stringChunks = new List<string>();
            if (line.Length == 0)
            {
                return stringChunks;
            }
            string chunk = string.Empty;
            char[] charArray = line.ToCharArray();
            int cursorStart = 0;
            int cursor = 0;
            for (cursor = 0; cursor < charArray.Length; ++cursor)
            {
                char nextChar = charArray[cursor];
                bool isComma = nextChar == ',';
                bool isSpace = Char.IsWhiteSpace(nextChar);
                if (isSpace || isComma)
                {
                    int chunkLength = cursor - cursorStart;
                    if(chunkLength > 0)
                    {
                        chunk = line.Substring(cursorStart, chunkLength);
                        stringChunks.Add(chunk);
                    }
                    cursorStart = cursor+1;
                }
                else if (!Char.IsLetterOrDigit(nextChar))
                {
                    throw new Exception("Expected commas spaces alpha numeric "+line);
                }
            }
            chunk = line.Substring(cursorStart, cursor - cursorStart).Trim();
            if (!string.IsNullOrEmpty(chunk))
            {
                stringChunks.Add(chunk);
            }
            
            return stringChunks;
        }

        public static void StringIntoChunks(string line, ref List<string> stringChunks)
        {
            stringChunks.Clear();
            if (line.Length == 0)
            {
                return;
            }

            string chunk = string.Empty;
            char[] charArray = line.ToCharArray();
            int cursorStart = 0;
            int cursor = 0;
            char nextChar = charArray[cursor];
            bool isLetter = Char.IsLetterOrDigit(nextChar);
            bool isBracket = false;
            bool isOpenQuote = false;
            for (cursor = 1; cursor < charArray.Length; ++cursor)
            {
                nextChar = charArray[cursor];
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
                        chunk = line.Substring(cursorStart, cursor - cursorStart).Trim();
                        if (!string.IsNullOrEmpty(chunk))
                        {
                            stringChunks.Add(chunk);
                        }
                        cursorStart = cursor;
                    }
                }
                isBracket = nextIsBracket;
            }
            chunk = line.Substring(cursorStart, cursor - cursorStart).Trim();
            if (!string.IsNullOrEmpty(chunk))
            {
                stringChunks.Add(chunk);
            }
            
            return;
        }

    }
