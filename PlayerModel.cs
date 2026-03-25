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
    
    [Export] public ShaderMaterial DefaultSkinMaterial;
    [Export] public ShaderMaterial DefaultLayerMaterial;
    [Export] public Texture2D DefaultTexture;
    
    private ShaderMaterial _currentSkinMaterial;
    private ShaderMaterial _currentLayerMaterial;
    private ImageTexture _currentTexture;

    [Export] private Color _pixelGridColor;

    public void SetMaterials(ShaderMaterial skin, ShaderMaterial layer)
    {
        _currentSkinMaterial = skin;
        _currentLayerMaterial = layer;
        foreach (PlayerModelPart part in Parts)
        {
            part.SetMaterials(skin, layer);
        }
    }

    public void SetTexture(ImageTexture texture)
    {
        _currentTexture = texture;
        _currentSkinMaterial.SetShaderParameter("albedo_texture", texture);
        _currentLayerMaterial.SetShaderParameter("albedo_texture", texture);
    }

    public void SetHoveredPixel(Vector2I pixel)
    {
        _currentSkinMaterial.SetShaderParameter("hovered_pixel", pixel);
        _currentLayerMaterial.SetShaderParameter("hovered_pixel", pixel);
    }

    public void SetPixelGridVisible(bool visible)
    {
        if (visible)
        {
            _currentSkinMaterial.SetShaderParameter("grid_color", _pixelGridColor);
            _currentLayerMaterial.SetShaderParameter("grid_color", _pixelGridColor);
        }
        else
        {
            _currentSkinMaterial.SetShaderParameter("grid_color", Color.Color8(0,0,0,0));
            _currentLayerMaterial.SetShaderParameter("grid_color", Color.Color8(0,0,0,0));
        }
    }

    public void SetOuterLayerVisible(bool visible)
    {
        foreach (PlayerModelPart part in Parts)
        {
            part.SetOuterLayerVisible(visible);
        }
    }
}
