using System;
using RimWorld;
using Verse;
using Verse.AI;
using UnityEngine;
using VEF;
using System.Collections.Generic;
using System.Linq;

namespace MakaiTechPsycast.BondIntertwined
{
	public class CompUnBondingTower : ThingComp
	{

		public CompProperties_UnBondingTower Props => (CompProperties_UnBondingTower)props;
        public override void CompTick()
        {
            base.CompTick();
			if(parent.IsHashIntervalTick(Props.tickRate))
			{
				DoTrigger();
            }
        }
        public void DoTrigger()
        {
            IReadOnlyList<Pawn> pawns = new List<Pawn>(MakaiUtility.GetNearbyPawnFriendAndFoe(parent.PositionHeld, parent.MapHeld, Props.radius));
            if (!pawns.EnumerableNullOrEmpty())
            {
                foreach (var pawn in pawns)
                {
                    if (!Props.hediff.NullOrEmpty())
                    {
                        float severity = Props.severityAmount;
                        float dur = Props.durationInHour * 2500f;
                        if (!Props.stats.NullOrEmpty())
                        {
                            foreach (var statDef in Props.stats)
                            {
                                severity *= pawn.GetStatValueForPawn(statDef, pawn);
                                dur *= pawn.GetStatValueForPawn(statDef, pawn);
                            }
                        }

                        foreach (var hediffDef in Props.hediff)
                        {
                            if (pawn.Faction == Faction.OfPlayer || !pawn.HostileTo(parent.Faction))
                            {
                                Hediff hediff = pawn.health.hediffSet.GetFirstHediffOfDef(hediffDef);
                                if (hediff != null)
                                {
                                    hediff.Severity += severity;
                                    hediff.TryGetComp<HediffComp_Disappears>().ticksToDisappear = Mathf.RoundToInt(dur);
                                }
                                else
                                {
                                    Hediff hediff2 = HediffMaker.MakeHediff(hediffDef, pawn);
                                    hediff2.TryGetComp<HediffComp_Disappears>().ticksToDisappear = Mathf.RoundToInt(dur);
                                    pawn.health.AddHediff(hediff2);
                                }
                            }
                        }

                    }
                    if ((pawn.Faction != Faction.OfPlayer && !pawn.Faction.HostileTo(Faction.OfPlayer)) && pawn.Faction.HasGoodwill)
                    {
                        if (pawn.Faction != null && pawn.Faction.HasGoodwill && Rand.Value <= 0.2f)
                        {
                            Faction.OfPlayer.TryAffectGoodwillWith(pawn.Faction, -1);
                        }
                        if (Rand.Value <= 0.2f && !pawn.Downed)
                        {
                            pawn.jobs.StartJob(JobMaker.MakeJob(JobDefOf.Vomit), JobCondition.InterruptForced, null, resumeCurJobAfterwards: false);
                        }
                    }
                }
            }
        }        
	}
}
