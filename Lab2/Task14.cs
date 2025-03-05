using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

public class Task14()
{
    private static double[,] zBuffer;
    public static void Run()
    {
        var vertices = ReadVertices("model_1.obj");
        var polygons = ReadPolygons("model_1.obj");

        using (var image = new Image<Rgba32>(1000, 1000))
        {
            InitializeZBuffer(image.Width, image.Height);
            RenderModel(image, vertices, polygons);
            image.Save("modelkaaaa.png");
        }
    }

    private static void InitializeZBuffer(int width, int height)
    {
        zBuffer = new double[width, height];
        for (int x = 0; x < width; x++)
            for (int y = 0; y < height; y++)
                zBuffer[x, y] = double.MaxValue;
    }

    private static List<Vertex> ReadVertices(string filePath)
    {
        List<Vertex> vertices = new List<Vertex>();
        foreach (var line in File.ReadLines(filePath))
        {
            if (line.StartsWith("v"))
            {
                string[] parts = line.Split(' ');
                if (parts.Length == 4)
                {
                    string partX = parts[1].Trim().Replace('.', ',');
                    string partY = parts[2].Trim().Replace('.', ',');
                    string partZ = parts[3].Trim().Replace('.', ',');

                    if (double.TryParse(partX, out double x) &&
                        double.TryParse(partY, out double y) &&
                        double.TryParse(partZ, out double z))
                    {
                        vertices.Add(new Vertex(x, y, z));
                    }
                }
            }
        }
        return vertices;
    }

    private static List<int[]> ReadPolygons(string filePath)
    {
        List<int[]> polygons = new List<int[]>();
        foreach (var line in File.ReadLines(filePath))
        {
            if (line.StartsWith('f'))
            {
                string[] parts = line.Split(' ');

                if (parts.Length >= 4)
                {
                    string[] v1 = parts[1].Split('/');
                    string[] v2 = parts[2].Split('/');
                    string[] v3 = parts[3].Split('/');

                    if (int.TryParse(v1[0], out int vertex1) &&
                        int.TryParse(v2[0], out int vertex2) &&
                        int.TryParse(v3[0], out int vertex3))
                    {
                        polygons.Add([vertex1 - 1, vertex2 - 1, vertex3 - 1]);
                    }
                }
            }
        }
        return polygons;
    }

    private static void RenderModel(Image<Rgba32> image, List<Vertex> vertices, List<int[]> polygons)
    {
        var lightDirection = new Vector3(0, 0, 1);

        foreach (var polygon in polygons)
        {
            try
            {
                var v0 = vertices[polygon[0]];
                var v1 = vertices[polygon[1]];
                var v2 = vertices[polygon[2]];

                var p0 = ProjectVertex(v0);
                var p1 = ProjectVertex(v1);
                var p2 = ProjectVertex(v2);

                Vector3 normal = CalculateNormal(v0, v1, v2);

                double cosTheta = Vector3.Dot(normal.Normalized(), lightDirection);
                if (cosTheta >= 0) continue;

                byte intensity = (byte)(-cosTheta * 255);
                var color = new Rgba32(intensity, intensity, intensity);

                DrawTriangleWithZBuffer(image, color, p0, p1, p2);
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"Error drawing triangle: {ex.Message}");
            }
            catch (IndexOutOfRangeException)
            {
                Console.WriteLine("Invalid vertex index in polygon");
            }
        }
    }
    static (double lambda0, double lambda1, double lambda2) CalculateBarycentric(
        int x, int y,
        double x0, double y0,
        double x1, double y1,
        double x2, double y2)
    {
        double denominator = (x0 - x2) * (y1 - y2) - (x1 - x2) * (y0 - y2);

        double lambda0 = ((x - x2) * (y1 - y2) - (x1 - x2) * (y - y2)) / denominator;
        double lambda1 = ((x0 - x2) * (y - y2) - (x - x2) * (y0 - y2)) / denominator;
        double lambda2 = 1.0 - lambda0 - lambda1;

        return (lambda0, lambda1, lambda2);
    }

    private static Vector3 CalculateNormal(Vertex v0, Vertex v1, Vertex v2)
    {
        Vector3 edge1 = new Vector3(v1.X - v0.X, v1.Y - v0.Y, v1.Z - v0.Z);
        Vector3 edge2 = new Vector3(v2.X - v0.X, v2.Y - v0.Y, v2.Z - v0.Z);
        return Vector3.Cross(edge1, edge2);
    }

    private static void DrawTriangleWithZBuffer(
        Image<Rgba32> image,
        Rgba32 color,
        Vertex v0,
        Vertex v1,
        Vertex v2)
    {
        int minX = (int)Math.Max(0, Math.Floor(Math.Min(v0.X, Math.Min(v1.X, v2.X))));
        int maxX = (int)Math.Min(image.Width - 1, Math.Ceiling(Math.Max(v0.X, Math.Max(v1.X, v2.X))));
        int minY = (int)Math.Max(0, Math.Floor(Math.Min(v0.Y, Math.Min(v1.Y, v2.Y))));
        int maxY = (int)Math.Min(image.Height - 1, Math.Ceiling(Math.Max(v0.Y, Math.Max(v1.Y, v2.Y))));

        image.ProcessPixelRows(accessor =>
        {
            for (int y = minY; y <= maxY; y++)
            {
                var row = accessor.GetRowSpan(y);
                for (int x = minX; x <= maxX; x++)
                {
                    var (l0, l1, l2) = CalculateBarycentric(x, y,
                        v0.X, v0.Y,
                        v1.X, v1.Y,
                        v2.X, v2.Y);

                    if (l0 >= 0 && l1 >= 0 && l2 >= 0)
                    {
                        // Задание 14: Расчет z-координаты
                        double z = l0 * v0.Z + l1 * v1.Z + l2 * v2.Z;

                        if (z < zBuffer[x, y])
                        {
                            zBuffer[x, y] = z;
                            row[x] = color;
                        }
                    }
                }
            }
        });
    }
    private static Vertex ProjectVertex(Vertex vertex)
    {
        vertex.X = (int)(6400 * vertex.X + 500);
        vertex.Y = (int)(6400 * vertex.Y + 500);
        return vertex;
    }
}
