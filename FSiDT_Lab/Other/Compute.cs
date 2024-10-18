using System;
using System.Drawing.Text;
using System.Windows.Controls;

namespace FSiDT_Lab
{
    public static class Compute
    {
        public static double EuclideDistance(List<double> a, List<double> b)
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

        public static double? CHIndex(List<DataRow> dataRows, int clustersCount)
        {
            var clustersDatas = KMeansClusterization(dataRows, clustersCount);

            return clustersDatas.Count == clustersCount 
                ? CHIndex(dataRows, clustersDatas)
                : null;
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
                var distance = Compute.EuclideDistance(clusterData.Coordinates!, commonCenter);
                var elementsCount = elementsByClusters[clusterData].Count;

                numerator += elementsCount * distance * distance / (clusterDatas.Count - 1);
            }

            var denominator = 0d;

            foreach (var clusterData in clusterDatas)
            {
                var elements = elementsByClusters[clusterData];

                foreach (var element in elements)
                {
                    var distance = Compute.EuclideDistance(element.Coordinates!, clusterData.Coordinates);
                    denominator += distance * distance / (dataRows.Count - clusterDatas.Count);
                }
            }

            return numerator / denominator;
        }

        public static List<ClusterData> StartClustersCenters(List<DataRow> data, int clustersCount)
        {
            var clustersDatas = new List<ClusterData>();
            var firstClusterCenter = data.Random();
            var clustersCentersCoordinates = new List<Coordinates> { firstClusterCenter.Coordinates! };

            while (clustersCentersCoordinates.Count < clustersCount)
            {
                var distances = data
                    .Select(point => clustersCentersCoordinates
                        .Min(coordinates => Compute.EuclideDistance(point.Coordinates!.Values, coordinates!.Values)))
                    .ToList();

                var newClusterCenter = GetNextClusterCenter(data, distances);

                clustersCentersCoordinates.Add(newClusterCenter);
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

            Coordinates GetNextClusterCenter(List<DataRow> data, List<double> distances)
            {
                double totalDistance = distances.Sum();
                double randomValue = Random.Shared.NextDouble() * totalDistance;

                double cumulativeSum = 0.0;

                var a = data
                    .Zip(distances, (point, distance) => new { point, distance })
                    .First(item => (cumulativeSum += item.distance) >= randomValue);


                return a.point.Coordinates!;
            }
        }

        public static List<ClusterData> KMeansClusterization(List<DataRow> data, int clustersCount)
        {
            //var clustersDatas = new List<ClusterData> 
            //{ 
            //    new ClusterData { Index = 0, Color = new ScottPlot.Color(255, 0, 0), Coordinates = new Coordinates { Values = [28, 31, 26, 24, 23 ]}},
            //    new ClusterData { Index = 1, Color = new ScottPlot.Color(0, 255, 0), Coordinates = new Coordinates { Values = [33, 34, 30, 31, 29 ]}},
            //};
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

            return clustersDatas.Where(IsNotEmpty).ToList();

            bool IsNotEmpty(ClusterData clusterData)
            {
                return data.Any(dataRow => dataRow.ClusterIndex == clusterData.Index);
            }
        }

        private static void ReassignClusters(List<DataRow> data, List<ClusterData> clusterDatas)
        {
            foreach (var dataRow in data)
            {
                var distances = clusterDatas
                    .Select(clusterData => EuclideDistance(dataRow.Coordinates!, clusterData.Coordinates!))
                    .ToList();

                var index = distances.IndexOf(distances.Min());

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

            if (!newAverageCoordinates.IsEqual(currentAverageCoordinates.Coordinates!))
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
