using Godot;
using System;
using System.Collections.Generic;

public partial class PlayerModel : Node3D
{
    [Export] public PlayerModelPart Head;
    [Export] public PlayerModelPart Body;
    [Export] public PlayerModelPart RightArm;
    [Export] public PlayerModelPart LeftArm;
    [Export] public PlayerModelPart RightLeg;
    [Export] public PlayerModelPart LeftLeg;
    
    public PlayerModelPart[] Parts => [Head, Body, RightArm, LeftArm, RightLeg, LeftLeg];

    public PaintableMesh[] PaintableMeshes
    {
        get
        {
            List<PaintableMesh> meshes = new List<PaintableMesh>();
            foreach (PlayerModelPart part in Parts)
            {
                meshes.AddRange(part.PaintableMeshes);
            }
            return meshes.ToArray();
        }
    }
    
    [Export] public Material DefaultSkinMaterial;
    [Export] public Material DefaultLayerMaterial;
    [Export] public Texture2D DefaultTexture;
    
    private Material _currentSkinMaterial;
    private Material _currentLayerMaterial;
    private Texture _currentTexture;

    public void SetMaterials(Material skin, Material layer)
    {
        _currentSkinMaterial = skin;
        _currentLayerMaterial = layer;
        foreach (PlayerModelPart part in Parts)
        {
            part.SetMaterials(skin, layer);
        }
    }

    public void SetTexture(Texture2D texture)
    {
        _currentTexture = texture;
        _currentSkinMaterial.Set("albedo_texture", texture);
        _currentLayerMaterial.Set("albedo_texture", texture);
    }
}
