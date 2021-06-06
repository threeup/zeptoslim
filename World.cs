using System.Collections.Generic;

namespace zeptolib
{
    public class World
    {

        public Dictionary<Vec2, Tile> flatmap = new Dictionary<Vec2, Tile>();
        public Dictionary<Vec3, Tile> tilemap = new Dictionary<Vec3, Tile>();
        public Dictionary<Vec3, Prop> propmap = new Dictionary<Vec3, Prop>();
        public Dictionary<Vec3, Pawn> pawnmap = new Dictionary<Vec3, Pawn>();

        public int l;
        public int w;

        public void Generate(int p_l, int p_w)
        {
            l = p_l;
            w = p_w;
            for (int y = 0; y < w; ++y)
            {
                for (int x = 0; x < l; ++x)
                {
                    AddTile(x, y, 0, Consts.MIDDLE_DOT);
                }
            }
        }


        public bool IsInBounds(int x, int y)
        {
            Vec2 vec = new Vec2(x, y);
            return flatmap.ContainsKey(vec);
        }


        public int ToIdx(int x, int y)
        {
            return x * w + y;
        }

        public void AddTile(int x, int y, int z, string c)
        {
            int idx = ToIdx(x, y);
            Tile t = new Tile(c);
            Vec3 realvec = new Vec3(x, y, z);
            Vec2 flatvec = new Vec2(x, y);
            t.position = realvec;
            tilemap.Add(realvec, t);
            if (!flatmap.ContainsKey(flatvec))
            {
                flatmap.Add(flatvec, t);
            }
        }

        public void AddPawn(Pawn obj)
        {
            if(pawnmap.ContainsKey(obj.position))
            {
                return;
            }
            pawnmap.Add(obj.position, obj);
        }

        

        public void AddProp(Prop obj)
        {
            if(propmap.ContainsKey(obj.position))
            {
                return;
            }
            propmap.Add(obj.position, obj);
        }


        public Tile GetTile(int x, int y)
        {
            Vec2 flatvec = new Vec2(x, y);
            if (flatmap.ContainsKey(flatvec))
            {
                return flatmap[flatvec];
            }
            return null;
        }



        public Pawn GetPawn(int x, int y)
        {
            for (int z = 10; z >= 0; --z)
            {
                Vec3 realvec = new Vec3(x, y, z);
                if (pawnmap.ContainsKey(realvec))
                {
                    return pawnmap[realvec];
                }
            }
            return null;
        }



        public Prop GetProp(int x, int y)
        {
            for (int z = 10; z >= 0; --z)
            {
                Vec3 realvec = new Vec3(x, y, z);
                if (propmap.ContainsKey(realvec))
                {
                    return propmap[realvec];
                }
            }
            return null;
        }

    }
}