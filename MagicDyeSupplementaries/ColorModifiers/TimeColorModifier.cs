using Terraria.ModLoader;
using Terraria;
using System;
using Microsoft.Xna.Framework;
using Terraria.ID;
using MagicDyeSupplementaries.Content.Items.Dyes;
using Terraria.DataStructures;

namespace MagicDyeSupplementaries.ColorModifiers
{
    public class Time : ModSystem
    {
        public override void PostSetupContent()
        {
            if (ModLoader.TryGetMod("MagicDye", out Mod mod) && !Main.dedServ)
            {
                mod.Call(new object[4]
                {
                    "AddColorMod",
                    "Time",
                    ModContent.ItemType<TimeDye>(),
                    ModifyColor
                });
            }
        }

        public static (Vector3 primaryColor, Vector3 secondaryColor, float saturation, float opacity) ModifyColor(Entity entity, Vector3 primaryColor, Vector3 secondaryColor, float saturation, float opacity)
        {
            Vector3 MorningVector = new Vector3(0.003f, 0.55f, 1f);
            Vector3 NoonVector = new Vector3(1f, 1f, 0f);
            Vector3 SunsetVector = new Vector3(0.83f, 0.17f, 0.5f);
            Vector3 NightVector = new Vector3(0.27f, 0.17f, 0.47f);
            Vector3 FinalColor;
            if (Main.dayTime)
            {
                if (Main.time < 27000.0)
                {
                    float AverageColor = (float)(Main.time / 27000.0);
                    float FloorColor = 1f - AverageColor;
                    FinalColor.X = (MorningVector.X * FloorColor + NoonVector.X * AverageColor);
                    FinalColor.Y = (MorningVector.Y * FloorColor + NoonVector.Y * AverageColor);
                    FinalColor.Z = (MorningVector.Z * FloorColor + NoonVector.Z * AverageColor);
                }
                else
                {
                    float NoonValue = 27000f;
                    float AverageColor = (float)((Main.time - (double)NoonValue) / (54000.0 - (double)NoonValue));
                    float FloorColor = 1f - AverageColor;
                    FinalColor.X = (NoonVector.X * FloorColor + SunsetVector.X * AverageColor);
                    FinalColor.Y = (NoonVector.Y * FloorColor + SunsetVector.Y * AverageColor);
                    FinalColor.Z = (NoonVector.Z * FloorColor + SunsetVector.Z * AverageColor);
                }
            }
            else if (Main.time < 16200.0)
            {
                float AverageColor = (float)(Main.time / 16200.0);
                float FloorColor = 1f - AverageColor;
                FinalColor.X = (SunsetVector.X * FloorColor + NightVector.X * AverageColor);
                FinalColor.Y = (SunsetVector.Y * FloorColor + NightVector.Y * AverageColor);
                FinalColor.Z = (SunsetVector.Z * FloorColor + NightVector.Z * AverageColor);
            }
            else
            {
                float NightValue = 16200f;
                float AverageColor = (float)((Main.time - (double)NightValue) / (32400.0 - (double)NightValue));
                float FloorColor = 1f - AverageColor;
                FinalColor.X = (NightVector.X * FloorColor + MorningVector.X * AverageColor);
                FinalColor.Y = (NightVector.Y * FloorColor + MorningVector.Y * AverageColor);
                FinalColor.Z = (NightVector.Z * FloorColor + MorningVector.Z * AverageColor);
            }
            return (FinalColor, FinalColor + secondaryColor, saturation, opacity);
        }
    }
}