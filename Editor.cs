using System;
using System.Collections.Generic;
using System.Linq;
using Godot;
using Newtonsoft.Json;
using SkinMerge.Data;

namespace SkinMerge;

public partial class Editor : Control
{
    public static Editor Instance { get; private set; }
    public static SkinProject CurrentProject { get; private set; }
    public SkinLayer[] SelectedLayers { get; private set; }
    public SkinLayer FocusedLayer { get; private set; }
    private Color _primaryColor = Color.Color8(255, 255, 255);
    private Color _secondaryColor = Color.Color8(0, 0, 0);
    public ImageSkinLayer PaintingLayer => FocusedLayer is ImageSkinLayer ? (ImageSkinLayer)FocusedLayer : null;

    public event Action EditorReady;
    public event Action<Tool> ToolSelected;
    
    [Export] public Tool[] Tools { get; private set; }
    [Export] public Shortcut SaveShortcut;
    
    [Export] private Tool _selectedTool;

    public Tool SelectedTool
    {
        get => _selectedTool;
        set
        {
            _selectedTool = value;
            ToolSelected?.Invoke(value);
        }
    }

    public Editor()
    {
        Instance = this;
        CurrentProject = SkinProject.LoadOrCreateProject("project.skp");
        CurrentProject.RootLayer.GenerateImage();
    }

    public override void _Ready()
    {
        SelectedTool = Tools[0];
        EditorReady?.Invoke();
        ToolSelected?.Invoke(_selectedTool);
    }

    public void UpdateSelectedLayers(SkinLayer[] selectedLayers, SkinLayer focusedLayer)
    {
        SelectedLayers = selectedLayers;
        FocusedLayer = focusedLayer;
    }

    public override void _UnhandledKeyInput(InputEvent @event)
    {
        if (SaveShortcut.MatchesEvent(@event))
        {
            CurrentProject.Save();
        }
    }
}