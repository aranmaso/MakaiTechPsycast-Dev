using System.Collections.Generic;
using System.Linq;
using RimWorld;
using RimWorld.Planet;
using Verse;
using VFECore.Abilities;

namespace MakaiTechPsycast.BondIntertwined
{
	public class AbilityExtension_WordOfBond : AbilityExtension_AbilityMod
	{
		public ThoughtDef thoughtDef;
		public override void Cast(GlobalTargetInfo[] targets, VFECore.Abilities.Ability ability)
		{
			base.Cast(targets, ability);
			if (targets[0].Thing is Pawn pawn && targets[1].Thing is Pawn pawn2)
			{
				pawn2.needs.mood.thoughts.memories.TryGainMemory(thoughtDef, pawn);
				pawn.needs.mood.thoughts.memories.TryGainMemory(thoughtDef, pawn2);
				/* pawn.equipment.DropAllEquipment(pawn.RandomAdjacentCell8Way());
				pawn.apparel.DropAll(pawn.RandomAdjacentCell8Way()); */
			}
		}
	}

}
