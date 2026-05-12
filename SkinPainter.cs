using Godot;
using System;
using SkinMerge;

public enum Part
{
    Head,
    Body,
    LeftArm,
    RightArm,
    LeftLeg,
    RightLeg,
    
    UV
}

public partial class SkinPainter : Node
{
    public static SkinPainter Instance { get; private set; }
    [Export] public PlayerModel PlayerModel { get; set; }
    [Export] public ShaderMaterial ModelMaterial;

    public override void _Ready()
    {
        Instance = this;

        PlayerModel = PlayerModel.Instance;
        PlayerModel.SetMaterials(ModelMaterial, ModelMaterial);

        Editor.CurrentProject.RootLayer.Updated += UpdateSkin;
        UpdateSkin(Editor.CurrentProject.RootLayer);
    }

    public void UpdateSkin(SkinLayer skin)
    {
        PlayerModel.SetTexture(skin.AsImageTexture());
    }

    private Vector2I _hoveredPixel;
    public void MouseMotion(Part part, Vector2 position)
    {
        Editor.Instance.SelectedTool.TextureMouseMotion(part, position);
    }

    public void MouseClicked(Part part, Vector2 position, MouseButton button, bool pressed)
    {
        Editor.Instance.SelectedTool.TextureMouseClicked(part, position, button, pressed);
    }

    public void MouseEntered(Part part)
    {
        Editor.Instance.SelectedTool.TextureMouseEntered(part);
    }
    
    public void MouseExited(Part part)
    {
        Editor.Instance.SelectedTool.TextureMouseExited(part);
    }

    public void FaceChanged()
    {
        Editor.Instance.SelectedTool.FaceChanged();
    }

    public override void _Input(InputEvent @event)
    {
        Editor.Instance.SelectedTool.Input(@event);
    }
}
