using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;
using RimWorld;
using VFECore.Abilities;
using VFECore.UItils;

namespace MakaiTechPsycast.GoldenOrder
{
	public class Dialog_ChooseTraitToRemove : Window
	{
		private Ability_ReEducate traitEducated;

		private Vector2 scrollPosition;

		private bool traitWasChosen = false;

		private Pawn targetPawn;

		private Vector2 scrollPos;

		private float lastHeight;

		public override Vector2 InitialSize => new Vector2(200f, 500f);

		public Dialog_ChooseTraitToRemove(Pawn pawn)
		{
			this.targetPawn = pawn;
			forcePause = true;
			doCloseButton = false;
			doCloseX = true;
			closeOnClickedOutside = true;
			closeOnAccept = false;
			closeOnCancel = true;
		}
		private void RemoveTrait(Trait trait)
		{
			targetPawn.story?.traits.RemoveTrait(trait);
		}
		public override void DoWindowContents(Rect inRect)
		{
			Text.Font = GameFont.Small;
			Rect outRect = new Rect(inRect);
			outRect.yMin += 20f;
			outRect.yMax -= 40f;
			outRect.width -= 16f;
			Rect viewRect = new Rect(0f, 0f, inRect.width - 20f, lastHeight);
			float num = 0f;
			Widgets.BeginScrollView(inRect, ref scrollPos, viewRect);
			try
			{
				float y = 0f;
				bool flag = false;
				foreach (Trait traits in targetPawn.story?.traits?.allTraits)
				{
					flag = true;
					Rect rect = new Rect(0f, y, viewRect.width * 0.7f, 32f);
					rect.x = outRect.center.x - 63f;
					rect.width = viewRect.width * 0.5f;
					if (Widgets.ButtonText(rect, traits.LabelCap))
					{
						traitWasChosen = true;
						RemoveTrait(traits);
						Close();
						return;
					}
					y += 35f;
				}
				if (flag)
				{
					y += 15f;
				}
			}
			finally
			{
				Widgets.EndScrollView();
			}
		}

		public override void PostClose()
		{
			if (!traitWasChosen)
			{
			}
			base.PostClose();
		}
	}
}
