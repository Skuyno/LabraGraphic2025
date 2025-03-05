using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

public class Task11()
{
    public static void Run()
    {
        var vertices = ReadVertices("model_1.obj");
        var polygons = ReadPolygons("model_1.obj");

        using (var image = new Image<Rgba32>(1000, 1000))
        {
            RenderModel(image, vertices, polygons);
            image.Save("modelkaa.png");
        }
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
        Random rand = new Random();
        foreach (var polygon in polygons)
        {
            if (polygon.Length != 3) continue;

            var color = new Rgba32(
                (byte)rand.Next(256),
                (byte)rand.Next(256),
                (byte)rand.Next(256)
            );

            try
            {
                var v0 = vertices[polygon[0]];
                var v1 = vertices[polygon[1]];
                var v2 = vertices[polygon[2]];

                var p0 = ProjectVertex(v0);
                var p1 = ProjectVertex(v1);
                var p2 = ProjectVertex(v2);

                Vector3 normal = CalculateNormal(v0, v1, v2);

                DrawTriangle(image, color,
                    p0.Item1, p0.Item2,
                    p1.Item1, p1.Item2,
                    p2.Item1, p2.Item2
                );
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

    public static void DrawTriangle(Image<Rgba32> image, Rgba32 color,
        double x0, double y0,
        double x1, double y1,
        double x2, double y2)
    {
        // Определение ограничивающего прямоугольника
        double xmin = Math.Max(0, Math.Min(x0, Math.Min(x1, x2)));
        double xmax = Math.Min(image.Width - 1, Math.Max(x0, Math.Max(x1, x2)));
        double ymin = Math.Max(0, Math.Min(y0, Math.Min(y1, y2)));
        double ymax = Math.Min(image.Height - 1, Math.Max(y0, Math.Max(y1, y2)));

        // Блокируем изображение для прямого доступа к пикселям
        image.ProcessPixelRows(accessor =>
        {
            for (int y = (int)ymin; y <= ymax; y++)
            {
                Span<Rgba32> pixelRow = accessor.GetRowSpan(y);

                for (int x = (int)xmin; x <= xmax; x++)
                {
                    var (lambda0, lambda1, lambda2) =
                        CalculateBarycentric(x, y, x0, y0, x1, y1, x2, y2);

                    if (lambda0 >= 0 && lambda1 >= 0 && lambda2 >= 0)
                    {
                        pixelRow[x] = color;
                    }

                }
            }
        });
    }
    private static (int, int) ProjectVertex(Vertex vertex)
    {
        int x = (int)(6400 * vertex.X + 500);
        int y = (int)(6400 * vertex.Y + 500);
        return (x, y);
    }
}

public struct Vector3
{
    public double X;
    public double Y;
    public double Z;

    public Vector3(double x, double y, double z)
    {
        X = x;
        Y = y;
        Z = z;
    }

    public Vector3 Normalized()
    {
        double length = Math.Sqrt(X * X + Y * Y + Z * Z);
        return new Vector3(X / length, Y / length, Z / length);
    }

    public static double Dot(Vector3 a, Vector3 b)
    {
        return a.X * b.X + a.Y * b.Y + a.Z * b.Z;
    }

    public static Vector3 Cross(Vector3 a, Vector3 b)
    {
        return new Vector3(
            a.Y * b.Z - a.Z * b.Y,
            a.Z * b.X - a.X * b.Z,
            a.X * b.Y - a.Y * b.X);
    }
}
