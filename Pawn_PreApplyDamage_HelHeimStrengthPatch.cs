using HarmonyLib;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace MakaiTechPsycast.DestinedDeath
{

    [HarmonyPatch(typeof(Pawn))]
    [HarmonyPatch("PreApplyDamage")]
    public class Pawn_PreApplyDamage_HelHeimStrengthPatch
    {
        private static void Postfix(ref DamageInfo dinfo, ref bool absorbed, ref Pawn __instance)
        {
            if(__instance == null || !__instance.health.hediffSet.HasHediff(MakaiTechPsy_DefOf.MakaiTechPsy_DD_Helheim))
            {
                return;
            }
            Hediff hediff = __instance.health.hediffSet.GetFirstHediffOfDef(MakaiTechPsy_DefOf.MakaiTechPsy_DD_Helheim);
            HediffComp_HelheimStrength shieldCount = hediff.TryGetComp<HediffComp_HelheimStrength>();
            if (shieldCount.ShieldCount > 0)
            {
                if(shieldCount.stopOnlyEnemy && dinfo.Instigator.Faction.HostileTo(__instance.Faction))
                {
                    shieldCount.ShieldCount -= 1;
                    if (shieldCount.ShieldCount == 0)
                    {
                        SoundDefOf.EnergyShield_Broken.PlayOneShot(new TargetInfo(__instance.Position,__instance.MapHeld));
                        Effecter effect2 = EffecterDefOf.Shield_Break.Spawn(__instance.Position, __instance.Map, 1f);
                        effect2.Cleanup();
                    }
                    absorbed = true;
                    Effecter effect = EffecterDefOf.Deflect_Metal.Spawn(__instance.Position, __instance.Map, 1f);
                    effect.Cleanup();
                    
                }
                else
                {
                    shieldCount.ShieldCount -= 1;
                    if (shieldCount.ShieldCount == 0)
                    {
                        SoundDefOf.EnergyShield_Broken.PlayOneShot(new TargetInfo(__instance.Position, __instance.MapHeld));
                        Effecter effect2 = EffecterDefOf.Shield_Break.Spawn(__instance.Position, __instance.Map, 1f);
                        effect2.Cleanup();
                    }
                    absorbed = true;
                    Effecter effect = EffecterDefOf.Deflect_Metal.Spawn(__instance.Position, __instance.Map, 1f);
                    effect.Cleanup();
                    
                }
                
            }
        }
    }
}
