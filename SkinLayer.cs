using Godot;

namespace SkinMerge;

public class SkinLayer : ISkinLayer
{
    private readonly Image _image;
    
    public SkinLayer(Image image)
    {
        _image = image;
        image.Convert(Image.Format.Rgba8);
    }

    public SkinLayer()
    {
        _image = new Image();
    }
    
    public Image AsImage() => _image;
}