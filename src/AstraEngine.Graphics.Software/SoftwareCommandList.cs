using AstraEngine.Assets;
using AstraEngine.Math;
using AstraEngine.Scene;

namespace AstraEngine.Graphics.Software;

public sealed class SoftwareCommandList : ICommandList
{
    private bool _isRecording;
    private SoftwareSwapChain? _currentSwapChain;
    private readonly List<Light> _lights = [];
    private AmbientLight _ambientLight = new();
    private readonly SoftwareTextureSampler _sampler = new();
    private AssetManager? _assets;

    /// <summary>
    /// Sets the asset manager used for resolving texture paths at render time.
    /// </summary>
    public void SetAssetManager(AssetManager assets)
    {
        _assets = assets;
    }

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
            softwareSwapChain.FrameBuffer.ClearDepth();
        }
    }

    public void Draw(int vertexCount, int startVertexLocation = 0)
    {
    }

    public void DrawIndexed(int indexCount, int startIndexLocation = 0, int baseVertexLocation = 0)
    {
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
            return;

        var fb = _currentSwapChain.FrameBuffer;
        var world = meshInstance.WorldMatrix;
        var view = camera.ViewMatrix;
        var projection = camera.ProjectionMatrix(aspectRatio);
        var wvp = world * view * projection;

        var mesh = meshInstance.Mesh;
        var material = meshInstance.Material;

        // Resolve texture from path if available
        var texture = ResolveTexture(material.AlbedoTexturePath);
        var hasTexture = texture is not null;

        for (var i = 0; i < mesh.Indices.Length; i += 3)
        {
            var v0 = mesh.Vertices[mesh.Indices[i + 0]];
            var v1 = mesh.Vertices[mesh.Indices[i + 1]];
            var v2 = mesh.Vertices[mesh.Indices[i + 2]];

            var p0 = TransformVertex(v0, world, wvp);
            var p1 = TransformVertex(v1, world, wvp);
            var p2 = TransformVertex(v2, world, wvp);

            var faceNormal = Vector3.Normalize(Vector3.Cross(
                p1.WorldPosition - p0.WorldPosition,
                p2.WorldPosition - p0.WorldPosition));

            if (Vector3.Dot(faceNormal, camera.Position - p0.WorldPosition) <= 0f)
                continue;

            if (hasTexture)
            {
                RasterizeTriangleTextured(fb, p0, p1, p2, faceNormal, material, texture!);
            }
            else
            {
                var triangleColor = ShadeTriangle(material, faceNormal, p0, p1, p2);
                RasterizeTriangle(fb, p0, p1, p2, triangleColor);
            }
        }
    }

    private TextureAsset? ResolveTexture(string? texturePath)
    {
        if (string.IsNullOrWhiteSpace(texturePath) || _assets is null)
            return null;

        try
        {
            return _assets.Load<TextureAsset>(texturePath);
        }
        catch
        {
            return null;
        }
    }

    private Color4 ComputeLighting(Color4 surfaceColor, Vector3 normal, Vector3 worldPosition)
    {
        var r = surfaceColor.R * _ambientLight.Color.R * _ambientLight.Intensity;
        var g = surfaceColor.G * _ambientLight.Color.G * _ambientLight.Intensity;
        var b = surfaceColor.B * _ambientLight.Color.B * _ambientLight.Intensity;

        foreach (var light in _lights)
        {
            if (!light.Enabled)
                continue;

            switch (light)
            {
                case DirectionalLight directional:
                    {
                        var lightDir = Vector3.Normalize(directional.Direction * -1f);
                        var ndotl = MathF.Max(0f, Vector3.Dot(normal, lightDir));
                        r += surfaceColor.R * directional.Color.R * directional.Intensity * ndotl;
                        g += surfaceColor.G * directional.Color.G * directional.Intensity * ndotl;
                        b += surfaceColor.B * directional.Color.B * directional.Intensity * ndotl;
                        break;
                    }

                case PointLight point:
                    {
                        var toLight = point.Position - worldPosition;
                        var dist = MathF.Sqrt(Vector3.Dot(toLight, toLight));
                        var attenuation = 1f / (1f + point.Attenuation * (dist / MathF.Max(0.001f, point.Range)));
                        var lightDir = Vector3.Normalize(toLight);
                        var ndotl = MathF.Max(0f, Vector3.Dot(normal, lightDir));
                        r += surfaceColor.R * point.Color.R * point.Intensity * ndotl * attenuation;
                        g += surfaceColor.G * point.Color.G * point.Intensity * ndotl * attenuation;
                        b += surfaceColor.B * point.Color.B * point.Intensity * ndotl * attenuation;
                        break;
                    }

                case SpotLight spot:
                    {
                        var toLight = spot.Position - worldPosition;
                        var dist = MathF.Sqrt(Vector3.Dot(toLight, toLight));
                        var lightDir = Vector3.Normalize(toLight);
                        var spotDir = Vector3.Normalize(spot.Direction * -1f);
                        var angle = MathF.Acos(System.Math.Clamp(Vector3.Dot(lightDir, spotDir), -1f, 1f)) * (180f / MathF.PI);

                        if (angle <= spot.OuterConeAngle)
                        {
                            var spotFactor = angle <= spot.InnerConeAngle
                                ? 1f
                                : 1f - ((angle - spot.InnerConeAngle) / MathF.Max(0.001f, spot.OuterConeAngle - spot.InnerConeAngle));
                            var attenuation = 1f / (1f + (dist / MathF.Max(0.001f, spot.Range)));
                            var ndotl = MathF.Max(0f, Vector3.Dot(normal, lightDir));

                            r += surfaceColor.R * spot.Color.R * spot.Intensity * ndotl * attenuation * spotFactor;
                            g += surfaceColor.G * spot.Color.G * spot.Intensity * ndotl * attenuation * spotFactor;
                            b += surfaceColor.B * spot.Color.B * spot.Intensity * ndotl * attenuation * spotFactor;
                        }
                        break;
                    }
            }
        }

        return new Color4(
            MathF.Min(1f, r),
            MathF.Min(1f, g),
            MathF.Min(1f, b),
            surfaceColor.A);
    }

    private Color4 ShadeTriangle(Material material, Vector3 normal, ProjectedVertex v0, ProjectedVertex v1, ProjectedVertex v2)
    {
        var baseColor = new Color4(
            ((v0.Color.R + v1.Color.R + v2.Color.R) / 3f) * material.BaseColor.R,
            ((v0.Color.G + v1.Color.G + v2.Color.G) / 3f) * material.BaseColor.G,
            ((v0.Color.B + v1.Color.B + v2.Color.B) / 3f) * material.BaseColor.B,
            material.Opacity);

        var centroid = (v0.WorldPosition + v1.WorldPosition + v2.WorldPosition) * (1f / 3f);
        return ComputeLighting(baseColor, normal, centroid);
    }

    private void RasterizeTriangleTextured(
        SoftwareFrameBuffer fb,
        ProjectedVertex p0, ProjectedVertex p1, ProjectedVertex p2,
        Vector3 faceNormal, Material material, TextureAsset texture)
    {
        var a = ScreenSpace(p0.Position, fb.Width, fb.Height);
        var b = ScreenSpace(p1.Position, fb.Width, fb.Height);
        var c = ScreenSpace(p2.Position, fb.Width, fb.Height);

        var area = Edge(a, b, c);
        if (area >= 0f)
            return;

        var minX = (int)MathF.Max(0, MathF.Floor(MathF.Min(a.X, MathF.Min(b.X, c.X))));
        var minY = (int)MathF.Max(0, MathF.Floor(MathF.Min(a.Y, MathF.Min(b.Y, c.Y))));
        var maxX = (int)MathF.Min(fb.Width - 1, MathF.Ceiling(MathF.Max(a.X, MathF.Max(b.X, c.X))));
        var maxY = (int)MathF.Min(fb.Height - 1, MathF.Ceiling(MathF.Max(a.Y, MathF.Max(b.Y, c.Y))));

        for (var y = minY; y <= maxY; y++)
        {
            for (var x = minX; x <= maxX; x++)
            {
                var p = new Vector2(x + 0.5f, y + 0.5f);

                var w0 = Edge(b, c, p);
                var w1 = Edge(c, a, p);
                var w2 = Edge(a, b, p);

                if (w0 < 0f || w1 < 0f || w2 < 0f)
                    continue;

                w0 /= area;
                w1 /= area;
                w2 /= area;

                var depth = (p0.Depth * w0) + (p1.Depth * w1) + (p2.Depth * w2);
                var index = (y * fb.Width) + x;

                if (depth >= fb.Depth[index])
                    continue;

                var u = (p0.UV.X * w0) + (p1.UV.X * w1) + (p2.UV.X * w2);
                var v = (p0.UV.Y * w0) + (p1.UV.Y * w1) + (p2.UV.Y * w2);

                var texColor = _sampler.Sample(texture, u, v);
                var surfaceColor = new Color4(
                    texColor.R * material.BaseColor.R,
                    texColor.G * material.BaseColor.G,
                    texColor.B * material.BaseColor.B,
                    material.Opacity);

                var worldPos = new Vector3(
                    (p0.WorldPosition.X * w0) + (p1.WorldPosition.X * w1) + (p2.WorldPosition.X * w2),
                    (p0.WorldPosition.Y * w0) + (p1.WorldPosition.Y * w1) + (p2.WorldPosition.Y * w2),
                    (p0.WorldPosition.Z * w0) + (p1.WorldPosition.Z * w1) + (p2.WorldPosition.Z * w2));

                var litColor = ComputeLighting(surfaceColor, faceNormal, worldPos);

                fb.Depth[index] = depth;
                fb.Pixels[index] = ToBgra32(litColor);
            }
        }
    }

    private static void RasterizeTriangle(
        SoftwareFrameBuffer fb,
        ProjectedVertex p0, ProjectedVertex p1, ProjectedVertex p2,
        Color4 triangleColor)
    {
        var a = ScreenSpace(p0.Position, fb.Width, fb.Height);
        var b = ScreenSpace(p1.Position, fb.Width, fb.Height);
        var c = ScreenSpace(p2.Position, fb.Width, fb.Height);

        var area = Edge(a, b, c);
        if (area >= 0f)
            return;

        var minX = (int)MathF.Max(0, MathF.Floor(MathF.Min(a.X, MathF.Min(b.X, c.X))));
        var minY = (int)MathF.Max(0, MathF.Floor(MathF.Min(a.Y, MathF.Min(b.Y, c.Y))));
        var maxX = (int)MathF.Min(fb.Width - 1, MathF.Ceiling(MathF.Max(a.X, MathF.Max(b.X, c.X))));
        var maxY = (int)MathF.Min(fb.Height - 1, MathF.Ceiling(MathF.Max(a.Y, MathF.Max(b.Y, c.Y))));

        for (var y = minY; y <= maxY; y++)
        {
            for (var x = minX; x <= maxX; x++)
            {
                var p = new Vector2(x + 0.5f, y + 0.5f);

                var w0 = Edge(b, c, p);
                var w1 = Edge(c, a, p);
                var w2 = Edge(a, b, p);

                if (w0 < 0f || w1 < 0f || w2 < 0f)
                    continue;

                w0 /= area;
                w1 /= area;
                w2 /= area;

                var depth = (p0.Depth * w0) + (p1.Depth * w1) + (p2.Depth * w2);
                var index = (y * fb.Width) + x;

                if (depth >= fb.Depth[index])
                    continue;

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

    private static ProjectedVertex TransformVertex(Vertex v, Matrix4x4 world, Matrix4x4 wvp)
    {
        var localPos4 = new Vector4(v.Position.X, v.Position.Y, v.Position.Z, 1f);
        var worldPos4 = world.Transform(localPos4);
        var worldPos = new Vector3(worldPos4.X, worldPos4.Y, worldPos4.Z);

        var clip = wvp.Transform(localPos4);

        if (clip.W != 0f)
            clip = new Vector4(clip.X / clip.W, clip.Y / clip.W, clip.Z / clip.W, 1f);

        return new ProjectedVertex(
            new Vector2(clip.X, clip.Y),
            clip.Z,
            v.Color,
            worldPos,
            v.UV);
    }

    public void Dispose() { }

    private readonly record struct ProjectedVertex(
        Vector2 Position,
        float Depth,
        Color4 Color,
        Vector3 WorldPosition,
        Vector2 UV);
}