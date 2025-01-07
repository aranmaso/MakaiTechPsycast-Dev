using Verse;
using RimWorld;
using System.Collections.Generic;
using UnityEngine;

namespace MakaiTechPsycast
{
    public class AbilityExtension_Roll1D20 : DefModExtension
    {
		public HediffDef hediffDefWhenFail;

		public HediffDef hediffDefWhenSuccess;

		public HediffDef hediffDefWhenGreatSuccess;

		public List<HediffDef> hediffsToApply;

		public int successThreshold;

		public int greatSuccessThreshold;

		public int failThreshold;

		public int skillRequirementSuccessThreshold;

		public int skillRequirementGreatSuccessThreshold;

		public int skillRequirementFailThreshold;

		public ThoughtDef memoryDefWhenFail;

		public ThoughtDef memoryDefWhenSuccess;

		public ThoughtDef memoryDefWhenGreatSuccess;

		public ThingDef thingToSpawnWhenFail;

		public ThingDef thingToSpawnWhenSuccess;

		public ThingDef thingToSpawnWhenGreatSuccess;

		public ThingDef projectileWhenSuccess;

		public ThingDef projectileWhenGreatSuccess;

		public ThingDef projectileWhenFail;

		public bool spawnProjectileAtRandom = false;

		public int projectileBurstCount = 1;

		public int tickBurstInterval = 60;

		public SoundDef soundDef;

		public StatDef multiplier;

		public float hours;

		public float costs;

		public int ticks;

		public DamageDef damageDef;

		public int damageAmount = 0;

		public SkillDef skillBonus;

		public SkillDef skillBonus2;

		public FleckDef fleckDef;

		public Vector3 fleckPosOffset;

		public int repeatEffect;

		public bool multiTarget;

		public bool targetOnlyEnemies = false;

		public bool targetOnlyDowned = false;

		public bool targetOnlyNonDowned = false;

		public bool targetOnlyPrisonerOrSlave = false;

		public bool testingMode;

		public SimpleCurve curve;
	}
}
