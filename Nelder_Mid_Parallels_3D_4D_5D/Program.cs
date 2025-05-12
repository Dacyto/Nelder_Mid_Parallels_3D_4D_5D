using System;
using System.Linq;
using System.Threading.Tasks;

namespace Nelder_Mid_Parallels_3D_4D_5D
{
    public static class Program
    {
        // === 3D ===
        public static double Function3D_1(Vector v)
        {
            double x = v.Components[0], y = v.Components[1], z = v.Components[2];
            return 28 * x * x + 28 * y * y + 13 * z * z - 30 * x * y - 28 * x * z + 11 * x - 14 * y - 6 * z + 3;
        }

        public static double Function3D_2(Vector v)
        {
            double x = v.Components[0], y = v.Components[1], z = v.Components[2];
            return 33 * x * x - 26 * x * y + 34 * x * z + 20 * y * y + 29 * z * z + 20 * x - 2 * y + 12 * z - 12;
        }

        public static double Function3D_3(Vector v)
        {
            double x = v.Components[0], y = v.Components[1], z = v.Components[2];
            return 19 * x * x - 8 * x * y - 16 * x * z + 39 * y * y + 12 * z * z + 19 * x + 27 * y + 1 * z - 15;
        }

        // === 4D ===
        public static double Function4D_1(Vector v)
        {
            double x = v.Components[0], y = v.Components[1], z = v.Components[2], w = v.Components[3];
            return 7 * x * x + 36 * y * y + 25 * z * z + 38 * w * w + 2 * x * y - 22 * x * z + 68 * y * w + 7 * x + 35 * y - 6 * z + 27 * w - 26;
        }

        public static double Function4D_2(Vector v)
        {
            double x = v.Components[0], y = v.Components[1], z = v.Components[2], w = v.Components[3];
            return 2 * x * x + 8 * x * y - 4 * x * z + 25 * y * y - 14 * y * w + 26 * z * z + 13 * w * w + 21 * x - 3 * y + 3 * z + 5 * w - 11;
        }

        public static double Function4D_3(Vector v)
        {
            double x = v.Components[0], y = v.Components[1], z = v.Components[2], w = v.Components[3];
            return 23 * x * x + 32 * x * y + 4 * x * z + 45 * y * y + 22 * y * w + 1 * z * z + 35 * w * w + 13 * x - 15 * y - 13 * z + 29 * w - 24;
        }

        // === 5D ===
        public static double Function5D_1(Vector v)
        {
            double x = v.Components[0], y = v.Components[1], z = v.Components[2], w = v.Components[3], t = v.Components[4];
            return 23 * x * x + 9 * y * y + 17 * z * z + 3 * w * w + 18 * t * t + 16 * x * y + 22 * x * z + 2 * y * w - 4 * y * t - 22 * z * t - 4 * w * t - 21 * x + y - 7 * z - 21 * w - 25 * t + 24;
        }

        public static double Function5D_2(Vector v)
        {
            double x = v.Components[0], y = v.Components[1], z = v.Components[2], w = v.Components[3], t = v.Components[4];
            return 22 * x * x - 6 * x * y + 6 * x * z + 23 * y * y - 18 * y * w - 6 * y * t + 17 * z * z - 18 * z * t + 17 * w * w - 4 * w * t + 22 * t * t + 4 * x - 7 * y + 17 * z - 19 * w - 15 * t - 5;
        }

        public static double Function5D_3(Vector v)
        {
            double x = v.Components[0], y = v.Components[1], z = v.Components[2], w = v.Components[3], t = v.Components[4];
            return 20 * x * x - 8 * x * y + 20 * x * z + 29 * y * y + 8 * y * w + 14 * y * t + 29 * z * z + 6 * z * t + 13 * w * w + 4 * w * t + 11 * t * t + 16 * x - 3 * y - 7 * z - 1 * w - 18 * t - 12;
        }

        public static Func<Vector, double> CombineFunctions(Func<Vector, double>[] funcs)
        {
            return v => funcs.Min(f => f(v));
        }

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

        private static void RunOptimization(Func<Vector, double> func, int dim, double[,] compact, int method)
        {
            var subspaces = SplitCompact(compact, dim);
            int totalSubspaces = subspaces.Length;

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
            });

            for (int i = 0; i < totalSubspaces; i++)
            {
                Console.WriteLine($"\nПодпространство {i + 1}:");
                for (int j = 0; j < dim; j++)
                    Console.WriteLine($"  Ось {j}: {subspaces[i][j, 0]}..{subspaces[i][j, 1]}");

                Console.WriteLine($"  Лучшая точка: [{string.Join(", ", results[i].Components.Select(x => x.ToString("F6")))}]");
                Console.WriteLine($"  Значение функции: {func(results[i]):F10}");
                Console.WriteLine($"  Итераций: {optimizers[i].IterationsCount}");
            }

            var best = results.OrderBy(r => func(r)).First();
            Console.WriteLine($"\nГлобальный минимум: [{string.Join(", ", best.Components.Select(x => x.ToString("F6")))}]");
            Console.WriteLine($"Значение функции: {func(best):F10}");

            Func<Vector, double>[] selectedFunctions = dim switch
            {
                3 => new Func<Vector, double>[] { Function3D_1, Function3D_2, Function3D_3 },
                4 => new Func<Vector, double>[] { Function4D_1, Function4D_2, Function4D_3 },
                5 => new Func<Vector, double>[] { Function5D_1, Function5D_2, Function5D_3 },
                _ => null
            };

            if (selectedFunctions != null)
            {
                int bestFuncIndex = -1;
                double bestVal = double.MaxValue;
                for (int i = 0; i < selectedFunctions.Length; i++)
                {
                    double val = selectedFunctions[i](best);
                    if (val < bestVal)
                    {
                        bestVal = val;
                        bestFuncIndex = i;
                    }
                }
                Console.WriteLine($"Минимум достигнут функцией №{bestFuncIndex + 1}");
            }
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
                    var functions3D = new Func<Vector, double>[] { Function3D_1, Function3D_2, Function3D_3 };
                    RunOptimization(CombineFunctions(functions3D), 3, globalCompact, method);
                    break;
                case 4:
                    var functions4D = new Func<Vector, double>[] { Function4D_1, Function4D_2, Function4D_3 };
                    RunOptimization(CombineFunctions(functions4D), 4, globalCompact, method);
                    break;
                case 5:
                    var functions5D = new Func<Vector, double>[] { Function5D_1, Function5D_2, Function5D_3 };
                    RunOptimization(CombineFunctions(functions5D), 5, globalCompact, method);
                    break;
                default:
                    Console.WriteLine("Неподдерживаемая размерность");
                    break;
            }
        }
    }
}
