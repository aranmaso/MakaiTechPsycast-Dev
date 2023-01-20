using RimWorld;
using RimWorld.Planet;
using Verse;
using Verse.Sound;
using VFECore;
using VanillaPsycastsExpanded;
using UnityEngine;
using Verse.AI;

namespace MakaiTechPsycast.TrueDestruction
{
    public class Ability_HeavenlyChain : VFECore.Abilities.Ability
    {
        public int shotLeft;

        public int tickUntilNextShot;
        public AbilityExtension_Roll1D20 modExtension => def.GetModExtension<AbilityExtension_Roll1D20>();

        private GlobalTargetInfo targetInfo;

        private Thing targetThing;
        public override void Tick()
        {
            base.Tick();
            if (tickUntilNextShot > 0)
            {
                tickUntilNextShot--;
                return;
            }
            if (shotLeft > 0)
            {
                DoShot(pawn, targetInfo, targetThing);
                tickUntilNextShot = modExtension.tickBurstInterval;
                shotLeft--;
            }
        }
        public void DoShot(Pawn instigator, GlobalTargetInfo globalTargetInfo, Thing targetThing = null)
        {
            IntVec3 spawnPosition = MakaiUtility.RandomCellAroundCellBase(globalTargetInfo.Cell, -4, 4);
            IntVec3 spawnPositionOffset = spawnPosition;
            spawnPositionOffset.z += 10;
            Projectile projectile = (Projectile)GenSpawn.Spawn(modExtension.projectileWhenSuccess, spawnPositionOffset, instigator.Map);

            projectile?.Launch(instigator, spawnPositionOffset.ToVector3(), spawnPosition, spawnPosition, ProjectileHitFlags.IntendedTarget);
            //modExtension.soundDef.PlayOneShot(new TargetInfo(spawnPosition, instigator.Map));
        }
        public override void Cast(params GlobalTargetInfo[] targets)
        {   
            base.Cast(targets);
            RollInfo rollinfo = new RollInfo();
            rollinfo = MakaiUtility.Roll1D20(pawn, modExtension.skillBonus, rollinfo, modExtension.skillBonus2);
            if (rollinfo.roll >= modExtension.successThreshold && rollinfo.roll < modExtension.greatSuccessThreshold)
            {
                shotLeft = modExtension.projectileBurstCount;
                tickUntilNextShot = modExtension.tickBurstInterval;
                targetThing = targets[0].Thing;
                targetInfo = targets[0];
                foreach (Pawn pawnAround in MakaiUtility.GetNearbyPawnFriendAndFoe(targets[0].Cell, pawn.Map, GetRadiusForPawn()))
                {
                    if (pawnAround == pawn) continue;
                    if(pawnAround.health.hediffSet.HasHediff(modExtension.hediffDefWhenSuccess))
                    {
                        pawnAround.health.RemoveHediff(MakaiUtility.GetFirstHediffOfDef(pawnAround, modExtension.hediffDefWhenSuccess));
                        Hediff hediff = MakaiUtility.CreateCustomHediffWithDuration(pawnAround, modExtension.hediffDefWhenSuccess, modExtension.hours, modExtension.ticks);
                        pawnAround.health.AddHediff(hediff);
                    }
                    else
                    {
                        Hediff hediff = MakaiUtility.CreateCustomHediffWithDuration(pawnAround, modExtension.hediffDefWhenSuccess, modExtension.hours, modExtension.ticks);
                        pawnAround.health.AddHediff(hediff);
                    }                    
                }
                Messages.Message("Makai_PassArollcheck".Translate(pawn.LabelShort, rollinfo.baseRoll, rollinfo.cumulativeBonusRoll, pawn.Named("USER")), pawn, MessageTypeDefOf.PositiveEvent);
                Messages.Message("Makai_PassArollcheckHeavenlyChain".Translate(pawn.LabelShort, pawn.Named("USER")), pawn, MessageTypeDefOf.PositiveEvent);
            }
            if (rollinfo.roll >= modExtension.greatSuccessThreshold)
            {
                shotLeft = modExtension.projectileBurstCount;
                tickUntilNextShot = modExtension.tickBurstInterval;
                targetThing = targets[0].Thing;
                targetInfo = targets[0];
                foreach (Pawn pawnAround in MakaiUtility.GetNearbyPawnFriendAndFoe(targets[0].Cell, pawn.Map, GetRadiusForPawn()))
                {
                    if (pawnAround == pawn) continue;
                    if (pawnAround.health.hediffSet.HasHediff(modExtension.hediffDefWhenSuccess))
                    {
                        pawnAround.health.RemoveHediff(MakaiUtility.GetFirstHediffOfDef(pawnAround, modExtension.hediffDefWhenSuccess));
                        Hediff hediff = MakaiUtility.CreateCustomHediffWithDuration(pawnAround, modExtension.hediffDefWhenSuccess, modExtension.hours, modExtension.ticks*2);
                        pawnAround.health.AddHediff(hediff);
                    }
                    else
                    {
                        Hediff hediff = MakaiUtility.CreateCustomHediffWithDuration(pawnAround, modExtension.hediffDefWhenSuccess, modExtension.hours, modExtension.ticks*2);
                        pawnAround.health.AddHediff(hediff);
                    }
                    ThingWithComps weapon = pawnAround.equipment?.Primary;
                    if(weapon != null)
                    {
                        pawnAround.equipment.TryDropEquipment(weapon, out weapon, pawnAround.RandomAdjacentCell8Way());
                    }                    
                }
                Messages.Message("Makai_GreatPassArollcheck".Translate(pawn.LabelShort, rollinfo.baseRoll, rollinfo.cumulativeBonusRoll, pawn.Named("USER")), pawn, MessageTypeDefOf.PositiveEvent);
                Messages.Message("Makai_GreatPassArollcheckHeavenlyChain".Translate(pawn.LabelShort, pawn.Named("USER")), pawn, MessageTypeDefOf.PositiveEvent);
            }
            if (rollinfo.roll < modExtension.successThreshold)
            {
                shotLeft = modExtension.projectileBurstCount;
                tickUntilNextShot = modExtension.tickBurstInterval;
                targetThing = targets[0].Thing;
                targetInfo = targets[0];
                foreach (Pawn pawnAround in MakaiUtility.GetNearbyPawnFriendAndFoe(targets[0].Cell, pawn.Map, GetRadiusForPawn()))
                {
                    if (pawnAround == pawn) continue;
                    if (pawnAround.health.hediffSet.HasHediff(modExtension.hediffDefWhenSuccess))
                    {
                        pawnAround.health.RemoveHediff(MakaiUtility.GetFirstHediffOfDef(pawnAround, modExtension.hediffDefWhenSuccess));
                        Hediff hediff = MakaiUtility.CreateCustomHediffWithDuration(pawnAround, modExtension.hediffDefWhenSuccess, modExtension.hours, modExtension.ticks/2);
                        pawnAround.health.AddHediff(hediff);
                    }
                    else
                    {
                        Hediff hediff = MakaiUtility.CreateCustomHediffWithDuration(pawnAround, modExtension.hediffDefWhenSuccess, modExtension.hours, modExtension.ticks/2);
                        pawnAround.health.AddHediff(hediff);
                    }
                }
                Messages.Message("Makai_FailArollcheck".Translate(pawn.LabelShort, rollinfo.baseRoll, rollinfo.cumulativeBonusRoll, pawn.Named("USER")), pawn, MessageTypeDefOf.NegativeEvent);
                Messages.Message("Makai_FailArollcheckHeavenlyChain".Translate(pawn.LabelShort, pawn.Named("USER")), pawn, MessageTypeDefOf.NegativeEvent);
            }
        }
    }
}
