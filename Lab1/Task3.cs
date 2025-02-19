
public class Task3
{
    public static void Run()
    {
        List<Vertex> vertices = ReadVertices("model_1.obj");
        Console.WriteLine("Первый элемент в списке вершин: " + vertices[0].X + " " + vertices[0].Y + " " + vertices[0].Z);
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

public struct Vertex
{
    public double X;
    public double Y;
    public double Z;

    public Vertex(double x, double y, double z)
    {
        X = x;
        Y = y;
        Z = z;
    }
}
