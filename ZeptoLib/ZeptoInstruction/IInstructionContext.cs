namespace ZeptoInstruction;

    public interface IInstructionContext
    {
        void AddVerbName(string verbName);
        bool ContainsVerbName(string verbName);
        System.Func<int, int> GetVerbAction(string verbName);
        string ToLongString();
    }
