namespace SkinMerge;

public enum ModelSide
{
    None = -1,
    Front,
    Back,
    Left,
    Right,
    Top,
    Bottom
}

public record struct ModelFace(Part part = Part.None, ModelSide side = ModelSide.None)
{
    public static ModelFace None => default;
    
    public ModelSide Side = side;
    public Part Part = part;
}