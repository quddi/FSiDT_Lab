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
            if (indexesChances.Sum().IsNotEqual(Constants.MaxRelativeChance))
                throw new ArgumentException("Indexes chances sum was not 100!");

            var randomChance = System.Random.Shared.NextDouble(0d, Constants.MaxRelativeChance);

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
    }
}
