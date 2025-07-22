using RimWorld.Planet;
using UnityEngine;
using HarmonyLib;
using System.Collections.Generic;
using VanillaPsycastsExpanded;
using VEF.Abilities;
using RimWorld;
using Verse;
using System.Linq;

namespace MakaiTechPsycast.DestinedDeath
{
    public class Ability_CollectSoul : VEF.Abilities.Ability
    {
		public IntVec3 targetCell;
        AbilityExtension_Roll1D20 modExtension => def.GetModExtension<AbilityExtension_Roll1D20>();
        public override bool ValidateTarget(LocalTargetInfo target, bool showMessages = true)
		{			
			if (modExtension.targetOnlyEnemies && target.Thing != null && (!target.Thing.HostileTo(pawn.Faction) || !target.Thing.Faction.HostileTo(pawn.Faction) || !target.Thing.HostileTo(pawn)))
			{
				if (showMessages)
				{
					Messages.Message("VFEA.TargetMustBeHostile".Translate(), target.Thing, MessageTypeDefOf.CautionInput, null);
				}
				return false;
			}
			return base.ValidateTarget(target, showMessages);
		}

		public override void ModifyTargets(ref GlobalTargetInfo[] targets)
		{
			targetCell = targets[0].Cell;
			base.ModifyTargets(ref targets);
		}
		public override void Cast(params GlobalTargetInfo[] targets)
        {
			base.Cast(targets);
            RollInfo rollinfo = new RollInfo();
            rollinfo = MakaiUtility.Roll1D20(pawn, modExtension.skillBonus, rollinfo);
            int roll = rollinfo.roll;
            if (roll >= modExtension.successThreshold && roll < modExtension.greatSuccessThreshold)
            {
                for (int i = 0; i < targets.Length; i++)
                {
                    GlobalTargetInfo globalTargetInfo = targets[i];
                    if (globalTargetInfo.Thing is Pawn victim && victim.Downed && !victim.health.hediffSet.HasHediff(modExtension.hediffDefWhenSuccess))
                    {
                        MoteBetween moteBetween = (MoteBetween)ThingMaker.MakeThing(VPE_DefOf.VPE_SoulOrbTransfer);
                        moteBetween.Attach(globalTargetInfo.Thing, pawn);
                        moteBetween.exactPosition = globalTargetInfo.Thing.DrawPos;
                        GenSpawn.Spawn(moteBetween, globalTargetInfo.Thing.Position, pawn.Map);
                    }
                }
                foreach (GlobalTargetInfo globalTargetInfo in targets)
                {
                    if (globalTargetInfo.Thing is Pawn pawn2)
                    {
                        if (modExtension.targetOnlyDowned && !(pawn2.Downed))
                        {
                            continue;
                        }
                        if (pawn2.health.hediffSet.HasHediff(modExtension.hediffDefWhenSuccess))
                        {
                            continue;
                        }
                        Hediff hediff = HediffMaker.MakeHediff(modExtension.hediffDefWhenSuccess, pawn2);                        
                        pawn2.health.AddHediff(hediff);
                        if (pawn.health.hediffSet.HasHediff(MakaiTechPsy_DefOf.MakaiTechPsy_DD_CollectSoul))
                        {
                            pawn.health.hediffSet.GetFirstHediffOfDef(MakaiTechPsy_DefOf.MakaiTechPsy_DD_CollectSoul).TryGetComp<HediffComp_SoulCollection>().SoulCount += 1;
                            pawn.health.hediffSet.GetFirstHediffOfDef(MakaiTechPsy_DefOf.MakaiTechPsy_DD_CollectSoul).TryGetComp<HediffComp_SoulCollection>().BonustToStat += 1;
                            pawn.health.hediffSet.GetFirstHediffOfDef(MakaiTechPsy_DefOf.MakaiTechPsy_DD_CollectSoul).Severity = pawn.health.hediffSet.GetFirstHediffOfDef(MakaiTechPsy_DefOf.MakaiTechPsy_DD_CollectSoul).TryGetComp<HediffComp_SoulCollection>().SoulCount;
                        }
                        else
                        {
                            Hediff hediff2 = HediffMaker.MakeHediff(MakaiTechPsy_DefOf.MakaiTechPsy_DD_CollectSoul, pawn);
                            pawn.health.AddHediff(hediff2);
                            pawn.health.hediffSet.GetFirstHediffOfDef(MakaiTechPsy_DefOf.MakaiTechPsy_DD_CollectSoul).TryGetComp<HediffComp_SoulCollection>().SoulCount += 1;
                            pawn.health.hediffSet.GetFirstHediffOfDef(MakaiTechPsy_DefOf.MakaiTechPsy_DD_CollectSoul).TryGetComp<HediffComp_SoulCollection>().BonustToStat += 1;
                        }
                        Messages.Message("Makai_PassArollcheckCollectSoul".Translate(pawn.LabelShort, pawn2.LabelShort, pawn.Named("USER")), pawn, MessageTypeDefOf.PositiveEvent);
                    }
                    else
                    {
                        continue;
                    }
                }                
                /*foreach(Thing item in GenRadial.RadialDistinctThingsAround(targetCell,pawn.Map,def.radius,true))
                {
                    if(item is Corpse corpse)
                    {
                        corpse.Strip();
                        string name = item.Label;
                        item.Destroy();
                        Hediff hediff2 = HediffMaker.MakeHediff(MakaiTechPsy_DefOf.MakaiTechPsy_DD_CollectSoul, pawn);
                        if (pawn.health.hediffSet.HasHediff(MakaiTechPsy_DefOf.MakaiTechPsy_DD_CollectSoul))
                        {
                            pawn.health.hediffSet.GetFirstHediffOfDef(MakaiTechPsy_DefOf.MakaiTechPsy_DD_CollectSoul).TryGetComp<HediffComp_SoulCollection>().SoulCount += 1;
                        }
                        else
                        {
                            pawn.health.AddHediff(hediff2);
                            pawn.health.hediffSet.GetFirstHediffOfDef(MakaiTechPsy_DefOf.MakaiTechPsy_DD_CollectSoul).TryGetComp<HediffComp_SoulCollection>().SoulCount += 1;
                        }
                        Messages.Message("Makai_PassArollcheckCollectSoul".Translate(pawn.LabelShort, name, pawn.Named("USER")), pawn, MessageTypeDefOf.PositiveEvent);
                    }
                }*/
                Messages.Message("Makai_PassArollcheck".Translate(pawn.LabelShort, rollinfo.baseRoll, rollinfo.cumulativeBonusRoll, pawn.Named("USER")), pawn, MessageTypeDefOf.PositiveEvent);
			}
			if (roll >= modExtension.greatSuccessThreshold)
			{
                for (int i = 0; i < targets.Length; i++)
                {
                    GlobalTargetInfo globalTargetInfo = targets[i];
                    if (globalTargetInfo.Thing is Pawn victim && victim.Downed && !victim.health.hediffSet.HasHediff(modExtension.hediffDefWhenSuccess))
                    {
                        MoteBetween moteBetween = (MoteBetween)ThingMaker.MakeThing(VPE_DefOf.VPE_SoulOrbTransfer);
                        moteBetween.Attach(globalTargetInfo.Thing, pawn);
                        moteBetween.exactPosition = globalTargetInfo.Thing.DrawPos;
                        GenSpawn.Spawn(moteBetween, globalTargetInfo.Thing.Position, pawn.Map);
                    }
                }
                foreach (GlobalTargetInfo globalTargetInfo in targets)
                {
                    if (globalTargetInfo.Thing is Pawn pawn2)
                    {
                        if (modExtension.targetOnlyDowned && !(pawn2.Downed))
                        {
                            continue;
                        }
                        if (pawn2.health.hediffSet.HasHediff(modExtension.hediffDefWhenGreatSuccess))
                        {
                            continue;
                        }
                        Hediff hediff = HediffMaker.MakeHediff(modExtension.hediffDefWhenGreatSuccess, pawn2);
                        pawn2.health.AddHediff(hediff);
                        if (pawn.health.hediffSet.HasHediff(MakaiTechPsy_DefOf.MakaiTechPsy_DD_CollectSoul))
                        {
                            pawn.health.hediffSet.GetFirstHediffOfDef(MakaiTechPsy_DefOf.MakaiTechPsy_DD_CollectSoul).TryGetComp<HediffComp_SoulCollection>().SoulCount += 2;
                            pawn.health.hediffSet.GetFirstHediffOfDef(MakaiTechPsy_DefOf.MakaiTechPsy_DD_CollectSoul).TryGetComp<HediffComp_SoulCollection>().BonustToStat += 2;
                            pawn.health.hediffSet.GetFirstHediffOfDef(MakaiTechPsy_DefOf.MakaiTechPsy_DD_CollectSoul).Severity = pawn.health.hediffSet.GetFirstHediffOfDef(MakaiTechPsy_DefOf.MakaiTechPsy_DD_CollectSoul).TryGetComp<HediffComp_SoulCollection>().SoulCount;
                        }
                        else
                        {
                            Hediff hediff2 = HediffMaker.MakeHediff(MakaiTechPsy_DefOf.MakaiTechPsy_DD_CollectSoul, pawn);
                            pawn.health.AddHediff(hediff2);
                            pawn.health.hediffSet.GetFirstHediffOfDef(MakaiTechPsy_DefOf.MakaiTechPsy_DD_CollectSoul).TryGetComp<HediffComp_SoulCollection>().SoulCount += 1;
                            pawn.health.hediffSet.GetFirstHediffOfDef(MakaiTechPsy_DefOf.MakaiTechPsy_DD_CollectSoul).TryGetComp<HediffComp_SoulCollection>().BonustToStat += 1;
                        }
                        Messages.Message("Makai_GreatPassArollcheckCollectSoul".Translate(pawn.LabelShort, pawn2.LabelShort, pawn.Named("USER")), pawn, MessageTypeDefOf.PositiveEvent);
                    }
                    else
                    {
						continue;
                    }                    
                }
                
                Messages.Message("Makai_GreatPassArollcheck".Translate(pawn.LabelShort, rollinfo.baseRoll, rollinfo.cumulativeBonusRoll, pawn.Named("USER")), pawn, MessageTypeDefOf.PositiveEvent);
			}
			if (roll < modExtension.successThreshold)
			{
                for (int i = 0; i < targets.Length; i++)
                {
                    GlobalTargetInfo globalTargetInfo = targets[i];
                    if (globalTargetInfo.Thing is Pawn victim && victim.Downed && !victim.health.hediffSet.HasHediff(modExtension.hediffDefWhenSuccess))
                    {
                        MoteBetween moteBetween = (MoteBetween)ThingMaker.MakeThing(VPE_DefOf.VPE_SoulOrbTransfer);
                        moteBetween.Attach(globalTargetInfo.Thing, pawn);
                        moteBetween.exactPosition = globalTargetInfo.Thing.DrawPos;
                        GenSpawn.Spawn(moteBetween, globalTargetInfo.Thing.Position, pawn.Map);
                    }
                }
                foreach (GlobalTargetInfo globalTargetInfo in targets)
                {
                    if (globalTargetInfo.Thing is Pawn pawn2)
                    {
                            if (modExtension.targetOnlyDowned && !(pawn2.Downed))
                            {
                                continue;
                            }
                            if (pawn2.health.hediffSet.HasHediff(modExtension.hediffDefWhenFail))
                            {
                                continue;
                            }
                            Hediff hediff = HediffMaker.MakeHediff(modExtension.hediffDefWhenFail, pawn2);
                            pawn2.health.AddHediff(hediff);
                            if (pawn.health.hediffSet.HasHediff(MakaiTechPsy_DefOf.MakaiTechPsy_DD_CollectSoul))
                            {
                                pawn.health.hediffSet.GetFirstHediffOfDef(MakaiTechPsy_DefOf.MakaiTechPsy_DD_CollectSoul).TryGetComp<HediffComp_SoulCollection>().SoulCount += 1;
                                pawn.health.hediffSet.GetFirstHediffOfDef(MakaiTechPsy_DefOf.MakaiTechPsy_DD_CollectSoul).TryGetComp<HediffComp_SoulCollection>().BonustToStat += 1;
                                pawn.health.hediffSet.GetFirstHediffOfDef(MakaiTechPsy_DefOf.MakaiTechPsy_DD_CollectSoul).Severity = pawn.health.hediffSet.GetFirstHediffOfDef(MakaiTechPsy_DefOf.MakaiTechPsy_DD_CollectSoul).TryGetComp<HediffComp_SoulCollection>().SoulCount;
                            }
                            else
                            {
                                Hediff hediff2 = HediffMaker.MakeHediff(MakaiTechPsy_DefOf.MakaiTechPsy_DD_CollectSoul, pawn);
                                pawn.health.AddHediff(hediff2);
                                pawn.health.hediffSet.GetFirstHediffOfDef(MakaiTechPsy_DefOf.MakaiTechPsy_DD_CollectSoul).TryGetComp<HediffComp_SoulCollection>().SoulCount += 1;
                                pawn.health.hediffSet.GetFirstHediffOfDef(MakaiTechPsy_DefOf.MakaiTechPsy_DD_CollectSoul).TryGetComp<HediffComp_SoulCollection>().BonustToStat += 1;
                            }
                        Messages.Message("Makai_FailArollcheckCollectSoul".Translate(pawn.LabelShort, pawn2.LabelShort, pawn.Named("USER")), pawn, MessageTypeDefOf.NegativeEvent);
                    }
                    else
                    {
                        continue;
                    }
                }
                
                Messages.Message("Makai_FailArollcheck".Translate(pawn.LabelShort, rollinfo.baseRoll, rollinfo.cumulativeBonusRoll, pawn.Named("USER")), pawn, MessageTypeDefOf.NegativeEvent);
			}
		}
    }
}
