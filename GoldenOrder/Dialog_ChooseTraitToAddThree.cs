using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;
using Verse.AI;
using RimWorld;
using Verse.Sound;
using HarmonyLib;

namespace MakaiTechPsycast.GoldenOrder
{
	public class Dialog_ChooseTraitToAddThree : Window
	{
		private static readonly AccessTools.FieldRef<TraitDef, float> commonality = AccessTools.FieldRefAccess<TraitDef, float>("commonality");
		private Pawn targetPawn;
		private bool roll;
		private List<TraitDef> preOpenGetTraitFromBase = DefDatabase<TraitDef>.AllDefs.ToList();
		private List<TraitDef> choose3 = new List<TraitDef>();

		private Vector2 scrollPos;

		private float lastHeight;

		public override Vector2 InitialSize => new Vector2(250f, 300f);

		public Dialog_ChooseTraitToAddThree(Pawn pawn)
		{
			targetPawn = pawn;
			forcePause = true;
			doCloseButton = false;
			doCloseX = false;
			closeOnClickedOutside = false;
			closeOnAccept = false;
			closeOnCancel = false;
			draggable = true;
			resizeable = true;
		}

		public override void PreOpen()
		{
			base.PreOpen();
			if (choose3 != null)
			{
				choose3.Clear();
			}
			for (int i = 0; i < 3; i++)
			{
				choose3.Add(preOpenGetTraitFromBase.Where(x => !choose3.Contains(x) && commonality(x) > 0).RandomElement());
			}
		}

		private void doChangeTrait(TraitDef trait)
		{
			if(trait.degreeDatas.Count() <= 1)
            {
				targetPawn.story.traits.GainTrait(new Trait(trait));
			}
			else
            {
				targetPawn.story.traits.GainTrait(new Trait(trait,1));

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
				foreach (TraitDef item in choose3)
				{
					Rect rect = new Rect(0f, y, viewRect.width, 28f);
					rect.x = outRect.center.x - 100f;
					rect.width = viewRect.width * 0.8f;
					Rect rect2 = new Rect(0f, y + 20f, viewRect.width * 0.75f - 15f, 32f);
					/*Widgets.Label(rect,hediff.LabelCap);*/
					if (Widgets.ButtonText(rect, item.defName.ToString(), true, true, true))
					{
						Close();
						doChangeTrait(item);
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
	}
}
