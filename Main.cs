
using Raylib_cs;
using static Raylib_cs.Raylib;
using static Raylib_cs.KeyboardKey;
using TacticGame;

namespace TacticGameRay;

public class Main
{
  public static CardManager? cardManager = null;
  public static World? world = null;
  public static Human? human = null;
  public Main()
  {
    InitWindow(1600, 1200, "Zepto");
    Font fontTtf = LoadFontEx("resources/recmonoduotone.ttf", 32, null, 250);
    
    cardManager = new CardManager();
    cardManager.Populate("TacticData", "actions.csv");
    world = new World();
    world.Generate(10, 10);
    human = new Human();
    human.Setup();
    Hero hero = new Hero();
    hero.Setup("TacticData", "StartRules.csv", "TileRules.csv", "EndRules.csv");
    human.Possess(hero);
    world.AddPawn(hero);
    Prop prop = new Prop();
    prop.position.Set(3, 3, 0);
    world.AddProp(prop);
    int camX = 0;
    int camY = 0;
    int? moveX = 0;
    int? moveY = 0;
    Slot? slotKey = null;

    while (!WindowShouldClose())
    {
      moveX = null;
      moveY = null;
      slotKey = null;
      // Update
      //----------------------------------------------------------------------------------

      if (IsKeyPressed(KEY_RIGHT)) { moveX = 1; }
      if (IsKeyPressed(KEY_LEFT)) { moveX = -1; }
      if (IsKeyPressed(KEY_UP)) { moveY = 1; }
      if (IsKeyPressed(KEY_DOWN)) { moveY = -1; }
      if (IsKeyPressed(KEY_SPACE)) { moveX = 0; moveY = 0; }
      if (IsKeyPressed(KEY_ONE)) { slotKey = Slot.One; }
      if (IsKeyPressed(KEY_TWO)) { slotKey = Slot.Two; }
      if (IsKeyPressed(KEY_THREE)) { slotKey = Slot.Three; }
      if (IsKeyPressed(KEY_FOUR)) { slotKey = Slot.Four; }
      if (IsKeyPressed(KEY_FIVE)) { slotKey = Slot.Five; }
      if (IsKeyPressed(KEY_ENTER)) { slotKey = Slot.Cycle; }


      //----------------------------------------------------------------------------------

      // Draw
      //----------------------------------------------------------------------------------
      Raylib.BeginDrawing();
      Raylib.ClearBackground(Color.WHITE);

      RayTextRender.Draw(world, human, camX, camY, fontTtf);

      Raylib.EndDrawing();

      //----------------------------------------------------------------------------------

      if (slotKey.HasValue)
      {
        human.SelectSlot(slotKey.Value);
      }
      else if (moveX.HasValue || moveY.HasValue)
      {
        human.Move(moveX ?? 0, moveY ?? 0);
        human.Tick();
        if (human.pawn != null)
        {
          world.MovePawn(human.pawn);
          camX = human.pawn.position.X;
          camY = human.pawn.position.Y;
        }
      }
      else
      {
        human.Tick();
      }
    }

    Raylib.CloseWindow();
  }
}