using Terraria.ModLoader;
using Terraria;
using System;
using Microsoft.Xna.Framework;
using Terraria.ID;
using MagicDyeSupplementaries.Content.Items.Dyes;
using Terraria.DataStructures;

namespace MagicDyeSupplementaries.ColorModifiers
{
    public class Life : ModSystem
    {
        public override void PostSetupContent()
        {
            if (ModLoader.TryGetMod("MagicDye", out Mod mod) && !Main.dedServ)
            {
                mod.Call(new object[4]
                {
                    "AddColorMod",
                    "Life",
                    ModContent.ItemType<LifeDye>(),
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
            float playerLife;
            float playerMaxLife;
            if (player != null && !player.isDisplayDollOrInanimate && !player.isHatRackDoll)
            {
                playerLife = (float)player.statLife;
                playerMaxLife = (float)player.statLifeMax2;
            }
            else
            {
                playerLife = 1f;
                playerMaxLife = 1f;
            }

            Vector3 newVector = Vector3.Lerp(new Vector3(0.4f, 0.4f, 0.4f), primaryColor, playerLife / playerMaxLife);

            return (newVector, newVector + secondaryColor, saturation, opacity);
        }
    }
}