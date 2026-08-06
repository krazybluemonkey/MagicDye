using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Graphics.Shaders;
using Terraria.GameContent.Creative;
using Microsoft.Xna.Framework.Graphics;

namespace MagicDyeSupplementaries.Content.Items.Dyes
{
	public class DeterminationDye : ModItem
	{
        public override void SetStaticDefaults()
        {
            if (!Main.dedServ)
            {
                GameShaders.Armor.BindShader(
                    Item.type,
                    new ArmorShaderData(Mod.Assets.Request<Effect>("Assets/Effects/SupplementariesEffect"), "ArmorDetermination")
                ).UseColor(1f, 1f, 1f).UseSecondaryColor(0f, 0f, 0f).UseSaturation(0.37f);
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
                .AddIngredient(ItemID.SilverDye)
                .AddIngredient(ItemID.BlackDye)
                .AddTile(TileID.DyeVat)
                .Register();
        }
    }

    public class InverseDeterminationDye : ModItem
    {
        public override void SetStaticDefaults()
        {
            if (!Main.dedServ)
            {
                GameShaders.Armor.BindShader(
                    Item.type,
                    new ArmorShaderData(Mod.Assets.Request<Effect>("Assets/Effects/SupplementariesEffect"), "ArmorDetermination")
                ).UseColor(0f, 0f, 0f).UseSecondaryColor(1f, 1f, 1f).UseSaturation(0.37f);
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
                .AddIngredient(ItemID.BlackDye)
                .AddIngredient(ItemID.SilverDye)
                .AddTile(TileID.DyeVat)
                .Register();
        }
    }
}