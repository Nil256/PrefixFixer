using Terraria;
using Terraria.ModLoader;

namespace PrefixFixer
{
    internal class UnloadedPrefix : ModPrefix
    {
        internal static int ActualType => ModContent.PrefixType<UnloadedPrefix>();

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Unloaded-Prefixed");
        }

        public override float RollChance(Item item)
        {
            return 0f;
        }

        public override bool CanRoll(Item item)
        {
            return false;
        }
    }
}
