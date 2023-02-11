using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;
using RimWorld;
using VFECore.Abilities;
using VFECore.UItils;
using Verse.Sound;

namespace MakaiTechPsycast.DistortedReality
{
	public class Dialog_ChooseHediffToRandom : Window
	{

		private bool hediffWasChosen = false;

		private Pawn targetPawn;

		private Vector2 scrollPos;

		private float lastHeight;

		public override Vector2 InitialSize => new Vector2(250f, 500f);

		public Dialog_ChooseHediffToRandom(Pawn pawn)
		{
			this.targetPawn = pawn;
			forcePause = true;
			doCloseButton = false;
			doCloseX = true;
			closeOnClickedOutside = true;
			closeOnAccept = false;
			closeOnCancel = true;
			resizeable = true;
		}
		private void RandomHediff(Hediff hediff)
		{
			List<HediffDef> chooseOneFromAllHediff = DefDatabase<HediffDef>.AllDefs.Where(MakaiUtility.FindNonInjuryHediffFromDatabase).ToList();
			Hediff newHediff = HediffMaker.MakeHediff(chooseOneFromAllHediff.RandomElement(),targetPawn,hediff.Part);
			newHediff.Severity = 0.5f;
			if(newHediff.TryGetComp<HediffComp_Disappears>() != null)
            {
				newHediff.TryGetComp<HediffComp_Disappears>().ticksToDisappear += 6000;

			}
			if(newHediff.Part == null)
            {
				newHediff.Part = targetPawn.RaceProps.body.AllParts.FirstOrFallback((BodyPartRecord br) => br.def == MakaiTechPsy_DefOf.Pelvis);
            }
			targetPawn.health.AddHediff(newHediff);
			MoteMaker.ThrowText(targetPawn.Position.ToVector3(),targetPawn.Map,hediff.Label + " => " + newHediff.Label);
			targetPawn.health.RemoveHediff(hediff);
		}
		/*public override void DoWindowContents(Rect inRect)
		{
			Rect viewRect = new Rect(0f, 0f, inRect.width - 20f, lastHeight);
			float num = 5f;
			float y = 0f;
			Widgets.BeginScrollView(inRect, ref scrollPos, viewRect);
			foreach (Hediff hediff in targetPawn.health?.hediffSet?.hediffs)
			{
				Rect rect = new Rect(0f, num, viewRect.width, 32f);
				Rect rectbutton = new Rect(0f, num, viewRect.width * 5f, 32f);
				rect.width = viewRect.width;
				if (Widgets.ButtonText(rectbutton, hediff.Label))
				{
					hediffWasChosen = true;
					RandomHediff(hediff);
					Close();
				}
				num += 35f;
			}
			lastHeight = num;
			Widgets.EndScrollView();
		}*/

		public override void DoWindowContents(Rect inRect)
		{
			Text.Font = GameFont.Small;
			Rect outRect = new Rect(inRect);
			outRect.yMin += 20f;
			outRect.yMax -= 40f;
			Rect viewRect = new Rect(0f, 0f, inRect.width, lastHeight);
			Widgets.BeginScrollView(inRect, ref scrollPos, viewRect, showScrollbars: true);
			List<Hediff> pawnHediff = targetPawn.health.hediffSet.hediffs;
            try
            {
				float y = 5f;
				foreach (Hediff hediff in pawnHediff)
				{
					if(hediff.Visible && !(hediff is Hediff_MissingPart))
                    {
						Rect rect = new Rect(0f, y, viewRect.width, 28f);
						rect.x = outRect.center.x - 100f;
						rect.width = viewRect.width * 0.8f;
						Rect rect2 = new Rect(0f, y + 20f, viewRect.width * 0.75f - 15f, 32f);
						/*Widgets.Label(rect,hediff.LabelCap);*/
						if (Widgets.ButtonText(rect, hediff.LabelCap, true, true, true))
						{
							hediffWasChosen = true;
							Close();
							RandomHediff(hediff);
							SoundDefOf.Click.PlayOneShotOnCamera();
							return;
						}
						y += 32f;
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
			if (!hediffWasChosen)
			{
			}
			base.PostClose();
		}
	}
}
