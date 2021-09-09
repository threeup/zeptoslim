using System;
using System.Collections.Generic;

namespace zeptolib
{
    public class Program
    {
        public static CardManager cardManager;
        public static World world;
        public static Human human;

        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            cardManager = new CardManager();
            cardManager.Populate("actions.csv");
            world = new World();
            world.Generate(10, 10);
            human = new Human();
            human.Setup();
            Hero hero = new Hero();
            human.Possess(hero);
            world.AddPawn(hero);
            Prop prop = new Prop();
            prop.position.Set(3, 3, 0);
            world.AddProp(prop);
            ConsoleKeyInfo val = default(ConsoleKeyInfo);
            int camX = 0;
            int camY = 0;
            int moveX = 0;
            int moveY = 0;
            Slot? slotKey = null;
            while (val.Key != ConsoleKey.Escape)
            {
                Console.Clear();
                ConsoleRenderer.Draw(world, human, camX, camY);
                val = Console.ReadKey();
                moveX = 0;
                moveY = 0;
                slotKey = null;
                switch (val.Key)
                {
                    case ConsoleKey.UpArrow: moveY = 1; break;
                    case ConsoleKey.DownArrow: moveY = -1; break;
                    case ConsoleKey.LeftArrow: moveX = -1; break;
                    case ConsoleKey.RightArrow: moveX = 1; break;
                    case ConsoleKey.D1: slotKey = Slot.One; break;
                    case ConsoleKey.D2: slotKey = Slot.Two; break;
                    case ConsoleKey.D3: slotKey = Slot.Three; break;
                    case ConsoleKey.D4: slotKey = Slot.Four; break;
                    case ConsoleKey.D5: slotKey = Slot.Five; break;
                    case ConsoleKey.Enter: slotKey = Slot.Cycle; break;
                    default: break;
                }
                if (slotKey.HasValue)
                {
                    human.SelectSlot(slotKey.Value);
                }
                else if(moveX != 0 || moveY != 0)
                {
                    human.Move(moveX, moveY);
                    human.Tick();
                    world.MovePawn(human.pawn);
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
