using Verse;

namespace MakaiTechPsycast.DestinedDeath
{
    public class HediffCompProperties_Decay : HediffCompProperties
    {
        public int interval = 250;

        public DamageDef damageDef;

        public float damageAmount = 2f;

        public HediffCompProperties_Decay()
        {
            compClass = typeof(HediffComp_Decay);
        }
    }

}
