using Godot;
using Godot.Collections;

namespace SkinMerge.UI;

[Tool]
public partial class DockableSplitContainer : SplitContainer
{
    public DockableLayout DockableLayout;
    
    [Export] private Control _visibilityContainer;
    
    public DockableSplitContainer()
    {
        ChildExitingTree += UpdateVisibilityDeferred;
        ChildEnteredTree += UpdateVisibilityDeferred;
    }

    public override void _Ready()
    {
        UpdateVisibility();
    }

    private void UpdateVisibility()
    {
        if(GetChildCount() == 0) SetVisibility(false);
        else SetVisibility(true);
    }

    private void UpdateVisibilityDeferred(Node node)
    {
        CallDeferred(MethodName.UpdateVisibility);
    }
    
    public void SetVisibility(bool visible)
    {
        if(_visibilityContainer == null) Visible = visible;
        else _visibilityContainer.Visible = visible;
    }

    public void DropTab(int index, Control tabControl, DockableTabContainer from)
    {
        DockableTabContainer tabContainer = new DockableTabContainer();
        tabContainer.SetAnchorsAndOffsetsPreset(LayoutPreset.FullRect);
        tabContainer.DragToRearrangeEnabled = true;
        tabContainer.TabsRearrangeGroup = from.TabsRearrangeGroup;
        
        AddChild(tabContainer);
        MoveChild(tabContainer, index);
        tabControl.Reparent(tabContainer);
    }
}