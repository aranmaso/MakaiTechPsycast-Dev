using HarmonyLib;
using RimWorld;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace MakaiTechPsycast
{
	public class CompFleckEmitter : ThingComp
	{
		private static readonly AccessTools.FieldRef<Projectile, Vector3> destination = AccessTools.FieldRefAccess<Projectile, Vector3>("destination");

		public int ticksSinceLastEmitted;

		private bool active = false;
		private CompProperties_FleckEmitter Props => (CompProperties_FleckEmitter)props;
		public override void CompTick()
		{
			if (!active && Props.alwaysOn)
			{
				active = true;
			}
			if (!active)
			{
				return;
			}
			CellRect currentViewRect = Find.CameraDriver.CurrentViewRect;
			currentViewRect.ClipInsideMap(parent.Map);
			if (!currentViewRect.Contains(parent.Position))
            {
				return;
            }
			base.CompTick();
			if (ticksSinceLastEmitted <= 0)
			{
				if (Props.moteDefs != null)
				{
					for(int i = 0; i < Props.countPerEmit;i++)
                    {
						Emit();
					}
				}
				else if(Props.fleckDefs != null && Props.useFleck)
                {
					for (int i = 0; i < Props.countPerEmit; i++)
					{
						Emit();
					}
				}
				ticksSinceLastEmitted = Rand.Range(Props.minEmissionInterval, Props.maxEmissionInterval);
			}
			else
			{
				ticksSinceLastEmitted--;
			}
		}

		private void Emit()
		{
			if (parent is Projectile projectile)
			{
				if(!Props.useFleck)
                {
					MoteThrown moteThrown = (MoteThrown)ThingMaker.MakeThing(Props.moteDefs.RandomElement());
					moteThrown.Scale = Rand.Range(Props.scale, Props.scaleMax);
					moteThrown.rotationRate = Rand.Range(Props.rotationMinRate, Props.rotationMaxRate);
					moteThrown.exactPosition = projectile.DrawPos + new Vector3(Rand.Range(Props.minRandomOffsetX, Props.maxRandomOffsetX), 0.5f, Rand.Range(Props.minRandomOffsetY, Props.maxRandomOffsetY));
					moteThrown.exactRotation = projectile.Rotation.AsAngle;
					if (Props.propelBackward)
					{
						if(projectile.usedTarget.Thing is Pawn usedTarget)
                        {
							moteThrown.Velocity = (parent.Position.ToVector3() - destination(projectile)) / Props.propelSlowdown;
						}
						else
                        {
							moteThrown.Velocity = (parent.Position.ToVector3() - projectile.usedTarget.Cell.ToVector3()) / Props.propelSlowdown;
						}
						
					}
					else
					{
						moteThrown.SetVelocity((float)Rand.Range(Props.minRotate, Props.maxRotate) + parent.Rotation.AsAngle, Rand.Range(Props.minSpeed, Props.maxSpeed));
					}
					GenSpawn.Spawn(moteThrown, parent.Position, parent.Map);
				}			
				else if(Props.useFleck)
                {
					Vector3 vector = projectile.DrawPos + new Vector3(Rand.Range(Props.minRandomOffsetX, Props.maxRandomOffsetX), 0.5f, Rand.Range(Props.minRandomOffsetY, Props.maxRandomOffsetY));
					if (vector.ShouldSpawnMotesAt(projectile.Map))
					{
						if (vector.InBounds(projectile.Map))
						{
							FleckCreationData dataStatic = MakaiUtility.GetDataStatic(vector, projectile.Map, Props.fleckDefs.RandomElement(), Rand.Range(Props.scale,Props.scaleMax));
							dataStatic.rotationRate = Rand.Range(Props.rotationMinRate, Props.rotationMaxRate);
							if(Props.propelBackward)
                            {
								dataStatic.velocity = (parent.Position.ToVector3() - projectile.usedTarget.Cell.ToVector3()) / Props.propelSlowdown;
							}
							dataStatic.velocityAngle = Rand.Range(0, 360);
							dataStatic.velocitySpeed = 0.12f;
							projectile.Map.flecks.CreateFleck(dataStatic);
						}
					}
				}
			}
		}


	}
}
