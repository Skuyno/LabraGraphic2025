using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

public class Task2
{
    public static void Run()
    {
        using (var image_dotted_line = new Image<Rgba32>(200, 200, new Rgba32(255, 255, 255, 255)))
        using (var image_dotted_line_v2 = new Image<Rgba32>(200, 200, new Rgba32(255, 255, 255, 255)))
        using (var image_x_loop_line = new Image<Rgba32>(200, 200, new Rgba32(255, 255, 255, 255)))
        using (var image_x_loop_line_hotfix_1 = new Image<Rgba32>(200, 200, new Rgba32(255, 255, 255, 255)))
        using (var image_x_loop_line_hotfix_2 = new Image<Rgba32>(200, 200, new Rgba32(255, 255, 255, 255)))
        using (var image_x_loop_line_v2 = new Image<Rgba32>(200, 200, new Rgba32(255, 255, 255, 255)))
        using (var image_x_loop_line_v2_no_y_calc = new Image<Rgba32>(200, 200, new Rgba32(255, 255, 255, 255)))
        using (var image_x_loop_line_v2_no_y_calc_for_some_unknown_reason = new Image<Rgba32>(200, 200, new Rgba32(255, 255, 255, 255)))
        using (var image_bresenham_line = new Image<Rgba32>(200, 200, new Rgba32(255, 255, 255, 255)))
        {
            for (int i = 0; i < 13; i++)
            {
                double angle = 2 * Math.PI * i / 13;
                int endX = (int)(100 + 95 * Math.Cos(angle));
                int endY = (int)(100 + 95 * Math.Sin(angle));

                dotted_line(image_dotted_line, 100, 100, endX, endY, 50, new Rgba32(0, 0, 0, 255));
                dotted_line_v2(image_dotted_line_v2, 100, 100, endX, endY, new Rgba32(0, 0, 0, 255));
                x_loop_line(image_x_loop_line, 100, 100, endX, endY, new Rgba32(0, 0, 0, 255));
                x_loop_line_hotfix_1(image_x_loop_line_hotfix_1, 100, 100, endX, endY, new Rgba32(0, 0, 0, 255));
                x_loop_line_hotfix_2(image_x_loop_line_hotfix_2, 100, 100, endX, endY, new Rgba32(0, 0, 0, 255));
                x_loop_line_v2(image_x_loop_line_v2, 100, 100, endX, endY, new Rgba32(0, 0, 0, 255));
                x_loop_line_v2_no_y_calc(image_x_loop_line_v2_no_y_calc, 100, 100, endX, endY, new Rgba32(0, 0, 0, 255));
                x_loop_line_v2_no_y_calc_for_some_unknown_reason(image_x_loop_line_v2_no_y_calc_for_some_unknown_reason, 100, 100, endX, endY, new Rgba32(0, 0, 0, 255));
                bresenham_line(image_bresenham_line, 100, 100, endX, endY, new Rgba32(0, 0, 0, 255));
            }
            image_dotted_line.Save("dotted_line.png");
            image_dotted_line.Save("dotted_line_v2.png");
            image_x_loop_line.Save("x_loop_line.png");
            image_x_loop_line_hotfix_1.Save("x_loop_line_hotfix_1.png");
            image_x_loop_line_hotfix_2.Save("x_loop_line_hotfix_2.png");
            image_x_loop_line_v2.Save("x_loop_line_v2.png");
            image_x_loop_line_v2_no_y_calc.Save("x_loop_line_v2_no_y_calc.png");
            image_x_loop_line_v2_no_y_calc_for_some_unknown_reason.Save("x_loop_line_v2_no_y_calc_for_some_unknown_reason.png");
            image_bresenham_line.Save("bresenham_line.png");
        }
    }

    static void dotted_line(Image<Rgba32> image, int x0, int y0, int x1, int y1, int count, Rgba32 color)
    {
        double step = 1.0 / count;

        for (double i = 0; i < 1; i += step)
        {
            int x = (int)Math.Round((1.0 - i) * x0 + i * x1);
            int y = (int)Math.Round((1.0 - i) * y0 + i * y1);

            image[x, y] = color;
        }
    }

    static void dotted_line_v2(Image<Rgba32> image, int x0, int y0, int x1, int y1, Rgba32 color)
    {
        int count = (int)Math.Sqrt(Math.Pow(x0 - x1, 2) + Math.Pow(y0 - y1, 2));
        double step = 1.0 / count;

        for (double i = 0; i < 1; i += step)
        {
            int x = (int)Math.Round((1.0 - i) * x0 + i * x1);
            int y = (int)Math.Round((1.0 - i) * y0 + i * y1);

            image[x, y] = color;
        }
    }

    static void x_loop_line(Image<Rgba32> image, int x0, int y0, int x1, int y1, Rgba32 color)
    {
        for (int x = x0; x <= x1; x++)
        {
            double t = (x - x0) / (double)(x1 - x0);
            int y = (int)Math.Round((1.0 - t) * y0 + t * y1);

            image[x, y] = color;
        }
    }

    static void x_loop_line_hotfix_1(Image<Rgba32> image, int x0, int y0, int x1, int y1, Rgba32 color)
    {
        if (x0 > x1)
        {
            int temp = x0;
            x0 = x1;
            x1 = temp;

            temp = y0;
            y0 = y1;
            y1 = temp;
        }
        for (int x = x0; x <= x1; x++)
        {
            double t = (x - x0) / (double)(x1 - x0);
            int y = (int)Math.Round((1.0 - t) * y0 + t * y1);

            image[x, y] = color;
        }
    }

    static void x_loop_line_hotfix_2(Image<Rgba32> image, int x0, int y0, int x1, int y1, Rgba32 color)
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

        for (int x = x0; x <= x1; x++)
        {
            double t = (x - x0) / (double)(x1 - x0);
            int y = (int)Math.Round((1.0 - t) * y0 + t * y1);

            if (xchange)
            {
                image[y, x] = color;
            }
            else
            {
                image[x, y] = color;
            }
        }
    }

    static void x_loop_line_v2(Image<Rgba32> image, int x0, int y0, int x1, int y1, Rgba32 color)
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

        for (int x = x0; x <= x1; x++)
        {
            double t = (x - x0) / (double)(x1 - x0);
            int y = (int)Math.Round((1.0 - t) * y0 + t * y1);

            if (xchange)
            {
                image[y, x] = color;
            }
            else
            {
                image[x, y] = color;
            }
        }
    }

    static void x_loop_line_v2_no_y_calc(Image<Rgba32> image, int x0, int y0, int x1, int y1, Rgba32 color)
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
        double dy = Math.Abs(y1 - y0) / (double)(x1 - x0);
        double derror = 0.0;
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
            if (derror > 0.5)
            {
                derror -= 1.0;
                y += y_update;
            }
        }
    }

    static void x_loop_line_v2_no_y_calc_for_some_unknown_reason(Image<Rgba32> image, int x0, int y0, int x1, int y1, Rgba32 color)
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
        double dy = 2 * (x1 - x0) * Math.Abs(y1 - y0) / (double)(x1 - x0);
        double derror = 0.0;
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
            if (derror > 2 * (x1 - x0) * 0.5)
            {
                derror -= 2 * (x1 - x0) * 1.0;
                y += y_update;
            }
        }
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
}