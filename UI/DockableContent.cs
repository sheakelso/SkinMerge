using Godot;

namespace SkinMerge.UI;

public partial class DockableContent : Control
{
    public void Undock()
    {
        Window window = new Window();
        GetWindow().AddChild(window);

        window.Size = new Vector2I((int)Size.X / 2, (int)Size.Y / 2);
        window.Position = GetWindow().Position + new Vector2I((int)Position.X, (int)Position.Y);
        window.Borderless = true;
        window.Popup();
    }
}