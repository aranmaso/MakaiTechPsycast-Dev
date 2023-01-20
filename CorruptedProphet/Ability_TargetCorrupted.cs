using RimWorld.Planet;
using UnityEngine;
using System.Collections.Generic;
using VanillaPsycastsExpanded;
using VFECore.Abilities;
using RimWorld;
using Verse;

namespace MakaiTechPsycast.CorruptedProphet
{
    public class Ability_TargetCorrupted : VFECore.Abilities.Ability
    {
		public IntVec3 targetCell;

		AbilityExtension_Roll1D20 modExtension => def.GetModExtension<AbilityExtension_Roll1D20>();
		public override bool ValidateTarget(LocalTargetInfo target, bool showMessages = true)
		{
			if (modExtension.targetOnlyEnemies && target.Thing != null && !target.Thing.HostileTo(pawn))
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
			if (pawn.health.hediffSet.HasHediff(MakaiTechPsy_DefOf.MakaiPsy_CP_TaintLevel) && roll >= modExtension.successThreshold && roll < modExtension.greatSuccessThreshold && pawn.health.hediffSet.GetFirstHediffOfDef(MakaiTechPsy_DefOf.MakaiPsy_CP_TaintLevel).Severity >= modExtension.costs)
            {
				foreach (GlobalTargetInfo globalTargetInfo in targets)
                {
					foreach (Thing thing in GenRadial.RadialDistinctThingsAround(globalTargetInfo.Cell, pawn.Map, def.radius, true))
					{
						if (thing is Pawn target && target.Faction != pawn.Faction)
						{
							/*target.mindState.mentalStateHandler.TryStartMentalState(MentalStateDefOf.PanicFlee);*/
							List<Apparel> apparels = target.apparel.WornApparel;
							float marketValue = 0;
							foreach (Apparel apparel in apparels)
							{
								if (apparel != null && apparel.MarketValue > 200)
								{
									marketValue += apparel.MarketValue;
								}
							}
							if (target.equipment.Primary != null)
							{
								float randomDropEquip = Rand.Value;
								if (randomDropEquip <= 0.25f)
								{
									ThingWithComps weapon = target.equipment.Primary;
									target.equipment.TryDropEquipment(weapon, out weapon, target.RandomAdjacentCell8Way());
									float damage = target.apparel.WornApparelCount;
									for (int i = 0; i < 1; i++)
									{
										target.TakeDamage(new DamageInfo(DamageDefOf.Cut, marketValue / 40, 1, -1, pawn, target.RaceProps.body.AllParts.FirstOrFallback((BodyPartRecord p) => p.def == BodyPartDefOf.Torso)));
									}
									Effecter effect = MakaiTechPsy_DefOf.MakaiPsy_TD_Blast.Spawn(target.Position, pawn.Map, 0.5f);
									effect.Cleanup();
								}
							}
							float randDropInv = Rand.Value;
							if (randDropInv <= 0.5f && (target.RaceProps.Humanlike || target.RaceProps.Animal) && (!target.RaceProps.IsMechanoid && !target.RaceProps.Insect))
                            {
								target.inventory.DropAllNearPawn(target.RandomAdjacentCell8Way());
								Faction.OfPlayer.TryAffectGoodwillWith(target.Faction, -10);
							}
							if (marketValue > 7000)
							{
								GenExplosion.DoExplosion(target.Position, pawn.Map, 2f, DamageDefOf.Bomb, null, 5);
							}
						}
                    }
				}
				pawn.health.hediffSet.GetFirstHediffOfDef(MakaiTechPsy_DefOf.MakaiPsy_CP_TaintLevel).Severity -= modExtension.costs;
				Messages.Message("Makai_PassArollcheck".Translate(pawn.LabelShort, baseRoll, cumulativeBonusRoll, pawn.Named("USER")), pawn, MessageTypeDefOf.PositiveEvent);
				Messages.Message("Makai_PassArollcheckTargetCorrupt".Translate(pawn.LabelShort, modExtension.costs, pawn.Named("USER")), pawn, MessageTypeDefOf.PositiveEvent);
			}
			if (pawn.health.hediffSet.HasHediff(MakaiTechPsy_DefOf.MakaiPsy_CP_TaintLevel) && roll >= modExtension.greatSuccessThreshold && pawn.health.hediffSet.GetFirstHediffOfDef(MakaiTechPsy_DefOf.MakaiPsy_CP_TaintLevel).Severity >= modExtension.costs)
            {
				foreach (GlobalTargetInfo globalTargetInfo in targets)
                {
					foreach (Thing thing in GenRadial.RadialDistinctThingsAround(globalTargetInfo.Cell, pawn.Map, def.radius, true))
                    {
						if (thing is Pawn target && target.Faction != pawn.Faction)
                        {
							/*target.mindState.mentalStateHandler.TryStartMentalState(MentalStateDefOf.PanicFlee);*/
							List<Apparel> apparels = target.apparel.WornApparel;
							float marketValue = 0;
							foreach (Apparel apparel in apparels)
							{
								if (apparel != null && apparel.MarketValue > 200)
								{
									marketValue += apparel.MarketValue;
								}
							}
							if (target.equipment.Primary != null)
                            {
								float randomDropEquip = Rand.Value;
								if (randomDropEquip <= 0.75f)
                                {
									ThingWithComps weapon = target.equipment.Primary;
									target.equipment.TryDropEquipment(weapon, out weapon, target.RandomAdjacentCell8Way());
									float damage = target.apparel.WornApparelCount;
									for(int i = 0; i < 1; i++)
                                    {
										target.TakeDamage(new DamageInfo(DamageDefOf.Cut, marketValue / 30, 1, -1, pawn, target.RaceProps.body.AllParts.FirstOrFallback((BodyPartRecord p) => p.def == BodyPartDefOf.Torso)));
									}
									Effecter effect = MakaiTechPsy_DefOf.MakaiPsy_TD_Blast.Spawn(target.Position, pawn.Map, 0.5f);
									effect.Cleanup();
								}
							}
							float randDropInv = Rand.Value;
							if (randDropInv <= 0.5f && (target.RaceProps.Humanlike || target.RaceProps.Animal) && (!target.RaceProps.IsMechanoid && !target.RaceProps.Insect))
							{
								target.inventory.DropAllNearPawn(target.RandomAdjacentCell8Way());
								Faction.OfPlayer.TryAffectGoodwillWith(target.Faction, -2);
							}
							if (marketValue > 5000)
                            {
								GenExplosion.DoExplosion(target.Position, pawn.Map, 2f, DamageDefOf.Bomb, null, 5);
							}
                        }
                    }
				}
				pawn.health.hediffSet.GetFirstHediffOfDef(MakaiTechPsy_DefOf.MakaiPsy_CP_TaintLevel).Severity += modExtension.costs;
				Messages.Message("Makai_GreatPassArollcheck".Translate(pawn.LabelShort, baseRoll, cumulativeBonusRoll, pawn.Named("USER")), pawn, MessageTypeDefOf.PositiveEvent);
				Messages.Message("Makai_GreatPassArollcheckTargetCorrupt".Translate(pawn.LabelShort, modExtension.costs, pawn.Named("USER")), pawn, MessageTypeDefOf.PositiveEvent);
			}
			if (pawn.health.hediffSet.HasHediff(MakaiTechPsy_DefOf.MakaiPsy_CP_TaintLevel) && roll < modExtension.successThreshold && pawn.health.hediffSet.GetFirstHediffOfDef(MakaiTechPsy_DefOf.MakaiPsy_CP_TaintLevel).Severity >= modExtension.costs)
			{
				foreach (GlobalTargetInfo globalTargetInfo in targets)
				{
					foreach (Thing thing in GenRadial.RadialDistinctThingsAround(globalTargetInfo.Cell, pawn.Map, def.radius, true))
					{
						if (thing is Pawn target && target.Faction != pawn.Faction)
						{
							/*target.mindState.mentalStateHandler.TryStartMentalState(MentalStateDefOf.Berserk);*/
							List<Apparel> apparels = target.apparel.WornApparel;
							float marketValue = 0;
							foreach (Apparel apparel in apparels)
							{
								if (apparel.MarketValue > 200)
								{
									marketValue += apparel.MarketValue;
								}
							}
							if (pawn.equipment.Primary != null)
							{
								float randomDropEquip = Rand.Value;									
								if(randomDropEquip <= 0.25f)
                                {
									ThingWithComps weapon = target.equipment.Primary;
									pawn.equipment.TryDropEquipment(weapon, out weapon, pawn.RandomAdjacentCell8Way());
									float damage = pawn.apparel.WornApparelCount;
									for (int i = 0; i < 1; i++)
									{
										BodyPartRecord bodyRec = target.RaceProps.body.AllParts.RandomElement();
										pawn.TakeDamage(new DamageInfo(DamageDefOf.Cut, marketValue / 30, 1, -1, pawn, bodyRec));
									}
									Effecter effect = MakaiTechPsy_DefOf.MakaiPsy_TD_Blast.Spawn(pawn.Position, pawn.Map, 0.5f);
									effect.Cleanup();
								}
							}
							if (marketValue > 9000)
							{
								GenExplosion.DoExplosion(pawn.Position, pawn.Map, 2f, DamageDefOf.Bomb, null,5);
							}
						}
					}
				}
				pawn.health.hediffSet.GetFirstHediffOfDef(MakaiTechPsy_DefOf.MakaiPsy_CP_TaintLevel).Severity -= modExtension.costs;
				Messages.Message("Makai_FailArollcheck".Translate(pawn.LabelShort, baseRoll, cumulativeBonusRoll, pawn.Named("USER")), pawn, MessageTypeDefOf.NegativeEvent);
				Messages.Message("Makai_FailArollcheckTargetCorrupt".Translate(pawn.LabelShort, modExtension.costs, pawn.Named("USER")), pawn, MessageTypeDefOf.NegativeEvent);

			}
			if(!pawn.health.hediffSet.HasHediff(MakaiTechPsy_DefOf.MakaiPsy_CP_TaintLevel) || pawn.health.hediffSet.GetFirstHediffOfDef(MakaiTechPsy_DefOf.MakaiPsy_CP_TaintLevel).Severity < modExtension.costs)
            {
				Messages.Message("Makai_CP_NotEnoughTaint".Translate(pawn.LabelShort, pawn.Named("USER")), pawn, MessageTypeDefOf.NegativeEvent);
            }
		}

    }
}
