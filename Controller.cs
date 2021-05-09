using System.Collections.Generic;

namespace zeptolib
{
    public class Controller
    {
        public Pawn pawn;
        public List<Item> items = new List<Item>();

        public void Possess(Pawn p_pawn) {
            if(pawn != null) {
                pawn.controlled = false;
            }
            if(p_pawn != null) {
                pawn = p_pawn;
                pawn.controlled = true;
            }
        }
        public virtual void Tick()
        {
            if (pawn != null) {
                
                pawn.Tick();
            }
        }
    }
}