using RimWorld;
using RimWorld.Planet;
using Verse;
using System.Linq;
using System.Collections.Generic;
using VFECore;
using VanillaPsycastsExpanded;
using UnityEngine;

namespace MakaiTechPsycast.StringOfFate
{
    public class Ability_TempFate : VFECore.Abilities.Ability
    {
        public override void Cast(params GlobalTargetInfo[] targets)
        {
            base.Cast(targets);
            AbilityExtension_Roll1D20 modExtension = def.GetModExtension<AbilityExtension_Roll1D20>();
            RollInfo rollinfo = new RollInfo();
            rollinfo = MakaiUtility.Roll1D20(pawn, modExtension.skillBonus, rollinfo);
            float rand = Rand.Value;
            if (rollinfo.roll >= modExtension.successThreshold && rollinfo.roll < modExtension.greatSuccessThreshold)
            {
                if (targets[0].Thing is Pawn targetPawn)
                {
                    if (targetPawn.health.hediffSet.hediffs.Where(x => x is Hediff_Injury || x is Hediff_MissingPart).FirstOrFallback() != null)
                    {
                        if (rand <= 0.5f)
                        {
                            List<Hediff> hediffs = targetPawn.health.hediffSet.hediffs.Where(MakaiUtility.FindBadHediff).ToList();
                            int num = 0;
                            for (int i = hediffs.Count - 1; i >= hediffs.Count / 2; i--)
                            {
                                if ((hediffs[i] is Hediff_Injury || hediffs[i] is Hediff_MissingPart) && hediffs[i].TendableNow())
                                {
                                    hediffs[i].Tended(Rand.Range(0.4f, 0.8f), 1f, 1);
                                    num++;
                                }
                            }
                            if (num > 0)
                            {
                                MoteMaker.ThrowText(pawn.DrawPos, pawn.Map, "NumWoundsTended".Translate(num), 3.65f);
                            }
                            Messages.Message("Makai_PassArollcheck".Translate(pawn.LabelShort, rollinfo.baseRoll, rollinfo.cumulativeBonusRoll, pawn.Named("USER")), pawn, MessageTypeDefOf.PositiveEvent);
                            Messages.Message("Makai_PassArollcheckTempFate".Translate(pawn.LabelShort, targetPawn.LabelShort, pawn.Named("USER")), pawn, MessageTypeDefOf.PositiveEvent);
                        }
                        else
                        {
                            BodyPartRecord bR = targetPawn.RaceProps.body.AllParts.Where(x => x.def == BodyPartDefOf.Lung).FirstOrDefault();
                            targetPawn.health.AddHediff(HediffDefOf.MissingBodyPart, targetPawn.RaceProps.body.AllParts.Where(x => x.def != BodyPartDefOf.Torso).RandomElement());
                            Messages.Message(pawn.LabelShort + " has failed fate", MessageTypeDefOf.NegativeEvent);
                        }
                    }
                }
            }
            if (rollinfo.roll >= modExtension.greatSuccessThreshold)
            {
                if (targets[0].Thing is Pawn targetPawn)
                {
                    if (targetPawn.health.hediffSet.hediffs.Where(x => x is Hediff_Injury || x is Hediff_MissingPart).FirstOrFallback() != null)
                    {
                        if (rand <= 0.5f)
                        {
                            List<Hediff> hediffs = targetPawn.health.hediffSet.hediffs.Where(MakaiUtility.FindBadHediff).ToList();
                            for (int i = hediffs.Count - 1; i >= 0; i--)
                            {
                                if (MakaiUtility.FindBadHediff(hediffs[i]))
                                {
                                    targetPawn.health.RemoveHediff(hediffs[i]);
                                }
                            }
                            Messages.Message("Makai_GreatPassArollcheck".Translate(pawn.LabelShort, rollinfo.baseRoll, rollinfo.cumulativeBonusRoll, pawn.Named("USER")), pawn, MessageTypeDefOf.PositiveEvent);
                            Messages.Message("Makai_GreatPassArollcheckTempFate".Translate(pawn.LabelShort, pawn.Named("USER")), pawn, MessageTypeDefOf.PositiveEvent);
                        }
                        else
                        {
                            BodyPartRecord bR = targetPawn.RaceProps.body.AllParts.Where(x => x.def == BodyPartDefOf.Lung).FirstOrDefault();
                            targetPawn.health.AddHediff(HediffDefOf.MissingBodyPart, targetPawn.RaceProps.body.AllParts.Where(x => x.def != BodyPartDefOf.Torso).RandomElement());
                            Messages.Message(pawn.LabelShort + " has failed fate", MessageTypeDefOf.NegativeEvent);
                        }
                    }                    
                }
            }
            if (rollinfo.roll < modExtension.successThreshold)
            {
                if (targets[0].Thing is Pawn targetPawn)
                {
                    if (targetPawn.health.hediffSet.hediffs.Where(x => x is Hediff_Injury || x is Hediff_MissingPart).FirstOrFallback() != null)
                    {
                        if (rand <= 0.5f)
                        {
                            int num = 0;
                            List<Hediff> hediffs = pawn.health.hediffSet.hediffs;
                            for (int num2 = hediffs.Count - 1; num2 >= hediffs.Count / 2; num2--)
                            {
                                if ((hediffs[num2] is Hediff_Injury || hediffs[num2] is Hediff_MissingPart) && hediffs[num2].TendableNow())
                                {
                                    hediffs[num2].Tended(Rand.Range(0.4f, 1f), 1f, 1);
                                    num++;
                                }
                            }
                            if (num > 0)
                            {
                                MoteMaker.ThrowText(pawn.DrawPos, pawn.Map, "NumWoundsTended".Translate(num), 3.65f);
                            }
                            Messages.Message("Makai_FailArollcheck".Translate(pawn.LabelShort, rollinfo.baseRoll, rollinfo.cumulativeBonusRoll, pawn.Named("USER")), pawn, MessageTypeDefOf.NegativeEvent);
                            Messages.Message("Makai_FailArollcheckTempFate".Translate(pawn.LabelShort, pawn.Named("USER")), pawn, MessageTypeDefOf.NegativeEvent);
                        }
                        else
                        {
                            BodyPartRecord bR = targetPawn.RaceProps.body.AllParts.Where(x => x.def == BodyPartDefOf.Lung).FirstOrDefault();
                            targetPawn.health.AddHediff(HediffDefOf.MissingBodyPart, targetPawn.RaceProps.body.AllParts.Where(x => x.def != BodyPartDefOf.Torso).RandomElement());
                            Messages.Message(pawn.LabelShort + " has failed fate", MessageTypeDefOf.NegativeEvent);
                        }
                    }                                      
                }
            }
        }
    }
}
