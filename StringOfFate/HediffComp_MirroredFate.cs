using System.Collections.Generic;
using System.Linq;
using RimWorld;
using UnityEngine;
using Verse;

namespace MakaiTechPsycast.StringOfFate
{
    public class HediffComp_MirroredFate : HediffComp
    {
        public HediffCompProperties_MirroredFate Props => (HediffCompProperties_MirroredFate) props;

        public MirroredFateInfo mirrorInfo = new MirroredFateInfo();

		public int reflectCount = 0;

		public string info;

		public override string CompLabelInBracketsExtra
		{
			get
			{
				if (reflectCount > 0)
				{
					return base.CompLabelInBracketsExtra + reflectCount + " left" + "," + info;
				}				
				return base.CompLabelInBracketsExtra;
			}
		}
		public override void CompExposeData()
		{
			Scribe_Values.Look(ref reflectCount, "reflectCount", 1);
		}
    }
}
