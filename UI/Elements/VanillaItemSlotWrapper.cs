using Terraria;
using Terraria.UI;
using Terraria.GameInput;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria.GameContent;
using Terraria.ID;

namespace MagicDye.UI.Elements
{
    internal class VanillaItemSlotWrapper : UIElement // taken from example mod, VanillaItemSlotWrapper.cs in \Old\UI\
    {
        internal Item Item;
        private readonly int _context;
        private readonly int _visual;
        private readonly float _scale;
        internal Func<Item, bool> ValidItemFunc;
        internal string HoverText;


        public VanillaItemSlotWrapper(int context = ItemSlot.Context.BankItem, int visual = ItemSlot.Context.BankItem, float scale = 1f)
        {
            _context = context;
            _visual = visual;
            _scale = scale;
            Item = new Item();
            Item.SetDefaults(ItemID.None);

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
                if (ValidItemFunc == null || ValidItemFunc(Main.mouseItem))
                {
                    // Handle handles all the click and hover actions based on the context.
                    ItemSlot.Handle(ref Item, _context);
                }
            }
            // Draw draws the slot itself and Item. Depending on context, the color will change, as will drawing other things like stack counts.
            ItemSlot.Draw(spriteBatch, ref Item, _visual, rectangle.TopLeft());
            Main.inventoryScale = oldScale;
            if (IsMouseHovering && Item.IsAir)
            {
                Main.hoverItemName = HoverText;
            }
        }
    }
}