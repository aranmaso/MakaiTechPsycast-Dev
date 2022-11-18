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
    public class Ability_RandomizedItemOnGround : VFECore.Abilities.Ability
    {
        public override void Cast(params GlobalTargetInfo[] targets)
        {
            base.Cast(targets);
            AbilityExtension_Roll1D20 modExtension = def.GetModExtension<AbilityExtension_Roll1D20>();
            int threshold1 = Rand.Range(1, 21);
            int threshold2 = Rand.Range(1, 21);
            RollInfo rollinfo = new RollInfo();
            List<SkillDef> randomSkill = DefDatabase<SkillDef>.AllDefs.ToList();
            rollinfo = MakaiUtility.Roll1D20(pawn, randomSkill.RandomElement(), rollinfo);
            if (rollinfo.roll >= Mathf.Min(threshold1, threshold2) && rollinfo.roll < Mathf.Max(threshold1, threshold2))
            {
                if (!(targets[0].Thing is Pawn) && targets[0].Thing.def.alwaysHaulable)
                {
                    Find.WindowStack.Add(new Dialog_ChooseRandomItem(targets[0].Thing, targets[0].Cell,pawn.Map, 5, 1));
                    Messages.Message("Makai_PassArollcheck".Translate(pawn.LabelShort, rollinfo.baseRoll, rollinfo.cumulativeBonusRoll, pawn.Named("USER")), pawn, MessageTypeDefOf.PositiveEvent);
                    Messages.Message("Makai_PassArollcheckRandomItem".Translate(pawn.LabelShort, pawn.Named("USER")), pawn, MessageTypeDefOf.PositiveEvent);
                }
                else if (!targets[0].Thing.def.alwaysHaulable)
                {
                    Messages.Message("target must always be haulable", MessageTypeDefOf.NegativeEvent);
                }
            }
            if (rollinfo.roll >= Mathf.Max(threshold1, threshold2))
            {
                if (!(targets[0].Thing is Pawn) && targets[0].Thing.def.alwaysHaulable)
                {
                    Find.WindowStack.Add(new Dialog_ChooseRandomItem(targets[0].Thing, targets[0].Cell, pawn.Map, 8, 2));
                    Messages.Message("Makai_GreatPassArollcheck".Translate(pawn.LabelShort, rollinfo.baseRoll, rollinfo.cumulativeBonusRoll, pawn.Named("USER")), pawn, MessageTypeDefOf.PositiveEvent);
                    Messages.Message("Makai_GreatPassArollcheckRandomItem".Translate(pawn.LabelShort, pawn.Named("USER")), pawn, MessageTypeDefOf.PositiveEvent);
                }
                else if (!targets[0].Thing.def.alwaysHaulable)
                {
                    Messages.Message("target must always be haulable", MessageTypeDefOf.NegativeEvent);
                }
            }
            if (rollinfo.roll < Mathf.Min(threshold1, threshold2))
            {
                if (!(targets[0].Thing is Pawn) && targets[0].Thing.def.alwaysHaulable)
                {
                    //Find.WindowStack.Add(new Dialog_ChooseRandomItem(targets[0].Thing, targets[0].Cell, pawn.Map, 2, 3));
                    targets[0].Thing.Destroy();
                    Thing thing = ThingMaker.MakeThing(DefDatabase<ThingDef>.AllDefs.Where(x => !x.MadeFromStuff && x.category == ThingCategory.Item && !x.IsCorpse && !x.IsEgg && !x.defName.Contains("VPE_Psyring") && !x.defName.Contains("Psytrainer")).RandomElement());
                    GenSpawn.Spawn(thing, targets[0].Cell, pawn.Map);
                    Messages.Message("Makai_FailArollcheck".Translate(pawn.LabelShort, rollinfo.baseRoll, rollinfo.cumulativeBonusRoll, pawn.Named("USER")), pawn, MessageTypeDefOf.NegativeEvent);
                    Messages.Message("Makai_FailArollcheckRandomItem".Translate(pawn.LabelShort, pawn.Named("USER")), pawn, MessageTypeDefOf.NegativeEvent);
                }   
                else if (!targets[0].Thing.def.alwaysHaulable)
                {
                    Messages.Message("target must always be haulable",MessageTypeDefOf.NegativeEvent);                   
                }
            }
        }
        public override bool CanHitTarget(LocalTargetInfo target)
        {
            return targetParams.CanTarget(target.Thing, this) && GenSight.LineOfSight(pawn.Position, target.Cell, pawn.Map, skipFirstCell: true);
        }
        public override bool ValidateTarget(LocalTargetInfo target, bool showMessages = true)
        {
            if (!base.ValidateTarget(target, showMessages))
            {
                return false;
            }
            if (target.Thing.MarketValue < 1f)
            {
                if (showMessages)
                {
                    Messages.Message("VPE.TooCheap".Translate(), MessageTypeDefOf.RejectInput, historical: false);
                }
                return false;
            }
            return true;
        }
    }
}
