using Godot;

namespace SkinMerge.UI.EditorUI;

public partial class ColorContent : EditorContent
{
    [Export] private ColorPicker _colorPicker;
    
    public override void _Ready()
    {
        Editor.CurrentProject.ColorsChanged += OnEditorColorsChanged;
    }

    public override void _Process(double delta)
    {
        if (_colorPicker.Color != Editor.CurrentProject.GetCurrentColor())
        {
            GD.Print(_colorPicker.Color);
            Editor.CurrentProject.SetCurrentColor(_colorPicker.Color);
        }
    }
    
    private void OnEditorColorsChanged(Color primary, Color secondary)
    {
        _colorPicker.SetPickColor(primary);
    }
}