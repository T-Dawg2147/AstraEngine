using AstraEngine.Math;
using AstraEngine.Scene;

namespace AstraEngine.Graphics.Software;

public sealed class SoftwareCommandList : ICommandList
{
    private bool _isRecording;
    private SoftwareSwapChain? _currentSwapChain;
    private readonly List<Light> _lights = [];
    private AmbientLight _ambientLight = new();

    public void Begin()
    {
        _isRecording = true;
    }

    public void End()
    {
        _isRecording = false;
        _currentSwapChain = null;
    }

    public void ClearColor(ISwapChain swapChain, Color4 color)
    {
        if (swapChain is SoftwareSwapChain softwareSwapChain)
        {
            _currentSwapChain = softwareSwapChain;
            softwareSwapChain.FrameBuffer.Clear(color);
        }
    }

    public void Draw(int vertexCount, int startVertexLocation = 0)
    {
        // Use DrawMesh from the sandbox.
    }

    public void DrawIndexed(int indexCount, int startIndexLocation = 0, int baseVertexLocation = 0)
    {
        // Use DrawMesh from the sandbox.
    }

    public void SetLights(IEnumerable<Light> lights)
    {
        _lights.Clear();
        _lights.AddRange(lights);
    }

    public void SetAmbientLight(AmbientLight light)
    {
        _ambientLight = light;
    }

    public void DrawMesh(MeshInstance meshInstance, Camera camera, float aspectRatio)
    {
        if (!_isRecording || _currentSwapChain is null)
        {
            return;
        }

        var fb = _currentSwapChain.FrameBuffer;
        var world = meshInstance.WorldMatrix;
        var view = camera.ViewMatrix;
        var projection = camera.ProjectionMatrix(aspectRatio);
        var wvp = world * view * projection;

        var mesh = meshInstance.Mesh;

        for (var i = 0; i < mesh.Indices.Length; i += 3)
        {
            var p0 = TransformVertex(mesh.Vertices[mesh.Indices[i + 0]], world, view, projection, wvp);
            var p1 = TransformVertex(mesh.Vertices[mesh.Indices[i + 1]], world, view, projection, wvp);
            var p2 = TransformVertex(mesh.Vertices[mesh.Indices[i + 2]], world, view, projection, wvp);

            var faceNormal = Vector3.Normalize(Vector3.Cross(p1.WorldPosition - p0.WorldPosition, p2.WorldPosition - p0.WorldPosition));
            if (Vector3.Dot(faceNormal, camera.Position - p0.WorldPosition) <= 0f)
            {
                continue;
            }

            var triangleColor = ShadeTriangle(meshInstance.Material, faceNormal, p0, p1, p2);
            RasterizeTriangle(fb, p0, p1, p2, triangleColor);
        }
    }

    private Color4 ShadeTriangle(Material material, Vector3 normal, ProjectedVertex v0, ProjectedVertex v1, ProjectedVertex v2)
    {
        var baseColor = new Color4(
            ((v0.Color.R + v1.Color.R + v2.Color.R) / 3f) * material.BaseColor.R,
            ((v0.Color.G + v1.Color.G + v2.Color.G) / 3f) * material.BaseColor.G,
            ((v0.Color.B + v1.Color.B + v2.Color.B) / 3f) * material.BaseColor.B,
            1f);

        var r = baseColor.R * _ambientLight.Color.R * _ambientLight.Intensity;
        var g = baseColor.G * _ambientLight.Color.G * _ambientLight.Intensity;
        var b = baseColor.B * _ambientLight.Color.B * _ambientLight.Intensity;

        foreach (var light in _lights)
        {
            if (!light.Enabled)
            {
                continue;
            }

            switch (light)
            {
                case DirectionalLight directional:
                    {
                        var lightDir = Vector3.Normalize(directional.Direction * -1f);
                        var ndotl = System.MathF.Max(0f, Vector3.Dot(normal, lightDir));
                        r += baseColor.R * directional.Color.R * directional.Intensity * ndotl;
                        g += baseColor.G * directional.Color.G * directional.Intensity * ndotl;
                        b += baseColor.B * directional.Color.B * directional.Intensity * ndotl;
                        break;
                    }

                case PointLight point:
                    {
                        var toLight = point.Position - v0.WorldPosition;
                        var dist = System.MathF.Sqrt(Vector3.Dot(toLight, toLight));
                        var attenuation = 1f / (1f + point.Attenuation * (dist / System.MathF.Max(0.001f, point.Range)));
                        var lightDir = Vector3.Normalize(toLight);
                        var ndotl = System.MathF.Max(0f, Vector3.Dot(normal, lightDir));
                        r += baseColor.R * point.Color.R * point.Intensity * ndotl * attenuation;
                        g += baseColor.G * point.Color.G * point.Intensity * ndotl * attenuation;
                        b += baseColor.B * point.Color.B * point.Intensity * ndotl * attenuation;
                        break;
                    }

                case SpotLight spot:
                    {
                        var toLight = spot.Position - v0.WorldPosition;
                        var dist = System.MathF.Sqrt(Vector3.Dot(toLight, toLight));
                        var lightDir = Vector3.Normalize(toLight);
                        var spotDir = Vector3.Normalize(spot.Direction * -1f);
                        var angle = System.MathF.Acos(System.MathF.Max(-1f, System.MathF.Min(1f, Vector3.Dot(lightDir, spotDir)))) * (180f / System.MathF.PI);

                        if (angle <= spot.OuterConeAngle)
                        {
                            var spotFactor = angle <= spot.InnerConeAngle ? 1f : 1f - ((angle - spot.InnerConeAngle) / System.MathF.Max(0.001f, spot.OuterConeAngle - spot.InnerConeAngle));
                            var attenuation = 1f / (1f + (dist / System.MathF.Max(0.001f, spot.Range)));
                            var ndotl = System.MathF.Max(0f, Vector3.Dot(normal, lightDir));

                            r += baseColor.R * spot.Color.R * spot.Intensity * ndotl * attenuation * spotFactor;
                            g += baseColor.G * spot.Color.G * spot.Intensity * ndotl * attenuation * spotFactor;
                            b += baseColor.B * spot.Color.B * spot.Intensity * ndotl * attenuation * spotFactor;
                        }

                        break;
                    }
            }
        }

        return new Color4(
            System.MathF.Min(1f, r),
            System.MathF.Min(1f, g),
            System.MathF.Min(1f, b),
            material.Opacity);
    }

    private static void RasterizeTriangle(
        SoftwareFrameBuffer fb,
        ProjectedVertex p0,
        ProjectedVertex p1,
        ProjectedVertex p2,
        Color4 triangleColor)
    {
        var a = ScreenSpace(p0.Position, fb.Width, fb.Height);
        var b = ScreenSpace(p1.Position, fb.Width, fb.Height);
        var c = ScreenSpace(p2.Position, fb.Width, fb.Height);

        var area = Edge(a, b, c);
        if (area >= 0f)
        {
            return;
        }

        var minX = (int)System.MathF.Max(0, System.MathF.Floor(System.MathF.Min(a.X, System.MathF.Min(b.X, c.X))));
        var minY = (int)System.MathF.Max(0, System.MathF.Floor(System.MathF.Min(a.Y, System.MathF.Min(b.Y, c.Y))));
        var maxX = (int)System.MathF.Min(fb.Width - 1, System.MathF.Ceiling(System.MathF.Max(a.X, System.MathF.Max(b.X, c.X))));
        var maxY = (int)System.MathF.Min(fb.Height - 1, System.MathF.Ceiling(System.MathF.Max(a.Y, System.MathF.Max(b.Y, c.Y))));

        for (var y = minY; y <= maxY; y++)
        {
            for (var x = minX; x <= maxX; x++)
            {
                var p = new Vector2(x + 0.5f, y + 0.5f);

                var w0 = Edge(b, c, p);
                var w1 = Edge(c, a, p);
                var w2 = Edge(a, b, p);

                if (w0 < 0f || w1 < 0f || w2 < 0f)
                {
                    continue;
                }

                w0 /= area;
                w1 /= area;
                w2 /= area;

                var depth = (p0.Depth * w0) + (p1.Depth * w1) + (p2.Depth * w2);
                var index = (y * fb.Width) + x;

                if (depth >= fb.Depth[index])
                {
                    continue;
                }

                fb.Depth[index] = depth;
                fb.Pixels[index] = ToBgra32(triangleColor);
            }
        }
    }

    private static Vector2 ScreenSpace(Vector2 ndc, int width, int height)
    {
        var x = (ndc.X * 0.5f + 0.5f) * width;
        var y = (1f - (ndc.Y * 0.5f + 0.5f)) * height;
        return new Vector2(x, y);
    }

    private static float Edge(Vector2 a, Vector2 b, Vector2 c)
        => (c.X - a.X) * (b.Y - a.Y) - (c.Y - a.Y) * (b.X - a.X);

    private static int ToBgra32(Color4 c)
    {
        static byte Clamp(float v) => (byte)System.Math.Clamp((int)(v * 255f), 0, 255);

        var r = Clamp(c.R);
        var g = Clamp(c.G);
        var b = Clamp(c.B);
        var a = Clamp(c.A);

        return (a << 24) | (r << 16) | (g << 8) | b;
    }

    private ProjectedVertex TransformVertex(Vertex v, Matrix4x4 wvp)
    {
        var worldPos4 = new Vector4(v.Position.X, v.Position.Y, v.Position.Z, 1f);
        var clip = wvp.Transform(worldPos4);

        if (clip.W != 0f)
        {
            clip = new Vector4(clip.X / clip.W, clip.Y / clip.W, clip.Z / clip.W, 1f);
        }

        return new ProjectedVertex(
            new Vector2(clip.X, clip.Y),
            clip.Z,
            v.Color,
            v.Position);
    }

    private readonly record struct ProjectedVertex(Vector2 Position, float Depth, Color4 Color, Vector3 WorldPosition);
}