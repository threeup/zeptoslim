namespace zeptolib
{
    public class Action
    {
        public enum Slot
        {
            One,
            Two,
            Three,
            Four,
            Five,
            Cycle,
        }
        
        public static bool PawnForward(Pawn pawn)
        {
            pawn.position += Vec3.Forward;
            return true;
        }
        
        public static bool PawnBackward(Pawn pawn)
        {
            pawn.position -= Vec3.Forward;
            return true;
        }
        
        public static bool PawnLeft(Pawn pawn)
        {
            pawn.position += Vec3.Left;
            return true;
        }
        
        public static bool PawnRight(Pawn pawn)
        {
            pawn.position -= Vec3.Left;
            return true;
        }
    }
}