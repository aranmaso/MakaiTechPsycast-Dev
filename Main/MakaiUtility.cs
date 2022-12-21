using System;
using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using System.Text;
using RimWorld;
using UnityEngine;
using Verse;
using VanillaPsycastsExpanded;

namespace MakaiTechPsycast
{
    public static class MakaiUtility
    {
        public static Hediff_Injury FindInjury(Pawn pawn, IEnumerable<BodyPartRecord> allowedBodyParts = null)
        {
            Hediff_Injury hediff_Injury = null;
            List<Hediff> hediffs = pawn.health.hediffSet.hediffs;
            for (int i = 0; i < hediffs.Count; i++)
            {
                if (hediffs[i] is Hediff_Injury hediff_Injury2 && hediff_Injury2.Visible && hediff_Injury2.def.everCurableByItem && (allowedBodyParts == null || allowedBodyParts.Contains(hediff_Injury2.Part)) && (hediff_Injury == null || hediff_Injury2.Severity > hediff_Injury.Severity))
                {
                    hediff_Injury = hediff_Injury2;
                }
            }
            return hediff_Injury;
        }
        public static bool FindNonInjuryHediffFromDatabase(HediffDef hediff)
        {
            return hediff.hediffClass != typeof(Hediff_Injury) && hediff.hediffClass != typeof(Hediff_MissingPart);
        }
        public static bool FindAllBuildingFromDatabase(ThingDef thingDef)
        {
            return thingDef.thingClass != null && (thingDef.thingClass == typeof(Building) || thingDef.thingClass.IsSubclassOf(typeof(Building))) && thingDef.thingClass != typeof(Frame);
        }
        public static TaggedString RestorePart(BodyPartRecord part, Pawn pawn)
        {
            pawn.health.RestorePart(part);
            return "HealingRestoreBodyPart".Translate(pawn, part.Label);
        }
        public static TaggedString Cure(Hediff hediff)
        {
            Pawn pawn = hediff.pawn;
            pawn.health.RemoveHediff(hediff);
            if (hediff.def.cureAllAtOnceIfCuredByItem)
            {
                int num = 0;
                while (true)
                {
                    num++;
                    if (num > 10000)
                    {
                        Log.Error("Too many iterations.");
                        break;
                    }
                    Hediff firstHediffOfDef = pawn.health.hediffSet.GetFirstHediffOfDef(hediff.def);
                    if (firstHediffOfDef == null)
                    {
                        break;
                    }
                    pawn.health.RemoveHediff(firstHediffOfDef);
                }
            }
            return "HealingCureHediff".Translate(pawn, hediff.def.label);
        }

        public static BodyPartRecord FindSmallestMissingBodyPart(Pawn pawn, float minCoverage = 0f)
        {
            BodyPartRecord bodyPartRecord = null;
            foreach (Hediff_MissingPart missingPartsCommonAncestor in pawn.health.hediffSet.GetMissingPartsCommonAncestors())
            {
                if (!(missingPartsCommonAncestor.Part.coverageAbsWithChildren < minCoverage) && !pawn.health.hediffSet.PartOrAnyAncestorHasDirectlyAddedParts(missingPartsCommonAncestor.Part) && (bodyPartRecord == null || missingPartsCommonAncestor.Part.coverageAbsWithChildren < bodyPartRecord.coverageAbsWithChildren))
                {
                    bodyPartRecord = missingPartsCommonAncestor.Part;
                }
            }
            return bodyPartRecord;
        }

        public static Hediff_Addiction FindAddiction(Pawn pawn)
        {
            List<Hediff> hediffs = pawn.health.hediffSet.hediffs;
            for (int i = 0; i < hediffs.Count; i++)
            {
                if (hediffs[i] is Hediff_Addiction hediff_Addiction && hediff_Addiction.Visible && hediff_Addiction.def.everCurableByItem)
                {
                    return hediff_Addiction;
                }
            }
            return null;
        }
        public static IntVec3 RandomCellAround(this Thing t, int cell)
        {
            CellRect cellRect = t.OccupiedRect();
            CellRect cellRect2 = cellRect.ExpandedBy(cell);
            IntVec3 randomCell;
            do
            {
                randomCell = cellRect2.RandomCell;
            }
            while (cellRect.Contains(randomCell));
            return randomCell;
        }
        public static IntVec3 RandomCellAroundBig(this Thing t)
        {
            System.Random randWarp = new System.Random();
            int randomWarp = randWarp.Next(1, 4);
            int randomWarp2 = randWarp.Next(1, 4);
            CellRect cellRect = t.OccupiedRect();
            CellRect cellRect2 = cellRect.ExpandedBy(randomWarp);
            IntVec3 randomCell;
            do
            {
                randomCell = cellRect2.RandomCell;
            }
            while (cellRect.Contains(randomCell));
            return randomCell;
        }
        public static bool FindBadHediff(Hediff hediff)
        {
            return hediff is Hediff_Injury || hediff is Hediff_MissingPart || hediff is Hediff_Addiction || hediff.def.tendable || hediff.def.makesSickThought || hediff.def.HasComp(typeof(HediffComp_Immunizable)) || hediff.def == HediffDefOf.BloodLoss || hediff.def.isBad;
        }

        public static bool FindUniqueMakaiHediff(Hediff hediff)
        {
            return hediff.def == MakaiTechPsy_DefOf.MakaiTechPsy_DD_LichSoul
                || hediff.def == MakaiTechPsy_DefOf.MakaiTechPsy_DD_MissingSoul
                || hediff.def == MakaiTechPsy_DefOf.MakaiPsy_SF_Counter
                || hediff.def == MakaiTechPsy_DefOf.MakaiTechPsy_DD_LifeLink
                || hediff.def == MakaiTechPsy_DefOf.MakaiTechPsy_DD_CollectSoul
                || hediff.def == MakaiTechPsy_DefOf.MakaiTechPsy_DR_DistortBulletBounce
                || hediff.def == MakaiTechPsy_DefOf.MakaiPsy_SF_Reverse
                || hediff.def == MakaiTechPsy_DefOf.MakaiPsy_SF_Accelerate
                || hediff.def == MakaiTechPsy_DefOf.Destined_Death;
        }
        public static bool HediffFilter(Hediff hediff)
        {
            return hediff is Hediff_Implant || hediff is Hediff_Injury || hediff is Hediff_MissingPart || hediff is Hediff_Addiction || hediff.def.tendable || hediff.def.makesSickThought || hediff.def.HasComp(typeof(HediffComp_Immunizable));
        }
        public static void ThrowObjectAt(IntVec3 thrower, Map map, IntVec3 targetCell, FleckDef fleck)
        {
            if (thrower.ShouldSpawnMotesAt(map))
            {
                float num = Rand.Range(3.8f, 5.6f);
                Vector3 vector = targetCell.ToVector3();
                vector.y = thrower.y;
                FleckCreationData dataStatic = FleckMaker.GetDataStatic(thrower.ToVector3(), map, fleck);
                dataStatic.rotationRate = Rand.Range(-300, 300);
                dataStatic.velocityAngle = (vector - dataStatic.spawnPosition).AngleFlat();
                dataStatic.velocitySpeed = num;
                dataStatic.airTimeLeft = Mathf.RoundToInt((dataStatic.spawnPosition - vector).MagnitudeHorizontal() / num);
                map.flecks.CreateFleck(dataStatic);
            }
        }
        public static Vector2 GetLine(float x1, float x2, float y1, float y2)
        {
            float m = (y2 - y1) / (x2 - x1);
            float b = y1 - (m * x1);
            return new Vector2(m, b);
        }
        public static Vector2 GetLine(Vector3 target1, Vector3 target2)
        {
            return GetLine(target1.x, target2.x, target1.z, target2.z);
        }
        public static float CalcLine(float x, Vector2 line)
        {
            return line.x * x + line.y;
        }
        public static float CalcVertLine(float x, Vector2 line)
        {
            return (x - line.y)/line.x;
        }
        public static void AddAll(Pawn pawn, List<Hediff> hediffs)
        {
            TryAdd();
            void TryAdd()
            {
                hediffs.RemoveAll(delegate (Hediff hediff)
                {
                    if (!pawn.health.hediffSet.PartIsMissing(hediff.Part))
                    {
                        try
                        {
                            pawn.health.AddHediff(hediff, hediff.Part);
                            return true;
                        }
                        catch (Exception arg)
                        {
                            Log.Error($"Error while swapping: {arg}");
                            return false;
                        }
                    }
                    return false;
                });
            }
        }
        public static RollInfo Roll1D20(Pawn pawn,SkillDef skillBonus, RollInfo Rinfo)
        {
            SkillRecord bonus = pawn.skills.GetSkill(skillBonus);
            System.Random rand = new System.Random();
            Rinfo.roll = rand.Next(1, 21);
            int rollBonus = bonus.Level / 5;
            Rinfo.baseRoll = Rinfo.roll;
            int rollBonusLucky = 0;
            int rollBonusUnLucky = 0;
            if (pawn.health.hediffSet.HasHediff(VPE_DefOf.VPE_Lucky))
            {
                rollBonusLucky = 20;
            }
            if (pawn.health.hediffSet.HasHediff(VPE_DefOf.VPE_UnLucky))
            {
                rollBonusUnLucky = -20;
            }
            Rinfo.roll += rollBonus + rollBonusLucky + rollBonusUnLucky;
            Rinfo.cumulativeBonusRoll = rollBonus + rollBonusLucky + rollBonusUnLucky;
            return Rinfo;
        }
        public static Hediff ApplyCustomHediffWithDuration(Pawn pawn, HediffDef hediffDef, float hours, int ticks,StatDef statDef = null)
        {
            float num = hours * 2500f + (float)ticks;
            num *= pawn.GetStatValue(statDef ?? StatDefOf.PsychicSensitivity);
            Hediff hediff = HediffMaker.MakeHediff(hediffDef, pawn);
            if(hediff.TryGetComp<HediffComp_Disappears>() != null)
            {
                hediff.TryGetComp<HediffComp_Disappears>().ticksToDisappear = Mathf.FloorToInt(num);
            }
            return hediff;
        }

        public static BodyPartRecord GetBodyPartFromDef(Pawn pawn,BodyPartDef bodyDef)
        {
            BodyPartRecord br = pawn.RaceProps.body.AllParts.FirstOrFallback(x => x.def == bodyDef);
            return br;
        }
        public static BodyPartRecord GetBodyPartFromHediff(Pawn pawn, HediffDef hediffDef)
        {

            BodyPartRecord br = pawn.health.hediffSet.GetFirstHediffOfDef(hediffDef).Part;
            return br;
        }
        public static MirroredFateInfo GetMirroredFateInfo(MirroredFateInfo minfo,int count,float percent,bool reflectOnlyEnemies,bool reflectOnlyFriendly,bool reflectMelee,bool reflectRanged,bool userTakeDamage)
        {
            minfo.reflectCountLeft = count;
            minfo.reflectPercent = percent;
            minfo.reflectOnlyEnemies = reflectOnlyEnemies;
            minfo.reflectOnlyFriendly = reflectOnlyFriendly;
            minfo.reflectMelee = reflectMelee;
            minfo.reflectRanged = reflectRanged;
            minfo.userTakeDamage = userTakeDamage;
            return minfo;
        }
        //GetNearbyPawnFriendOrFoe and it related. this chunk is all credit to Smartkar for the original
        public static List<Pawn> GetNearbyPawnFriendAndFoe(IntVec3 center, Map map, float radius)
        {
            List<Pawn> list = new List<Pawn>();
            float num = radius * radius;
            foreach (Pawn item in map.mapPawns.AllPawnsSpawned)
            {
                if (item.Spawned && !item.Dead)
                {
                    float num2 = item.Position.DistanceToSquared(center);
                    if (num2 <= num)
                    {
                        list.Add(item);
                    }
                }
            }
            return list;
        }

        public static List<Pawn> GetNearbyPawnFriendOnly(IntVec3 center,Faction faction,Map map,float radius)
        {
            List<Pawn> list = new List<Pawn>();
            float num = radius * radius;
            foreach(Pawn pawn in map.mapPawns.AllPawnsSpawned)
            {
                if(pawn.Spawned && !pawn.Dead && !pawn.Faction.HostileTo(faction))
                {
                    float num2 = pawn.Position.DistanceToSquared(center);
                    if(num2 <= num)
                    {
                        list.Add(pawn);
                    }
                }
            }
            return list;
        }

        public static List<Pawn> GetNearbyPawnFoeOnly(IntVec3 center, Faction faction, Map map, float radius)
        {
            List<Pawn> list = new List<Pawn>();
            float num = radius * radius;
            foreach (Pawn pawn in map.mapPawns.AllPawnsSpawned)
            {
                if (pawn.Spawned && !pawn.Dead && (pawn.Faction.HostileTo(faction) || pawn.HostileTo(faction)))
                {
                    float num2 = pawn.Position.DistanceToSquared(center);
                    if (num2 <= num)
                    {
                        list.Add(pawn);
                    }
                }
            }
            return list;
        }
        public static FleckCreationData GetDataStatic(Vector3 loc, Map map, FleckDef fleckDef, float scale = 1f)
        {
            FleckCreationData result = default(FleckCreationData);
            result.def = fleckDef;
            result.spawnPosition = loc;
            result.scale = scale;
            result.ageTicksOverride = -1;
            return result;
        }
        public static void ThrowFleck(FleckDef fleckDef,Vector3 c, Map map, float size)
        {
            Vector3 vector = c;
            if (vector.ShouldSpawnMotesAt(map))
            {        
                if (vector.InBounds(map))
                {
                    FleckCreationData dataStatic = GetDataStatic(vector, map, fleckDef, size);
                    dataStatic.rotationRate = Rand.Range(-3f, 3f);
                    dataStatic.velocityAngle = Rand.Range(0, 360);
                    dataStatic.velocitySpeed = 0.12f;
                    map.flecks.CreateFleck(dataStatic);
                }                
            }
        }

        public static void ThrowFleckSlightVariationPosition(FleckDef fleckDef, Vector3 c, Map map, float size)
        {
            Vector3 vector = c;
            if (vector.ShouldSpawnMotesAt(map))
            {
                vector += size * new Vector3(Rand.Value - 0.5f, 0f, Rand.Value - 0.5f);
                if (vector.InBounds(map))
                {
                    FleckCreationData dataStatic = GetDataStatic(vector, map, fleckDef, size);
                    dataStatic.rotationRate = Rand.Range(-3f, 3f);
                    dataStatic.velocityAngle = Rand.Range(0, 360);
                    dataStatic.velocitySpeed = 0.12f;
                    map.flecks.CreateFleck(dataStatic);
                }
            }
        }
        public static Soul GetPawnCopy(Thing thing ,Pawn pawn)
        {
            if(thing is Soul soul)
            {
                soul.originalPawn = pawn;
                soul.ownerName = pawn.Name.ToStringFull;
                soul.name = pawn.Name;
                soul.title = pawn.story?.title;
                soul.skills = pawn.skills?.skills;
                soul.childhood = pawn.story?.Childhood;
                soul.adulthood = pawn.story?.Adulthood;
                soul.traits = pawn.story?.traits?.allTraits;
                soul.relations = pawn.relations.DirectRelations ?? new List<DirectPawnRelation>();
                soul.relatedPawns = pawn.relations.RelatedPawns?.ToHashSet() ?? new HashSet<Pawn>();
                foreach (Pawn otherPawn in pawn.relations.RelatedPawns)
                {
                    foreach (PawnRelationDef rel2 in pawn.GetRelations(otherPawn))
                    {
                        if (!soul.relations.Any((DirectPawnRelation r) => r.def == rel2 && r.otherPawn == otherPawn) && !rel2.implied)
                        {
                            soul.relations.Add(new DirectPawnRelation(rel2, otherPawn, 0));
                        }
                    }
                    soul.relatedPawns.Add(otherPawn);
                }
                soul.priorities = new Dictionary<WorkTypeDef, int>();
                if (pawn.workSettings != null && Traverse.Create(pawn.workSettings).Field("priorities").GetValue<DefMap<WorkTypeDef, int>>() != null)
                {
                    foreach (WorkTypeDef allDef in DefDatabase<WorkTypeDef>.AllDefs)
                    {
                        soul.priorities[allDef] = pawn.workSettings.GetPriority(allDef);
                    }
                }
                soul.records = Traverse.Create(pawn.records).Field("records").GetValue<DefMap<RecordDef, float>>();
                if (pawn.Faction != soul.faction)
                {
                    soul.faction = pawn.Faction;
                }
                if (ModsConfig.IdeologyActive)
                {
                    if (pawn.ideo != null && pawn.Ideo != null)
                    {
                        soul.ideo = pawn.Ideo;
                        soul.certainty = pawn.ideo.Certainty;
                        Precept_Role role = pawn.Ideo.GetRole(pawn);
                        if (role is Precept_RoleMulti precept_RoleMulti)
                        {
                            soul.precept_RoleMulti = precept_RoleMulti;
                            soul.precept_RoleSingle = null;
                        }
                        else if (role is Precept_RoleSingle precept_RoleSingle)
                        {
                            soul.precept_RoleMulti = null;
                            soul.precept_RoleSingle = precept_RoleSingle;
                        }
                    }
                    Pawn_StoryTracker story = pawn.story;
                    if (story != null && story.favoriteColor.HasValue)
                    {
                        soul.favColor = pawn.story.favoriteColor.Value;
                    }
                }
                return soul;
            }
            return (Soul)thing;
        }
        public static Pawn applySoulData(Soul soul,Pawn pawn)
        {
            pawn.Name = soul.name;
            pawn.skills.skills.Clear();
            if (soul.skills != null)
            {
                foreach (SkillRecord skill in soul.skills)
                {
                    SkillRecord skillRecord = new SkillRecord(pawn, skill.def);
                    skillRecord.passion = skill.passion;
                    skillRecord.levelInt = skill.levelInt;
                    skillRecord.xpSinceLastLevel = skill.xpSinceLastLevel;
                    skillRecord.xpSinceMidnight = skill.xpSinceMidnight;
                    pawn.skills.skills.Add(skillRecord);
                }
            }
            if (soul.childhood != null)
            {
                pawn.story.Childhood = soul.childhood;
            }
            if (soul.adulthood != null)
            {
                pawn.story.Adulthood = soul.adulthood;
            }
            pawn.story.traits.allTraits = soul.traits;
            foreach (DirectPawnRelation rel in pawn.relations.DirectRelations)
            {
                pawn.relations.DirectRelations.Remove(rel);
            }
            foreach (DirectPawnRelation rel in soul.relations)
            {
                pawn.relations.DirectRelations.Add(rel);
            }
            foreach (Pawn relatedPawn in soul.relatedPawns)
            {
                if(relatedPawn.Name == soul.name)
                {
                    continue;
                }
                if(relatedPawn.needs?.mood?.thoughts?.memories != null)
                {
                    foreach (Thought_Memory memory in relatedPawn.needs.mood.thoughts.memories.Memories)
                    {
                        if (memory?.otherPawn != null)
                        {
                            Pawn otherPawn = memory.otherPawn;
                            if (otherPawn != null && otherPawn.Name?.ToStringFull.Length > 0 && memory.otherPawn.Name.ToStringFull == pawn.Name?.ToStringFull && memory.otherPawn != pawn)
                            {
                                memory.otherPawn = pawn;
                            }
                        }
                    }
                }
                if (relatedPawn.relations != null)
                {
                    foreach (DirectPawnRelation directRelation in relatedPawn.relations.DirectRelations)
                    {
                        Name obj = soul.name;
                        if (obj != null && obj.ToStringFull.Length > 0 && soul.name.ToStringFull == directRelation.otherPawn?.Name?.ToStringFull && directRelation.otherPawn != pawn)
                        {
                            directRelation.otherPawn = pawn;
                            if (pawn.relations.GetDirectRelation(directRelation.def, relatedPawn) == null)
                            {
                                pawn.relations.AddDirectRelation(directRelation.def, relatedPawn);
                            }
                        }
                    }
                }
                if (soul.relations == null)
                {
                    continue;
                }
                foreach (DirectPawnRelation relation in soul.relations)
                {
                    foreach (DirectPawnRelation directRelation2 in relatedPawn.relations.DirectRelations)
                    {
                        if (relation.def != directRelation2.def)
                        {
                            continue;
                        }
                        Pawn otherPawn3 = directRelation2.otherPawn;
                        if (otherPawn3 != null && otherPawn3.Name?.ToStringFull.Length > 0 && directRelation2.otherPawn.Name.ToStringFull == pawn.Name.ToStringFull && directRelation2.otherPawn != pawn)
                        {
                            directRelation2.otherPawn = pawn;
                            if (pawn.relations.GetDirectRelation(directRelation2.def, relatedPawn) == null)
                            {
                                pawn.relations.AddDirectRelation(directRelation2.def, relatedPawn);
                            }
                        }
                    }
                }
                if(soul.originalPawn != null)
                {
                    foreach (Pawn potentiallyRelatedPawn in pawn.relations.PotentiallyRelatedPawns)
                    {
                        if (potentiallyRelatedPawn.needs?.mood?.thoughts?.memories != null)
                        {
                            foreach (Thought_Memory memory3 in potentiallyRelatedPawn.needs.mood.thoughts.memories.Memories)
                            {
                                if (memory3.otherPawn != null)
                                {
                                    Pawn otherPawn4 = memory3.otherPawn;
                                    if (otherPawn4 != null && otherPawn4.Name?.ToStringFull.Length > 0 && memory3.otherPawn.Name.ToStringFull == pawn.Name?.ToStringFull && memory3.otherPawn != pawn)
                                    {
                                        memory3.otherPawn = pawn;
                                    }
                                }
                            }
                        }
                        if (potentiallyRelatedPawn?.relations != null)
                        {
                            foreach (DirectPawnRelation directRelation3 in potentiallyRelatedPawn.relations.DirectRelations)
                            {
                                if (directRelation3.otherPawn == pawn && directRelation3.otherPawn != pawn)
                                {
                                    directRelation3.otherPawn = pawn;
                                }
                            }
                        }
                        if (potentiallyRelatedPawn.needs?.mood?.thoughts == null)
                        {
                            continue;
                        }
                        foreach (Thought_Memory memory4 in potentiallyRelatedPawn.needs.mood.thoughts.memories.Memories)
                        {
                            if (memory4 is Thought_MemorySocial && memory4.otherPawn == soul.originalPawn && memory4.otherPawn != pawn)
                            {
                                memory4.otherPawn = pawn;
                            }
                        }
                    }
                }
            }
            Traverse traverse = Traverse.Create(pawn.workSettings).Field("pawn");
            if (traverse.GetValue() == null)
            {
                traverse.SetValue(pawn);
            }
            Traverse traverse2 = Traverse.Create(pawn.workSettings).Field("priorities");
            if (traverse2.GetValue() == null)
            {
                traverse2.SetValue(new DefMap<WorkTypeDef, int>());
            }
            if (soul.priorities != null)
            {
                foreach (KeyValuePair<WorkTypeDef, int> priority in soul.priorities)
                {
                    pawn.workSettings.SetPriority(priority.Key, priority.Value);
                }
            }
            if (pawn.records == null)
            {
                pawn.records = new Pawn_RecordsTracker(pawn);
            }
            if (soul.records != null)
            {
                Traverse.Create(pawn.records).Field("records").SetValue(soul.records);
            }
            if (soul.faction != null)
            {
                pawn.SetFaction(soul.faction);
            }
            if (ModsConfig.IdeologyActive)
            {
                if (soul.precept_RoleMulti != null)
                {
                    if (soul.precept_RoleMulti.chosenPawns == null)
                    {
                        soul.precept_RoleMulti.chosenPawns = new List<IdeoRoleInstance>();
                    }
                    soul.precept_RoleMulti.chosenPawns.Add(new IdeoRoleInstance(soul.precept_RoleMulti)
                    {
                        pawn = pawn
                    });
                    soul.precept_RoleMulti.FillOrUpdateAbilities();
                }
                if (soul.precept_RoleSingle != null)
                {
                    soul.precept_RoleSingle.chosenPawn = new IdeoRoleInstance(soul.precept_RoleMulti)
                    {
                        pawn = pawn
                    };
                    soul.precept_RoleSingle.FillOrUpdateAbilities();
                }
                if (soul.ideo != null)
                {
                    pawn.ideo.SetIdeo(soul.ideo);
                    Traverse.Create(pawn.ideo).Field("certainty").SetValue(soul.certainty);
                }
                if (soul.favColor.HasValue)
                {
                    pawn.story.favoriteColor = soul.favColor.Value;
                }
            }
            return pawn;
        }

    }
}
