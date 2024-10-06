namespace FSiDT_Lab
{
    public static class Compute
    {
        public static double EuclidDistance(List<double> a, List<double> b)
        {
            if (a.Count == 0)
                throw new ArgumentException("a contains 0 elements!");

            if (b.Count == 0)
                throw new ArgumentException("b contains 0 elements!");

            if (a.Count != b.Count)
                throw new ArgumentException($"a contains {a.Count} elements, when b {b.Count}!");

            return Math.Sqrt(a
                .Zip(b, (aElement, bElement) => Math.Pow(aElement - bElement, 2))
                .Sum());
        }
    }
}
