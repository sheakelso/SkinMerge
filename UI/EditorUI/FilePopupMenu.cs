using System.IO;
using Godot;

namespace SkinMerge.UI.EditorUI;

public partial class FilePopupMenu : PopupMenu
{
    public enum Item
    {
        Save,
        Import,
        Export
    }
    public override void _Ready()
    {
        Clear();
        AddItem("Save");
        AddItem("Import...", (int)Item.Import);
        AddItem("Export...", (int)Item.Export);
        IdPressed += ItemClicked;
    }

    private void ItemClicked(long id)
    {
        Item item = (Item)(int)id;

        switch (item)
        {
            case Item.Save:
                Editor.CurrentProject.Save();
                break;
            case Item.Import:
                HandleImport();
                break;
            case Item.Export:
                HandleExport();
                break;
        }
    }

    private void HandleImport()
    {
        DisplayServer.FileDialogShow("Select a file to import", "", "", false, DisplayServer.FileDialogMode.OpenFile, ["*.png,*.jpg,*.jpeg;Image Files;image/png,image/jpeg"], new Callable(this, MethodName.ImportCallback));
    }

    private void ImportCallback(bool status, string[] selectedPaths, int selectedFilterIndex)
    {
        if (status)
        {
            Image image = Image.LoadFromFile(selectedPaths[0]);
            if(image.GetHeight() != 64 || image.GetWidth() != 64) return;
            Editor.CurrentProject.RootLayer.AddLayer(new ImageSkinLayer(image, new DirectoryInfo(selectedPaths[0]).Name));
        }
    }
    
    private Image _imageToExport;
    private void HandleExport()
    {
        SkinLayer layer = Editor.CurrentProject.RootLayer;
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