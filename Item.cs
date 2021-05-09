namespace zeptolib
{
    public class Item
    {
        public string Name { get; private set; }
        public int Count { get; protected set; }

        public Item(string p_name, int p_count)
        {
            Name = p_name;
            Count = p_count;
        }
    }
}