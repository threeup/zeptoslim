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

        public List<Action> tileActions;
        public List<Condition> endConditions;

        public string GetChar() { return c; }

        public void Setup(string startFile, string tileFile, string endFile)
        {
            RuleLib.LoadStart(this, startFile);
            RuleLib.LoadTile(this, tileFile);
            RuleLib.LoadEnd(this, endFile);
        }

        public void Tick()
        {

        }
        public int GetAttrib(string key)
        {
            if (attributes.ContainsKey(key))
            {
                return attributes[key];
            }
            return 0;
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