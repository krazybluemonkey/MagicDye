using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using MagicDyeSupplementaries.Content.Items.Dyes;
using System.Collections.Generic;

namespace MagicDyeSupplementaries
{
    class MagicDyeSupplementariesPlayer : ModPlayer
    {
        public override void GetDyeTraderReward(List<int> dyeItemIDsPool)
        {
            dyeItemIDsPool.Add(ModContent.ItemType<SepiaToneDye>());
            dyeItemIDsPool.Add(ModContent.ItemType<GrayScaleDye>());
            dyeItemIDsPool.Add(ModContent.ItemType<PosterizeDye>());
            dyeItemIDsPool.Add(ModContent.ItemType<DeterminationDye>());
        }
    }
}