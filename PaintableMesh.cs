using Godot;
using Godot.Collections;

public partial class PaintableMesh : StaticBody3D
{
    [Export] public MeshInstance3D Mesh;
    [Export] public ShaderMaterial PaintOverlayMaterial;
    public override void _InputEvent(Camera3D camera, InputEvent @event, Vector3 eventPosition, Vector3 normal, int shapeIdx)
    {
        if (@event is InputEventMouseMotion eventMouseMotion)
        {
            Vector3 localPosition = Mesh.ToLocal(eventPosition);
            Vector3 localNormal = Mesh.ToLocal(normal);


            Array arrays = Mesh.Mesh.SurfaceGetArrays(0);
            Vector3[] vertices = arrays[(int)Godot.Mesh.ArrayType.Vertex].AsVector3Array();
            Vector2[] uvs = arrays[(int)Godot.Mesh.ArrayType.TexUV].AsVector2Array();
            Vector3[] normals = arrays[(int)Godot.Mesh.ArrayType.Normal].AsVector3Array();
            
            GD.Print(GetClosestPoint(vertices, localPosition));
            
            PaintOverlayMaterial.SetShaderParameter("mouse_position", eventPosition);
        }
    }

    public Vector3 GetClosestPoint(Vector3[] points, Vector3 position)
    {
        Vector3 closestPoint = points[0];
        float closestDistance = float.MaxValue;
        for (int i = 1; i < points.Length; i++)
        {
            float distance = points[i].DistanceTo(position);
            if (distance <= closestDistance)
            {
                closestPoint = points[i];
                closestDistance = distance;
            }
        }
        return closestPoint;
    }
}
