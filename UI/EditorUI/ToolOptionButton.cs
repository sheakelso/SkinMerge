using System;
using System.Collections.Generic;
using Godot;
using Godot.Collections;

namespace SkinMerge.UI.EditorUI;

public partial class ToolOptionButton : OptionButton
{
    public override void _Ready()
    {
        ItemSelected += OnSelected;
        AddTools();
    }
    
    private void OnSelected(long index)
    {
        Editor.Instance.SelectedTool = Editor.Instance.Tools[index];
    }

    private void AddTools()
    {
        for (int i = 0; i < Editor.Instance.Tools.Length; i++)
        {
            Tool tool =  Editor.Instance.Tools[i];
            AddIconItem(tool.Icon, tool.Name, i);
        }
    }
}