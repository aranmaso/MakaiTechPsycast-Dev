using System;
using RimWorld;
using Verse;
using Verse.AI;
using UnityEngine;
using VEF;
using System.Collections.Generic;

namespace MakaiTechPsycast.CorruptedProphet
{
	public class CompCorruptedTower : ThingComp
	{
		public CompProperties_CorruptedTower Props => (CompProperties_CorruptedTower)props;

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
			IReadOnlyList<Pawn> pawns = new List<Pawn>(MakaiUtility.GetNearbyPawnFriendAndFoe(parent.Position, parent.Map, Props.radius));
			if(!pawns.EnumerableNullOrEmpty())
			{
				int count = 0;
                foreach (var pawn in pawns)
                {
                    if (pawn.HostileTo(Faction.OfPlayer) || (pawn.HostileTo(Faction.OfPlayer) && pawn.AnimalOrWildMan()) && (pawn.Faction != Faction.OfMechanoids || pawn.Faction != Faction.OfInsects))
                    {
                        if (Rand.Value <= 0.5f)
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
                        count++;
                        if (count >= (Props.targetCount + 1))
                        {
                            break;
                        }
                    }
                }
            }
		}
	}
}
