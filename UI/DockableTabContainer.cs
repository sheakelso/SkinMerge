using Godot;
using Godot.Collections;

namespace SkinMerge.UI;

public partial class DockableTabContainer : TabContainer
{
    private TabBar _tabBar;
    private DropBoxRenderer _dropBoxRenderer;
    
    private DockableSplitContainer SplitContainer => GetParent() as DockableSplitContainer;
    private DropBoxRenderer DropBoxRenderer => SplitContainer.DockableLayout.DropBoxRenderer;
    
    private Rect2 TopDropBoxRect => new(Vector2.Zero, Size.X, DropBoxRenderer.DropBoxWidth);
    private Rect2 BottomDropBoxRect => new(0f, Size.Y - DropBoxRenderer.DropBoxWidth, Size.X, DropBoxRenderer.DropBoxWidth);
    private Rect2 LeftDropBoxRect => new(Vector2.Zero, DropBoxRenderer.DropBoxWidth, Size.Y);
    private Rect2 RightDropBoxRect => new(Size.X - DropBoxRenderer.DropBoxWidth, 0f, DropBoxRenderer.DropBoxWidth, Size.Y);

    public override void _GuiInput(InputEvent @event)
    {
        if (@event is InputEventMouseButton mouseButton)
        {
            GD.Print(mouseButton.Position);
        }
    }

    public override void _Ready()
    {
        _tabBar = GetChild<TabBar>(0, true);

        _tabBar.MouseFilter = MouseFilterEnum.Pass;
        
        DragToRearrangeEnabled = true;
        TabsRearrangeGroup = 0;
        MouseExited += OnMouseExited;
        ChildEnteredTree += (_) => FreeIfEmptyDeferred();
        ChildExitingTree += (_) => FreeIfEmptyDeferred();
    }

    public override bool _CanDropData(Vector2 atPosition, Variant data)
    {
        if (SplitContainer == null) return true;
        return UpdateDropBoxRect(atPosition);
    }

    public override void _DropData(Vector2 atPosition, Variant data)
    {
        if (SplitContainer == null) return;
        Dictionary dict = data.AsGodotDictionary();
        if(dict["type"].AsString() != "tab") return;

        string path = dict["from_path"].AsString();
        int index = path.LastIndexOf('/');
        path = path.Substring(0, index);
        
        DockableTabContainer tabContainer = GetNode(path) as DockableTabContainer;
        Control tabControl = tabContainer.GetChild(dict["tab_index"].AsInt32()) as Control;
        
        SplitContainer.DropTab(GetDropIndex(atPosition), tabControl, this);
        
        DropBoxRenderer.HideDropBox(this);
    }

    private void FreeIfEmpty()
    {
        if(GetChildCount() == 0) QueueFree();
    }
    
    private void FreeIfEmptyDeferred()
    {
        CallDeferred(MethodName.FreeIfEmpty);
    }
    
    private void OnMouseExited()
    {
        if (SplitContainer == null) return;
        DropBoxRenderer.HideDropBox(this);
    }

    private bool UpdateDropBoxRect(Vector2 atPosition)
    {
        Vector2 atOpposite = Size - atPosition;
        if (SplitContainer.Vertical)
        {
            if (atPosition.Y - _tabBar.Size.Y < DropBoxRenderer.DropAreaWidth)
            {
                DropBoxRenderer.ShowDropBox(this, TopDropBoxRect);
                return true;
            }
            else if (atOpposite.Y < DropBoxRenderer.DropAreaWidth)
            {
                DropBoxRenderer.ShowDropBox(this, BottomDropBoxRect);
                return true;
            }
            else DropBoxRenderer.HideDropBox(this);
        }
        else
        {
            if (atPosition.X < DropBoxRenderer.DropAreaWidth)
            {
                DropBoxRenderer.ShowDropBox(this, LeftDropBoxRect);
                return true;
            }
            else if (atOpposite.X < DropBoxRenderer.DropAreaWidth)
            {
                DropBoxRenderer.ShowDropBox(this, RightDropBoxRect);
                return true;
            }
            else DropBoxRenderer.HideDropBox(this);
        }
        
        return false;
    }

    private int GetDropIndex(Vector2 atPosition)
    {
        if (SplitContainer.Vertical)
        {
            if(atPosition.Y < Size.Y / 2) return GetIndex();
            return GetIndex() + 1;
        }
        else
        {
            if(atPosition.X < Size.X / 2) return GetIndex();
            return GetIndex() + 1;
        }
    }
    
}