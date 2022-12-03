using RimWorld;
using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace MakaiTechPsycast.DistortedReality
{
    public class HediffComp_DistortedUltimateCount : HediffComp
    {
        public int FailCount;

		public override string CompLabelInBracketsExtra
		{
			get
			{
				if (FailCount > 0)
				{
					return base.CompLabelInBracketsExtra + FailCount + " count";
				}
				return base.CompLabelInBracketsExtra;
			}
		}
		public override void CompExposeData()
		{
			Scribe_Values.Look(ref FailCount, "FailCount", 1);
		}
	}
}
