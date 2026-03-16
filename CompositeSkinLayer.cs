using System;
using System.Collections.Generic;
using Godot;

namespace SkinMerge;

public partial class CompositeSkinLayer : ISkinLayer
{
    private readonly List<ISkinLayer> _layers = new List<ISkinLayer>();
    
    public Image AsImage()
    {
        byte[] data = new byte[64 * 64 * 4];
        
        for (int i = 0; i < _layers.Count; i++)
        {
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
                    byte newA = (byte)Mathf.RoundToInt((1.0f - currentAlpha) * alpha);
                    
                    data[pixelIndex] = newR;
                    data[pixelIndex + 1] = newG;
                    data[pixelIndex + 2] = newB;
                    data[pixelIndex + 3] = newA;
                }
            }
        }

        return Image.CreateFromData(64, 64, false, Image.Format.Rgba8, data);
    }

    public void AddLayer(ISkinLayer layer)
    {
        _layers.Add(layer);
    }
}