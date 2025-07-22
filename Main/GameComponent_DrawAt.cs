using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace MakaiTechPsycast.Main
{
    public class GameComponent_DrawAt : GameComponent
    {
        public GameComponent_DrawAt(Game game)
        {
        }

        public List<Pawn> animatedPawns = new List<Pawn>();

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Collections.Look(ref animatedPawns, "animatedPawns",LookMode.Reference);
        }
        public void AddToList(Pawn pawn)
        {
            if(!animatedPawns.Contains(pawn))
            {
                animatedPawns.Add(pawn);
            }
        }

        public void RemoveFromList(Pawn pawn)
        {
            if(animatedPawns.Contains(pawn))
            {
                animatedPawns.Remove(pawn);
            }
        }
    }
}
