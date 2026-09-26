using Godot;
using System.Collections.Generic;

namespace SkinMerge;

public static class UVMappings
{
    
    public static readonly Rect2I[] HeadUv =
    [
        new Rect2I(new Vector2I(8, 8), new Vector2I(8, 8)), //front
        new Rect2I(new Vector2I(24, 8), new Vector2I(8, 8)), //back
        new Rect2I(new Vector2I(16, 8), new Vector2I(8, 8)), //left
        new Rect2I(new Vector2I(0, 8), new Vector2I(8, 8)), //right
        new Rect2I(new Vector2I(8, 0), new Vector2I(8, 8)), //top
        new Rect2I(new Vector2I(16, 0), new Vector2I(8, 8)) //bottom
    ];
    
    public static readonly Rect2I[] HatUv =
    [
        new Rect2I(new Vector2I(40, 8), new Vector2I(8, 8)),
        new Rect2I(new Vector2I(56, 8), new Vector2I(8, 8)),
        new Rect2I(new Vector2I(48, 8), new Vector2I(8, 8)),
        new Rect2I(new Vector2I(32, 8), new Vector2I(8, 8)),
        new Rect2I(new Vector2I(40, 0), new Vector2I(8, 8)),
        new Rect2I(new Vector2I(48, 0), new Vector2I(8, 8))
    ];
    
    public static readonly Rect2I[] BodyLayerUv =
    [
        new Rect2I(new Vector2I(20, 36), new Vector2I(8, 12)),
        new Rect2I(new Vector2I(32, 36), new Vector2I(8, 12)),
        new Rect2I(new Vector2I(28, 36), new Vector2I(4, 12)),
        new Rect2I(new Vector2I(16, 36), new Vector2I(4, 12)),
        new Rect2I(new Vector2I(20, 32), new Vector2I(8, 4)),
        new Rect2I(new Vector2I(28, 32), new Vector2I(8, 4)),
    ];
    
    public static readonly Rect2I[] BodyUv =
    [
        new Rect2I(new Vector2I(20, 20), new Vector2I(8, 12)),
        new Rect2I(new Vector2I(32, 20), new Vector2I(8, 12)),
        new Rect2I(new Vector2I(28, 20), new Vector2I(4, 12)),
        new Rect2I(new Vector2I(16, 20), new Vector2I(4, 12)),
        new Rect2I(new Vector2I(20, 16), new Vector2I(8, 4)),
        new Rect2I(new Vector2I(28, 16), new Vector2I(8, 4)),
    ];

    public static readonly Rect2I[] RightArmUv =
    [
        new Rect2I(new Vector2I(44, 20), new Vector2I(4, 12)),
        new Rect2I(new Vector2I(52, 20), new Vector2I(4, 12)),
        new Rect2I(new Vector2I(48, 20), new Vector2I(4, 12)),
        new Rect2I(new Vector2I(40, 20), new Vector2I(4, 12)),
        new Rect2I(new Vector2I(44, 16), new Vector2I(4, 4)),
        new Rect2I(new Vector2I(48, 16), new Vector2I(4, 4))
    ];
    
    public static readonly Rect2I[] RightArmLayerUv =
    [
        new Rect2I(new Vector2I(44, 36), new Vector2I(4, 12)),
        new Rect2I(new Vector2I(52, 36), new Vector2I(4, 12)),
        new Rect2I(new Vector2I(48, 36), new Vector2I(4, 12)),
        new Rect2I(new Vector2I(40, 36), new Vector2I(4, 12)),
        new Rect2I(new Vector2I(44, 32), new Vector2I(4, 4)),
        new Rect2I(new Vector2I(48, 32), new Vector2I(4, 4))
    ];
    
    public static readonly Rect2I[] RightLegUv =
    [
        new Rect2I(new Vector2I(4, 20), new Vector2I(4, 12)),
        new Rect2I(new Vector2I(12, 20), new Vector2I(4, 12)),
        new Rect2I(new Vector2I(8, 20), new Vector2I(4, 12)),
        new Rect2I(new Vector2I(0, 20), new Vector2I(4, 12)),
        new Rect2I(new Vector2I(4, 16), new Vector2I(4, 4)),
        new Rect2I(new Vector2I(8, 16), new Vector2I(4, 4))
    ];
    
    public static readonly Rect2I[] RightLegLayerUv =
    [
        new Rect2I(new Vector2I(4, 36), new Vector2I(4, 12)),
        new Rect2I(new Vector2I(12, 36), new Vector2I(4, 12)),
        new Rect2I(new Vector2I(8, 36), new Vector2I(4, 12)),
        new Rect2I(new Vector2I(0, 36), new Vector2I(4, 12)),
        new Rect2I(new Vector2I(4, 32), new Vector2I(4, 4)),
        new Rect2I(new Vector2I(8, 32), new Vector2I(4, 4))
    ];

    public static readonly Rect2I[] LeftLegLayerUv =
    [
        new Rect2I(new Vector2I(4, 52), new Vector2I(4, 12)),
        new Rect2I(new Vector2I(12, 52), new Vector2I(4, 12)),
        new Rect2I(new Vector2I(8, 52), new Vector2I(4, 12)),
        new Rect2I(new Vector2I(0, 52), new Vector2I(4, 12)),
        new Rect2I(new Vector2I(4, 48), new Vector2I(4, 4)),
        new Rect2I(new Vector2I(8, 48), new Vector2I(4, 4))
    ];
    
    public static readonly Rect2I[] LeftLegUv =
    [
        new Rect2I(new Vector2I(20, 52), new Vector2I(4, 12)),
        new Rect2I(new Vector2I(28, 52), new Vector2I(4, 12)),
        new Rect2I(new Vector2I(24, 52), new Vector2I(4, 12)),
        new Rect2I(new Vector2I(16, 52), new Vector2I(4, 12)),
        new Rect2I(new Vector2I(20, 48), new Vector2I(4, 4)),
        new Rect2I(new Vector2I(24, 48), new Vector2I(4, 4))
    ];
    
    public static readonly Rect2I[] LeftArmUv =
    [
        new Rect2I(new Vector2I(36, 52), new Vector2I(4, 12)),
        new Rect2I(new Vector2I(44, 52), new Vector2I(4, 12)),
        new Rect2I(new Vector2I(40, 52), new Vector2I(4, 12)),
        new Rect2I(new Vector2I(32, 52), new Vector2I(4, 12)),
        new Rect2I(new Vector2I(36, 48), new Vector2I(4, 4)),
        new Rect2I(new Vector2I(40, 48), new Vector2I(4, 4))
    ];
    
    public static readonly Rect2I[] LeftArmLayerUv =
    [
        new Rect2I(new Vector2I(52, 52), new Vector2I(4, 12)),
        new Rect2I(new Vector2I(60, 52), new Vector2I(4, 12)),
        new Rect2I(new Vector2I(56, 52), new Vector2I(4, 12)),
        new Rect2I(new Vector2I(48, 52), new Vector2I(4, 12)),
        new Rect2I(new Vector2I(52, 48), new Vector2I(4, 4)),
        new Rect2I(new Vector2I(56, 48), new Vector2I(4, 4))
    ];
    
    public static readonly Dictionary<Part, Rect2I[]> PlayerUvs = new()
    {
        {Part.Head, HeadUv},
        {Part.Body, BodyUv},
        {Part.LeftArm, LeftArmUv},
        {Part.RightArm, RightArmUv},
        {Part.LeftLeg, LeftLegUv},
        {Part.RightLeg, RightLegUv},
        {Part.HeadLayer, HatUv},
        {Part.RightLegLayer, RightLegLayerUv},
        {Part.RightArmLayer, RightArmLayerUv},
        {Part.LeftLegLayer, LeftLegLayerUv},
        {Part.LeftArmLayer, LeftArmLayerUv},
        {Part.BodyLayer, BodyLayerUv}
    };

    public static Rect2I GetFaceRect(ModelFace face)
    {
        return PlayerUvs[face.Part][(int)face.Side];
    }

    public static ModelFace GetModelFaceFromPixel(Vector2I pixel)
    {
        foreach (KeyValuePair<Part, Rect2I[]> kv in PlayerUvs)
        {
            for (int i = 0; i < kv.Value.Length; i++)
            {
                if(kv.Value[i].HasPoint(pixel)) return new ModelFace(kv.Key, (ModelSide)i);
            }
        }

        return ModelFace.None;
    }
}