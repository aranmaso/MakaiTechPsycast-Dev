using RimWorld;
using System.Collections.Generic;
using Verse;

namespace MakaiTechPsycast.PassionedFlame
{
    public class HediffComp_GatheringFlame : HediffComp
    {
        public HediffCompProperties_GatheringFlame Props => (HediffCompProperties_GatheringFlame)props;

        private int totalDurationLeft;

        private int activateInterval;

        public int passionDrained;

        public int numOfPassion;

        public override void CompExposeData()
        {
            base.CompExposeData();
            Scribe_Values.Look(ref totalDurationLeft, "totalDurationLeft",0);
            Scribe_Values.Look(ref activateInterval, "activateInterval", 0);
            Scribe_Values.Look(ref passionDrained, "passionDrained", 0);
        }

        public override void CompPostMake()
        {
            base.CompPostMake();
            totalDurationLeft = Props.ticksToDisappear;
        }

        public override void CompPostTick(ref float severityAdjustment)
        {
            if(Pawn.IsHashIntervalTick(activateInterval))
            {
                DoCheckPassion();
            }
            totalDurationLeft--;
        }
        public override bool CompShouldRemove
        {
            get
            {
                if (totalDurationLeft == 0 || numOfPassion >= 12)
                {                    
                    return true;
                }
                return false;
            }
        }

        public override IEnumerable<Gizmo> CompGetGizmos()
        {
            if(Prefs.DevMode)
            {
                Command_Action command_doTarget = new Command_Action();
                command_doTarget.defaultLabel = "DEV: Trigger Now";
                command_doTarget.defaultDesc = "DEV: Trigger Now";
                command_doTarget.action = delegate
                {
                    DoCheckPassion();
                };
                yield return command_doTarget;
            }            
        }

        public override void CompPostPostRemoved()
        {
            float severity = Props.curve.Evaluate(passionDrained);
            Hediff hediff = MakaiUtility.CreateCustomHediffWithDurationNoMultiplier(Pawn,Props.hediffToApplyOnceRemove,168,0);
            hediff.Severity = severity;
            Pawn.health.AddHediff(hediff);
        }
        public override string CompLabelInBracketsExtra
        {
            get
            {
                if (passionDrained >= 0)
                {
                    return base.CompLabelInBracketsExtra + passionDrained;
                }
                return base.CompLabelInBracketsExtra;
            }
        }

        public override string CompDebugString()
        {
            return "totalDurationLeft: " + totalDurationLeft;
        }

        public void DoCheckPassion()
        {
            numOfPassion = 0;
            foreach (SkillRecord item in Pawn.skills.skills)
            {                
                if (item.passion == Passion.None)
                {
                    numOfPassion++;
                    continue;
                }
                item.passion--;
                passionDrained++;
                break;
            }
        }
    }
}
