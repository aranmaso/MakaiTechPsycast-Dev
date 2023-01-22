using RimWorld;
using Verse;
using System.Collections.Generic;
using UnityEngine;

namespace MakaiTechPsycast.DistortedReality
{
    public class HediffComp_DistortedShield : HediffComp
    {
        public int defenseCount;
        public bool stopOnlyEnemy;
        public int tickSinceTrigger;

        public HediffCompProperties_DistortedShield Props => (HediffCompProperties_DistortedShield)props;

        public override string CompLabelInBracketsExtra
        {
            get
            {
                if (defenseCount > 0)
                {
                    return base.CompLabelInBracketsExtra + defenseCount + " left";
                }
                return base.CompLabelInBracketsExtra;
            }
        }
        public override void CompExposeData()
        {
            {
                Scribe_Values.Look(ref defenseCount, "ShieldCount", 1);
                Scribe_Values.Look(ref tickSinceTrigger, "tickSinceTrigger", 0);
            }
        }
        public override void CompPostMake()
        {
            tickSinceTrigger = Find.TickManager.TicksGame + 5;
            defenseCount = Props.defenseCount;
            stopOnlyEnemy = Props.stopOnlyEnemy;
        }

        public override void CompPostTick(ref float severityAdjustment)
        {

            if (Find.TickManager.TicksGame == tickSinceTrigger)
            {
                foreach(Thing item in GenRadial.RadialDistinctThingsAround(Pawn.Position,Pawn.Map,2f,true))
                {
                    if(!(item is Projectile projectile))
                    {
                        continue;
                    }
                    if(stopOnlyEnemy && (projectile.Launcher.Faction.HostileTo(Pawn.Faction) || projectile.Launcher.HostileTo(Pawn)))
                    {
                        projectile.Destroy();
                        Effecter effect = MakaiTechPsy_DefOf.MakaiPsy_DD_Suck.Spawn(item.Position, Pawn.Map, 1);
                        effect.Cleanup();
                        defenseCount--;
                        if(defenseCount == 0)
                        {
                            Pawn.health.RemoveHediff(Pawn.health.hediffSet.GetFirstHediffOfDef(parent.def));
                        }
                    }
                    else if(!stopOnlyEnemy)
                    {
                        projectile.Destroy();
                        Effecter effect = MakaiTechPsy_DefOf.MakaiPsy_DD_Suck.Spawn(item.Position, Pawn.Map, 1);
                        effect.Cleanup();
                        defenseCount--;
                        if (defenseCount == 0)
                        {
                            Pawn.health.RemoveHediff(Pawn.health.hediffSet.GetFirstHediffOfDef(parent.def));
                        }
                    }                    
                }        
                tickSinceTrigger += 5;
            }
            /*Vector3 pos = Pawn.DrawPos;
            pos.y = AltitudeLayer.MoteOverhead.AltitudeFor();
            Matrix4x4 matrix = default(Matrix4x4);
            matrix.SetTRS(pos, Quaternion.Euler(0, rotation, 0), new Vector3(1.5f, 1f, 1.5f));
            Graphics.DrawMesh(MeshPool.plane10, matrix, bubbleMat, 0, null, 0, materialBlock);*/
        }
    }
}
