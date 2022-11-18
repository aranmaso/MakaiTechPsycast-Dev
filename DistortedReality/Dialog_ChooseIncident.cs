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
	public class Dialog_ChooseIncident : Window
	{

		private bool thingWasChoosen = false;

		private Map map;
		private Pawn casterPawn;
		private int count;
		private int roll;
		private List<IncidentDef> preOpenMakeListFromBase = DefDatabase<IncidentDef>.AllDefs.ToList();
		private List<IncidentDef> choose5 = new List<IncidentDef>();

		private Vector2 scrollPos;

		private float lastHeight;

		public override Vector2 InitialSize => new Vector2(250f, 300f);

		public Dialog_ChooseIncident(Map map,int choiceCount,int roll)
		{
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
		private void RandomEvent(IncidentDef incidentDef)
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
			if (choose5 != null)
			{
				choose5.Clear();
			}
			if(roll == 1)
            {
				for (int i = 0; i < count; i++)
				{
					choose5.Add(preOpenMakeListFromBase.Where(x => !choose5.Contains(x)).RandomElement());
				}
			}
			else if(roll == 2)
            {
				for (int i = 0; i < count; i++)
				{
					choose5.Add(preOpenMakeListFromBase.Where(x => !choose5.Contains(x)).RandomElement());
				}
			}
			else if(roll == 3)
            {
				for (int i = 0; i < count; i++)
				{
					choose5.Add(preOpenMakeListFromBase.Where(x => (x.category == IncidentCategoryDefOf.ThreatBig) && !choose5.Contains(x)).RandomElement());
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
			int count = 0;
			try
			{
				float y = 5f;
				foreach (IncidentDef item in choose5)
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
						RandomEvent(item);
						SoundDefOf.Click.PlayOneShotOnCamera();
						return;
					}
					y += 32f;
					count++;
					if (count == 5)
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
