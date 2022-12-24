using System;
using System.Collections.Generic;
using System.Text;
using RimWorld;
using UnityEngine;
using Verse;

namespace MakaiTechPsycast.StringOfFate
{
    public class HediffComp_UltimateFate : HediffComp
    {
        public HediffCompProperties_UltimateFate Props => (HediffCompProperties_UltimateFate)props;

        public int fatedCount = 0;

        public float totalDamage = 0;

        public int threshold = 0;

        public int maxThresholdPerHit = 0;

        public float rotation = 0;

        private Material bubbleMat;
        public Material BubbleMat
        {
            get
            {
                if(Props.texturePath != null && bubbleMat == null)
                {
                    bubbleMat = MaterialPool.MatFrom(Props.texturePath, ShaderDatabase.MoteGlow);
                    return bubbleMat;
                }
                return bubbleMat;
            }
        }
        public GraphicData graphicData => Props.graphicData;

        public override bool CompShouldRemove
        {
            get
            {
                if(fatedCount <= 0)
                {
                    return true;
                }
                return false;
            }
        }

        public override void CompExposeData()
        {
            base.CompExposeData();
            Scribe_Values.Look(ref fatedCount,"fatedCount",0);
            Scribe_Values.Look(ref totalDamage, "totalDamage", 0);
            Scribe_Values.Look(ref maxThresholdPerHit, "maxThresholdPerHit", 0);
            Scribe_Values.Look(ref threshold, "threshold", 0);
        }
        public override string CompLabelInBracketsExtra
        {
            get
            {                
                if(fatedCount > 0)
                {
                    return base.CompLabelInBracketsExtra + fatedCount + "(" + Mathf.FloorToInt(totalDamage) + ")";
                }
                return base.CompLabelInBracketsExtra;
            }
        }
        public virtual void DrawAt(Vector3 drawPos)
        {
            if (Props.graphicData != null)
            {
                drawPos.y = AltitudeLayer.Terrain.AltitudeFor();
                graphicData.Graphic.Draw(drawPos, Pawn.Rotation, Pawn);                
            }
            if (Props.texturePath != null)
            {
                drawPos.y = AltitudeLayer.Terrain.AltitudeFor();
                float angle = rotation;
                Vector3 s = new Vector3(2f, -0.2f, 2f);
                Matrix4x4 matrix = default(Matrix4x4);
                matrix.SetTRS(drawPos, Quaternion.AngleAxis(angle, Vector3.up), s);
                Graphics.DrawMesh(MeshPool.plane10, matrix, BubbleMat, 0);
            }
            rotation++;
            if(rotation == 360f)
            {
                rotation = 0;
            }
        }
        public override string CompDescriptionExtra => CustomDescription(base.CompDescriptionExtra);
        public string CustomDescription(string Desc)
        {
            StringBuilder builder = new StringBuilder(Desc);
            builder.AppendLine();
            builder.AppendLine();
            builder.AppendLine("threshold: " + threshold);
            builder.AppendLine("maxThresholdPerHit: " + maxThresholdPerHit);
            return builder.ToString().TrimEndNewlines();
        }
        public override void Notify_PawnPostApplyDamage(DamageInfo dinfo, float totalDamageDealt)
        {
            base.Notify_PawnPostApplyDamage(dinfo, totalDamageDealt);
            totalDamage += totalDamageDealt;
        }
    }
}
