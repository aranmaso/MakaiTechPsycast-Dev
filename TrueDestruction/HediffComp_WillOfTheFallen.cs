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
            base.Notify_KilledPawn(victim, dinfo);
            foreach(Thought item in victim.needs?.mood?.thoughts?.memories?.Memories)
            {
                if(buffQuality == 0)
                {
                    count += (int)Math.Abs(item.MoodOffset())/2;
                }
                else if(buffQuality == 1)
                {
                    count += (int)Math.Abs(item.MoodOffset());
                }
                else if(buffQuality == 2)
                {
                    count += (int)Math.Abs(item.MoodOffset()) * 2;
                }
            }
            if(Props.soundDefOnTrigger != null)
            {
                Props.soundDefOnTrigger.PlayOneShot(new TargetInfo(Pawn.Position,Pawn.Map));
            }
        }
        public override void Notify_PawnUsedVerb(Verb verb, LocalTargetInfo target)
        {
            base.Notify_PawnUsedVerb(verb, target);
            if(verb.CurrentTarget.HasThing)
            {
                verb.CurrentTarget.Thing.TakeDamage(new DamageInfo(verb.GetDamageDef(), count * 0.05f, 1f,instigator:Pawn,weapon:verb.EquipmentSource.def ?? null));
                count -= Mathf.FloorToInt(count * 0.05f);
            }            
        }
    }
}
