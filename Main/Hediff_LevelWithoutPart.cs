using UnityEngine;
using Verse;

namespace MakaiTechPsycast
{
	public class Hediff_LevelWithoutPart : HediffWithComps
	{
		public int level = 1;
		/*public override string Label
		{
			get
			{
				string labelInBrackets = LabelInBrackets;
				return LabelBase + (labelInBrackets.NullOrEmpty() ? "" : (" (" + labelInBrackets + ")"));
			}
		}*/

		public override bool ShouldRemove => level == 0;

		public override void PostAdd(DamageInfo? dinfo)
		{
		}

		public override void Tick()
		{
			base.Tick();
			Severity = level;
		}
		public virtual void ChangeLevel(int levelOffset)
		{
			level = (int)Mathf.Clamp(level + levelOffset, def.minSeverity, def.maxSeverity);
		}

		public virtual void SetLevelTo(int targetLevel)
		{
			if (targetLevel != level)
			{
				ChangeLevel(targetLevel - level);
			}
		}

		public override void ExposeData()
		{
			base.ExposeData();
            Scribe_Values.Look(ref level, "level", 0);
		}
	}
}
