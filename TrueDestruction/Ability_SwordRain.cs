using RimWorld;
using RimWorld.Planet;
using Verse;
using Verse.Sound;
using VFECore;
using VanillaPsycastsExpanded;
using UnityEngine;

namespace MakaiTechPsycast.TrueDestruction
{
    public class Ability_SwordRain : VFECore.Abilities.Ability
    {
        public int shotLeft;

        public int tickUntilNextShot;
        public AbilityExtension_Roll1D20 modExtension => def.GetModExtension<AbilityExtension_Roll1D20>();

        private GlobalTargetInfo targetInfo;

        private Thing targetThing;
        public override void Tick()
        {           
            if(shotLeft > 0)
            {
                tickUntilNextShot--;
                if (tickUntilNextShot <= 0)
                {
                    DoShot(pawn, targetInfo, targetThing);
                    shotLeft--;
                    tickUntilNextShot = modExtension.tickBurstInterval;
                }            
            }
        }
        public void DoShot(Pawn instigator, GlobalTargetInfo globalTargetInfo,Thing targetThing = null)
        {
            IntVec3 spawnPosition = MakaiUtility.RandomCellAroundCellBase(globalTargetInfo.Cell,-4,4);
            IntVec3 spawnPositionOffset = spawnPosition;
            spawnPositionOffset.z += 10;
            Projectile projectile = (Projectile)GenSpawn.Spawn(modExtension.projectileWhenSuccess, spawnPositionOffset, instigator.Map);
            if (targetThing != null)
            {
                float rand = Rand.Value;
                if (rand <= 0.5f)
                {
                    projectile?.Launch(instigator, spawnPositionOffset.ToVector3(), targetThing, targetThing, ProjectileHitFlags.IntendedTarget);
                    MakaiTechPsy_DefOf.Shot_ChargeBlaster.PlayOneShot(new TargetInfo(targetThing.Position, instigator.Map));
                }
                else
                {
                    projectile?.Launch(instigator, spawnPositionOffset.ToVector3(), spawnPosition, spawnPosition, ProjectileHitFlags.IntendedTarget);
                    MakaiTechPsy_DefOf.Shot_ChargeBlaster.PlayOneShot(new TargetInfo(targetThing.Position, instigator.Map));
                }                
            }
            else
            {
                projectile?.Launch(instigator, spawnPositionOffset.ToVector3(), spawnPosition, spawnPosition, ProjectileHitFlags.IntendedTarget);
                MakaiTechPsy_DefOf.Shot_ChargeBlaster.PlayOneShot(new TargetInfo(spawnPosition, instigator.Map));
            }
            //pawn.Map.weatherManager.eventHandler.AddEvent(new WeatherEvent_LightningStrike(instigator.Map, spawnPosition));
        }
        public override void Cast(params GlobalTargetInfo[] targets)
        {
            base.Cast(targets);            
            RollInfo rollinfo = new RollInfo();
            rollinfo = MakaiUtility.Roll1D20(pawn, modExtension.skillBonus, rollinfo,modExtension.skillBonus2);
            if (rollinfo.roll >= modExtension.successThreshold && rollinfo.roll < modExtension.greatSuccessThreshold)
            {
                shotLeft = modExtension.projectileBurstCount;
                tickUntilNextShot = modExtension.tickBurstInterval;
                targetThing = targets[0].Thing;
                targetInfo = targets[0];

                Messages.Message("Makai_PassArollcheck".Translate(pawn.LabelShort, rollinfo.baseRoll, rollinfo.cumulativeBonusRoll, pawn.Named("USER")), pawn, MessageTypeDefOf.PositiveEvent);
                Messages.Message("Makai_PassArollcheckSwordRain".Translate(pawn.LabelShort, shotLeft, pawn.Named("USER")), pawn, MessageTypeDefOf.PositiveEvent);
            }
            if (rollinfo.roll >= modExtension.greatSuccessThreshold)
            {
                shotLeft = modExtension.projectileBurstCount * 2;
                tickUntilNextShot = modExtension.tickBurstInterval;
                targetThing = targets[0].Thing;
                targetInfo = targets[0];
                Messages.Message("Makai_GreatPassArollcheck".Translate(pawn.LabelShort, rollinfo.baseRoll, rollinfo.cumulativeBonusRoll, pawn.Named("USER")), pawn, MessageTypeDefOf.PositiveEvent);
                Messages.Message("Makai_GreatPassArollcheckSwordRain".Translate(pawn.LabelShort, shotLeft, pawn.Named("USER")), pawn, MessageTypeDefOf.PositiveEvent);
            }
            if (rollinfo.roll < modExtension.successThreshold)
            {
                shotLeft = Mathf.RoundToInt(modExtension.projectileBurstCount / 2);
                tickUntilNextShot = modExtension.tickBurstInterval;
                targetThing = targets[0].Thing;
                targetInfo = targets[0];
                Messages.Message("Makai_FailArollcheck".Translate(pawn.LabelShort, rollinfo.baseRoll, rollinfo.cumulativeBonusRoll, pawn.Named("USER")), pawn, MessageTypeDefOf.NegativeEvent);
                Messages.Message("Makai_FailArollcheckSwordRain".Translate(pawn.LabelShort, shotLeft, pawn.Named("USER")), pawn, MessageTypeDefOf.NegativeEvent);
            }
        }
    }
}
