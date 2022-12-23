using RimWorld;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace MakaiTechPsycast.StringOfFate
{
    public class HediffComp_FateFavor : HediffComp
    {
        public HediffCompProperties_FateFavor Props => (HediffCompProperties_FateFavor)props;

        public IEnumerable<int> favorCount = new List<int>() {1,2,3,4,5,6,7};

        private int currentFavorInt = 0;

        private HediffDef currentHediffSelect;
        public override void CompExposeData()
        {
            Scribe_Values.Look(ref currentFavorInt, "currentFavorInt", 0);
            Scribe_Defs.Look(ref currentHediffSelect, "currentHediffSelect");
            base.CompExposeData();
        }
        public override void CompPostMake()
        {
            currentHediffSelect = Props.favorList.RandomElement();
            base.CompPostMake();
        }
        public override IEnumerable<Gizmo> CompGetGizmos()
        {
            Command_Action command_Action = new Command_Action();
            command_Action.defaultLabel = "Call: "+ currentFavorInt + " Favor";
            command_Action.icon = ContentFinder<Texture2D>.Get(Props.uiIcon);
            command_Action.action = delegate
            {
                if(((Hediff_LevelWithoutPart)parent).level >= currentFavorInt)
                {
                    ((Hediff_LevelWithoutPart)parent).ChangeLevel(-currentFavorInt);
                    GiveFavorHediff(currentHediffSelect,currentFavorInt);
                    SoundDefOf.PsycastPsychicEffect.PlayOneShot(new TargetInfo(Pawn.Position, Pawn.Map));
                    MoteMaker.ThrowText(Pawn.DrawPos, Pawn.Map, currentHediffSelect.label + " gained");
                }
                else
                {
                    MoteMaker.ThrowText(Pawn.DrawPos,Pawn.Map,"not enough Favor left");
                }                
            };
            yield return command_Action;

            Command_Action command_ActionFavor = new Command_Action();
            command_ActionFavor.defaultLabel = "Favor: " + currentFavorInt;
            command_ActionFavor.defaultDesc = "Favor: " + currentFavorInt;
            command_ActionFavor.icon = ContentFinder<Texture2D>.Get(Props.uiIcon);
            command_ActionFavor.action = delegate
            {
                List<FloatMenuOption> list = new List<FloatMenuOption>();
                foreach (var option in favorCount)
                {
                    list.Add(new FloatMenuOption(option.ToString(), delegate
                    {
                        command_ActionFavor.defaultLabel = "Favor: " + option;
                        currentFavorInt = option;
                    }));
                }
                Find.WindowStack.Add(new FloatMenu(list));
            };
            yield return command_ActionFavor;

            Command_Action command_ActionHediff = new Command_Action();
            command_ActionHediff.defaultLabel = "Blessing: " + currentHediffSelect.label;
            command_ActionHediff.defaultDesc = "Blessing: " + currentHediffSelect.label;
            command_ActionHediff.icon = ContentFinder<Texture2D>.Get(Props.uiIcon);
            command_ActionHediff.action = delegate
            {
                List<FloatMenuOption> list = new List<FloatMenuOption>();
                foreach (var option in Props.favorList)
                {
                    list.Add(new FloatMenuOption(option.label, delegate
                    {
                        currentHediffSelect = option;
                    }));
                }
                Find.WindowStack.Add(new FloatMenu(list));
            };
            yield return command_ActionHediff;
        }

        public void GiveFavorHediff(HediffDef hediffDef,int hours)
        {
            Hediff hediff = MakaiUtility.CreateCustomHediffWithDuration(Pawn, hediffDef, hours, 0);
            hediff.TryGetComp<HediffComp_Disappears>().ticksToDisappear = hours * 2500;
            hediff.Severity = hours;
            Pawn.health.AddHediff(hediff);
        }
    }
}
