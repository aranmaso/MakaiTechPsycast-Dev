using Verse;
using RimWorld;
using System.Collections.Generic;
using UnityEngine;

namespace MakaiTechPsycast.DestinedDeath
{
    public class HediffComp_LifeLink : HediffComp
    {
        public List<Thing> linkedPawn = new List<Thing>();

        public float totalDamage = 0;

        public HediffCompProperties_LifeLink Props => (HediffCompProperties_LifeLink)props;

        public override string CompLabelInBracketsExtra
        {
            get
            {
                if (totalDamage > 0)
                {
                    return base.CompLabelInBracketsExtra + Mathf.FloorToInt(totalDamage) + " dmg";
                }
                return base.CompLabelInBracketsExtra;
            }
        }
        public override void CompExposeData()
        {
            base.CompExposeData();
            Scribe_Collections.Look(ref linkedPawn,true, "linkedPawn", LookMode.Reference);
            Scribe_Values.Look(ref totalDamage, "totalDamage", 0);
        }
        public override void Notify_PawnPostApplyDamage(DamageInfo dinfo, float totalDamageDealt)
        {
            base.Notify_PawnPostApplyDamage(dinfo, totalDamageDealt);
            if(dinfo.Def == MakaiTechPsy_DefOf.DestinedDeath_SharedDamage || dinfo.Def == MakaiTechPsy_DefOf.DestinedDeath_LinkExplosion)
            {
                return;
            }
            totalDamage += dinfo.Amount;
            foreach(Pawn item in linkedPawn)
            {
                if(item == parent.pawn)
                {
                    continue;
                }
                item.TakeDamage(new DamageInfo(MakaiTechPsy_DefOf.DestinedDeath_SharedDamage, dinfo.Amount, dinfo.ArmorPenetrationInt, dinfo.Angle, parent.pawn, dinfo.HitPart, dinfo.Weapon, dinfo.Category));
                item.health.hediffSet.GetFirstHediffOfDef(parent.def).TryGetComp<HediffComp_LifeLink>().totalDamage = totalDamage;
            }
        }
    }
}
