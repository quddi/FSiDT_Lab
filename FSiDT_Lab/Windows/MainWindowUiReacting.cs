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
                if (_context == null)
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

            _context = new Context(FirstSignComboBox, SecondSignComboBox)
            {
                CurrentData = DataLoader.LoadValues()
            };

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
            _context.ClustersCentersDatas = new();

            var firstClusterCenter = _context.CurrentData!.Random();

            var rowPairDistances = _context.CurrentData!
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

            for (int i = 0; i < _context.ClustersCount - 1; i++)
            {
                var index = ExtensionsMethods.GetRandomIndex(chances);

                clustersCentersCoordinates.Add(rowPairDistances[index].Item1.Coordinates!);

                rowPairDistances.RemoveAt(index);
                squaredDistances.RemoveAt(index);
                chances.RemoveAt(index);
            }

            for (int i = 0; i < clustersCentersCoordinates.Count; i++)
            {
                _context.ClustersCentersDatas.Add
                (
                    new ClusterData
                    {
                        Index = i,
                        Coordinates = clustersCentersCoordinates[i],
                        Color = Random.Shared.NextColor()
                    }
                );
            }
        }

        private void Clusterize()
        {
            ComputeClustersCenters();

            var changedAnyCenter = true;

            for (int i = 0; i < Constants.MaxClasterizationIterations && changedAnyCenter; i++)
            {
                ReassignClusters();

                var grouped = _context.CurrentData!.GroupBy(x => x.ClusterIndex);
                changedAnyCenter = false;

                foreach (var grouping in grouped)
                {
                    changedAnyCenter = TryRecomputeCenter(grouping) || changedAnyCenter;
                }
            }

            if (_context.SignComboBoxesOk) 
                UpdateTwoSignsPlot();

            UpdateParallelCoordinatesPlot();
            UpdateDataTable();
        }

        private bool TryRecomputeCenter(IGrouping<int?, DataRow> grouping)
        {
            bool changedAnyCenter = false;

            var clusterIndex = grouping.Key;

            var newAverageCoordinates = GetAverageCoordinates(grouping);
            var currentAverageCoordinates = _context.ClustersCentersDatas![grouping.Key!.Value];

            if (newAverageCoordinates.IsEqual(currentAverageCoordinates.Coordinates))
                changedAnyCenter = true;

            _context.ClustersCentersDatas![grouping.Key!.Value].Coordinates = newAverageCoordinates;
            
            return changedAnyCenter;
        }

        private void ReassignClusters()
        {
            foreach (var dataRow in _context.CurrentData!)
            {
                var nearestCenter = _context.ClustersCentersDatas!.MinBy(centerData =>
                    Compute.EuclidDistance(dataRow.Coordinates!, centerData.Coordinates));

                var index = _context.ClustersCentersDatas!.IndexOf(nearestCenter!);

                dataRow.ClusterIndex = index;
            }
        }

        private Coordinates GetAverageCoordinates(IGrouping<int?, DataRow?> grouping)
        {
            var result = new Coordinates(_context.Dimensions!.Value);

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
