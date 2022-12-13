using RimWorld;
using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace MakaiTechPsycast.DestinedDeath
{
    public class HediffComp_SoulCollection : HediffComp
    {
        public int SoulCount;
		public float BonustToStat;
        public int ticksSinceTrigger;
		public int ticksSincePsyRestore;
		public bool isRestoreFocus = false;
		public bool restoreFocus;
		HediffCompProperties_SoulCollection Props => (HediffCompProperties_SoulCollection)props;
		public override string CompLabelInBracketsExtra
		{
			get
			{
				if (SoulCount > 0)
				{
					return base.CompLabelInBracketsExtra + SoulCount + " Souls";
				}
				return base.CompLabelInBracketsExtra;
			}
		}
		public override void CompExposeData()
        {
            Scribe_Values.Look(ref SoulCount, "SoulCount", 1);
            Scribe_Values.Look(ref BonustToStat, "BonustToStat", 1);
        }
        public override void Notify_PawnKilled()
        {
            base.Notify_PawnKilled();
			GenExplosion.DoExplosion(parent.pawn.Position,parent.pawn.Map,Mathf.RoundToInt(SoulCount / 2),DamageDefOf.Bomb,parent.pawn,SoulCount/2,0.2f,null,null,null,null,null,0,1,null,false,null,0,1,0,false,null);
        }
        public override IEnumerable<Gizmo> CompGetGizmos()
        {
			Command_Action command_Action = new Command_Action();
			command_Action.defaultLabel = "Exchange soul for psyfocus";
			command_Action.defaultDesc = Props.restoreFocusDesc;
			command_Action.icon = ContentFinder<Texture2D>.Get(Props.uiIcon);
			command_Action.action = delegate
			{
				float focusDiff = 1f - parent.pawn.psychicEntropy.CurrentPsyfocus;
				if(parent.pawn.psychicEntropy.CurrentPsyfocus < 1f)
                {
					parent.pawn.psychicEntropy.OffsetPsyfocusDirectly(focusDiff);
					SoulCount -= 1;
					BonustToStat -= 1;
					parent.Severity = SoulCount;
				}
			};
			yield return command_Action;
			if(Prefs.DevMode)
            {
				Command_Action command_Action2 = new Command_Action();
				command_Action2.defaultLabel = "Debug: Fill Focus now";
				command_Action2.action = delegate
				{
					float focusDiff = 1f - parent.pawn.psychicEntropy.CurrentPsyfocus;
					if (parent.pawn.psychicEntropy.CurrentPsyfocus < 1f)
					{
						parent.pawn.psychicEntropy.OffsetPsyfocusDirectly(focusDiff);
					}
				};
				yield return command_Action2;
				Command_Action command_Action3 = new Command_Action();
				command_Action3.defaultLabel = "Debug: Increase Soul by 1";
				command_Action3.action = delegate
				{
					SoulCount += 1;
					BonustToStat += 1;
					parent.Severity = SoulCount;
				};
				yield return command_Action3;
			}

			/*Command_Toggle command_ToggleRestore = new Command_Toggle();
			if (isRestoreFocus)
			{
				command_ToggleRestore.defaultLabel = "Restore Focus";
				command_ToggleRestore.defaultDesc = Props.restoreFocusDesc;
			}
			else
			{
				command_ToggleRestore.defaultLabel = "Restore Disabled";
				command_ToggleRestore.defaultDesc = Props.restoreFocusDesc;
			}
			command_ToggleRestore.hotKey = KeyBindingDefOf.Command_ItemForbid;
			command_ToggleRestore.icon = ContentFinder<Texture2D>.Get(Props.uiIcon);
			command_ToggleRestore.isActive = () => isRestoreFocus;
			command_ToggleRestore.toggleAction = delegate
			{
				isRestoreFocus = !isRestoreFocus;
				if (isRestoreFocus)
				{
					restoreFocus = true;
				}
				else
				{
					restoreFocus = false;
				}
			};
			yield return command_ToggleRestore;*/
		}
        /*public override void CompPostMake()
		{
			base.CompPostMake();
			ticksSinceTrigger = Find.TickManager.TicksGame + Props.interval;
			ticksSincePsyRestore = Find.TickManager.TicksGame + Props.interval;
		}*/
        /*public override void CompPostTick(ref float severityAdjustment)
        {
            if (Find.TickManager.TicksGame == ticksSinceTrigger)
            {
				parent.Severity = SoulCount;
				BonustToStat = SoulCount;
				ticksSinceTrigger += Props.interval;
            }*/
			/*if (Find.TickManager.TicksGame == ticksSincePsyRestore)
            {
				if(parent.pawn.psychicEntropy.CurrentPsyfocus < 0.5f && restoreFocus && SoulCount > 0)
                {
					Log.Message("Restored");
					parent.pawn.psychicEntropy.OffsetPsyfocusDirectly(1f);
					SoulCount -= 1;
				}
				ticksSincePsyRestore += Props.interval;
			}*/
        
    }
}
