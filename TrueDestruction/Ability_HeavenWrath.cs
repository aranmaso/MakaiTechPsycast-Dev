using RimWorld;
using RimWorld.Planet;
using Verse;
using VFECore;
using VanillaPsycastsExpanded;
using UnityEngine;

namespace MakaiTechPsycast.TrueDestruction
{
    public class Ability_HeavenWrath : VFECore.Abilities.Ability
    {
        public int counter;

        public int interval = 120;

        public int tickBetween;

        public int strikeCounter = 0;

        public int level;

        public int strikeMax;

        public int countLeft;
        public override void Tick()
        {
            base.Tick();
            if(countLeft > 0)
            {
                tickBetween++;
                if (tickBetween >= Mathf.Max(interval - level * 2, 30))
                {
                    DoStrike();
                    tickBetween = 0;
                }
            }                        
        }
        public void DoStrike()
        {
            if (counter >= 0 && counter < 5)
            {
                foreach (Pawn pawns in pawn.Map.mapPawns.AllPawnsSpawned.InRandomOrder())
                {
                    if (!pawns.HostileTo(pawn.Faction) || !pawns.Faction.HostileTo(pawn.Faction) || pawns.Downed) continue;

                    MakaiTD_PowerBeam orbitalStrike = (MakaiTD_PowerBeam)GenSpawn.Spawn(MakaiTechPsy_DefOf.MakaiPsy_TD_Beam, pawns.Position, pawns.Map);
                    orbitalStrike.duration = 60;
                    orbitalStrike.instigator = pawn;
                    orbitalStrike.damageAmount = 2;
                    orbitalStrike.StartStrike();
                    Effecter effect = MakaiTechPsy_DefOf.MakaiPsy_TD_Blast.Spawn(pawns.Position, pawns.Map, 0.5f);
                    effect.Cleanup();

                    //pawns.Map.weatherManager.eventHandler.AddEvent(new WeatherEvent_LightningStrikeGreen(pawns.Map, pawns.Position));
                    strikeCounter++;
                    if (strikeCounter >= strikeMax) break;                    
                }
                level++;
                counter++;
                strikeCounter = 0;
                countLeft--;
            }
            if (counter >= 5 && counter < 10)
            {
                foreach (Pawn pawns in pawn.Map.mapPawns.AllPawnsSpawned.InRandomOrder())
                {
                    if (!pawns.HostileTo(pawn.Faction) || !pawns.Faction.HostileTo(pawn.Faction) || pawns.Downed) continue;

                    MakaiTD_PowerBeam orbitalStrike = (MakaiTD_PowerBeam)GenSpawn.Spawn(MakaiTechPsy_DefOf.MakaiPsy_TD_Beam, pawns.Position, pawns.Map);
                    orbitalStrike.duration = 60;
                    orbitalStrike.instigator = pawn;
                    orbitalStrike.damageAmount = 5;
                    orbitalStrike.StartStrike();
                    Effecter effect = MakaiTechPsy_DefOf.MakaiPsy_TD_Blast.Spawn(pawns.Position, pawns.Map, 0.5f);
                    effect.Cleanup();

                    //pawns.Map.weatherManager.eventHandler.AddEvent(new WeatherEvent_LightningStrikeGreen(pawns.Map, pawns.Position));
                    strikeCounter++;
                    if (strikeCounter >= strikeMax+1) break;
                }
                level++;
                counter++;
                strikeCounter = 0;
                countLeft--;
            }
            if (counter >= 10)
            {
                foreach (Pawn pawns in pawn.Map.mapPawns.AllPawnsSpawned.InRandomOrder())
                {
                    if (!pawns.HostileTo(pawn.Faction) || !pawns.Faction.HostileTo(pawn.Faction) || pawns.Downed) continue;

                    MakaiTD_PowerBeam orbitalStrike = (MakaiTD_PowerBeam)GenSpawn.Spawn(MakaiTechPsy_DefOf.MakaiPsy_TD_Beam, pawns.Position, pawns.Map);
                    orbitalStrike.duration = 60;
                    orbitalStrike.instigator = pawn;
                    orbitalStrike.damageAmount = 10;
                    orbitalStrike.StartStrike();
                    Effecter effect = MakaiTechPsy_DefOf.MakaiPsy_TD_Blast.Spawn(pawns.Position, pawns.Map, 0.5f);
                    effect.Cleanup();

                    //pawns.Map.weatherManager.eventHandler.AddEvent(new WeatherEvent_LightningStrikeGreen(pawns.Map, pawns.Position));
                    strikeCounter++;
                    if (strikeCounter >= strikeMax+2) break;
                }
                level++;
                counter++;
                strikeCounter = 0;
                countLeft--;
            }
        }
        public override void Cast(params GlobalTargetInfo[] targets)
        {
            base.Cast(targets);
            AbilityExtension_Roll1D20 modExtension = def.GetModExtension<AbilityExtension_Roll1D20>();
            RollInfo rollinfo = new RollInfo();
            rollinfo = MakaiUtility.Roll1D20(pawn, modExtension.skillBonus, rollinfo, modExtension.skillBonus2);
            if (rollinfo.roll >= modExtension.successThreshold && rollinfo.roll < modExtension.greatSuccessThreshold)
            {
                strikeMax = 5;
                countLeft = 30;
                level = 1;
                counter = 0;
                pawn.Map.weatherManager.curWeather = WeatherDef.Named("RainyThunderstorm");
                pawn.Map.weatherManager.TransitionTo(WeatherDef.Named("RainyThunderstorm"));
                Messages.Message("Makai_PassArollcheck".Translate(pawn.LabelShort, rollinfo.baseRoll, rollinfo.cumulativeBonusRoll, pawn.Named("USER")), pawn, MessageTypeDefOf.PositiveEvent);
                Messages.Message("Makai_PassArollcheckHeavenWrath".Translate(pawn.LabelShort, pawn.Named("USER")), pawn, MessageTypeDefOf.PositiveEvent);
            }
            if (rollinfo.roll >= modExtension.greatSuccessThreshold)
            {
                strikeMax = 5;
                countLeft = 60;
                level = 1;
                counter = 0;
                pawn.Map.weatherManager.curWeather = WeatherDef.Named("RainyThunderstorm");
                pawn.Map.weatherManager.TransitionTo(WeatherDef.Named("RainyThunderstorm"));
                Messages.Message("Makai_GreatPassArollcheck".Translate(pawn.LabelShort, rollinfo.baseRoll, rollinfo.cumulativeBonusRoll, pawn.Named("USER")), pawn, MessageTypeDefOf.PositiveEvent);
                Messages.Message("Makai_GreatPassArollcheckHeavenWrath".Translate(pawn.LabelShort, pawn.Named("USER")), pawn, MessageTypeDefOf.PositiveEvent);
            }
            if (rollinfo.roll < modExtension.successThreshold)
            {
                strikeMax = 3;
                countLeft = 15;
                level = 1;
                counter = 0;
                pawn.Map.weatherManager.curWeather = WeatherDef.Named("RainyThunderstorm");
                pawn.Map.weatherManager.TransitionTo(WeatherDef.Named("RainyThunderstorm"));
                Messages.Message("Makai_FailArollcheck".Translate(pawn.LabelShort, rollinfo.baseRoll, rollinfo.cumulativeBonusRoll, pawn.Named("USER")), pawn, MessageTypeDefOf.NegativeEvent);
                Messages.Message("Makai_FailArollcheckHeavenWrath".Translate(pawn.LabelShort, pawn.Named("USER")), pawn, MessageTypeDefOf.NegativeEvent);
            }
        }
    }
}
