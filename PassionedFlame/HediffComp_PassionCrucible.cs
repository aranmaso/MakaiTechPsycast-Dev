using RimWorld;
using Verse;
using System.Collections.Generic;
using UnityEngine;
using Verse.Sound;

namespace MakaiTechPsycast.PassionedFlame
{
    public class HediffComp_PassionCrucible : HediffComp
    {
        public HediffCompProperties_PassionCrucible Props => (HediffCompProperties_PassionCrucible)props;

        public IEnumerable<int> blessingCount = new List<int>() { 1, 2, 3, 4, 5 };

        public int blessingCountSelected = 1;

        private int PassionCountInit;

        private int currentStack;

        private int interval = 2500;

        private int tickUntilCheck;

        private HediffDef curHediff;
        public override void CompPostTick(ref float severityAdjustment)
        {
            if (Find.TickManager.TicksGame == tickUntilCheck)
            {
                CheckPassion();
                if(Pawn.Inspired)
                {
                    currentStack++;
                }
                tickUntilCheck += interval;
            }
        }

        public override string CompLabelInBracketsExtra
        {
            get
            {
                if (currentStack >= 0)
                {
                    return base.CompLabelInBracketsExtra + currentStack;
                }
                return base.CompLabelInBracketsExtra;
            }
        }

        public override void CompExposeData()
        {
            Scribe_Values.Look(ref PassionCountInit, "PassionCountInit", 0);
            Scribe_Values.Look(ref tickUntilCheck, "tickUntilCheck", 0);
            Scribe_Values.Look(ref currentStack, "currentStack", 0);
            Scribe_Defs.Look(ref curHediff, "curHediff");
        }

        public override IEnumerable<Gizmo> CompGetGizmos()
        {
            if(Pawn.Drafted)
            {
                Command_Action command_ActionDoThing = new Command_Action();
                command_ActionDoThing.defaultLabel = "Call: " + blessingCountSelected + " Favor";
                command_ActionDoThing.icon = ContentFinder<Texture2D>.Get(Props.uiIcon);
                command_ActionDoThing.action = delegate
                {
                    if (currentStack >= blessingCountSelected)
                    {
                        currentStack -= blessingCountSelected;
                        GiveFavorHediff(curHediff, blessingCountSelected,blessingCountSelected);
                        SoundDefOf.PsycastPsychicEffect.PlayOneShot(new TargetInfo(Pawn.Position, Pawn.Map));
                        MoteMaker.ThrowText(Pawn.DrawPos, Pawn.Map, curHediff.label + " gained");
                    }
                    else
                    {
                        MoteMaker.ThrowText(Pawn.DrawPos, Pawn.Map, "not enough stack left");
                    }
                };
                yield return command_ActionDoThing;

                Command_Action command_ActionHediffSelect = new Command_Action();
                command_ActionHediffSelect.defaultLabel = "Blessing: " + curHediff.label;
                command_ActionHediffSelect.defaultDesc = "Blessing: " + curHediff.label;
                command_ActionHediffSelect.icon = ContentFinder<Texture2D>.Get(Props.uiIcon);
                command_ActionHediffSelect.action = delegate
                {
                    List<FloatMenuOption> list = new List<FloatMenuOption>();
                    foreach (var option in Props.possibleHediffs)
                    {
                        list.Add(new FloatMenuOption(option.label, delegate
                        {
                            command_ActionHediffSelect.defaultLabel = "Blessing: " + option;
                            curHediff = option;
                        }));
                    }
                    Find.WindowStack.Add(new FloatMenu(list));
                };
                yield return command_ActionHediffSelect;

                Command_Action command_ActionFavor = new Command_Action();
                command_ActionFavor.defaultLabel = "Favor: " + blessingCountSelected;
                command_ActionFavor.defaultDesc = "Favor: " + blessingCountSelected;
                command_ActionFavor.icon = ContentFinder<Texture2D>.Get(Props.uiIcon);
                command_ActionFavor.action = delegate
                {
                    List<FloatMenuOption> list = new List<FloatMenuOption>();
                    foreach (var option in blessingCount)
                    {
                        list.Add(new FloatMenuOption(option.ToString(), delegate
                        {
                            command_ActionFavor.defaultLabel = "Favor: " + option;
                            blessingCountSelected = option;
                        }));
                    }
                    Find.WindowStack.Add(new FloatMenu(list));
                };
                yield return command_ActionFavor;
            }            
        }

        public void GiveFavorHediff(HediffDef hediffDef, int hours,float severity)
        {
            int num = hours * 2500;
            Hediff hediff = MakaiUtility.CreateCustomHediffWithDurationNoMultiplier(Pawn, hediffDef, 0, num);      
            hediff.Severity = severity;
            Pawn.health.AddHediff(hediff);
        }

        public void CheckPassion()
        {
            if(Pawn.skills.PassionCount > PassionCountInit)
            {
                currentStack += Pawn.skills.PassionCount - PassionCountInit;
                PassionCountInit = currentStack;
            }
        }
        public override void CompPostMake()
        {
            PassionCountInit = Pawn.skills.PassionCount;
            tickUntilCheck = Find.TickManager.TicksGame + interval;
            curHediff = Props.possibleHediffs.RandomElement();
        }
        
    }
}
