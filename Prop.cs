namespace zeptolib
{
    public class Prop : IRenderable
    {
        public char c = 'P';
        public Vec3 position = Vec3.Zero;
        
        public char GetChar() { return c; }
    }
}