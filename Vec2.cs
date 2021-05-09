namespace zeptolib
{
    public struct Vec2
    {
        public static readonly Vec2 Zero = new Vec2(0, 0);
        public static readonly Vec2 One = new Vec2(1, 1);
        public static readonly Vec2 Forward = new Vec2(1, 0);//length
        public static readonly Vec2 Left = new Vec2(0, 1);//width
        int x;
        int y;

        public int X { get { return x; } }
        public int Y { get { return y; } }
        public int Z { get { return 0; } }

        public Vec2(int p_x, int p_y)
        {
            x = p_x;
            y = p_y;
        }
        public void Set(int p_x, int p_y)
        {
            x = p_x;
            y = p_y;
        }
        public void SetX(int val) { x = val; }
        public void SetY(int val) { y = val; }
        public void MoveX(int val) { x += val; }
        public void MoveY(int val) { y += val; }
        
        public static Vec2 operator+(Vec2 lhs, Vec2 rhs)
        {
            return new Vec2(lhs.X + rhs.X, lhs.Y + rhs.Y);
        }
        public static Vec2 operator-(Vec2 lhs, Vec2 rhs)
        {
            return new Vec2(lhs.X + rhs.X, lhs.Y + rhs.Y);
        }
    }
}