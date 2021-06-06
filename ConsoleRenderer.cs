using System;
using System.Text;

namespace zeptolib
{
    public class ConsoleRenderer
    {
        public static int radius = 4;
        public static void Draw(World w, Human h, int x, int y)
        {
            int row=0;
            
            Console.WriteLine("Hero, the hero: X" + x + " Y" + y);
            row++;

            for(int yy=y+radius; yy>=y-radius; --yy)
            {
                StringBuilder line = new StringBuilder();
                for(int xx=x-radius; xx<=x+radius; ++xx)
                {
                    IRenderable obj = null;
                    if(obj == null)
                    {
                        obj = w.GetPawn(xx,yy);
                    }
                    if(obj == null)
                    {
                        obj = w.GetProp(xx,yy);
                    }
                    if(obj == null)
                    {
                        obj = w.GetTile(xx,yy);
                    }
                    if(obj != null) {
                        line.Append(obj.GetChar());
                    }
                    else
                    {
                        line.Append(Consts.ALMOST_EQUAL);
                    }
                }
                if(row >=2 && row <= 6)
                {
                    int itx = row-2;
                    if(h.items.Count > itx)
                    {
                        line.Append("  ["+(itx+1)+"] ");
                        line.Append(h.items[itx].Name);
                    }
                }
                Console.WriteLine(line);
                row++;
            }

        }
    }
}