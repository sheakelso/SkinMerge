using Godot;

namespace SkinMerge.UI.EditorUI;

public partial class PlayerModelContent : EditorContent
{
    [Export] private Button _headButton;
    [Export] private Button _bodyButton;
    [Export] private Button _rightArmButton;
    [Export] private Button _leftArmButton;
    [Export] private Button _rightLegButton;
    [Export] private Button _leftLegButton;

    [Export] private CheckButton _showOuterLayerButton;
    [Export] private CheckButton _showPixelGridButton;
    
    public override void _Ready()
    {
        _headButton.Toggled += on => OnButttonToggled("head", on);
        _bodyButton.Toggled += on => OnButttonToggled("body", on);
        _rightArmButton.Toggled += on => OnButttonToggled("rightArm", on);
        _leftArmButton.Toggled += on => OnButttonToggled("leftArm", on);
        _rightLegButton.Toggled += on => OnButttonToggled("rightLeg", on);
        _leftLegButton.Toggled += on => OnButttonToggled("leftLeg", on);

        _showOuterLayerButton.Toggled += OnToggleShowOuterLayer;
        _showPixelGridButton.Toggled += OnToggleShowPixelGrid;
    }

    private void OnButttonToggled(string part, bool toggled)
    {
        switch (part)
        {
            case "head":
                SkinPainter.Instance.PlayerModel.Head.Visible = toggled;
                break;
            case "body":
                SkinPainter.Instance.PlayerModel.Body.Visible = toggled;
                break;
            case "rightArm":
                SkinPainter.Instance.PlayerModel.RightArm.Visible = toggled;
                break;
            case "leftArm":
                SkinPainter.Instance.PlayerModel.LeftArm.Visible = toggled;
                break;
            case "rightLeg":
                SkinPainter.Instance.PlayerModel.RightLeg.Visible = toggled;
                break;
            case "leftLeg":
                SkinPainter.Instance.PlayerModel.LeftLeg.Visible = toggled;
                break;
        }
    }

    private void OnToggleShowPixelGrid(bool toggled)
    {
        SkinPainter.Instance.PlayerModel.SetPixelGridVisible(toggled);
    }
    
    private void OnToggleShowOuterLayer(bool toggled)
    {
        SkinPainter.Instance.PlayerModel.SetOuterLayerVisible(toggled);
    }
}