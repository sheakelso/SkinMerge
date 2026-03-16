using Godot;
using System;

public partial class PlayerModelPart : Node
{
    [Export] public PaintableMesh Skin;
    [Export] public PaintableMesh Layer;
    
    public PaintableMesh[] PaintableMeshes => [Skin];

    public void SetMaterials(Material skin, Material layer)
    {
        Skin.Mesh.MaterialOverride = skin;
        //Layer.Mesh.MaterialOverride = layer;
    }
}
