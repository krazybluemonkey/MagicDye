using Terraria.ModLoader;
using Terraria;
using System;
using Microsoft.Xna.Framework;
using Terraria.ID;
using MagicDyeSupplementaries.Content.Items.Dyes;
using Terraria.DataStructures;

namespace MagicDyeSupplementaries.ColorModifiers
{
    public class DeterminedHeart : ModSystem
    {
        public override void PostSetupContent()
        {
            if (ModLoader.TryGetMod("MagicDye", out Mod mod) && !Main.dedServ)
            {
                mod.Call(new object[4]
                {
                    "AddColorMod",
                    "Shining Heart",
                    ModContent.ItemType<ShiningHeartDye>(),
                    ModifyColor
                });
            }
        }

        public static (Vector3 primaryColor, Vector3 secondaryColor, float saturation, float opacity) ModifyColor(Entity entity, Vector3 primaryColor, Vector3 secondaryColor, float saturation, float opacity)
        {
            Vector3 newVector = new Vector3(1f - (Main.demonTorch * 0.1f), 0.3f - (Main.demonTorch * 0.2f), 0.5f + (Main.demonTorch * 0.2f));

            return (newVector, newVector + secondaryColor, saturation, opacity);
        }
    }
}