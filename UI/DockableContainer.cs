using Godot;
using Godot.Collections;

namespace SkinMerge.UI;

[Tool]
[GlobalClass]
public partial class DockableContainer : TabContainer
{
    private TabBar _tabBar;
    
    public DockableContainer()
    {
        _tabBar = GetChildren(true)[0] as TabBar;
        _tabBar?.SetDragForwarding(OnTabDragCallable, CanTabDropCallable, OnTabDropCallable);
    }

    private Callable OnTabDragCallable => new(this, MethodName.OnTabDrag);
    private void OnTabDrag(Vector2 position)
    {
        ForceDrag(5, new Control());
    }

    private Callable CanTabDropCallable => new(this, MethodName.CanTabDrop);
    private void CanTabDrop(Vector2 position, Variant variant)
    {
        
    }

    private Callable OnTabDropCallable => new(this, MethodName.OnTabDrop);
    private void OnTabDrop(Vector2 position, Variant variant)
    {
        
    }
}