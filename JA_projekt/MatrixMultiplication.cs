namespace JA_projekt
{
    public static class MatrixMultiplication
    {
        private static double[,] multiplyMatrixes(double[,] matrixA, double[,] matrixB)
        {
            var rowsA = matrixA.GetLength(0);
            var colsA = matrixA.GetLength(1);
            var rowsB = matrixB.GetLength(0);
            var colsB = matrixB.GetLength(1);

            if (colsA != rowsB)
            {
                return new[,] { { -1.0 }, { -1.0 }, { -1.0 } };
            }

            double[,] result = new double[rowsA, colsB];

            var sum = 0.0;
            for (int i = 0; i < rowsA; i++)
            {
                for (int j = 0; j < colsB; j++)
                {
                    sum = 0;
                    for (int k = 0; k < colsA; k++)
                    {
                        sum += matrixA[i, k] * matrixB[k, j];
                    }
                    result[i, j] = sum;
                }
            }

            return result;
        }

        public static double[,] RGBtoLMS(double[,] rgbMatrix)
        {
            double[,] lms = new[,]
            {
            { 17.8824,  43.5161,    4.1194 },
            { 3.4557,   27.1554,    3.8671 },
            { 0.0300,   0.1843,     1.4671 }
            };

            return multiplyMatrixes(lms, rgbMatrix);

        }

        public static double[,] LMStoRGB(double[,] lmsMatrix)
        {
            double[,] rgb = new[,]
            {
            { 0.0809,   -0.1305,    0.1167 },
            { -0.0102,  0.0540,     -0.1136 },
            { -0.0004,  -0.0041,    0.6935 }
            };

            return multiplyMatrixes(rgb, lmsMatrix);

        }

        public static double[,] LMStoProtanopia(double[,] lmsMatrix)
        {
            double[,] protanopia = new[,]
            {
            { 0.0,    2.0234,   -2.5258 },
            { 0.0,    1.0,      0.0 },
            { 0.0,    0.0,      1.0 }
            };

            return multiplyMatrixes(protanopia, lmsMatrix);

        }
    }
}
