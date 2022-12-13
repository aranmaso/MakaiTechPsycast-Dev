using System;
using RimWorld;
using Verse;
using Verse.AI;
using UnityEngine;
using VFECore;

namespace MakaiTechPsycast.BondIntertwined
{
	public class CompBondingTower : ThingComp
	{
		private int nextTest = 0;

		private int pawnCount = 0;

		JobDef currentJob = JobDefOf.FleeAndCower;

		[MayRequireIdeology]
		JobDef danceJob = JobDefOf.Dance;

		

		public CompProperties_BondingTower Props => (CompProperties_BondingTower)props;

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
			if(this.Props != null)
            {
				if (Find.TickManager.TicksGame != nextTest)
				{
					return;
				}
				foreach (Pawn pawn in MakaiUtility.GetNearbyPawnFriendAndFoe(parent.Position, parent.Map, Props.radius))
				{
					foreach (HediffDef hediffDef in Props.hediff)
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
							if (pawn.health.hediffSet.HasHediff(hediffDef) && num > 0f && hediffDef.initialSeverity > 0f)
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
					if (pawn.HostileTo(Faction.OfPlayer) || (pawn.HostileTo(Faction.OfPlayer) && pawn.AnimalOrWildMan()) && (pawn.Faction != Faction.OfMechanoids || pawn.Faction != Faction.OfInsects))
					{
						float factionRelationMend = Rand.Value;
						if (!(pawn.RaceProps.Animal) && pawn.Faction.HasGoodwill && factionRelationMend <= 0.1f && pawn.Faction != null)
						{
							Faction.OfPlayer.TryAffectGoodwillWith(pawn.Faction, 2);
						}
						if (danceJob != null)
						{
							currentJob = danceJob;
						}
						float danceChance = Rand.Value;
						if (pawn.CurJob.def != currentJob && danceChance <= 0.3f && !pawn.Downed)
						{
							pawn.jobs.StartJob(JobMaker.MakeJob(currentJob), JobCondition.InterruptForced, null, resumeCurJobAfterwards: false);
						}
						/*if (pawn.CurJob.def == JobDefOf.Dance)
						{
							pawn.jobs.EndCurrentJob(JobCondition.InterruptForced);
						}*/

					}
				}
				nextTest += Props.tickRate;
			}
		}
	}
}
