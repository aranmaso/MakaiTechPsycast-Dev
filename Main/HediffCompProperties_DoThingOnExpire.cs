using Verse;
using RimWorld;
using UnityEngine;
using System.Collections.Generic;

namespace MakaiTechPsycast
{
    public class HediffCompProperties_DoThingOnExpire : HediffCompProperties
    {
        public EffecterDef effecterDef;

        public List<EffecterDef> effecterDefs;

        public bool doMultipleEffecter;

        public DamageDef damageDef;

        public float damageAmount;

        public float armorPenetration = 1f;

        public BodyPartDef bodyPartDef;

        public bool doExplosion;

        public DamageDef explosionDamageDef;

        public float explosionRadius;

        public ThingDef projectileDef;

        public bool spawnProjectileOffset;

        public Vector3 drawOffset = Vector3.zero;

        public FleckDef fleckDef;

        public Vector3 fleckPosOffset;

        public HediffCompProperties_DoThingOnExpire()
        {
            compClass = typeof(HediffComp_DoThingOnExpire);
        }
    }
}
