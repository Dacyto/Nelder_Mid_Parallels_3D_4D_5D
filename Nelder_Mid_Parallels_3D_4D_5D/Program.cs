using System;
using System.Linq;
using System.Threading.Tasks;

namespace Nelder_Mid_Parallels_3D_4D_5D
{
    public static class Program
    {
        public static double Function3D(Vector v) // размерность N = 3
        {
            double x = v.Components[0], y = v.Components[1], z = v.Components[2];
            //return 28 * x * x + 28 * y * y + 13 * z * z - 30 * x * y - 28 * x * z + 11 * x - 14 * y - 6 * z + 3;    // Q1(X)
            //return 33 * x * x - 26 * x * y + 34 * x * z + 20 * y * y + 29 * z * z + 20 * x - 2 * y + 12 * z - 12;  // Q2(X)
            return 19 * x * x - 8 * x * y - 16 * x * z + 39 * y * y + 12 * z * z + 19 * x + 27* y + 1 * z - 15; // Q3(X)
            //return 23 * x * x - 18 * x * y - 30 * x * z + 15 * y * y + 32 * z * z - 4 * x + 38 * y + 2 * z - 40;    // Q4(X)
            //return 27 * x * x - 30 * x * y - 4 * x * z + 25*y*y + 25*z*z + 20 *x + 17 * y + 29 * z - 1; // Q5(X)
            //return 22 * x*x - 14 * x * y + 30 * x * z + 18*y*y + 19* z*z + 35* x - 8 * y - 11 * z - 22;   // Q6(X)
            //return 25 * x*x + 2 * x * y - 8 * x * z + 2 * y*y + 39*z*z + 37* x + 6 * y - 39 * z - 39;    // Q7(X)
            //return 15 * x*x + 18 *x *y - 32* x * z + 27 * y*y + 22 * z*z - 37 * x - 19*y + 14* z + 13;   // Q8(X)
            //return 12 * x*x + 30 * x*y -6 * x*z + 28*y*y + 10*z*z - 9 * x - 5 * y + 25*z + 36;       // Q9(X)
            //return 38 * x*x + 22 * x*y - 2*x*z + 26 * y*y + 34 * z*z + 8 * x - 35 * y + 10*z + 3;    // Q10(X)
        }
        public static double Function4D(Vector v)  // размерность N = 4
        {
            double x = v.Components[0], y = v.Components[1], z = v.Components[2], w = v.Components[3];
            //return 7 * x * x + 36 * y * y + 25 * z * z + 38 * w * w + 2 * x * y - 22 * x * z + 68 * y * w + 7 * x + 35 * y - 6 * z + 27 * w - 26;   // Q11(X)
            //return 2 * x*x + 8 * x*y - 4 * x*z + 25*y*y - 14*y*w + 26*z*z + 13*w*w + 21*x - 3*y + 3*z + 5 *w - 11; // Q12(X)
            return 23 * x*x + 32 * x*y + 4 * x*z + 45*y*y + 22 *y*w + 1*z*z + 35*w*w + 13*x - 15*y - 13*z + 29*w - 24;  // Q13(X)
            //return 14 * x*x - 10 * x*y + 6*x*z + 5*y*y + 16*y*w + 49*z*z + 28*w*w + 16*x - 14*y + 40*z + 22*w + 32;  // Q14(X)
            //return 49 * x*x + 50*x*y - 36*x*z + 46*y*y + 22*y*w + 48*z*z + 5*w*w - 7*x + 41*y + 2*z - 16*w + 4;  // Q15(X)
            //return 44 * x*x + 32*x*y + 56*x*z + 40*y*y + 26*y*w + 27*z*z + 17*w*w - 15*x + 19*y - 19*z + 45*w + 46;   // Q16(X)
            //return 7 * x*x - 4*x*y - 18*x*z + 30*y*y - 64*y*w + 22*z*z + 49*w*w - 19*x -44*y - 2*z + 9*w + 30;  // Q17(X)
            //return 16 * x*x -24*x*y + 40*x*z + 48*y*y - 18*y*w + 34*z*z + 32*w*w + 35*x + 7 * y + 46*z + 37*w - 43;   // Q18(X)
            //return 18 * x*x - 48*x*y + 10*x*z + 49 * y*y + 18*y*w + 43*z*z + 6*w*w + 32*x -22 *y -35*z - 12*w + 48;   // Q19(X)
            //return 24 * x*x - 16*x*y + 12*x*z + 44*y*y - 84*y*w + 26*z*z + 44*w*w - 4*x -28*y - 31*z + 27*w + 28;   // Q20(X)
        }
        public static double Function5D(Vector v) // размерность N = 5
        {
            double x = v.Components[0], y = v.Components[1], z = v.Components[2], w = v.Components[3], t = v.Components[4];
            //return 23 * x * x + 9 * y * y + 17 * z * z + 3 * w * w + 18 * t * t +16 * x * y + 22 * x * z + 2 * y * w - 4 * y * t - 22 * z * t - 4 * w * t - 21 * x + y - 7 * z - 21 * w - 25 * t + 24;  // Q21(X)
            //return 22 * x*x - 6*x*y + 6*x*z + 23*y*y - 18*y*w - 6*y*t + 17*z*z - 18*z*t + 17*w*w - 4*w*t + 22*t*t + 4*x - 7*y + 17*z -19*w - 15*t -5;  // Q22(X)
            return 20*x*x - 8*x*y + 20*x*z + 29*y*y + 8*y*w + 14*y*t + 29*z*z + 6*z*t + 13*w*w + 4*w*t + 11*t*t + 16*x - 3*y - 7*z - 1*w - 18*t -12;   // Q23(X)
            //return 17*x*x - 2*x*y + 6*x*z + 16*y*y - 2*y*w - 16*y*t + 29*z*z - 14*z*t + 10*w*w + 22*w*t + 20*t*t -13 *x + 12*y + 9*z + 2*w + 26*t + 20;  // Q24(X)
            //return 10*x*x - 10*x*y + 20*x*z + 26*y*y - 6*y*w -12*y*t + 20*z*z -10*z*t + 22*w*w - 8*w*t + 10*t*t -23*x + 10*y + 21*z + 19*w + 21*t - 29; // Q25(X)
            //return 16*x*x + 8*x*y -26 *x*z + 29*y*y - 42*y*w + 14*y*t + 18*z*z - 10*z*t + 29*w*w + 14*w*t + 24*t*t + 20*x + 8*y - 19*z - 2*w - 12*t - 25;  // Q26(X)
            //return 28 * x*x - 24*x*y - 20*x*z + 19*y*y - 16*y*w - 18*y*t + 26*z*z - 20*z*t + 19*w*w + 40*w*t + 27*t*t - 5*x + 10*y - 11*z + 13*w - 30*t - 13; // Q27(X)
            //return 24 * x*x + 4*x*y + 30*x*z + 10*y*y + 26*y*w + 2*y*t + 21*z*z + 4*z*t + 25*w*w - 18*w*t + 18*t*t + 29*x + 13*y + 23*z + 3*w + 19*t + 17;     // Q28(X)
            //return 22 * x*x  + 12*x*y + 8*x*z + 27*y*y + 4*y*w + 22*y*t + 25*z*z + 28*z*t + 24*w*w - 6*w*t + 26*t*t - 9*x + 1*y - 19*z - 1*w - 19*t + 9;   // Q29(X)
            //return 26 *x*x + 26*x*y + 8*x*z + 22*y*y - 10*y*w + 28*y*t + 26*z*z -24 *z*t + 14*w*w - 10*w*t + 17*t*t - 28*x + 12*y - 20*z - 11*w - 13*t + 23;   // Q30(X)
        }


        // Генерация под-компактов для N-мерного случая
        public static double[][,] SplitCompact(double[,] compact, int dimensions)
        {
            int subspaces = (int)Math.Pow(2, dimensions);
            double[][,] result = new double[subspaces][,];

            // Вычисляем середины для каждой оси
            double[] midPoints = new double[dimensions];
            for (int i = 0; i < dimensions; i++)
                midPoints[i] = (compact[i, 0] + compact[i, 1]) / 2;

            // Генерируем все возможные комбинации
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

                // Центр текущего под-компакта
                double[] center = new double[dim];
                for (int j = 0; j < dim; j++)
                    center[j] = (subspaces[i][j, 0] + subspaces[i][j, 1]) / 2;

                Vector x0 = new Vector(center);
                Vector[] simplex = GenerateInitialSimplex(x0, 1.5);
                results[i] = optimizers[i].Optimize(func, simplex, subspaces[i]);
            });

            // Вывод результатов
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

            // Общий компакт для всех измерений
            double[,] globalCompact = new double[5, 2] {
                { -20, 20 }, { -20, 20 }, { -20, 20 }, { -20, 20 }, { -20, 20 } 
            };

            switch (dimension)
            {
                case 3:
                    RunOptimization(Function3D, 3, globalCompact, method);
                    break;
                case 4:
                    RunOptimization(Function4D, 4, globalCompact, method);
                    break;
                case 5:
                    RunOptimization(Function5D, 5, globalCompact, method);
                    break;
                default:
                    Console.WriteLine("Неподдерживаемая размерность");
                    break;
            }
        
        }
    }
}