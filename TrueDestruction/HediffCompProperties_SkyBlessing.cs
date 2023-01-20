using RimWorld;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace MakaiTechPsycast.TrueDestruction
{
    public class HediffCompProperties_SkyBlessing : HediffCompProperties
    {
        public string uiIcon;
        public HediffCompProperties_SkyBlessing()
        {
            compClass = typeof(HediffComp_SkyBlessing);
        }
    }
}
