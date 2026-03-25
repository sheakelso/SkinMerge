using System.IO;
using Godot;

namespace SkinMerge.UI.EditorUI;

public partial class LayerPopupMenu : PopupMenu
{
    public enum Item
    {
        Export
    }
    public override void _Ready()
    {
        Clear();
        AddItem("Export Layer...", (int)Item.Export);
        IdPressed += ItemClicked;
    }

    private void ItemClicked(long id)
    {
        Item item = (Item)(int)id;

        switch (item)
        {
            case Item.Export:
                HandleExport();
                break;
        }
    }
    
    private Image _imageToExport;
    private void HandleExport()
    {
        SkinLayer layer = Editor.Instance.FocusedLayer;
        if(layer == null) return;
        _imageToExport = layer.AsImage();
        DisplayServer.FileDialogShow("Export layer...", "", "export.png", false, DisplayServer.FileDialogMode.SaveFile, ["*.png;.PNG File;image/png"], new Callable(this, MethodName.ExportCallback));
    }

    private void ExportCallback(bool status, string[] selectedPaths, int selectedFilterIndex)
    {
        if (status)
        {
            _imageToExport.SavePng(selectedPaths[0]);
        }
    }
}