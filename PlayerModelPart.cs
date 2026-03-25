using Godot;
using System;

public partial class PlayerModelPart : Node3D
{
    [Export] public PaintableMesh Skin;
    [Export] public PaintableMesh Layer;
    
    public PaintableMesh[] PaintableMeshes => [Skin];

    public void SetMaterials(ShaderMaterial skin, ShaderMaterial layer)
    {
        Skin.Mesh.MaterialOverride = skin;
        Layer.Mesh.MaterialOverride = layer;
    }

    public void SetOuterLayerVisible(bool visible)
    {
        Layer.Mesh.Visible = visible;
    }
}
