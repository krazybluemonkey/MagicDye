using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Graphics.Shaders;
using Terraria.GameContent.Creative;
using MagicDye.Common.Shaders;
using Terraria.DataStructures;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria.Localization;
using MagicDye.Content.Tiles;

namespace MagicDye.Content.Items.Dyes
{
    public abstract class MagicDyeBase : ModItem
    {
        public override string Texture => "MagicDye/Content/Items/Dyes/MagicDye_Base";

        public virtual int DyeSlot => 0;


        public override void SetStaticDefaults()
        {
            if (!Main.dedServ)
            {
                GameShaders.Armor.BindShader(
                    Item.type,
                    new MagicDyeArmorShaderData(Main.PixelShaderRef, "ColorOnly")
                ).UseDyeSlot(DyeSlot);
            }

            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 3;
        }

        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.maxStack = 9999;
            Item.value = Item.sellPrice(0, 0, 20, 0);
            Item.rare = ItemRarityID.Blue;
            Item.dye = Item.dye;
        }

        public override void PostDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
        {
            var previousState = MagicDye.GetSpriteBatchInformation(spriteBatch);
            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, previousState.samplerState, previousState.depthStencilState, previousState.rasterizerState, previousState.customEffect, previousState.transformMatrix);
            Asset<Texture2D> texture = ModContent.Request<Texture2D>("MagicDye/Content/Items/Dyes/MagicDye_Overlay"); // this is violet because "the color of magic is octarine". but really i needed a colored overlay for shader purposes
            DrawData data = new DrawData
            {
                texture = (Texture2D)texture,
                position = position,
                color = drawColor,
                rotation = 0f,
                origin = origin,
                scale = new Vector2(scale),
                effect = SpriteEffects.None,
                shader = Item.dye
            };
            GameShaders.Armor.Apply(GameShaders.Armor.GetShaderIdFromItemId(Item.type), Main.LocalPlayer, data);

            data.Draw(spriteBatch);
            spriteBatch.End();
            spriteBatch.Begin(previousState.sortMode, previousState.blendState, previousState.samplerState, previousState.depthStencilState, previousState.rasterizerState, previousState.customEffect, previousState.transformMatrix);
        }

        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
        {
            Main.GetItemDrawFrame(Item.type, out var itemTexture, out var itemFrame);
            Vector2 origin = itemFrame.Size() / 2f;
            Vector2 drawPosition = Item.Bottom - Main.screenPosition - new Vector2(0, origin.Y);

            var previousState = MagicDye.GetSpriteBatchInformation(spriteBatch);
            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, previousState.samplerState, previousState.depthStencilState, previousState.rasterizerState, previousState.customEffect, previousState.transformMatrix);
            Asset<Texture2D> texture = ModContent.Request<Texture2D>("MagicDye/Content/Items/Dyes/MagicDye_Overlay");
            DrawData data = new DrawData
            {
                texture = (Texture2D)texture,
                position = drawPosition,
                color = lightColor,
                rotation = rotation,
                origin = origin,
                scale = new Vector2(scale),
                effect = SpriteEffects.None,
                shader = Item.dye
            };
            GameShaders.Armor.Apply(GameShaders.Armor.GetShaderIdFromItemId(Item.type), Main.LocalPlayer, data);

            data.Draw(spriteBatch);
            spriteBatch.End();
            spriteBatch.Begin(previousState.sortMode, previousState.blendState, previousState.samplerState, previousState.depthStencilState, previousState.rasterizerState, previousState.customEffect, previousState.transformMatrix);
        }
        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            string tooltipPrimaryColor = MagicDye.ClampColor(Main.LocalPlayer.GetModPlayer<MagicDyePlayer>().MagicDyePrimaryColors[DyeSlot]).Hex3();
            string tooltipSecondaryColor = MagicDye.ClampColor(Main.LocalPlayer.GetModPlayer<MagicDyePlayer>().MagicDyeSecondaryColors[DyeSlot]).Hex3();
            TooltipLine primaryColorTooltip = new TooltipLine(Mod, "Magic Dye: Primary Color", $"{Language.GetText("Mods.MagicDye.UI.PrimaryColor").Value}: [c/{tooltipPrimaryColor}:{Main.LocalPlayer.GetModPlayer<MagicDyePlayer>().MagicDyePrimaryColors[DyeSlot].X} {Main.LocalPlayer.GetModPlayer<MagicDyePlayer>().MagicDyePrimaryColors[DyeSlot].Y} {Main.LocalPlayer.GetModPlayer<MagicDyePlayer>().MagicDyePrimaryColors[DyeSlot].Z}]");
            tooltips.Add(primaryColorTooltip);
            TooltipLine secondaryColorTooltip = new TooltipLine(Mod, "Magic Dye: Secondary Color", $"{Language.GetText("Mods.MagicDye.UI.SecondaryColor").Value}: [c/{tooltipSecondaryColor}:{Main.LocalPlayer.GetModPlayer<MagicDyePlayer>().MagicDyeSecondaryColors[DyeSlot].X} {Main.LocalPlayer.GetModPlayer<MagicDyePlayer>().MagicDyeSecondaryColors[DyeSlot].Y} {Main.LocalPlayer.GetModPlayer<MagicDyePlayer>().MagicDyeSecondaryColors[DyeSlot].Z}]");
            tooltips.Add(secondaryColorTooltip);
            TooltipLine passTooltip = new TooltipLine(Mod, "Magic Dye: Pass", $"{Language.GetText("Mods.MagicDye.UI.Pass").Value}: {Main.LocalPlayer.GetModPlayer<MagicDyePlayer>().MagicDyePasses[DyeSlot]}");
            tooltips.Add(passTooltip);
            TooltipLine saturationTooltip = new TooltipLine(Mod, "Magic Dye: Saturation", $"{Language.GetText("Mods.MagicDye.UI.Saturation").Value}: {Main.LocalPlayer.GetModPlayer<MagicDyePlayer>().MagicDyeSaturation[DyeSlot]}");
            tooltips.Add(saturationTooltip);
            TooltipLine opacityTooltip = new TooltipLine(Mod, "Magic Dye: Opacity", $"{Language.GetText("Mods.MagicDye.UI.Opacity").Value}: {Main.LocalPlayer.GetModPlayer<MagicDyePlayer>().MagicDyeOpacity[DyeSlot]}");
            tooltips.Add(opacityTooltip);

            if (Main.LocalPlayer.GetModPlayer<MagicDyePlayer>().MagicDyeColorMod1[DyeSlot] != "" && Main.LocalPlayer.GetModPlayer<MagicDyePlayer>().MagicDyeColorMod1[DyeSlot] != "None")
            {
                TooltipLine colorMod1tooltip = new TooltipLine(Mod, "Magic Dye: Color Modifier 1", $"{Language.GetText("Mods.MagicDye.UI.ColorModOne").Value}: {Main.LocalPlayer.GetModPlayer<MagicDyePlayer>().MagicDyeColorMod1[DyeSlot]}");
                tooltips.Add(colorMod1tooltip);
            }
            if (Main.LocalPlayer.GetModPlayer<MagicDyePlayer>().MagicDyeColorMod2[DyeSlot] != "" && Main.LocalPlayer.GetModPlayer<MagicDyePlayer>().MagicDyeColorMod2[DyeSlot] != "None")
            {
                TooltipLine colorMod2tooltip = new TooltipLine(Mod, "Magic Dye: Color Modifier 2", $"{Language.GetText("Mods.MagicDye.UI.ColorModTwo").Value}: {Main.LocalPlayer.GetModPlayer<MagicDyePlayer>().MagicDyeColorMod2[DyeSlot]}");
                tooltips.Add(colorMod2tooltip);
            }
        }

    }

    public class MagicDyeSlot1 : MagicDyeBase
    {
        public override int DyeSlot => 0;
        public override void AddRecipes()
        {
            CreateRecipe(2)
                .AddIngredient(ItemID.BottledWater, 1)
                .AddIngredient(ItemID.FallenStar, 1)
                .AddTile(ModContent.TileType<MagicDyeVatTile>())
                .Register();
        }
    }

    public class MagicDyeSlot2 : MagicDyeBase
    {
        public override int DyeSlot => 1;
    }

    public class MagicDyeSlot3 : MagicDyeBase
    {
        public override int DyeSlot => 2;
    }

    public class MagicDyeSlot4 : MagicDyeBase
    {
        public override int DyeSlot => 3;
    }

    public class MagicDyeSlot5 : MagicDyeBase
    {
        public override int DyeSlot => 4;
    }

    public class MagicDyeSlot6 : MagicDyeBase
    {
        public override int DyeSlot => 5;
    }

    public class MagicDyeSlot7 : MagicDyeBase
    {
        public override int DyeSlot => 6;
    }

    public class MagicDyeSlot8 : MagicDyeBase
    {
        public override int DyeSlot => 7;
    }

    public class MagicDyeSlot9 : MagicDyeBase
    {
        public override int DyeSlot => 8;
    }

    public class MagicDyeSlot10 : MagicDyeBase
    {
        public override int DyeSlot => 9;
    }

    public class MagicDyeSlot11 : MagicDyeBase
    {
        public override int DyeSlot => 10;
    }

    public class MagicDyeSlot12 : MagicDyeBase
    {
        public override int DyeSlot => 11;
    }

    public class MagicDyeSlot13 : MagicDyeBase
    {
        public override int DyeSlot => 12;
    }

    public class MagicDyeSlot14 : MagicDyeBase
    {
        public override int DyeSlot => 13;
    }

    public class MagicDyeSlot15 : MagicDyeBase
    {
        public override int DyeSlot => 14;
    }

    public class MagicDyeSlot16 : MagicDyeBase
    {
        public override int DyeSlot => 15;
    }

    public class MagicDyeSlot17 : MagicDyeBase
    {
        public override int DyeSlot => 16;
    }

    public class MagicDyeSlot18 : MagicDyeBase
    {
        public override int DyeSlot => 17;
    }

    public class MagicDyeSlot19 : MagicDyeBase
    {
        public override int DyeSlot => 18;
    }

    public class MagicDyeSlot20 : MagicDyeBase
    {
        public override int DyeSlot => 19;
    }

    public class MagicDyeSlot21 : MagicDyeBase
    {
        public override int DyeSlot => 20;
    }

    public class MagicDyeSlot22 : MagicDyeBase
    {
        public override int DyeSlot => 21;
    }

    public class MagicDyeSlot23 : MagicDyeBase
    {
        public override int DyeSlot => 22;
    }

    public class MagicDyeSlot24 : MagicDyeBase
    {
        public override int DyeSlot => 23;
    }

    public class MagicDyeSlot25 : MagicDyeBase
    {
        public override int DyeSlot => 24;
    }

    public class MagicDyeSlot26 : MagicDyeBase
    {
        public override int DyeSlot => 25;
    }

    public class MagicDyeSlot27 : MagicDyeBase
    {
        public override int DyeSlot => 26;
    }

    public class MagicDyeSlot28 : MagicDyeBase
    {
        public override int DyeSlot => 27;
    }

    public class MagicDyeSlot29 : MagicDyeBase
    {
        public override int DyeSlot => 28;
    }

    public class MagicDyeSlot30 : MagicDyeBase
    {
        public override int DyeSlot => 29;
    }

    public class MagicDyeSlot31 : MagicDyeBase
    {
        public override int DyeSlot => 30;
    }

    public class MagicDyeSlot32 : MagicDyeBase
    {
        public override int DyeSlot => 31;
    }
}