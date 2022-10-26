using RimWorld.Planet;
using VanillaPsycastsExpanded;
using VFECore.Abilities;
using UnityEngine;
using Verse;
using RimWorld;
using System;

namespace MakaiTechPsycast.BondIntertwined
{
	public class AbilityExtension_MindCleanse : AbilityExtension_AbilityMod
	{
		public HediffDef hediffDefWhenFail;

		public HediffDef hediffDefWhenGreatSuccess;

		public ThoughtDef memoryDefWhenFail;

		public StatDef multiplier;

		public float hours = 1f;

		public int ticks;

		public SkillDef skillBonus;
		public override void Cast(GlobalTargetInfo[] targets, VFECore.Abilities.Ability ability)
		{
			base.Cast(targets, ability);
			if (targets[0].Thing is Pawn pawn2)
			{
				SkillRecord bonus = ability.pawn.skills.GetSkill(skillBonus);
				System.Random rand = new System.Random();
				int roll = rand.Next(1, 21);
				int rollBonus = bonus.levelInt/5;
				int baseRoll = roll;
				int rollBonusLucky = 0;
				int rollBonusUnLucky = 0;
				if (ability.pawn.health.hediffSet.HasHediff(VPE_DefOf.VPE_Lucky))
                {
					rollBonusLucky = 20;
                }
				if (ability.pawn.health.hediffSet.HasHediff(VPE_DefOf.VPE_UnLucky))
				{
					rollBonusUnLucky = -20;
				}
				roll += rollBonus + rollBonusLucky + rollBonusUnLucky;
				int cumulativeBonusRoll = rollBonus + rollBonusLucky + rollBonusUnLucky;
				if (roll >= 5 && roll < 15 && pawn2 != null)
				{
					pawn2.needs.mood.thoughts.memories.Memories.Clear();
					Messages.Message("Makai_PassArollcheck".Translate(ability.pawn.LabelShort, baseRoll, cumulativeBonusRoll, ability.pawn.Named("USER")), ability.pawn, MessageTypeDefOf.PositiveEvent);
					Messages.Message("Makai_PassArollcheckCleanse".Translate(pawn2.LabelShort, pawn2.Named("USER2")), pawn2, MessageTypeDefOf.PositiveEvent);
				}
				if (roll > 15 && pawn2 != null)
				{
					pawn2.needs.mood.thoughts.memories.Memories.Clear();
					float num = hours * 2500f + (float)ticks;
					float statValue = ability.pawn.GetStatValue(multiplier ?? StatDefOf.PsychicSensitivity);
					num *= statValue;
					Hediff hediff = HediffMaker.MakeHediff(hediffDefWhenGreatSuccess ?? VPE_DefOf.PsychicComa, pawn2);
					hediff.TryGetComp<HediffComp_Disappears>().ticksToDisappear = Mathf.FloorToInt(num);
					pawn2.health.AddHediff(hediff);
					Messages.Message("Makai_GreatPassArollcheck".Translate(ability.pawn.LabelShort, baseRoll, cumulativeBonusRoll, ability.pawn.Named("USER")), ability.pawn, MessageTypeDefOf.PositiveEvent);
					Messages.Message("Makai_GreatPassArollcheckCleanse".Translate(pawn2.LabelShort, pawn2.Named("USER2")), pawn2, MessageTypeDefOf.PositiveEvent);
				}
				if (roll <= 4 && pawn2 != null)
				{
					pawn2.relations.ClearAllRelations();

					Hediff hediff = HediffMaker.MakeHediff(hediffDefWhenFail ?? VPE_DefOf.PsychicComa, pawn2);
					pawn2.health.AddHediff(hediff);

					pawn2.needs.mood.thoughts.memories.TryGainMemory(memoryDefWhenFail, ability.pawn);
					Messages.Message("Makai_FailArollcheck".Translate(ability.pawn.LabelShort, baseRoll, cumulativeBonusRoll, ability.pawn.Named("USER")), ability.pawn, MessageTypeDefOf.PositiveEvent);
					Messages.Message("Makai_FailArollcheckCleanse".Translate(pawn2.LabelShort, pawn2.Named("USER2")), pawn2, MessageTypeDefOf.PositiveEvent);
				}
			}
		}
	}

}
