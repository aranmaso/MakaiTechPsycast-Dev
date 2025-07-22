using System;
using Verse;
using UnityEngine;
using RimWorld;
using VanillaPsycastsExpanded;

namespace MakaiTechPsycast
{
    public class MirroredFateInfo : IExposable
    {
        public int reflectCountLeft;

        public float reflectPercent;

        public bool reflectOnlyEnemies;

        public bool reflectOnlyFriendly;

        public bool reflectMelee;

        public bool reflectRanged;

        public bool userTakeDamage;

        public void ExposeData()
        {
            Scribe_Values.Look(ref reflectCountLeft, "reflectCountLeft");
            Scribe_Values.Look(ref reflectPercent, "reflectPercent");
            Scribe_Values.Look(ref reflectOnlyEnemies, "reflectOnlyEnemies");
            Scribe_Values.Look(ref reflectOnlyFriendly, "reflectOnlyFriendly");
            Scribe_Values.Look(ref reflectMelee, "reflectMelee");
            Scribe_Values.Look(ref reflectRanged, "reflectRanged");
            Scribe_Values.Look(ref userTakeDamage, "userTakeDamage");
        }
    }
}
