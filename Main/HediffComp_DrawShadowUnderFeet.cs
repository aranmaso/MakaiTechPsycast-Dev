using System.Collections.Generic;
using RimWorld;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace MakaiTechPsycast
{
    [StaticConstructorOnStartup]
    public class HediffComp_DrawShadowUnderFeet : HediffComp
    {
        private static readonly Material shadowMaterial = MaterialPool.MatFrom("Things/Skyfaller/SkyfallerShadowCircle", ShaderDatabase.Transparent);

        private HediffCompProperties_DrawShadowUnderFeet Props => (HediffCompProperties_DrawShadowUnderFeet)props;

        public override void CompPostTick(ref float severityAdjustment)
        {
            DrawThing(Pawn.DrawPos);
        }
        public void DrawThing(Vector3 drawLoc, float height = 1f)
        {
            if (shadowMaterial != null)
            {
                float num = Props.shadowSize * Mathf.Lerp(1f, 0.6f, height);
                Vector3 s = new Vector3(num, 1f, num);
                Vector3 vector = new Vector3(0f, -0.01f, 0f);
                Matrix4x4 matrix = default(Matrix4x4);
                matrix.SetTRS(drawLoc + vector, Quaternion.identity, s);
                Graphics.DrawMesh(MeshPool.plane10, matrix, shadowMaterial, 0);
            }
        }
    }
}
