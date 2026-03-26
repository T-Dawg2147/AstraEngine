using AstraEngine.Math;
using AstraEngine.Scene;

namespace AstraEngine.Assets;

internal static class ObjMeshLoader
{
    public static Mesh Load(string path)
    {
        var positions = new List<Vector3>();
        var normals = new List<Vector3>();
        var texCoords = new List<Vector2>();
        var vertices = new List<Vertex>();
        var indices = new List<int>();

        foreach (var rawLine in File.ReadLines(path))
        {
            var line = rawLine.Trim();
            if (string.IsNullOrWhiteSpace(line) || line.StartsWith("#"))
            {
                continue;
            }

            var parts = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length == 0)
            {
                continue;
            }

            switch (parts[0])
            {
                case "v":
                    positions.Add(new Vector3(
                        float.Parse(parts[1], System.Globalization.CultureInfo.InvariantCulture),
                        float.Parse(parts[2], System.Globalization.CultureInfo.InvariantCulture),
                        float.Parse(parts[3], System.Globalization.CultureInfo.InvariantCulture)));
                    break;

                case "vn":
                    normals.Add(new Vector3(
                        float.Parse(parts[1], System.Globalization.CultureInfo.InvariantCulture),
                        float.Parse(parts[2], System.Globalization.CultureInfo.InvariantCulture),
                        float.Parse(parts[3], System.Globalization.CultureInfo.InvariantCulture)));
                    break;

                case "vt":
                    texCoords.Add(new Vector2(
                        float.Parse(parts[1], System.Globalization.CultureInfo.InvariantCulture),
                        parts.Length >= 3
                            ? float.Parse(parts[2], System.Globalization.CultureInfo.InvariantCulture)
                            : 0f));
                    break;

                case "f":
                    if (parts.Length < 4)
                    {
                        continue;
                    }

                    // Triangulate faces with more than 3 vertices (fan triangulation)
                    for (var i = 2; i < parts.Length - 1; i++)
                    {
                        AddFaceVertex(parts[1], positions, texCoords, normals, vertices, indices);
                        AddFaceVertex(parts[i], positions, texCoords, normals, vertices, indices);
                        AddFaceVertex(parts[i + 1], positions, texCoords, normals, vertices, indices);
                    }
                    break;
            }
        }

        return new Mesh(vertices.ToArray(), indices.ToArray());
    }

    private static void AddFaceVertex(
        string face,
        List<Vector3> positions,
        List<Vector2> texCoords,
        List<Vector3> normals,
        List<Vertex> vertices,
        List<int> indices)
    {
        var faceParts = face.Split('/');

        var positionIndex = int.Parse(faceParts[0]) - 1;

        var hasTexCoord = faceParts.Length >= 2 && !string.IsNullOrWhiteSpace(faceParts[1]);
        var texCoordIndex = hasTexCoord ? int.Parse(faceParts[1]) - 1 : -1;

        var hasNormal = faceParts.Length >= 3 && !string.IsNullOrWhiteSpace(faceParts[2]);
        var normalIndex = hasNormal ? int.Parse(faceParts[2]) - 1 : -1;

        var position = positions[positionIndex];

        var normal = normalIndex >= 0 && normalIndex < normals.Count
            ? normals[normalIndex]
            : Vector3.UnitZ;

        var uv = texCoordIndex >= 0 && texCoordIndex < texCoords.Count
            ? texCoords[texCoordIndex]
            : Vector2.Zero;

        var vertex = new Vertex(position, normal, new Color4(1f, 1f, 1f, 1f), uv);
        vertices.Add(vertex);
        indices.Add(vertices.Count - 1);
    }
}