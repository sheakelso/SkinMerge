using System.Collections.Generic;
using Godot;
using Newtonsoft.Json;

namespace SkinMerge.Data;

public class ColorPalette
{
    [JsonProperty("name")] public string Name;
    [JsonProperty("rows")] private List<ColorPaletteRow> _rows = new List<ColorPaletteRow>();
    [JsonIgnore] public ColorPaletteRow[] Rows => _rows.ToArray();

    public ColorPalette(string name)
    {
        Name = name;
    }

    public ColorPaletteRow this[int index] => _rows[index];

    public ColorPaletteRow AddNewRow(string name)
    {
        ColorPaletteRow row = new ColorPaletteRow(name);
        _rows.Add(row);
        return row;
    }
    public void RemoveRow(int index) => _rows.RemoveAt(index);
}