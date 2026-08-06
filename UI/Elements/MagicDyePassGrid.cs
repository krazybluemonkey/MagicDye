using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Terraria.Audio;
using Terraria.GameContent.UI.Elements;
using Terraria.ID;
using Terraria.UI;


namespace MagicDye.UI.Elements
{
    internal class MagicDyePassGrid : UIPanel // modified from Terraria.GameContent.UI.Elements.UIBestiarySortingOptionsGrid
    {
        private List<string> _passes;
        private UIList _list;
        public UIElement LastSelected;
        public string StoredPass;
        public string SelectedPass;

        public event Action<string> OnClickingOption;
        public event MouseEvent OnMousingOverOption;
        public event MouseEvent OnMousingOffOption;

        public MagicDyePassGrid(List<string> passes)
        {
            _passes = passes;
            if (passes == null)
                _passes = new List<string>();
            Width = new StyleDimension(0f, 1f);
            Height = new StyleDimension(0f, 1f);
            BackgroundColor = new Color(35, 40, 83) * 0.5f;
            BorderColor = new Color(35, 40, 83) * 0.5f;
            IgnoresMouseInteraction = false;
            SetPadding(0f);
            BuildList();
        }

        public UIElement GetGridItemByString(string pass)
        {

            IEnumerator<UIElement> passList = _list.GetEnumerator();
            while (passList.MoveNext())
            {
                if ( ((UIText)passList.Current).Text == pass)
                {
                    return passList.Current;
                }
            }
            return null;
        }

        private void BuildList()
        {
            int num = 2;
            int num2 = 26 + num;
            int num3 = 0;
            for (int i = 0; i < _passes.Count; i++)
            {
                num3++;
            }

            UIPanel uIPanel = new UIPanel
            {
                Width = new StyleDimension(400f, 0f),
                Height = new StyleDimension(num3 * num2 + 5 + 3, 0f),
                HAlign = 1f,
                VAlign = 0f,
                Left = new StyleDimension(0f, 0f),
                Top = new StyleDimension(0f, 0f)
            };

            uIPanel.BorderColor = new Color(89, 116, 213, 255) * 0.9f;
            uIPanel.BackgroundColor = new Color(73, 94, 171) * 0.9f;
            uIPanel.SetPadding(0f);
            Append(uIPanel);

            // pass list
            _list = new UIList
            {
                HAlign = 0f,
                VAlign = 0f,
                Width = new StyleDimension(0f, 1f),
                Height = new StyleDimension(0f, 1f),
                ListPadding = 6f,
            };
            _list.ManualSortMethod = (e) => { };

            int num4 = 0;
            for (int j = 0; j < _passes.Count; j++)
            {
                UIText uIText = new UIText(_passes[j], 1.1f);

                uIText.TextColor = Color.Gray;
                uIText.OnMouseOver += MouseOver;
                uIText.OnMouseOut += MouseFade;
                uIText.OnLeftClick += ClickOption;
                uIText.SetSnapPoint("SortSteps", num4);
                _list.Add(uIText);
                num4++;
            }


            uIPanel.Append(_list);

            //pass list scroll bar
            UIScrollbar uIScrollBar = new UIScrollbar();
            uIScrollBar.SetView(100f, 1000f);
            uIScrollBar.Height.Set(0f, 1f);
            uIScrollBar.HAlign = 1f;
            _list.SetScrollbar(uIScrollBar);
            uIPanel.Append(uIScrollBar);
        }

        private void MouseOver(UIMouseEvent evt, UIElement listeningElement)
        {
            SoundEngine.PlaySound(SoundID.MenuTick);
            if ((evt.Target) != LastSelected)
            {
                ((UIText)evt.Target).TextColor = Color.White;
            }
            this.OnMousingOverOption?.Invoke(evt, evt.Target);
        }

        private void MouseFade(UIMouseEvent evt, UIElement listeningElement)
        {
            if ((evt.Target) != LastSelected)
            {
                ((UIText)evt.Target).TextColor = Color.Gray;
            }
            this.OnMousingOffOption?.Invoke(evt, evt.Target);
        }

        private void ClickOption(UIMouseEvent evt, UIElement listeningElement)
        {
            if (LastSelected != null)
            {
                ((UIText)LastSelected).TextColor = Color.Gray;
            }
            SoundEngine.PlaySound(SoundID.MenuTick);
            LastSelected = evt.Target;
            ((UIText)LastSelected).TextColor = Color.Yellow;
            if (this.OnClickingOption != null)
            {
                this.OnClickingOption(((UIText)LastSelected).Text);
            }
        }

    }
}
