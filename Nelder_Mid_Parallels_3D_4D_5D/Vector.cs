using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nelder_Mid_Parallels_3D_4D_5D
{

    public class Vector
    {
        public double[] Components { get; }

        public Vector(params double[] components)
        {
            Components = components;
        }

        public int Dimension => Components.Length;

        public static Vector operator +(Vector a, Vector b)
        {
            if (a.Dimension != b.Dimension)
                throw new ArgumentException("Vectors must have the same dimension");

            double[] result = new double[a.Dimension];
            for (int i = 0; i < a.Dimension; i++)
                result[i] = a.Components[i] + b.Components[i];

            return new Vector(result);
        }

        public static Vector operator -(Vector a, Vector b)
        {
            if (a.Dimension != b.Dimension)
                throw new ArgumentException("Vectors must have the same dimension");

            double[] result = new double[a.Dimension];
            for (int i = 0; i < a.Dimension; i++)
                result[i] = a.Components[i] - b.Components[i];

            return new Vector(result);
        }

        public static Vector operator *(double scalar, Vector v)
        {
            double[] result = new double[v.Dimension];
            for (int i = 0; i < v.Dimension; i++)
                result[i] = scalar * v.Components[i];

            return new Vector(result);
        }

        public double Norm()
        {
            double sum = 0;
            foreach (var x in Components)
                sum += x * x;
            return Math.Sqrt(sum);
        }
    }
}