using Verse;
using System.Collections.Generic;
using RimWorld;
using System.Text;
using UnityEngine;

namespace MakaiTechPsycast
{
    public class Soul : ThingWithComps
    {
		public string ownerName;
		public Name name;
		public string title;
		public Pawn originalPawn;
		public List<SkillRecord> skills;
		public List<Trait> traits;
		public BackstoryDef childhood;
		public BackstoryDef adulthood;
		public List<DirectPawnRelation> relations;
		public HashSet<Pawn> relatedPawns;
		public Dictionary<WorkTypeDef, int> priorities;
		public DefMap<RecordDef, float> records = new DefMap<RecordDef, float>();
		public Faction faction;
		public Ideo ideo;
		public Color? favColor;
		public float certainty;
		public Precept_RoleMulti precept_RoleMulti;
		public Precept_RoleSingle precept_RoleSingle;

		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look(ref ownerName, "ownerName");
			Scribe_Deep.Look(ref name, "name");
			Scribe_Values.Look(ref title, "title");
			Scribe_References.Look(ref originalPawn, "originalPawn", saveDestroyedThings: true);
			Scribe_Collections.Look(ref skills, "skills", LookMode.Undefined);
			Scribe_Collections.Look(ref traits,"traits",LookMode.Undefined);
			Scribe_Defs.Look(ref childhood, "childhood");
			Scribe_Defs.Look(ref adulthood, "adulthood");
			Scribe_Collections.Look(ref relations,"relation",LookMode.Undefined);
			Scribe_Collections.Look(ref priorities, "priorities");
			Scribe_Deep.Look(ref records, "records");
			Scribe_References.Look(ref faction, "faction", saveDestroyedThings: true);
			if (ModsConfig.IdeologyActive)
            {
				Scribe_References.Look(ref ideo, "ideo", saveDestroyedThings: true);
				Scribe_Values.Look(ref favColor, "favoriteColor");
				Scribe_Values.Look(ref certainty, "certainty", 0f);
				Scribe_References.Look(ref precept_RoleSingle, "precept_RoleSingle");
				Scribe_References.Look(ref precept_RoleMulti, "precept_RoleMulti");
			}
		}
		public override string LabelNoCount
		{
			get
			{
				if (ownerName == null)
				{
					return base.LabelNoCount;
				}
				return ownerName + "'s" + " " + "Lich Soul";
			}
		}
		public override string Label
		{
			get
			{
				if (ownerName == null)
				{
					return base.Label;
				}
				return ownerName +"'s" + " " + "Lich Soul";
			}
		}
		public override string DescriptionFlavor => CustomDescription(base.DescriptionFlavor);

		public override string DescriptionDetailed => CustomDescription(base.DescriptionDetailed);
		public string CustomDescription(string Desc)
		{
			StringBuilder builder = new StringBuilder(Desc);
			builder.AppendLine();
			builder.AppendLine();
			if (ownerName != null)
			{
				builder.AppendLine("Name: " + ownerName);
				if (title != null)
				{
					builder.Append(" " + "Title: " + title);
				}
			}
			if (faction != null)
			{
				builder.AppendLine("Faction: " + faction);
			}
			if (childhood != null)
			{
				builder.AppendLine("Childhood: " + childhood.title.CapitalizeFirst());
			}
			if (adulthood != null)
			{
				builder.AppendLine("Adulthood: " + adulthood.title.CapitalizeFirst());
			}
			if (ModsConfig.IdeologyActive)
			{
				builder.AppendLine("Ideology: " + ideo.name);
			}
			builder.AppendLine();
			if (skills != null && skills.Count > 0)
			{
				builder.AppendLine("Skill");
				foreach (SkillRecord item in skills)
				{
					if(item.passion >= Passion.Major)
                    {
						builder.AppendLine("<color=#FF8000>" + item.def.label.CapitalizeFirst() + "</color>" + "**" + ": " + item.levelInt + " " + "(" + item.xpSinceLastLevel + "xp" + ")");
					}
					if(item.passion == Passion.Minor)
                    {
						builder.AppendLine("<color=#EDEA72>" + item.def.label.CapitalizeFirst() + "</color>" + "*" + ": " + item.levelInt + " " + "(" + item.xpSinceLastLevel + "xp" + ")");
					}
					if(item.passion == Passion.None)
                    {
						builder.AppendLine("<color=#FFFFFF>" + item.def.label.CapitalizeFirst() + "</color>" + ": " + item.levelInt + " " + "(" + item.xpSinceLastLevel + "xp" + ")");
					}
				}
			}
			builder.AppendLine();
			if (traits != null && traits.Count > 0)
			{
				builder.Append("Trait: ");
				foreach (Trait item in traits)
				{
					builder.Append(item.LabelCap + ",");
				}
			}
			return builder.ToString().TrimEndNewlines();
		}
	}
}
