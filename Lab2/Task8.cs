using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using System;
using System.Numerics;

public class Task8
{
    public static void Run()
    {
        // Пока ничего
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
}