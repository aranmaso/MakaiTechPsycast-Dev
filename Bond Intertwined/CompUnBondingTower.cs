using System;
using RimWorld;
using Verse;
using Verse.AI;
using UnityEngine;
using VFECore;

namespace MakaiTechPsycast.BondIntertwined
{
	public class CompUnBondingTower : ThingComp
	{
		private int nextTest = 0;

		public CompProperties_UnBondingTower Props => (CompProperties_UnBondingTower)props;

		public override void PostExposeData()
		{
			Scribe_Values.Look(ref nextTest, "nextTest", 0);
			base.PostExposeData();
		}

		public override void PostPostMake()
		{
			nextTest = Find.TickManager.TicksGame + Props.tickRate;
			base.PostPostMake();
		}

		public override void CompTick()
		{
			base.CompTick();
			if (Find.TickManager.TicksGame != nextTest)
			{
				return;
			}
			foreach (Thing item in GenRadial.RadialDistinctThingsAround(parent.Position, parent.Map, Props.radius, useCenter: true))
			{
				if (!(item is Pawn pawn))
				{
					continue;
				}
				foreach (HediffDef hediffDef in this.Props.hediff)
				{
					float num = Props.severityAmount;
					float dur = Props.durationInHour * 2500f;
					if (pawn.Faction == Faction.OfPlayer || !pawn.HostileTo(Faction.OfPlayer))
					{
						if (!Props.stats.NullOrEmpty())
						{
							foreach (StatDef stat in Props.stats)
							{
								num *= pawn.GetStatValue(stat);
								dur *= pawn.GetStatValue(stat);
							}
						}
						if (pawn.health.hediffSet.HasHediff(hediffDef) && num > 0f && hediffDef.initialSeverity != null)
						{
							pawn.health.hediffSet.GetFirstHediffOfDef(hediffDef).Severity += num;
						}
						else
						{
							Hediff hediff = HediffMaker.MakeHediff(hediffDef, pawn);
							hediff.TryGetComp<HediffComp_Disappears>().ticksToDisappear = Mathf.FloorToInt(dur);
							pawn.health.AddHediff(hediff);
						}
					}
				}
				if ((pawn.Faction != Faction.OfPlayer && !pawn.Faction.HostileTo(Faction.OfPlayer)) && (pawn.Faction != Faction.OfMechanoids || pawn.Faction != Faction.OfInsects))
				{
					float factionRelationDamageChance = Rand.Value;
					if (pawn.Faction != null && pawn.Faction.HasGoodwill && factionRelationDamageChance <= 0.2f)
					{
						Faction.OfPlayer.TryAffectGoodwillWith(pawn.Faction, -1);
					}
					float vomit = Rand.Value;
					if (vomit <= 0.2f && !pawn.Downed)
					{
						pawn.jobs.StartJob(JobMaker.MakeJob(JobDefOf.Vomit), JobCondition.InterruptForced, null, resumeCurJobAfterwards: false);
					}
					if (pawn.CurJob.def == JobDefOf.Vomit)
					{
						pawn.jobs.EndCurrentJob(JobCondition.InterruptForced);
					}

				}
			}
			nextTest += Props.tickRate;
		}
	}
}
