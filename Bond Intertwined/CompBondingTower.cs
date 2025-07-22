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
	public class CompBondingTower : ThingComp
	{
	
		public CompProperties_BondingTower Props => (CompProperties_BondingTower)props;

		public void DoTrigger()
		{
			IReadOnlyList<Pawn> pawns = MakaiUtility.GetNearbyPawnFriendAndFoe(parent.PositionHeld, parent.MapHeld, Props.radius).ToList();
			if(!pawns.EnumerableNullOrEmpty())
			{
				foreach(var item in  pawns)
				{
					if(!Props.hediff.NullOrEmpty())
					{
						float severity = Props.severityAmount;
                        float dur = Props.durationInHour * 2500f;
						if(!Props.stats.NullOrEmpty())
						{
							foreach(var statDef in Props.stats)
							{
								severity *= item.GetStatValueForPawn(statDef,item);
								dur *= item.GetStatValueForPawn(statDef,item);
							}
						}

                        foreach (var hediffDef in Props.hediff)
						{
                            if (item.Faction == Faction.OfPlayer || !item.HostileTo(parent.Faction))
                            {
                                Hediff hediff = item.health.hediffSet.GetFirstHediffOfDef(hediffDef);
								if(hediff != null)
								{
									hediff.Severity += severity;
									hediff.TryGetComp<HediffComp_Disappears>().ticksToDisappear = Mathf.RoundToInt(dur);
                                }
								else
								{
                                    Hediff hediff2 = HediffMaker.MakeHediff(hediffDef, item);
                                    hediff2.TryGetComp<HediffComp_Disappears>().ticksToDisappear = Mathf.RoundToInt(dur);
                                    item.health.AddHediff(hediff2);
                                }
                            }
                        }
						
					}
					if (item.HostileTo(parent)
						|| item.HostileTo(parent.Faction)
						|| item.AnimalOrWildMan() && item.HostileTo(parent.Faction)
						|| item.RaceProps.Animal && item.mindState.mentalStateHandler.CurStateDef == MentalStateDefOf.Manhunter
						|| item.mindState.mentalStateHandler.InMentalState && item.mindState.mentalStateHandler.CurStateDef == MentalStateDefOf.Berserk)
					{
                        float factionRelationMend = Rand.Value;
                        if (!item.RaceProps.Animal && item.Faction.HasGoodwill && factionRelationMend <= 0.1f && item.Faction != null)
                        {
							if(item.Faction != Faction.OfPlayer)
							{
                                Faction.OfPlayer.TryAffectGoodwillWith(item.Faction, 2);
                            }
                        }
						if(Rand.Chance(0.3f))
						{
							if(ModsConfig.IdeologyActive)
							{
                                if (item.CurJob.def != JobDefOf.Dance && !item.Downed)
                                {
									item.pather.StopDead();
									item.jobs.StopAll();
                                    item.jobs.StartJob(JobMaker.MakeJob(JobDefOf.Dance), JobCondition.InterruptForced, null, resumeCurJobAfterwards: false);
                                }
                            }    
							else
							{
                                if (item.CurJob.def != JobDefOf.FleeAndCower && !item.Downed)
                                {
                                    item.pather.StopDead();
                                    item.jobs.StopAll();
                                    item.jobs.StartJob(JobMaker.MakeJob(JobDefOf.FleeAndCower), JobCondition.InterruptForced, null, resumeCurJobAfterwards: false);
                                }
                            }
                        }
                    }
				}
			}
		}
		public override void CompTick()
		{
			if(parent.IsHashIntervalTick(Props.tickRate))
			{
				DoTrigger();
			}			
		}
	}
}
