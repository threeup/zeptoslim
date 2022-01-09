
using Raylib_cs;
using TacticGame;
using System.Text;

namespace TacticGameRay;

public static class RayTextRender
{
  public static int radius = 4;
  public static void Draw(World w, Human h, int x, int y, Font? font)
  {
    int row = 0;

    string humanAttributes = h.GetAttributesString();


    Raylib.DrawText("Hero, the hero: " + humanAttributes, 12, 12, 50, Color.BLACK);


    for (int yy = y + radius; yy >= y - radius; --yy)
    {
      StringBuilder line = new StringBuilder();
      for (int xx = x - radius; xx <= x + radius; ++xx)
      {
        IRenderable? obj = null;
        if (obj == null)
        {
          obj = w.GetPawn(xx, yy);
        }
        if (obj == null)
        {
          obj = w.GetProp(xx, yy);
        }
        if (obj == null)
        {
          obj = w.GetTile(xx, yy);
        }
        if (obj != null)
        {
          line.Append(obj.GetChar());
        }
        else
        {
          line.Append(Consts.ALMOST_EQUAL);
        }
      }
      if (row >= 2 && row <= 6)
      {
        int itx = row - 2;
        if (h.cards.Count > itx)
        {
          line.Append("  [" + (itx + 1) + "] ");
          line.Append(h.cards[itx].Name);
        }
      }
      if (font.HasValue)
      {
        Raylib.DrawTextEx(font.Value, line.ToString(), new System.Numerics.Vector2(30, 60 + row * 30), 22, 1.0f, Color.BLACK);
      }
      else
      {
        Raylib.DrawText(line.ToString(), 12, 60 + row * 30, 30, Color.BLACK);
      }
      row++;
    }

  }
}
