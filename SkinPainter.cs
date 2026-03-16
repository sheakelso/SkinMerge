using Godot;
using System;

public partial class SkinPainter : Node
{
    [Export] public PlayerModel PlayerModel { get; set; }
    [Export] public Material PaintOverlayMaterial { get; set; }

    public override void _Ready()
    {
        
    }
}
