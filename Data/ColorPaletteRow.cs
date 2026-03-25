using System;
using System.Collections.Generic;
using Godot;
using Newtonsoft.Json;

namespace SkinMerge.Data;

public class ColorPaletteRow
{
    [JsonProperty("name")] public string Name;
    [JsonProperty("colors")] private List<ColorData> _colors = new List<ColorData>();
    [JsonIgnore] public Color[] Colors => _colors.ConvertAll(c => c.ToColor()).ToArray();

    public ColorPaletteRow(string name)
    {
        Name = name;
    }

    public void AddColor(Color color) => _colors.Add(new ColorData(color));
    public void RemoveColor(int index) => _colors.RemoveAt(index);

    public void MoveColor(int from, int to)
    {
        if(from < 0 || from >= _colors.Count) return;
        to = Math.Clamp(to, 0, _colors.Count - 1);
        
        ColorData color = _colors[from];
        _colors.RemoveAt(from);
        _colors.Insert(to, color);
    }
}