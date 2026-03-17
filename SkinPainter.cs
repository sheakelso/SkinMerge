using Godot;
using System;
using SkinMerge;

public partial class SkinPainter : Node
{
    [Export] public PlayerModel PlayerModel { get; set; }
    [Export] public Image DefaultTexture { get; set; }
    [Export] public Image RedTexture { get; set; }
    
    private CompositeSkinLayer _skin;

    public override void _Ready()
    {
        PlayerModel.SetMaterials(PlayerModel.DefaultSkinMaterial, PlayerModel.DefaultLayerMaterial);
        
        _skin = new CompositeSkinLayer();
        SkinLayer layer = new SkinLayer(DefaultTexture);
        SkinLayer layer2 = new SkinLayer(RedTexture);
        _skin.AddLayer(layer);
        _skin.AddLayer(layer2);
        
        Image image = _skin.AsImage();
        ImageTexture imageTexture = ImageTexture.CreateFromImage(image);
        
        PlayerModel.SetTexture(imageTexture);
    }
}
