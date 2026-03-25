using Godot;

namespace SkinMerge.UI.EditorUI;

public partial class EditorContent : Control
{
    [Export] protected Editor Editor { get; private set; }

    public override void _Ready()
    {
        if(Editor != null) Initialize();
    }

    public void SetEditor(Editor editor)
    {
        Editor = editor;
        Initialize();
    }

    protected virtual void Initialize()
    {
        
    }
}