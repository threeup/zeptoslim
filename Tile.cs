namespace zeptolib
{
    public class Tile : IRenderable
    {

        public string c;
        public Vec3 position;
        
        public string GetChar() { return c; }
        public Tile(string p_c) {
            c = p_c;
        }
    }
}