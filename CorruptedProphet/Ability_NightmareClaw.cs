using RimWorld.Planet;
using UnityEngine;
using System.Collections.Generic;
using VanillaPsycastsExpanded;
using VEF.Abilities;
using RimWorld;
using Verse;

namespace MakaiTechPsycast.CorruptedProphet
{
    public class Ability_NightmareClaw : VEF.Abilities.Ability
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
					MakaiCP_PowerBeam orbitalStrike = (MakaiCP_PowerBeam)GenSpawn.Spawn(MakaiTechPsy_DefOf.MakaiPsy_CP_Beam, globalTargetInfo.Cell, pawn.Map);
					orbitalStrike.duration = GetDurationForPawn();
					orbitalStrike.instigator = pawn;
					orbitalStrike.StartStrike();
					pawn.health.hediffSet.GetFirstHediffOfDef(MakaiTechPsy_DefOf.MakaiPsy_CP_TaintLevel).Severity -= modExtension.costs;
					Messages.Message("Makai_PassArollcheck".Translate(pawn.LabelShort, baseRoll, cumulativeBonusRoll, pawn.Named("USER")), pawn, MessageTypeDefOf.PositiveEvent);
					Messages.Message("Makai_PassArollcheckBeam".Translate(pawn.LabelShort, modExtension.costs, pawn.Named("USER")), pawn, MessageTypeDefOf.PositiveEvent);
				}
			}
			if (pawn.health.hediffSet.HasHediff(MakaiTechPsy_DefOf.MakaiPsy_CP_TaintLevel) && roll >= modExtension.greatSuccessThreshold && pawn.health.hediffSet.GetFirstHediffOfDef(MakaiTechPsy_DefOf.MakaiPsy_CP_TaintLevel).Severity >= modExtension.costs)
            {
				foreach (GlobalTargetInfo globalTargetInfo in targets)
				{
					MakaiCP_PowerBeam orbitalStrike = (MakaiCP_PowerBeam)GenSpawn.Spawn(MakaiTechPsy_DefOf.MakaiPsy_CP_Beam, globalTargetInfo.Cell, pawn.Map);
					orbitalStrike.duration = GetDurationForPawn()*2;
					orbitalStrike.instigator = pawn;
					orbitalStrike.StartStrike();
					pawn.health.hediffSet.GetFirstHediffOfDef(MakaiTechPsy_DefOf.MakaiPsy_CP_TaintLevel).Severity -= modExtension.costs;
					Messages.Message("Makai_GreatPassArollcheck".Translate(pawn.LabelShort, baseRoll, cumulativeBonusRoll, pawn.Named("USER")), pawn, MessageTypeDefOf.PositiveEvent);
					Messages.Message("Makai_GreatPassArollcheckBeam".Translate(pawn.LabelShort, modExtension.costs, pawn.Named("USER")), pawn, MessageTypeDefOf.PositiveEvent);
				}
			}
			if (pawn.health.hediffSet.HasHediff(MakaiTechPsy_DefOf.MakaiPsy_CP_TaintLevel) && roll < modExtension.successThreshold && pawn.health.hediffSet.GetFirstHediffOfDef(MakaiTechPsy_DefOf.MakaiPsy_CP_TaintLevel).Severity >= modExtension.costs)
			{
				foreach (GlobalTargetInfo globalTargetInfo in targets)
				{
					MakaiCP_PowerBeam orbitalStrike = (MakaiCP_PowerBeam)GenSpawn.Spawn(MakaiTechPsy_DefOf.MakaiPsy_CP_Beam, globalTargetInfo.Cell, pawn.Map);
					orbitalStrike.duration = GetDurationForPawn();
					orbitalStrike.instigator = pawn;
					orbitalStrike.StartStrike();
					pawn.health.hediffSet.GetFirstHediffOfDef(MakaiTechPsy_DefOf.MakaiPsy_CP_TaintLevel).Severity -= modExtension.costs;
					Messages.Message("Makai_FailArollcheck".Translate(pawn.LabelShort, baseRoll, cumulativeBonusRoll, pawn.Named("USER")), pawn, MessageTypeDefOf.PositiveEvent);
					Messages.Message("Makai_FailArollcheckBeam".Translate(pawn.LabelShort, modExtension.costs, pawn.Named("USER")), pawn, MessageTypeDefOf.PositiveEvent);
				}
			}
			if (!pawn.health.hediffSet.HasHediff(MakaiTechPsy_DefOf.MakaiPsy_CP_TaintLevel) || pawn.health.hediffSet.GetFirstHediffOfDef(MakaiTechPsy_DefOf.MakaiPsy_CP_TaintLevel).Severity < modExtension.costs)
			{
				Messages.Message("Makai_CP_NotEnoughTaint".Translate(pawn.LabelShort, pawn.Named("USER")), pawn, MessageTypeDefOf.NegativeEvent);
			}
		}
	}
}
