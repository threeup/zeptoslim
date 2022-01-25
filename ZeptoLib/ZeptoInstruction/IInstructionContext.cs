namespace ZeptoInstruction;

    public interface IInstructionContext
    {
        void AddMethodName(string methodName);
        bool ContainsMethodName(string methodName);
        public void SetMethodPtr(string methodName, Func<int, int> f);
        System.Func<int, int> GetMethodPtr(string methodName);
        string ToLongString();
    }
