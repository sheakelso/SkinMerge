using Godot;

namespace SkinMerge.UI;

public partial class DropBoxRenderer : Control
{
    [Export] public float DropBoxWidth = 1f;
    [Export] public float DropAreaWidth = 1f;
    [Export] public StyleBox DropBoxStyleBox;
    
    private Rect2 _dropBoxRect;
    private DockableTabContainer _dropTabContainer;

    public override void _Draw()
    {
        if (_dropTabContainer != null) DrawStyleBox(DropBoxStyleBox, _dropBoxRect);
    }
    
    public void ShowDropBox(DockableTabContainer tabContainer, Rect2 dropBoxRect)
    {
        _dropTabContainer = tabContainer;
        _dropBoxRect = dropBoxRect;
        _dropBoxRect.Position += tabContainer.GlobalPosition;
        QueueRedraw();
    }

    public void HideDropBox(DockableTabContainer tabContainer)
    {
        if(_dropTabContainer == tabContainer) _dropTabContainer = null;
        QueueRedraw();
    }
}