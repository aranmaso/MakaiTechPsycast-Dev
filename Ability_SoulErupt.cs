using RimWorld.Planet;
using UnityEngine;
using HarmonyLib;
using System.Collections.Generic;
using VanillaPsycastsExpanded;
using VFECore.Abilities;
using RimWorld;
using Verse;
using System.Linq;

namespace MakaiTechPsycast.DestinedDeath
{
    public class Ability_SoulErupt : VFECore.Abilities.Ability
    {
        public override void Cast(params GlobalTargetInfo[] targets)
        {
			base.Cast(targets);
			AbilityExtension_Roll1D20 modExtension = def.GetModExtension<AbilityExtension_Roll1D20>();
			SkillRecord bonus = pawn.skills.GetSkill(modExtension.skillBonus);
			System.Random rand = new System.Random();
			int roll = rand.Next(1, 21);
			int rollBonus = bonus.Level / 5;
			int baseRoll = roll;
			int rollBonusLucky = 0;
			int rollBonusUnLucky = 0;
			if (pawn.health.hediffSet.HasHediff(VPE_DefOf.VPE_Lucky))
			{
				rollBonusLucky = 20;
			}
			if (pawn.health.hediffSet.HasHediff(VPE_DefOf.VPE_UnLucky))
			{
				rollBonusUnLucky = -20;
			}
			roll += rollBonus + rollBonusLucky + rollBonusUnLucky;
			int cumulativeBonusRoll = rollBonus + rollBonusLucky + rollBonusUnLucky;
			if(roll >= modExtension.successThreshold && roll < modExtension.greatSuccessThreshold)
            {
                if (targets[0].Thing is Pawn targetPawn)
                {
					if(!targetPawn.health.hediffSet.HasHediff(MakaiTechPsy_DefOf.MakaiTechPsy_DD_MissingSoul))
                    {
						Effecter effect = MakaiTechPsy_DefOf.MakaiPsy_DD_Blast.Spawn(targetPawn.Position, pawn.Map, 0.5f);
						effect.Cleanup();
						GenExplosion.DoExplosion(targetPawn.Position, pawn.Map, 1.9f, modExtension.damageDef, pawn, modExtension.damageAmount, 1, null, null, null, targetPawn, null, 0, 0, GasType.RotStink);
						Messages.Message("Makai_PassArollcheck".Translate(pawn.LabelShort, baseRoll, cumulativeBonusRoll, pawn.Named("USER")), pawn, MessageTypeDefOf.PositiveEvent);
						Messages.Message("Makai_PassArollcheckSoulSurge".Translate(pawn.LabelShort, pawn.Named("USER")), pawn, MessageTypeDefOf.PositiveEvent);
					}
					else if (targetPawn.health.hediffSet.HasHediff(MakaiTechPsy_DefOf.MakaiTechPsy_DD_MissingSoul))
					{
						Messages.Message("Makai_SoulSurgeFail".Translate(pawn.LabelShort,targetPawn.LabelShort, pawn.Named("USER")), pawn, MessageTypeDefOf.NegativeEvent);
					}
				}
            }
			if (roll >= modExtension.greatSuccessThreshold)
			{
				if (targets[0].Thing is Pawn targetPawn)
				{
					if (!targetPawn.health.hediffSet.HasHediff(MakaiTechPsy_DefOf.MakaiTechPsy_DD_MissingSoul))
					{
						Effecter effect = MakaiTechPsy_DefOf.MakaiPsy_DD_Blast.Spawn(targetPawn.Position, pawn.Map, 0.5f);
						effect.Cleanup();
						GenExplosion.DoExplosion(targetPawn.Position, pawn.Map, 3.9f, modExtension.damageDef, pawn, modExtension.damageAmount*2, 1, null, null, null, targetPawn, null, 0, 0, GasType.RotStink);
						Messages.Message("Makai_GreatPassArollcheck".Translate(pawn.LabelShort, baseRoll, cumulativeBonusRoll, pawn.Named("USER")), pawn, MessageTypeDefOf.PositiveEvent);
						Messages.Message("Makai_GreatPassArollcheckSoulSurge".Translate(pawn.LabelShort, pawn.Named("USER")), pawn, MessageTypeDefOf.PositiveEvent);
					}
					else if (targetPawn.health.hediffSet.HasHediff(MakaiTechPsy_DefOf.MakaiTechPsy_DD_MissingSoul))
					{
						Messages.Message("Makai_SoulSurgeFail".Translate(pawn.LabelShort, targetPawn.LabelShort, pawn.Named("USER")), pawn, MessageTypeDefOf.NegativeEvent);
					}
				}
			}
			if (roll < modExtension.successThreshold)
			{
				if (targets[0].Thing is Pawn targetPawn)
				{
					if (!targetPawn.health.hediffSet.HasHediff(MakaiTechPsy_DefOf.MakaiTechPsy_DD_MissingSoul))
					{
						Effecter effect = MakaiTechPsy_DefOf.MakaiPsy_DD_Blast.Spawn(targetPawn.Position, pawn.Map, 0.5f);
						effect.Cleanup();
						GenExplosion.DoExplosion(targetPawn.Position, pawn.Map, 1f, modExtension.damageDef, pawn, modExtension.damageAmount, 1, null, null, null, targetPawn, null, 0, 0, GasType.RotStink);
						Messages.Message("Makai_FailArollcheck".Translate(pawn.LabelShort, baseRoll, cumulativeBonusRoll, pawn.Named("USER")), pawn, MessageTypeDefOf.PositiveEvent);
						Messages.Message("Makai_FailArollcheckSoulSurge".Translate(pawn.LabelShort, pawn.Named("USER")), pawn, MessageTypeDefOf.NegativeEvent);
					}
					else if (targetPawn.health.hediffSet.HasHediff(MakaiTechPsy_DefOf.MakaiTechPsy_DD_MissingSoul))
					{
						Messages.Message("Makai_SoulSurgeFail".Translate(pawn.LabelShort, targetPawn.LabelShort, pawn.Named("USER")), pawn, MessageTypeDefOf.NegativeEvent);
					}
				}
			}
		}
    }
}
