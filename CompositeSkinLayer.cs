using System;
using System.Collections.Generic;
using Godot;
using Newtonsoft.Json;

namespace SkinMerge;

public partial class CompositeSkinLayer : SkinLayer
{
    public event Action<CompositeSkinLayer, SkinLayer> LayerRemoved;
    public event Action<CompositeSkinLayer, SkinLayer> LayerAdded;
    
    [JsonProperty("layers")] private readonly List<SkinLayer> _layers = new List<SkinLayer>();
    [JsonIgnore] private Image _cachedImage;
    [JsonIgnore] public SkinLayer[] Layers => _layers.ToArray();

    public CompositeSkinLayer(string name)
    {
        Name = name;
        GenerateImage();
    }
    
    public override Image AsImage() => _cachedImage;

    public void GenerateImage()
    {
        byte[] data = new byte[64 * 64 * 4];
        
        for (int i = _layers.Count - 1; i >= 0; i--)
        {
            if(!_layers[i].Visible) continue;
            byte[] layerData = _layers[i].AsImage().GetData();
            for (int x = 0; x < 64; x++)
            {
                for (int y = 0; y < 64; y++)
                {
                    int pixelIndex = (y * 64 + x) * 4;
                    float alpha = layerData[pixelIndex + 3] / 255f;
                    float currentAlpha = data[pixelIndex + 3] / 255f;
                    
                    byte newR =  (byte)Mathf.Lerp(data[pixelIndex], layerData[pixelIndex], alpha);
                    byte newG =  (byte)Mathf.Lerp(data[pixelIndex + 1], layerData[pixelIndex + 1], alpha);
                    byte newB = (byte)Mathf.Lerp(data[pixelIndex + 2], layerData[pixelIndex + 2], alpha);
                    byte newA = (byte)Mathf.RoundToInt(((1.0f - currentAlpha) * alpha + currentAlpha) * 255f);
                    
                    data[pixelIndex] = newR;
                    data[pixelIndex + 1] = newG;
                    data[pixelIndex + 2] = newB;
                    data[pixelIndex + 3] = newA;
                }
            }
        }

        _cachedImage = Image.CreateFromData(64, 64, false, Image.Format.Rgba8, data);
        InvokeUpdated();
    }

    public void AddLayer(SkinLayer layer)
    {
        layer.Updated += OnLayerUpdated;
        _layers.Add(layer);
        GenerateImage();
        LayerAdded?.Invoke(this, layer);
    }

    public void RemoveLayer(SkinLayer layer)
    {
        layer.Updated -= OnLayerUpdated;
        _layers.Remove(layer);
        GenerateImage();
        LayerRemoved?.Invoke(this, layer);
    }

    private void OnLayerUpdated(SkinLayer layer)
    {
        GenerateImage();
    }
}