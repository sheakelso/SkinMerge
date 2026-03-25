using Godot;
using Newtonsoft.Json;

namespace SkinMerge.Data;

public class ColorData
{
    [JsonProperty("r")] public byte R { get; set; }
    [JsonProperty("g")] public byte G { get; set; }
    [JsonProperty("b")] public byte B { get; set; }
    
    public ColorData(Color color)
    {
        R = (byte)color.R8;
        G = (byte)color.G8;
        B = (byte)color.B8;
    }

    public Color ToColor()
    {
        return Color.Color8(R, G, B);
    }
}