using RimWorld;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace MakaiTechPsycast.TrueDestruction
{
    public class HediffComp_BlazingTrail : HediffComp
    {

        public int interval;
        public HediffCompProperties_BlazingTrail Props => (HediffCompProperties_BlazingTrail)props;

        public override void CompExposeData()
        {
            Scribe_Values.Look(ref interval, "interval", 0);
        }
        public override void CompPostTick(ref float severityAdjustment)
        {
            base.CompPostTick(ref severityAdjustment);
            interval++;
            if (interval >= 10)
            {
                if (Pawn.jobs.curJob.def == JobDefOf.Goto
                || Pawn.jobs.curJob.def == JobDefOf.Follow
                || Pawn.jobs.curJob.def == JobDefOf.GotoWander
                || Pawn.jobs.curJob.def == JobDefOf.Flee
                || Pawn.jobs.curJob.def == JobDefOf.AttackMelee)
                {
                    if(Pawn.Position.GetFirstThing(Pawn.Map,ThingDefOf.Fire) == null)
                    {
                        Fire obj = (Fire)ThingMaker.MakeThing(ThingDefOf.Fire);
                        obj.fireSize = 1f;
                        GenSpawn.Spawn(obj, Pawn.Position, Pawn.Map, Rot4.North);
                    }
                    foreach(Pawn hostile in MakaiUtility.GetNearbyPawnFoeOnly(Pawn.Position,Pawn.Faction,Pawn.Map,1f))
                    {
                        hostile.TryAttachFire(1f);
                    }
                }
                interval = 0;
            }            
        }

        public override IEnumerable<Gizmo> CompGetGizmos()
        {
            yield return new Command_Action
            {
                defaultLabel = "extinguish the trail",
                defaultDesc = "put out the flame on your feet",
                icon = ContentFinder<Texture2D>.Get(Props.uiIcon),
                action = delegate
                {
                    Pawn.health.RemoveHediff(parent);
                }
            };
        }
    }
}
