

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.UI;
using Terraria.UI.Gamepad;

namespace MagicDye.UI.Elements;
public class UIDynamicDyeCollection : UIElement // based loosely on Terraria.GameContent.UI.Elements.UIDynamicItemCollection
{
    private List<int> _itemIdsAvailableToShow = new List<int>(); 
    private List<bool> _itemClicked = new List<bool>();
    private List<bool> _itemHovered = new List<bool>();
    private List<int> _itemIdsToLoadTexturesFor = new List<int>();
    private int _itemsPerLine;
    private List<SnapPoint> _dummySnapPoints = new List<SnapPoint>();
    private Item _item = new Item();
    public int clickedItemID;

    public UIDynamicDyeCollection()
    {
        Width = new StyleDimension(0f, 1f);
        HAlign = 0.5f;
        UpdateSize();
    }

    protected override void DrawSelf(SpriteBatch spriteBatch)
    {
        Main.inventoryScale = 0.84615386f;
        GetGridParameters(out var startX, out var startY, out var startItemIndex, out var endItemIndex);
        int num = _itemsPerLine;
        for (int i = startItemIndex; i < endItemIndex; i++)
        {
            int index = _itemIdsAvailableToShow[i];
            Rectangle itemSlotHitbox = GetItemSlotHitbox(startX, startY, startItemIndex, i);
            Item inv = ContentSamples.ItemsByType[index];
            if (TextureAssets.Item[index].State == AssetState.NotLoaded)
                num--;

            bool cREATIVE_ItemSlotShouldHighlightAsSelected = false;
            ResetHovered();
            if (base.IsMouseHovering && itemSlotHitbox.Contains(Main.MouseScreen.ToPoint()) && !PlayerInput.IgnoreMouseInterface)
            {
                _item.SetDefaults(inv.type);
                inv = _item;
                Main.LocalPlayer.mouseInterface = true;
                ItemSlot.OverrideHover(ref inv, ItemSlot.Context.CreativeInfinite);
                LeftClick(i, this);
                Hover(i, inv, ItemSlot.Context.CreativeInfinite);
                cREATIVE_ItemSlotShouldHighlightAsSelected = true;
            }

            UILinkPointNavigator.Shortcuts.CREATIVE_ItemSlotShouldHighlightAsSelected = cREATIVE_ItemSlotShouldHighlightAsSelected;
            DrawSlot(spriteBatch, ref inv, _itemClicked[i], _itemHovered[i], itemSlotHitbox.TopLeft());
            if (num <= 0)
                break;
        }

        while (_itemIdsToLoadTexturesFor.Count > 0 && num > 0)
        {
            int num3 = _itemIdsToLoadTexturesFor[0];
            _itemIdsToLoadTexturesFor.RemoveAt(0);
            if (TextureAssets.Item[num3].State == AssetState.NotLoaded)
            {
                Main.instance.LoadItem(num3);
                num -= 4;
            }
        }
    }

    public static void DrawSlot(SpriteBatch spriteBatch, ref Item inv, bool clicked, bool hovered, Vector2 position, Color lightColor = default(Color))
    {
        Player player = Main.player[Main.myPlayer];
        Item item = inv;
        float inventoryScale = Main.inventoryScale;
        Color color = Color.White;
        if (lightColor != Color.Transparent)
            color = lightColor;

        Texture2D value = TextureAssets.InventoryBack18.Value;
        Color color2 = new Color(33, 15, 91, 220);

        if (hovered)
        {
            color2 = new Color(84, 97, 182, 255);
        }

        if (clicked)
        {
            if (hovered)
            {

                color2 = new Color(255, 255, 105, 255);
            }
            else
            {
                color2 = new Color(255, 227, 24, 255);
            }
        }

        bool highlightThingsForMouse = PlayerInput.SettingsForUI.HighlightThingsForMouse; 
        
        spriteBatch.Draw(value, position, null, color2, 0f, default(Vector2), inventoryScale, SpriteEffects.None, 0f);

        int num9 = -1;

        if ((item.type <= ItemID.None || item.stack <= 0) && num9 != -1)
        {
            Texture2D value6 = TextureAssets.Extra[ExtrasID.EquipIcons].Value;
            Rectangle rectangle = value6.Frame(3, 6, num9 % 3, num9 / 3);
            rectangle.Width -= 2;
            rectangle.Height -= 2;

            spriteBatch.Draw(value6, position + value.Size() / 2f * inventoryScale, rectangle, Color.White * 0.35f, 0f, rectangle.Size() / 2f, inventoryScale, SpriteEffects.None, 0f);
        }

        Vector2 vector = value.Size() * inventoryScale;
        if (item.type > ItemID.None && item.stack > 0)
        {
            float scale = ItemSlot.DrawItemIcon(item, ItemSlot.Context.CreativeInfinite, spriteBatch, position + vector / 2f, inventoryScale, 32f, color);
        }
    }

    public void LeftClick(int index, UIElement listeningElement)
    {
        Player player = Main.player[Main.myPlayer];
        bool flag = Main.mouseLeftRelease && Main.mouseLeft;
        if (flag)
        {
            SoundEngine.PlaySound(SoundID.MenuTick);
            ResetClicked();
            _itemClicked[index] = true;
            clickedItemID = _itemIdsAvailableToShow[index];
        }
    }

    public void Hover(int index, Item inv, int context)
    {
        ResetHovered();
        _itemHovered[index] = true;
        ItemSlot.MouseHover(ref inv, context);
    }

    public void ResetClicked()
    {
        for (int i = 0; i < _itemClicked.Count; i++)
        {
            _itemClicked[i] = false;
        }
    }
    public void ResetHovered()
    {
        for (int i = 0; i < _itemHovered.Count; i++)
        {
            _itemHovered[i] = false;
        }
    }

    private Rectangle GetItemSlotHitbox(int startX, int startY, int startItemIndex, int i)
    {
        int num = i - startItemIndex;
        int num2 = num % _itemsPerLine;
        int num3 = num / _itemsPerLine;
        return new Rectangle(startX + num2 * 44, startY + num3 * 44, 44, 44);
    }

    private void GetGridParameters(out int startX, out int startY, out int startItemIndex, out int endItemIndex)
    {
        Rectangle rectangle = GetDimensions().ToRectangle();
        Rectangle viewCullingArea = base.Parent.GetViewCullingArea();
        int x = rectangle.Center.X;
        startX = x - (int)((float)(44 * _itemsPerLine) * 0.5f);
        startY = rectangle.Top;
        startItemIndex = 0;
        endItemIndex = _itemIdsAvailableToShow.Count;
        int num = (Math.Min(viewCullingArea.Top, rectangle.Top) - viewCullingArea.Top) / 44;
        startY += -num * 44;
        startItemIndex += -num * _itemsPerLine;
        int num2 = (int)Math.Ceiling((float)viewCullingArea.Height / 44f) * _itemsPerLine;
        if (endItemIndex > num2 + startItemIndex + _itemsPerLine)
            endItemIndex = num2 + startItemIndex + _itemsPerLine;
    }

    public override void Recalculate()
    {
        base.Recalculate();
        UpdateSize();
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
        if (base.IsMouseHovering)
            Main.LocalPlayer.mouseInterface = true;
    }

    public void SetContentsToShow(List<int> itemIdsToShow)
    {
        _itemIdsAvailableToShow.Clear();
        _itemClicked.Clear();
        _itemHovered.Clear();
        _itemIdsToLoadTexturesFor.Clear();
        _itemIdsAvailableToShow.AddRange(itemIdsToShow);
        for (int i = 0; i < itemIdsToShow.Count; i++)
        {
            _itemClicked.Add(false);
            _itemHovered.Add(false);
        }
        _itemIdsToLoadTexturesFor.AddRange(itemIdsToShow);
        UpdateSize();
    }

    public int GetItemsPerLine() => _itemsPerLine;

    public override List<SnapPoint> GetSnapPoints()
    {
        List<SnapPoint> list = new List<SnapPoint>();
        GetGridParameters(out var startX, out var startY, out var startItemIndex, out var endItemIndex);
        _ = _itemsPerLine;
        Rectangle viewCullingArea = base.Parent.GetViewCullingArea();
        int num = endItemIndex - startItemIndex;
        while (_dummySnapPoints.Count < num)
        {
            _dummySnapPoints.Add(new SnapPoint("CreativeInfinitesSlot", 0, Vector2.Zero, Vector2.Zero));
        }

        int num2 = 0;
        Vector2 vector = GetDimensions().Position();
        for (int i = startItemIndex; i < endItemIndex; i++)
        {
            Point center = GetItemSlotHitbox(startX, startY, startItemIndex, i).Center;
            if (viewCullingArea.Contains(center))
            {
                SnapPoint snapPoint = _dummySnapPoints[num2];
                snapPoint.ThisIsAHackThatChangesTheSnapPointsInfo(Vector2.Zero, center.ToVector2() - vector, num2);
                snapPoint.Calculate(this);
                num2++;
                list.Add(snapPoint);
            }
        }

        foreach (UIElement element in Elements)
        {
            list.AddRange(element.GetSnapPoints());
        }

        return list;
    }

    public void UpdateSize()
    {
        int num = (_itemsPerLine = GetDimensions().ToRectangle().Width / 44);
        int num2 = (int)Math.Ceiling((float)_itemIdsAvailableToShow.Count / (float)num);
        MinHeight.Set(44 * num2, 0f);
    }
}
