using System;
using System.Collections.Generic;

namespace zeptolib
{
    public class Program
    {
        public static World world;
        public static Human human;

        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            world = new World();
            world.Generate(10, 10);
            human = new Human();
            Hero hero = new Hero();
            human.Possess(hero);
            world.AddPawn(hero);
            Prop prop = new Prop();
            prop.position.Set(3, 3, 0);
            world.AddProp(prop);
            ConsoleKeyInfo val = default(ConsoleKeyInfo);
            int camX = 0;
            int camY = 0;
            Action.Slot? inputKey = null;
            while (val.Key != ConsoleKey.Escape)
            {
                Console.Clear();
                ConsoleRenderer.Draw(world, human, camX, camY);
                val = Console.ReadKey();
                inputKey = null;
                switch (val.Key)
                {
                    case ConsoleKey.UpArrow: camY += 1; break;
                    case ConsoleKey.DownArrow: camY -= 1; break;
                    case ConsoleKey.LeftArrow: camX -= 1; break;
                    case ConsoleKey.RightArrow: camX += 1; break;
                    case ConsoleKey.D1: inputKey = Action.Slot.One; break;
                    case ConsoleKey.D2: inputKey = Action.Slot.Two; break;
                    case ConsoleKey.D3: inputKey = Action.Slot.Three; break;
                    case ConsoleKey.D4: inputKey = Action.Slot.Four; break;
                    case ConsoleKey.D5: inputKey = Action.Slot.Five; break;
                    case ConsoleKey.Enter: inputKey = Action.Slot.Cycle; break;
                    default: break;
                }
                if (inputKey.HasValue)
                {
                    human.DoAction(inputKey.Value);
                    human.Tick();
                    camX = human.pawn.position.X;
                    camY = human.pawn.position.Y;
                }
                else
                {
                    human.Tick();
                }
            }
        }
    }
}
