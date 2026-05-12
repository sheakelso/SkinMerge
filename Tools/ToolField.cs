using Godot;

namespace SkinMerge;

public class ToolField
{
    public string Name { get; protected set; }
    protected object Value;
}

public class ToolField<T> : ToolField
{
    public new T Value
    {
        get => (T)base.Value;
        set => base.Value = value;
    }
}

public class ToolFieldFloat : ToolField<float>
{
    public float MaxValue { get; private set; }
    public float MinValue { get; private set; }
    public float Step { get; private set; }
    public bool Slider { get; private set; }
    
    public ToolFieldFloat(string name = "Tool Field", int value = 0, int minValue = int.MinValue, int maxValue = int.MaxValue, float step = 1, bool slider = false)
    {
        Name = name;
        Value = value;
        MinValue = minValue;
        MaxValue = maxValue;
        Step = step;
        Slider = slider;
    }
}

public class ToolFieldInt : ToolField<int>
{
    public int MaxValue { get; private set; }
    public int MinValue { get; private set; }
    public int Step { get; private set; }
    public bool Slider { get; private set; }

    public ToolFieldInt(string name = "Tool Field", int value = 0, int minValue = int.MinValue, int maxValue = int.MaxValue, int step = 1, bool slider = false)
    {
        Name = name;
        Value = value;
        MinValue = minValue;
        MaxValue = maxValue;
        Step = step;
        Slider = slider;
    }
}