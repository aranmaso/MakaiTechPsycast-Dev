using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;
using Verse.AI;
using RimWorld;
using VEF.Abilities;
using VEF.Utils;
using Verse.Sound;
using HarmonyLib;

namespace MakaiTechPsycast.GoldenOrder
{
	public class Dialog_ChooseTraitToRemove : Window
	{
		//private static readonly AccessTools.FieldRef<TraitDef, float> commonality = AccessTools.FieldRefAccess<TraitDef, float>("commonality");
		private Pawn targetPawn;
		private bool roll;
		private List<TraitDef> preOpenGetTraitFromBase = DefDatabase<TraitDef>.AllDefs.Where(x => x.commonality > 0).ToList();

		private Vector2 scrollPos;

		private float lastHeight;

		public override Vector2 InitialSize => new Vector2(250f, 300f);

		public Dialog_ChooseTraitToRemove(Pawn pawn, bool great)
		{
			targetPawn = pawn;
			roll = great;
			forcePause = true;
			doCloseButton = false;
			doCloseX = false;
			closeOnClickedOutside = false;
			closeOnAccept = false;
			closeOnCancel = false;
			draggable = true;
			resizeable = true;
		}

		private void doChangeTrait(Trait trait)
        {
			if(!roll)
            {
				targetPawn.story.traits.RemoveTrait(trait);
				TraitDef selectedTrait = preOpenGetTraitFromBase.RandomElement();				
				if (selectedTrait.degreeDatas.Count() <= 1)
				{
					targetPawn.story.traits.GainTrait(new Trait(selectedTrait));
				}
				else
				{
					targetPawn.story.traits.GainTrait(new Trait(selectedTrait, 1));

				}
			}
			else
            {
				targetPawn.story.traits.RemoveTrait(trait);
				Find.WindowStack.Add(new Dialog_ChooseTraitToAddThree(targetPawn));
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
				foreach (Trait item in targetPawn.story.traits.allTraits)
				{
					Rect rect = new Rect(0f, y, viewRect.width, 28f);
					rect.x = outRect.center.x - 100f;
					rect.width = viewRect.width * 0.8f;
					Rect rect2 = new Rect(0f, y + 20f, viewRect.width * 0.75f - 15f, 32f);
					/*Widgets.Label(rect,hediff.LabelCap);*/
					if (Widgets.ButtonText(rect, item.Label, true, true, true))
					{
						Close();
						doChangeTrait(item);
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
		}
	}
}
