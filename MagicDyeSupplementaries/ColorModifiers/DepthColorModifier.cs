using Terraria.ModLoader;
using Terraria;
using System;
using Microsoft.Xna.Framework;
using Terraria.ID;
using MagicDyeSupplementaries.Content.Items.Dyes;
using Terraria.DataStructures;

namespace MagicDyeSupplementaries.ColorModifiers
{
    public class Depth : ModSystem
    {
        public override void PostSetupContent()
        {
            if (ModLoader.TryGetMod("MagicDye", out Mod mod) && !Main.dedServ)
            {
                mod.Call(new object[4]
                {
                    "AddColorMod",
                    "Depth",
                    ModContent.ItemType<DepthDye>(),
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
            if (player == null) return (new Vector3(-100, -100, -100), new Vector3(-100, -100, -100), -100, -100);
            float surface = (float)(Main.worldSurface * 0.45) * 16f;
            float underground = (float)(Main.worldSurface + Main.rockLayer) * 8f;
            float caverns = (float)(Main.rockLayer + (double)Main.maxTilesY) * 8f;
            float underworld = (float)(Main.maxTilesY - 150) * 16f;
            Vector2 center = player.Center;
            if (center.Y < surface)
            {
                float Distance = center.Y / surface;
                float Modifier = 1f - Distance;
                newVector.X = (0.45f * Modifier + 0.1f * Distance);
                newVector.Y = (0.63f * Modifier + 0.85f * Distance);
                newVector.Z = (0.97f * Modifier + 0.36f * Distance);
            }
            else if (center.Y < underground)
            {
                float PreviousLayer = surface;
                float Distance = (center.Y - PreviousLayer) / (underground - PreviousLayer);
                float Modifier = 1f - Distance;
                newVector.X = (0.1f * Modifier + 0.6f * Distance);
                newVector.Y = (0.85f * Modifier + 0.42f * Distance);
                newVector.Z = (0.36f * Modifier + 0.3f * Distance);
            }
            else if (center.Y < caverns)
            {
                float PreviousLayer = underground;
                float Distance = (center.Y - PreviousLayer) / (caverns - PreviousLayer);
                float Modifier = 1f - Distance;
                newVector.X = (0.6f * Modifier + 0.5f * Distance);
                newVector.Y = (0.42f * Modifier + 0.5f * Distance);
                newVector.Z = (0.3f * Modifier + 0.5f * Distance);
            }
            else if (center.Y < underworld)
            {
                float PreviousLayer = caverns;
                float Distance = (center.Y - PreviousLayer) / (underworld - PreviousLayer);
                float Modifier = 1f - Distance;
                newVector.X = (0.5f * Modifier + 1f * Distance);
                newVector.Y = (0.5f * Modifier + 0.2f * Distance);
                newVector.Z = (0.5f * Modifier + 0.05f * Distance);
            }
            else
            {
                newVector.X = 1f;
                newVector.Y = 0.2f;
                newVector.Z = 0.04f;
            }

            return (newVector, newVector + secondaryColor, saturation, opacity);
        }
    }
}