using System.Collections.Generic;
using System.Linq;
using RimWorld;
using MakaiTechPsycast;
using UnityEngine;
using Verse;

namespace MakaiTechPsycast
{
	public class MakaiTD_PowerBeam : OrbitalStrike
	{
		public float damageAmount = 5;

		public float armorPen = 1f;

		public const float Radius = 2f;

		private const int FiresStartedPerTick = 0;

		private static readonly IntRange FlameDamageAmountRange = new IntRange(2, 5);

		private static readonly IntRange CorpseFlameDamageAmountRange = new IntRange(1, 1);

		private static List<Thing> tmpThings = new List<Thing>();

		private int ticksSinceLastCheck = 0;

		public override void StartStrike()
		{
			base.StartStrike();
		}

		public override void Tick()
		{
			base.Tick();
			if (!base.Destroyed)
			{
				ticksSinceLastCheck++;
				if(ticksSinceLastCheck >= 10)
                {
					DoDamge();
					ticksSinceLastCheck = 0;

				}
				/*for (int i = 0; i < 1; i++)
				{
					DoDamge();
				}*/
			}
		}

		private void DoDamge()
		{
			IntVec3 c = (from x in GenRadial.RadialCellsAround(base.Position, 2f, useCenter: true)
						 where x.InBounds(base.Map)
						 select x).RandomElementByWeight((IntVec3 x) => 1f - Mathf.Min(x.DistanceTo(base.Position) / 2f, 1f) + 0.05f);
			tmpThings.Clear();
			tmpThings.AddRange(c.GetThingList(base.Map));
			for (int i = 0; i < tmpThings.Count; i++)
			{
				int num = ((tmpThings[i] is Corpse) ? CorpseFlameDamageAmountRange.RandomInRange : FlameDamageAmountRange.RandomInRange);
				Pawn pawn = tmpThings[i] as Pawn;
				/*BattleLogEntry_DamageTaken battleLogEntry_DamageTaken = null;
				if (pawn != null)
				{
					battleLogEntry_DamageTaken = new BattleLogEntry_DamageTaken(pawn, RulePackDefOf.DamageEvent_PowerBeam, instigator as Pawn);
					Find.BattleLog.Add(battleLogEntry_DamageTaken);
				}*/
				//tmpThings[i].TakeDamage(new DamageInfo(MakaiTechPsy_DefOf.TrueDestruction_LightningTowerBeam, 5, 0f, -1f, instigator, null, weaponDef)).AssociateWithLog(battleLogEntry_DamageTaken);
				tmpThings[i].TakeDamage(new DamageInfo(MakaiTechPsy_DefOf.TrueDestruction_LightningTowerBeam, damageAmount, armorPen, -1f, instigator, null, weaponDef));
			}
			tmpThings.Clear();
		}
	}
}
