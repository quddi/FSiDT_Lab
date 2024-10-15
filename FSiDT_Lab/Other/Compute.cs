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

        public static double CHIndex(List<DataRow> dataRows, int clustersCount)
        {
            var clustersDatas = KMeansClusterization(dataRows, clustersCount);

            return CHIndex(dataRows, clustersDatas);
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

        public static List<ClusterData> StartClustersCenters(List<DataRow> data, int clustersCount)
        {
            List<ClusterData> clustersDatas = new();

            var firstClusterCenter = data.Random();

            var rowPairDistances = data!
                .Except(new List<DataRow> { firstClusterCenter })
                .Select(row => (row, Math.Pow(Compute.EuclidDistance(firstClusterCenter.Coordinates!, row.Coordinates!), 2f)))
                .ToList();

            var squaredDistances = rowPairDistances
                .Select(pair => pair.Item2)
                .ToList();

            var sum = squaredDistances.Sum();

            var chances = squaredDistances
                .Select(squaredDistance => squaredDistance / sum)
                .ToList();

            var clustersCentersCoordinates = new List<Coordinates> { firstClusterCenter.Coordinates! };

            for (int i = 0; i < clustersCount - 1; i++)
            {
                var index = ExtensionsMethods.GetRandomIndex(chances);

                clustersCentersCoordinates.Add(rowPairDistances[index].Item1.Coordinates!);

                rowPairDistances.RemoveAt(index);
                squaredDistances.RemoveAt(index);
                chances.RemoveAt(index);
            }

            for (int i = 0; i < clustersCentersCoordinates.Count; i++)
            {
                clustersDatas.Add
                (
                    new ClusterData
                    {
                        Index = i,
                        Coordinates = clustersCentersCoordinates[i],
                        Color = Random.Shared.NextColor()
                    }
                );
            }

            return clustersDatas;
        }

        public static List<ClusterData> KMeansClusterization(List<DataRow> data, int clustersCount)
        {
            var clustersDatas = Compute.StartClustersCenters(data, clustersCount);
            var changedAnyCenter = true;

            for (int i = 0; i < Constants.MaxClasterizationIterations && changedAnyCenter; i++)
            {
                ReassignClusters(data, clustersDatas);

                var grouped = data.GroupBy(x => x.ClusterIndex);
                changedAnyCenter = false;

                foreach (var grouping in grouped)
                {
                    changedAnyCenter = TryRecomputeCenter(data, clustersDatas, grouping) || changedAnyCenter;
                }
            }

            return clustersDatas;
        }

        private static void ReassignClusters(List<DataRow> data, List<ClusterData> clusterDatas)
        {
            foreach (var dataRow in data)
            {
                var nearestCenter = clusterDatas.MinBy(centerData =>
                    Compute.EuclidDistance(dataRow.Coordinates!, centerData.Coordinates));

                var index = clusterDatas.IndexOf(nearestCenter!);

                dataRow.ClusterIndex = index;
            }
        }

        private static bool TryRecomputeCenter(List<DataRow> data, List<ClusterData> clusterDatas, IGrouping<int?, DataRow> grouping)
        {
            bool changedAnyCenter = false;

            var clusterIndex = grouping.Key;
            var dimensions = data.First().Coordinates!.Values.Count;
            var newAverageCoordinates = Compute.AverageCoordinates(dimensions, grouping);
            var currentAverageCoordinates = data[grouping.Key!.Value];

            if (newAverageCoordinates.IsEqual(currentAverageCoordinates.Coordinates!))
                changedAnyCenter = true;

            clusterDatas[grouping.Key!.Value].Coordinates = newAverageCoordinates;

            return changedAnyCenter;
        }

        public static Coordinates AverageCoordinates(int dimensions, IGrouping<int?, DataRow?> grouping)
        {
            var result = new Coordinates(dimensions);

            foreach (var dataRow in grouping)
            {
                for (int i = 0; i < result.Values.Count; i++)
                {
                    result.Values[i] += dataRow![i];
                }
            }

            var count = (double)grouping.Count();

            for (int i = 0; i < result.Values.Count; i++)
            {
                result.Values[i] /= count;
            }

            return result;
        }
    }
}
