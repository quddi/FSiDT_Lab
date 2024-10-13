using System.Drawing;

namespace FSiDT_Lab
{
    public static class ExtensionsMethods
    {
        public static T Random<T>(this IList<T> list)
        {
            return list[System.Random.Shared.Next(list.Count)];
        }

        public static int GetRandomIndex(List<double> indexesChances)
        {
            var maxChance = indexesChances.Sum();

            var randomChance = System.Random.Shared.NextDouble(0d, maxChance);

            var sum = 0d;

            for (int i = 0; i < indexesChances.Count; i++)
            {
                if (sum < randomChance && randomChance < sum + indexesChances[i])
                    return i;

                sum += indexesChances[i];
            }

            return indexesChances.Count - 1;
        }

        public static bool IsEqual(this double a, double b)
        {
            double v = Math.Abs(a - b);
            return v < Constants.DoubleComparsionTolerance;
        }

        public static bool IsEqual(this Coordinates a, Coordinates b)
        {
            return a.Values
                .Zip(b.Values, (x, y) => (x, y))
                .All(pair => pair.x.Equals(pair.y));
        }

        public static bool IsNotEqual(this double a, double b)
        {
            return !a.IsEqual(b);
        }

        public static bool IsNotEqual(this Coordinates a, Coordinates b)
        {
            return !a.IsEqual(b);
        }

        public static double NextDouble(this System.Random random, double min, double max)
        {
            return random.NextDouble() * (max - min) + min;
        }

        public static ScottPlot.Color NextColor(this System.Random random)
        {
            return new ScottPlot.Color
            (
                (byte)random.Next(Constants.MaxColorComponent + 1),
                (byte)random.Next(Constants.MaxColorComponent + 1),
                (byte)random.Next(Constants.MaxColorComponent + 1)
            );
        }

        public static ScottPlot.Color ConvertToScott(this System.Drawing.Color color)
        {
            return new ScottPlot.Color(color);
        }

        public static System.Drawing.Color ConvertToDrawing(this ScottPlot.Color color)
        {
            return System.Drawing.Color.FromArgb(color.A, color.R, color.G, color.B);
        }

        public static System.Windows.Media.Color ConvertToMedia(this ScottPlot.Color color)
        {
            return new System.Windows.Media.Color
            {
                A = color.A,
                R = color.R,
                G = color.G,
                B = color.B,
            };
        }

        public static List<double> Average(this List<List<double>> doubles)
        {
            var dimensions = doubles.First().Count;
            var count = doubles.Count;
            var result = Enumerable.Repeat(0.0, dimensions).ToList();

            foreach (var list in doubles!)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    result[i] += list[i] / count;
                }
            }

            return result;
        }

        public static Coordinates Average(this IEnumerable<Coordinates> coordinates)
        {
            return new Coordinates 
            { 
                Values = coordinates.Select(coordinate => coordinate.Values).ToList().Average()
            };
        }
    }
}
