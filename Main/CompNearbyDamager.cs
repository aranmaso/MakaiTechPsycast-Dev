using Verse;
using RimWorld;
using System.Collections.Generic;

namespace MakaiTechPsycast
{
    public class CompNearbyDamager : ThingComp
    {
        public CompProperties_NearbyDamager Props => (CompProperties_NearbyDamager)props;

        public int tickSinceTriggerContact;
        public int tickSinceTriggerSpew;
        public override void PostExposeData()
        {
            Scribe_Values.Look(ref tickSinceTriggerContact, "tickSinceTriggerContact", 0);            
            Scribe_Values.Look(ref tickSinceTriggerSpew, "tickSinceTriggerSpew", 0);            
        }

        public override void PostPostMake()
        {
            tickSinceTriggerContact = Find.TickManager.TicksGame + Props.contactInterval;
            tickSinceTriggerSpew = Find.TickManager.TicksGame + Props.spewInterval;
            base.PostPostMake();
        }

        public override void CompTick()
        {
            base.CompTick();
            if(Props.damageDefs != null)
            {
                if (Find.TickManager.TicksGame == tickSinceTriggerContact)
                {
                    doTriggerContact();
                    tickSinceTriggerContact += Props.contactInterval;
                }
            }                
            if(Props.projectileDefs != null)
            {
                if (Find.TickManager.TicksGame == tickSinceTriggerSpew)
                {
                    doTriggerSpew();
                    tickSinceTriggerSpew += Props.spewInterval;
                }
            }            
        }
        public void doTriggerSpew()
        {
            if (Props.projectileDefs != null)
            {
                int count = 0;
                IDictionary<Pawn, IntVec3> pawnWithLocation = MakaiUtility.GetNearbyPawnWithPosition(parent.Position, parent.Map, Props.spewRadius);
                foreach (Pawn pawn in pawnWithLocation.Keys)
                {
                    if (pawn.Faction != parent.Faction || pawn.Faction.HostileTo(parent.Faction) || pawn.HostileTo(parent.Faction))
                    {                        
                        Projectile projectile = (Projectile)GenSpawn.Spawn(Props.projectileDefs.RandomElement(), parent.Position, parent.Map);
                        if (!Props.spawnFromAbove)
                        {
                            projectile.Launch(parent, pawnWithLocation[pawn], pawn, ProjectileHitFlags.IntendedTarget);
                        }
                        else
                        {
                            IntVec3 offset = new IntVec3(pawnWithLocation[pawn].x, pawnWithLocation[pawn].y, pawnWithLocation[pawn].z + 5);
                            projectile.Launch(parent, offset.ToVector3(), pawn, pawn, ProjectileHitFlags.IntendedTarget);
                        }
                        count++;
                    }                    
                    if (count >= 5)
                    {
                        break;
                    }
                }
            }
        }   
        public void doTriggerContact()
        {
            if(Props.damageDefs != null)
            {
                if(Props.contactAffectFriendly)
                {
                    foreach (Pawn pawn in MakaiUtility.GetNearbyPawnFriendAndFoe(parent.Position, parent.Map, Props.contactRadius))
                    {
                        BodyPartRecord br = null;
                        if (!pawn.Downed)
                        {
                            br = pawn.RaceProps.body.AllParts.RandomElement();
                        }                       
                        else
                        {
                            br = pawn.RaceProps.body.AllParts.FirstOrDefault(x => x.height == BodyPartHeight.Middle);
                        }
                        pawn.TakeDamage(new DamageInfo(Props.damageDefs.RandomElement(), Props.damageAmount, 2f,hitPart: br ?? null));
                    }
                }
                else
                {
                    foreach (Pawn pawn in MakaiUtility.GetNearbyPawnFoeOnly(parent.Position, parent.Faction, parent.Map, Props.contactRadius))
                    {
                        BodyPartRecord br = null;
                        if (!pawn.Downed)
                        {
                            br = pawn.RaceProps.body.AllParts.RandomElement();
                        }
                        else
                        {
                            br = pawn.RaceProps.body.AllParts.FirstOrDefault(x => x.def == BodyPartDefOf.Torso);
                        }
                        pawn.TakeDamage(new DamageInfo(Props.damageDefs.RandomElement(), Props.damageAmount, 2f,hitPart: br ?? null));
                    }
                }                
            }              
        }
    }
}
