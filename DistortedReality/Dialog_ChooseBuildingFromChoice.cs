using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;
using RimWorld;
using Verse.Sound;

namespace MakaiTechPsycast.DistortedReality
{
	public class Dialog_ChooseBuildingFromChoice : Window
	{

		private bool thingWasChoosen = false;

		private Thing targetThing;
		private Pawn casterPawn;
		private int roll;
		private List<ThingDef> preOpenMakeListFromBase = DefDatabase<ThingDef>.AllDefs.Where(MakaiUtility.FindAllBuildingFromDatabase).ToList();
		private List<ThingDef> choose5 = new List<ThingDef>();

		private Vector2 scrollPos;

		private float lastHeight;

		public override Vector2 InitialSize => new Vector2(250f, 300f);

		public Dialog_ChooseBuildingFromChoice(Thing thing,Pawn pawn,int rolResult)
		{
			this.targetThing = thing;
			this.casterPawn = pawn;
			roll = rolResult;
			forcePause = true;
			doCloseButton = false;
			doCloseX = false;
			closeOnClickedOutside = false;
			closeOnAccept = false;
			closeOnCancel = false;
			draggable = true;
			resizeable = true;
		}
		private void RandomBuilding(ThingDef thing)
		{
			IntVec3 pos = targetThing.Position;
			Map map = targetThing.Map;
			try
            {
				Thing.allowDestroyNonDestroyable = true;
				targetThing.Destroy();
			}
			finally
            {
				Thing.allowDestroyNonDestroyable = false;
			}			
			if (thing.MadeFromStuff)
            {
                Thing newThing = ThingMaker.MakeThing(thing, DefDatabase<ThingDef>.AllDefsListForReading.Where((ThingDef stuffDef) => stuffDef.IsStuff).RandomElement());
				newThing.SetFaction(casterPawn.Faction);
				GenSpawn.Spawn(newThing, pos, map);
				
			}
			if (!thing.MadeFromStuff)
			{
				Thing newThing = ThingMaker.MakeThing(thing);
				newThing.SetStuffDirect(DefDatabase<ThingDef>.AllDefsListForReading.Where((ThingDef stuffDef) => stuffDef.IsStuff).RandomElement());
				newThing.SetFaction(casterPawn.Faction);
				GenSpawn.Spawn(newThing, pos, map);
			}
		}

        public override void PreOpen()
        {
            base.PreOpen();
			if(choose5 != null)
            {
				choose5.Clear();
            }
			for (int i = 0;i < 3;i++)
            {
				choose5.Add(preOpenMakeListFromBase.RandomElement());
			}
        }

        public override void DoWindowContents(Rect inRect)
		{
			Text.Font = GameFont.Small;
			Rect outRect = new Rect(inRect);
			outRect.yMin += 20f;
			outRect.yMax -= 40f;
			Rect viewRect = new Rect(0f, 0f, inRect.width, lastHeight);
			Widgets.BeginScrollView(inRect, ref scrollPos, viewRect, showScrollbars: true);
			int count = 0;
			try
			{
				float y = 5f;
				foreach (ThingDef item in choose5)
				{
					Rect rect = new Rect(0f, y, viewRect.width, 28f);
					rect.x = outRect.center.x - 100f;
					rect.width = viewRect.width * 0.8f;
					Rect rect2 = new Rect(0f, y + 20f, viewRect.width * 0.75f - 15f, 32f);
					/*Widgets.Label(rect,hediff.LabelCap);*/
					if (Widgets.ButtonText(rect, item.label, true, true, true))
					{
						thingWasChoosen = true;
						Close();
						RandomBuilding(item);
						SoundDefOf.Click.PlayOneShotOnCamera();
						return;
					}
					y += 32f;
					count++;
					if(count == 5)
                    {
						break;
                    }
				}
				lastHeight = y;
			}
			finally
			{
				Widgets.EndScrollView();
			}
		}

		public override void PostClose()
		{
			if (!thingWasChoosen)
			{
			}
			base.PostClose();
		}
	}
}
