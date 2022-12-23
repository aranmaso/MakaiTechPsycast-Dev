using System;
using System.Collections.Generic;
using System.Text;
using RimWorld;
using Verse;

namespace MakaiTechPsycast.StringOfFate
{
    public class HediffComp_UltimateFate : HediffComp
    {
        public HediffCompProperties_UltimateFate Props => (HediffCompProperties_UltimateFate)props;

        public int fatedCount = 0;

        public float totalDamage = 0;

        public int threshold = 0;

        public int maxThresholdPerHit = 0;

        public override void CompExposeData()
        {
            base.CompExposeData();
            Scribe_Values.Look(ref fatedCount,"fatedCount",0);
            Scribe_Values.Look(ref totalDamage, "totalDamage", 0);
            Scribe_Values.Look(ref maxThresholdPerHit, "maxThresholdPerHit", 0);
            //Scribe_Values.Look(ref threshold, "threshold", 0);
        }

        public override string CompDescriptionExtra => CustomDescription(base.CompDescriptionExtra);
        public string CustomDescription(string Desc)
        {
            StringBuilder builder = new StringBuilder(Desc);
            builder.AppendLine();
            builder.AppendLine();
            builder.AppendLine("threshold: " + threshold);
            builder.AppendLine("maxThresholdPerHit: " + maxThresholdPerHit);
            return builder.ToString().TrimEndNewlines();
        }
        public override void Notify_PawnPostApplyDamage(DamageInfo dinfo, float totalDamageDealt)
        {
            base.Notify_PawnPostApplyDamage(dinfo, totalDamageDealt);
            totalDamage += totalDamageDealt;
        }
    }
}
