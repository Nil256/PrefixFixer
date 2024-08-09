using System.Reflection;
using Terraria.Localization;
using Terraria.ModLoader;

namespace PrefixFixer
{
	public class PrefixFixer : Mod
	{
    }

    internal static class ExtensionMethods
    {
        public static void SetDefault(this LocalizedText self, string value)
        {
            self.GetType().GetField("_value", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(self, value);
        }
    }
}