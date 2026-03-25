using Godot;
using Godot.Collections;

namespace SkinMerge.UI;

public partial class DockableLayout : Control
{
    [Export] private DockableSplitContainer _leftSplitContainer = null;
    [Export] private DockableSplitContainer _rightSplitContainer = null;
    [Export] private DockableSplitContainer _bottomSplitContainer = null;
    [Export] private DockableSplitContainer _topSplitContainer = null;
    [Export] private Control _mainPanel = null;
    [Export] private DropBoxRenderer _dropBoxRenderer = null;
    [Export] private PackedScene _windowFrameScene;
    public DropBoxRenderer DropBoxRenderer => _dropBoxRenderer;
    
    private Dictionary _currentDragData;

    public override void _Notification(int what)
    {
        if (what == NotificationDragBegin)
        {
            _currentDragData = GetViewport().GuiGetDragData().AsGodotDictionary();
        }
        if (what == NotificationDragEnd && !GetViewport().GuiIsDragSuccessful())
        {
            GD.Print(_currentDragData);
            if(_currentDragData["type"].AsString() != "tab") return;

            string path = _currentDragData["from_path"].AsString();
            int index = path.LastIndexOf('/');
            path = path.Substring(0, index);
        
            DockableTabContainer tabContainer = GetNode(path) as DockableTabContainer;
            Control tabControl = tabContainer.GetChild(_currentDragData["tab_index"].AsInt32()) as Control;

            DockableWindowFrame frame = _windowFrameScene.Instantiate<DockableWindowFrame>();

            Window tabWindow = new DockableContentWindow(frame, tabControl);
            GetWindow().AddChild(tabWindow);
        }
    }

    public override void _Ready()
    {
        if(_leftSplitContainer != null) _leftSplitContainer.DockableLayout = this;
        if(_rightSplitContainer != null) _rightSplitContainer.DockableLayout = this;
        if(_bottomSplitContainer != null) _bottomSplitContainer.DockableLayout = this;
        if(_topSplitContainer != null) _topSplitContainer.DockableLayout = this;
    }
}