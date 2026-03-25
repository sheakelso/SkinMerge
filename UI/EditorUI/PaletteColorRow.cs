using System;
using System.Collections.Generic;
using System.Linq;
using Godot;
using SkinMerge.Data;

namespace SkinMerge.UI.EditorUI;

public partial class PaletteColorRow : HFlowContainer
{
    public event Action<PaletteColorButton> ColorButtonPressed; 
    
    private Texture2D _addIcon;
    private Button _addButton;
    
    private LineEdit _rowNameField;
    
    private ColorPaletteRow _colorPaletteRow;
    
    public PaletteColorButton[] ColorButtons => _colorButtons.ToArray();
    private readonly List<PaletteColorButton> _colorButtons = new List<PaletteColorButton>();

    public PaletteColorRow() : this(null, new ColorPaletteRow("New row"))
    {
        
    }

    public PaletteColorRow(Texture2D addIcon, ColorPaletteRow row)
    {
        _addIcon = addIcon;
        ThemeTypeVariation = "PaletteColorRow";
        
        if (GetChildCount(true) - GetChildCount() == 0)
        {
            _addButton = CreateAddButton();
            _rowNameField = CreateRowNameField(row.Name);
        }
        else
        {
            _addButton = GetChildren(true).Last(x => x is Button) as Button;
            if (_addButton == null) _addButton = CreateAddButton();
            
            MarginContainer nameMargin = GetChildren(true).Last(x => x is MarginContainer) as MarginContainer;
            _rowNameField = nameMargin.GetChild(0) as LineEdit;
            if (_rowNameField == null) _rowNameField = CreateRowNameField(row.Name);
        }
        
        _addButton.Pressed += OnAddButtonPressed;
        
        _colorPaletteRow = row;
        _rowNameField.TextSubmitted += SubmitRowName;
        UpdateColorButtons();
    }

    private void UpdateColorButtons()
    {
        foreach (Node child in GetChildren())
        {
            child.QueueFree();
        }
        _colorButtons.Clear();

        foreach (Color color in _colorPaletteRow.Colors)
        {
            AddColorButton(color);
        }
    }

    private Button CreateAddButton()
    {
        _addButton = new Button();
        _addButton.Icon = _addIcon;
        _addButton.ExpandIcon = true;
        _addButton.Flat = true;
        _addButton.ThemeTypeVariation = "PaletteRowAddButton";
        _addButton.CustomMinimumSize = new Vector2(35, 35);
        AddChild(_addButton, false, InternalMode.Back);
        
        return _addButton;
    }

    private LineEdit CreateRowNameField(string text)
    {
        MarginContainer buttonMargin = new MarginContainer();
        buttonMargin.AddThemeConstantOverride("margin_right", 4);
        buttonMargin.AddThemeConstantOverride("margin_top", 0);
        buttonMargin.AddThemeConstantOverride("margin_bottom", 0);
        buttonMargin.CustomMinimumSize = new Vector2(0, 35);
        AddChild(buttonMargin, false, InternalMode.Front);
        
        _rowNameField = new LineEdit();
        _rowNameField.Editable = true;
        _rowNameField.ExpandToTextLength = true;
        _rowNameField.ThemeTypeVariation = "PaletteRowNameField";
        _rowNameField.Text = text;
        buttonMargin.AddChild(_rowNameField);
        
        return _rowNameField;
    }

    private void SubmitRowName(string text)
    {
        _colorPaletteRow.Name = text;
        _rowNameField.ReleaseFocus();
    }
    
    private void OnAddButtonPressed()
    {
        _colorPaletteRow.AddColor(Editor.CurrentProject.GetCurrentColor());
        UpdateColorButtons();
    }

    private void AddColorButton(Color color)
    {
        PaletteColorButton colorButton = new PaletteColorButton();
        colorButton.CustomMinimumSize = new Vector2(35, 35);
        colorButton.Color = color;
        colorButton.Pressed += () => ColorButtonPressed?.Invoke(colorButton);
        
        _colorButtons.Add(colorButton);
        AddChild(colorButton);
    }
}