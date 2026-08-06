using Terraria.ModLoader;
using Terraria;
using System;
using Microsoft.Xna.Framework;
using Terraria.ID;
using MagicDyeSupplementaries.Content.Items.Dyes;
using Terraria.DataStructures;
using Microsoft.Xna.Framework.Graphics;

namespace MagicDyeSupplementaries.ColorModifiers
{
    public class Riches : ModSystem
    {
        public override void PostSetupContent()
        {
            if (ModLoader.TryGetMod("MagicDye", out Mod mod) && !Main.dedServ)
            {
                mod.Call(new object[4]
                {
                    "AddColorMod",
                    "Riches",
                    ModContent.ItemType<RichesDye>(),
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
            Vector3 newVector = new Vector3();
            long StackValue = 0L;
            for (int i = 0; i < 54; i++)
            {
                if (player.inventory[i].type == ItemID.CopperCoin)
                {
                    StackValue += player.inventory[i].stack;
                }
                if (player.inventory[i].type == ItemID.SilverCoin)
                {
                    StackValue += (long)player.inventory[i].stack * 100L;
                }
                if (player.inventory[i].type == ItemID.GoldCoin)
                {
                    StackValue += (long)player.inventory[i].stack * 10000L;
                }
                if (player.inventory[i].type == ItemID.PlatinumCoin)
                {
                    StackValue += (long)player.inventory[i].stack * 1000000L;
                }
            }
            if (StackValue < 0 || StackValue > 999999999)
            {
                StackValue = 999999999L;
            }
            float FiveGold = Item.buyPrice(0, 5);
            float FiftyGold = Item.buyPrice(0, 50);
            float TwoPlatinum = Item.buyPrice(2);
            Vector3 tempColor1 = new Color(226, 118, 76).ToVector3();
            Vector3 tempColor2 = new Color(174, 194, 196).ToVector3();
            Vector3 tempColor3 = new Color(204, 181, 72).ToVector3();
            Vector3 tempColor4 = new Color(161, 172, 173).ToVector3();

            if ((float)StackValue < FiveGold)
            {
                float Difference = (float)StackValue / FiveGold;
                float Modifier = 1f - Difference;
                newVector.X = (tempColor1.X * Modifier + tempColor2.X * Difference);
                newVector.Y = (tempColor1.Y * Modifier + tempColor2.Y * Difference);
                newVector.Z = (tempColor1.Z * Modifier + tempColor2.Z * Difference);
            }
            else if ((float)StackValue < FiftyGold)
            {
                float PreviousValue = FiveGold;
                float Difference = ((float)StackValue - PreviousValue) / (FiftyGold - PreviousValue);
                float Modifier = 1f - Difference;
                newVector.X = (tempColor2.X * Modifier + tempColor3.X * Difference);
                newVector.Y = (tempColor2.Y * Modifier + tempColor3.Y * Difference);
                newVector.Z = (tempColor2.Z * Modifier + tempColor3.Z * Difference);
            }
            else if ((float)StackValue < TwoPlatinum)
            {

                float PreviousValue = FiftyGold;
                float Difference = ((float)StackValue - PreviousValue) / (TwoPlatinum - PreviousValue);
                float Modifier = 1f - Difference;
                newVector.X = (tempColor3.X * Modifier + tempColor4.X * Difference);
                newVector.Y = (tempColor3.Y * Modifier + tempColor4.Y * Difference);
                newVector.Z = (tempColor3.Z * Modifier + tempColor4.Z * Difference);
            }
            else
            {
                newVector = tempColor4;
            }

            return (newVector, newVector + secondaryColor, saturation, opacity);
        }
    }
}