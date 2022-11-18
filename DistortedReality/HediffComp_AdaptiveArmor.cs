using RimWorld;
using Verse;

namespace MakaiTechPsycast.DistortedReality
{
    public class HediffComp_AdaptiveArmor : HediffComp
    {
        public HediffCompProperties_AdaptiveArmor Props => (HediffCompProperties_AdaptiveArmor)props;
        public override void Notify_PawnPostApplyDamage(DamageInfo dinfo, float totalDamageDealt)
        {
            base.Notify_PawnPostApplyDamage(dinfo, totalDamageDealt);
            if(Props.armorPenInfo != null)
            {
                foreach (ArmorCategoryInfoList item in Props.armorPenInfo)
                {
                    if (item.armorCategoryDef == dinfo.Def.armorCategory)
                    {
                        ApplyRandomBuff_ArmorCategory(item.armorRect);
                    }
                }
            }

            if (Props.damageDefs != null)
            {
                foreach (DamageDefInfoList item in Props.damageDefs)
                {
                    if (dinfo.Def == item.damageDef)
                    {
                        ApplyBuffDamageDef(item.armorRect);
                    }
                }
            }
        }

        public void ApplyRandomBuff_ArmorCategory(HediffDef hediffDef)
        {
            if(parent.pawn.health.hediffSet.HasHediff(hediffDef))
            {
                float chance = Rand.Value;
                if(chance <= 0.25f)
                {
                    parent.pawn.health.hediffSet.GetFirstHediffOfDef(hediffDef).TryGetComp<HediffComp_Disappears>().ticksToDisappear = 625;
                }
                else
                {
                    parent.pawn.health.hediffSet.GetFirstHediffOfDef(hediffDef).Severity += Props.severityPerHit;
                }
            }
            else
            {
                Hediff hediff = HediffMaker.MakeHediff(hediffDef, parent.pawn);
                hediff.TryGetComp<HediffComp_Disappears>().ticksToDisappear = 625;
                parent.pawn.health.AddHediff(hediff);
            }
        }

        public void ApplyBuffDamageDef(HediffDef hediffDef)
        {
            if (parent.pawn.health.hediffSet.HasHediff(hediffDef))
            {
                parent.pawn.health.hediffSet.GetFirstHediffOfDef(hediffDef).TryGetComp<HediffComp_Disappears>().ticksToDisappear = 625;
                return;
            }
            Hediff hediff = HediffMaker.MakeHediff(hediffDef, parent.pawn);
            hediff.TryGetComp<HediffComp_Disappears>().ticksToDisappear = 625;
            parent.pawn.health.AddHediff(hediff);
        }
    }
}
