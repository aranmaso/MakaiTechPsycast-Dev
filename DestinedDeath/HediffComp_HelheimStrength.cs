using Verse;
using RimWorld;

namespace MakaiTechPsycast.DestinedDeath
{
    public class HediffComp_HelheimStrength : HediffComp
    {
		public int ShieldCount;

		public bool stopOnlyEnemy;
        public HediffCompProperties_HelheimStrength Props => (HediffCompProperties_HelheimStrength)props;

		public override string CompLabelInBracketsExtra
		{
			get
			{
				if (ShieldCount > 0)
				{
					return base.CompLabelInBracketsExtra + ShieldCount + " stacks";
				}
				return base.CompLabelInBracketsExtra;
			}
		}
		public override void CompExposeData()
		{
			Scribe_Values.Look(ref ShieldCount, "ShieldCount", 1);
		}
        public override void CompPostMake()
        {
            base.CompPostMake();
			ShieldCount = Props.ShieldStack;
			stopOnlyEnemy = Props.countOnlyEnemyAttack;

		}
	}
}
