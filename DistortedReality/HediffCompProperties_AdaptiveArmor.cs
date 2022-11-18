using Verse;
using RimWorld;
using System.Collections.Generic;

namespace MakaiTechPsycast.DistortedReality
{
    public class HediffCompProperties_AdaptiveArmor : HediffCompProperties
    {
        public List<DamageDefInfoList> damageDefs;

        public List<ArmorCategoryInfoList> armorPenInfo;

        public List<HediffDef> possibleBuff;

        public float severityPerHit;

        public HediffCompProperties_AdaptiveArmor()
        {
            compClass = typeof(HediffComp_AdaptiveArmor);
        }
    }

    public class ArmorCategoryInfoList
    {
        public DamageArmorCategoryDef armorCategoryDef;

        public HediffDef armorRect;
    }

    public class DamageDefInfoList
    {
        public DamageDef damageDef;

        public HediffDef armorRect;
    }
}
