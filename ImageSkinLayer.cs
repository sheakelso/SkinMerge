using System;
using Godot;
using Newtonsoft.Json;

namespace SkinMerge;

public class ImageSkinLayer : SkinLayer
{
    [JsonIgnore] private readonly Image _image;
    [JsonProperty("image")] public string Base64 => Convert.ToBase64String(_image.SavePngToBuffer());

    [JsonConstructor]
    public ImageSkinLayer(string image)
    {
        _image = new Image();
        _image.LoadPngFromBuffer(Convert.FromBase64String(image));
    }
    
    public ImageSkinLayer(Image image, string name = "New Layer")
    {
        Name = name;
        _image = image;
        image.Convert(Image.Format.Rgba8);
    }

    public ImageSkinLayer(string name, Image image = null)
    {
        Name = name;
        _image = Image.CreateEmpty(64, 64, false, Image.Format.Rgba8);
    }
    
    public override Image AsImage() => _image;

    public void SetPixel(int x, int y, Color color)
    {
        _image.SetPixel(x, y, color);
        InvokeUpdated();
    }
}