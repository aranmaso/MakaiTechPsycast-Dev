using Verse;
using RimWorld;
using VanillaPsycastsExpanded;
using System.Collections.Generic;
using UnityEngine;

namespace MakaiTechPsycast
{
    public class Projectile_Testing : Bullet
    {
        public ModExtension_TestingProjectile modExtension => base.def.GetModExtension<ModExtension_TestingProjectile>();

        protected override void Impact(Thing hitThing, bool blockedByShield = false)
        {
            base.Impact(hitThing);
            if (modExtension != null && hitThing != null && hitThing is Pawn pawn && pawn.Faction != base.launcher.Faction)
            {
                BodyPartRecord bR = pawn.RaceProps.body.AllParts.RandomElement();
                
                if(pawn.health.hediffSet.HasHediff(HediffDefOf.Flu))
                {
                    pawn.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.Flu).Severity += 0.1f;
                }
                else
                {
                    pawn.health.AddHediff(HediffDefOf.Flu);
                }
                if(pawn.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.Flu).Severity >= 0.8f && !pawn.Dead)
                {
                    for(int i = 0;i < 5; i++)
                    {
                        MakaiTD_PowerBeam orbitalStrike = (MakaiTD_PowerBeam)GenSpawn.Spawn(modExtension.thing, pawn.RandomAdjacentCell8Way(), pawn.Map);
                        orbitalStrike.duration = 60;
                        orbitalStrike.instigator = null;
                        orbitalStrike.StartStrike();
                    }
                }
                if(base.launcher is Pawn paw && pawn != null)
                {
                    SkillRecord SR = paw.skills.GetSkill(SkillDefOf.Intellectual);
                    SkillRecord SR2 = pawn.skills.GetSkill(SkillDefOf.Intellectual);
                    SR.Learn(100000f, direct: true);
                    SR2.levelInt -= 1;
                }
                if(base.launcher is Building_Turret BT)
                {
                    if(BT.HitPoints < BT.MaxHitPoints)
                    {
                        BT.HitPoints += Mathf.FloorToInt(BT.MaxHitPoints * 0.01f);
                    }
                    if(BT.HitPoints > BT.MaxHitPoints)
                    {
                        BT.HitPoints = BT.MaxHitPoints;
                    }
                    if(BT.IsBurning())
                    {
                        GenExplosion.DoExplosion(BT.Position, BT.Map, 2f, DamageDefOf.Extinguish, null);
                    }
                }
            }
            if (modExtension != null && modExtension.healAlly == true && hitThing != null && hitThing is Pawn pawn2 && pawn2.Faction == base.launcher.Faction)
            {
                if(pawn2.health.hediffSet.GetInjuriesTendable().EnumerableCount() > 0)
                {
                    Hediff_Injury HI = (Hediff_Injury)pawn2.health.hediffSet.GetInjuriesTendable().RandomElement();
                    pawn2.health.RemoveHediff(HI);
                }
                if(pawn2.health.hediffSet.GetMissingPartsCommonAncestors().Count > 0)
                {
                    Hediff_MissingPart hediffs = pawn2.health.hediffSet.GetMissingPartsCommonAncestors().RandomElement();
                    BodyPartRecord bodyPartRecord = hediffs.Part;
                    pawn2.health.RestorePart(bodyPartRecord);
                }
            }
            if(hitThing is Building building && building.Faction == Faction.OfPlayer)
            {
                if (building.HitPoints < building.MaxHitPoints)
                {
                    building.HitPoints += Mathf.Min(Mathf.FloorToInt(building.MaxHitPoints * 0.01f),building.MaxHitPoints);
                }
                if(building.HitPoints > building.MaxHitPoints)
                {
                    building.HitPoints = building.MaxHitPoints;
                }
            }
        }
    }
}
