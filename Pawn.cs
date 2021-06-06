namespace zeptolib
{
    public class Pawn : IRenderable
    {
        public bool controlled = false;
        public Vec3 position = Vec3.Zero;
        public string c = Consts.POINTY_CIRCLE;

        public string GetChar() { return c; }

        public void Tick()
        {
            position.MoveX(1);
        }
    }
}