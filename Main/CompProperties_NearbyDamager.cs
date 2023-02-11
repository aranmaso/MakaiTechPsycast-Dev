using Verse;
using RimWorld;
using System.Collections.Generic;

namespace MakaiTechPsycast
{
    public class CompProperties_NearbyDamager : CompProperties
    {
        public float contactRadius;

        public float spewRadius;

        public int contactInterval;

        public int spewInterval;

        public List<DamageDef> damageDefs;

        public float damageAmount;

        public List<ThingDef> projectileDefs;

        public bool spawnFromAbove;

        public bool contactAffectFriendly;

        public StatDef statDef;

        public CompProperties_NearbyDamager()
        {
            compClass = typeof(CompNearbyDamager);
        }
    }
}
