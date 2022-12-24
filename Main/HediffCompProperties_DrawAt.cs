using RimWorld;
using UnityEngine;
using Verse;

namespace MakaiTechPsycast
{
    public class HediffCompProperties_DrawAt : HediffCompProperties
    {
        public GraphicData graphicData;
        public GraphicData graphicData2;
        public GraphicData graphicData3;

        public string texturePath;
        public string texturePath2;
        public string texturePath3;

        public bool rotateReverse;
        public bool rotateReverse2;
        public bool rotateReverse3;

        public AltitudeLayer altitudeLayerGraphicData;
        public AltitudeLayer altitudeLayerGraphicData2;
        public AltitudeLayer altitudeLayerGraphicData3;

        public AltitudeLayer altitudeLayerMatrix;
        public AltitudeLayer altitudeLayerMatrix2;
        public AltitudeLayer altitudeLayerMatrix3;

        public string shaderTypeForMatrix;
        public string shaderTypeForMatrix2;
        public string shaderTypeForMatrix3;

        public Vector3 drawOffset = Vector3.zero;
        public Vector3 drawOffset2 = Vector3.zero;
        public Vector3 drawOffset3 = Vector3.zero;

        public float rotateSpeed = 1f;
        public float rotateSpeed2 = 1f;
        public float rotateSpeed3 = 1f;

        public bool notRotate;
        public bool notRotate2;
        public bool notRotate3;

        public bool onlyWhenDrafted;
        public bool onlyWhenDrafted2;
        public bool onlyWhenDrafted3;

        public HediffCompProperties_DrawAt()
        {
            compClass = typeof(HediffComp_DrawAt);
        }
    }
}
