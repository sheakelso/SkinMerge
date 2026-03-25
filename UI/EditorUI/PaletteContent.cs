using System.Collections.Generic;
using Godot;
using SkinMerge.Data;
using ColorPalette = SkinMerge.Data.ColorPalette;

namespace SkinMerge.UI.EditorUI;

public partial class PaletteContent : EditorContent
{
    [Export] private OptionButton _paletteButton;
    [Export] private LineEdit _paletteNameEdit;
    [Export] private Button _paletteRenameButton;
    [Export] private Control _rowParent;
    [Export] private Texture2D _addIcon;
    
    private Button _addRowButton;
    
    private ColorPalette _currentPalette;
    
    private List<PaletteColorRow> _rows = new List<PaletteColorRow>();

    public override void _Ready()
    {
        if (_rowParent.GetChildCount(true) - _rowParent.GetChildCount() == 0)
        {
            _addRowButton = CreateAddRowButton();
        }
        else
        {
            if (_rowParent.GetChild(_rowParent.GetChildCount(true) - 1, true) is HBoxContainer container)
            {
                if (container.GetChild(0) is Button addRowButton)
                {
                    _addRowButton = addRowButton;
                }
                else
                {
                    _addRowButton = CreateAddRowButton();
                }
            }
            else
            {
                _addRowButton = CreateAddRowButton();
            }
        }
        
        _paletteButton.ItemSelected += OnPaletteSelected;
        _paletteNameEdit.Visible = false;
        _paletteNameEdit.TextSubmitted += OnPaletteNameSubmitted;
        _paletteRenameButton.Pressed += OnPaletteRenameButtonPressed;
        _addRowButton.Pressed += OnAddRowButtonPressed;
        UpdatePaletteOptions();
        
        OnPaletteSelected(_paletteButton.Selected);
    }

    private void UpdatePaletteOptions()
    {
        _paletteButton.Clear();
        foreach (ColorPalette palette in Editor.CurrentProject.Palettes)
        {
            _paletteButton.AddItem(palette.Name);
        }

        if (Editor.CurrentProject.Palettes.Length == 0)
        {
            _paletteButton.AddItem("");
        }
        _paletteButton.AddSeparator();
        _paletteButton.AddItem("New Palette");
    }

    private void OnPaletteSelected(long index)
    {
        if (index == _paletteButton.GetItemCount() - 1)
        {
            _currentPalette = Editor.CurrentProject.CreatePalette("New Palette");
            UpdatePaletteOptions();
            _paletteButton.Selected = Editor.CurrentProject.Palettes.Length - 1;
        }
        if (Editor.CurrentProject.Palettes.Length == 0)
        {
            return;
        }
        _currentPalette = Editor.CurrentProject.Palettes[index];
        InitializeRows();
    }

    private void InitializeRows()
    {
        foreach (Node child in _rowParent.GetChildren())
        {
            _rowParent.RemoveChild(child);
        }
        foreach (ColorPaletteRow row in _currentPalette.Rows)
        {
            CreateRow(row);
        }
    }

    private void OnPaletteNameSubmitted(string newtext)
    {
        _paletteButton.Visible = true;
        _paletteNameEdit.Visible = false;
        _currentPalette.Name = newtext;
        UpdatePaletteOptions();
    }

    private void OnPaletteRenameButtonPressed()
    {
        _paletteButton.Visible = false;
        _paletteNameEdit.Visible = true;
        _paletteNameEdit.Text = _currentPalette.Name;
    }

    private void OnAddRowButtonPressed()
    {
        ColorPaletteRow row = _currentPalette.AddNewRow("New row");
        CreateRow(row);
    }

    private Button CreateAddRowButton()
    {
        HBoxContainer buttonContainer = new();
        buttonContainer.Alignment = BoxContainer.AlignmentMode.Center;
        _rowParent.AddChild(buttonContainer, false, InternalMode.Back);
            
        _addRowButton = new Button();
        _addRowButton.Text = "New Row";
        buttonContainer.AddChild(_addRowButton);
        
        return _addRowButton;
    }

    private PaletteColorButton[] GetColorButtons()
    {
        List<PaletteColorButton> buttons = new List<PaletteColorButton>();
        foreach (PaletteColorRow row in _rows)
        {
            buttons.AddRange(row.ColorButtons);
        }
        return buttons.ToArray();
    }

    private void CreateRow(ColorPaletteRow row = null)
    {
        if(row == null) row = new ColorPaletteRow("New Row");
        PaletteColorRow rowDisplay = new PaletteColorRow(_addIcon, row);
        rowDisplay.CustomMinimumSize = new Vector2(0, 35);
        rowDisplay.ColorButtonPressed += OnColorButtonClicked;
        _rows.Add(rowDisplay);
        _rowParent.AddChild(rowDisplay);
    }

    private void OnColorButtonClicked(PaletteColorButton button)
    {
        Editor.CurrentProject.SetCurrentColor(button.Color);

        foreach (PaletteColorButton colorButton in GetColorButtons())
        {
            if(colorButton != button) colorButton.SetPressed(false);
        }
    }
}