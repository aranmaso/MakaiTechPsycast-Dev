using Verse;
using RimWorld;
using System.Collections.Generic;
using System.Linq;

namespace MakaiTechPsycast.DistortedReality
{
    public class HediffComp_ChangingHediff : HediffComp
    {
        public HediffCompProperties_ChangingHediff Props => (HediffCompProperties_ChangingHediff)props;

        public override void CompPostPostAdd(DamageInfo? dinfo)
        {
            base.CompPostPostAdd(dinfo);
            Hediff hediff = HediffMaker.MakeHediff(Props.hediffList.RandomElement(), parent.pawn);
            hediff.TryGetComp<HediffComp_Disappears>().ticksToDisappear = 2500;
            parent.pawn.health.AddHediff(hediff, parent.pawn.RaceProps.body.AllParts.RandomElement());

        }
        public override void CompPostTick(ref float severityAdjustment)
        {
            base.CompPostTick(ref severityAdjustment);
            if (parent.pawn.IsHashIntervalTick(Props.interval))
            {
                ChangeHediffRandomly(parent.pawn);
            }
        }

        public void ChangeHediffRandomly(Pawn pawn)
        {
            HediffDef x = null;
            foreach(HediffDef item in Props.hediffList)
            {
                if(pawn.health.hediffSet.HasHediff(item))
                {
                    x = item;
                    pawn.health.RemoveHediff(pawn.health.hediffSet.GetFirstHediffOfDef(item));
                }
            }
            /*Hediff hediff = HediffMaker.MakeHediff((from h in Props.hediffList
                                                   where h != x
                                                   select h).RandomElement(),pawn);*/
            Hediff hediff = HediffMaker.MakeHediff(Props.hediffList.Where(h => h != x).RandomElement(),pawn);
            hediff.TryGetComp<HediffComp_Disappears>().ticksToDisappear = 2500;
            pawn.health.AddHediff(hediff, pawn.RaceProps.body.AllParts.RandomElement());
        }
    }
}
