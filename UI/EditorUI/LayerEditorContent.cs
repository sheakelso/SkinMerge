using System.Collections.Generic;
using Godot;

namespace SkinMerge.UI.EditorUI;

public partial class LayerEditorContent : EditorContent
{
    [Export] private Button _newLayerButton;
    [Export] private Button _newFolderButton;
    [Export] private Button _deleteButton;
    [Export] private Control _treeParent;
    [Export] private Texture2D _visibleButtonTexture;
    [Export] private Texture2D _hiddenButtonTexture;
    
    private Tree _layerTree;
    private Dictionary<TreeItem, SkinLayer> _skinLayers;
    
    protected override void Initialize()
    {
        _layerTree = new Tree();
        _layerTree.Columns = 3;
        _layerTree.SetColumnExpand(1, true);
        _layerTree.SetColumnTitle(1, "Layer");
        _layerTree.SetColumnExpand(0, false);
        _layerTree.SetColumnTitle(0, "Visible");
        _layerTree.SetColumnExpand(2, false);
        _layerTree.SetColumnTitle(2, "Preview");
        _layerTree.SelectMode = Tree.SelectModeEnum.Multi;
        _layerTree.HideRoot = true;
        _layerTree.ThemeTypeVariation = "LayerTree";
        _layerTree.SetAnchorsAndOffsetsPreset(LayoutPreset.FullRect);
        
        _treeParent.AddChild(_layerTree);
        LoadTree();

        _layerTree.EmptyClicked += TreeOnEmptyClicked;
        _layerTree.MultiSelected += TreeOnMultiSelected;
        _layerTree.ItemEdited += OnItemEdited;

        _newLayerButton.Pressed += OnNewLayerClicked;
        _newFolderButton.Pressed += OnNewFolderClicked;
        _deleteButton.Pressed += OnDeleteClicked;
    }
    

    private TreeItem GetTreeItem(SkinLayer layer)
    {
        foreach (TreeItem item in _skinLayers.Keys)
        {
            if(_skinLayers[item] == layer) return item;
        }
        return null;
    }

    private void OnNewLayerClicked()
    {
        if (Editor.FocusedLayer is CompositeSkinLayer compositeLayer)
        {
            compositeLayer.AddLayer(new ImageSkinLayer("New Layer"));
        }
        else Editor.CurrentProject.RootLayer.AddLayer(new ImageSkinLayer("New Layer"));
    }

    private void OnNewFolderClicked()
    {
        if (Editor.FocusedLayer is CompositeSkinLayer compositeLayer)
        {
            compositeLayer.AddLayer(new CompositeSkinLayer("New Folder"));
        }
        else Editor.CurrentProject.RootLayer.AddLayer(new CompositeSkinLayer("New Folder"));
    }

    private void OnDeleteClicked()
    {
        TreeItem selectedItem = _layerTree.GetSelected();
        if(_skinLayers[selectedItem.GetParent()] is CompositeSkinLayer parentLayer) 
            parentLayer.RemoveLayer(_skinLayers[selectedItem]);
    }

    private void TreeOnEmptyClicked(Vector2 pos, long index)
    {
        _layerTree.DeselectAll();
        UpdateSelection();
    }
    
    private void TreeOnMultiSelected(TreeItem item, long column, bool selected)
    {
        UpdateSelection();
    }

    private void UpdateSelection()
    {
        Editor.UpdateSelectedLayers(GetSelectedLayers(), GetFocusedLayer());
    }

    private SkinLayer[] GetSelectedLayers()
    {
        List<SkinLayer> selectedLayers = new List<SkinLayer>();

        TreeItem selectedItem = _layerTree.GetNextSelected(null);
        while (selectedItem != null)
        {
            selectedLayers.Add(_skinLayers[selectedItem]);
            selectedItem = _layerTree.GetNextSelected(selectedItem);
        }
        
        return selectedLayers.ToArray();
    }

    private SkinLayer GetFocusedLayer()
    {
        TreeItem focusedItem = _layerTree.GetSelected();
        return focusedItem != null ? _skinLayers[focusedItem] : null;
    }

    private void LoadTree()
    {
        _skinLayers = new Dictionary<TreeItem, SkinLayer>();
        _layerTree.Clear();
        LoadTreeLayer(Editor.CurrentProject.RootLayer);
    }

    private void LoadTreeLayer(SkinLayer layer, TreeItem parent = null)
    {
        TreeItem layerItem = _layerTree.CreateItem(parent);
        layerItem.SetText(1, layer.Name);
        layerItem.SetIcon(2, layer.AsImageTexture());
        layerItem.SetSelectable(2, false);
        layerItem.SetCellMode(0, TreeItem.TreeCellMode.Check);
        layerItem.SetSelectable(0, false);
        layerItem.SetEditable(0, true);
        layerItem.SetChecked(0, true);
        
        _skinLayers.Add(layerItem, layer);

        layer.Updated += UpdateIcon;

        if (layer is CompositeSkinLayer compositeLayer)
        {
            foreach (SkinLayer sublayer in compositeLayer.Layers)
            {
                LoadTreeLayer(sublayer, layerItem);
            }
            
            compositeLayer.LayerAdded += OnLayerAdded;
            compositeLayer.LayerRemoved += OnLayerRemoved;
        }
    }

    private void UpdateIcon(SkinLayer layer)
    {
        TreeItem layerItem = GetTreeItem(layer);
        if (layerItem == null) return;
        
        layerItem.SetIcon(2, layer.AsImageTexture());
    }

    private void OnLayerRemoved(CompositeSkinLayer parent, SkinLayer layer)
    {
        TreeItem parentItem = GetTreeItem(parent);
        TreeItem layerItem = GetTreeItem(layer);
        parentItem.RemoveChild(layerItem);
    }
    
    private void OnLayerAdded(CompositeSkinLayer parent, SkinLayer layer)
    {
        TreeItem parentItem = GetTreeItem(parent);
        LoadTreeLayer(layer, parentItem);
    }

    private void OnItemEdited()
    {
        TreeItem editedItem = _layerTree.GetEdited();
        if (editedItem == null) return;
        _skinLayers[editedItem].Visible = editedItem.IsChecked(0);
    }
}