using Verse;
using RimWorld;
using VanillaPsycastsExpanded;
using System.Collections.Generic;
using UnityEngine;

namespace MakaiTechPsycast
{
    public class CompProperties_ProximityBurst : CompProperties
    {
        public int shrapnelCount;

        public float range;

        public float burstRange = 3;

        public List<ThingDef> shrapnelBulletDef;

        public int checkInterval = 60;

        public bool proximityOnTargetDistance;
        public int distanceFromTargetToProx;

        public bool proximityOnNearestFoe;

        public bool proximityOnIntendedTarget;

        public bool proximityOnTimer;
        public int proximityTimer;

        public CompProperties_ProximityBurst()
        {
            compClass = typeof(CompProximityBurst);
        }
    }
}
