using System;
using System.Collections.Generic;
using System.Linq;
using Godot;
using Newtonsoft.Json;

namespace SkinMerge.Data;

public class SkinProject
{
    [JsonProperty("rootLayer")] public CompositeSkinLayer RootLayer = new CompositeSkinLayer("Root");
    [JsonProperty("usingPrimaryColor")] public bool UsingPrimaryColor = true;
    [JsonProperty("palettes")] private List<ColorPalette> _palettes = new List<ColorPalette>();
    
    [JsonProperty("primaryColor")] private ColorData _primaryColorData = new ColorData(Color.Color8(255, 255, 255));
    [JsonProperty("secondaryColor")] private ColorData _secondaryColorData = new ColorData(Color.Color8(0, 0, 0));
    
    [JsonIgnore] public string ProjectName { get; set; }
    
    [JsonIgnore] public ColorPalette[] Palettes => _palettes.ToArray();
    [JsonIgnore] public Color PrimaryColor
    {
        get => _primaryColorData.ToColor();
        set
        {
            _primaryColorData = new ColorData(value);
            ColorsChanged?.Invoke(PrimaryColor, SecondaryColor);
        }
    }
    [JsonIgnore] public Color SecondaryColor
    {
        get => _secondaryColorData.ToColor();
        set
        {
            _secondaryColorData = new ColorData(value);
            ColorsChanged?.Invoke(PrimaryColor, SecondaryColor);
        }
    }
    
    public Color GetCurrentColor()
    {
        if (UsingPrimaryColor) return PrimaryColor;
        else return SecondaryColor;
    }

    public void SetCurrentColor(Color color)
    {
        if(UsingPrimaryColor) PrimaryColor = color;
        else SecondaryColor = color;
    }

    public ColorPalette CreatePalette(string name)
    {
        ColorPalette palette = new ColorPalette(name);
        AddPalette(palette);
        return palette;
    }

    public void AddPalette(ColorPalette palette)
    {
        _palettes.Add(palette);
        PaletteAdded?.Invoke(palette);
    }

    public void Save()
    {
        string json = JsonConvert.SerializeObject(this, new JsonSerializerSettings
        {
            Formatting = Formatting.Indented,
            NullValueHandling = NullValueHandling.Ignore,
            TypeNameHandling = TypeNameHandling.Auto
        });

        FileAccess file = FileAccess.Open("user://projects/" + ProjectName, FileAccess.ModeFlags.Write);
        file.StoreString(json);
        file.Close();
    }
    
    public static SkinProject LoadOrCreateProject(string fileName)
    {
        if (!DirAccess.DirExistsAbsolute("user://projects"))
        {
            DirAccess.MakeDirAbsolute("user://projects");
        }

        if (FileAccess.FileExists("user://projects/" + fileName))
        {
            FileAccess access = FileAccess.Open("user://projects/" + fileName, FileAccess.ModeFlags.Read);
            string json = access.GetAsText();
            SkinProject project = JsonConvert.DeserializeObject<SkinProject>(json, new JsonSerializerSettings
            {
                Formatting = Formatting.Indented,
                NullValueHandling = NullValueHandling.Ignore,
                TypeNameHandling = TypeNameHandling.Auto
            });
            project.ProjectName = fileName;
            return project;
        }
        else
        {
            SkinProject newProject = new SkinProject();
            newProject.ProjectName = fileName;
            return newProject;
        }
    }

    public event Action<Color, Color> ColorsChanged;
    public event Action<ColorPalette> PaletteAdded;
}