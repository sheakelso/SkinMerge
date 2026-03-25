using Godot;

namespace SkinMerge.UI;

public partial class DockableContentWindow : Window
{
    public DockableContentWindow(DockableWindowFrame frame, Control content)
    {
        Size = new Vector2I((int)content.GetParentControl().Size.X, (int)content.GetParentControl().Size.Y);
        Borderless = true;
        InitialPosition = WindowInitialPosition.Absolute;
        
        frame.SetAnchorsAndOffsetsPreset(Control.LayoutPreset.FullRect);
        AddChild(frame);

        DockableWindowTabContainer tabContainer = new DockableWindowTabContainer();
        tabContainer.SetAnchorsAndOffsetsPreset(Control.LayoutPreset.FullRect);
        content.Reparent(tabContainer);

        frame.SetContent(tabContainer);
    }
}