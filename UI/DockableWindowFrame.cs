using Godot;

namespace SkinMerge.UI;

public partial class DockableWindowFrame : Control
{
    [Export] private Control _contentParent;
    [Export] private Button _closeButton;
    
    private bool _isDragging;
    private Vector2I _offset;

    public override void _Ready()
    {
        _closeButton.Pressed += CloseButtonOnPressed;
    }

    private void CloseButtonOnPressed()
    {
        GetWindow().QueueFree();
    }

    public override void _GuiInput(InputEvent @event)
    {
        if (@event is InputEventMouseButton mouseButton)
        {
            if (mouseButton.Pressed && mouseButton.ButtonIndex == MouseButton.Left)
            {
                _isDragging = true;
                _offset = DisplayServer.MouseGetPosition() - GetWindow().Position;
            }

            if (!mouseButton.Pressed && mouseButton.ButtonIndex == MouseButton.Left)
            {
                _isDragging = false;
            }
        }

        if (@event is InputEventMouseMotion mouseMotion)
        {
            if (_isDragging)
            {
                GetWindow().Position = DisplayServer.MouseGetPosition() - _offset;
            }
        }
    }

    public void SetContent(Control content)
    {
        _contentParent.AddChild(content);
    }
}