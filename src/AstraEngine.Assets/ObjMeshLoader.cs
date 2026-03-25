using AstraEngine.Math;
using AstraEngine.Scene;

namespace AstraEngine.Assets;

internal static class ObjMeshLoader
{
    public static Mesh Load(string path)
    {
        var positions = new List<Vector3>();
        var normals = new List<Vector3>();
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

                case "f":
                    if (parts.Length < 4)
                    {
                        continue;
                    }

                    for (var i = 1; i <= 3; i++)
                    {
                        var face = parts[i];
                        var faceParts = face.Split('/');

                        var positionIndex = int.Parse(faceParts[0]) - 1;
                        var normalIndex = faceParts.Length >= 3 && !string.IsNullOrWhiteSpace(faceParts[2])
                            ? int.Parse(faceParts[2]) - 1
                            : -1;

                        var position = positions[positionIndex];
                        var normal = normalIndex >= 0 && normalIndex < normals.Count
                            ? normals[normalIndex]
                            : Vector3.UnitZ;

                        var vertex = new Vertex(position, normal, new Color4(1f, 1f, 1f, 1f));
                        vertices.Add(vertex);
                        indices.Add(vertices.Count - 1);
                    }
                    break;
            }
        }

        return new Mesh(vertices.ToArray(), indices.ToArray());
    }
}