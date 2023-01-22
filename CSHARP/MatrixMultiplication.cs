namespace CSHARP
{
    //D:\JA_projekt\JA_projekt\CSHARP\bin\Release\net6.0\CSHARP.dll
    public static class MatrixMultiplication
    {
        private static float[,] multiplyMatrixes(float[,] matrixA, float[,] matrixB)
        {
            var rowsA = matrixA.GetLength(0);
            var colsA = matrixA.GetLength(1);
            var rowsB = matrixB.GetLength(0);
            var colsB = matrixB.GetLength(1);

            if (colsA != rowsB)
            {
                return new[,] { { -1.0f }, { -1.0f }, { -1.0f } };
            }

            float[,] result = new float[rowsA, colsB];

            float sum = 0.0f;
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

        public static float[,] RGBtoLMS(float[,] rgbMatrix)
        {
            float[,] lms = new[,]
            {
            { 17.8824f,  43.5161f,    4.1194f },
            { 3.4557f,   27.1554f,    3.8671f },
            { 0.0300f,   0.1843f,     1.4671f }
            };

            return multiplyMatrixes(lms, rgbMatrix);

        }

        public static float[,] LMStoRGB(float[,] lmsMatrix)
        {
            float[,] rgb = new[,]
            {
            { 0.0809f,   -0.1305f,    0.1167f },
            { -0.0102f,  0.0540f,     -0.1136f },
            { -0.0004f,  -0.0041f,    0.6935f }
            };

            return multiplyMatrixes(rgb, lmsMatrix);

        }

        public static float[,] LMStoProtanopia(float[,] lmsMatrix)
        {
            float[,] protanopia = new[,]
            {
            { 0.0f,    2.0234f,   -2.5258f },
            { 0.0f,    1.0f,      0.0f },
            { 0.0f,    0.0f,      1.0f }
            };

            return multiplyMatrixes(protanopia, lmsMatrix);

        }

        public static float[,] rgbToPronatopia(float[,] rgb)
        {
            return
                 LMStoRGB(
                     LMStoProtanopia(
                               RGBtoLMS(
                                   rgb)));
        }
    }
}

