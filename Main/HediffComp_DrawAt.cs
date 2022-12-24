using System;
using System.Collections.Generic;
using System.Text;
using RimWorld;
using UnityEngine;
using Verse;

namespace MakaiTechPsycast
{
    public class HediffComp_DrawAt : HediffComp
    {
        public HediffCompProperties_DrawAt Props => (HediffCompProperties_DrawAt)props;

        public GraphicData graphicData => Props.graphicData;

        public float rotation = 0;
        public float rotation2 = 0;
        public float rotation3 = 0;

        public Shader shader
        {
            get
            {
                if(Props.shaderTypeForMatrix == "MoteGlow")
                {
                    return ShaderDatabase.MoteGlow;
                }
                if(Props.shaderTypeForMatrix == "Transparent")
                {
                    return ShaderDatabase.Transparent;
                }
                if (Props.shaderTypeForMatrix == "TransparentPostLight")
                {
                    return ShaderDatabase.TransparentPostLight;
                }
                return ShaderDatabase.Cutout;
            }
        }
        public Shader shader2
        {
            get
            {
                if (Props.shaderTypeForMatrix2 == "MoteGlow")
                {
                    return ShaderDatabase.MoteGlow;
                }
                if (Props.shaderTypeForMatrix2 == "Transparent")
                {
                    return ShaderDatabase.Transparent;
                }
                if (Props.shaderTypeForMatrix2 == "TransparentPostLight")
                {
                    return ShaderDatabase.TransparentPostLight;
                }
                return ShaderDatabase.Cutout;
            }
        }
        public Shader shader3
        {
            get
            {
                if (Props.shaderTypeForMatrix3 == "MoteGlow")
                {
                    return ShaderDatabase.MoteGlow;
                }
                if (Props.shaderTypeForMatrix3 == "Transparent")
                {
                    return ShaderDatabase.Transparent;
                }
                if (Props.shaderTypeForMatrix3 == "TransparentPostLight")
                {
                    return ShaderDatabase.TransparentPostLight;
                }
                return ShaderDatabase.Cutout;
            }
        }
        private Material textureMat;
        public Material TextureMat
        {
            get
            {
                if (Props.texturePath != null && textureMat == null)
                {
                    textureMat = MaterialPool.MatFrom(Props.texturePath, shader);
                    return textureMat;
                }
                return textureMat;
            }
        }
        private Material textureMat2;
        public Material TextureMat2
        {
            get
            {
                if (Props.texturePath2 != null && textureMat2 == null)
                {
                    textureMat2 = MaterialPool.MatFrom(Props.texturePath2, shader2);
                    return textureMat2;
                }
                return textureMat2;
            }
        }
        private Material textureMat3;
        public Material TextureMat3
        {
            get
            {
                if (Props.texturePath3 != null && textureMat3 == null)
                {
                    textureMat3 = MaterialPool.MatFrom(Props.texturePath3, shader3);
                    return textureMat3;
                }
                return textureMat3;
            }
        }

        public virtual void DrawAt(Vector3 drawPos)
        {
            if (Props.graphicData != null)
            {
                drawPos.y = Props.altitudeLayerGraphicData.AltitudeFor();
                graphicData.Graphic.Draw(drawPos, Pawn.Rotation, Pawn);
            }
            if (Props.texturePath != null)
            {
                if(Props.onlyWhenDrafted && !Pawn.Drafted)
                {
                    return;
                }
                drawPos.y = Props.altitudeLayerMatrix.AltitudeFor();
                float angle = rotation;
                if(Props.rotateReverse)
                {
                    angle = -angle;
                }
                if (Props.notRotate)
                {
                    angle = 0;
                }
                Vector3 s = Props.drawOffset;
                Matrix4x4 matrix = default(Matrix4x4);
                matrix.SetTRS(drawPos, Quaternion.AngleAxis(angle, Vector3.up), s);
                Graphics.DrawMesh(MeshPool.plane10, matrix, TextureMat, 0);

                rotation += Props.rotateSpeed;

                if (rotation == 360)
                {
                    rotation = 0;
                }
            }
            if (Props.texturePath2 != null)
            {
                if (Props.onlyWhenDrafted2 && !Pawn.Drafted)
                {
                    return;
                }
                drawPos.y = Props.altitudeLayerMatrix2.AltitudeFor();
                float angle = rotation2;
                if (Props.rotateReverse2)
                {
                    angle = -angle;
                }
                if(Props.notRotate2)
                {
                    angle = 0;
                }
                Vector3 s = Props.drawOffset2;
                Matrix4x4 matrix = default(Matrix4x4);
                matrix.SetTRS(drawPos, Quaternion.AngleAxis(angle, Vector3.up), s);
                Graphics.DrawMesh(MeshPool.plane10, matrix, TextureMat2, 0);

                rotation2 += Props.rotateSpeed2;

                if (rotation2 == 360)
                {
                    rotation2 = 0;
                }
            }
            if (Props.texturePath3 != null)
            {
                if (Props.onlyWhenDrafted3 && !Pawn.Drafted)
                {
                    return;
                }
                drawPos.y = Props.altitudeLayerMatrix3.AltitudeFor();
                float angle = rotation3;
                if (Props.rotateReverse3)
                {
                    angle = -angle;
                }
                if (Props.notRotate3)
                {
                    angle = 0;
                }
                Vector3 s = Props.drawOffset3;
                Matrix4x4 matrix = default(Matrix4x4);
                matrix.SetTRS(drawPos, Quaternion.AngleAxis(angle, Vector3.up), s);
                Graphics.DrawMesh(MeshPool.plane10, matrix, TextureMat3, 0);

                rotation3 += Props.rotateSpeed3;

                if (rotation3 == 360)
                {
                    rotation3 = 0;
                }
            }
        }

    }
}
