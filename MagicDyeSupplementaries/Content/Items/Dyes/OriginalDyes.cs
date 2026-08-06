using Terraria;
using Terraria.ID;
using ReLogic.Content;
using Terraria.ModLoader;
using Terraria.Graphics.Shaders;
using Terraria.GameContent.Creative;
using Microsoft.Xna.Framework.Graphics;

namespace MagicDyeSupplementaries.Content.Items.Dyes
{
	public class SepiaToneDye : ModItem
    {
		public override void SetStaticDefaults()
		{
            if (!Main.dedServ)
            {
                GameShaders.Armor.BindShader(
                    Item.type,
                    new ArmorShaderData(Mod.Assets.Request<Effect>("Assets/Effects/SupplementariesEffect"), "ArmorSepiaTone")
                );
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
	}

	public class GrayScaleDye : ModItem
	{
		public override void SetStaticDefaults()
		{
            if (!Main.dedServ)
            {
                GameShaders.Armor.BindShader(
                    Item.type,
                    new ArmorShaderData(Mod.Assets.Request<Effect>("Assets/Effects/SupplementariesEffect"), "ArmorGrayScale")
                );
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
	}
    public class PosterizeDye : ModItem
    {
        public override void SetStaticDefaults()
        {
            if (!Main.dedServ)
            {
                GameShaders.Armor.BindShader(
                    Item.type,
                    new ArmorShaderData(Mod.Assets.Request<Effect>("Assets/Effects/SupplementariesEffect"), "ArmorPosterize")
                    .UseColor(3.0f, 0.75f, 0f) //R controls color total, G controls gamma correction
                );
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
    }
}