using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Nelder_Mid_Parallels_3D_4D_5D
{
    public class NelderMeadOptimizer
    {
        public double Alpha { get; set; } = 1.0;    // Коэффициент отражения
        public double Gamma { get; set; } = 2.0;    // Коэффициент растяжения
        public double Beta { get; set; } = 0.5;     // Коэффициент сжатия
        public double Delta { get; set; } = 0.5;    // Коэффициент редукции  (shrink)
        public int MaxIterations { get; set; } = 1000;
        public double Tolerance { get; set; } = 1e-6;
        public int StretchMethod { get; set; } = 1; // 1-базовый, 2-пассивный, 3-золотое сечение
        public int PassiveSearchSteps { get; set; } = 1000; // Количество шагов для пассивного перебора
        public int IterationsCount { get; private set; }    // Добавляем свойство для отслеживания количества итераций

        public Vector Optimize(Func<Vector, double> func, Vector[] initialSimplex, double[,] compact)
        {
            IterationsCount = 0;
            int n = initialSimplex[0].Dimension;
            if (initialSimplex.Length != n + 1)
                throw new ArgumentException("Initial simplex must have N+1 points");

            Vector[] simplex = initialSimplex.ToArray();
            double[] values = simplex.Select(func).ToArray();

            for (int iter = 0; iter < MaxIterations; iter++)
            {
                IterationsCount++;
                // 1. Сортировка вершин по значениям функции
                Array.Sort(values, simplex, Comparer<double>.Default);


                // 2. Проверка на сходимость (разброс значений)
                double spread = values.Max() - values.Min();
                if (spread < Tolerance)
                {
                    //Console.WriteLine($"Количество итераций = {iter}");
                    break;
                }


                // 3. Вычисление центра тяжести (без худшей точки)
                Vector centroid = CalculateCentroid(simplex, n);


                // 4. Отражение худшей точки
                Vector xWorst = simplex[n];
                Vector xReflected = centroid + Alpha * (centroid - xWorst);
                double fReflected = func(xReflected);

                if (fReflected < values[n - 1] && fReflected >= values[0])
                {
                    // Принимаем отражённую точку
                    simplex[n] = xReflected;
                    values[n] = fReflected;
                    continue;
                }


                // 5. Растяжение (выбор метода)
                if (fReflected < values[0])
                {
                    Vector reflectionDirection = xReflected - centroid;
                    Vector perturbedDirection = PerturbDirection(reflectionDirection, 45);
                    double alphaMax = FindMaxAlphaInDirection(centroid, perturbedDirection, compact);

                    Vector xExpanded;
                    switch (StretchMethod)
                    {
                        case 1: // Базовый
                            xExpanded = centroid + Gamma * (xReflected - centroid);
                            break;
                        case 2: // Пассивный перебор
                            xExpanded = PassiveSearchExpansion(func, centroid, perturbedDirection, alphaMax);
                            break;
                        case 3: // Золотое сечение
                            xExpanded = GoldenSectionExpansion(func, centroid, perturbedDirection, alphaMax);
                            break;
                        default:
                            throw new ArgumentException("Invalid stretch method");
                    }

                    simplex[n] = func(xExpanded) < fReflected ? xExpanded : xReflected;
                    values[n] = func(simplex[n]);
                    continue;
                }

                // 6. Сжатие (если отражение не помогло)
                Vector xContracted = centroid + Beta * (xWorst - centroid);
                double fContracted = func(xContracted);

                if (fContracted < values[n])
                {
                    simplex[n] = xContracted;
                    values[n] = fContracted;
                    continue;
                }

                // 7. Редукция (если сжатие не помогло)
                for (int i = 1; i < n + 1; i++)
                {
                    simplex[i] = simplex[0] + Delta * (simplex[i] - simplex[0]);
                    values[i] = func(simplex[i]);
                }
            }

            return simplex[0];      // Возвращаем лучшую точку
        }


        public static double FindMaxAlphaInDirection(Vector centroid, Vector direction, double[,] compact)
        {
            double alphaMax = double.PositiveInfinity;
            int n = centroid.Dimension;

            for (int i = 0; i < n; i++)
            {
                double dirComponent = direction.Components[i];
                if (Math.Abs(dirComponent) < 1e-10)
                    continue;

                double lower = compact[i, 0];
                double upper = compact[i, 1];
                double centroidComponent = centroid.Components[i];

                double alphaToLower = (lower - centroidComponent) / dirComponent;
                double alphaToUpper = (upper - centroidComponent) / dirComponent;

                if (dirComponent > 0)
                {
                    if (alphaToUpper < alphaMax)
                        alphaMax = alphaToUpper;
                }
                else
                {
                    if (alphaToLower < alphaMax)
                        alphaMax = alphaToLower;
                }
            }

            //Console.WriteLine($"\nМаксимальная альфа = {alphaMax}");
            return alphaMax;
        }

        private static Vector PerturbDirection(Vector direction, double maxAngleDegrees)
        {
            Random rand = new Random();
            double maxAngleRad = maxAngleDegrees * Math.PI / 180;
            double[] perturbedComponents = new double[direction.Dimension];

            for (int i = 0; i < direction.Dimension; i++)
            {
                double angle = (rand.NextDouble() * 2 - 1) * maxAngleRad;
                perturbedComponents[i] = direction.Components[i] * Math.Cos(angle);
            }

            return new Vector(perturbedComponents);
        }



        // Метод пассивного перебора
        private Vector PassiveSearchExpansion(Func<Vector, double> func, Vector centroid, Vector direction, double alphaMax)
        {
            double bestAlpha = 0;
            double bestValue = func(centroid);
            int steps = PassiveSearchSteps;

            for (int i = 1; i <= steps; i++)
            {
                double alpha = alphaMax * i / steps;
                Vector x = centroid + alpha * direction;
                double currentValue = func(x);

                if (currentValue < bestValue)
                {
                    bestValue = currentValue;
                    bestAlpha = alpha;
                }
            }

            return centroid + bestAlpha * direction;
        }


        // Метод золотого сечения (без изменений)
        private Vector GoldenSectionExpansion(Func<Vector, double> func, Vector centroid, Vector direction, double alphaMax)
        {
            const double phi = 1.618033988749895;
            double a = 0;
            double b = alphaMax;
            double alpha1 = b - (b - a) / phi;
            double alpha2 = a + (b - a) / phi;

            Vector x1 = centroid + alpha1 * direction;
            Vector x2 = centroid + alpha2 * direction;
            double f1 = func(x1);
            double f2 = func(x2);

            while (Math.Abs(b - a) > 1e-6)
            {
                if (f1 < f2)
                {
                    b = alpha2;
                    alpha2 = alpha1;
                    f2 = f1;
                    alpha1 = b - (b - a) / phi;
                    x1 = centroid + alpha1 * direction;
                    f1 = func(x1);
                }
                else
                {
                    a = alpha1;
                    alpha1 = alpha2;
                    f1 = f2;
                    alpha2 = a + (b - a) / phi;
                    x2 = centroid + alpha2 * direction;
                    f2 = func(x2);
                }
            }

            return f1 < f2 ? x1 : x2;
        }


        private Vector CalculateCentroid(Vector[] simplex, int n)
        {
            Vector centroid = new Vector(new double[n]);
            for (int i = 0; i < n; i++)
                centroid = centroid + simplex[i];
            return (1.0 / n) * centroid;
        }

    }
}