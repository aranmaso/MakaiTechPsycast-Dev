using HarmonyLib;
using RimWorld;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using VanillaPsycastsExpanded;
using Verse;
using VFECore.Abilities;

namespace MakaiTechPsycast
{
	[HarmonyPatch(typeof(PawnCapacityUtility), "CalculateCapacityLevel")]
	public static class PawnCapacityUtility_CalculateCapacityLevel_Universal
	{
		public static void Postfix(ref float __result, HediffSet diffSet, PawnCapacityDef capacity, List<PawnCapacityUtility.CapacityImpactor> impactors = null, bool forTradePrice = false)
		{
			if (diffSet.pawn == null)
			{
				return;
			}
			List<float> list = new List<float>();
			foreach(Hediff item in diffSet.pawn.health.hediffSet.hediffs)
            {
				HediffExtension_MinCapacity modExtension = item.def.GetModExtension<HediffExtension_MinCapacity>();
				if(modExtension != null)
                {
					PawnCapacityMinLevel pawnCapacityMinLevel = modExtension.pawnCapacityMinLevels.FirstOrDefault((PawnCapacityMinLevel x) => x.capacity == capacity);
					if (pawnCapacityMinLevel != null)
					{
						list.Add(pawnCapacityMinLevel.minLevel);
					}
				}
			}				
			if (list.Any())
			{
				float b = list.Max();
				__result = Mathf.Max(__result, b);
			}
		}
	}
}
