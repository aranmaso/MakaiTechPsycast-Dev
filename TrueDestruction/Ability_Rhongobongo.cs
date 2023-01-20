using RimWorld;
using RimWorld.Planet;
using Verse;
using VFECore;
using VanillaPsycastsExpanded;
using UnityEngine;

namespace MakaiTechPsycast.TrueDestruction
{
    public class Ability_Rhongobongo : VFECore.Abilities.Ability
    {
        public AbilityExtension_Roll1D20 modExtension => def.GetModExtension<AbilityExtension_Roll1D20>();
        public override void Cast(params GlobalTargetInfo[] targets)
        {
            base.Cast(targets);            
            RollInfo rollinfo = new RollInfo();
            rollinfo = MakaiUtility.Roll1D20(pawn, modExtension.skillBonus, rollinfo, modExtension.skillBonus2);
            if (rollinfo.roll >= modExtension.successThreshold && rollinfo.roll < modExtension.greatSuccessThreshold)
            {
                foreach (GlobalTargetInfo globalTargetInfo in targets)
                {
                    Vector3 spawnPosition = new Vector3(globalTargetInfo.Cell.x, globalTargetInfo.Cell.y, globalTargetInfo.Cell.z + 15);
                    Projectile projectile = (Projectile)GenSpawn.Spawn(modExtension.projectileWhenSuccess, globalTargetInfo.Cell, pawn.Map);
                    Effecter effect = MakaiTechPsy_DefOf.MakaiPsy_SmokeExplosion.Spawn(spawnPosition.ToIntVec3(), pawn.Map, 0.5f);
                    effect.Cleanup();
                    if (globalTargetInfo.HasThing)
                    {
                        projectile.Launch(pawn, spawnPosition, globalTargetInfo.Thing, globalTargetInfo.Thing, ProjectileHitFlags.IntendedTarget);
                    }
                    else
                    {
                        projectile.Launch(pawn, spawnPosition, globalTargetInfo.Cell, globalTargetInfo.Cell, ProjectileHitFlags.IntendedTarget);
                    }
                }
                Messages.Message("Makai_PassArollcheck".Translate(pawn.LabelShort, rollinfo.baseRoll, rollinfo.cumulativeBonusRoll, pawn.Named("USER")), pawn, MessageTypeDefOf.PositiveEvent);
                Messages.Message("Makai_PassArollcheckRhongo".Translate(pawn.LabelShort, pawn.Named("USER")), pawn, MessageTypeDefOf.PositiveEvent);
            }
            if (rollinfo.roll >= modExtension.greatSuccessThreshold)
            {
                foreach (GlobalTargetInfo globalTargetInfo in targets)
                {
                    Vector3 spawnPosition = new Vector3(globalTargetInfo.Cell.x, globalTargetInfo.Cell.y, globalTargetInfo.Cell.z + 15);
                    Projectile projectile = (Projectile)GenSpawn.Spawn(modExtension.projectileWhenSuccess, globalTargetInfo.Cell, pawn.Map);
                    Effecter effect = MakaiTechPsy_DefOf.MakaiPsy_SmokeExplosion.Spawn(spawnPosition.ToIntVec3(), pawn.Map, 0.5f);
                    effect.Cleanup();
                    if (globalTargetInfo.HasThing)
                    {
                        projectile.Launch(pawn, spawnPosition, globalTargetInfo.Thing, globalTargetInfo.Thing, ProjectileHitFlags.IntendedTarget);
                    }
                    else
                    {
                        projectile.Launch(pawn, spawnPosition, globalTargetInfo.Cell, globalTargetInfo.Cell, ProjectileHitFlags.IntendedTarget);
                    }
                }
                Messages.Message("Makai_GreatPassArollcheck".Translate(pawn.LabelShort, rollinfo.baseRoll, rollinfo.cumulativeBonusRoll, pawn.Named("USER")), pawn, MessageTypeDefOf.PositiveEvent);
                Messages.Message("Makai_GreatPassArollcheckRhongo".Translate(pawn.LabelShort, pawn.Named("USER")), pawn, MessageTypeDefOf.PositiveEvent);
            }
            if (rollinfo.roll < modExtension.successThreshold)
            {
                foreach (GlobalTargetInfo globalTargetInfo in targets)
                {
                    Vector3 spawnPosition = new Vector3(globalTargetInfo.Cell.x, globalTargetInfo.Cell.y, globalTargetInfo.Cell.z + 15);
                    Projectile projectile = (Projectile)GenSpawn.Spawn(modExtension.projectileWhenSuccess, globalTargetInfo.Cell, pawn.Map);
                    Effecter effect = MakaiTechPsy_DefOf.MakaiPsy_SmokeExplosion.Spawn(spawnPosition.ToIntVec3(), pawn.Map, 0.5f);
                    effect.Cleanup();
                    if (globalTargetInfo.HasThing)
                    {
                        projectile.Launch(pawn, spawnPosition, globalTargetInfo.Thing, globalTargetInfo.Thing, ProjectileHitFlags.IntendedTarget);
                    }
                    else
                    {
                        projectile.Launch(pawn, spawnPosition, globalTargetInfo.Cell, globalTargetInfo.Cell, ProjectileHitFlags.IntendedTarget);
                    }
                }
                Messages.Message("Makai_FailArollcheck".Translate(pawn.LabelShort, rollinfo.baseRoll, rollinfo.cumulativeBonusRoll, pawn.Named("USER")), pawn, MessageTypeDefOf.NegativeEvent);
                Messages.Message("Makai_FailArollcheckRhongo".Translate(pawn.LabelShort, pawn.Named("USER")), pawn, MessageTypeDefOf.NegativeEvent);
            }
        }
    }
}
