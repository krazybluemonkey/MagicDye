using MagicDye.UI.Systems;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent.Drawing;
using Terraria.GameContent.ObjectInteractions;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace MagicDye.Content.Tiles
{
    internal class MagicDyeVatTile : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileFrameImportant[Type] = true;
            TileID.Sets.InteractibleByNPCs[Type] = true;
            TileID.Sets.HasOutlines[Type] = true;
            TileID.Sets.DisableSmartCursor[Type] = true;
            TileObjectData.newTile.CopyFrom(TileObjectData.Style3x3);
            TileObjectData.newTile.DrawYOffset = 2;
            TileObjectData.addTile(Type);
            LocalizedText name = CreateMapEntryName();
            AddMapEntry(new Color(144, 148, 144), name);

            DustType = DustID.WoodFurniture;
            AdjTiles = new int[] { TileID.DyeVat };
            AnimationFrameHeight = 54;
        }
        public override void MouseOver(int i, int j)
        {
            Player player = Main.LocalPlayer;
            player.noThrow = 2;
            player.cursorItemIconEnabled = true;
            player.cursorItemIconID = ModContent.ItemType<MagicDyeVatItem>();
        }

        public override bool HasSmartInteract(int i, int j, SmartInteractScanSettings settings)
        {
            return true;
        }

        public override bool RightClick(int i, int j)
        {
            MagicDyeUISystem uiRef = ModContent.GetInstance<MagicDyeUISystem>();
            if (uiRef.MagicDyeInterface.CurrentState != uiRef.MagicDyeUI)
            {
                uiRef.ShowMagicDyeUI();
            }
            return true;
        }

        public override void AnimateTile(ref int frame, ref int frameCounter)
        {
            frame = Main.tileFrame[TileID.DyeVat];
        }
    }

    internal class MagicDyeVatItem : ModItem
    {
        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.DyeVat);
            Item.createTile = ModContent.TileType<MagicDyeVatTile>();
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.DyeVat)
                .AddIngredient(ItemID.GoldBar, 4)
                .AddIngredient(ItemID.Diamond, 1)
                .AddIngredient(ItemID.FallenStar, 10)
                .AddTile(TileID.WorkBenches)
                .Register();
            CreateRecipe()
                .AddIngredient(ItemID.DyeVat)
                .AddIngredient(ItemID.PlatinumBar, 4)
                .AddIngredient(ItemID.Diamond, 1)
                .AddIngredient(ItemID.FallenStar, 10)
                .AddTile(TileID.WorkBenches)
                .Register();
        }
    }
}