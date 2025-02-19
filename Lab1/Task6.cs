using System;
using System.Collections.Generic;
using System.IO;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

public class Task6
{
    public static void Run()
    {
        var vertices = ReadVertices("model_1.obj");
        var polygons = ReadPolygons("model_1.obj");

        using (var image = new Image<Rgba32>(1000, 1000, new Rgba32(255, 255, 255, 255)))
        {
            foreach (var polygon in polygons)
            {
                var v1 = vertices[polygon[0]];
                var v2 = vertices[polygon[1]];
                var v3 = vertices[polygon[2]];

                var p1 = ProjectVertex(v1);
                var p2 = ProjectVertex(v2);
                var p3 = ProjectVertex(v3);

                bresenham_line(image, p1.Item1, p1.Item2, p2.Item1, p2.Item2, new Rgba32(0,0,0,255));
                bresenham_line(image, p2.Item1, p2.Item2, p3.Item1, p3.Item2, new Rgba32(0,0,0,255));
                bresenham_line(image, p3.Item1, p3.Item2, p1.Item1, p1.Item2, new Rgba32(0,0,0,255));
            }

            image.Save("edges_image.png");
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

    static void bresenham_line(Image<Rgba32> image, int x0, int y0, int x1, int y1, Rgba32 color)
    {
        bool xchange = false;

        if (Math.Abs(x0 - x1) < Math.Abs(y0 - y1))
        {
            int temp = x0;
            x0 = y0;
            y0 = temp;

            temp = x1;
            x1 = y1;
            y1 = temp;

            xchange = true;
        }

        if (x0 > x1)
        {
            int temp = x0;
            x0 = x1;
            x1 = temp;

            temp = y0;
            y0 = y1;
            y1 = temp;
        }

        int y = y0;
        double dy = 2 * Math.Abs(y1 - y0);
        double derror = 0;
        int y_update = (y1 > y0) ? 1 : -1;

        for (int x = x0; x <= x1; x++)
        {

            if (xchange)
            {
                image[y, x] = color;
            }
            else
            {
                image[x, y] = color;
            }

            derror += dy;
            if (derror > (x1 - x0))
            {
                derror -= 2 * (x1 - x0);
                y += y_update;
            }
        }
    }

    private static (int, int) ProjectVertex(Vertex vertex)
    {
        int x = (int)(6400 * vertex.X + 500);
        int y = (int)(6400 * vertex.Y + 500);
        return (x, y);
    }
}