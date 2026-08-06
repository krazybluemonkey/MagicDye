using Terraria.ModLoader;
using Terraria;
using System;
using Microsoft.Xna.Framework;
using Terraria.ID;
using MagicDyeSupplementaries.Content.Items.Dyes;
using Terraria.DataStructures;
using Terraria.Map;

namespace MagicDyeSupplementaries.ColorModifiers
{
    public class Speed : ModSystem
    {
        public override void PostSetupContent()
        {
            if (ModLoader.TryGetMod("MagicDye", out Mod mod) && !Main.dedServ)
            {
                mod.Call(new object[4]
                {
                    "AddColorMod",
                    "Speed",
                    ModContent.ItemType<SpeedDye>(),
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
            Vector2 EnitiyVelocity;

            if (player != null)
            {
                if (!player.isDisplayDollOrInanimate && !player.isHatRackDoll)
                {
                    EnitiyVelocity = player.velocity;
                }
                else
                {
                    EnitiyVelocity = Main.LocalPlayer.velocity;
                }
            }
            else
            {
                return (new Vector3(-100, -100, -100), new Vector3(-100, -100, -100), -100, -100);
            }
            Vector3 InputColor1 = primaryColor;
            Vector3 InputColor2 = secondaryColor;
            Vector3 FinalColor;
            float AbsoluteVelocity = Math.Abs(EnitiyVelocity.X) + Math.Abs(EnitiyVelocity.Y);
            float VelocityMax = 10f;
            //if (AbsoluteVelocity > VelocityMax) this caps it but i figured it'd be fun to make it able to become intense at higher speeds :) 
            //    AbsoluteVelocity = VelocityMax;
            //}
            float CurrentColor = AbsoluteVelocity / VelocityMax;
            float ColorFloor = 1f - CurrentColor;

            FinalColor.X = (InputColor2.X * CurrentColor + InputColor1.X * ColorFloor);
            FinalColor.Y = (InputColor2.Y * CurrentColor + InputColor1.Y * ColorFloor);
            FinalColor.Z = (InputColor2.Z * CurrentColor + InputColor1.Z * ColorFloor);

            return (FinalColor, FinalColor, saturation, opacity);
        }
    }
}