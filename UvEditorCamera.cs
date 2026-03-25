using Godot;

namespace SkinMerge;

public partial class UvEditorCamera : Camera3D
{
    [Export] private float _zoomStep = 0.1f;
    [Export] private float _panSpeed = 0.1f;
    [Export] private float _followSpeed = 0.5f;
    [Export] private float _minSize = 0.1f;
    [Export] private float _maxSize = 50.0f;
    
    private Vector3 _targetPosition;
    private float _targetSize;

    public override void _Ready()
    {
        _targetPosition = Position;
        _targetSize = Size;
    }

    public override void _Process(double delta)
    {
        Position = Position.Lerp(_targetPosition, _followSpeed * (float)delta);
        Size = Mathf.Lerp(Size, _targetSize, _followSpeed * (float)delta);
    }

    private Vector2 _lastPosition;
    private bool _ready;
    public override void _UnhandledInput(InputEvent @event)
    {
        if(EditorCamera.Instance.DisableInput) return;
        if (@event is InputEventMouseButton eventMouseButton)
        {
            if (eventMouseButton.Pressed)
            {
                if (eventMouseButton.ButtonIndex == MouseButton.WheelUp)
                {
                    _targetSize = Mathf.Clamp(_targetSize - _zoomStep, _minSize, _maxSize);
                }
                
                if (eventMouseButton.ButtonIndex == MouseButton.WheelDown)
                {
                    _targetSize = Mathf.Clamp(_targetSize + _zoomStep, _minSize, _maxSize);
                }
            }
        }
        
        if (@event is InputEventMouseMotion eventMouseMotion)
        {
            Vector2 newPosition = GetViewport().GetMousePosition() / new Vector2(GetViewport().GetTexture().GetWidth(), GetViewport().GetTexture().GetWidth());
            if (!_ready)
            {
                _ready = true;
                _lastPosition = newPosition;
                return;
            }
            Vector2 delta = newPosition - _lastPosition;
            _lastPosition = newPosition;
            if (Input.IsMouseButtonPressed(MouseButton.Right))
            {
                Vector3 translation = Vector3.Zero;
                translation += Basis.X * -delta.X;
                translation += Basis.Y * delta.Y;
                _targetPosition += translation * _panSpeed * _targetSize;
            }
        }
    }
}