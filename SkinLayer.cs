using System;
using Godot;
using Newtonsoft.Json;

namespace SkinMerge;

public abstract class SkinLayer
{
    [JsonProperty("name")] public string Name;
    [JsonProperty("visible")] private bool _visible = true;

    [JsonIgnore] public bool Visible
    {
        get => _visible;
        set
        {
            _visible = value;
            Updated?.Invoke(this);
        }
    }
    
    public event Action<SkinLayer> Updated;
    public abstract Image AsImage();
    public ImageTexture AsImageTexture() => ImageTexture.CreateFromImage(AsImage());
    protected void InvokeUpdated() => Updated?.Invoke(this);
}