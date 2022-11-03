using RimWorld;
using RimWorld.Planet;
using Verse;
using System.Collections.Generic;
using System.Linq;
using VFECore;
using VanillaPsycastsExpanded;
using UnityEngine;

namespace MakaiTechPsycast.DistortedReality
{
    public class Ability_DistortEquipment : VFECore.Abilities.Ability
    {
        public override void Cast(params GlobalTargetInfo[] targets)
        {
            base.Cast(targets);
            AbilityExtension_Roll1D20 modExtension = def.GetModExtension<AbilityExtension_Roll1D20>();
            int threshold1 = Rand.Range(1,21);
            int threshold2 = Rand.Range(1,21);
            RollInfo rollinfo = new RollInfo();
            List<SkillDef> randomSkill = DefDatabase<SkillDef>.AllDefs.ToList();
            rollinfo = MakaiUtility.Roll1D20(pawn, randomSkill.RandomElement(), rollinfo);
            if (rollinfo.roll >= Mathf.Min(threshold1, threshold2) && rollinfo.roll < Mathf.Max(threshold1, threshold2))
            {
                if (targets[0].Thing is Pawn targetPawn && targetPawn.equipment.Primary != null)
                {
                    ThingWithComps weapon = targetPawn.equipment.Primary;
                    List<ThingDef> equipmentInGame = DefDatabase<ThingDef>.AllDefsListForReading.Where((ThingDef thingItem) => (thingItem.IsWeapon)).ToList();
                    List<ThingDef> stuffThing = DefDatabase<ThingDef>.AllDefsListForReading.Where((ThingDef stuffDef) => stuffDef.IsStuff).ToList();
                    ThingDef choosedWeapon = equipmentInGame.RandomElement();
                    ThingWithComps createWeapon = weapon;
                    if (choosedWeapon.MadeFromStuff)
                    {
                        createWeapon = ThingMaker.MakeThing(choosedWeapon, stuffThing.RandomElement()) as ThingWithComps;
                    }
                    else
                    {
                        createWeapon = ThingMaker.MakeThing(choosedWeapon) as ThingWithComps;
                    }
                    createWeapon.SetStuffDirect(stuffThing.RandomElement());
                    if(createWeapon.TryGetComp<CompQuality>() != null)
                    {
                        List<QualityCategory> qCata = QualityUtility.AllQualityCategories;
                        createWeapon.TryGetComp<CompQuality>().SetQuality(qCata.RandomElement(),ArtGenerationContext.Colony);
                    }
                    targetPawn.equipment.Remove(weapon);
                    targetPawn.equipment.AddEquipment(createWeapon);
                    Messages.Message("Makai_PassArollcheck".Translate(pawn.LabelShort, rollinfo.baseRoll, rollinfo.cumulativeBonusRoll, pawn.Named("USER")), pawn, MessageTypeDefOf.PositiveEvent);
                    Messages.Message("Makai_PassArollcheckDistortEquipment".Translate(pawn.LabelShort, weapon.LabelShort, targetPawn.LabelShort, createWeapon.LabelShort, pawn.Named("USER")), pawn, MessageTypeDefOf.PositiveEvent);
                }
                else
                {
                    Messages.Message("Target doesn't have weapon equipped", MessageTypeDefOf.NeutralEvent);
                }
            }
            if (rollinfo.roll >= Mathf.Max(threshold1, threshold2))
            {
                if (targets[0].Thing is Pawn targetPawn && targetPawn.equipment.Primary != null)
                {
                    ThingWithComps weapon = targetPawn.equipment.Primary;
                    List<ThingDef> equipmentInGame = DefDatabase<ThingDef>.AllDefsListForReading.Where((ThingDef thingItem) => (thingItem.IsWeapon)).ToList();
                    List<ThingDef> stuffThing = DefDatabase<ThingDef>.AllDefsListForReading.Where((ThingDef stuffDef) => stuffDef.IsStuff).ToList();
                    ThingDef choosedWeapon = equipmentInGame.RandomElement();
                    ThingWithComps createWeapon = weapon;
                    if (choosedWeapon.MadeFromStuff)
                    {
                        createWeapon = ThingMaker.MakeThing(choosedWeapon, stuffThing.RandomElement()) as ThingWithComps;
                    }
                    else
                    {
                        createWeapon = ThingMaker.MakeThing(choosedWeapon) as ThingWithComps;
                    }
                    createWeapon.SetStuffDirect(stuffThing.RandomElement());
                    if (createWeapon.TryGetComp<CompQuality>() != null)
                    {
                        List<QualityCategory> qCata = QualityUtility.AllQualityCategories;
                        createWeapon.TryGetComp<CompQuality>().SetQuality(qCata.RandomElement(), ArtGenerationContext.Colony);
                    }
                    targetPawn.equipment.TryDropEquipment(weapon,out weapon,pawn.RandomAdjacentCell8Way());
                    targetPawn.equipment.AddEquipment(createWeapon);
                    Messages.Message("Makai_GreatPassArollcheck".Translate(pawn.LabelShort, rollinfo.baseRoll, rollinfo.cumulativeBonusRoll, pawn.Named("USER")), pawn, MessageTypeDefOf.PositiveEvent);
                    Messages.Message("Makai_GreatPassArollcheckDistortEquipment".Translate(pawn.LabelShort, weapon.LabelShort, targetPawn.LabelShort, createWeapon.LabelShort, pawn.Named("USER")), pawn, MessageTypeDefOf.PositiveEvent);
                }
                else
                {
                    Messages.Message("Target doesn't have weapon equipped", MessageTypeDefOf.NeutralEvent);
                }
            }
            if (rollinfo.roll < Mathf.Min(threshold1, threshold2))
            {
                if (targets[0].Thing is Pawn targetPawn && targetPawn.equipment.Primary != null)
                {
                    ThingWithComps weapon = targetPawn.equipment.Primary;
                    List<ThingDef> equipmentInGame = DefDatabase<ThingDef>.AllDefsListForReading.Where((ThingDef thingItem) => (thingItem.IsWeapon)).ToList();
                    List<ThingDef> stuffThing = DefDatabase<ThingDef>.AllDefsListForReading.Where((ThingDef stuffDef) => stuffDef.IsStuff).ToList();
                    ThingDef choosedWeapon = equipmentInGame.RandomElement();
                    ThingWithComps createWeapon = weapon;
                    if (choosedWeapon.MadeFromStuff)
                    {
                        createWeapon = ThingMaker.MakeThing(choosedWeapon, stuffThing.RandomElement()) as ThingWithComps;
                    }
                    else
                    {
                        createWeapon = ThingMaker.MakeThing(choosedWeapon) as ThingWithComps;
                    }
                    createWeapon.SetStuffDirect(stuffThing.RandomElement());
                    if (createWeapon.TryGetComp<CompQuality>() != null)
                    {
                        List<QualityCategory> qCata = QualityUtility.AllQualityCategories;
                        createWeapon.TryGetComp<CompQuality>().SetQuality(qCata.RandomElement(), ArtGenerationContext.Colony);
                    }
                    targetPawn.equipment.Remove(weapon);
                    targetPawn.equipment.AddEquipment(createWeapon);
                    Messages.Message("Makai_FailArollcheck".Translate(pawn.LabelShort, rollinfo.baseRoll, rollinfo.cumulativeBonusRoll, pawn.Named("USER")), pawn, MessageTypeDefOf.PositiveEvent);
                    Messages.Message("Makai_FailArollcheckDistortEquipment".Translate(pawn.LabelShort, weapon.LabelShort, targetPawn.LabelShort, createWeapon.LabelShort, pawn.Named("USER")), pawn, MessageTypeDefOf.PositiveEvent);
                }
                else
                {
                    Messages.Message("Target doesn't have weapon equipped", MessageTypeDefOf.NeutralEvent);
                }
            }
        }
    }
}
