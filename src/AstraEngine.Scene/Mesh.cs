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
                new Vertex(new Vector3(0f, 0.5f, 0f), new Vector3(0f, 0f, -1f), new Color4(1f, 1f, 1f, 1f)),
                new Vertex(new Vector3(-0.5f, -0.5f, 0f), new Vector3(0f, 0f, -1f), new Color4(1f, 1f, 1f, 1f)),
                new Vertex(new Vector3(0.5f, -0.5f, 0f), new Vector3(0f, 0f, -1f), new Color4(1f, 1f, 1f, 1f))
            ],
            [0, 1, 2]);
    }

    public static Mesh CreateCube()
    {
        var vertices = new[]
        {
            new Vertex(new Vector3(-0.5f, -0.5f, -0.5f), new Vector3( 0f,  0f, -1f), new Color4(1f, 0f, 0f, 1f)),
            new Vertex(new Vector3( 0.5f, -0.5f, -0.5f), new Vector3( 0f,  0f, -1f), new Color4(0f, 1f, 0f, 1f)),
            new Vertex(new Vector3( 0.5f,  0.5f, -0.5f), new Vector3( 0f,  0f, -1f), new Color4(0f, 0f, 1f, 1f)),
            new Vertex(new Vector3(-0.5f,  0.5f, -0.5f), new Vector3( 0f,  0f, -1f), new Color4(1f, 1f, 0f, 1f)),
            new Vertex(new Vector3(-0.5f, -0.5f,  0.5f), new Vector3( 0f,  0f,  1f), new Color4(1f, 0f, 1f, 1f)),
            new Vertex(new Vector3( 0.5f, -0.5f,  0.5f), new Vector3( 0f,  0f,  1f), new Color4(0f, 1f, 1f, 1f)),
            new Vertex(new Vector3( 0.5f,  0.5f,  0.5f), new Vector3( 0f,  0f,  1f), new Color4(1f, 1f, 1f, 1f)),
            new Vertex(new Vector3(-0.5f,  0.5f,  0.5f), new Vector3( 0f,  0f,  1f), new Color4(0.2f, 0.2f, 0.2f, 1f))
        };

        var indices = new[]
        {
            0,1,2, 2,3,0,
            4,5,6, 6,7,4,
            0,4,7, 7,3,0,
            1,5,6, 6,2,1,
            3,2,6, 6,7,3,
            0,1,5, 5,4,0
        };

        return new Mesh(vertices, indices);
    }
}

public readonly record struct Vertex(Vector3 Position, Vector3 Normal, Color4 Color);