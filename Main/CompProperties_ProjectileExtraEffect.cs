using System.Collections.Generic;
using UnityEngine;
using VanillaPsycastsExpanded;
using Verse;

namespace MakaiTechPsycast
{
    public class CompProperties_ProjectileExtraEffect : CompProperties
    {
        public int interval;

        public int hurtInterval = 10;

        public int catchInterval = 10;

        public List<ThingDef> projectileDefs;

        public HediffDef hediffBonus;

        public float damageAmount;

        public float armorPen;

        public float radius;

        public float hurtRadius;

        public float pullRadius;

        public float catchRadius;

        public float pullSpeed;

        public bool shootMoreBullet;

        public bool hurtNearbyPawn;

        public bool pullPawn;

        public bool makeFlyer;

        public bool makeGoToJob;

        public bool catchAndLaunchBackBullet;

        public bool shootAtRandom;

        public bool hurtEnemyOnly = false;
        public CompProperties_ProjectileExtraEffect()
        {
            compClass = typeof(CompProjectileExtraEffect);
        }
    }
}
