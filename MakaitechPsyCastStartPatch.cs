using HarmonyLib;
using Verse;

namespace MakaiTechPsycast
{
	[StaticConstructorOnStartup]
	public static class MakaitechPsyCastStartPatch
	{
		static MakaitechPsyCastStartPatch()
		{
			new Harmony("FarmerJoe.MakaiTechPsy").PatchAll();
			Log.Message("MakaiTech Psycast patch successful");
		}
	}
}
