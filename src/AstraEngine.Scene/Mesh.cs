using AstraEngine.Math;

namespace AstraEngine.Scene;

public sealed class Mesh
{
    public Mesh(Vertex[] vertices, int[] indices)
    {
        Vertices = vertices;
        Indices = indices;
    }

    public Vertex[] Vertices { get; }
    public int[] Indices { get; }

    public static Mesh CreateTriangle()
    {
        return new Mesh(
            [
                new Vertex(new Vector3(0f, 0.5f, 0f),    new Vector3(0f, 0f, -1f), new Color4(1f, 1f, 1f, 1f), new Vector2(0.5f, 0f)),
                new Vertex(new Vector3(-0.5f, -0.5f, 0f), new Vector3(0f, 0f, -1f), new Color4(1f, 1f, 1f, 1f), new Vector2(0f, 1f)),
                new Vertex(new Vector3(0.5f, -0.5f, 0f),  new Vector3(0f, 0f, -1f), new Color4(1f, 1f, 1f, 1f), new Vector2(1f, 1f))
            ],
            [0, 1, 2]);
    }

    public static Mesh CreateCube()
    {
        var w = Color4.White;

        var vertices = new[]
        {
            // Front face (z = -0.5, normal = 0,0,-1)
            new Vertex(new Vector3(-0.5f, -0.5f, -0.5f), new Vector3(0f, 0f, -1f), w, new Vector2(0f, 1f)),
            new Vertex(new Vector3( 0.5f, -0.5f, -0.5f), new Vector3(0f, 0f, -1f), w, new Vector2(1f, 1f)),
            new Vertex(new Vector3( 0.5f,  0.5f, -0.5f), new Vector3(0f, 0f, -1f), w, new Vector2(1f, 0f)),
            new Vertex(new Vector3(-0.5f,  0.5f, -0.5f), new Vector3(0f, 0f, -1f), w, new Vector2(0f, 0f)),

            // Back face (z = 0.5, normal = 0,0,1)
            new Vertex(new Vector3( 0.5f, -0.5f,  0.5f), new Vector3(0f, 0f, 1f), w, new Vector2(0f, 1f)),
            new Vertex(new Vector3(-0.5f, -0.5f,  0.5f), new Vector3(0f, 0f, 1f), w, new Vector2(1f, 1f)),
            new Vertex(new Vector3(-0.5f,  0.5f,  0.5f), new Vector3(0f, 0f, 1f), w, new Vector2(1f, 0f)),
            new Vertex(new Vector3( 0.5f,  0.5f,  0.5f), new Vector3(0f, 0f, 1f), w, new Vector2(0f, 0f)),

            // Left face (x = -0.5, normal = -1,0,0)
            new Vertex(new Vector3(-0.5f, -0.5f,  0.5f), new Vector3(-1f, 0f, 0f), w, new Vector2(0f, 1f)),
            new Vertex(new Vector3(-0.5f, -0.5f, -0.5f), new Vector3(-1f, 0f, 0f), w, new Vector2(1f, 1f)),
            new Vertex(new Vector3(-0.5f,  0.5f, -0.5f), new Vector3(-1f, 0f, 0f), w, new Vector2(1f, 0f)),
            new Vertex(new Vector3(-0.5f,  0.5f,  0.5f), new Vector3(-1f, 0f, 0f), w, new Vector2(0f, 0f)),

            // Right face (x = 0.5, normal = 1,0,0)
            new Vertex(new Vector3(0.5f, -0.5f, -0.5f), new Vector3(1f, 0f, 0f), w, new Vector2(0f, 1f)),
            new Vertex(new Vector3(0.5f, -0.5f,  0.5f), new Vector3(1f, 0f, 0f), w, new Vector2(1f, 1f)),
            new Vertex(new Vector3(0.5f,  0.5f,  0.5f), new Vector3(1f, 0f, 0f), w, new Vector2(1f, 0f)),
            new Vertex(new Vector3(0.5f,  0.5f, -0.5f), new Vector3(1f, 0f, 0f), w, new Vector2(0f, 0f)),

            // Top face (y = 0.5, normal = 0,1,0)
            new Vertex(new Vector3(-0.5f, 0.5f, -0.5f), new Vector3(0f, 1f, 0f), w, new Vector2(0f, 1f)),
            new Vertex(new Vector3( 0.5f, 0.5f, -0.5f), new Vector3(0f, 1f, 0f), w, new Vector2(1f, 1f)),
            new Vertex(new Vector3( 0.5f, 0.5f,  0.5f), new Vector3(0f, 1f, 0f), w, new Vector2(1f, 0f)),
            new Vertex(new Vector3(-0.5f, 0.5f,  0.5f), new Vector3(0f, 1f, 0f), w, new Vector2(0f, 0f)),

            // Bottom face (y = -0.5, normal = 0,-1,0)
            new Vertex(new Vector3(-0.5f, -0.5f,  0.5f), new Vector3(0f, -1f, 0f), w, new Vector2(0f, 1f)),
            new Vertex(new Vector3( 0.5f, -0.5f,  0.5f), new Vector3(0f, -1f, 0f), w, new Vector2(1f, 1f)),
            new Vertex(new Vector3( 0.5f, -0.5f, -0.5f), new Vector3(0f, -1f, 0f), w, new Vector2(1f, 0f)),
            new Vertex(new Vector3(-0.5f, -0.5f, -0.5f), new Vector3(0f, -1f, 0f), w, new Vector2(0f, 0f)),
        };

        var indices = new[]
        {
             0, 1, 2,  2, 3, 0,   // Front
             4, 5, 6,  6, 7, 4,   // Back
             8, 9,10, 10,11, 8,   // Left
            12,13,14, 14,15,12,   // Right
            16,17,18, 18,19,16,   // Top
            20,21,22, 22,23,20,   // Bottom
        };

        return new Mesh(vertices, indices);
    }
}

public readonly record struct Vertex(Vector3 Position, Vector3 Normal, Color4 Color, Vector2 UV);