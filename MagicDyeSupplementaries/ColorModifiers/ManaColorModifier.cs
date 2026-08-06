using Terraria.ModLoader;
using Terraria;
using System;
using Microsoft.Xna.Framework;
using Terraria.ID;
using MagicDyeSupplementaries.Content.Items.Dyes;
using Terraria.DataStructures;

namespace MagicDyeSupplementaries.ColorModifiers
{
    public class Mana : ModSystem
    {
        public override void PostSetupContent()
        {
            if (ModLoader.TryGetMod("MagicDye", out Mod mod) && !Main.dedServ)
            {
                mod.Call(new object[4]
                {
                    "AddColorMod",
                    "Mana",
                    ModContent.ItemType<ManaDye>(),
                    ModifyColor
                });
            }
        }

        public static (Vector3 primaryColor, Vector3 secondaryColor, float saturation, float opacity) ModifyColor(Entity entity, Vector3 primaryColor, Vector3 secondaryColor, float saturation, float opacity)
        {

            Player player = entity as Player;
            Projectile projectile;
            if (entity is Projectile)
            {
                projectile = entity as Projectile;
                player = Main.player[projectile.owner];
            }
            float playerMana;
            float playerMaxMana;
            if (player != null && !player.isDisplayDollOrInanimate && !player.isHatRackDoll)
            {
                playerMana = (float)player.statMana;
                playerMaxMana = (float)player.statManaMax2;
            }
            else
            {
                playerMana = 1f;
                playerMaxMana = 1f;
            }

            Vector3 newVector = Vector3.Lerp(new Vector3(1f, 1f, 1f), primaryColor, playerMana / playerMaxMana);

            return (newVector, newVector + secondaryColor, saturation, opacity);
        }
    }
}