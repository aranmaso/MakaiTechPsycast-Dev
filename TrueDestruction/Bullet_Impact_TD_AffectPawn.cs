using HarmonyLib;
using MakaiTechPsycast;
using RimWorld;
using Verse;

namespace MakaiTechPsycast.TrueDestruction
{
    [HarmonyPatch(typeof(Bullet))]
    [HarmonyPatch("Impact")]
    public class Bullet_Impact_TD_AffectPawn
    {
		private static void Postfix(ref Thing hitThing, ref Bullet __instance)
		{
			ThingDef equipmentDef = __instance.EquipmentDef;
			Building_TurretGun turret = __instance.Launcher as Building_TurretGun;
			Pawn pawn2 = hitThing as Pawn;
			if (equipmentDef == null || turret == null || pawn2 == null)
			{
				return;
			}
			CompProperties_TD_Turret compProperties = equipmentDef.GetCompProperties<CompProperties_TD_Turret>();
			if (compProperties == null)
			{
				return;
			}
			float rand = Rand.Value;
			if (pawn2 != null && rand <= 0.25f)
            {
				pawn2.health.AddHediff(compProperties.hediffDef);
            }
		}
	}
}
