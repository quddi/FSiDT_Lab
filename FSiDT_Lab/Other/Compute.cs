using System.Windows.Controls;

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

        public static double CHIndex(List<DataRow> dataRows, List<ClusterData> clusterDatas)
        {
            var commonCenter = dataRows.Select(row => row.Coordinates)!.Average();
            var elementsByClusters = dataRows
                .GroupBy(row => row.ClusterIndex)
                .ToDictionary
                (
                    groupable => clusterDatas.First(clusterData => clusterData.Index == groupable.Key),
                    groupable => groupable.ToList()
                )!;

            var numerator = 0d;

            foreach (var clusterData in clusterDatas)
            {
                var distance = Compute.EuclidDistance(clusterData.Coordinates, commonCenter);
                var elementsCount = elementsByClusters[clusterData].Count;

                numerator += elementsCount * distance * distance / (clusterDatas.Count - 1);
            }

            var denominator = 0d;

            foreach (var clusterData in clusterDatas)
            {
                var elements = elementsByClusters[clusterData];

                foreach (var element in elements)
                {
                    var distance = Compute.EuclidDistance(element.Coordinates!, clusterData.Coordinates);
                    denominator += distance * distance / (dataRows.Count - clusterDatas.Count);
                }
            }

            return numerator / denominator;
        }
    }
}
