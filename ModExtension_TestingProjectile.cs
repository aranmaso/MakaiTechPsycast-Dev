using RimWorld;
using Verse;

namespace MakaiTechPsycast
{
    public class ModExtension_TestingProjectile : DefModExtension
    {
        public float chance = 0.05f;
        public HediffDef hediff;
        public ThingDef thing;
        public bool healAlly;
    }
}
