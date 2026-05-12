using Godot;

namespace SkinMerge.UI.EditorUI;

public partial class ToolSettings : HBoxContainer
{
    public override void _Ready()
    {
        Editor.Instance.ToolSelected += OnToolSelected;
    }

    public void OnToolSelected(Tool tool)
    {
        foreach (Node child in GetChildren())
        {
            child.QueueFree();
        }
        foreach (ToolField field in tool.GetFields())
        {
            if(field is ToolFieldInt fieldInt) CreateIntField(fieldInt);
            if(field is ToolFieldFloat fieldFloat) CreateFloatField(fieldFloat);
        }
    }

    public void CreateIntField(ToolFieldInt field)
    {
        if(field.Slider) CreateIntSlider(field);
        else CreateIntSpinBox(field);
    }
    
    public void CreateFloatField(ToolFieldFloat field)
    {
        if(field.Slider) CreateFloatSlider(field);
        else CreateFloatSpinBox(field);
    }

    private void CreateIntSlider(ToolFieldInt field)
    {
        HBoxContainer fieldBox = CreateFieldBox(field.Name);
        
        HSlider slider = new HSlider();
        slider.Value = field.Value;
        slider.MinValue = field.MinValue;
        slider.MaxValue = field.MaxValue;
        slider.Step = field.Step;
        slider.ValueChanged += (newValue) => OnFieldValueChanged(field, (int)newValue);
        
        fieldBox.AddChild(slider);
        AddChild(fieldBox);
    }
    
    private void CreateFloatSlider(ToolFieldFloat field)
    {
        HBoxContainer fieldBox = CreateFieldBox(field.Name);
        
        HSlider slider = new HSlider();
        slider.Value = field.Value;
        slider.MinValue = field.MinValue;
        slider.MaxValue = field.MaxValue;
        slider.Step = field.Step;
        slider.CustomMinimumSize = new Vector2(100, 0);
        slider.SizeFlagsVertical = SizeFlags.ShrinkCenter;
        slider.ValueChanged += (newValue) => OnFieldValueChanged(field, (float)newValue);
        
        fieldBox.AddChild(slider);
        AddChild(fieldBox);
    }
    
    private void CreateIntSpinBox(ToolFieldInt field)
    {
        HBoxContainer fieldBox = CreateFieldBox(field.Name);
        
        SpinBox spinBox = new SpinBox();
        spinBox.Value = field.Value;
        spinBox.MinValue = field.MinValue;
        spinBox.MaxValue = field.MaxValue;
        spinBox.Step = field.Step;
        spinBox.ValueChanged += (newValue) => OnFieldValueChanged(field, (int)newValue);
        
        fieldBox.AddChild(spinBox);
        AddChild(fieldBox);
    }
    
    private void CreateFloatSpinBox(ToolFieldFloat field)
    {
        HBoxContainer fieldBox = CreateFieldBox(field.Name);
        
        SpinBox spinBox = new SpinBox();
        spinBox.Value = field.Value;
        spinBox.MinValue = field.MinValue;
        spinBox.MaxValue = field.MaxValue;
        spinBox.Step = field.Step;
        spinBox.ValueChanged += (newValue) => OnFieldValueChanged(field, (float)newValue);
        
        fieldBox.AddChild(spinBox);
        AddChild(fieldBox);
    }

    private HBoxContainer CreateFieldBox(string name)
    {
        HBoxContainer fieldBox = new HBoxContainer();
        fieldBox.SetThemeTypeVariation("FieldBox");
        
        Label label = new Label();
        label.Text = name;
        label.SetThemeTypeVariation("FieldLabel");
        
        fieldBox.AddChild(label);
        
        return fieldBox;
    }

    private void OnFieldValueChanged<T>(ToolField<T> field, T value)
    {
        field.Value = value;
    }
}