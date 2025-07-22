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
            Scribe_Values.Look(ref defenseCount, "ShieldCount", 1);
            Scribe_Values.Look(ref stopOnlyEnemy, "stopOnlyEnemy", false);
        }
        public override void CompPostMake()
        {
            defenseCount = Props.defenseCount;
            stopOnlyEnemy = Props.stopOnlyEnemy;
        }
        public void DoTrigger()
        {
            foreach (var item in GenRadial.RadialDistinctThingsAround(Pawn.Position, Pawn.Map, 2f, true))
            {
                if (item is not Projectile projectile)
                {
                    continue;
                }
                if(stopOnlyEnemy && !projectile.Launcher.Faction.HostileTo(Pawn.Faction) || !projectile.Launcher.HostileTo(Pawn))
                {
                    continue;
                }

                projectile.Launch(parent.pawn, projectile.Launcher, projectile.Launcher, ProjectileHitFlags.IntendedTarget);                
                Effecter effect = MakaiTechPsy_DefOf.MakaiPsy_DD_Suck.Spawn(item.Position, Pawn.Map, 1);
                effect.Cleanup();
                defenseCount--;
                if (defenseCount == 0)
                {
                    Pawn.health.RemoveHediff(Pawn.health.hediffSet.GetFirstHediffOfDef(parent.def));
                }
            }
        }
        public override void CompPostTick(ref float severityAdjustment)
        {
            if(Pawn.IsHashIntervalTick(5))
            {
                DoTrigger();
            }
        }
    }
}
