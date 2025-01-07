using RimWorld;
using System.Collections.Generic;
using Verse;

namespace MakaiTechPsycast.PassionedFlame
{
    public class HediffComp_AkashaRecord : HediffComp
    {
        public HediffCompProperties_AkashaRecord Props => (HediffCompProperties_AkashaRecord)props;
        public override void Notify_PawnPostApplyDamage(DamageInfo dinfo, float totalDamageDealt)
        {
            float severityToDeduct = Props.curve.Evaluate(dinfo.Amount);
            parent.Severity -= severityToDeduct;
            MoteMaker.ThrowText(Pawn.Position.ToVector3Shifted(), Pawn.Map, "Blocked: " + dinfo.Def.LabelCap, color: UnityEngine.Color.white);
        }
    }
}
