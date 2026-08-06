using Terraria.ModLoader;
using Terraria;
using System;
using Microsoft.Xna.Framework;
using Terraria.ID;
using MagicDyeSupplementaries.Content.Items.Dyes;
using Terraria.DataStructures;
using static MagicDyeSupplementaries.Content.Items.Dyes.TimeDye;

namespace MagicDyeSupplementaries.ColorModifiers
{
    public class PlayerEyes : ModSystem
    {
        public override void PostSetupContent()
        {
            if (ModLoader.TryGetMod("MagicDye", out Mod mod) && !Main.dedServ)
            {
                mod.Call(new object[4]
                {
                    "AddColorMod",
                    "Player Eyes",
                    ModContent.ItemType<FamiliarEyesDye>(),
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
            if (player != null)
            {
                if (player.isDisplayDollOrInanimate || player.isHatRackDoll)
                {
                    player = Main.LocalPlayer;
                }
            }
            else
            {
                return (new Vector3(-100, -100, -100), new Vector3(-100, -100, -100), -100, -100);
            }

            Vector3 newVector = player.eyeColor.ToVector3();

            return (newVector, newVector + secondaryColor, saturation, opacity);
        }
    }
}