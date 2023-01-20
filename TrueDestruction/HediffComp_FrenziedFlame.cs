using RimWorld;
using Verse;

namespace MakaiTechPsycast.TrueDestruction
{
    public class HediffComp_FrenziedFlame : HediffComp
    {
        public HediffCompProperties_FrenziedFlame Props => (HediffCompProperties_FrenziedFlame)props;

        public override void Notify_PawnUsedVerb(Verb verb, LocalTargetInfo target)
        {
            base.Notify_PawnUsedVerb(verb, target);
            parent.Severity += Props.severityPerHit;
        }

        public override void CompPostTick(ref float severityAdjustment)
        {
            base.CompPostTick(ref severityAdjustment);
            if(Pawn.IsHashIntervalTick(2500))
            {
                if(!Pawn.IsFighting())
                {
                    parent.Severity = 1;
                }
            }
        }
    }
}
