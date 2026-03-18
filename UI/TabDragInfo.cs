using Godot;

namespace SkinMerge.UI;

public class TabDragInfo
{
    public readonly long TabIndex;
    public readonly Vector2 StartPosition;
    
    public bool Dragging;

    public TabDragInfo(long tabIndex, Vector2 startPosition)
    {
        TabIndex = tabIndex;
        StartPosition = startPosition;
        Dragging = false;
    }

    public void StartDragging()
    {
        Dragging = true;
    }
}