using Verse;
using RimWorld;
using UnityEngine;

namespace MakaiTechPsycast.PassionedFlame
{
    public class HediffComp_PassionBuff : HediffComp
    {
        public HediffCompProperties_PassionBuff Props => (HediffCompProperties_PassionBuff)props;

        public int tickSinceTrigger;

        public int passionCount;
        public override void CompExposeData()
        {
            Scribe_Values.Look(ref tickSinceTrigger, "tickSinceTrigger", 0);
            Scribe_Values.Look(ref passionCount, "passionCount", 0);
        }

        public override void CompPostMake()
        {
            tickSinceTrigger = Find.TickManager.TicksGame + Props.updateInterval;
            UpdateSeverity();
        }
        public override void CompPostTick(ref float severityAdjustment)
        {
            if (Find.TickManager.TicksGame == tickSinceTrigger)
            {
                UpdateSeverity();
                tickSinceTrigger += Props.updateInterval;                
            }
        }

        public void UpdateSeverity()
        {
            passionCount = 0;
            if(Props.countAllPassion)
            {
                foreach (SkillRecord item in Pawn.skills.skills)
                {
                    if (item.passion != Passion.None)
                    {
                        passionCount++;
                    }
                    else
                    {
                        continue;
                    }
                }
            }    
            if(Props.countOnlyMajor)
            {
                foreach(SkillRecord item in Pawn.skills.skills)
                {
                    if(item.passion != Passion.Major)
                    {
                        continue;
                    }
                    passionCount++;
                }
            }
            if (Props.countOnlyMinor)
            {
                foreach (SkillRecord item in Pawn.skills.skills)
                {
                    if (item.passion != Passion.Minor)
                    {
                        continue;
                    }
                    passionCount++;
                }
            }
            if(parent.Severity <= Props.maxSeverityToAffect)
            {
                parent.Severity = passionCount;
            }            
        }
    }
}
