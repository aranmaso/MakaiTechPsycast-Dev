using RimWorld.Planet;
using HarmonyLib;
using VanillaPsycastsExpanded;
using VFECore.Abilities;
using RimWorld;
using Verse;

namespace MakaiTechPsycast.BondIntertwined
{
    public class Ability_RelationMend : VFECore.Abilities.Ability
    {
		private static readonly AccessTools.FieldRef<Pawn_PsychicEntropyTracker, float> currentEntropy = AccessTools.FieldRefAccess<Pawn_PsychicEntropyTracker, float>("currentEntropy");
		public override void Cast(params GlobalTargetInfo[] targets)
		{
			base.Cast(targets);
			if (targets[0].Thing is Pawn pawn1 && targets[1].Thing is Pawn pawn2)
			{
				AbilityExtension_Roll1D20 modExtension = def.GetModExtension<AbilityExtension_Roll1D20>();
				SkillRecord bonus = pawn1.skills.GetSkill(modExtension.skillBonus);
				System.Random rand = new System.Random();
				int roll = rand.Next(1, 21);
				int rollBonus = bonus.Level / 5;
				int baseRoll = roll;
				int rollBonusLucky = 0;
				int rollBonusUnLucky = 0;
				if (pawn1.health.hediffSet.HasHediff(VPE_DefOf.VPE_Lucky))
				{
					rollBonusLucky = 20;
				}
				if (pawn1.health.hediffSet.HasHediff(VPE_DefOf.VPE_UnLucky))
				{
					rollBonusUnLucky = -20;
				}
				roll += rollBonus + rollBonusLucky + rollBonusUnLucky;
				int cumulativeBonusRoll = rollBonus + rollBonusLucky + rollBonusUnLucky;
				if (roll > modExtension.successThreshold && roll < modExtension.greatSuccessThreshold)
                {
					if(pawn1.relations.OpinionOf(pawn2) < 50)
                    {
						pawn1.needs.mood.thoughts.memories.TryGainMemory(modExtension.memoryDefWhenSuccess, pawn2);
						pawn2.needs.mood.thoughts.memories.TryGainMemory(modExtension.memoryDefWhenSuccess, pawn1);
						Messages.Message("Makai_PassArollcheck".Translate(pawn.LabelShort, baseRoll, cumulativeBonusRoll, pawn.Named("USER")), pawn, MessageTypeDefOf.PositiveEvent);
						Messages.Message("Makai_PassArollcheckMendRelation".Translate(pawn1.LabelShort, pawn2.LabelShort, pawn1.Named("USER1"), pawn2.Named("USER2")), pawn2, MessageTypeDefOf.PositiveEvent);
					}
					else
                    {
						Messages.Message("RelationTooHigh".Translate(pawn1.LabelShort,pawn2.LabelShort,pawn1.Named("USER1"), pawn2.Named("USER2")), MessageTypeDefOf.NegativeEvent);
                    }
                }
				if (roll >= modExtension.greatSuccessThreshold)
				{
					if (pawn1.relations.OpinionOf(pawn2) < 75)
					{
						pawn1.needs.mood.thoughts.memories.TryGainMemory(modExtension.memoryDefWhenGreatSuccess, pawn2);
						pawn2.needs.mood.thoughts.memories.TryGainMemory(modExtension.memoryDefWhenGreatSuccess, pawn1);
						pawn.psychicEntropy.OffsetPsyfocusDirectly(0.5f);
						currentEntropy(pawn.psychicEntropy) -= 5f;
						Messages.Message("Makai_GreatPassArollcheck".Translate(pawn.LabelShort, baseRoll, cumulativeBonusRoll, pawn.Named("USER")), pawn, MessageTypeDefOf.PositiveEvent);
						Messages.Message("Makai_GreatPassArollcheckMendRelation".Translate(pawn1.LabelShort, pawn2.LabelShort,pawn1.Named("USER1"), pawn2.Named("USER2")), pawn2, MessageTypeDefOf.PositiveEvent);
					}
					else
					{
						Messages.Message("RelationTooHigh".Translate(pawn1.LabelShort, pawn2.LabelShort, pawn1.Named("USER1"), pawn2.Named("USER2")), MessageTypeDefOf.NegativeEvent);
					}
				}
				if (roll < modExtension.successThreshold)
				{
					if (pawn1.relations.OpinionOf(pawn2) < 25)
					{
						pawn1.needs.mood.thoughts.memories.TryGainMemory(modExtension.memoryDefWhenGreatSuccess, pawn2);
						pawn2.needs.mood.thoughts.memories.TryGainMemory(modExtension.memoryDefWhenGreatSuccess, pawn1);
						Messages.Message("Makai_FailArollcheck".Translate(pawn.LabelShort, baseRoll, cumulativeBonusRoll, pawn.Named("USER")), pawn, MessageTypeDefOf.NegativeEvent);
						Messages.Message("Makai_FailArollcheckMendRelation".Translate(pawn1.LabelShort, pawn2.LabelShort, pawn1.Named("USER1"), pawn2.Named("USER2")), pawn2, MessageTypeDefOf.NegativeEvent);
					}
					else
					{
						Messages.Message("RelationTooHigh".Translate(pawn1.LabelShort, pawn2.LabelShort, pawn1.Named("USER1"), pawn2.Named("USER2")), MessageTypeDefOf.NegativeEvent);
					}
				}
			}
		}
	}
}
