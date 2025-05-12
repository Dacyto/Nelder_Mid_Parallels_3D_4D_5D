using System;
using System.Linq;
using System.Threading.Tasks;

namespace Nelder_Mid_Parallels_3D_4D_5D
{
    public static class Program
    {
        // Функции 3D с понятными именами переменных
        public static Func<Vector, double>[] Functions3D = new Func<Vector, double>[]
        {
            v => { double x = v.Components[0], y = v.Components[1], z = v.Components[2];
                return 28*x*x + 28*y*y + 13*z*z - 30*x*y - 28*x*z + 11*x - 14*y - 6*z + 3; },    // Q1(X)
            v => { double x = v.Components[0], y = v.Components[1], z = v.Components[2];
                return 19*x*x - 8*x*y - 16*x*z + 39*y*y + 12*z*z + 19*x + 27*y + 1*z - 15; },   // Q3(X)
            v => { double x = v.Components[0], y = v.Components[1], z = v.Components[2];
                return 23*x*x - 18*x*y - 30*x*z + 15*y*y + 32*z*z - 4*x + 38*y + 2*z - 40; }    // Q4(X)
        };

        // Функции 4D с понятными именами переменных
        public static Func<Vector, double>[] Functions4D = new Func<Vector, double>[]
        {
            v => { double x = v.Components[0], y = v.Components[1], z = v.Components[2], w = v.Components[3];
                return 7*x*x + 36*y*y + 25*z*z + 38*w*w + 2*x*y - 22*x*z + 68*y*w + 7*x + 35*y - 6*z + 27*w - 26; },   // Q11(X)
            v => { double x = v.Components[0], y = v.Components[1], z = v.Components[2], w = v.Components[3];
                return 23*x*x + 32*x*y + 4*x*z + 45*y*y + 22*y*w + 1*z*z + 35*w*w + 13*x - 15*y - 13*z + 29*w - 24; }, // Q13(X)
            v => { double x = v.Components[0], y = v.Components[1], z = v.Components[2], w = v.Components[3];
                return 14*x*x - 10*x*y + 6*x*z + 5*y*y + 16*y*w + 49*z*z + 28*w*w + 16*x - 14*y + 40*z + 22*w + 32; }  // Q14(X)
        };

        // Функции 5D с понятными именами переменных
        public static Func<Vector, double>[] Functions5D = new Func<Vector, double>[]
        {
            v => { double x = v.Components[0], y = v.Components[1], z = v.Components[2], w = v.Components[3], t = v.Components[4];
                return 23*x*x + 9*y*y + 17*z*z + 3*w*w + 18*t*t + 16*x*y + 22*x*z + 2*y*w - 4*y*t - 22*z*t - 4*w*t - 21*x + y - 7*z - 21*w - 25*t + 24; },  // Q21(X)
            v => { double x = v.Components[0], y = v.Components[1], z = v.Components[2], w = v.Components[3], t = v.Components[4];
                return 20*x*x - 8*x*y + 20*x*z + 29*y*y + 8*y*w + 14*y*t + 29*z*z + 6*z*t + 13*w*w + 4*w*t + 11*t*t + 16*x - 3*y - 7*z - 1*w - 18*t - 12; }, // Q23(X)
            v => { double x = v.Components[0], y = v.Components[1], z = v.Components[2], w = v.Components[3], t = v.Components[4];
                return 17*x*x - 2*x*y + 6*x*z + 16*y*y - 2*y*w - 16*y*t + 29*z*z - 14*z*t + 10*w*w + 22*w*t + 20*t*t - 13*x + 12*y + 9*z + 2*w + 26*t + 20; } // Q24(X)
        };

        public static double[][,] SplitCompact(double[,] compact, int dimensions)
        {
            int subspaces = (int)Math.Pow(2, dimensions);
            double[][,] result = new double[subspaces][,];

            double[] midPoints = new double[dimensions];
            for (int i = 0; i < dimensions; i++)
                midPoints[i] = (compact[i, 0] + compact[i, 1]) / 2;

            for (int i = 0; i < subspaces; i++)
            {
                result[i] = new double[dimensions, 2];
                for (int dim = 0; dim < dimensions; dim++)
                {
                    bool useUpperHalf = (i & (1 << dim)) != 0;
                    result[i][dim, 0] = useUpperHalf ? midPoints[dim] : compact[dim, 0];
                    result[i][dim, 1] = useUpperHalf ? compact[dim, 1] : midPoints[dim];
                }
            }
            return result;
        }

        public static Vector[] GenerateInitialSimplex(Vector x0, double step)
        {
            int n = x0.Dimension;
            Vector[] simplex = new Vector[n + 1];
            simplex[0] = x0;

            for (int i = 1; i <= n; i++)
            {
                double[] point = x0.Components.ToArray();
                point[i - 1] += step;
                simplex[i] = new Vector(point);
            }

            return simplex;
        }

        private static void RunMultiFunctionOptimization(Func<Vector, double>[] functions, int dim, double[,] compact, int method)
        {
            var subspaces = SplitCompact(compact, dim);
            int totalSubspaces = subspaces.Length;

            var allResults = new (Vector Point, double Value, int FunctionIndex, int Iterations)[functions.Length * totalSubspaces];


            // двойное параллельное выполнение ( по функциям и по подпространствам)
            Parallel.For(0, functions.Length, funcIndex =>
            {
                var func = functions[funcIndex];
                var results = new Vector[totalSubspaces];
                var optimizers = new NelderMeadOptimizer[totalSubspaces];

                Parallel.For(0, totalSubspaces, i =>
                {
                    optimizers[i] = new NelderMeadOptimizer
                    {
                        StretchMethod = method,
                        MaxIterations = 2000,
                        Tolerance = 1e-8
                    };

                    double[] center = new double[dim];
                    for (int j = 0; j < dim; j++)
                        center[j] = (subspaces[i][j, 0] + subspaces[i][j, 1]) / 2;

                    Vector x0 = new Vector(center);
                    Vector[] simplex = GenerateInitialSimplex(x0, 1.5);
                    results[i] = optimizers[i].Optimize(func, simplex, subspaces[i]);
                    allResults[funcIndex * totalSubspaces + i] = (results[i], func(results[i]), funcIndex, optimizers[i].IterationsCount);
                });
            });

            // Вывод результатов для каждого подпространства и функции
            for (int i = 0; i < totalSubspaces; i++)
            {
                Console.WriteLine($"\nПодпространство {i + 1}:");
                for (int j = 0; j < dim; j++)
                    Console.WriteLine($"  Ось {j}: {subspaces[i][j, 0]}..{subspaces[i][j, 1]}");

                for (int funcIndex = 0; funcIndex < functions.Length; funcIndex++)
                {
                    var result = allResults[funcIndex * totalSubspaces + i];
                    Console.WriteLine($"  Функция {funcIndex + 1}:");
                    Console.WriteLine($"    Лучшая точка: [{string.Join(", ", result.Point.Components.Select(x => x.ToString("F6")))}]");
                    Console.WriteLine($"    Значение функции: {result.Value:F10}");
                    Console.WriteLine($"    Итераций: {result.Iterations}");
                }
            }

            // Нахождение глобального минимума среди всех функций и подпространств
            var globalMin = allResults.OrderBy(r => r.Value).First();
            Console.WriteLine($"\nГлобальный минимум (Функция {globalMin.FunctionIndex + 1}):");
            Console.WriteLine($"  Точка: [{string.Join(", ", globalMin.Point.Components.Select(x => x.ToString("F6")))}]");
            Console.WriteLine($"  Значение функции: {globalMin.Value:F10}");
            Console.WriteLine($"  Итераций: {globalMin.Iterations}");
        }


        public static void Main()
        {
            Console.WriteLine("Выберите размерность (3, 4 или 5):");
            int dimension = int.Parse(Console.ReadLine());

            Console.WriteLine("Выберите метод растяжения:");
            Console.WriteLine("1 - Базовый алгоритм");
            Console.WriteLine("2 - Пассивный перебор");
            Console.WriteLine("3 - Золотое сечение");
            int method = int.Parse(Console.ReadLine());

            double[,] globalCompact = new double[5, 2] {
                { -20, 20 }, { -20, 20 }, { -20, 20 }, { -20, 20 }, { -20, 20 }
            };

            switch (dimension)
            {
                case 3:
                    RunMultiFunctionOptimization(Functions3D, 3, globalCompact, method);
                    break;
                case 4:
                    RunMultiFunctionOptimization(Functions4D, 4, globalCompact, method);
                    break;
                case 5:
                    RunMultiFunctionOptimization(Functions5D, 5, globalCompact, method);
                    break;
                default:
                    Console.WriteLine("Неподдерживаемая размерность");
                    break;
            }
        }
    }
}