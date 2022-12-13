using System;

namespace RGUnityTools
{
    public static class RGXMath
    {
        //==================
        //General constants:
        //==================

        /// <summary>
        /// Square root of 2.
        /// </summary>
        public const float pythagoras = 1.41421356f;

        /// <summary>
        /// Square root of 3.
        /// </summary>
        public const float theodorus = 1.73205080f;

        /// <summary>
        /// Cube root of 2.
        /// </summary>
        public const float delian = 1.25992104f;

        //===================
        //Geometry constants:
        //===================

        /// <summary>
        ///Ratio of a circle's circumference to its diameter.
        /// </summary>
        public const float pi = 3.14159265f;

        /// <summary>
        ///The Golden Ratio.
        /// </summary>
        public const float phi = 1.61803398f;

        /// <summary>
        ///The Golden Angle in radians.
        /// </summary>
        public const float bRad = 2.39996322f;

        /// <summary>
        ///The Golden Angle in degrees.
        /// </summary>
        public const float bDeg = 137.507764f;

        //==================
        //Physics constants:
        //==================

        /// <summary>
        /// Acceleration caused by gravity in a vacuum on Earth's surface.
        /// </summary>
        public const float g = 9.80665f;

        /// <summary>
        /// Gravitational constant.
        /// </summary>
        public const float G = 0.00000000006674f;

        //==========
        //Sequences:
        //==========

        /// <summary>
        /// Returns the nth term of the Lucas sequence where n is less than 99. p = term 0. q = term 1.
        /// </summary>
        public static long LucasSequence(int n, int p, int q)
        {
            if (n >= 99)
                return 0;

            long a = p;
            long b = q;

            if (n == 0)
                return a;
            if (n == 1)
                return b;

            long l = 0;

            for (int i = 2; i <= n; i++)
            {
                l = a + b;
                a = b;
                b = l;
            }

            return l;
        }

        /// <summary>
        /// Returns the nth Lucas number where n is less than 99.
        /// </summary>
        public static long LucasNumber(int n)
        {
            return LucasSequence(n, 2, 1);
        }

        /// <summary>
        /// Returns the nth Fibonacci number where n is less than 99.
        /// </summary>
        public static long FibonacciNumber(int n)
        {
            return LucasSequence(n, 0, 1);
        }

        /// <summary>
        /// Returns X and Y co-ordiantes of evenly spaced points on the cicumference of a circle with a given centre point and radius.
        /// </summary>
        public static float[,] PointsOnACircle(float xPosition = 0f, float yPosition = 0f, int quantity = 10, float radius = 1f)
        {
            float theta = (float)Math.PI * 2 / quantity;
            float angle;

            float[,] newPoints = new float[quantity, 2];

            for (int i = 0; i < quantity; i++)
            {
                angle = theta * (i + 1);

                newPoints[i, 0] = radius * (float)Math.Cos(angle) + xPosition;
                newPoints[i, 1] = radius * (float)Math.Sin(angle) + yPosition;
            }

            return newPoints;
        }
    }
}
