using Verse;
using RimWorld;
using UnityEngine;

namespace MakaiTechPsycast.GoldenOrder
{
    public class HediffComp_PathOfNaraka : HediffComp
    {
        public HediffCompProperties_PathOfNaraka Props => (HediffCompProperties_PathOfNaraka)props;

        public float currentStack = 0f;

		public int quality = 1;

        public int maxStack => Props.maxStack;

        //public float costPerTrigger => Props.costPerTrigger;

		public override string CompLabelInBracketsExtra
		{
			get
			{
				if (currentStack >= 0f)
				{
					return base.CompLabelInBracketsExtra + currentStack.ToString("P2");
				}
				return base.CompLabelInBracketsExtra;
			}
		}
		public override void CompExposeData()
		{
			Scribe_Values.Look(ref currentStack, "currentStack", 0f);
			Scribe_Values.Look(ref quality, "quality", 1);
			base.CompExposeData();
		}

        public override void CompPostTick(ref float severityAdjustment)
        {
            base.CompPostTick(ref severityAdjustment);
			if (Pawn.psychicEntropy.IsCurrentlyMeditating && currentStack <= Mathf.Max((float)maxStack*quality,1))
			{
				IncreaseStack();
			}
			/*if (Pawn.InMentalState && currentStack >= costPerTrigger)
			{
				Pawn.MentalState.RecoverFromState();
				currentStack -= costPerTrigger;
			}*/
		}
		public void IncreaseStack()
		{
			currentStack += base.Pawn.GetStatValue(StatDefOf.MeditationFocusGain) / 60000f;
		}

	}
}
