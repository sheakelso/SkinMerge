using Godot;

namespace SkinMerge.UI.EditorUI;

public partial class ViewportEditorContent : EditorContent
{
    [Export] private CheckButton _3dButton;
    [Export] private CheckButton _uvButton;
    [Export] private OptionButton _splitDirectionButton;
    [Export] private Button _swapButton;

    [Export] private Control _3dControl;
    [Export] private Control _uvControl;
    [Export] private SplitContainer _splitContainer;

    public override void _Ready()
    {
        _3dButton.Toggled += On3dButtonToggled;
        _uvButton.Toggled += OnUvButtonToggled;
        _splitDirectionButton.ItemSelected += OnSplitDirectionSelected;
        _swapButton.Pressed += OnSwapButtonPressed;
    }

    private void On3dButtonToggled(bool toggled)
    {
        if (toggled)
        {
            _3dControl.Visible = true;
            return;
        }

        if (!_uvButton.IsPressed())
        {
            _uvButton.SetPressed(true);
        }
        
        _3dControl.Visible = false;
    }
    
    private void OnUvButtonToggled(bool toggled)
    {
        if (toggled)
        {
            _uvControl.Visible = true;
            return;
        }

        if (!_3dButton.IsPressed())
        {
            _3dButton.SetPressed(true);
        }
        
        _uvControl.Visible = false;
    }

    private void OnSplitDirectionSelected(long index)
    {
        _splitContainer.Vertical = index == 1;
    }

    private void OnSwapButtonPressed()
    {
        _splitContainer.MoveChild(_splitContainer.GetChild(0), 1);
    }
}