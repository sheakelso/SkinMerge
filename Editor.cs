using System;
using System.Collections.Generic;
using System.Linq;
using Godot;
using Newtonsoft.Json;
using SkinMerge.Data;

namespace SkinMerge;

public partial class Editor : Node
{
    public static Editor Instance { get; private set; }
    public static SkinProject CurrentProject { get; private set; }
    public SkinLayer[] SelectedLayers { get; private set; }
    public SkinLayer FocusedLayer { get; private set; }
    private Color _primaryColor = Color.Color8(255, 255, 255);
    private Color _secondaryColor = Color.Color8(0, 0, 0);
    public ImageSkinLayer PaintingLayer => FocusedLayer is ImageSkinLayer ? (ImageSkinLayer)FocusedLayer : null;

    public Editor()
    {
        Instance = this;
        CurrentProject = SkinProject.LoadOrCreateProject("project.skp");
        CurrentProject.RootLayer.GenerateImage();
    }

    public void UpdateSelectedLayers(SkinLayer[] selectedLayers, SkinLayer focusedLayer)
    {
        SelectedLayers = selectedLayers;
        FocusedLayer = focusedLayer;
    }
}