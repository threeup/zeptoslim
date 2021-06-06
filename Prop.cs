namespace zeptolib
{
    public class Prop : IRenderable
    {
        public string c = Consts.DOUBLE_DAGGER;
        public Vec3 position = Vec3.Zero;
        
        public string GetChar() { return c; }
    }
}