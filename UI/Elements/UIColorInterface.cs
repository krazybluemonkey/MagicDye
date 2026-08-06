using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent.UI.Elements;
using Terraria.GameInput;
using Terraria.Localization;
using Terraria.UI;

namespace MagicDye.UI.Elements;

public class UIColorInterface : UIElement // simple color interfacing elements
{
    private Color _color;
    private UIPanel _panel;
    private UIText _colorText;
    private float _value;

    public float Value
    {
        get
        {
            return _value;
        }
        set
        {
            _value = value;
        }
    }

    public UIPanel Panel
    {
        get
        {
            return _panel;
        }
        set
        {
            _panel = value;
        }
    }
    public UIColorInterface(float val, Color color)
    {
        _color = color;
        _value = val;
        Height.Set(28f, 0f);
        Width.Set(28f * 4, 0f);
        SetPadding(0f);
        _panel = new UIPanel
        {
            Width = new StyleDimension(Width.Pixels - PaddingRight - PaddingLeft, 0f),
            Height = new StyleDimension(28f, 0f),
            VAlign = 1f,
            HAlign = 0.5f
        };
        _panel.SetPadding(0f);
        Append(_panel);
        UIText uIText = new UIText("", 0.8f)
        {
            HAlign = 0.5f,
            VAlign = 0.5f,
            IgnoresMouseInteraction = true
        };
        _panel.Append(uIText);
        _colorText = uIText;
    }

    public override void ScrollWheel(UIScrollWheelEvent evt)
    {
        base.ScrollWheel(evt);
        if (_panel != null && _panel.IsMouseHovering)
            _value += 0.05f *Math.Sign(evt.ScrollWheelValue);
    }

    protected override void DrawSelf(SpriteBatch spriteBatch)
    {
        _value = Math.Clamp(_value, -10f, 10f);
        _colorText.SetText(_value.ToString("0.000"));
        if (_value >= 0)
        {
            _panel.BackgroundColor = Color.Lerp(Color.Black, _color, _value);
        }
        else
        {
            _panel.BackgroundColor = Color.Lerp(Color.Black, new Color(1f - _color.R, 1f - _color.G, 1f - _color.B), - _value );
        }

        if (_panel.IsMouseHovering)
        {
            PlayerInput.LockVanillaMouseScroll("MagicDye/ColorIntereface");
        }
    }
}
