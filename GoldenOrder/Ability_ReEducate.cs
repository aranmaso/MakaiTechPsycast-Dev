using RimWorld.Planet;
using System.Collections.Generic;
using VanillaPsycastsExpanded;
using VFECore.Abilities;
using RimWorld;
using Verse;
using System.Linq;
using Verse.AI;

namespace MakaiTechPsycast.GoldenOrder
{
    public class Ability_ReEducate : VFECore.Abilities.Ability
    {
		public Trait trait;
		public override void Cast(params GlobalTargetInfo[] targets)
        {
			base.Cast(targets);
			AbilityExtension_Roll1D20 modExtension = def.GetModExtension<AbilityExtension_Roll1D20>();
			RollInfo rollinfo = new RollInfo();
			rollinfo = MakaiUtility.Roll1D20(pawn, modExtension.skillBonus, rollinfo);
			if (rollinfo.roll >= modExtension.successThreshold && rollinfo.roll < modExtension.greatSuccessThreshold)
            {
				foreach (GlobalTargetInfo globalTargetInfo in targets)
				if (trait == null)
				{
					pawn.jobs.EndCurrentJob(JobCondition.InterruptForced);
					Messages.Message("No trait choosed",MessageTypeDefOf.NeutralEvent);
				}
                if (targets[0].Thing is Pawn pawn2)
                {
					Find.WindowStack.Add(new Dialog_ChooseTraitToRemove(pawn2));
				}
			}

		}
    }
}
