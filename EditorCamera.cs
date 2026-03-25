using System;
using Godot;

namespace SkinMerge;

public partial class EditorCamera : Node3D
{
    public static EditorCamera Instance { get; private set; }
    
    [Export] public Camera3D Camera { get; set; }
    [Export] public Node3D Target { get; set; }
    [Export] public Node3D CameraTarget { get; set; }
    
    
    [Export] public float FollowSpeed { get; set; } = 0.1f;
    [Export] public float MouseSensitivity { get; set; } = 1f;
    [Export] public float MinCameraDistance { get; set; } = 0.5f;
    [Export] public float MaxCameraDistance { get; set; } = 10f;
    [Export] public float CameraDistanceStep { get; set; } = 0.1f;
    [Export] public float PanSpeed { get; set; } = 5f;

    public bool DisableInput = false;

    public EditorCamera()
    {
        Instance = this;
    }

    public override void _Process(double delta)
    {
        float deltaFollow = (float)delta * FollowSpeed;
        
        Vector3 rotation = Rotation;
        rotation.X = Mathf.LerpAngle(rotation.X, Target.Rotation.X, deltaFollow);
        rotation.Y = Mathf.LerpAngle(rotation.Y, Target.Rotation.Y, deltaFollow);
        rotation.Z = Mathf.LerpAngle(rotation.Z, Target.Rotation.Z, deltaFollow);
        Rotation = rotation;
        
        Vector3 position = Position;
        position.X = Mathf.Lerp(position.X, Target.Position.X, deltaFollow);
        position.Y = Mathf.Lerp(position.Y, Target.Position.Y, deltaFollow);
        position.Z = Mathf.Lerp(position.Z, Target.Position.Z, deltaFollow);
        Position = position;
        
        Vector3 cameraPosition = Camera.Position;
        cameraPosition.X = Mathf.Lerp(cameraPosition.X, CameraTarget.Position.X, deltaFollow);
        cameraPosition.Y = Mathf.Lerp(cameraPosition.Y, CameraTarget.Position.Y, deltaFollow);
        cameraPosition.Z = Mathf.Lerp(cameraPosition.Z, CameraTarget.Position.Z, deltaFollow);
        Camera.Position = cameraPosition;
    }

    private Vector2 _lastPosition;
    private bool _ready;
    public override void _UnhandledInput(InputEvent @event)
    {
        if(DisableInput) return;
        if (@event is InputEventMouseButton eventMouseButton)
        {
            if (eventMouseButton.Pressed)
            {
                if (eventMouseButton.ButtonIndex == MouseButton.WheelUp)
                {
                    Vector3 translation = -CameraTarget.Position.Normalized() * CameraDistanceStep;
                    Vector3 newPos = CameraTarget.Position + translation;
                    if (newPos.Length() > MinCameraDistance && newPos.Length() < MaxCameraDistance)
                    {
                        CameraTarget.Position = newPos;
                    }
                }
                
                if (eventMouseButton.ButtonIndex == MouseButton.WheelDown)
                {
                    Vector3 translation = CameraTarget.Position.Normalized() * CameraDistanceStep;
                    Vector3 newPos = CameraTarget.Position + translation;
                    if (newPos.Length() > MinCameraDistance && newPos.Length() < MaxCameraDistance)
                    {
                        CameraTarget.Position = newPos;
                    }
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
                Target.Position += translation;
            }
            
            if (!Input.IsMouseButtonPressed(MouseButton.Left)) return;
            
            delta = eventMouseMotion.Relative;
            
            float toPositive = 90f - Target.RotationDegrees.X;

            float yDeg = Mathf.Clamp(Mathf.RadToDeg(-delta.Y * MouseSensitivity), -180f + toPositive, toPositive);
            
            Target.Rotate(Target.Transform.Basis.X.Normalized(), Mathf.DegToRad(yDeg));
            Target.RotateY(-delta.X * MouseSensitivity);
        }
    }
}