using HarmonyLib;
using RimWorld;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using VanillaPsycastsExpanded;
using Verse;
using VFECore.Abilities;

namespace MakaiTechPsycast.DestinedDeath
{
	[HarmonyPatch(typeof(Pawn), "Kill")]
	public static class Pawn_Undead_Patch
	{
		private static bool Prefix(Pawn __instance)
		{
			Hediff firstHediffOfDef = __instance.health.hediffSet.GetFirstHediffOfDef(MakaiTechPsy_DefOf.MakaiTechPsy_DD_LichSoul);
			if (firstHediffOfDef != null)
			{
				return false;
			}
			return true;
		}
	}
	[HarmonyPatch(typeof(Pawn_HealthTracker), "ShouldBeDowned")]
	public static class Patch_Undead_CantDowned_Patch
	{
		private static bool Prefix(Pawn ___pawn)
		{
			Hediff firstHediffOfDef = ___pawn.health.hediffSet.GetFirstHediffOfDef(MakaiTechPsy_DefOf.MakaiTechPsy_DD_LichSoul);
			if (firstHediffOfDef != null)
			{
				return false;
			}
			return true;
		}
	}
	[HarmonyPatch(typeof(PawnCapacityUtility), "CalculateCapacityLevel")]
	public static class PawnCapacityUtility_CalculateCapacityLevel
	{
		public static void Postfix(ref float __result, HediffSet diffSet, PawnCapacityDef capacity, List<PawnCapacityUtility.CapacityImpactor> impactors = null, bool forTradePrice = false)
		{
			Hediff LichSoul = diffSet.pawn.health.hediffSet.GetFirstHediffOfDef(MakaiTechPsy_DefOf.MakaiTechPsy_DD_LichSoul);
			if (LichSoul == null)
			{
				return;
			}
			List<float> list = new List<float>();
			if (LichSoul != null)
			{
				HediffExtension_LichSoul modExtension = LichSoul.def.GetModExtension<HediffExtension_LichSoul>();
				PawnCapacityMinLevel pawnCapacityMinLevel = modExtension.pawnCapacityMinLevels.FirstOrDefault((PawnCapacityMinLevel x) => x.capacity == capacity);
				if (pawnCapacityMinLevel != null)
				{
					list.Add(pawnCapacityMinLevel.minLevel);
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
