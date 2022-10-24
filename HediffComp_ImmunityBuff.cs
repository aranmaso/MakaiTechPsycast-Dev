﻿using Verse;
using RimWorld;
using System.Collections.Generic;
using System.Text;

namespace MakaiTechPsycast
{
    public class HediffComp_ImmunityBuff : HediffComp
    {
        public int intervalTick;

        public int intervalTickShort;

        private string desc;

        public HediffCompProperties_ImmunityBuff Props => (HediffCompProperties_ImmunityBuff)props;
        public override void CompExposeData()
        {
            Scribe_Values.Look(ref intervalTick, "intervalTick", 0);
            Scribe_Values.Look(ref intervalTickShort, "intervalTickShort", 0);
        }
        public override void CompPostMake()
        {
            base.CompPostMake();
            intervalTick = Find.TickManager.TicksGame + Props.checkInterval;
            intervalTickShort = Find.TickManager.TicksGame + 5;
        }
        public override string CompDescriptionExtra
        {
            get
            {
                if (Props.immunityList.Count > 0)
                {
                    StringBuilder builder = new StringBuilder();
                    builder.Append(base.CompDescriptionExtra);
                    builder.AppendLine("immune to");
                    builder.AppendLine();
                    foreach(HediffDef item in Props.immunityList)
                    {
                        builder.AppendLine("-" + item.label.ToString());
                    }
                    return builder.ToString().TrimEndNewlines();
                }
                return base.CompDescriptionExtra;
            }
        }
        public override void CompPostTick(ref float severityAdjustment)
        {
            base.CompPostTick(ref severityAdjustment);
            if(Find.TickManager.TicksGame == intervalTick)
            {
                TryReplaceHediff(parent.pawn);
                intervalTick += Props.checkInterval;
            }
            if(Find.TickManager.TicksGame == intervalTickShort)
            {
                TryImmuneHediff(parent.pawn);
                intervalTickShort += 5;
            }
        }

        public void TryImmuneHediff(Pawn pawn)
        {
            if(pawn.health == null || pawn.health.hediffSet == null)
            {
                return;
            }
            //remove hediff that immune
            foreach(HediffDef item in Props.immunityList)
            {
                Hediff firstHediffOfDef = pawn.health.hediffSet.GetFirstHediffOfDef(item);
                if(firstHediffOfDef != null)
                {
                    pawn.health.RemoveHediff(firstHediffOfDef);
                }
            }
        }
        public void TryReplaceHediff(Pawn pawn)
        {
            if (pawn.health == null || pawn.health.hediffSet == null || Props.hediffInfoList == null)
            {
                return;
            }
            foreach(HediffInfoList item in Props.hediffInfoList)
            {
                Hediff replacedHediff = pawn.health.hediffSet.GetFirstHediffOfDef(item.hediffToImmune);
                if(replacedHediff != null && item.hediffToReplaceWith != null)
                {
                    pawn.health.RemoveHediff(replacedHediff);
                    BodyPartRecord bRecord = null;
                    if (replacedHediff.Part != null)
                    {
                        bRecord = replacedHediff.Part;
                    }
                    Hediff replaceWith = HediffMaker.MakeHediff(item.hediffToReplaceWith,pawn);
                    pawn.health.AddHediff(replaceWith,bRecord);
                }
            }
        }
    }
}