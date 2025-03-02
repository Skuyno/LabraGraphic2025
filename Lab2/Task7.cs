
public class Task7
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
}