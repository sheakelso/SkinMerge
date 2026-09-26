using Godot;

namespace SkinMerge;

[GlobalClass]
public abstract partial class Tool : Resource
{
    [Export] public string Name { get; private set; }
    [Export] public Texture2D Icon { get; private set; }
    public abstract ToolField[] GetFields();
    public virtual void TextureMouseMotion(Part part, Vector2 position) { }
    public virtual void TextureMouseClicked(Part part, Vector2 position, MouseButton button, bool pressed) { }
    public virtual void Input(InputEvent @event) { }
    public virtual void FaceChanged(ModelFace newFace) { }
    public virtual void TextureMouseEntered(Part part) { }
    public virtual void TextureMouseExited(Part part) { }
}