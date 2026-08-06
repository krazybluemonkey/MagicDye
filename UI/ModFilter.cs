using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.GameContent.UI.Elements;
using Terraria.Localization;
using Terraria.UI;

namespace MagicDye.UI;

public class ModFilters // based on Terraria.GameContent.Creative.ItemFilters
{
    public class VanillaDye : IItemEntryFilter, IEntryFilter<Item>
    {
        public bool FitsFilter(Item entry) => entry.ModItem == null;

        public string GetDisplayNameKey() => Language.GetTextValue("Mods.MagicDye.UI.Filters.Vanilla");

        public UIElement GetImage()
        {
            Asset<Texture2D> asset = Main.Assets.Request<Texture2D>("Images/UI/Creative/Infinite_Icons");
            return new UIImageFramed(asset, asset.Frame(11, 1, 4).OffsetSize(-2, 0))
            {
                HAlign = 0.5f,
                VAlign = 0.5f
            };
        }
    }
    public class ModdedDye : IItemEntryFilter, IEntryFilter<Item>
    {
        public string ModName = "";
        public bool FitsFilter(Item entry) => entry.ModItem != null && entry.ModItem.Mod.DisplayNameClean == ModName;

        public string GetDisplayNameKey() => ModName;


        public void SetModName(string name) => ModName = name;

        public UIElement GetImage()
        {
            Asset<Texture2D> asset = Main.Assets.Request<Texture2D>("Images/UI/Creative/Infinite_Icons");
            return new UIImageFramed(asset, asset.Frame(11, 1, 4).OffsetSize(-2, 0))
            {
                HAlign = 0.5f,
                VAlign = 0.5f
            };
        }
    }

}
