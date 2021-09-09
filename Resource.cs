namespace zeptolib
{
    public class Resource
    {
        public string Name { get; private set; }
        public int Count { get; protected set; }

        public Resource(string p_name, int p_count)
        {
            Name = p_name;
            Count = p_count;
        }
    }
}