using Terraria.ModLoader;
using Terraria;
using System;
using Microsoft.Xna.Framework;
using Terraria.ID;
using MagicDyeSupplementaries.Content.Items.Dyes;

namespace MagicDyeSupplementaries.ColorModifiers
{
    public class ShiftingRainbowColorModifier : ModSystem
    {
        public override void PostSetupContent()
        {
            if (ModLoader.TryGetMod("MagicDye", out Mod mod) && !Main.dedServ)
            {
                mod.Call(new object[4]
                {
                    "AddColorMod",
                    "Shifting Rainbow",
                    ModContent.ItemType<ShiftingRainbowDye>(),
                    ModifyColor
                });
            }
        }

        public static (Vector3 primaryColor, Vector3 secondaryColor, float saturation, float opacity) ModifyColor(Entity entity, Vector3 primaryColor, Vector3 secondaryColor, float saturation, float opacity)
        {
            Vector3 rainbowVector = Main.DiscoColor.ToVector3();

            return (rainbowVector + primaryColor, rainbowVector + secondaryColor, saturation, opacity);
        }
    }
}