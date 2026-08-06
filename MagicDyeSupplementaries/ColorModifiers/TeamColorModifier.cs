using Terraria.ModLoader;
using Terraria;
using System;
using Microsoft.Xna.Framework;
using Terraria.ID;
using MagicDyeSupplementaries.Content.Items.Dyes;
using Terraria.DataStructures;

namespace MagicDyeSupplementaries.ColorModifiers
{
    public class Team : ModSystem
    {
        public override void PostSetupContent()
        {
            if (ModLoader.TryGetMod("MagicDye", out Mod mod) && !Main.dedServ)
            {
                mod.Call(new object[4]
                {
                    "AddColorMod",
                    "Team",
                    ItemID.TeamDye,
                    ModifyColor
                });
            }
        }

        public static (Vector3 primaryColor, Vector3 secondaryColor, float saturation, float opacity) ModifyColor(Entity entity, Vector3 primaryColor, Vector3 secondaryColor, float saturation, float opacity)
        {
            Vector3 newVector = new Vector3(0, 0, 0);
            Player player = entity as Player;
            Projectile projectile;
            if (entity is Projectile)
            {
                projectile = entity as Projectile;
                player = Main.player[projectile.owner];
            }
            if (player == null || player.team == 0 && player.team > Main.teamColor.Length)
            {
                return (new Vector3(-100, -100, -100), new Vector3(-100, -100, -100), -100, -100);
            }

            newVector = new Vector3(Main.teamColor[player.team].R, Main.teamColor[player.team].G, Main.teamColor[player.team].B);

            return (newVector, newVector + secondaryColor, saturation, opacity);
        }
    }
}