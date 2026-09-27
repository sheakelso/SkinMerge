using Godot;

namespace SkinMerge;

[GlobalClass]
public partial class PencilTool : Tool
{
    public ToolFieldInt BrushSize { get; } = new("Brush Size", 0, 1, 100);
    public override ToolField[] GetFields() => [];

    private Vector2I _hoveredPixel;
    private Vector2I _lastPixel;
    private ModelFace _lastFace = ModelFace.None;
    
    public override void TextureMouseMotion(Part part, Vector2 position)
    {
        Vector2I pixel = new Vector2I((int)position.X, (int)position.Y);
        if (pixel != _hoveredPixel)
        {
            _hoveredPixel = pixel;
            SkinPainter.Instance.ModelMaterial.SetShaderParameter("hovered_pixel", pixel);
        }

        if (_painting)
        {
            ImageSkinLayer layer = Editor.Instance.PaintingLayer;
            Color color = Editor.CurrentProject.GetCurrentColor();
            
            layer.SetPixel(pixel.X, pixel.Y, color);
            //return;

            if (_lastPixel == -Vector2I.One)
            {
                _lastPixel = pixel;
                return;
            }
            
            Vector2I delta = pixel - _lastPixel;
            Vector2I deltaAbs = delta.Abs();

            if (deltaAbs.X > deltaAbs.Y)
            {
                float m = (float)delta.Y / delta.X;
                for (int x = _lastPixel.X; x <= pixel.X; x++)
                {
                    float y = m * (x - _lastPixel.X) + _lastPixel.Y;
                    if(delta.Y > 0 && y <= pixel.Y) layer.SetPixel(x, (int)y, color);
                    else if(delta.Y < 0 && y >= pixel.Y) layer.SetPixel(x, (int)y, color);
                }
            }
            else
            {
                float m = (float)delta.X / delta.Y;
                for (int y = _lastPixel.Y; y <= pixel.Y; y++)
                {
                    float x = m * (y - _lastPixel.Y) + _lastPixel.X;
                    if(delta.X > 0 && x <= pixel.X) layer.SetPixel((int)x, y, color);
                    else if(delta.X < 0 && x >= pixel.X) layer.SetPixel((int)x, y, color);
                }
            }
        }
        
        _lastPixel = pixel;
    }

    private bool _painting;
    public override void TextureMouseClicked(Part part, Vector2 position, MouseButton button, bool pressed)
    {
        if (button == MouseButton.Left && pressed)
        {
            _painting = true;
            EditorCamera.Instance.DisableInput = true;
        }
    }

    public override void TextureMouseEntered(Part part)
    {
        Godot.Input.SetDefaultCursorShape(Godot.Input.CursorShape.Cross);
    }

    public override void TextureMouseExited(Part part)
    {
        Godot.Input.SetDefaultCursorShape();
    }

    public override void Input(InputEvent @event)
    {
        if (@event is InputEventMouseButton button)
        {
            if (button.ButtonIndex == MouseButton.Left && !button.Pressed)
            {
                _painting = false;
                EditorCamera.Instance.DisableInput = false;
            }
        }
    }

    public override void FaceChanged(ModelFace newFace)
    {
        _lastPixel = -Vector2I.One;
    }
}