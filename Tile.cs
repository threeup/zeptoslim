namespace zeptolib
{
    public class Tile : IRenderable
    {

        public char c;
        public Vec3 position;
        
        public char GetChar() { return c; }
        public Tile(char p_c) {
            c = p_c;
        }
    }
}