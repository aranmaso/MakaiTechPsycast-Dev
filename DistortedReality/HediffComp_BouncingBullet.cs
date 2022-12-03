using RimWorld;
using Verse;

namespace MakaiTechPsycast.DistortedReality
{
    public class HediffComp_BouncingBullet : HediffComp
    {
        public int bouncingCountLeft = 0;

        public HediffCompProperties_BouncingBullet Props => (HediffCompProperties_BouncingBullet)props;

		public override string CompLabelInBracketsExtra
		{
			get
			{
				if (bouncingCountLeft > 0)
				{
					return base.CompLabelInBracketsExtra + bouncingCountLeft + " left";
				}
				return base.CompLabelInBracketsExtra;
			}
		}
		public override void CompExposeData()
		{
			Scribe_Values.Look(ref bouncingCountLeft, "ShieldCount", 1);
		}

		public override void CompPostMake()
		{
			base.CompPostMake();
			if(Props.bounceCount > 0)
            {
				bouncingCountLeft = Props.bounceCount;
			}
		}
	}
}
