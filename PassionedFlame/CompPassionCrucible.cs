using Verse;
using RimWorld;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using VanillaPsycastsExpanded;

namespace MakaiTechPsycast.PassionedFlame
{
    public class CompPassionCrucible : ThingComp
    {
        public CompProperties_PassionCrucible Props => (CompProperties_PassionCrucible)props;

        //public int tickSinceTrigger;

        public int passionCount;

        public override void PostPostMake()
        {
            //tickSinceTrigger = Find.TickManager.TicksGame + Props.interval;
        }

        public override void PostExposeData()
        {
            base.PostExposeData();
            //Scribe_Values.Look(ref tickSinceTrigger, "tickSinceTrigger",0);
            Scribe_Values.Look(ref passionCount, "passionCount", 0);
        }

        public override void CompTick()
        {
            //base.CompTick();
            /*if(Find.TickManager.TicksGame == tickSinceTrigger)
            {
                DoTrigger();
                tickSinceTrigger+=Props.interval;
            }*/
        }
        public override IEnumerable<Gizmo> CompGetGizmosExtra()
        {
            Command_Action command_doTarget = new Command_Action();
            command_doTarget.defaultLabel = "Give Passion";
            command_doTarget.defaultDesc = "Give Passion";
            command_doTarget.icon = ContentFinder<Texture2D>.Get(Props.uiIcon);
            command_doTarget.action = delegate
            {                
                Pawn pawn;
                Find.Targeter.BeginTargeting(GetTargetingParameters(), delegate (LocalTargetInfo t)
                {
                    if(passionCount > 0)
                    {
                        passionCount--;
                        pawn = t.Pawn;
                        Vector3 position = pawn.Position.ToVector3Shifted();
                        MakaiUtility.ThrowFleckStationary(MakaiTechPsy_DefOf.MakaiPsyMote_DistortTwo, position, parent.Map,1f);
                        Find.WindowStack.Add(new Dialog_ChooseSkillToImprove(pawn));
                    }
                    else
                    {
                        Messages.Message("not enough passion stored",MessageTypeDefOf.NeutralEvent);
                    }
                });
            };
            yield return command_doTarget;

            Command_Action command_takePassion = new Command_Action();
            command_takePassion.defaultLabel = "Absorb Passion";
            command_takePassion.defaultDesc = "Absorb Passion";
            command_takePassion.icon = ContentFinder<Texture2D>.Get(Props.uiIcon);
            command_takePassion.action = delegate
            {
                Pawn pawn;
                Find.Targeter.BeginTargeting(GetTargetingParameters(), delegate (LocalTargetInfo t)
                {
                    pawn = t.Pawn;
                    foreach(SkillRecord sR in pawn.skills.skills)
                    {
                        if(sR.passion > Passion.None)
                        {
                            passionCount++;
                            sR.passion = Passion.None;                            
                        }
                    }
                    Vector3 position = pawn.Position.ToVector3Shifted();
                    MakaiUtility.ThrowFleckStationary(MakaiTechPsy_DefOf.MakaiPsyMote_DistortTwo, position, parent.Map, 1f);
                    MoteBetween moteBetween = (MoteBetween)ThingMaker.MakeThing(MakaiTechPsy_DefOf.MakaiPsyMote_Orb);
                    moteBetween.Attach(pawn, parent);
                    moteBetween.exactPosition = pawn.DrawPos;
                    GenSpawn.Spawn(moteBetween, parent.Position, pawn.Map);
                });
            };
            yield return command_takePassion;
        }

        public TargetingParameters GetTargetingParameters()
        {
            return new TargetingParameters
            {
                canTargetPawns = true,
                canTargetAnimals = false,
                canTargetBuildings = false,
                canTargetItems = false,
                mapObjectTargetsMustBeAutoAttackable = false,
                validator = (TargetInfo x) => x.Thing is Pawn
            };
        }


        public void DoTrigger()
        {
            foreach(Pawn item in MakaiUtility.GetNearbyPawnFoeOnly(parent.Position,parent.Faction,parent.Map,Props.radius))
            {
                foreach(SkillRecord sR in item.skills.skills)
                {
                    if(sR.passion > Passion.None)
                    {
                        passionCount++;
                        sR.passion = Passion.None;
                    }
                }
            }
            foreach (Pawn item in MakaiUtility.GetNearbyPawnFriendAndFoe(parent.Position, parent.Map, Props.radius))
            {
                if(!item.IsPrisoner || !item.IsPrisonerOfColony)
                {
                    continue;
                }
                foreach (SkillRecord sR in item.skills.skills)
                {
                    if (sR.passion > Passion.None)
                    {
                        passionCount++;
                        sR.passion = Passion.None;
                    }
                }
            }
        }

        public override string CompInspectStringExtra()
        {
            string text = "Passioned Flame stored" + ": " + passionCount + "unit";
            return text + base.CompInspectStringExtra();
        }
    }
}
