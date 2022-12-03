using System.Collections.Generic;
using System.Linq;
using RimWorld;
using UnityEngine;
using Verse;

namespace MakaiTechPsycast.DestinedDeath
{
    public class HediffComp_Decay : HediffComp
    {
        public int tickSinceTrigger;
        public HediffCompProperties_Decay Props => (HediffCompProperties_Decay)props;
        public override void CompExposeData()
        {
            Scribe_Values.Look(ref tickSinceTrigger, "tickSinceTrigger", 0);
        }
        public override void CompPostMake()
        {
            tickSinceTrigger = Find.TickManager.TicksGame + Props.interval;
            base.CompPostMake();
        }
        public override void CompPostTick(ref float severityAdjustment)
        {
            if (Pawn.IsHashIntervalTick(Props.interval))
            {
                Pawn pawn = parent.pawn;
                pawn.TakeDamage(new DamageInfo(Props.damageDef, Props.damageAmount, 100));
                tickSinceTrigger += Props.interval;
            }
        }
        public override void Notify_PawnDied()
        {
            List<BodyPartRecord> list = new List<BodyPartRecord>(parent.pawn.RaceProps.body.AllParts.Where((BodyPartRecord part) => parent.pawn.health.hediffSet.PartIsMissing(part)));
            int partCount = 0;
            if(list.Count > 0)
            {
                foreach (BodyPartRecord item in list)
                {
                    partCount++;
                }
                if (partCount > 0)
                {
                    GenExplosion.DoExplosion(parent.pawn.Corpse.Position, parent.pawn.Corpse.Map, 1, DamageDefOf.Flame, null, partCount);
                }
            }
        }
    }
}
