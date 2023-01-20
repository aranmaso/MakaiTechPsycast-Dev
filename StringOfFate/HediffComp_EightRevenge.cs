using Verse;
using RimWorld;
using System.Collections.Generic;

namespace MakaiTechPsycast.StringOfFate
{
    public class HediffComp_EightRevenge : HediffComp
    {
        public HediffCompProperties_EightRevenge Props => (HediffCompProperties_EightRevenge)props;

        public int multiplier = 0;

        public override string CompLabelInBracketsExtra
        {
            get
            {
                if (multiplier > 0)
                {
                    return base.CompLabelInBracketsExtra + "x" +multiplier;
                }
                return base.CompLabelInBracketsExtra;
            }
        }
        public override void CompExposeData()
        {
            base.CompExposeData();
            Scribe_Values.Look(ref multiplier, "multiplier", 0);
        }
        public override void Notify_PawnPostApplyDamage(DamageInfo dinfo, float totalDamageDealt)
        {
            base.Notify_PawnPostApplyDamage(dinfo, totalDamageDealt);
            /*List<Thing> ignoreThing = new List<Thing>();
            ignoreThing.Add(Pawn);*/
            //instigator.Map.weatherManager.eventHandler.AddEvent(new WeatherEvent_LightningStrikeGreen(instigator.Map,instigator.Position));
            //GenExplosion.DoExplosion(Pawn.Position, Pawn.Map, 2.9f, DamageDefOf.Bomb, Pawn, 1, 1, ignoredThings: ignoreThing);
            if(dinfo.Instigator is Pawn instigator && instigator.health.hediffSet.HasHediff(parent.def))
            {
                MoteMaker.ThrowText(Pawn.Position.ToVector3(),Pawn.Map,"Can't reflect each other");
                return;
            }
            dinfo.Instigator.TakeDamage(new DamageInfo(dinfo.Def,dinfo.Amount * multiplier, 9999,hitPart: dinfo.HitPart));            
            Pawn.health.RemoveHediff(parent);
        }
    }
}
