namespace zeptolib
{
    public struct Vec3
    {
        public static readonly Vec3 Zero = new Vec3(0, 0, 0);
        public static readonly Vec3 One = new Vec3(1, 1, 1);
        public static readonly Vec3 Forward = new Vec3(1, 0, 0);//length
        public static readonly Vec3 Left = new Vec3(0, 1, 0);//width
        public static readonly Vec3 Up = new Vec3(0, 0, 1);//height
        int x;
        int y;
        int z;

        public int X { get { return x; } }
        public int Y { get { return y; } }
        public int Z { get { return z; } }


        public Vec3(int p_x, int p_y, int p_z)
        {
            x = p_x;
            y = p_y;
            z = p_z;
        }
        public void Set(int p_x, int p_y, int p_z)
        {
            x = p_x;
            y = p_y;
            z = p_z;
        }
        public void SetX(int val) { x = val; }
        public void SetY(int val) { y = val; }
        public void SetZ(int val) { z = val; }
        public void MoveX(int val) { x += val; }
        public void MoveY(int val) { y += val; }
        public void MoveZ(int val) { z += val; }

        public static Vec3 operator+(Vec3 lhs, Vec3 rhs)
        {
            return new Vec3(lhs.X + rhs.X, lhs.Y + rhs.Y, lhs.Z + rhs.Z);
        }
        public static Vec3 operator-(Vec3 lhs, Vec3 rhs)
        {
            return new Vec3(lhs.X - rhs.X, lhs.Y - rhs.Y, lhs.Z - rhs.Z);
        }
        public static Vec3 operator*(int val, Vec3 rhs)
        {
            return new Vec3(val* rhs.X, val* rhs.Y, val* rhs.Z);
        }
    }
}