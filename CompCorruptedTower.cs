using System;
using RimWorld;
using Verse;
using Verse.AI;
using UnityEngine;
using VFECore;

namespace MakaiTechPsycast.CorruptedProphet
{
	public class CompCorruptedTower : ThingComp
	{
		private int nextTest = 0;

		private int pawnCount = 0;
		public CompProperties_CorruptedTower Props => (CompProperties_CorruptedTower)props;

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
			if (this.Props != null)
			{
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
					if (pawn.HostileTo(Faction.OfPlayer) || (pawn.HostileTo(Faction.OfPlayer) && pawn.AnimalOrWildMan()) && (pawn.Faction != Faction.OfMechanoids || pawn.Faction != Faction.OfInsects))
					{
						float rand = Rand.Value;
						if(rand <0.5f)
                        {
							pawn.mindState.mentalStateHandler.TryStartMentalState(MentalStateDefOf.Berserk);
							Effecter effect = MakaiTechPsy_DefOf.MakaiPsy_Ring_ExpandY.Spawn(pawn.Position, pawn.Map, 0.5f);
							effect.Cleanup();
						}
						else
                        {
							pawn.mindState.mentalStateHandler.TryStartMentalState(MentalStateDefOf.Wander_Psychotic);
							Effecter effect = MakaiTechPsy_DefOf.MakaiPsy_Ring_ExpandY.Spawn(pawn.Position, pawn.Map, 0.5f);
							effect.Cleanup();
						}
						pawnCount++;
						if (pawnCount >= (Props.targetCount + 1))
						{
							break;
						}
					}
				}
				pawnCount = 0;
				nextTest += Props.tickRate;
			}
		}
	}
}
