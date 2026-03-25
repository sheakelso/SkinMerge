using Godot;

namespace SkinMerge.UI.EditorUI;

[Tool]
[GlobalClass]
public partial class PaletteColorButton : Button
{
    private Material _transparentMaterial = ResourceLoader.Load<Material>("materials/grid.tres");

    [Export]
    public Material TransparentMaterial
    {
        get => _transparentMaterial;
        private set
        {
            _transparentMaterial = value;
            UpdatePanel();
        }
    }
    
    private Color _color;
    [Export] public Color Color
    {
        get => _color;
        set
        {
            _color = value;
            UpdateStyleBox();
        }
    }
    
    private StyleBoxFlat _normalStyle;
    private StyleBoxFlat _hoverStyle;
    private StyleBoxFlat _pressedStyle;
    private StyleBoxFlat _hoverPressedStyle;
    private Panel _transparentPanel;

    public PaletteColorButton()
    {
        ThemeTypeVariation = "PaletteColorButton";
        ToggleMode = true;
        
        if (GetChildCount(true) != 0)
        {
            _transparentPanel = GetChild(0, true) as Panel;
        }
        else
        {
            _transparentPanel = new Panel();
            AddChild(_transparentPanel, false, InternalMode.Front);
        }
        
        UpdatePanel();
    }

    public override void _Ready()
    {
        UpdateStyleBox();
    }

    public override void _UnhandledInput(InputEvent @event)
    {
        if (IsPressed())
        {
            if (@event is InputEventKey { Pressed: true, Keycode: Key.Left })
            {
                if(GetIndex() != 0) GetParent().MoveChild(this, GetIndex() - 1);
            }
            
            if (@event is InputEventKey { Pressed: true, Keycode: Key.Right })
            {
                if(GetIndex() != GetParent().GetChildCount() - 1) GetParent().MoveChild(this, GetIndex() + 1);
            }
        }
    }

    private void UpdatePanel()
    {
        StyleBoxFlat transparentStyle = new StyleBoxFlat();
        transparentStyle.SetCornerRadiusAll(0);
        
        _transparentPanel.SetAnchorsAndOffsetsPreset(LayoutPreset.FullRect);
        _transparentPanel.Material = _transparentMaterial;
        _transparentPanel.AddThemeStyleboxOverride("panel", transparentStyle);
        _transparentPanel.ShowBehindParent = true;
        _transparentPanel.MouseFilter = MouseFilterEnum.Pass;
        _transparentPanel.QueueRedraw();
    }

    private void UpdateStyleBox()
    {
        RemoveThemeStyleboxOverride("normal");
        StyleBox styleBox = GetThemeStylebox("normal", ThemeTypeVariation);
        _normalStyle = styleBox.Duplicate() as StyleBoxFlat;
        if(_normalStyle == null) _normalStyle = new StyleBoxFlat();
        
        RemoveThemeStyleboxOverride("hover");
        StyleBox hoverBox = GetThemeStylebox("hover", ThemeTypeVariation);
        _hoverStyle = hoverBox.Duplicate() as StyleBoxFlat;
        if(_hoverStyle == null) _hoverStyle = new StyleBoxFlat();
        
        RemoveThemeStyleboxOverride("pressed");
        StyleBox pressedBox = GetThemeStylebox("pressed", ThemeTypeVariation);
        _pressedStyle = pressedBox.Duplicate() as StyleBoxFlat;
        if(_pressedStyle == null) _pressedStyle = new StyleBoxFlat();
        
        RemoveThemeStyleboxOverride("hover_pressed");
        StyleBox hoverPressedBox = GetThemeStylebox("hover_pressed", ThemeTypeVariation);
        _hoverPressedStyle = hoverPressedBox.Duplicate() as StyleBoxFlat;
        if(_hoverPressedStyle == null) _hoverPressedStyle = new StyleBoxFlat();
        
        _normalStyle.BgColor = _color;
        _pressedStyle.BgColor = _color;
        _hoverStyle.BgColor = _color;
        _hoverPressedStyle.BgColor = _color;
        
        AddThemeStyleboxOverride("normal", _normalStyle);
        AddThemeStyleboxOverride("pressed", _pressedStyle);
        AddThemeStyleboxOverride("hover", _hoverStyle);
        AddThemeStyleboxOverride("hover_pressed", _hoverPressedStyle);
        
        QueueRedraw();
    }
}