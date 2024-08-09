using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace PrefixFixer
{
    internal class PrefixFixerGlobalItem : GlobalItem
    {
        internal string prefixMod;
        internal string prefixName;

        public override bool InstancePerEntity => true;

        public override GlobalItem Clone(Item from, Item to)
        {
            PrefixFixerGlobalItem cloned = (PrefixFixerGlobalItem)base.Clone(from, to);
            cloned.prefixMod = prefixMod;
            cloned.prefixName = prefixName;
            return cloned;
        }

        public override void LoadData(Item item, TagCompound tag)
        {
            if (!tag.ContainsKey("ModPrefix_Mod") || !tag.ContainsKey("ModPrefix_Name"))
            {
                prefixMod = null;
                prefixName = null;
                return;
            }
            prefixMod = tag.GetString("ModPrefix_Mod");
            prefixName = tag.GetString("ModPrefix_Name");
            int type = FindPrefix(prefixMod, prefixName);
            if (type == -1)
            {
                item.Prefix(UnloadedPrefix.ActualType);
            }
            else if (item.prefix != type)
            {
                item.Prefix(type);
            }
        }

        private static int FindPrefix(string mod, string name)
        {
            ModPrefix prefix;
            for (var i = PrefixID.Count; i < PrefixLoader.PrefixCount; i++)
            {
                if (i == UnloadedPrefix.ActualType)
                {
                    continue;
                }
                prefix = PrefixLoader.GetPrefix(i);
                if (prefix.Mod.Name == mod && prefix.Name == name)
                {
                    return i;
                }
            }
            return -1;
        }

        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            if (item.prefix != UnloadedPrefix.ActualType)
            {
                return;
            }
            int index = tooltips.FindIndex(tip => tip.Mod == "Terraria" && tip.Name == "SetBonus");
            if (index == -1)
            {
                index = tooltips.FindIndex(tip => tip.Mod == "Terraria" && tip.Name == "Expert");
            }
            if (index == -1)
            {
                index = tooltips.FindIndex(tip => tip.Mod == "Terraria" && tip.Name == "Master");
            }
            if (index == -1)
            {
                index = tooltips.FindIndex(tip => tip.Mod == "Terraria" && tip.Name == "JourneyResearch");
            }
            if (index == -1)
            {
                index = tooltips.FindIndex(tip => tip.Mod == "Terraria" && tip.Name == "ModifiedByMods");
            }
            if (index == -1)
            {
                index = tooltips.FindIndex(tip => tip.Mod == "Terraria" && tip.Name == "BestiaryNotes");
            }
            if (index != -1)
            {
                tooltips.Insert(index, new TooltipLine(Mod, "ModPrefixData1", "Unloaded Prefix") { IsModifier = true });
                index++;
                tooltips.Insert(index, new TooltipLine(Mod, "ModPrefixData2", $"Mod: {prefixMod}") { IsModifier = true });
                index++;
                tooltips.Insert(index, new TooltipLine(Mod, "ModPrefixData3", $"Name: {prefixName}") { IsModifier = true });
            }
            else
            {
                tooltips.Add(new TooltipLine(Mod, "ModPrefixData1", "Unloaded Prefix") { IsModifier = true });
                tooltips.Add(new TooltipLine(Mod, "ModPrefixData2", $"Mod: {prefixMod}") { IsModifier = true });
                tooltips.Add(new TooltipLine(Mod, "ModPrefixData3", $"Name: {prefixName}") { IsModifier = true });
            }
        }

        public override void SaveData(Item item, TagCompound tag)
        {
            if (item.prefix < PrefixID.Count)
            {
                return;
            }
            if (item.prefix != UnloadedPrefix.ActualType)
            {
                ModPrefix prefix = PrefixLoader.GetPrefix(item.prefix);
                if (prefix == null)
                {
                    return;
                }
                prefixMod = prefix.Mod.Name;
                prefixName = prefix.Name;
            }
            tag["ModPrefix_Mod"] = prefixMod;
            tag["ModPrefix_Name"] = prefixName;
        }
    }
}
