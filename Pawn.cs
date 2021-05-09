namespace zeptolib
{
    public class Pawn : IRenderable
    {
        public bool controlled = false;
        public Vec3 position = Vec3.Zero;
        public char c ='P';

        public char GetChar() { return c; }

        public void Tick()
        {
            position.MoveX(1);
        }
    }
}