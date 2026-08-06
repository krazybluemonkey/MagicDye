using Terraria;
using Terraria.ID;
using ReLogic.Content;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.Graphics.Shaders;
using Terraria.GameContent.Creative;
using Microsoft.Xna.Framework.Graphics;

namespace MagicDyeSupplementaries.Content.Items.Dyes
{
	public class ForceFieldDye : ModItem
	{
		public override void SetStaticDefaults()
		{
            if (!Main.dedServ)
            {
                GameShaders.Armor.BindShader(
                    Item.type,
                    new ArmorShaderData(Main.PixelShaderRef, "ArmorForceField")
                ).UseColor(0f, 2f, 3f);
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
        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
        {
            Asset<Texture2D> texture = ModContent.Request<Texture2D>("DyeHard/Content/Items/Dyes/ForceFieldDye_Glow");
            Main.spriteBatch.Draw(texture.Value, new Vector2(Item.position.X - Main.screenPosition.X + Item.width * 0.5f, Item.position.Y - Main.screenPosition.Y + Item.height - texture.Height() * 0.5f + 2f), new Rectangle(0, 0, texture.Width(), texture.Height()), Color.White, rotation, texture.Size() * 0.5f, scale, SpriteEffects.None, 0f);
        }
        public override void AddRecipes()
        {
            CreateRecipe(2)
                .AddIngredient(ItemID.MartianConduitPlating, 5)
                .AddIngredient(ItemID.BottledWater)
                .AddTile(TileID.DyeVat)
                .Register();
		}
	}
}