using Terraria;
using Terraria.ID;
using ReLogic.Content;
using Terraria.ModLoader;
using Terraria.Graphics.Shaders;
using Terraria.GameContent.Creative;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;

namespace MagicDyeSupplementaries.Content.Items.Dyes
{
    public class BiomeDye : ModItem
    {
        public override void SetStaticDefaults()
        {
            if (!Main.dedServ)
            {
                GameShaders.Armor.BindShader(
                    Item.type,
                    new SupplementariesShaderData(Main.PixelShaderRef, "ArmorColored")
                ).UseNewSaturation(1.2f).UseType(1);
            }

            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 3;
        }
        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.maxStack = 9999;
            Item.value = Item.sellPrice(0, 0, 75, 0);
            Item.rare = ItemRarityID.Green;
            Item.dye = Item.dye;
        }
        public override void AddRecipes()
        {
            CreateRecipe(2)
                .AddIngredient(ItemID.BiomeHairDye)
                .AddIngredient(ItemID.BottledWater)
                .AddTile(TileID.DyeVat)
                .Register();
        }
    }

    public class CrystalShineDye : ModItem
    {
        public override void SetStaticDefaults()
        {
            if (!Main.dedServ)
            {
                GameShaders.Armor.BindShader(
                    Item.type,
                    new SupplementariesShaderData(Main.PixelShaderRef, "ArmorColored")
                ).UseNewSaturation(1.2f).UseType(2);
            }

            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 3;
        }
        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.maxStack = 9999;
            Item.value = Item.sellPrice(0, 0, 75, 0);
            Item.rare = ItemRarityID.Green;
            Item.dye = Item.dye;
        }
        public override void AddRecipes()
        {
            CreateRecipe(2)
                    .AddIngredient(ItemID.CrystalShard, 5)
                    .AddIngredient(ItemID.BottledWater)
                    .AddTile(TileID.DyeVat)
                    .Register();
        }
    }

    public class DemonFireDye : ModItem
    {
        public override void SetStaticDefaults()
        {
            if (!Main.dedServ)
            {
                GameShaders.Armor.BindShader(
                    Item.type,
                    new SupplementariesShaderData(Main.PixelShaderRef, "ArmorColored")
                ).UseNewSaturation(1.2f).UseType(3);
            }

            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 3;
        }
        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.maxStack = 9999;
            Item.value = Item.sellPrice(0, 0, 75, 0);
            Item.rare = ItemRarityID.Green;
            Item.dye = Item.dye;
        }
        public override void AddRecipes()
        {
            CreateRecipe(2)
                    .AddIngredient(ItemID.Obsidian, 5)
                    .AddIngredient(ItemID.BottledWater)
                    .AddTile(TileID.DyeVat)
                    .Register();
        }
    }

    public class DepthDye : ModItem
    {
        public override void SetStaticDefaults()
        {
            if (!Main.dedServ)
            {
                GameShaders.Armor.BindShader(
                    Item.type,
                    new SupplementariesShaderData(Main.PixelShaderRef, "ArmorColored")
                ).UseNewSaturation(1.2f).UseType(4);
            }

            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 3;
        }
        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.maxStack = 9999;
            Item.value = Item.sellPrice(0, 0, 75, 0);
            Item.rare = ItemRarityID.Green;
            Item.dye = Item.dye;
        }
        public override void AddRecipes()
        {

            CreateRecipe(2)
                .AddIngredient(ItemID.DepthHairDye)
                .AddIngredient(ItemID.BottledWater)
                .AddTile(TileID.DyeVat)
                .Register();
        }
    }

    public class LifeDye : ModItem
    {
        public override void SetStaticDefaults()
        {
            if (!Main.dedServ)
            {
                GameShaders.Armor.BindShader(
                    Item.type,
                    new SupplementariesShaderData(Main.PixelShaderRef, "ArmorColored")
                ).UseNewSaturation(1.2f).UseNewColor(1f, 0f, 0f).UseType(6);
            }

            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 3;
        }
        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.maxStack = 9999;
            Item.value = Item.sellPrice(0, 0, 75, 0);
            Item.rare = ItemRarityID.Green;
            Item.dye = Item.dye;
        }
        public override void AddRecipes()
        {
            CreateRecipe(2)
                .AddIngredient(ItemID.LifeHairDye)
                .AddIngredient(ItemID.BottledWater)
                .AddTile(TileID.DyeVat)
                .Register();
        }
    }


    public class ManaDye : ModItem
    {
        public override void SetStaticDefaults()
        {
            if (!Main.dedServ)
            {
                GameShaders.Armor.BindShader(
                    Item.type,
                    new SupplementariesShaderData(Main.PixelShaderRef, "ArmorColored")
                ).UseNewSaturation(1.2f).UseNewColor(0f, 0f, 1f).UseType(7);
            }

            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 3;
        }
        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.maxStack = 9999;
            Item.value = Item.sellPrice(0, 0, 75, 0);
            Item.rare = ItemRarityID.Green;
            Item.dye = Item.dye;
        }
        public override void AddRecipes()
        {
            CreateRecipe(2)
                .AddIngredient(ItemID.ManaHairDye)
                .AddIngredient(ItemID.BottledWater)
                .AddTile(TileID.DyeVat)
                .Register();
        }
    }

    public class RichesDye : ModItem
    {
        public override void SetStaticDefaults()
        {
            if (!Main.dedServ)
            {
                GameShaders.Armor.BindShader(
                    Item.type,
                    new SupplementariesShaderData(Main.PixelShaderRef, "ArmorColored")
                ).UseNewSaturation(1.2f).UseType(8);
            }

            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 3;
        }
        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.maxStack = 9999;
            Item.value = Item.sellPrice(0, 0, 75, 0);
            Item.rare = ItemRarityID.Green;
            Item.dye = Item.dye;
        }
        public override void AddRecipes()
        {
            CreateRecipe(2)
                .AddIngredient(ItemID.MoneyHairDye)
                .AddIngredient(ItemID.BottledWater)
                .AddTile(TileID.DyeVat)
                .Register();
        }
    }

    public class ShiftingRainbowDye : ModItem
    {
        public override void SetStaticDefaults()
        {
            if (!Main.dedServ)
            {
                GameShaders.Armor.BindShader(
                    Item.type,
                    new SupplementariesShaderData(Main.PixelShaderRef, "ArmorColored")
                ).UseNewSaturation(1.2f).UseNewOpacity(1f).UseType(9);

            }

            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 3;
        }
        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.maxStack = 9999;
            Item.value = Item.sellPrice(0, 1, 50, 0);
            Item.rare = ItemRarityID.Orange;
            Item.dye = Item.dye;
        }
        public override void AddRecipes()
        {
            CreateRecipe(2)
                .AddIngredient(ItemID.RainbowBrick, 5)
                .AddIngredient(ItemID.BottledWater)
                .AddTile(TileID.DyeVat)
                .Register();
        }
    }

    public class ShiningHeartDye : ModItem
    {
        public override void SetStaticDefaults()
        {
            if (!Main.dedServ)
            {
                GameShaders.Armor.BindShader(
                    Item.type,
                    new SupplementariesShaderData(Main.PixelShaderRef, "ArmorColored")
                ).UseNewSaturation(1.2f).UseType(5);
            }

            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 3;
        }
        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.maxStack = 9999;
            Item.value = Item.sellPrice(0, 0, 75, 0);
            Item.rare = ItemRarityID.Green;
            Item.dye = Item.dye;
        }
        public override void AddRecipes()
        {
            CreateRecipe(2)
                .AddIngredient(ItemID.LifeCrystal)
                .AddIngredient(ItemID.BottledWater)
                .AddTile(TileID.DyeVat)
                .Register();
        }
    }

    public class SpeedDye : ModItem
    {
        public override void SetStaticDefaults()
        {
            if (!Main.dedServ)
            {
                GameShaders.Armor.BindShader(
                    Item.type,
                    new SupplementariesShaderData(Main.PixelShaderRef, "ArmorColored")
                ).UseNewColor(0f, 0f, 0f).UseNewSecondaryColor(0.3f, 1f, 0.78f).UseNewSaturation(1.2f).UseType(10);
            }

            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 3;
        }
        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.maxStack = 9999;
            Item.value = Item.sellPrice(0, 0, 75, 0);
            Item.rare = ItemRarityID.Green;
            Item.dye = Item.dye;
        }
        public override void AddRecipes()
        {
            CreateRecipe(2)
                .AddIngredient(ItemID.SpeedHairDye)
                .AddIngredient(ItemID.BottledWater)
                .AddTile(TileID.DyeVat)
                .Register();
        }
    }

    public class StarLightDye : ModItem
    {
        public override void SetStaticDefaults()
        {
            if (!Main.dedServ)
            {
                GameShaders.Armor.BindShader(
                    Item.type,
                    new SupplementariesShaderData(Main.PixelShaderRef, "ArmorColored")
                ).UseNewSaturation(1.2f).UseType(11);
            }

            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 3;
        }
        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.maxStack = 9999;
            Item.value = Item.sellPrice(0, 0, 75, 0);
            Item.rare = ItemRarityID.Green;
            Item.dye = Item.dye;
        }
        public override void AddRecipes()
        {
            CreateRecipe(2)
                    .AddIngredient(ItemID.FallenStar, 5)
                    .AddIngredient(ItemID.BottledWater)
                    .AddTile(TileID.DyeVat)
                    .Register();
        }
    }
    public class TimeDye : ModItem
    {
        public override void SetStaticDefaults()
        {
            if (!Main.dedServ)
            {
                GameShaders.Armor.BindShader(
                    Item.type,
                    new SupplementariesShaderData(Main.PixelShaderRef, "ArmorColored")
                ).UseNewSaturation(1.2f).UseType(12);
            }

            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 3;
        }
        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.maxStack = 9999;
            Item.value = Item.sellPrice(0, 0, 75, 0);
            Item.rare = ItemRarityID.Green;
            Item.dye = Item.dye;
        }
        public override void AddRecipes()
        {
            CreateRecipe(2)
                    .AddIngredient(ItemID.TimeHairDye)
                    .AddIngredient(ItemID.BottledWater)
                    .AddTile(TileID.DyeVat)
                    .Register();
        }

        public class FamiliarHairDye : ModItem
        {
            public override string Texture => "MagicDyeSupplementaries/Content/Items/Dyes/SpecialDyeBase";
            public override void SetStaticDefaults()
            {
                if (!Main.dedServ)
                {
                    GameShaders.Armor.BindShader(
                        Item.type,
                        new SupplementariesShaderData(Main.PixelShaderRef, "ArmorColored")
                    ).UseNewSaturation(1.2f).UseType(13);
                }

                CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 3;
            }
            public override void SetDefaults()
            {
                Item.width = 20;
                Item.height = 20;
                Item.maxStack = 9999;
                Item.value = Item.sellPrice(0, 0, 75, 0);
                Item.rare = ItemRarityID.Green;
                Item.dye = Item.dye;
            }
            public override void PostDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
            {
                Asset<Texture2D> texture = ModContent.Request<Texture2D>("MagicDyeSupplementaries/Content/Items/Dyes/FamilarDye_Overlay");
                spriteBatch.Draw(texture.Value, position, frame, Main.LocalPlayer.hairColor, 0, origin, scale, SpriteEffects.None, 0);
            }
            public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
            {
                Asset<Texture2D> texture = ModContent.Request<Texture2D>("MagicDyeSupplementaries/Content/Items/Dyes/FamilarDye_Overlay");
                Main.GetItemDrawFrame(Item.type, out var itemTexture, out var itemFrame);
                Vector2 origin = itemFrame.Size() / 2f;
                Vector2 drawPosition = Item.Bottom - Main.screenPosition - new Vector2(0, origin.Y);
                Color Recolor = new Color(Math.Clamp(Main.LocalPlayer.hairColor.R - (255 - (lightColor.R)), 0, 255), Math.Clamp(Main.LocalPlayer.hairColor.G - (255 - (lightColor.G)), 0, 255), Math.Clamp(Main.LocalPlayer.hairColor.B - (255 - (lightColor.B)), 0, 255), lightColor.A);

                spriteBatch.Draw(texture.Value, drawPosition, itemFrame, Recolor, rotation, origin, scale, SpriteEffects.None, 0);
            }
            public override void AddRecipes()
            {
                CreateRecipe(2)
                        .AddIngredient(ItemID.FamiliarWig)
                        .AddIngredient(ItemID.BottledWater)
                        .AddTile(TileID.DyeVat)
                        .Register();
            }
        }

        public class FamiliarSkinDye : ModItem
        {
            public override string Texture => "MagicDyeSupplementaries/Content/Items/Dyes/SpecialDyeBase";
            public override void SetStaticDefaults()
            {
                if (!Main.dedServ)
                {
                    GameShaders.Armor.BindShader(
                        Item.type,
                        new SupplementariesShaderData(Main.PixelShaderRef, "ArmorColored")
                    ).UseNewSaturation(1.2f).UseType(14);
                }

                CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 3;
            }
            public override void SetDefaults()
            {
                Item.width = 20;
                Item.height = 20;
                Item.maxStack = 9999;
                Item.value = Item.sellPrice(0, 0, 75, 0);
                Item.rare = ItemRarityID.Green;
                Item.dye = Item.dye;
            }
            public override void PostDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
            {
                Asset<Texture2D> texture = ModContent.Request<Texture2D>("MagicDyeSupplementaries/Content/Items/Dyes/FamilarDye_Overlay");
                spriteBatch.Draw(texture.Value, position, frame, Main.LocalPlayer.skinColor, 0, origin, scale, SpriteEffects.None, 0);
            }
            public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
            {
                Asset<Texture2D> texture = ModContent.Request<Texture2D>("MagicDyeSupplementaries/Content/Items/Dyes/FamilarDye_Overlay");
                Main.GetItemDrawFrame(Item.type, out var itemTexture, out var itemFrame);
                Vector2 origin = itemFrame.Size() / 2f;
                Vector2 drawPosition = Item.Bottom - Main.screenPosition - new Vector2(0, origin.Y);
                Color Recolor = new Color(Math.Clamp(Main.LocalPlayer.skinColor.R - (255 - (lightColor.R)), 0, 255), Math.Clamp(Main.LocalPlayer.skinColor.G - (255 - (lightColor.G)), 0, 255), Math.Clamp(Main.LocalPlayer.skinColor.B - (255 - (lightColor.B)), 0, 255), lightColor.A);

                spriteBatch.Draw(texture.Value, drawPosition, itemFrame, Recolor, rotation, origin, scale, SpriteEffects.None, 0);
            }
            public override void AddRecipes()
            {
                CreateRecipe(2)
                        .AddIngredient(ItemID.FamiliarWig)
                        .AddIngredient(ItemID.BottledWater)
                        .AddTile(TileID.DyeVat)
                        .Register();
            }
        }

        public class FamiliarEyesDye : ModItem
        {
            public override string Texture => "MagicDyeSupplementaries/Content/Items/Dyes/SpecialDyeBase";
            public override void SetStaticDefaults()
            {
                if (!Main.dedServ)
                {
                    GameShaders.Armor.BindShader(
                        Item.type,
                        new SupplementariesShaderData(Main.PixelShaderRef, "ArmorColored")
                    ).UseNewSaturation(1.2f).UseType(15);
                }

                CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 3;
            }
            public override void SetDefaults()
            {
                Item.width = 20;
                Item.height = 20;
                Item.maxStack = 9999;
                Item.value = Item.sellPrice(0, 0, 75, 0);
                Item.rare = ItemRarityID.Green;
                Item.dye = Item.dye;
            }
            public override void PostDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
            {
                Asset<Texture2D> texture = ModContent.Request<Texture2D>("MagicDyeSupplementaries/Content/Items/Dyes/FamilarDye_Overlay");
                spriteBatch.Draw(texture.Value, position, frame, Main.LocalPlayer.eyeColor, 0, origin, scale, SpriteEffects.None, 0);
            }
            public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
            {
                Asset<Texture2D> texture = ModContent.Request<Texture2D>("MagicDyeSupplementaries/Content/Items/Dyes/FamilarDye_Overlay");
                Main.GetItemDrawFrame(Item.type, out var itemTexture, out var itemFrame);
                Vector2 origin = itemFrame.Size() / 2f;
                Vector2 drawPosition = Item.Bottom - Main.screenPosition - new Vector2(0, origin.Y);
                Color Recolor = new Color(Math.Clamp(Main.LocalPlayer.eyeColor.R - (255 - (lightColor.R)), 0, 255), Math.Clamp(Main.LocalPlayer.eyeColor.G - (255 - (lightColor.G)), 0, 255), Math.Clamp(Main.LocalPlayer.eyeColor.B - (255 - (lightColor.B)), 0, 255), lightColor.A);

                spriteBatch.Draw(texture.Value, drawPosition, itemFrame, Recolor, rotation, origin, scale, SpriteEffects.None, 0);
            }
            public override void AddRecipes()
            {
                CreateRecipe(2)
                        .AddIngredient(ItemID.FamiliarWig)
                        .AddIngredient(ItemID.BottledWater)
                        .AddTile(TileID.DyeVat)
                        .Register();
            }
        }

        public class FamiliarShirtDye : ModItem
        {
            public override string Texture => "MagicDyeSupplementaries/Content/Items/Dyes/SpecialDyeBase";
            public override void SetStaticDefaults()
            {
                if (!Main.dedServ)
                {
                    GameShaders.Armor.BindShader(
                        Item.type,
                        new SupplementariesShaderData(Main.PixelShaderRef, "ArmorColored")
                    ).UseNewSaturation(1.2f).UseType(16);
                }

                CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 3;
            }
            public override void SetDefaults()
            {
                Item.width = 20;
                Item.height = 20;
                Item.maxStack = 9999;
                Item.value = Item.sellPrice(0, 0, 75, 0);
                Item.rare = ItemRarityID.Green;
                Item.dye = Item.dye;
            }
            public override void PostDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
            {
                Asset<Texture2D> texture = ModContent.Request<Texture2D>("MagicDyeSupplementaries/Content/Items/Dyes/FamilarDye_Overlay");
                spriteBatch.Draw(texture.Value, position, frame, Main.LocalPlayer.shirtColor, 0, origin, scale, SpriteEffects.None, 0);
            }
            public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
            {
                Asset<Texture2D> texture = ModContent.Request<Texture2D>("MagicDyeSupplementaries/Content/Items/Dyes/FamilarDye_Overlay");
                Main.GetItemDrawFrame(Item.type, out var itemTexture, out var itemFrame);
                Vector2 origin = itemFrame.Size() / 2f;
                Vector2 drawPosition = Item.Bottom - Main.screenPosition - new Vector2(0, origin.Y);
                Color Recolor = new Color(Math.Clamp(Main.LocalPlayer.shirtColor.R - (255 - (lightColor.R)), 0, 255), Math.Clamp(Main.LocalPlayer.shirtColor.G - (255 - (lightColor.G)), 0, 255), Math.Clamp(Main.LocalPlayer.shirtColor.B - (255 - (lightColor.B)), 0, 255), lightColor.A);

                spriteBatch.Draw(texture.Value, drawPosition, itemFrame, Recolor, rotation, origin, scale, SpriteEffects.None, 0);
            }
            public override void AddRecipes()
            {
                CreateRecipe(2)
                        .AddIngredient(ItemID.FamiliarShirt)
                        .AddIngredient(ItemID.BottledWater)
                        .AddTile(TileID.DyeVat)
                        .Register();
            }
        }

        public class FamiliarUndershirtDye : ModItem
        {
            public override string Texture => "MagicDyeSupplementaries/Content/Items/Dyes/SpecialDyeBase";
            public override void SetStaticDefaults()
            {
                if (!Main.dedServ)
                {
                    GameShaders.Armor.BindShader(
                        Item.type,
                        new SupplementariesShaderData(Main.PixelShaderRef, "ArmorColored")
                    ).UseNewSaturation(1.2f).UseType(17);
                }

                CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 3;
            }
            public override void SetDefaults()
            {
                Item.width = 20;
                Item.height = 20;
                Item.maxStack = 9999;
                Item.value = Item.sellPrice(0, 0, 75, 0);
                Item.rare = ItemRarityID.Green;
                Item.dye = Item.dye;
            }
            public override void PostDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
            {
                Asset<Texture2D> texture = ModContent.Request<Texture2D>("MagicDyeSupplementaries/Content/Items/Dyes/FamilarDye_Overlay");
                spriteBatch.Draw(texture.Value, position, frame, Main.LocalPlayer.underShirtColor, 0, origin, scale, SpriteEffects.None, 0);
            }
            public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
            {
                Asset<Texture2D> texture = ModContent.Request<Texture2D>("MagicDyeSupplementaries/Content/Items/Dyes/FamilarDye_Overlay");
                Main.GetItemDrawFrame(Item.type, out var itemTexture, out var itemFrame);
                Vector2 origin = itemFrame.Size() / 2f;
                Vector2 drawPosition = Item.Bottom - Main.screenPosition - new Vector2(0, origin.Y);
                Color Recolor = new Color(Math.Clamp(Main.LocalPlayer.underShirtColor.R - (255 - (lightColor.R)), 0, 255), Math.Clamp(Main.LocalPlayer.underShirtColor.G - (255 - (lightColor.G)), 0, 255), Math.Clamp(Main.LocalPlayer.underShirtColor.B - (255 - (lightColor.B)), 0, 255), lightColor.A);

                spriteBatch.Draw(texture.Value, drawPosition, itemFrame, Recolor, rotation, origin, scale, SpriteEffects.None, 0);
            }
            public override void AddRecipes()
            {
                CreateRecipe(2)
                        .AddIngredient(ItemID.FamiliarShirt)
                        .AddIngredient(ItemID.BottledWater)
                        .AddTile(TileID.DyeVat)
                        .Register();
            }
        }

        public class FamiliarPantsDye : ModItem
        {
            public override string Texture => "MagicDyeSupplementaries/Content/Items/Dyes/SpecialDyeBase";
            public override void SetStaticDefaults()
            {
                if (!Main.dedServ)
                {
                    GameShaders.Armor.BindShader(
                        Item.type,
                        new SupplementariesShaderData(Main.PixelShaderRef, "ArmorColored")
                    ).UseNewSaturation(1.2f).UseType(18);
                }

                CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 3;
            }
            public override void SetDefaults()
            {
                Item.width = 20;
                Item.height = 20;
                Item.maxStack = 9999;
                Item.value = Item.sellPrice(0, 0, 75, 0);
                Item.rare = ItemRarityID.Green;
                Item.dye = Item.dye;
            }
            public override void PostDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
            {
                Asset<Texture2D> texture = ModContent.Request<Texture2D>("MagicDyeSupplementaries/Content/Items/Dyes/FamilarDye_Overlay");
                spriteBatch.Draw(texture.Value, position, frame, Main.LocalPlayer.pantsColor, 0, origin, scale, SpriteEffects.None, 0);
            }
            public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
            {
                Asset<Texture2D> texture = ModContent.Request<Texture2D>("MagicDyeSupplementaries/Content/Items/Dyes/FamilarDye_Overlay");
                Main.GetItemDrawFrame(Item.type, out var itemTexture, out var itemFrame);
                Vector2 origin = itemFrame.Size() / 2f;
                Vector2 drawPosition = Item.Bottom - Main.screenPosition - new Vector2(0, origin.Y);
                Color Recolor = new Color(Math.Clamp(Main.LocalPlayer.pantsColor.R - (255 - (lightColor.R)), 0, 255), Math.Clamp(Main.LocalPlayer.pantsColor.G - (255 - (lightColor.G)), 0, 255), Math.Clamp(Main.LocalPlayer.pantsColor.B - (255 - (lightColor.B)), 0, 255), lightColor.A);

                spriteBatch.Draw(texture.Value, drawPosition, itemFrame, Recolor, rotation, origin, scale, SpriteEffects.None, 0);
            }
            public override void AddRecipes()
            {
                CreateRecipe(2)
                        .AddIngredient(ItemID.FamiliarPants)
                        .AddIngredient(ItemID.BottledWater)
                        .AddTile(TileID.DyeVat)
                        .Register();
            }
        }
        public class FamiliarShoesDye : ModItem
        {
            public override string Texture => "MagicDyeSupplementaries/Content/Items/Dyes/SpecialDyeBase";
            public override void SetStaticDefaults()
            {
                if (!Main.dedServ)
                {
                    GameShaders.Armor.BindShader(
                        Item.type,
                        new SupplementariesShaderData(Main.PixelShaderRef, "ArmorColored")
                    ).UseNewSaturation(1.2f).UseType(19);
                }

                CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 3;
            }
            public override void SetDefaults()
            {
                Item.width = 20;
                Item.height = 20;
                Item.maxStack = 9999;
                Item.value = Item.sellPrice(0, 0, 75, 0);
                Item.rare = ItemRarityID.Green;
                Item.dye = Item.dye;
            }
            public override void PostDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
            {
                Asset<Texture2D> texture = ModContent.Request<Texture2D>("MagicDyeSupplementaries/Content/Items/Dyes/FamilarDye_Overlay");
                spriteBatch.Draw(texture.Value, position, frame, Main.LocalPlayer.shoeColor, 0, origin, scale, SpriteEffects.None, 0);
            }
            public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
            {
                Asset<Texture2D> texture = ModContent.Request<Texture2D>("MagicDyeSupplementaries/Content/Items/Dyes/FamilarDye_Overlay");
                Main.GetItemDrawFrame(Item.type, out var itemTexture, out var itemFrame);
                Vector2 origin = itemFrame.Size() / 2f;
                Vector2 drawPosition = Item.Bottom - Main.screenPosition - new Vector2(0, origin.Y);
                Color Recolor = new Color(Math.Clamp(Main.LocalPlayer.shoeColor.R - (255 - (lightColor.R)), 0, 255), Math.Clamp(Main.LocalPlayer.shoeColor.G - (255 - (lightColor.G)), 0, 255), Math.Clamp(Main.LocalPlayer.shoeColor.B - (255 - (lightColor.B)), 0, 255), lightColor.A);

                spriteBatch.Draw(texture.Value, drawPosition, itemFrame, Recolor, rotation, origin, scale, SpriteEffects.None, 0);
            }
            public override void AddRecipes()
            {
                CreateRecipe(2)
                        .AddIngredient(ItemID.FamiliarPants)
                        .AddIngredient(ItemID.BottledWater)
                        .AddTile(TileID.DyeVat)
                        .Register();
            }
        }
    }
}