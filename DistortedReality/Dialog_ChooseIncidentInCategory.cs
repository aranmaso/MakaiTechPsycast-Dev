using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;
using Verse.AI;
using RimWorld;
using VFECore.Abilities;
using VFECore.UItils;
using Verse.Sound;
using System;

namespace MakaiTechPsycast.DistortedReality
{
	public class Dialog_ChooseIncidentInCategory : Window
	{

		private bool thingWasChoosen = false;

		private IncidentCategoryDef incCagDef;
		private Map map;
		private Pawn casterPawn;
		private int count;
		private int roll;
		private List<IncidentDef> choose = new List<IncidentDef>();

		private Vector2 scrollPos;

		private float lastHeight;

		public override Vector2 InitialSize => new Vector2(250f, 300f);

		public Dialog_ChooseIncidentInCategory(IncidentCategoryDef incidentCagDef, Map map, int choiceCount, int roll)
		{
			this.incCagDef = incidentCagDef;
			this.map = map;
			this.count = choiceCount;
			this.roll = roll;
			forcePause = true;
			doCloseButton = false;
			doCloseX = false;
			closeOnClickedOutside = false;
			closeOnAccept = false;
			closeOnCancel = false;
			draggable = true;
			resizeable = true;
		}
		private void DoEvent(IncidentDef incidentDef)
		{
			int num = 0;
			do
			{
				try
				{
					if (incidentDef.Worker.TryExecute(StorytellerUtility.DefaultParmsNow(incidentDef.category, map)))
					{
						return;
					}
				}
				catch (Exception)
				{
				}
				num++;
			}
			while (num <= 500);
			Log.Error("[VPE] Exceeded 500 tries to spawn random event");
		}

		public override void PreOpen()
		{
			base.PreOpen();
			if (choose != null)
			{
				choose.Clear();
			}
			if (roll == 1)
			{
				/*for (int i = 0; i < DefDatabase<IncidentDef>.AllDefs.Where(x => !choose.Contains(x) && x.category == incCagDef).Count(); i++)
				{
					choose.Add(DefDatabase<IncidentDef>.AllDefs.Where(x => !choose.Contains(x) && x.category == incCagDef).FirstOrDefault());
				}*/
				foreach(IncidentDef item in DefDatabase<IncidentDef>.AllDefs.Where(x => !choose.Contains(x) && x.category == incCagDef))
                {
					choose.Add(item);
                }
			}
			else if (roll == 2)
			{
				/*for (int i = 0; i < DefDatabase<IncidentDef>.AllDefs.Where(x => !choose.Contains(x) && x.category == incCagDef).Count(); i++)
				{
					choose.Add(DefDatabase<IncidentDef>.AllDefs.Where(x => !choose.Contains(x) && x.category == incCagDef).FirstOrDefault());
				}*/
				foreach (IncidentDef item in DefDatabase<IncidentDef>.AllDefs.Where(x => !choose.Contains(x) && x.category == incCagDef))
				{
					choose.Add(item);
				}
			}
			else if (roll == 3)
			{
				/*for (int i = 0; i < DefDatabase<IncidentDef>.AllDefs.Where(x => !choose.Contains(x) && x.category == incCagDef).Count(); i++)
				{
					choose.Add(DefDatabase<IncidentDef>.AllDefs.Where(x => !choose.Contains(x) && x.category == incCagDef).FirstOrDefault());
				}*/
				foreach (IncidentDef item in DefDatabase<IncidentDef>.AllDefs.Where(x => !choose.Contains(x) && x.category == incCagDef))
				{
					choose.Add(item);
				}
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
			try
			{
				float y = 5f;
				Rect rectBack = new Rect(0f, y, viewRect.width, 28f);
				rectBack.x = outRect.center.x - 100f;
				rectBack.width = viewRect.width * 0.8f;
				Rect rect2Back = new Rect(0f, y + 20f, viewRect.width * 0.75f - 15f, 32f);
				if (Widgets.ButtonText(rectBack, "<< Back", true, true, true))
				{
					Close();
					Find.WindowStack.Add(new Dialog_ChooseIncidentCategory(map, count, roll));
					SoundDefOf.Click.PlayOneShotOnCamera();
					return;
				}
				y += 32f;
				foreach (IncidentDef item in choose)
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
						DoEvent(item);
						SoundDefOf.Click.PlayOneShotOnCamera();
						return;
					}
					y += 32f;
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
