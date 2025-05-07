using System;
using System.Text;

namespace GaussZeidelOptimization
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;

            double[] u = { 0.0, 0.0 };
            int maxIterations = 100;
            double tolerance = 1e-6;

            GaussZeidelMethod(u, maxIterations, tolerance);

            Console.WriteLine($"Оптимальна точка: u1 = {u[0]:F2}, u2 = {u[1]:F2}");
            Console.WriteLine($"Значення функції: {ComputeFunction(u):F2}");
        }

        static void GaussZeidelMethod(double[] u, int maxIterations, double tolerance)
        {
            for (int iter = 0; iter < maxIterations; iter++)
            {
                double[] prevU = (double[])u.Clone();

                for (int i = 0; i < u.Length; i++)
                {
                    double uNew = LineSearch(u, i);
                    u[i] = Math.Max(uNew, -5);
                }

                if (IsConverged(u, prevU, tolerance))
                {
                    Console.WriteLine($"Збіжність досягнута за {iter} ітераціями.");
                    return;
                }

                Console.WriteLine($"Ітерація {iter + 1}: u1 = {u[0]:F2}, u2 = {u[1]:F2}, Функція: {ComputeFunction(u):F2}");
            }
            Console.WriteLine($"Максимальна кількість ітерацій досягнута.");
        }

        static bool IsConverged(double[] u, double[] prevU, double tolerance)
        {
            for (int i = 0; i < u.Length; i++)
            {
                if (Math.Abs(u[i] - prevU[i]) >= tolerance)
                    return false;
            }
            return true;
        }

        static double ComputeFunction(double[] u)
        {
            return u[0] + u[1] - Math.Pow(u[0] + u[1], 2) - 4 * Math.Pow(u[0], 2);
        }

        static double LineSearch(double[] u, int variableIndex)
        {
            double h = 0.1;
            double bestValue = ComputeFunction(u);
            double bestPoint = u[variableIndex];

            double[] candidate = (double[])u.Clone();
            candidate[variableIndex] = u[variableIndex] - h;
            double newValue = ComputeFunction(candidate);
            if (newValue < bestValue)
            {
                bestValue = newValue;
                bestPoint = candidate[variableIndex];
            }

            candidate = (double[])u.Clone();
            candidate[variableIndex] = u[variableIndex] + h;
            newValue = ComputeFunction(candidate);
            if (newValue < bestValue)
            {
                bestValue = newValue;
                bestPoint = candidate[variableIndex];
            }

            return bestPoint;
        }
    }
}