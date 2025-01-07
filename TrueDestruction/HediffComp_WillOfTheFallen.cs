using RimWorld;
using System;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace MakaiTechPsycast.TrueDestruction
{
    public class HediffComp_WillOfTheFallen : HediffComp
    {
        public HediffCompProperties_WillOfTheFallen Props => (HediffCompProperties_WillOfTheFallen)props;

        public int count;

        public bool absorbThought = false;

        public int buffQuality;

        public override void CompExposeData()
        {
            base.CompExposeData();
            Scribe_Values.Look(ref count, "count",0);
            Scribe_Values.Look(ref absorbThought, "absorbThought", false);
            Scribe_Values.Look(ref buffQuality, "buffQuality", 1);
        }
        public override string CompLabelInBracketsExtra
        {
            get
            {
                if(count >= 0)
                {
                    return base.CompLabelInBracketsExtra + count;
                }
                return base.CompLabelInBracketsExtra;
            }
        }

        public override void Notify_KilledPawn(Pawn victim, DamageInfo? dinfo)
        {
            if(victim.RaceProps.Humanlike)
            {
                if(victim.needs != null)
                {
                    if (victim.needs?.mood?.thoughts != null)
                    {
                        foreach (var item in victim.needs?.mood?.thoughts?.memories?.Memories)
                        {
                            if (item == null)
                            {
                                continue;
                            }
                            if (buffQuality == 0)
                            {
                                count += (int)Math.Abs(item.MoodOffset()) / 2;
                            }
                            else if (buffQuality == 1)
                            {
                                count += (int)Math.Abs(item.MoodOffset());
                            }
                            else if (buffQuality == 2)
                            {
                                count += (int)Math.Abs(item.MoodOffset()) * 2;
                            }
                        }
                    }
                    else
                    {
                        count += Rand.Range(5, 20);
                    }
                }
                else
                {
                    count += Rand.Range(5, 20);
                }

            }
            else
            {
                count += Rand.Range(5,20);
            }
            if(Props.soundDefOnTrigger != null)
            {
                Props.soundDefOnTrigger.PlayOneShot(new TargetInfo(Pawn.Position,Pawn.Map));
            }
        }

        public override void Notify_PawnUsedVerb(Verb verb, LocalTargetInfo target)
        {
            base.Notify_PawnUsedVerb(verb, target);
            if (verb.GetType() == typeof(Verb_BeatFire)) return;
            if (verb.GetType() == typeof(Verb_CastAbility)) return;
            if (verb.GetType() == typeof(Verb_CastPsycast)) return;
            if (verb.GetType() == typeof(Verb_CastAbilityTouch)) return;
            if (verb.GetType() == typeof(Verb_CastAbilityJump)) return;
            if(target.HasThing)
            {
                //target.Thing.TakeDamage(new DamageInfo(MakaiTechPsy_DefOf.TrueDestruction_BonusDamage, count * 0.1f, 1f,instigator:Pawn,weapon:verb.EquipmentSource.def ?? null));                                            
                target.Thing.TakeDamage(new DamageInfo(MakaiTechPsy_DefOf.TrueDestruction_BonusDamage, count * 0.1f, 9999f,instigator:Pawn));
                float rand = Rand.Value;
                if (rand <= 0.9f)
                {
                    count -= Mathf.FloorToInt(count * 0.1f);
                }
                float rand2 = Rand.Value;
                if (rand2 <= 0.10f)
                {
                    count += 10;
                }
            }            
        }
    }
}
