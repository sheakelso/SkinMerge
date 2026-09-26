using System;
using System.Collections.Generic;
using Godot;
using SkinMerge;
using Array = Godot.Collections.Array;

public partial class PaintableMesh : StaticBody3D
{
    [Export] public MeshInstance3D Mesh;
    [Export] public Part Part;
    
    private static bool _isPainting = false;
    private Vector2I _textureResolution = new(64, 64);
    private Vector2I _hoveredPixel =  new(-1, -1);

    public override void _Ready()
    {
        MouseEntered += OnEntered;
        MouseExited += OnExited;
    }
    
    private ModelFace _lastFace;
    private void UpdateFace(Vector3 eventPosition)
    {
        Vector2 texturePos =  GetTextureMousePosition(eventPosition);
        Vector2I pixelPos = new Vector2I((int)texturePos.X, (int)texturePos.Y);
        ModelFace modelFace = UVMappings.GetModelFaceFromPixel(pixelPos);

        if (!modelFace.Equals(_lastFace))
        {
            _lastFace = modelFace;
            SkinPainter.Instance.FaceChanged(modelFace);
            GD.Print("Part: " + modelFace.Part + ", Side: " + modelFace.Side);
        }
    }

    private ModelSide GetSide(Vector3 normal)
    {
        float xAbs = Mathf.Abs(normal.X);
        float yAbs = Mathf.Abs(normal.Y);
        float zAbs = Mathf.Abs(normal.Z);

        if (xAbs > yAbs && xAbs > zAbs)
        {
            if (normal.X > 0) return ModelSide.Front;
            return ModelSide.Back;
        }

        if (yAbs > xAbs && yAbs > zAbs)
        {
            if (normal.Y > 0) return ModelSide.Left;
            return ModelSide.Right;
        }

        if (zAbs > xAbs && zAbs > yAbs)
        {
            if (normal.Z > 0) return ModelSide.Top;
            return ModelSide.Bottom;
        }

        return ModelSide.None;
    }

    private void OnEntered()
    {
        SkinPainter.Instance.MouseEntered(Part);
    }

    private void OnExited()
    {
        SkinPainter.Instance.MouseExited(Part);
        _lastFace = ModelFace.None;
    }

    public override void _InputEvent(Camera3D camera, InputEvent @event, Vector3 eventPosition, Vector3 normal, int shapeIdx)
    {
        if (@event is InputEventMouseMotion)
        {
            UpdateFace(eventPosition);
            SkinPainter.Instance.MouseMotion(Part, GetTextureMousePosition(eventPosition));
        }

        if (@event is InputEventMouseButton mouseButton)
        {
            SkinPainter.Instance.MouseClicked(Part, GetTextureMousePosition(eventPosition), mouseButton.ButtonIndex, mouseButton.Pressed);
        }
    }

    public Vector2 GetTextureMousePosition(Vector3 eventPosition)
    {
        Vector2 uv = GetUVMousePosition(eventPosition);
        return uv * _textureResolution;
    }

    public Vector2 GetUVMousePosition(Vector3 eventPosition)
    {
        Vector3 localPosition = Mesh.ToLocal(eventPosition);

        Array arrays = Mesh.Mesh.SurfaceGetArrays(0);
        Vector3[] vertices = arrays[(int)Godot.Mesh.ArrayType.Vertex].AsVector3Array();
        Vector2[] uvs = arrays[(int)Godot.Mesh.ArrayType.TexUV].AsVector2Array();
            
        int triangleIndex = GetClosestTriangleIndex(vertices, localPosition);

        Vector3[] triangle = [vertices[triangleIndex], vertices[triangleIndex + 1], vertices[triangleIndex + 2]];
        Vector2[] triangleUvs = [uvs[triangleIndex], uvs[triangleIndex + 1], uvs[triangleIndex + 2]];
        
        return GetUVAtPoint(triangle, triangleUvs, localPosition);
    }

    private static int GetClosestTriangleIndex(Vector3[] vertices, Vector3 point)
    {
        float closestDistance = float.MaxValue;
        int closestTriangleIndex = 0;

        for (int i = 0; i < vertices.Length; i += 3)
        {
            Vector3 closestPoint = ClosestPointOnTriangle(point, vertices[i], vertices[i + 1], vertices[i + 2]);
            float dist = point.DistanceTo(closestPoint);
            if (dist <= closestDistance)
            {
                closestDistance = dist;
                closestTriangleIndex = i;
            }
        }

        return closestTriangleIndex;
    }
    
    private static Vector3 ClosestPointOnTriangle(Vector3 p, Vector3 a, Vector3 b, Vector3 c)
    {
        Vector3 ab = b - a;
        Vector3 ac = c - a;
        Vector3 ap = p - a;

        float d1 = ab.Dot(ap);
        float d2 = ac.Dot(ap);
        if (d1 <= 0f && d2 <= 0f) return a; //#1

        Vector3 bp = p - b;
        float d3 = ab.Dot(bp);
        float d4 = ac.Dot(bp);
        if (d3 >= 0f && d4 <= d3) return b; //#2

        Vector3 cp = p - c;
        float d5 = ab.Dot(cp);
        float d6 = ac.Dot(cp);
        if (d6 >= 0f && d5 <= d6) return c; //#3

        float vc = d1 * d4 - d3 * d2;
        if (vc <= 0f && d1 >= 0f && d3 <= 0f)
        {
            float v1 = d1 / (d1 - d3);
            return a + v1 * ab; //#4
        }
    
        float vb = d5 * d2 - d1 * d6;
        if (vb <= 0f && d2 >= 0f && d6 <= 0f)
        {
            float v2 = d2 / (d2 - d6);
            return a + v2 * ac; //#5
        }
    
        float va = d3 * d6 - d5 * d4;
        if (va <= 0f && (d4 - d3) >= 0f && (d5 - d6) >= 0f)
        {
            float v3 = (d4 - d3) / ((d4 - d3) + (d5 - d6));
            return b + v3 * (c - b); //#6
        }

        float denom = 1f / (va + vb + vc);
        float v = vb * denom;
        float w = vc * denom;
        return a + v * ab + w * ac; //#0
    }

    private static Vector2 GetUVAtPoint(Vector3[] triangle, Vector2[] uvs, Vector3 point)
    {
        Vector3 v0 = triangle[1] - triangle[0];
        Vector3 v1 = triangle[2] - triangle[0];
        Vector3 v2 = point - triangle[0];
        
        float d00 = v0.Dot(v0);
        float d01 = v0.Dot(v1);
        float d11 = v1.Dot(v1);
        float d20 = v2.Dot(v0);
        float d21 = v2.Dot(v1);

        float denom = d00 * d11 - d01 * d01;
        
        float v = (d11 * d20 - d01 * d21) / denom;
        float w = (d00 * d21 - d01 * d20) / denom;
        float u = 1f - v - w;
        return u * uvs[0] + v * uvs[1] + w * uvs[2];
    }

    private Vector3 GetNormalAtPoint(Vector3 eventPosition)
    {
        Vector3 localPosition = Mesh.ToLocal(eventPosition);
        
        Array arrays = Mesh.Mesh.SurfaceGetArrays(0);
        Vector3[] vertices = arrays[(int)Godot.Mesh.ArrayType.Vertex].AsVector3Array();
        Vector3[] normals = arrays[(int)Godot.Mesh.ArrayType.Normal].AsVector3Array();
            
        int triangleIndex = GetClosestTriangleIndex(vertices, localPosition);
        
        return normals[triangleIndex];
    }
}
