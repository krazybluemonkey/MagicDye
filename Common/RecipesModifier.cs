using Terraria.ModLoader;
using Terraria;

namespace MagicDye.Common
{
    public static class MagicDyeVatConsumptionRules
    {
        public static void DyeConsume(Recipe recipe, int type, ref int amount, bool isDecrafting)
        {
            if (!Main.LocalPlayer.adjTile[ModContent.TileType<Content.Tiles.MagicDyeVatTile>()] || isDecrafting)
            {
                return;
            }

            int amountUsed = 0;

            for (int i = 0; i < amount; i++)
            {
                if (!Main.rand.NextBool(3))
                {
                    amountUsed++;
                }
            }

            amount = amountUsed;
        }
    }

    public class RecipesModifier : ModSystem
    {
        public override void PostAddRecipes()
        {
            for (int i = 0; i < Recipe.numRecipes; i++)
            {
                Recipe recipe = Main.recipe[i];

                if (recipe.createItem.dye != 0)
                {
                    recipe.AddConsumeIngredientCallback(MagicDyeVatConsumptionRules.DyeConsume);
                }
            }
        }
    }
}