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
	public class Dialog_ChooseIncidentCategory : Window
	{

		private bool thingWasChoosen = false;

		private Map map;
		private Pawn casterPawn;
		private int count;
		private int roll;
		/*private IncidentCategoryDef incidentCategoryDef;
		private List<IncidentDef> preOpenMakeListFromBase = DefDatabase<IncidentDef>.AllDefs.ToList();*/
		private List<IncidentCategoryDef> choose = new List<IncidentCategoryDef>();

		private Vector2 scrollPos;

		private float lastHeight;

		public override Vector2 InitialSize => new Vector2(350f, 300f);

		public Dialog_ChooseIncidentCategory(Map map, int choiceCount, int roll)
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
		private void ChoosedCategory(IncidentCategoryDef incidentDef, Map map, int choiceCount, int roll)
		{
			Find.WindowStack.Add(new Dialog_ChooseIncidentInCategory(incidentDef,map, count, roll));
		}

		public override void PreOpen()
		{
			base.PreOpen();
			if (choose != null)
			{
				choose.Clear();
			}
			if(roll == 1)
            {
				/*for (int i = 0; i < 4; i++)
				{
					choose.Add(DefDatabase<IncidentCategoryDef>.AllDefs.Where(x => !choose.Contains(x) && x == IncidentCategoryDefOf.ShipChunkDrop || x == IncidentCategoryDefOf.AllyAssistance || x == IncidentCategoryDefOf.OrbitalVisitor || x == IncidentCategoryDefOf.GiveQuest).FirstOrDefault());
				}*/
				foreach (IncidentCategoryDef item in DefDatabase<IncidentCategoryDef>.AllDefs.Where(x => !choose.Contains(x) && x == IncidentCategoryDefOf.ShipChunkDrop || x == IncidentCategoryDefOf.AllyAssistance || x == IncidentCategoryDefOf.OrbitalVisitor || x == IncidentCategoryDefOf.GiveQuest))
                {
					choose.Add(item);
                }
			}
			if (roll == 2)
			{
				foreach (IncidentCategoryDef item in DefDatabase<IncidentCategoryDef>.AllDefs)
				{
					choose.Add(item);
				}
			}
			if (roll == 3)
			{
				/*for (int i = 0; i < DefDatabase<IncidentCategoryDef>.AllDefs.Where(x => !choose.Contains(x) && x == IncidentCategoryDefOf.ThreatBig || x == IncidentCategoryDefOf.DeepDrillInfestation || x == IncidentCategoryDefOf.DiseaseAnimal || x == IncidentCategoryDefOf.DiseaseHuman).Count(); i++)
				{
					choose.Add(DefDatabase<IncidentCategoryDef>.AllDefs.Where(x => !choose.Contains(x) && x == IncidentCategoryDefOf.ThreatBig || x == IncidentCategoryDefOf.DeepDrillInfestation || x == IncidentCategoryDefOf.DiseaseAnimal || x == IncidentCategoryDefOf.DiseaseHuman).FirstOrDefault());
				}*/
				foreach(IncidentCategoryDef item in DefDatabase<IncidentCategoryDef>.AllDefs.Where(x => !choose.Contains(x) && x == IncidentCategoryDefOf.ThreatBig || x == IncidentCategoryDefOf.DeepDrillInfestation || x == IncidentCategoryDefOf.DiseaseAnimal || x == IncidentCategoryDefOf.DiseaseHuman))
                {
					choose.Add(item);
                }
			}
			/*foreach(IncidentCategoryDef item in DefDatabase<IncidentCategoryDef>.AllDefs)
            {
				choose.Add(item);
            }*/

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
				foreach (IncidentCategoryDef item in choose)
				{
					Rect rect = new Rect(0f, y, viewRect.width, 28f);
					rect.x = outRect.center.x - 150f;
					rect.width = viewRect.width * 0.8f;
					/*Widgets.Label(rect,hediff.LabelCap);*/
					if (Widgets.ButtonText(rect,"Category - " + item.defName, true, true, true))
					{
						thingWasChoosen = true;
						Close();
						ChoosedCategory(item,map,count,roll);
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
