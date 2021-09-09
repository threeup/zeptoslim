namespace zeptolib
{
    public class Pawn : IRenderable
    {
        public bool controlled = false;
        public Vec3 lastPosition = Vec3.Zero;
        public Vec3 position = Vec3.Zero;
        public string c = Consts.POINTY_CIRCLE;

        public string GetChar() { return c; }

        public void Tick()
        {
            
        }

        public void Move(int moveX, int moveY)
        {
            lastPosition = position;
            position.MoveX(moveX);
            position.MoveY(moveY);
        }

        public void CollidePawn(Pawn other)
        {
            position = lastPosition;
        }
        public void CollideProp(Prop other)
        {
            //ok
        }
    }
}