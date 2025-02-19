
public class Task5
{
    public static void Run()
    {
        var polygons = ReadPolygons("model_1.obj");
        Console.WriteLine("Первый пилигон в списке полигонов: " + polygons[0][0] + " " + polygons[0][1] + " " + polygons[0][2]);
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
}