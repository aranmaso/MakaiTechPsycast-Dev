using Verse;
using RimWorld;
using System.Collections.Generic;
using System.Text;

namespace MakaiTechPsycast
{
    public class HediffComp_ImmunityBuff : HediffComp
    {

        public HediffCompProperties_ImmunityBuff Props => (HediffCompProperties_ImmunityBuff)props;
        /*public override void CompExposeData()
        {
            Scribe_Values.Look(ref intervalTick, "intervalTick", 0);
        }*/
        /*public override void CompPostMake()
        {
            base.CompPostMake();
            intervalTick = Find.TickManager.TicksGame + Props.checkInterval;
            intervalTickShort = Find.TickManager.TicksGame + Props.checkIntervalShort;
        }*/
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
            if (parent.pawn.IsHashIntervalTick(Props.checkInterval))
            {
                TryImmuneHediff(parent.pawn);
                TryReplaceHediff(parent.pawn);
            }
        }

        public void TryImmuneHediff(Pawn pawn)
        {
            if(pawn.health == null || pawn.health.hediffSet == null || Props.immunityList == null)
            {
                return;
            }
            //remove hediff that immune
            foreach(HediffDef item in Props.immunityList)
            {
                if (pawn.health.hediffSet.HasHediff(item))
                {
                    pawn.health.RemoveHediff(pawn.health.hediffSet.GetFirstHediffOfDef(item));
                    MoteMaker.ThrowText(pawn.Position.ToVector3(),pawn.Map,"Immunity: " + item.LabelCap,color:UnityEngine.Color.green);
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
                if(pawn.health.hediffSet.HasHediff(item.hediffToImmune) && item.hediffToReplaceWith != null)
                {
                    BodyPartRecord bRecord = null;
                    if (replacedHediff.Part != null)
                    {
                        bRecord = replacedHediff.Part;
                    }
                    Hediff replaceWith = HediffMaker.MakeHediff(item.hediffToReplaceWith,pawn);
                    pawn.health.RemoveHediff(pawn.health.hediffSet.GetFirstHediffOfDef(item.hediffToImmune));
                    pawn.health.AddHediff(replaceWith,bRecord);
                    MoteMaker.ThrowText(pawn.Position.ToVector3(), pawn.Map, "Immunity: " + replacedHediff.Label);
                    MoteMaker.ThrowText(pawn.Position.ToVector3(), pawn.Map, "Replace with: " + replaceWith.Label);
                }
            }
        }
    }
}
