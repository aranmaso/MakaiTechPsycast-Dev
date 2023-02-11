using Verse;
using RimWorld;
using System.Collections.Generic;
using UnityEngine;
using VFECore.Abilities;

namespace MakaiTechPsycast.StringOfFate
{
    public class CompConvergenceOfFate : ThingComp
    {
        public CompProperties_ConvergenceOfFate Props => (CompProperties_ConvergenceOfFate)props;

        public int triggerRate;

        public int tickSinceTrigger;

        public float radius;

        private bool isToggledOn = true;

        private bool hediffToggle = true;
        public override void PostPostMake()
        {
            triggerRate = Props.tickRate;
            radius = Props.radius;
            base.PostPostMake();
        }
        public override void PostExposeData()
        {
            Scribe_Values.Look(ref tickSinceTrigger, "tickSinceTrigger", 0);
            Scribe_Values.Look(ref triggerRate, "triggerRate", Props.tickRate);
            Scribe_Values.Look(ref isToggledOn, "isToggledOn", true);
            Scribe_Values.Look(ref hediffToggle, "hediffToggle", true);
            base.PostExposeData();
        }

        public override IEnumerable<Gizmo> CompGetGizmosExtra()
        {
            Command_Toggle command_Toggle = new Command_Toggle();
            if (isToggledOn)
            {
                command_Toggle.defaultLabel = "Aura Enabled";
                command_Toggle.defaultDesc = "Aura Enabled";
            }
            else
            {
                command_Toggle.defaultLabel = "Aura Disabled";
                command_Toggle.defaultDesc = "Aura Disabled";
            }
            command_Toggle.hotKey = KeyBindingDefOf.Command_ItemForbid;
            command_Toggle.icon = ContentFinder<Texture2D>.Get(Props.uiIcon);
            command_Toggle.isActive = () => isToggledOn;
            command_Toggle.toggleAction = delegate
            {
                isToggledOn = !isToggledOn;
            };
            yield return command_Toggle;
            if(isToggledOn)
            {
                //toggle applied Hediff
                Command_Toggle command_ToggleHediff = new Command_Toggle();
                if(hediffToggle)
                {
                    command_ToggleHediff.defaultLabel = "hediff: " + Props.hediffDef.label;
                    command_ToggleHediff.defaultDesc = "hediff: " + Props.hediffDef.label;
                }
                else
                {
                    command_ToggleHediff.defaultLabel = "hediff: " + Props.hediffDefSecond.label;
                    command_ToggleHediff.defaultDesc = "hediff: " + Props.hediffDefSecond.label;
                }
                command_ToggleHediff.icon = ContentFinder<Texture2D>.Get(Props.uiIcon);
                command_ToggleHediff.isActive = () => hediffToggle;
                command_ToggleHediff.toggleAction = delegate
                {
                    hediffToggle = !hediffToggle;
                };
                yield return command_ToggleHediff;

                Command_Action command_Action = new Command_Action();
                command_Action.defaultLabel = "increase interval:" + triggerRate;
                command_Action.icon = ContentFinder<Texture2D>.Get(Props.uiIcon);
                command_Action.action = delegate
                {
                    triggerRate += 250;
                };
                yield return command_Action;

                Command_Action command_Action2 = new Command_Action();
                command_Action2.defaultLabel = "decrease interval:" + triggerRate;
                command_Action2.icon = ContentFinder<Texture2D>.Get(Props.uiIcon);
                if (triggerRate <= 250)
                {
                    command_Action2.defaultDesc = "can't lower beyond minimum value";
                }
                command_Action2.action = delegate
                {
                    if(triggerRate > 250)
                    {
                        triggerRate -= 250;
                    }                    
                    else
                    {
                        Messages.Message("minimum reached", MessageTypeDefOf.NegativeEvent);
                    }
                };
                yield return command_Action2;
            }            
        }
        public override void CompTick()
        {
            tickSinceTrigger++;
            if (tickSinceTrigger >= triggerRate)
            {
                if(isToggledOn)
                {
                    GiveHediff();
                }                
                tickSinceTrigger = 0;
            }            
            base.CompTick();
        }


        public void GiveHediff()
        {
            HediffDef hediffDef;
            if (hediffToggle)
            {
                hediffDef = Props.hediffDef;
            }
            else
            {
                hediffDef = Props.hediffDefSecond;
            }
            foreach (Pawn item in MakaiUtility.GetNearbyPawnFriendOnly(parent.Position,parent.Faction,parent.Map,radius))
            {                                
                if (hediffToggle && item.health.hediffSet.HasHediff(Props.hediffDef))
                {
                    continue;
                }
                else if(!hediffToggle && item.health.hediffSet.HasHediff(Props.hediffDefSecond))
                {
                    continue;
                }
                if(hediffToggle && item.health.hediffSet.HasHediff(Props.hediffDefSecond))
                {
                    item.health.RemoveHediff(MakaiUtility.GetFirstHediffOfDef(item,Props.hediffDefSecond));
                }
                else if (!hediffToggle && item.health.hediffSet.HasHediff(Props.hediffDef))                
                {
                    item.health.RemoveHediff(MakaiUtility.GetFirstHediffOfDef(item, Props.hediffDef));
                }
                Hediff hediff = HediffMaker.MakeHediff(hediffDef, item);
                hediff.TryGetComp<HediffComp_Disappears>().ticksToDisappear = 5000;
                item.health.AddHediff(hediff);
                Effecter effect = MakaiTechPsy_DefOf.MakaiPsy_Ring_ExpandY.Spawn(item.Position,item.Map,1f);
                effect.Cleanup();
            }    
        }
    }
}
