using Terraria;
using Terraria.UI;
using Terraria.GameInput;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria.GameContent.UI.Elements;
using Terraria.Audio;
using Terraria.ID;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria.Localization;
using MagicDye.UI.Elements;
using MagicDye.UI.Systems;
using Terraria.Graphics.Shaders;
using System.Linq;
using Terraria.DataStructures;
using Terraria.GameContent.UI.States;
using System.Reflection;
using Terraria.GameContent.Creative;
using static Terraria.GameContent.Creative.CreativeUI;
using Terraria.GameContent.NetModules;
using Terraria.Net;
using ReLogic.OS;
using Newtonsoft.Json.Linq;
using Terraria.GameContent;
using Newtonsoft.Json;
using MagicDye.Common;

namespace MagicDye.UI
{
    class MagicDyeUI : UIState
    {
        private int _workingSlot = 0;
        private VanillaItemSlotWrapper _researchSlot;
        private UIPanel _researchButton;
        private int _researchSlotButtonValue;
        internal MagicDyeUISystem interfaceRef;
        private Vector3 _tempPrimaryColor = new Vector3(0.5f, 0.5f, 0.5f);
        private Vector3 _tempSecondaryColor = new Vector3(0.5f, 0.5f, 0.5f);
        private float _tempSaturation = 1f;
        private float _tempOpacity = 1f;
        private string _tempPass = "";
        private string _tempColorMod1 = "";
        private string _tempColorMod2 = "";
        private int _currentPage = 1;
        private UIColoredImageButton _colorAndPassesButton;
        private UIColoredImageButton _researchAndTemplatesButton;
        private UIPanel _slotsPanel;
        private UIColoredImageButton _copyTemplateButton;
        private UIColoredImageButton _pasteTemplateButton;
        private UIImageButton _filterButton;
        private UIDynamicDyeCollection _dyeGrid;
        private UIList _dyeList;
        internal Player tempPlayer;
        private List<int> _itemIdsAvailableTotal = new List<int>();
        private List<int> _itemIdsAvailableToShow = new List<int>();
        private EntrySorter<int, ICreativeItemSortStep> _sorter;
        private EntryFilterer<Item, IItemEntryFilter> _filterer;
        private int _lastItemIdSacrificed;
        private int _lastItemAmountWeHad;
        private int _lastItemAmountWeNeededTotal;
        private UISearchBar _searchBar;
        private UIPanel _searchBoxPanel;
        private string _searchString;
        private bool _didClickSomething;
        private bool _didClickSearchBar;
        private int _currentFilterIndex = -1;
        private List<string> _modList;
        private int _modListIndex = 0;
        private List<string> _passList;
        private List<string> _colorModList;
        private Dictionary<string, int> _passDictionary;
        private MagicDyePassGrid _passGrid;
        private MagicDyePassGrid _colorMods1Grid;
        private MagicDyePassGrid _colorMods2Grid;
        private UIPanel _BG;
        private UIPanel _pagePanel;
        private UIColorInterface _rPInterface;
        private UIColorInterface _gPInterface;
        private UIColorInterface _bPInterface;
        private UIColorInterface _rSInterface;
        private UIColorInterface _gSInterface;
        private UIColorInterface _bSInterface;
        private UIColorInterface _SatInterface;
        private UIColorInterface _OpacityInterface;
        private UIText _infoText;
        private int _infoTimeout = 0;
        private enum sliderTypes
        {
            Primary,
            Secondary,
            Saturation,
            Opacity,
        }

        // UI building
        private void AddSlotPanel(UIElement parent)
        {
            _slotsPanel = new UIPanel
            {
                Width = new StyleDimension(parent.Width.Pixels - parent.PaddingRight - parent.PaddingLeft, 0f),
                Height = new StyleDimension(28f, 0f),
                VAlign = 1f
            };
            _slotsPanel.BackgroundColor = new Color(35, 40, 83);
            _slotsPanel.BorderColor = new Color(35, 40, 83);
            _slotsPanel.SetPadding(0f);
            _slotsPanel.OnScrollWheel += Scroll_Slot;
            _slotsPanel.OnMouseOver += Hover_Panel;
            _slotsPanel.OnMouseOut += HoverOut_Panel;
            _slotsPanel.OnLeftClick += Click_SlotForward;
            _slotsPanel.OnRightClick += Click_SlotBack;
            parent.Append(_slotsPanel);
            UIText uIText = new UIText("", 0.8f)
            {
                HAlign = 0.5f,
                VAlign = 0.5f,
                IgnoresMouseInteraction = true
            };
            uIText.OnUpdate += Update_IndexText;
            _slotsPanel.Append(uIText);
        }

        private LocalizedText GetSliderText(sliderTypes id)
        {
            switch (id)
            {
                case sliderTypes.Primary:
                    return Language.GetText("Mods.MagicDye.UI.PrimaryColor");
                case sliderTypes.Secondary:
                    return Language.GetText("Mods.MagicDye.UI.SecondaryColor");
                case sliderTypes.Saturation:
                    return Language.GetText("Mods.MagicDye.UI.Saturation");
                default:
                    return Language.GetText("Mods.MagicDye.UI.Opacity");
            }
        }

        private void AddColorPanel(UIElement parent, float HAlign, float VAlign, sliderTypes id)
        {
            // color selector container
            UIPanel colorPanel = new();
            colorPanel.Width.Set(144, 0);
            colorPanel.Height.Set(116, 0);
            colorPanel.HAlign = HAlign;
            colorPanel.VAlign = VAlign;
            colorPanel.PaddingTop = 0.05f;
            colorPanel.PaddingBottom = 0f;
            parent.Append(colorPanel);

            // color label
            UITextPanel<LocalizedText> colorLabel = new(GetSliderText(id), 0.7f)
            {
                VAlign = 0f,
                HAlign = 0.5f,
                Top = new StyleDimension(-20f, 0f)
            };
            colorPanel.Append(colorLabel);

            // color selector type handling
            switch (id)
            {
                case sliderTypes.Primary:
                    //R
                    _rPInterface = new UIColorInterface(_tempPrimaryColor.X, new Color(1f, 0f, 0f))
                    {
                        HAlign = 0.5f,
                        VAlign = 0.2f
                    };
                    _rPInterface.OnUpdate += Update_RPPanel;
                    _rPInterface.Panel.OnMouseOver += Hover_Panel;
                    _rPInterface.Panel.OnMouseOut += HoverOut_Panel;
                    _rPInterface.Panel.OnLeftClick += LeftClick_RPPanel;
                    _rPInterface.Panel.OnRightClick += RightClick_RPPanel;
                    colorPanel.Append(_rPInterface);
                    //G
                    _gPInterface = new UIColorInterface(_tempPrimaryColor.Y, new Color(0f, 1f, 0f))
                    {
                        HAlign = 0.5f,
                        VAlign = 0.55f
                    };
                    _gPInterface.OnUpdate += Update_GPPanel;
                    _gPInterface.Panel.OnMouseOver += Hover_Panel;
                    _gPInterface.Panel.OnMouseOut += HoverOut_Panel;
                    _gPInterface.Panel.OnLeftClick += LeftClick_GPPanel;
                    _gPInterface.Panel.OnRightClick += RightClick_GPPanel;
                    colorPanel.Append(_gPInterface);
                    //B
                    _bPInterface = new UIColorInterface(_tempPrimaryColor.Z, new Color(0f, 0f, 1f))
                    {
                        HAlign = 0.5f,
                        VAlign = 0.9f
                    };
                    _bPInterface.OnUpdate += Update_BPPanel;
                    _bPInterface.Panel.OnMouseOver += Hover_Panel;
                    _bPInterface.Panel.OnMouseOut += HoverOut_Panel;
                    _bPInterface.Panel.OnLeftClick += LeftClick_BPPanel;
                    _bPInterface.Panel.OnRightClick += RightClick_BPPanel;
                    colorPanel.Append(_bPInterface);
                    break;
                case sliderTypes.Secondary:
                    //R
                    _rSInterface = new UIColorInterface(_tempSecondaryColor.X, new Color(1f, 0f, 0f))
                    {
                        HAlign = 0.5f,
                        VAlign = 0.2f
                    };
                    _rSInterface.OnUpdate += Update_RSPanel;
                    _rSInterface.Panel.OnMouseOver += Hover_Panel;
                    _rSInterface.Panel.OnMouseOut += HoverOut_Panel;
                    _rSInterface.Panel.OnLeftClick += LeftClick_RSPanel;
                    _rSInterface.Panel.OnRightClick += RightClick_RSPanel;
                    colorPanel.Append(_rSInterface);
                    //G
                    _gSInterface = new UIColorInterface(_tempSecondaryColor.Y, new Color(0f, 1f, 0f))
                    {
                        HAlign = 0.5f,
                        VAlign = 0.55f
                    };
                    _gSInterface.OnUpdate += Update_GSPanel;
                    _gSInterface.Panel.OnMouseOver += Hover_Panel;
                    _gSInterface.Panel.OnMouseOut += HoverOut_Panel;
                    _gSInterface.Panel.OnLeftClick += LeftClick_GSPanel;
                    _gSInterface.Panel.OnRightClick += RightClick_GSPanel;
                    colorPanel.Append(_gSInterface);
                    //B
                    _bSInterface = new UIColorInterface(_tempSecondaryColor.Z, new Color(0f, 0f, 1f))
                    {
                        HAlign = 0.5f,
                        VAlign = 0.9f
                    };
                    _bSInterface.OnUpdate += Update_BSPanel;
                    _bSInterface.Panel.OnMouseOver += Hover_Panel;
                    _bSInterface.Panel.OnMouseOut += HoverOut_Panel;
                    _bSInterface.Panel.OnLeftClick += LeftClick_BSPanel;
                    _bSInterface.Panel.OnRightClick += RightClick_BSPanel;
                    colorPanel.Append(_bSInterface);
                    break;
                case sliderTypes.Saturation:
                    colorPanel.Height.Set(54, 0);
                    _SatInterface = new UIColorInterface(_tempSaturation, new Color(1f, 1f, 1f))
                    {
                        HAlign = 0.5f,
                        VAlign = 0.65f
                    };
                    _SatInterface.OnUpdate += Update_SatPanel;
                    _SatInterface.Panel.OnMouseOver += Hover_Panel;
                    _SatInterface.Panel.OnMouseOut += HoverOut_Panel;
                    _SatInterface.Panel.OnLeftClick += LeftClick_SatPanel;
                    _SatInterface.Panel.OnRightClick += RightClick_SatPanel;
                    colorPanel.Append(_SatInterface);
                    break;
                case sliderTypes.Opacity:
                    colorPanel.Height.Set(54, 0);
                    _OpacityInterface = new UIColorInterface(_tempOpacity, new Color(1f, 1f, 1f))
                    {
                        HAlign = 0.5f,
                        VAlign = 0.65f
                    };
                    _OpacityInterface.OnUpdate += Update_OpacityPanel;
                    _OpacityInterface.Panel.OnMouseOver += Hover_Panel;
                    _OpacityInterface.Panel.OnMouseOut += HoverOut_Panel;
                    _OpacityInterface.Panel.OnLeftClick += LeftClick_OpacityPanel;
                    _OpacityInterface.Panel.OnRightClick += RightClick_OpacityPanel;
                    colorPanel.Append(_OpacityInterface);
                    break;
            }
        }

        private void AddCopyAndPaste(UIElement parent)
        {
            // copy button
            UIColoredImageButton copyButton = new UIColoredImageButton(Main.Assets.Request<Texture2D>("Images/UI/CharCreation/Copy"), isSmall: true)
            {
                VAlign = 0.95f,
                HAlign = 0f,
                Left = StyleDimension.FromPixelsAndPercent(0f, 0f)
            };
            copyButton.OnLeftMouseDown += Click_CopyString;
            parent.Append(copyButton);
            _copyTemplateButton = copyButton;

            // paste button
            UIColoredImageButton pasteButton = new UIColoredImageButton(Main.Assets.Request<Texture2D>("Images/UI/CharCreation/Paste"), isSmall: true)
            {
                VAlign = 0.95f,
                HAlign = 0f,
                Left = StyleDimension.FromPixelsAndPercent(40f, 0f)
            };
            pasteButton.OnLeftMouseDown += Click_PasteString;
            parent.Append(pasteButton);
            _pasteTemplateButton = pasteButton;

            // information text for various parts of the ui. mostly used by copy & paste
            _infoText = new UIText("", 0.8f)
            {
                VAlign = 1f,
                HAlign = 0f,
                Left = StyleDimension.FromPixelsAndPercent(0f, 0f),
                IgnoresMouseInteraction = true
            };
            _infoText.OnDraw += Update_InfoText;
            parent.Append(_infoText);
        }

        public void AddSearchBar(UIElement parent)
        {
            // template filter button
            _filterButton = new UIImageButton(ModContent.Request<Texture2D>("MagicDye/UI/Button_Filter", ReLogic.Content.AssetRequestMode.ImmediateLoad)) // ImmediateLoad is needed for proper width 
            {
                VAlign = 0.5f,
                HAlign = 0.03f,
            };
            _filterButton.OnLeftClick += LeftClick_CycleFilter;
            _filterButton.OnRightClick += RightClick_CycleFilter;
            _filterButton.SetHoverImage(Main.Assets.Request<Texture2D>("Images/UI/Bestiary/Button_Search_Border"));
            _filterButton.SetVisibility(1f, 1f);
            parent.Append(_filterButton);

            // template search button
            UIImageButton searchButton = new UIImageButton(Main.Assets.Request<Texture2D>("Images/UI/Bestiary/Button_Search"))
            {
                VAlign = 0.5f,
                HAlign = 0.03f,
                Left = StyleDimension.FromPixelsAndPercent(25f, 0f)
            };
            searchButton.OnLeftClick += Click_SearchArea;
            searchButton.SetHoverImage(Main.Assets.Request<Texture2D>("Images/UI/Bestiary/Button_Search_Border"));
            searchButton.SetVisibility(1f, 1f);
            parent.Append(searchButton);

            // template search bar container
            _searchBoxPanel = new UIPanel
            {
                Width = new StyleDimension(0.03f - _filterButton.Width.Pixels - searchButton.Width.Pixels - 3f, 0.95f),
                Height = new StyleDimension(0f, 1f),
                VAlign = 0.5f,
                HAlign = 1f
            };
            _searchBoxPanel.BackgroundColor = new Color(35, 40, 83);
            _searchBoxPanel.BorderColor = new Color(35, 40, 83);
            _searchBoxPanel.SetPadding(0f);
            _searchBoxPanel.OnLeftClick += Click_SearchArea;
            parent.Append(_searchBoxPanel);

            // template search bar
            _searchBar = new UISearchBar(Language.GetText("UI.PlayerNameSlot"), 0.8f)
            {
                Width = new StyleDimension(0f, 1f),
                Height = new StyleDimension(0f, 1f),
                HAlign = 0f,
                VAlign = 0.5f,
                Left = new StyleDimension(0f, 0f),
                IgnoresMouseInteraction = true
            };
            _searchBar.OnContentsChanged += ContentsChanged_Search;
            _searchBar.OnStartTakingInput += InputStart_Search;
            _searchBar.OnEndTakingInput += InputEnd_Search;
            _searchBar.OnNeedingVirtualKeyboard += VirtualKeyboard_Search;
            _searchBar.OnCanceledTakingInput += InputCanceled_Search;
            _searchBoxPanel.Append(_searchBar);

            // template search cancel button
            UIImageButton uIImageButton2 = new UIImageButton(Main.Assets.Request<Texture2D>("Images/UI/SearchCancel"))
            {
                HAlign = 1f,
                VAlign = 0.5f,
                Left = new StyleDimension(-2f, 0f)
            };
            uIImageButton2.OnMouseOver += Hover_searchCancelButton;
            uIImageButton2.OnLeftClick += Click_searchCancelButton;
            _searchBoxPanel.Append(uIImageButton2);
        }

        private void AddDyeList(UIElement parent)
        {
            // dye list scroll area
            UIPanel dyeListPanel = new UIPanel();
            dyeListPanel.Width.Set(260, 0);
            dyeListPanel.Height.Set(230, 0);
            dyeListPanel.HAlign = 0.96f;
            dyeListPanel.VAlign = 0.5f;
            dyeListPanel.PaddingLeft = 1f;
            dyeListPanel.BackgroundColor *= 0.8f;
            dyeListPanel.BorderColor *= 0.8f;
            parent.Append(dyeListPanel);

            // label
            UITextPanel<LocalizedText> labelText = new(Language.GetText("Mods.MagicDye.UI.Templates"), 0.7f)
            {
                VAlign = 0f,
                HAlign = 0.5f,
                Top = new StyleDimension(-37f, 0f)
            };
            dyeListPanel.Append(labelText);

            //search bar
            UIElement searchPanel = new UIElement
            {
                Height = new StyleDimension(24f, 0f),
                Width = new StyleDimension(0f, 1.5f)
            };
            searchPanel.SetPadding(0f);
            dyeListPanel.Append(searchPanel);

            // item list
            UIDynamicDyeCollection item = _dyeGrid = new UIDynamicDyeCollection();
            _filterer = new EntryFilterer<Item, IItemEntryFilter>();
            List<IItemEntryFilter> list = new List<IItemEntryFilter> {
                new ModFilters.VanillaDye(),
                new ModFilters.ModdedDye()
            };
            List<IItemEntryFilter> list2 = [.. list];
            _filterer.AddFilters(list2);
            _filterer.SetSearchFilterObject(new ItemFilters.BySearch());
            _sorter = new EntrySorter<int, ICreativeItemSortStep>();
            _sorter.AddSortSteps(new List<ICreativeItemSortStep>
            {
                new SortingSteps.ByCreativeSortingId(),
                new SortingSteps.Alphabetical()
            });
            _dyeGrid.OnLeftClick += LeftClick_DyeGrid;

            AddSearchBar(searchPanel);
            _searchBar.SetContents(null, forced: true);

            // dye list
            _dyeList = new UIList();
            _dyeList.HAlign = 0f;
            _dyeList.VAlign = 1f;
            _dyeList.Width.Set(0f, 1f);
            _dyeList.Height.Set(0f, 0.85f);
            _dyeList.ListPadding = 2f;
            dyeListPanel.Append(_dyeList);
            _dyeList.Add(item);

            //dye list scroll bar
            UIScrollbar dyeListScrollBar = new UIScrollbar();
            dyeListScrollBar.SetView(100f, 1000f);
            dyeListScrollBar.Height.Set(0f, 0.85f);
            dyeListScrollBar.HAlign = 1.05f;
            dyeListScrollBar.VAlign = 1f;
            _dyeList.SetScrollbar(dyeListScrollBar);
            dyeListPanel.Append(dyeListScrollBar);
        }

        private void AddPreviewPanel(UIElement parent)
        {
            // preview panel
            UIPanel previewPanel = new UIPanel();
            previewPanel.Width.Set(190, 0);
            previewPanel.Height.Set(326, 0);
            previewPanel.HAlign = 0f;
            previewPanel.VAlign = 0f;

            // buttons for swapping slots
            AddSlotPanel(previewPanel);
            parent.Append(previewPanel);

            // dye preview
            tempPlayer = new Player();
            UICharacter uiPlayer = new UICharacter(tempPlayer, false, false, 3f);
            uiPlayer.HAlign = 0.5f;
            uiPlayer.VAlign = 0.8f;
            previewPanel.Append(uiPlayer);
        }

        private void AddResearchPanel(UIElement parent)
        {
            // research panel
            UIPanel researchPanel = new UIPanel();
            researchPanel.Width.Set(158, 0);
            researchPanel.Height.Set(158, 0);
            researchPanel.HAlign = 0f;
            researchPanel.VAlign = 0.5f;
            parent.Append(researchPanel);

            // amount researched
            UIPanel amountTextBG = new UIPanel
            {
                Top = new StyleDimension(10f, 0f),
                Left = new StyleDimension(0f, 0f),
                HAlign = 0.5f,
                VAlign = 0.5f,
                Width = new StyleDimension(72f, 0f),
                Height = new StyleDimension(25f, 0f),
                IgnoresMouseInteraction = true,
            };
            amountTextBG.BackgroundColor = new Color(29, 33, 70, 255);
            amountTextBG.BorderColor = new Color(29, 33, 70, 255);
            UIText amountText = new UIText("", 0.8f)
            {
                HAlign = 0.5f,
                VAlign = 0.5f,
                IgnoresMouseInteraction = true
            };
            amountText.OnUpdate += Update_DescriptionText;
            amountTextBG.Append(amountText);
            researchPanel.Append(amountTextBG);

            // research button
            _researchButton = new UIPanel
            {
                Top = new StyleDimension(0f, 0f),
                Left = new StyleDimension(0f, 0f),
                HAlign = 0.5f,
                VAlign = 1f,
                Width = new StyleDimension(124f, 0f),
                Height = new StyleDimension(30f, 0f)
            };

            // research button text
            UIText researchText = new UIText(Language.GetText("CreativePowers.ConfirmInfiniteItemSacrifice"), 0.8f)
            {
                IgnoresMouseInteraction = true,
                HAlign = 0.5f,
                VAlign = 0.5f
            };
            researchText.OnUpdate += Update_ResearchText;
            _researchButton.Append(researchText);
            _researchButton.OnLeftClick += LeftClick_SacrificeButton;
            _researchButton.OnRightClick += RightClick_SacrificeButton;
            _researchButton.OnScrollWheel += Scroll_SacrificeButton;
            _researchButton.OnMouseOver += Hover_SacrificeButton;
            _researchButton.OnMouseOut += HoverOut_SacrificeButton;
            researchPanel.Append(_researchButton);

            // dye reserch slot
            _researchSlot = new VanillaItemSlotWrapper(ItemSlot.Context.ChestItem, ItemSlot.Context.DisplayDollDye, 1f)
            {
                ValidItemFunc = item => 
                {
                    if (item.IsAir || !item.IsAir && !(item.dye == 0) && !item.accessory && item.createTile == -1 && !item.consumable && item.damage == -1)
                    {
                        return true;
                    }
                    if ((item.dye == 0) || item.accessory || item.createTile != -1 || item.consumable || item.damage != -1)
                    {
                        _infoTimeout = 60 * 3;
                        _infoText.SetText(Language.GetText("Mods.MagicDye.UI.Information.ResearchInvalidTypeError"));
                        _infoText.Recalculate();
                        return false;
                    }
                    return false;
                }
            };

            _researchSlot.HAlign = 0.5f;
            _researchSlot.VAlign = 0f;
            researchPanel.Append(_researchSlot);
        }

        private void AddChangeAndCancel(UIElement parent)
        {
            // change text
            UITextButton change = new UITextButton(Language.GetText("GameUI.Change"), 0.9f);
            change.TextOriginY = 1f;
            change.Width.Set(100, 0);
            change.Height.Set(30, 0);
            change.HAlign = 0.4f;
            change.VAlign = 0.94f;
            change.TextColor = Color.Gray;
            change.OnMouseOver += Hover_TextButtons;
            change.OnMouseOut += HoverOut_TextButtons;
            change.OnLeftClick += Click_Change;
            change.OnUpdate += Update_TextButtons;
            parent.Append(change);

            // cancel text
            UITextButton cancel = new UITextButton(Language.GetText("GameUI.Cancel"), 0.9f);
            cancel.TextOriginY = 1f;
            cancel.Width.Set(100, 0);
            cancel.Height.Set(30, 0);
            cancel.HAlign = 0.6f;
            cancel.VAlign = 0.94f;
            cancel.TextColor = Color.Gray;
            cancel.OnMouseOver += Hover_TextButtons;
            cancel.OnMouseOut += HoverOut_TextButtons;
            cancel.OnLeftClick += Click_Cancel;
            cancel.OnUpdate += Update_TextButtons;
            parent.Append(cancel);
        }

        private void AddPasses(UIElement parent, float HAlign, float VAlign)
        {

            _passGrid = new MagicDyePassGrid(_passList);
            _passGrid.OnLeftClick += Click_ClosePassGrid;
            _passGrid.OnClickingOption += Click_PassOption;
            //_passGrid.OnMousingOverOption += UpdatePass;
            //_passGrid.OnMousingOffOption += ResetMousedPass;

            UIPanel passPanel = new()
            {
                Width = new StyleDimension(144f, 0f),
                Height = new StyleDimension(54f, 0f),
                HAlign = HAlign,
                VAlign = VAlign,
                PaddingTop = 0.05f,
                PaddingBottom = 0f,
            };
            parent.Append(passPanel);

            UITextPanel<LocalizedText> passLabel = new(Language.GetText("Mods.MagicDye.UI.Pass"), 0.7f)
            {
                VAlign = 0f,
                HAlign = 0.5f,
                Top = new StyleDimension(-20f, 0f)
            };
            passPanel.Append(passLabel);

            UIPanel passButton = new UIPanel()
            {
                PaddingTop = 0f,
                PaddingBottom = 0f,
                Left = new StyleDimension(0f, 0f),
                HAlign = 0.5f,
                VAlign = 0.65f,
                Width = new StyleDimension(passPanel.Width.Pixels - passPanel.PaddingRight - passPanel.PaddingLeft, 0f),
                Height = new StyleDimension(28f, 0f),
                OverflowHidden = true
            };
            passButton.OnLeftClick += OpenOrClosePassOptions;
            passButton.OnMouseOver += Hover_Panel;
            passButton.OnMouseOut += HoverOut_Panel;
            passPanel.Append(passButton);
            UIText passText = new UIText("", 0.8f)
            {
                Left = new StyleDimension(0f, 0f),
                Top = new StyleDimension(0f, 0f),
                VAlign = 0.5f,
                HAlign = 0f,
                TextOriginX = 0f,
                TextOriginY = 0f,
                IgnoresMouseInteraction = true
            };
            passText.OnUpdate += Update_PassText;
            passButton.Append(passText);
        }

        private void AddColorMod1(UIElement parent, float HAlign, float VAlign)
        {
            _colorMods1Grid = new MagicDyePassGrid(_colorModList);
            _colorMods1Grid.OnLeftClick += Click_CloseColorModGrid1;
            _colorMods1Grid.OnClickingOption += Click_ColotMod1Option;
            //_colorMods1Grid.OnMousingOverOption += UpdateColorMod1;
            //_colorMods1Grid.OnMousingOffOption += ResetMousedColorMod1;

            UIPanel ColorMod1Panel = new()
            {
                Width = new StyleDimension(144f, 0f),
                Height = new StyleDimension(54f, 0f),
                HAlign = HAlign,
                VAlign = VAlign,
                PaddingTop = 0.05f,
                PaddingBottom = 0f,
            };
            parent.Append(ColorMod1Panel);

            UITextPanel<LocalizedText> ColorMod1Label = new(Language.GetText("Mods.MagicDye.UI.ColorModOne"), 0.7f)
            {
                VAlign = 0f,
                HAlign = 0.5f,
                Top = new StyleDimension(-20f, 0f)
            };
            ColorMod1Panel.Append(ColorMod1Label);

            UIPanel ColorMods1 = new UIPanel()
            {
                PaddingTop = 0f,
                PaddingBottom = 0f,
                Left = new StyleDimension(0f, 0f),
                HAlign = 0.5f,
                VAlign = 0.65f,
                Width = new StyleDimension(ColorMod1Panel.Width.Pixels - ColorMod1Panel.PaddingRight - ColorMod1Panel.PaddingLeft, 0f),
                Height = new StyleDimension(28f, 0f),
                OverflowHidden = true
            };
            ColorMods1.OnLeftClick += OpenOrCloseColorModGrid1;
            ColorMods1.OnMouseOver += Hover_Panel;
            ColorMods1.OnMouseOut += HoverOut_Panel;
            ColorMod1Panel.Append(ColorMods1);
            UIText colorMods1Text = new UIText("", 0.8f)
            {
                Left = new StyleDimension(0f, 0f),
                Top = new StyleDimension(0f, 0f),
                VAlign = 0.5f,
                HAlign = 0f,
                TextOriginX = 0f,
                TextOriginY = 0f,
                IgnoresMouseInteraction = true
            };
            colorMods1Text.OnUpdate += Update_ColotMod1Text;
            ColorMods1.Append(colorMods1Text);
        }

        private void AddColorMod2(UIElement parent, float HAlign, float VAlign)
        {
            _colorMods2Grid = new MagicDyePassGrid(_colorModList);
            _colorMods2Grid.OnLeftClick += Click_CloseColorModGrid2;
            _colorMods2Grid.OnClickingOption += Click_ColorMod2Option;
            //_colorMods2Grid.OnMousingOverOption += UpdateColorMod2;
            //_colorMods2Grid.OnMousingOffOption += ResetMousedColorMod2;

            UIPanel ColorMod2Panel = new()
            {
                Width = new StyleDimension(144f, 0f),
                Height = new StyleDimension(54f, 0f),
                HAlign = HAlign,
                VAlign = VAlign,
                PaddingTop = 0.05f,
                PaddingBottom = 0f,
            };
            parent.Append(ColorMod2Panel);

            UITextPanel<LocalizedText> ColorMod2Label = new(Language.GetText("Mods.MagicDye.UI.ColorModTwo"), 0.7f)
            {
                VAlign = 0f,
                HAlign = 0.5f,
                Top = new StyleDimension(-20f, 0f)
            };
            ColorMod2Panel.Append(ColorMod2Label);

            UIPanel ColorMods2 = new UIPanel()
            {
                PaddingTop = 0f,
                PaddingBottom = 0f,
                Left = new StyleDimension(0f, 0f),
                HAlign = 0.5f,
                VAlign = 0.65f,
                Width = new StyleDimension(ColorMod2Panel.Width.Pixels - ColorMod2Panel.PaddingRight - ColorMod2Panel.PaddingLeft, 0f),
                Height = new StyleDimension(28f, 0f),
                OverflowHidden = true
            };
            ColorMods2.OnLeftClick += OpenOrCloseColorModGrid2;
            ColorMods2.OnMouseOver += Hover_Panel;
            ColorMods2.OnMouseOut += HoverOut_Panel;
            ColorMod2Panel.Append(ColorMods2);
            UIText colorMods2Text = new UIText("", 0.8f)
            {
                Left = new StyleDimension(0f, 0f),
                Top = new StyleDimension(0f, 0f),
                VAlign = 0.5f,
                HAlign = 0f,
                TextOriginX = 0f,
                TextOriginY = 0f,
                IgnoresMouseInteraction = true
            };
            colorMods2Text.OnUpdate += Update_ColorMod2Text;
            ColorMods2.Append(colorMods2Text);
        }

        private void AddCPPageElements(UIElement parent)
        {
            ReturnResearchItems();
            AddColorPanel(parent, 0.25f, 0.1f, sliderTypes.Primary);
            AddColorPanel(parent, 0.75f, 0.1f, sliderTypes.Secondary);
            AddColorPanel(parent, 0.25f, 0.65f, sliderTypes.Saturation);
            AddColorPanel(parent, 0.75f, 0.65f, sliderTypes.Opacity);
            AddPasses(parent, 0f, 1f);
            AddColorMod1(parent, 0.5f, 1f);
            AddColorMod2(parent, 1f, 1f);
        }

        private void AddRTPageElements(UIElement parent)
        {
            AddResearchPanel(parent);
            AddDyeList(parent);
        }

        private void AddPageButtons(UIElement parent)
        {
            AddCPPageElements(_pagePanel);
            _currentPage = 1;

            // color & pass button
            UIColoredImageButton CPButton = new UIColoredImageButton(ModContent.Request<Texture2D>("MagicDye/UI/Button_Color"), isSmall: true)
            {
                HAlign = 0.315f,
                VAlign = 0.81f,
                Left = StyleDimension.FromPixelsAndPercent(0f, 0f)
            };
            CPButton.OnLeftClick += Click_CPButton;
            CPButton.SetSelected(true);
            parent.Append(CPButton);
            _colorAndPassesButton = CPButton;

            // research & templates button
            UIColoredImageButton RTButton = new UIColoredImageButton(ModContent.Request<Texture2D>("MagicDye/UI/Button_Research"), isSmall: true)
            {
                HAlign = 0.315f,
                VAlign = 0.81f,
                Left = new StyleDimension(CPButton.Width.Pixels + 4f, 0f),
            };
            RTButton.OnLeftClick += Click_RTButton;
            parent.Append(RTButton);
            _researchAndTemplatesButton = RTButton;
        }

        private void BuildUI()
        {
            // main BG
            _BG = new UIPanel();
            _BG.Width.Set(678, 0);
            _BG.Height.Set(450, 0);
            _BG.BackgroundColor = new Color(33, 15, 91, 255) * 0.685f;
            _BG.HAlign = 0.5f;
            _BG.VAlign = 0.5f;
            Append(_BG);

            // page panel
            _pagePanel = new UIPanel();
            _pagePanel.Width.Set(462, 0);
            _pagePanel.Height.Set(326, 0);
            _pagePanel.HAlign = 1f;
            _pagePanel.VAlign = 0f;
            _BG.Append(_pagePanel);

            // update research & pass lists
            UpdateDyesList();

            // page buttons
            AddPageButtons(_BG);

            // dye preview
            AddPreviewPanel(_BG);

            //copy & paste area
            AddCopyAndPaste(_BG);

            // change and cancel buttons
            AddChangeAndCancel(_BG);
        }

        // Misc Events & Functions
        private void UpdateDyeVars(int itemID, bool noColor)
        {

            var shaderDataInfo = GetDyeInformation(itemID);
            FieldInfo shaderPass = typeof(ShaderData).GetField("_passName", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Static);
            string PassName = (string)shaderPass.GetValue(GameShaders.Armor.GetShaderFromItemId(itemID));
            var UIPlayer = tempPlayer.GetModPlayer<MagicDyePlayer>();
            if (UIPlayer.MagicDyePrimaryColors == null)
            {
                UIPlayer.Initialize();
            }
            if (!noColor)
            {
                _tempPrimaryColor = new Vector3(shaderDataInfo.uColor.X, shaderDataInfo.uColor.Y, shaderDataInfo.uColor.Z);
                _tempSecondaryColor = new Vector3(shaderDataInfo.uSecondaryColor.X, shaderDataInfo.uSecondaryColor.Y, shaderDataInfo.uSecondaryColor.Z);
                _tempSaturation = shaderDataInfo.uSaturation;
                _tempOpacity = shaderDataInfo.uOpacity;
                UIPlayer.MagicDyePrimaryColors[_workingSlot] = shaderDataInfo.uColor;
                UIPlayer.MagicDyeSecondaryColors[_workingSlot] = shaderDataInfo.uSecondaryColor;
                UIPlayer.MagicDyeSaturation[_workingSlot] = shaderDataInfo.uSaturation;
                UIPlayer.MagicDyeOpacity[_workingSlot] = shaderDataInfo.uOpacity;
                ColorModifier tempMod = MagicDye.instance.colorModifiers.Find(x => x.itemType == itemID);
                if (tempMod != null)
                {
                    UIPlayer.MagicDyeColorMod1[_workingSlot] = tempMod.name;
                    _tempColorMod1 = tempMod.name;
                }
                else
                {
                    UIPlayer.MagicDyeColorMod1[_workingSlot] = "None";
                    _tempColorMod1 = "None";
                }
                UIPlayer.MagicDyeColorMod2[_workingSlot] = "None";
                _tempColorMod2 = "None";
            }
            _tempPass = shaderDataInfo.passName;
            UIPlayer.MagicDyeItem[_workingSlot] = new Item(itemID);
            UIPlayer.MagicDyePasses[_workingSlot] = shaderDataInfo.passName;
        }

        private void OnCloseReset() // shoddy attempt at fixing a shader swapping issue. didn't work.
        {
            var MainPlayer = Main.LocalPlayer.GetModPlayer<MagicDyePlayer>();

            _tempPass = "ArmorColored";


            tempPlayer = null;
            _workingSlot = 0;

        }

        private void OpenOrClosePassOptions(UIMouseEvent evt, UIElement listeningElement)
        {
            if (_passGrid.Parent != null)
            {
                ClosePassGrid();
                return;
            }
            _BG.RemoveChild(_passGrid);
            _BG.Append(_passGrid);
            _passGrid.StoredPass = _tempPass;
            if (_passGrid != null && _passGrid.LastSelected == null)
            {
                _passGrid.LastSelected = _passGrid.GetGridItemByString(_tempPass);
                if (_passGrid.LastSelected != null)
                {
                    ((UIText)_passGrid.GetGridItemByString(_tempPass)).TextColor = Color.Yellow;
                }
            }
        }

        private void OpenOrCloseColorModGrid1(UIMouseEvent evt, UIElement listeningElement)
        {
            if (_colorMods1Grid.Parent != null)
            {
                CloseColorModGrid1();
                return;
            }
            _BG.RemoveChild(_colorMods1Grid);
            _BG.Append(_colorMods1Grid);
            _colorMods1Grid.StoredPass = _tempColorMod1;
            if (_colorMods1Grid != null && _colorMods1Grid.LastSelected == null)
            {
                _colorMods1Grid.LastSelected = _colorMods1Grid.GetGridItemByString(_tempColorMod1);
                if (_colorMods1Grid.LastSelected != null)
                {
                    ((UIText)_colorMods1Grid.GetGridItemByString(_tempColorMod1)).TextColor = Color.Yellow;
                }
            }
        }

        private void OpenOrCloseColorModGrid2(UIMouseEvent evt, UIElement listeningElement)
        {
            if (_colorMods2Grid.Parent != null)
            {
                CloseColorModGrid2();
                return;
            }
            _BG.RemoveChild(_colorMods2Grid);
            _BG.Append(_colorMods2Grid);
            _colorMods2Grid.StoredPass = _tempColorMod2;
            if (_colorMods2Grid != null && _colorMods2Grid.LastSelected == null)
            {
                _colorMods2Grid.LastSelected = _colorMods2Grid.GetGridItemByString(_tempColorMod2);
                if (_colorMods2Grid.LastSelected != null)
                {
                    ((UIText)_colorMods2Grid.GetGridItemByString(_tempColorMod2)).TextColor = Color.Yellow;
                }
            }
        }

        private void InitPreviewPlayer()
        {
            var UIPlayer = tempPlayer.GetModPlayer<MagicDyePlayer>();
            var MainPlayer = Main.LocalPlayer.GetModPlayer<MagicDyePlayer>();
            for (int i = 0; i < 20; i++)
            {
                tempPlayer.armor[i] = Main.LocalPlayer.armor[i];
            }
            // if not wearing any armor, give the preview dye robes
            if (tempPlayer.armor[0].IsAir && tempPlayer.armor[1].IsAir && tempPlayer.armor[2].IsAir && tempPlayer.armor[10].IsAir && tempPlayer.armor[11].IsAir && tempPlayer.armor[12].IsAir)
            {
                tempPlayer.armor[10] = new Item(ItemID.DyeTraderTurban);
                tempPlayer.armor[11] = new Item(ItemID.DyeTraderRobe);
            }

            for (int i = 0; i < 5; i++)
            {
                tempPlayer.miscEquips[i] = Main.LocalPlayer.miscEquips[i];
            }

            if (UIPlayer.MagicDyePrimaryColors == null)
            {
                UIPlayer.Initialize();
            }

            CopyColors(MainPlayer, UIPlayer);

            UpdatePreview(tempPlayer, _workingSlot);

            tempPlayer.team = Main.LocalPlayer.team;
        }

        private void ResetPass(string str)
        {
            if (_passGrid.StoredPass != null)
            {

                if (_passDictionary.TryGetValue(str, out int value))
                {
                    UpdateDyeVars(value, true);
                    UpdateColorValues();
                }
            }
        }

        private void ClosePassGrid()
        {
            _BG.RemoveChild(_passGrid);
        }

        private void CloseColorModGrid1()
        {
            if (_colorMods1Grid.StoredPass != null)
            {
                _tempColorMod1 = _colorMods1Grid.StoredPass;
            }
            _BG.RemoveChild(_colorMods1Grid);
        }

        private void CloseColorModGrid2()
        {
            if (_colorMods2Grid.StoredPass != null)
            {
                _tempColorMod2 = _colorMods2Grid.StoredPass;
            }
            _BG.RemoveChild(_colorMods2Grid);
        }

        public void ReturnResearchItems()
        {
            if (_researchSlot != null && !_researchSlot.Item.IsAir)
            {
                // allows items to be returned without pickup text 
                GetItemSettings getItemInDropItemCheck = GetItemSettings.GetItemInDropItemCheck;
                Item oldItem = Main.LocalPlayer.GetItem(Main.LocalPlayer.whoAmI, _researchSlot.Item, getItemInDropItemCheck);
                Item newItem = Main.LocalPlayer.QuickSpawnClonedItemDirect(Main.LocalPlayer.GetSource_Misc("PlayerDropItemCheck"), oldItem, oldItem.stack);
                newItem.newAndShiny = false;

                _researchSlot.Item = new Item();
            }
        }
        private void UpdatePreview(Player playerEntity, int slot)
        {

            var UIPlayer = playerEntity.GetModPlayer<MagicDyePlayer>();

            for (int i = 0; i < 10; i++)
            {
                playerEntity.dye[i] = new Item(MagicDye.dyeLookUp[slot]);
            }

            for (int i = 0; i < 5; i++)
            {
                playerEntity.miscDyes[i] = new Item(MagicDye.dyeLookUp[slot]);
            }


            _tempPrimaryColor = new Vector3(UIPlayer.MagicDyePrimaryColors[_workingSlot].X, UIPlayer.MagicDyePrimaryColors[_workingSlot].Y, UIPlayer.MagicDyePrimaryColors[_workingSlot].Z);
            _tempSecondaryColor = new Vector3(UIPlayer.MagicDyeSecondaryColors[_workingSlot].X, UIPlayer.MagicDyeSecondaryColors[_workingSlot].Y, UIPlayer.MagicDyeSecondaryColors[_workingSlot].Z);
            _tempSaturation = UIPlayer.MagicDyeSaturation[_workingSlot];
            _tempOpacity = UIPlayer.MagicDyeOpacity[_workingSlot];
            _tempPass = UIPlayer.MagicDyePasses[_workingSlot];
            _tempColorMod1 = UIPlayer.MagicDyeColorMod1[_workingSlot];
            _tempColorMod2 = UIPlayer.MagicDyeColorMod2[_workingSlot];
            if (_rPInterface != null)
            {
                _rPInterface.Value = UIPlayer.MagicDyePrimaryColors[_workingSlot].X;
                _gPInterface.Value = UIPlayer.MagicDyePrimaryColors[_workingSlot].Y;
                _bPInterface.Value = UIPlayer.MagicDyePrimaryColors[_workingSlot].Z;
            }
            if (_rSInterface != null)
            {
                _rSInterface.Value = UIPlayer.MagicDyeSecondaryColors[_workingSlot].X;
                _gSInterface.Value = UIPlayer.MagicDyeSecondaryColors[_workingSlot].Y;
                _bSInterface.Value = UIPlayer.MagicDyeSecondaryColors[_workingSlot].Z;
            }
            if (_SatInterface != null)
            {
                _SatInterface.Value = UIPlayer.MagicDyeSaturation[_workingSlot];
            }
            if (_OpacityInterface != null)
            {
                _OpacityInterface.Value = UIPlayer.MagicDyeOpacity[_workingSlot];
            }
        }

        private void UpdateWorkingSlot(int value)
        {
            var tempNum = _workingSlot + value;
            if (tempNum < 0)
            {
                tempNum = MagicDye.dyeLookUp.Count() - 1;
            }
            if (tempNum > MagicDye.dyeLookUp.Count() - 1)
            {
                tempNum = 0;
            }
            _workingSlot = tempNum;
            UpdatePreview(tempPlayer, _workingSlot);
        }

        private void UpdateDyesList()
        {
            if (_itemIdsAvailableTotal != null)
            {
                _itemIdsAvailableTotal.Clear();

                Main.LocalPlayerCreativeTracker.ItemSacrifices.FillListOfItemsThatCanBeObtainedInfinitely(_itemIdsAvailableTotal);

                for (int i = _itemIdsAvailableTotal.Count - 1; i > -1; i--)
                {
                    if (ContentSamples.ItemsByType[_itemIdsAvailableTotal[i]].dye == 0 || ContentSamples.ItemsByType[_itemIdsAvailableTotal[i]].ModItem?.Mod == MagicDye.instance || ContentSamples.ItemsByType[_itemIdsAvailableTotal[i]].accessory || ContentSamples.ItemsByType[_itemIdsAvailableTotal[i]].consumable || ContentSamples.ItemsByType[_itemIdsAvailableTotal[i]].createTile != -1 || ContentSamples.ItemsByType[_itemIdsAvailableTotal[i]].damage != -1)
                    {
                        _itemIdsAvailableTotal.RemoveAt(i);
                    }
                }
                BuildModList(_itemIdsAvailableTotal);
                BuildPassList(_itemIdsAvailableTotal);
                BuildColorModList(_itemIdsAvailableTotal);

                if (_dyeGrid != null)
                {
                    _itemIdsAvailableToShow.Clear();
                    _itemIdsAvailableToShow.AddRange(_itemIdsAvailableTotal.Where((int x) => _filterer.FitsFilter(ContentSamples.ItemsByType[x])));
                    _itemIdsAvailableToShow.Sort(_sorter);
                    _dyeGrid.SetContentsToShow(_itemIdsAvailableToShow);
                }
            }
        }

        private void BuildModList(List<int> itemList)
        {
            _modList = new List<string>();
            for (int i = 0; i < itemList.Count; i++)
            {
                if (ContentSamples.ItemsByType[itemList[i]].ModItem != null)
                {
                    string modName = ContentSamples.ItemsByType[itemList[i]].ModItem.Mod.DisplayNameClean;
                    if (!_modList.Contains(modName))
                        _modList.Add(modName);
                }
            }
            _modList.RemoveAll(s => string.IsNullOrWhiteSpace(s));
            _modList.Sort();
        }

        private void BuildPassList(List<int> itemList)
        {
            _passList = new List<string>();
            _passDictionary = new Dictionary<string, int>();
            for (int i = 0; i < itemList.Count; i++)
            {
                string passName = GetDyeInformation(itemList[i]).passName;
                if (!_passList.Contains(passName))
                {
                    _passList.Add(passName);
                    _passDictionary.Add(passName, itemList[i]);
                }

            }
            _passList.RemoveAll(s => string.IsNullOrWhiteSpace(s));
            _passList.Sort();
        }

        private void BuildColorModList(List<int> itemList)
        {
            _colorModList = new List<string>();
            for (int i = 0; i < itemList.Count; i++)
            {
                ColorModifier tempMod = MagicDye.instance.colorModifiers.Find(x => x.itemType == itemList[i]);
                if (tempMod != null)
                {
                    if (!_colorModList.Contains(tempMod.name))
                    {
                        _colorModList.Add(tempMod.name);
                    }
                }
            }
            _colorModList.RemoveAll(s => string.IsNullOrWhiteSpace(s));
            _colorModList.Sort();
            _colorModList.Insert(0, "None");
        }

        private void UpdateColorValues()
        {
            var UIPlayer = tempPlayer.GetModPlayer<MagicDyePlayer>();
            if (UIPlayer.MagicDyePrimaryColors == null)
            {
                UIPlayer.Initialize();
            }

            UIPlayer.MagicDyePrimaryColors[_workingSlot].X = _tempPrimaryColor.X;
            UIPlayer.MagicDyePrimaryColors[_workingSlot].Y = _tempPrimaryColor.Y;
            UIPlayer.MagicDyePrimaryColors[_workingSlot].Z = _tempPrimaryColor.Z;

            UIPlayer.MagicDyeSecondaryColors[_workingSlot].X = _tempSecondaryColor.X;
            UIPlayer.MagicDyeSecondaryColors[_workingSlot].Y = _tempSecondaryColor.Y;
            UIPlayer.MagicDyeSecondaryColors[_workingSlot].Z = _tempSecondaryColor.Z;

            UIPlayer.MagicDyeSaturation[_workingSlot] = _tempSaturation;
            UIPlayer.MagicDyeOpacity[_workingSlot] = _tempOpacity;
            UIPlayer.MagicDyePasses[_workingSlot] = _tempPass;
            UIPlayer.MagicDyeColorMod1[_workingSlot] = _tempColorMod1;
            UIPlayer.MagicDyeColorMod2[_workingSlot] = _tempColorMod2;

        }

        private void CopyColors(MagicDyePlayer doner, MagicDyePlayer receiver)
        {
            for (int i = 0; i < MagicDye.dyeLookUp.Count(); i++)
            {
                receiver.MagicDyePrimaryColors[i] = doner.MagicDyePrimaryColors[i];
                receiver.MagicDyeSecondaryColors[i] = doner.MagicDyeSecondaryColors[i];
                receiver.MagicDyeSaturation[i] = doner.MagicDyeSaturation[i];
                receiver.MagicDyeOpacity[i] = doner.MagicDyeOpacity[i];
                receiver.MagicDyePasses[i] = doner.MagicDyePasses[i];
                receiver.MagicDyeColorMod1[i] = doner.MagicDyeColorMod1[i];
                receiver.MagicDyeColorMod2[i] = doner.MagicDyeColorMod2[i];
                receiver.MagicDyeItem = doner.MagicDyeItem;
            }
        }

        private (Vector3 uColor, Vector3 uSecondaryColor, float uSaturation, float uOpacity, string passName) GetDyeInformation(int DyeID)
        {
            ArmorShaderData DataRef = GameShaders.Armor.GetShaderFromItemId(DyeID);

            Vector3 uColor = (Vector3)typeof(ArmorShaderData).GetField("_uColor", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Static).GetValue(DataRef);
            Vector3 uSecondaryColor = (Vector3)typeof(ArmorShaderData).GetField("_uSecondaryColor", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Static).GetValue(DataRef);
            float uSaturation = (float)typeof(ArmorShaderData).GetField("_uSaturation", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Static).GetValue(DataRef);
            float uOpacity = (float)typeof(ArmorShaderData).GetField("_uOpacity", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Static).GetValue(DataRef);
            string passName = (string)typeof(ShaderData).GetField("_passName", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Static).GetValue(DataRef);

            return (uColor, uSecondaryColor, uSaturation, uOpacity, passName);
        }

        private void UpdateResearchSlotDye(int value)
        {
            var tempNum = _researchSlotButtonValue + value;
            if (tempNum < 0)
            {
                tempNum = MagicDye.dyeLookUp.Count() - 1;
            }
            if (tempNum > MagicDye.dyeLookUp.Count() - 1)
            {
                tempNum = 0;
            }
            _researchSlotButtonValue = tempNum;
            int stack = _researchSlot.Item.stack;
            _researchSlot.Item = new Item(MagicDye.dyeLookUp[tempNum]);
            _researchSlot.Item.stack = stack;
            _researchSlot.Item.newAndShiny = false;
        }

        public bool ShouldDrawSacrificeArea()
        {
            if (!_researchSlot.Item.IsAir)
                return true;

            Item mouseItem = Main.mouseItem;
            if (mouseItem.IsAir)
                return false;

            if (!CreativeItemSacrificesCatalog.Instance.TryGetSacrificeCountCapToUnlockInfiniteItems(mouseItem.type, out var amountNeeded))
                return false;

            if (Main.LocalPlayerCreativeTracker.ItemSacrifices.GetSacrificeCount(mouseItem.type) < amountNeeded)
                return true;

            return false;
        }

        private void RememberDyeSacrifice(int itemId, int amountWeHave, int amountWeNeedTotal)
        {
            _lastItemIdSacrificed = itemId;
            _lastItemAmountWeHad = amountWeHave;
            _lastItemAmountWeNeededTotal = amountWeNeedTotal;
        }

        private void ForgetDyeSacrifice()
        {
            _lastItemIdSacrificed = 0;
            _lastItemAmountWeHad = 0;
            _lastItemAmountWeNeededTotal = 0;
        }

        public bool GetSacrificeNumbers(out int itemIdChecked, out int amountWeHave, out int amountNeededTotal)
        {
            amountWeHave = 0;
            amountNeededTotal = 0;
            itemIdChecked = 0;
            Item item = _researchSlot.Item;
            if (!item.IsAir)
                itemIdChecked = item.type;
            if (item.ModItem?.Mod == MagicDye.instance)
                return false;
            if (!Main.LocalPlayerCreativeTracker.ItemSacrifices.TryGetSacrificeNumbers(item.type, out amountWeHave, out amountNeededTotal))
                return false;

            return true;
        }

        public void SacrificeWhatYouCan()
        {
            GetSacrificeNumbers(out int itemIdChecked, out int amountWeHave, out int amountNeededTotal);
            int amountWeSacrificed;
            switch (SacrificeDye(out amountWeSacrificed))
            {
                case CreativeUI.ItemSacrificeResult.SacrificedAndDone:
                    RememberDyeSacrifice(itemIdChecked, amountWeHave + amountWeSacrificed, amountNeededTotal);
                    break;
                case CreativeUI.ItemSacrificeResult.SacrificedButNotDone:
                    RememberDyeSacrifice(itemIdChecked, amountWeHave + amountWeSacrificed, amountNeededTotal);
                    break;
            }
        }

        public ItemSacrificeResult SacrificeDye(out int amountWeSacrificed)
        {
            return SacrificeDye(ref _researchSlot.Item, out amountWeSacrificed);
        }

        public unsafe ItemSacrificeResult SacrificeDye(ref Item item, out int amountWeSacrificed, bool returnRemainderToPlayer = false)
        {
            int amountNeededTotal = 0;
            int amountWeHave = 0;
            amountWeSacrificed = 0;


            if (_researchSlot.Item.ModItem?.Mod == MagicDye.instance)
                return ItemSacrificeResult.CannotSacrifice;

            if (!ItemLoader.CanResearch(item))
                return ItemSacrificeResult.CannotSacrifice;

            if (!Main.LocalPlayerCreativeTracker.ItemSacrifices.TryGetSacrificeNumbers(item.type, out amountWeHave, out amountNeededTotal))
                return ItemSacrificeResult.CannotSacrifice;

            int num = Utils.Clamp(amountNeededTotal - amountWeHave, 0, amountNeededTotal);
            if (num == 0)
                return ItemSacrificeResult.CannotSacrifice;

            int num2 = Math.Min(num, item.stack);
            if (!Main.ServerSideCharacter)
            {
                Main.LocalPlayerCreativeTracker.ItemSacrifices.RegisterItemSacrifice(item.type, num2);
            }
            else
            {
                NetPacket packet = NetCreativeUnlocksPlayerReportModule.SerializeSacrificeRequest(item.type, num2);
                NetManager.Instance.SendToServerOrLoopback(packet);
            }

            bool num3 = num2 == num;

            ItemLoader.OnResearched(item, num3);

            item.stack -= num2;
            if (item.stack <= 0)
                item.TurnToAir();
            SoundEngine.PlaySound(SoundID.ResearchComplete);
            UpdateDyesList();
            amountWeSacrificed = num2;
            if (item.stack > 0 && returnRemainderToPlayer)
            {
                item.position.X = Main.player[Main.myPlayer].Center.X - (float)(item.width / 2);
                item.position.Y = Main.player[Main.myPlayer].Center.Y - (float)(item.height / 2);
                item = Main.LocalPlayer.GetItem(Main.myPlayer, item, GetItemSettings.InventoryUIToInventorySettings);
            }

            if (!num3)
                return ItemSacrificeResult.SacrificedButNotDone;

            return ItemSacrificeResult.SacrificedAndDone;
        }

        private void IncrementFilterIndex(int value)
        {
            _currentFilterIndex = _currentFilterIndex + value;
            if (_currentFilterIndex < -1)
            {
                _currentFilterIndex = 1;
            }
            if (_currentFilterIndex > 1)
            {
                _currentFilterIndex = -1;
            }
        }

        private void FilterStep(int value)
        {
            IncrementFilterIndex(value);
            _filterer.ActiveFilters.Clear();
            if (_currentFilterIndex != -1)
            {
                _filterer.ToggleFilter(_currentFilterIndex);
            }
        }

        private void UpdateFilter(int value)
        {
            if (_currentFilterIndex != 1)
            {
                if (_modList.Count == 0)
                {
                    return;
                }
                FilterStep(value);
                if (_currentFilterIndex == 1)
                {
                    _modListIndex = value == -1 ? _modList.Count - 1 : 0;
                    ModFilters.ModdedDye moddedFilter = (ModFilters.ModdedDye)_filterer.AvailableFilters[_currentFilterIndex];
                    moddedFilter.ModName = _modList[_modListIndex];
                }
            }
            else
            {
                _modListIndex = _modListIndex + value;
                if (_modListIndex > -1 && _modListIndex < _modList.Count)
                {
                    ModFilters.ModdedDye moddedFilter = (ModFilters.ModdedDye)_filterer.AvailableFilters[_currentFilterIndex];
                    moddedFilter.ModName = _modList[_modListIndex];
                }
                else
                {
                    _modListIndex = value == -1 ? _modList.Count - 1 : 0;
                    FilterStep(value);
                }
            }


            UpdateDyesList();
        }

        // Debug Functions
        private void PrintInformation()
        {
            var MainPlayer = Main.LocalPlayer.GetModPlayer<MagicDyePlayer>();
            Main.NewText($"primary color: r{MainPlayer.MagicDyePrimaryColors[_workingSlot].X}, g{MainPlayer.MagicDyePrimaryColors[_workingSlot].Y}, b{MainPlayer.MagicDyePrimaryColors[_workingSlot].Z}");
            Main.NewText($"saturation: {MainPlayer.MagicDyeSaturation[_workingSlot]}f");
            Main.NewText($"pass: {MainPlayer.MagicDyePasses[_workingSlot]}");
            Main.NewText($"color mod 1: {MainPlayer.MagicDyeColorMod1[_workingSlot]}");
            Main.NewText($"color mod 2: {MainPlayer.MagicDyeColorMod2[_workingSlot]}");
            Main.NewText($"Item ID: {MainPlayer.MagicDyeItem[_workingSlot].type}");
            Main.NewText($"{MagicDye.GetItemIDFromPass(MainPlayer.MagicDyePasses[_workingSlot])}");
        }

        // these go unused due to breaking dye previews.
        private void ResetMousedPass(UIMouseEvent evt, UIElement listeningElement)
        {
            if (_passGrid.StoredPass != null)
            {
                _tempPass = _passGrid.StoredPass;

                ResetPass(_passGrid.StoredPass);
            }
        }

        private void UpdatePass(UIMouseEvent evt, UIElement listeningElement)
        {
            UIText element = ((UIText)listeningElement);
            _tempPass = element.Text;

            ResetPass(element.Text);
        }

        private void ResetMousedColorMod1(UIMouseEvent evt, UIElement listeningElement)
        {
            if (_colorMods1Grid.StoredPass != null)
            {
                _tempColorMod1 = _colorMods1Grid.StoredPass;
            }
        }

        private void UpdateColorMod1(UIMouseEvent evt, UIElement listeningElement)
        {
            UIText element = ((UIText)listeningElement);
            _tempColorMod1 = element.Text;
        }

        private void ResetMousedColorMod2(UIMouseEvent evt, UIElement listeningElement)
        {
            if (_colorMods2Grid.StoredPass != null)
            {
                _tempColorMod2 = _colorMods2Grid.StoredPass;
            }
        }

        private void UpdateColorMod2(UIMouseEvent evt, UIElement listeningElement)
        {
            UIText element = ((UIText)listeningElement);
            _tempColorMod2 = element.Text;
        }


        // Click Events
        private void Click_Change(UIMouseEvent evt, UIElement listeningElement)
        {
            // currently only tests functionality
            var UIPlayer = tempPlayer.GetModPlayer<MagicDyePlayer>();
            var MainPlayer = Main.LocalPlayer.GetModPlayer<MagicDyePlayer>();
            CopyColors(UIPlayer, MainPlayer);

            //PrintInformation()
            SoundEngine.PlaySound(SoundID.MenuTick);
        }

        private void Click_Cancel(UIMouseEvent evt, UIElement listeningElement)
        {
            interfaceRef = ModContent.GetInstance<MagicDyeUISystem>();
            SoundEngine.PlaySound(SoundID.MenuClose);
            OnCloseReset();
            interfaceRef.HideMagicDyeUI();
        }

        private void Click_SlotBack(UIMouseEvent evt, UIElement listeningElement)
        {
            UpdateWorkingSlot(-1);
        }

        private void Click_SlotForward(UIMouseEvent evt, UIElement listeningElement)
        {
            UpdateWorkingSlot(1);
        }

        private void Click_CopyString(UIMouseEvent evt, UIElement listeningElement)
        {
            SoundEngine.PlaySound(SoundID.MenuTick);
            string text = JsonHelper.SerializeWithCustomIndenting(new Dictionary<string, object> {
                { "version", 1 },
                { "primaryColor", new List<float>(){ (float)Math.Round(_tempPrimaryColor.X, 3), (float)Math.Round(_tempPrimaryColor.Y, 3), (float)Math.Round(_tempPrimaryColor.Z, 3) } },
                { "secondaryColor", new List<float>(){ (float)Math.Round(_tempSecondaryColor.X, 3), (float)Math.Round(_tempSecondaryColor.Y, 3), (float)Math.Round(_tempSecondaryColor.Z, 3) } },
                { "saturation", (float)Math.Round(_tempSaturation, 3)},
                { "opacity", (float)Math.Round(_tempOpacity, 3) },
                { "pass", _tempPass },
                { "color modifier 1", _tempColorMod1 },
                { "color modifier 2", _tempColorMod2 },

            });
            Platform.Get<IClipboard>().Value = text;
            _infoTimeout = 60 * 3;
            _infoText.SetText(Language.GetText("Mods.MagicDye.UI.Information.CopySuccess"));
            _infoText.Recalculate();
        }

        private void Click_PasteString(UIMouseEvent evt, UIElement listeningElement)
        {
            SoundEngine.PlaySound(SoundID.MenuTick);
            var UIPlayer = tempPlayer.GetModPlayer<MagicDyePlayer>();
            if (UIPlayer.MagicDyePrimaryColors == null)
            {
                UIPlayer.Initialize();
            }
            string value = Platform.Get<IClipboard>().Value;
            int num = value.IndexOf("{");
            if (num == -1)
            {
                _infoTimeout = 60 * 3;
                _infoText.SetText(Language.GetText("Mods.MagicDye.UI.Information.PasteInvalidJSONError"));
                _infoText.Recalculate();
                return;
            }

            value = value.Substring(num);
            int num2 = value.LastIndexOf("}");
            if (num2 == -1)
            {
                _infoTimeout = 60 * 3;
                _infoText.SetText(Language.GetText("Mods.MagicDye.UI.Information.PasteInvalidJSONError"));
                _infoText.Recalculate();
                return;
            }

            value = value.Substring(0, num2 + 1);
            Dictionary<string, object> dictionary = JsonConvert.DeserializeObject<Dictionary<string, object>>(value, new JsonSerializerSettings()
            {
                Error = (sender, error) => error.ErrorContext.Handled = true
            });
            if (dictionary == null)
            {
                _infoTimeout = 60 * 3;
                _infoText.SetText(Language.GetText("Mods.MagicDye.UI.Information.PasteInvalidJSONError"));
                _infoText.Recalculate();
                return;
            }

            Dictionary<string, object> dictionary2 = new Dictionary<string, object>();
            Vector3 TempPrimaryColor = new Vector3(1f, 1f, 1f);
            Vector3 TempSecondaryColor = new Vector3(1f, 1f, 1f);
            float TempSaturation = 1f;
            float TempOpacity = 1f;
            string TempPass = "";
            string TempColorMod1 = "";
            string TempColorMod2 = "";

            foreach (KeyValuePair<string, object> item in dictionary)
            {
                dictionary2[item.Key.ToLower()] = item.Value;
            }

            if (dictionary2.Count != 8)
            {
                _infoTimeout = 60 * 3;
                _infoText.SetText(Language.GetText("Mods.MagicDye.UI.Information.PasteJSONSizeError"));
                _infoText.Recalculate();
                return;
            }

            if (dictionary2.TryGetValue("version", out var value2))
            {
                long TempLong;
                try
                {
                    TempLong = (long)value2;
                }
                catch
                {
                    _infoTimeout = 60 * 3;
                    _infoText.SetText(Language.GetText("Mods.MagicDye.UI.Information.PasteJSONVersionError"));
                    _infoText.Recalculate();
                    return;
                }
            }

            if (dictionary2.TryGetValue("pass", out value2))
            {
                string TempString;
                try
                {
                    TempString = (string)value2;
                }
                catch
                {
                    _infoTimeout = 60 * 3;
                    _infoText.SetText(Language.GetText("Mods.MagicDye.UI.Information.PasteInvalidPassError"));
                    _infoText.Recalculate();
                    return;
                }

                if (_passList.Contains(TempString))
                {
                    TempPass = TempString;
                }
                else
                {
                    _infoTimeout = 60 * 3;
                    _infoText.SetText(Language.GetText("Mods.MagicDye.UI.Information.PasteInvalidPassUnresearchedError"));
                    _infoText.Recalculate();
                    return;
                }
            }

            if (dictionary2.TryGetValue("color modifier 1", out value2))
            {
                string TempString;
                try
                {
                    TempString = (string)value2;
                }
                catch
                {
                    _infoTimeout = 60 * 3;
                    _infoText.SetText(Language.GetText("Mods.MagicDye.UI.Information.PasteInvalidColorModifierError"));
                    _infoText.Recalculate();
                    return;
                }

                if (_colorModList.Contains(TempString))
                {
                    TempColorMod1 = TempString;
                }
                else
                {
                    _infoTimeout = 60 * 3;
                    _infoText.SetText(Language.GetText("Mods.MagicDye.UI.Information.PasteInvalidColorModifierUnresearchedError"));
                    _infoText.Recalculate();
                    return;
                }
            }

            if (dictionary2.TryGetValue("color modifier 2", out value2))
            {
                string TempString;
                try
                {
                    TempString = (string)value2;
                }
                catch
                {
                    _infoTimeout = 60 * 3;
                    _infoText.SetText(Language.GetText("Mods.MagicDye.UI.Information.PasteInvalidColorModifierError"));
                    _infoText.Recalculate();
                    return;
                }

                if (_colorModList.Contains(TempString))
                {
                    TempColorMod2 = TempString;
                }
                else
                {
                    _infoTimeout = 60 * 3;
                    _infoText.SetText(Language.GetText("Mods.MagicDye.UI.Information.PasteInvalidColorModifierUnresearchedError"));
                    _infoText.Recalculate();
                    return;
                }
            }

            if (dictionary2.TryGetValue("primarycolor", out value2))
            {
                JArray tempArray = (JArray)value2;
                float r;
                float g;
                float b;

                try
                {
                    r = (float)tempArray[0];
                    g = (float)tempArray[1];
                    b = (float)tempArray[2];
                }
                catch
                {
                    _infoTimeout = 60 * 3;
                    _infoText.SetText(Language.GetText("Mods.MagicDye.UI.Information.PasteInvalidColorError"));
                    _infoText.Recalculate();
                    return;
                }
                TempPrimaryColor = new Vector3(r, g, b);
            }

            if (dictionary2.TryGetValue("secondarycolor", out value2))
            {
                JArray tempArray = (JArray)value2;
                float r;
                float g;
                float b;

                try
                {
                    r = (float)tempArray[0];
                    g = (float)tempArray[1];
                    b = (float)tempArray[2];
                }
                catch
                {
                    _infoTimeout = 60 * 3;
                    _infoText.SetText(Language.GetText("Mods.MagicDye.UI.Information.PasteInvalidColorError"));
                    _infoText.Recalculate();
                    return;
                }
                TempSecondaryColor = new Vector3(r, g, b);
            }

            if (dictionary2.TryGetValue("saturation", out value2))
            {
                float sat;
                try
                {
                    sat = Convert.ToSingle(value2);
                }
                catch
                {
                    _infoTimeout = 60 * 3;
                    _infoText.SetText(Language.GetText("Mods.MagicDye.UI.Information.PasteInvalidSaturationError"));
                    _infoText.Recalculate();
                    return;
                }

                TempSaturation = sat;
            }

            if (dictionary2.TryGetValue("opacity", out value2))
            {
                float opacity;
                try
                {
                    opacity = Convert.ToSingle(value2);
                }
                catch
                {
                    _infoTimeout = 60 * 3;
                    _infoText.SetText(Language.GetText("Mods.MagicDye.UI.Information.PasteInvalidOpacityError"));
                    _infoText.Recalculate();
                    return;
                }

                TempOpacity = opacity;
            }

            _tempPrimaryColor = TempPrimaryColor;
            _tempSecondaryColor = TempSecondaryColor;
            _tempSaturation = TempSaturation;
            _tempOpacity = TempOpacity;
            _tempPass = TempPass;
            _tempColorMod1 = TempColorMod1;
            _tempColorMod2 = TempColorMod2;

            if (_passGrid != null && _passGrid.LastSelected != null)
            {
                ((UIText)_passGrid.LastSelected).TextColor = Color.Gray;
                _passGrid.LastSelected = _passGrid.GetGridItemByString(_tempPass);
                if (_passGrid.LastSelected != null)
                {
                    ((UIText)_passGrid.GetGridItemByString(_tempPass)).TextColor = Color.Yellow;
                }
            }

            if (_colorMods1Grid != null && _colorMods1Grid.LastSelected != null)
            {
                ((UIText)_colorMods1Grid.LastSelected).TextColor = Color.Gray;
                _colorMods1Grid.LastSelected = _colorMods1Grid.GetGridItemByString(_tempColorMod1);
                if (_colorMods1Grid.LastSelected != null)
                {
                    ((UIText)_colorMods1Grid.GetGridItemByString(_tempColorMod1)).TextColor = Color.Yellow;
                }
            }

            if (_colorMods2Grid != null && _colorMods2Grid.LastSelected != null)
            {
                ((UIText)_colorMods2Grid.LastSelected).TextColor = Color.Gray;
                _colorMods2Grid.LastSelected = _colorMods2Grid.GetGridItemByString(_tempColorMod2);
                if (_colorMods2Grid.LastSelected != null)
                {
                    ((UIText)_colorMods2Grid.GetGridItemByString(_tempColorMod2)).TextColor = Color.Yellow;
                }
            }

            UpdateColorValues();
            MagicDye.GetItemIDFromPass(_tempPass);
            UIPlayer.MagicDyeItem[_workingSlot] = new Item(MagicDye.GetItemIDFromPass(_tempPass));
            if (_rPInterface != null)
            {
                _rPInterface.Value = _tempPrimaryColor.X;
                _gPInterface.Value = _tempPrimaryColor.Y;
                _bPInterface.Value = _tempPrimaryColor.Z;
            }
            if (_rSInterface != null)
            {
                _rSInterface.Value = _tempSecondaryColor.X;
                _gSInterface.Value = _tempSecondaryColor.Y;
                _bSInterface.Value = _tempSecondaryColor.Z;
            }
            if (_SatInterface != null)
            {
                _SatInterface.Value = _tempSaturation;
            }
            if (_OpacityInterface != null)
            {
                _OpacityInterface.Value = _tempOpacity;
            }
            _infoTimeout = 60 * 3;
            _infoText.SetText(Language.GetText("Mods.MagicDye.UI.Information.PasteSuccess"));
            _infoText.Recalculate();
        }

        private void Click_CPButton(UIMouseEvent evt, UIElement listeningElement)
        {
            UIColoredImageButton element = ((UIColoredImageButton)listeningElement);
            SoundEngine.PlaySound(SoundID.MenuTick);
            element.SetSelected(true);
            _researchAndTemplatesButton.SetSelected(false);
            if (_currentPage != 1)
            {
                _pagePanel.RemoveAllChildren();
                AddCPPageElements(_pagePanel);
                _currentPage = 1;
            }
        }

        private void Click_RTButton(UIMouseEvent evt, UIElement listeningElement)
        {
            UIColoredImageButton element = ((UIColoredImageButton)listeningElement);
            SoundEngine.PlaySound(SoundID.MenuTick);
            element.SetSelected(true);
            _colorAndPassesButton.SetSelected(false);
            if (_currentPage != 2)
            {
                _pagePanel.RemoveAllChildren();
                AddRTPageElements(_pagePanel);
                _currentPage = 2;
            }
        }
        private void Click_searchCancelButton(UIMouseEvent evt, UIElement listeningElement)
        {
            if (_searchBar.HasContents)
            {
                _searchBar.SetContents(null, forced: true);
                SoundEngine.PlaySound(SoundID.MenuClose);
            }
            else
            {
                SoundEngine.PlaySound(SoundID.MenuTick);
            }
        }

        private void Click_SearchArea(UIMouseEvent evt, UIElement listeningElement)
        {
            if (evt.Target.Parent != _searchBoxPanel)
            {
                _searchBar.ToggleTakingText();
                _didClickSearchBar = true;
            }
        }

        private void Click_ClosePassGrid(UIMouseEvent evt, UIElement listeningElement)
        {
            if (evt.Target == _passGrid)
            {
                ClosePassGrid();
            }
        }

        private void Click_CloseColorModGrid1(UIMouseEvent evt, UIElement listeningElement)
        {
            if (evt.Target == _colorMods1Grid)
            {
                CloseColorModGrid1();
            }
        }

        private void Click_CloseColorModGrid2(UIMouseEvent evt, UIElement listeningElement)
        {
            if (evt.Target == _colorMods2Grid)
            {
                CloseColorModGrid2();
            }
        }

        private void Click_PassOption(string text)
        {
            _tempPass = text;

            if (_passDictionary.TryGetValue(text, out int value))
            {
                UpdateDyeVars(value, true);
                UpdateColorValues();
            }
            ClosePassGrid();
        }

        private void Click_ColotMod1Option(string text)
        {
            _colorMods1Grid.StoredPass = null;
            _tempColorMod1 = text;

            CloseColorModGrid1();
        }

        private void Click_ColorMod2Option(string text)
        {
            _colorMods2Grid.StoredPass = null;
            _tempColorMod2 = text;

            CloseColorModGrid2();
        }

        private void LeftClick_DyeGrid(UIMouseEvent evt, UIElement listeningElement)
        {
            _dyeGrid = ((UIDynamicDyeCollection)evt.Target);
            if (_dyeGrid.clickedItemID == null) // fixes clicking empty grid spaces resulting in a crash
                return;
            UpdateDyeVars(_dyeGrid.clickedItemID, false);
        }

        private void LeftClick_BPPanel(UIMouseEvent evt, UIElement listeningElement)
        {
            _bPInterface.Value = (float)Math.Round(_bPInterface.Value + 0.005, 3);
        }

        private void LeftClick_GPPanel(UIMouseEvent evt, UIElement listeningElement)
        {
            _gPInterface.Value = (float)Math.Round(_gPInterface.Value + 0.005, 3);
        }

        private void LeftClick_RPPanel(UIMouseEvent evt, UIElement listeningElement)
        {
            _rPInterface.Value = (float)Math.Round(_rPInterface.Value + 0.005, 3);
        }

        private void LeftClick_GSPanel(UIMouseEvent evt, UIElement listeningElement)
        {
            _gSInterface.Value = (float)Math.Round(_gSInterface.Value + 0.005, 3);
        }

        private void LeftClick_RSPanel(UIMouseEvent evt, UIElement listeningElement)
        {
            _rSInterface.Value = (float)Math.Round(_rSInterface.Value + 0.005, 3);
        }
        private void LeftClick_BSPanel(UIMouseEvent evt, UIElement listeningElement)
        {
            _bSInterface.Value = (float)Math.Round(_bSInterface.Value + 0.005, 3);
        }

        private void LeftClick_SatPanel(UIMouseEvent evt, UIElement listeningElement)
        {
            _SatInterface.Value = (float)Math.Round(_SatInterface.Value + 0.005, 3);
        }

        private void LeftClick_OpacityPanel(UIMouseEvent evt, UIElement listeningElement)
        {
            _OpacityInterface.Value = (float)Math.Round(_OpacityInterface.Value + 0.005, 3);
        }

        private void LeftClick_SacrificeButton(UIMouseEvent evt, UIElement listeningElement)
        {
            if (_researchSlot.Item.ModItem?.Mod == MagicDye.instance)
            {
                UpdateResearchSlotDye(1);
            }
            else
                SacrificeWhatYouCan();
        }

        private void LeftClick_CycleFilter(UIMouseEvent evt, UIElement listeningElement)
        {
            UpdateFilter(1);
        }

        private void RightClick_RPPanel(UIMouseEvent evt, UIElement listeningElement)
        {
            _rPInterface.Value = (float)Math.Round(_rPInterface.Value - 0.005, 3);
        }

        private void RightClick_GPPanel(UIMouseEvent evt, UIElement listeningElement)
        {
            _gPInterface.Value = (float)Math.Round(_gPInterface.Value - 0.005, 3);
        }

        private void RightClick_BPPanel(UIMouseEvent evt, UIElement listeningElement)
        {
            _bPInterface.Value = (float)Math.Round(_bPInterface.Value - 0.005, 3);
        }

        private void RightClick_RSPanel(UIMouseEvent evt, UIElement listeningElement)
        {
            _rSInterface.Value = (float)Math.Round(_rSInterface.Value - 0.005, 3);
        }

        private void RightClick_GSPanel(UIMouseEvent evt, UIElement listeningElement)
        {
            _gSInterface.Value = (float)Math.Round(_gSInterface.Value - 0.005, 3);
        }

        private void RightClick_BSPanel(UIMouseEvent evt, UIElement listeningElement)
        {
            _bSInterface.Value = (float)Math.Round(_bSInterface.Value - 0.005, 3);
        }

        private void RightClick_SatPanel(UIMouseEvent evt, UIElement listeningElement)
        {
            _SatInterface.Value = (float)Math.Round(_SatInterface.Value - 0.005, 3);
        }

        private void RightClick_OpacityPanel(UIMouseEvent evt, UIElement listeningElement)
        {
            _OpacityInterface.Value = (float)Math.Round(_OpacityInterface.Value - 0.005, 3);
        }

        private void RightClick_SacrificeButton(UIMouseEvent evt, UIElement listeningElement)
        {
            if (_researchSlot.Item.ModItem?.Mod == MagicDye.instance)
            {
                UpdateResearchSlotDye(-1);
            }
        }

        private void RightClick_CycleFilter(UIMouseEvent evt, UIElement listeningElement)
        {
            UpdateFilter(-1);
        }

        // Scroll Events
        private void Scroll_Slot(UIScrollWheelEvent evt, UIElement listeningElement)
        {
            UpdateWorkingSlot(1 * Math.Sign(evt.ScrollWheelValue));
        }

        private void Scroll_SacrificeButton(UIScrollWheelEvent evt, UIElement listeningElement)
        {
            if (_researchSlot.Item.ModItem?.Mod == MagicDye.instance)
            {
                UpdateResearchSlotDye(1 * Math.Sign(evt.ScrollWheelValue));
            }
        }

        // Hover Events
        private void Hover_TextButtons(UIMouseEvent evt, UIElement listeningElement)
        {
            UITextButton text = ((UITextButton)evt.Target);
            SoundEngine.PlaySound(SoundID.MenuTick);
            text.ShadowColor = Color.Brown;
            text.Scale = 1.1f;
        }

        private void Hover_Panel(UIMouseEvent evt, UIElement listeningElement)
        {
            UIPanel element = ((UIPanel)listeningElement);
            SoundEngine.PlaySound(SoundID.MenuTick);
            element.BorderColor = Color.Gold;
        }

        private void Hover_SacrificeButton(UIMouseEvent evt, UIElement listeningElement)
        {
            SoundEngine.PlaySound(SoundID.MenuTick);
            ((UIPanel)evt.Target).BackgroundColor = new Color(73, 94, 171);
            ((UIPanel)evt.Target).BorderColor = Colors.FancyUIFatButtonMouseOver;
        }

        private void Hover_searchCancelButton(UIMouseEvent evt, UIElement listeningElement)
        {
            SoundEngine.PlaySound(SoundID.MenuTick);
        }

        private void HoverOut_Panel(UIMouseEvent evt, UIElement listeningElement)
        {
            UIPanel element = ((UIPanel)listeningElement);
            element.BorderColor = Color.Black;
        }

        private void HoverOut_TextButtons(UIMouseEvent evt, UIElement listeningElement)
        {
            UITextButton text = ((UITextButton)evt.Target);
            text.ShadowColor = Color.Black;
            text.Scale = 0.9f;
        }

        private void HoverOut_SacrificeButton(UIMouseEvent evt, UIElement listeningElement)
        {
            ((UIPanel)evt.Target).BackgroundColor = new Color(63, 82, 151) * 0.8f;
            ((UIPanel)evt.Target).BorderColor = Color.Black;
        }

        // Update Events
        private void Update_TextButtons(UIElement listeningElement)
        {
            UITextButton text = ((UITextButton)listeningElement);
            int textColor = (Main.mouseTextColor * 2 + 255) / 3;
            text.TextColor = new Color(textColor, (int)((double)textColor / 1.1), textColor / 2, textColor); ;
        }

        private void Update_RPPanel(UIElement listeningElement)
        {
            _rPInterface.Value = (float)Math.Round(_rPInterface.Value, 3);
            _tempPrimaryColor.X = _rPInterface.Value;

            UpdateColorValues();
        }

        private void Update_GPPanel(UIElement listeningElement)
        {
            _gPInterface.Value = (float)Math.Round(_gPInterface.Value, 3);
            _tempPrimaryColor.Y = _gPInterface.Value;

            UpdateColorValues();
        }

        private void Update_BPPanel(UIElement listeningElement)
        {
            _bPInterface.Value = (float)Math.Round(_bPInterface.Value, 3);
            _tempPrimaryColor.Z = _bPInterface.Value;

            UpdateColorValues();
        }

        private void Update_RSPanel(UIElement listeningElement)
        {
            _rSInterface.Value = (float)Math.Round(_rSInterface.Value, 3);
            _tempSecondaryColor.X = _rSInterface.Value;

            UpdateColorValues();
        }

        private void Update_GSPanel(UIElement listeningElement)
        {
            _gSInterface.Value = (float)Math.Round(_gSInterface.Value, 3);
            _tempSecondaryColor.Y = _gSInterface.Value;

            UpdateColorValues();
        }

        private void Update_BSPanel(UIElement listeningElement)
        {
            _bSInterface.Value = (float)Math.Round(_bSInterface.Value, 3);
            _tempSecondaryColor.Z = _bSInterface.Value;

            UpdateColorValues();
        }
        private void Update_SatPanel(UIElement listeningElement)
        {
            _SatInterface.Value = (float)Math.Round(_SatInterface.Value, 3);
            _tempSaturation = _SatInterface.Value;

            UpdateColorValues();
        }

        private void Update_OpacityPanel(UIElement listeningElement)
        {
            _OpacityInterface.Value = (float)Math.Round(_OpacityInterface.Value, 3);
            _tempOpacity = _OpacityInterface.Value;

            UpdateColorValues();
        }

        private void Update_IndexText(UIElement affectedElement)
        {
            UIText uIText = affectedElement as UIText;
            uIText.SetText((_workingSlot + 1).ToString());
        }

        private void Update_ResearchText(UIElement affectedElement)
        {
            UIText text = affectedElement as UIText;
            if (_researchSlot.Item.ModItem?.Mod == MagicDye.instance)
            {
                if (_researchSlotButtonValue == -1)
                {
                    for (int i = 0; i < MagicDye.dyeLookUp.Count(); i++)
                    {
                        if (MagicDye.dyeLookUp[i] == _researchSlot.Item.type)
                        {
                            _researchSlotButtonValue = i;
                            break;
                        }
                    }
                }
                text.SetText((_researchSlotButtonValue + 1).ToString());
            }
            else
            {
                _researchSlotButtonValue = -1;
                if (text.Text != Language.GetTextValue("CreativePowers.ConfirmInfiniteItemSacrifice"))
                    text.SetText(Language.GetText("CreativePowers.ConfirmInfiniteItemSacrifice"));

            }
        }

        private void Update_DescriptionText(UIElement affectedElement)
        {
            UIText uIText = affectedElement as UIText;
            int itemIdChecked;
            int amountWeHave;
            int amountNeededTotal;
            bool sacrificeNumbers = GetSacrificeNumbers(out itemIdChecked, out amountWeHave, out amountNeededTotal);
            ShouldDrawSacrificeArea();
            if (!Main.mouseItem.IsAir)
                ForgetDyeSacrifice();

            if (itemIdChecked == 0)
            {
                if (_lastItemIdSacrificed != 0 && _lastItemAmountWeNeededTotal != _lastItemAmountWeHad)
                    uIText.SetText($"({_lastItemAmountWeHad}/{_lastItemAmountWeNeededTotal})");
                else
                    uIText.SetText("???");

                return;
            }

            ForgetDyeSacrifice();
            if (!sacrificeNumbers)
                uIText.SetText("X");
            else
                uIText.SetText($"({amountWeHave}/{amountNeededTotal})");
        }

        private void Update_InfoText(UIElement affectedElement)
        {

            UIText uIText = affectedElement as UIText;
            if (_infoTimeout > 0)
            {
                _infoTimeout -= 1;
            }
            if (_infoTimeout == 0)
            {
                uIText.SetText("");
                uIText.Recalculate();
            }
        }

        private void Update_ColorMod2Text(UIElement listeningElement)
        {
            UIText element = ((UIText)listeningElement);
            element.SetText(_tempColorMod2);
        }

        private void Update_PassText(UIElement listeningElement)
        {
            UIText element = ((UIText)listeningElement);
            element.SetText(_tempPass);
        }

        private void Update_ColotMod1Text(UIElement listeningElement)
        {
            UIText element = ((UIText)listeningElement);
            element.SetText(_tempColorMod1);
        }

        // Virtual Keyboard Events & Functionality
        private void ContentsChanged_Search(string contents)
        {
            if (!Main.gameMenu)
            {
                _searchString = contents;
                _filterer.SetSearchFilter(contents);
                UpdateDyesList();
            }
        }

        private void InputStart_Search()
        {
            _searchBoxPanel.BorderColor = Main.OurFavoriteColor;
        }

        private void InputEnd_Search()
        {
            _searchBoxPanel.BorderColor = new Color(35, 40, 83);
        }

        private void VirtualKeyboard_Search()
        {
            int maxInputLength = 40;
            UIVirtualKeyboard uIVirtualKeyboard = new UIVirtualKeyboard(Language.GetText("UI.PlayerNameSlot").Value, _searchString, FinishedName_Search, GoBackHere_Search, 3, allowEmpty: true);
            uIVirtualKeyboard.SetMaxInputLength(maxInputLength);
            uIVirtualKeyboard.CustomEscapeAttempt = Escape_Search;
            IngameFancyUI.OpenUIState(uIVirtualKeyboard);
        }

        private bool Escape_Search()
        {
            IngameFancyUI.Close();
            Main.playerInventory = true;
            if (_searchBar.IsWritingText)
            {
                _searchBar.ToggleTakingText();
            }
            return true;
        }

        private void GoBackHere_Search()
        {
            IngameFancyUI.Close();
            _searchBar.ToggleTakingText();
        }

        private void FinishedName_Search(string name)
        {
            string contents = name.Trim();
            _searchBar.SetContents(contents);
            GoBackHere_Search();
        }

        private void InputCanceled_Search()
        {
            Main.LocalPlayer.ToggleInv();
        }

        //UI Functionality
        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            string text = null;
            if (Main.mouseItem.IsAir)
            {
                if (_copyTemplateButton.IsMouseHovering)
                    text = Language.GetTextValue("Mods.MagicDye.UI.CopyToolTip");

                if (_pasteTemplateButton.IsMouseHovering)
                    text = Language.GetTextValue("Mods.MagicDye.UI.PasteToolTip");

                if (_filterButton != null && _filterButton.IsMouseHovering)
                {
                    if (_currentFilterIndex == -1)
                        text = $"{Language.GetTextValue("Mods.MagicDye.UI.Filters.Filter")}: {Language.GetTextValue("Mods.MagicDye.UI.Filters.All")}";
                    else
                        text = $"{Language.GetTextValue("Mods.MagicDye.UI.Filters.Filter")}: {_filterer.AvailableFilters[_currentFilterIndex].GetDisplayNameKey()}";
                }

                if (_slotsPanel.IsMouseHovering)
                    text = Language.GetTextValue("Mods.MagicDye.UI.WorkingSlotToolTip");

                if (_researchSlot != null && _researchSlot.IsMouseHovering && _researchSlot.Item.IsAir)
                    text = Language.GetTextValue("Mods.MagicDye.UI.ResearchToolTip");

                if (_colorAndPassesButton.IsMouseHovering)
                    text = Language.GetTextValue("Mods.MagicDye.UI.ColorPageToolTip");

                if (_researchAndTemplatesButton.IsMouseHovering)
                    text = Language.GetTextValue("Mods.MagicDye.UI.ResearchPageToolTip");
            }

            if (text != null)
            {
                float x = FontAssets.MouseText.Value.MeasureString(text).X;
                Vector2 vector = new Vector2(Main.mouseX, Main.mouseY) + new Vector2(16f);
                if (vector.Y > (float)(Main.screenHeight - 30))
                    vector.Y = Main.screenHeight - 30;

                if (vector.X > (float)Main.screenWidth - x)
                    vector.X = Main.screenWidth - 460;

                Utils.DrawBorderStringFourWay(spriteBatch, FontAssets.MouseText.Value, text, vector.X, vector.Y, new Color(Main.mouseTextColor, Main.mouseTextColor, Main.mouseTextColor, Main.mouseTextColor), Color.Black, Vector2.Zero);
            }
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            interfaceRef = ModContent.GetInstance<MagicDyeUISystem>();

            //if (interfaceRef.MagicDyeInterface.CurrentState == interfaceRef.MagicDyeUI)
            //{
            //    Main.LocalPlayer.releaseInventory = false;
            //}

            // previously MagicDyeUI was set to let you close using the inventory key.
            // this however isn't conducive to being able to use the research feature. so i commented it out.
            //if (Main.LocalPlayer.controlInv && interfaceRef.MagicDyeInterface.CurrentState == interfaceRef.MagicDyeUI)
            //{
            //    OnCloseReset();
            //    interfaceRef.HideMagicDyeUI();
            //}
        }

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            base.DrawSelf(spriteBatch);

            if (tempPlayer != null)
            {
                tempPlayer.socialIgnoreLight = true;
            }


            Vector2 mousePosition = UserInterface.ActiveInstance.MousePosition;
            bool isHoveringUI = _BG.ContainsPoint(new Vector2((int)mousePosition.X, (int)mousePosition.Y));
            if (isHoveringUI)
            {
                Main.LocalPlayer.mouseInterface = true;
                PlayerInput.LockVanillaMouseScroll("MagicDye/UI");
            }
        }

        public override void OnActivate()
        {
            if (!Main.gameMenu)
            {
                _tempPass = "";
                _currentFilterIndex = -1;
                _modListIndex = 0;
                RemoveAllChildren();
                BuildUI();
                InitPreviewPlayer();
            }
        }

        public override void OnDeactivate()
        {
            ReturnResearchItems();
        }
        public override void OnInitialize()
        {

        }
    }
}