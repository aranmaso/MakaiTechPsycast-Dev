using Verse;
using RimWorld;
using UnityEngine;
using System.Collections.Generic;

namespace MakaiTechPsycast
{
    public class HediffComp_DoThingOnExpire : HediffComp
    {
        public HediffCompProperties_DoThingOnExpire Props => (HediffCompProperties_DoThingOnExpire)props;

        public float damamgeBonus;
        public override void CompPostPostRemoved()
        {
            base.CompPostPostRemoved();
            DoEffect();
        }

        public void DoEffect()
        {
            if(Props.doMultipleEffecter)
            {
                foreach(EffecterDef item in Props.effecterDefs)
                {
                    Effecter effecter = item.Spawn(Pawn.Position,Pawn.Map);
                    effecter.Cleanup();
                }
            }
            else
            {
                Effecter effect = Props.effecterDef.Spawn(Pawn.Position, Pawn.Map);
                effect.Cleanup();
            }
            if(Props.damageDef != null)
            {
                BodyPartRecord bR = MakaiUtility.GetBodyPartFromDef(Pawn,Props.bodyPartDef);
                Pawn.TakeDamage(new DamageInfo(Props.damageDef,Props.damageAmount + damamgeBonus, Props.armorPenetration,hitPart: bR ?? null));
            }
            if(Props.doExplosion)
            {
                GenExplosion.DoExplosion(Pawn.Position,Pawn.Map,Props.explosionRadius, Props.explosionDamageDef ?? Props.damageDef,null,Mathf.FloorToInt(Props.damageAmount + damamgeBonus),Props.armorPenetration);
            }
            if(Props.projectileDef != null)
            {
                Vector3 spawnPosition = Pawn.Position.ToVector3Shifted();
                if(Props.spawnProjectileOffset)
                {
                    spawnPosition += Props.drawOffset;
                }
                Projectile projectile = (Projectile)GenSpawn.Spawn(Props.projectileDef, Pawn.Position, Pawn.Map);
                projectile.Launch(Pawn, spawnPosition,Pawn.Position,Pawn.Position, ProjectileHitFlags.IntendedTarget);
            }
            if(Props.fleckDef != null)
            {
                Vector3 fleckPosition = Pawn.Position.ToVector3Shifted();
                if(Props.fleckPosOffset != null)
                {
                    fleckPosition += Props.fleckPosOffset;
                }
                MakaiUtility.ThrowFleckStationary(Props.fleckDef, fleckPosition, Pawn.Map, 1f);
            }
        }
    }
}
