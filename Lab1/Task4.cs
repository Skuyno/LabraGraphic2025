using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

public class Task4
{
    public static void Run()
    {
        List<Vertex> vertices = ReadVertices("model_1.obj");

        using (var image = new Image<Rgba32>(1000, 1000, new Rgba32(255, 255, 255, 255)))
        {
            foreach (var vertex in vertices)
            {
                int x = (int)(400 * vertex.X + 500);
                int y = (int)(400 * vertex.Y + 500);

                if (x >= 0 && x < 1000 && y >= 0 && y < 1000)
                {
                    image[x, y] = new Rgba32(0,0,0,255);
                }
            }

            image.Save("vertices_image.png");
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
}
