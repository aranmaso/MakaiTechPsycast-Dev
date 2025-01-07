using RimWorld;
using UnityEngine;
using Verse;

namespace MakaiTechPsycast.PassionedFlame
{
    public class HediffComp_FakeSun : HediffComp
    {
        public HediffCompProperties_FakeSun Props => (HediffCompProperties_FakeSun)props;

        public int interval;
        public override void CompPostTick(ref float severityAdjustment)
        {
            if(Pawn.IsHashIntervalTick(interval))
            {
                DoTrigger();
            }
        }

        public override void CompPostMake()
        {
            base.CompPostMake();
            interval = Props.tickInterval;
        }

        public override void CompExposeData()
        {
            Scribe_Values.Look(ref interval, "interval",0);
        }
        public void DoTrigger()
        {
            foreach(Pawn item in MakaiUtility.GetNearbyPawnFriendAndFoe(Pawn.Position,Pawn.Map,5f))
            {
                if (MakaiUtility.GetPawnIsHostileToPlayer(item))
                {
                    DoTriggerHostile(item,Props.projectile);
                }
                else
                {
                    DoTriggerFriendly(item);
                }
            }
        }
        public void DoTriggerFriendly(Pawn pawn)
        {
            if (pawn.health.hediffSet.HasHediff(Props.hediffDef))
            {
                MakaiUtility.GetFirstHediffOfDef(pawn,Props.hediffDef).TryGetComp<HediffComp_Disappears>().ticksToDisappear = Props.durationTick;
            }
            else
            {
                Hediff hediff = MakaiUtility.CreateCustomHediffWithDurationNoMultiplier(pawn, Props.hediffDef, 0, Props.durationTick);
                pawn.health.AddHediff(hediff);
            }
        }
        public void DoTriggerHostile(Pawn pawn,ThingDef thing)
        {
            Vector3 intvect = Pawn.Position.ToVector3Shifted();
            Vector3 intvecOffset = new Vector3(intvect.x, intvect.y, intvect.z + 7.5f);
            Projectile projectile = (Projectile)GenSpawn.Spawn(thing, Pawn.Position, Pawn.Map);
            projectile.Launch(Pawn, intvecOffset,pawn,pawn,ProjectileHitFlags.IntendedTarget);
            if (MakaiUtility.GetFirstHediffOfDef(pawn,HediffDefOf.Heatstroke) != null)
            {
                pawn.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.Heatstroke).Severity += Rand.Range(0.01f, 0.1f);
            }            
            else
            {
                Hediff hediff = HediffMaker.MakeHediff(HediffDefOf.Heatstroke, pawn);
                hediff.Severity = Rand.Range(0.01f, 0.1f);
                pawn.health.AddHediff(hediff);
            }
        }
    }
}
