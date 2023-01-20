using RimWorld;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace MakaiTechPsycast.TrueDestruction
{
    public class HediffCompProperties_BlazingTrail : HediffCompProperties
    {
        public string uiIcon;
        public HediffCompProperties_BlazingTrail()
        {
            compClass = typeof(HediffComp_BlazingTrail);
        }
    }
}
