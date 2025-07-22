using RimWorld;
using RimWorld.Planet;
using Verse;
using Verse.Sound;
using VEF;
using VanillaPsycastsExpanded;
using UnityEngine;

namespace MakaiTechPsycast.PassionedFlame
{
    public class Ability_ShootFromAroundTarget : VEF.Abilities.Ability
    {
        public int shotLeft = 0;

        public int tickUntilNextShot;

        public AbilityExtension_Roll1D20 modExtension => def.GetModExtension<AbilityExtension_Roll1D20>();

        private GlobalTargetInfo targetInfo;

        private Thing targetThing;

        private ThingDef projectileDef;

        public override void Tick()
        {
            if (shotLeft > 0)
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

        public void DoShot(Pawn instigator, GlobalTargetInfo globalTargetInfo, Thing targetThing = null)
        {
            IntVec3 spawnPosition = GenRadial.RadialCellsAround(globalTargetInfo.Cell,5,true).RandomElement();
            Projectile projectile = (Projectile)GenSpawn.Spawn(projectileDef, spawnPosition, instigator.Map);
            if (targetThing != null)
            {
                projectile?.Launch(instigator, spawnPosition.ToVector3Shifted(), targetThing, targetThing, ProjectileHitFlags.IntendedTarget);
                MakaiUtility.ThrowFleckStationary(MakaiTechPsy_DefOf.MakaiPsyMote_ReflectProjectile,spawnPosition.ToVector3Shifted(),instigator.Map,2);               
                MakaiTechPsy_DefOf.Shot_ChargeBlaster.PlayOneShot(new TargetInfo(targetThing.Position, instigator.Map));
            }
            //pawn.Map.weatherManager.eventHandler.AddEvent(new WeatherEvent_LightningStrike(instigator.Map, spawnPosition));
        }
        public override void Cast(params GlobalTargetInfo[] targets)
        {
            base.Cast(targets);
            RollInfo rollinfo = new RollInfo();
            rollinfo = MakaiUtility.Roll1D20PassionCount(pawn, rollinfo);
            if (rollinfo.roll >= modExtension.successThreshold && rollinfo.roll < modExtension.greatSuccessThreshold)
            {
                if (targets[0].Thing is Pawn targetPawn)
                {
                    foreach (SkillRecord item in targetPawn.skills.skills)
                    {
                        if (item.passion != Passion.None)
                        {
                            shotLeft++;
                        }
                    }
                    if(shotLeft==0)
                    {
                        foreach (SkillRecord item in pawn.skills.skills)
                        {
                            if (item.passion != Passion.None)
                            {
                                shotLeft++;
                            }
                        }
                    }
                    tickUntilNextShot = modExtension.tickBurstInterval;
                    targetThing = targets[0].Thing;
                    targetInfo = targets[0];
                    projectileDef = modExtension.projectileWhenSuccess;
                    Messages.Message("Makai_PassArollcheck".Translate(pawn.LabelShort, rollinfo.baseRoll, rollinfo.cumulativeBonusRoll, pawn.Named("USER")), pawn, MessageTypeDefOf.PositiveEvent);
                    Messages.Message("Makai_PassArollcheckPinncer".Translate(pawn.LabelShort,targetPawn.LabelShort, pawn.Named("USER"), pawn.Named("USER2")), pawn, MessageTypeDefOf.PositiveEvent);
                }
            }
            if (rollinfo.roll >= modExtension.greatSuccessThreshold)
            {
                if (targets[0].Thing is Pawn targetPawn)
                {
                    foreach (SkillRecord item in targetPawn.skills.skills)
                    {
                        if (item.passion != Passion.None)
                        {
                            shotLeft++;
                        }
                    }
                    if (shotLeft == 0)
                    {
                        foreach (SkillRecord item in pawn.skills.skills)
                        {
                            if (item.passion != Passion.None)
                            {
                                shotLeft++;
                            }
                        }
                    }
                    tickUntilNextShot = modExtension.tickBurstInterval;
                    targetThing = targets[0].Thing;
                    targetInfo = targets[0];
                    projectileDef = modExtension.projectileWhenGreatSuccess;
                    Messages.Message("Makai_GreatPassArollcheck".Translate(pawn.LabelShort, rollinfo.baseRoll, rollinfo.cumulativeBonusRoll, pawn.Named("USER")), pawn, MessageTypeDefOf.PositiveEvent);
                    Messages.Message("Makai_GreatPassArollcheckPinncer".Translate(pawn.LabelShort, targetPawn.LabelShort, pawn.Named("USER"), pawn.Named("USER2")), pawn, MessageTypeDefOf.PositiveEvent);
                }
            }
            if (rollinfo.roll < modExtension.successThreshold)
            {
                if (targets[0].Thing is Pawn targetPawn)
                {
                    foreach (SkillRecord item in targetPawn.skills.skills)
                    {
                        if (item.passion != Passion.None)
                        {
                            shotLeft++;
                        }
                    }
                    if (shotLeft == 0)
                    {
                        foreach (SkillRecord item in pawn.skills.skills)
                        {
                            if (item.passion != Passion.None)
                            {
                                shotLeft++;
                            }
                        }
                    }
                    tickUntilNextShot = modExtension.tickBurstInterval;
                    targetThing = targets[0].Thing;
                    targetInfo = targets[0];
                    projectileDef = modExtension.projectileWhenFail;
                    Messages.Message("Makai_FailArollcheck".Translate(pawn.LabelShort, rollinfo.baseRoll, rollinfo.cumulativeBonusRoll, pawn.Named("USER")), pawn, MessageTypeDefOf.NegativeEvent);
                    Messages.Message("Makai_FailArollcheckPinncer".Translate(pawn.LabelShort, targetPawn.LabelShort, pawn.Named("USER"), pawn.Named("USER2")), pawn, MessageTypeDefOf.NegativeEvent);
                }
            }
        }
    }
}
