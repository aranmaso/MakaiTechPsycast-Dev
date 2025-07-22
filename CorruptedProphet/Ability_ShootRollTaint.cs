using RimWorld.Planet;
using UnityEngine;
using System.Collections.Generic;
using VanillaPsycastsExpanded;
using VEF.Abilities;
using RimWorld;
using Verse;

namespace MakaiTechPsycast.CorruptedProphet
{
    public class Ability_ShootRollTaint : VEF.Abilities.Ability
    {
        public override void Cast(params GlobalTargetInfo[] targets)
        {
			base.Cast(targets);
			AbilityExtension_Roll1D20 modExtension = def.GetModExtension<AbilityExtension_Roll1D20>();
            RollInfo rollInfo = new RollInfo();
            rollInfo = pawn.R1D20(modExtension.skillBonus, modExtension.skillBonus2 ?? null);
            int baseRoll = rollInfo.baseRoll;
            int roll = rollInfo.roll;
            int cumulativeBonusRoll = rollInfo.cumulativeBonusRoll;
            if (pawn.health.hediffSet.HasHediff(MakaiTechPsy_DefOf.MakaiPsy_CP_TaintLevel) && roll >= modExtension.successThreshold && roll < modExtension.greatSuccessThreshold && pawn.health.hediffSet.GetFirstHediffOfDef(MakaiTechPsy_DefOf.MakaiPsy_CP_TaintLevel).Severity >= modExtension.costs)
            {
				foreach (GlobalTargetInfo globalTargetInfo in targets)
                {
					Projectile projectile = (Projectile)GenSpawn.Spawn(modExtension.projectileWhenSuccess, pawn.Position, pawn.Map);
					if (globalTargetInfo.Thing is Pawn pawn2)
					{
						projectile.Launch(pawn, pawn.DrawPos, pawn2, pawn2, ProjectileHitFlags.IntendedTarget);
					}
					else
					{
						projectile.Launch(pawn, pawn.DrawPos, globalTargetInfo.Cell, globalTargetInfo.Cell, ProjectileHitFlags.IntendedTarget);
					}
					pawn.health.hediffSet.GetFirstHediffOfDef(MakaiTechPsy_DefOf.MakaiPsy_CP_TaintLevel).Severity -= modExtension.costs;
					Messages.Message("Makai_PassArollcheck".Translate(pawn.LabelShort, baseRoll, cumulativeBonusRoll, pawn.Named("USER")), pawn, MessageTypeDefOf.PositiveEvent);
					Messages.Message("Makai_PassArollcheckGrab".Translate(pawn.LabelShort, modExtension.costs, pawn.Named("USER")), pawn, MessageTypeDefOf.PositiveEvent);
				}
			}
			if (pawn.health.hediffSet.HasHediff(MakaiTechPsy_DefOf.MakaiPsy_CP_TaintLevel) && roll >= modExtension.greatSuccessThreshold && pawn.health.hediffSet.GetFirstHediffOfDef(MakaiTechPsy_DefOf.MakaiPsy_CP_TaintLevel).Severity >= modExtension.costs)
			{
				foreach (GlobalTargetInfo globalTargetInfo in targets)
				{
					Projectile projectile = (Projectile)GenSpawn.Spawn(modExtension.projectileWhenGreatSuccess, Caster.Position, pawn.Map);
					if (globalTargetInfo.Thing is Pawn pawn2)
					{
						projectile.Launch(pawn, pawn.DrawPos, pawn2, pawn2, ProjectileHitFlags.IntendedTarget);
						IntVec3 intVec = pawn.OccupiedRect().AdjacentCells.MinBy((IntVec3 cell) => cell.DistanceTo(pawn2.Position));
						PawnFlyer_Pulled pawnFlyer_Pulled = (PawnFlyer_Pulled)PawnFlyer.MakeFlyer(MakaiTechPsy_DefOf.MakaiPsy_Pull, pawn2, intVec,null,null);
						GenSpawn.Spawn(pawnFlyer_Pulled, intVec, pawn.Map);
					}
					else
					{
						projectile.Launch(pawn, pawn.DrawPos, globalTargetInfo.Cell, globalTargetInfo.Cell, ProjectileHitFlags.IntendedTarget);
					}
					pawn.health.hediffSet.GetFirstHediffOfDef(MakaiTechPsy_DefOf.MakaiPsy_CP_TaintLevel).Severity -= modExtension.costs;
					Messages.Message("Makai_GreatPassArollcheck".Translate(pawn.LabelShort, baseRoll, cumulativeBonusRoll, pawn.Named("USER")), pawn, MessageTypeDefOf.PositiveEvent);
					Messages.Message("Makai_GreatPassArollcheckGrab".Translate(pawn.LabelShort, modExtension.costs, pawn.Named("USER")), pawn, MessageTypeDefOf.PositiveEvent);
				}
			}
			if (pawn.health.hediffSet.HasHediff(MakaiTechPsy_DefOf.MakaiPsy_CP_TaintLevel) && roll < modExtension.successThreshold && pawn.health.hediffSet.GetFirstHediffOfDef(MakaiTechPsy_DefOf.MakaiPsy_CP_TaintLevel).Severity >= modExtension.costs)
			{
				foreach (GlobalTargetInfo globalTargetInfo in targets)
				{
					Projectile projectile = (Projectile)GenSpawn.Spawn(modExtension.projectileWhenFail, globalTargetInfo.Cell, pawn.Map);
					if (globalTargetInfo.Thing is Pawn pawn2)
					{
						projectile.Launch(pawn2, pawn2.DrawPos, pawn, pawn, ProjectileHitFlags.IntendedTarget);
					}
					else
					{
						projectile.Launch(pawn, pawn.DrawPos, globalTargetInfo.Cell, globalTargetInfo.Cell, ProjectileHitFlags.IntendedTarget);
					}
					pawn.health.hediffSet.GetFirstHediffOfDef(MakaiTechPsy_DefOf.MakaiPsy_CP_TaintLevel).Severity -= modExtension.costs;
					Messages.Message("Makai_FailArollcheck".Translate(pawn.LabelShort, baseRoll, cumulativeBonusRoll, pawn.Named("USER")), pawn, MessageTypeDefOf.PositiveEvent);
					Messages.Message("Makai_FailArollcheckGrab".Translate(pawn.LabelShort, modExtension.costs, pawn.Named("USER")), pawn, MessageTypeDefOf.PositiveEvent);
				}
			}
			if (!pawn.health.hediffSet.HasHediff(MakaiTechPsy_DefOf.MakaiPsy_CP_TaintLevel) || pawn.health.hediffSet.GetFirstHediffOfDef(MakaiTechPsy_DefOf.MakaiPsy_CP_TaintLevel).Severity < modExtension.costs)
			{
				Messages.Message("Makai_CP_NotEnoughTaint".Translate(pawn.LabelShort, pawn.Named("USER")), pawn, MessageTypeDefOf.NegativeEvent);
			}
		}
    }
}
