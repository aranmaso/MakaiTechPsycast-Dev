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
	public class Dialog_ChooseRandomItem : Window
	{

		private bool thingWasChoosen = false;
		private Thing itemOnGround = null;
		private Map targetMap;
		private Pawn casterPawn;
		private int count;
		private int roll;
		private IntVec3 position;
		private List<ThingDef> preOpenMakeListFromBase = DefDatabase<ThingDef>.AllDefs.Where(x => x.category == ThingCategory.Item && !x.IsCorpse && x.BaseMarketValue > 0 && !x.defName.Contains("VPE_Psyring") && !x.defName.Contains("Psytrainer")).ToList();
		private List<ThingDef> choose5 = new List<ThingDef>();

		private Vector2 scrollPos;

		private float lastHeight;

		public override Vector2 InitialSize => new Vector2(250f, 300f);

		public Dialog_ChooseRandomItem(Thing thing,IntVec3 pos ,Map map, int choiceCount, int roll)
		{
			itemOnGround = thing;
			position = pos;
			this.targetMap = map;
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
		private void RandomItem(ThingDef thingDef)
		{
			Thing thing = null;
			if(thingDef.MadeFromStuff)
            {
				thing = ThingMaker.MakeThing(thingDef, DefDatabase<ThingDef>.AllDefsListForReading.Where((ThingDef stuffDef) => stuffDef.IsStuff).RandomElement());
			}
			else
            {
				thing = ThingMaker.MakeThing(thingDef);
			}
			itemOnGround.Destroy();
			thing.stackCount = Rand.Range(1,thing.def.stackLimit);
			GenSpawn.Spawn(thing, position, targetMap);
		}

		public override void PreOpen()
		{
			base.PreOpen();
			if (choose5 != null)
			{
				choose5.Clear();
			}
			if (roll == 1)
			{
				for (int i = 0; i < count; i++)
				{
					choose5.Add(preOpenMakeListFromBase.Where(x => !choose5.Contains(x)).RandomElement());
				}
			}
			else if (roll == 2)
			{
				for (int i = 0; i < count; i++)
				{
					choose5.Add(preOpenMakeListFromBase.Where(x => !choose5.Contains(x)).RandomElement());
				}
			}
			else if (roll == 3)
			{
				for (int i = 0; i < count; i++)
				{
					choose5.Add(preOpenMakeListFromBase.Where(x => !choose5.Contains(x)).RandomElement());
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
						RandomItem(item);
						SoundDefOf.Click.PlayOneShotOnCamera();
						return;
					}
					y += 32f;
					count++;
					if (count == this.count)
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
