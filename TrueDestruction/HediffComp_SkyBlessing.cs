using RimWorld;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace MakaiTechPsycast.TrueDestruction
{
    public class HediffComp_SkyBlessing : HediffComp
    {
        public bool isToggledOn = true;

        public bool isTriggered = false;

        public bool isBursted = false;

        public int useCountLeft = 1;

        public HediffCompProperties_SkyBlessing Props => (HediffCompProperties_SkyBlessing)props;

        public override string CompLabelInBracketsExtra
        {
            get
            {
                if (useCountLeft > 0)
                {
                    return base.CompLabelInBracketsExtra + useCountLeft;
                }
                return base.CompLabelInBracketsExtra;
            }
        }
        public override void CompExposeData()
        {
            base.CompExposeData();
            Scribe_Values.Look(ref isToggledOn, "isToggledOn", true);
            Scribe_Values.Look(ref isTriggered, "isTriggered", false);
            Scribe_Values.Look(ref isBursted, "isBursted", false);
            Scribe_Values.Look(ref useCountLeft, "useCountLeft", 1);
        }
        public override void CompPostTick(ref float severityAdjustment)
        {
            base.CompPostTick(ref severityAdjustment);
            if(Pawn.IsHashIntervalTick(300) && isTriggered)
            {
                isTriggered = false;
                isBursted = false;
            }
            if(useCountLeft == 0)
            {
                Pawn.health.RemoveHediff(parent);
            }
        }

        public override IEnumerable<Gizmo> CompGetGizmos()
        {
            Command_Toggle command_Toggle = new Command_Toggle();
            if (isToggledOn)
            {
                command_Toggle.defaultLabel = "sky blessing enabled";
                command_Toggle.defaultDesc = "projectile is being pulled upward";
            }
            else
            {
                command_Toggle.defaultLabel = "rejected blessing";
                command_Toggle.defaultDesc = "sky no longer give you favor";
            }
            command_Toggle.hotKey = KeyBindingDefOf.Command_ItemForbid;
            command_Toggle.icon = ContentFinder<Texture2D>.Get(Props.uiIcon);
            command_Toggle.isActive = () => isToggledOn;
            command_Toggle.toggleAction = delegate
            {
                isToggledOn = !isToggledOn;
            };
            yield return command_Toggle;

        }
    }
}
