using Terraria;
using Terraria.UI;
using Terraria.GameInput;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria.GameContent;

namespace MagicDye.UI.Elements
{
    internal class MagicDyeGridItem : UIElement // modified from example mod, VanillaItemSlotWrapper.cs in \Old\UI\
    {
        internal Item Item;
        private int _visual;
        private readonly float _scale;
        internal Func<Item, bool> ValidItemFunc;
        internal string HoverText;


        public MagicDyeGridItem(int type,int context = ItemSlot.Context.ChatItem, int visual = ItemSlot.Context.BankItem, float scale = 1f)
        {
            _visual = visual;
            _scale = scale;
            Item = new Item(type);

            Width.Set(TextureAssets.InventoryBack9.Value.Width * scale, 0f);
            Height.Set(TextureAssets.InventoryBack9.Value.Height * scale, 0f);
        }

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            float oldScale = Main.inventoryScale;
            Main.inventoryScale = _scale;
            Microsoft.Xna.Framework.Rectangle rectangle = GetDimensions().ToRectangle();

            if (ContainsPoint(Main.MouseScreen) && !PlayerInput.IgnoreMouseInterface)
            {
                Main.LocalPlayer.mouseInterface = true;
            }
            // Draw draws the slot itself and Item. Depending on context, the color will change, as will drawing other things like stack counts.
            ItemSlot.Draw(spriteBatch, ref Item, _visual, rectangle.TopLeft());
            Main.inventoryScale = oldScale;
        }
        protected void SetVisual(int visual)
        {
            _visual = visual;
        }

    }
}