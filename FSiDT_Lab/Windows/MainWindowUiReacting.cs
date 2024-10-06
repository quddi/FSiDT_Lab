using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace FSiDT_Lab
{
    public partial class MainWindow : Window
    {
        private bool CanProceed
        {
            get
            {
                if (_currentData == null)
                {
                    MessageBox.Show("Для продовження завантажте дані!");
                    return false;
                }

                return true;
            }
        }

        private void UploadDataButtonClickHandler(object _, RoutedEventArgs __)
        {
            ResetAll();

            _currentData = DataLoader.LoadValues();

            UpdateDataTable();
            UpdateSignComboBoxesItems();
            UpdateParallelCoordinatesPlot();
            UpdateClustersCount();
        }

        private void FirstSignComboBoxSelectionChangedHandler(object _, SelectionChangedEventArgs eventArgs)
        {
            UpdateTwoSignsPlot();
            UpdateSignComboBoxesState();
        }

        private void SecondSignComboBoxSelectionChangedHandler(object sender, SelectionChangedEventArgs eventArgs)
        {
            UpdateTwoSignsPlot();
            UpdateSignComboBoxesState();
        }

        private void SwapSignComboBoxesClickHandler(object _, RoutedEventArgs __)
        {
            (FirstSignComboBox.SelectedIndex, SecondSignComboBox.SelectedIndex) 
                =
            (SecondSignComboBox.SelectedIndex, FirstSignComboBox.SelectedIndex);
        }

        private void ClustersCountInputTextBoxTextChangedHandler(object _, TextChangedEventArgs __)
        {
            if (_isInitialized)
                UpdateClustersCount();
        }

        private void ClusterizeButtonClickHandler(object _, RoutedEventArgs __)
        {
            if (!CanProceed)
                return;

            Clusterize();
        }

        private void ComputeClustersCenters()
        {
            var firstClusterCenter = _currentData!.Random();

            var rowPairDistances = _currentData!
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

            var clustersCenters = new List<Coordinates> { firstClusterCenter.Coordinates! };

            for (int i = 0; i < _clustersCount - 1; i++)
            {
                var index = ExtensionsMethods.GetRandomIndex(chances);

                clustersCenters.Add(rowPairDistances[index].Item1.Coordinates!);

                rowPairDistances.RemoveAt(index);
                squaredDistances.RemoveAt(index);
                chances.RemoveAt(index);
            }

            _clustersCenters = clustersCenters;
        }

        private void Clusterize()
        {
            ComputeClustersCenters();

            var changedAnyCenter = true;

            for (int i = 0; i < Constants.MaxClasterizationIterations && changedAnyCenter; i++)
            {
                ReassignClusters();

                var grouped = _currentData!.GroupBy(x => x.ClusterIndex);
                changedAnyCenter = false;

                foreach (var grouping in grouped)
                {
                    changedAnyCenter = TryRecomputeCenter(grouping) || changedAnyCenter;
                }
            }
        }

        private bool TryRecomputeCenter(IGrouping<int?, DataRow> grouping)
        {
            bool changedAnyCenter = false;

            var clusterIndex = grouping.Key;

            var newAverageCoordinates = GetAverageCoordinates(grouping);
            var currentAverageCoordinates = _clustersCenters![grouping.Key!.Value];

            if (newAverageCoordinates.IsEqual(currentAverageCoordinates))
                changedAnyCenter = true;

            _clustersCenters![grouping.Key!.Value] = newAverageCoordinates;
            
            return changedAnyCenter;
        }

        private void ReassignClusters()
        {
            foreach (var dataRow in _currentData!)
            {
                var nearestCenter = _clustersCenters!.MinBy(center =>
                    Compute.EuclidDistance(dataRow.Coordinates!, center));

                var index = _clustersCenters!.IndexOf(nearestCenter!);

                dataRow.ClusterIndex = index;
            }
        }

        private Coordinates GetAverageCoordinates(IGrouping<int?, DataRow?> grouping)
        {
            var result = new Coordinates(Dimensions!.Value);

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
