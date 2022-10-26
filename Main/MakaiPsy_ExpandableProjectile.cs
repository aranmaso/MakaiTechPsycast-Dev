using System.Collections.Generic;
using System.Linq;
using RimWorld;
using Verse;
using VFECore;

namespace MakaiTechPsycast
{
	public class MakaiPsy_ExpandableProjectile : ExpandableProjectile
	{
		public override void DoDamage(IntVec3 pos)
		{
			base.DoDamage(pos);
			try
			{
				if (!(pos != launcher.Position) || launcher.Map == null || !pos.InBounds(launcher.Map))
				{
					return;
				}
				List<Thing> list = launcher.Map.thingGrid.ThingsListAt(pos);
				for (int num = list.Count - 1; num >= 0; num--)
				{
					if (IsDamagable(list[num]))
					{
						customImpact = true;
						Impact(list[num],false);
						customImpact = false;
					}
				}
			}
			catch
			{
			}
		}
		public override bool IsDamagable(Thing t)
		{
			if (base.IsDamagable(t))
			{
				return t.def != ThingDefOf.Fire;
			}
			return false;
		}
	}

}
