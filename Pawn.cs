using System.Collections.Generic;

namespace zeptolib
{
    public class Pawn : IRenderable
    {

        public Dictionary<string, int> attributes = new Dictionary<string, int>();
        public bool controlled = false;
        public Vec3 lastPosition = Vec3.Zero;
        public Vec3 position = Vec3.Zero;
        public string c = Consts.POINTY_CIRCLE;

        public string GetChar() { return c; }

        public void Setup()
        {
            attributes.Add(Attribs.X, 1);
            attributes.Add(Attribs.Y, 1);
            attributes.Add(Attribs.HP, 10);
        }

        public void Tick()
        {

        }

        public void SetAttrib(string key, int amount)
        {
            if (attributes.ContainsKey(key))
            {
                attributes[key] = amount;
            }
            else
            {
                attributes.Add(key, amount);
            }
        }

        public void ModifyAttrib(string key, int amount)
        {
            if (attributes.ContainsKey(key))
            {
                attributes[key] += amount;
            }
            else
            {
                attributes.Add(key, amount);
            }
        }

        public void Move()
        {
            lastPosition = position;
            position.SetX(attributes[Attribs.X]);
            position.SetY(attributes[Attribs.Y]);
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