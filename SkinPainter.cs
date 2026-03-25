using Godot;
using System;
using SkinMerge;

public partial class SkinPainter : Node
{
    public static SkinPainter Instance { get; private set; }
    
    [Export] public PlayerModel PlayerModel { get; set; }
    [Export] public ImageTexture Texture { get; set; }

    public override void _Ready()
    {
        Instance = this;

        PlayerModel.SetMaterials(PlayerModel.DefaultSkinMaterial, PlayerModel.DefaultLayerMaterial);

        Editor.CurrentProject.RootLayer.Updated += UpdateSkin;
        UpdateSkin(Editor.CurrentProject.RootLayer);
    }

    public void UpdateSkin(SkinLayer skin)
    {
        PlayerModel.SetTexture(skin.AsImageTexture());
    }
}
