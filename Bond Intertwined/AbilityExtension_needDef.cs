using Verse;
using RimWorld;

namespace MakaiTechPsycast.BondIntertwined
{
    public class AbilityExtension_needDef : DefModExtension
    {
        public NeedDef needDef;

		public bool applyDamageToExplosionCellsNeighbors;

		public float chanceToStartFire;

		public bool damageFalloff;

		public float explosionArmorPenetration = -1f;

		public int explosionDamageAmount = -1;

		public DamageDef explosionDamageDef;

		public float? explosionDirection;

		public float explosionRadius;

		public SoundDef explosionSound;

		public bool onCaster;

		public float postExplosionSpawnChance;

		public int postExplosionSpawnThingCount = 1;

		public ThingDef postExplosionSpawnThingDef;

		public float preExplosionSpawnChance;

		public int preExplosionSpawnThingCount = 1;

		public ThingDef preExplosionSpawnThingDef;

		public bool casterImmune;
	}
}
