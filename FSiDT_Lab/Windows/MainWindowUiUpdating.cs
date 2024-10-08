
using ScottPlot;
using System.Drawing;
using System.Reflection.Metadata;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;

namespace FSiDT_Lab
{
    public partial class MainWindow : Window
    {
        private List<string> _firstSignComboBoxItemsSource = new();
        private List<string> _secondSignComboBoxItemsSource = new();
        private Dictionary<Coordinates, ScottPlot.Color> _clustersColors = new();

        private string FirstSelectedLabel => _firstSignComboBoxItemsSource[FirstSignComboBox.SelectedIndex];
        private string SecondSelectedLabel => _secondSignComboBoxItemsSource[SecondSignComboBox.SelectedIndex];

        private int FirstSelectedIndex => SignLabel.ToIndex(FirstSelectedLabel)!.Value;
        private int SecondSelectedIndex => SignLabel.ToIndex(SecondSelectedLabel)!.Value;

        private bool AnySignComboBoxInEmpty => FirstSignComboBox.SelectedIndex == Constants.NoElementComboBoxIndex ||
                SecondSignComboBox.SelectedIndex == Constants.NoElementComboBoxIndex;

        private bool SignComboBoxesHaveSameValue => FirstSignComboBox.SelectedIndex == SecondSignComboBox.SelectedIndex;

        private bool SignComboBoxesOk => !AnySignComboBoxInEmpty && !SignComboBoxesHaveSameValue;

        private bool IsClusterized => _currentData!.All(row => row.ClusterIndex != null);

        private void UpdateDataTable()
        {
            if (_currentData == null)
                return;

            ResetDataTable();
            
            SetColumns();

            foreach (var row in _currentData)
            {
                DataTable.Items.Add(row);
            }
        }

        private void SetColumns()
        {
            DataTable.Columns.Add
            (
                new DataGridTextColumn()
                {
                    Header = "№",
                    FontSize = Constants.FontSize,
                    Binding = new Binding("Index"),
                    Width = 30
                }
            );

            for (int i = 0; i < Dimensions; i++)
            {
                DataTable.Columns.Add
                (
                    new DataGridTextColumn
                    {
                        Header = SignLabel.FromIndex(i),
                        FontSize = Constants.FontSize,
                        Binding = new Binding($"[{i}]"),
                        Width = 100
                    }
                );
            }

            if (IsClusterized)
            {
                DataTable.Columns.Add
                (
                    new DataGridTextColumn
                    {
                        Header = "Кластер",
                        FontSize = Constants.FontSize,
                        Binding = new Binding($"ClusterIndex"),
                        Width = 100
                    }
                );
            }   
        }

        private void UpdateSignComboBoxesItems()
        {
            if (_currentData == null) return;

            _firstSignComboBoxItemsSource.Clear();
            _secondSignComboBoxItemsSource.Clear();

            FirstSignComboBox.SelectedIndex = Constants.NoElementComboBoxIndex;
            SecondSignComboBox.SelectedIndex = Constants.NoElementComboBoxIndex;

            _firstSignComboBoxItemsSource = Enumerable.Range(0, Dimensions!.Value)
                .Select(SignLabel.FromIndex)
                .ToList()!;

            _secondSignComboBoxItemsSource = Enumerable.Range(0, Dimensions!.Value)
                .Select(SignLabel.FromIndex)
                .ToList()!;

            FirstSignComboBox.ItemsSource = _firstSignComboBoxItemsSource;
            SecondSignComboBox.ItemsSource = _secondSignComboBoxItemsSource;
        }

        private void UpdateSignComboBoxesState()
        {
            if (AnySignComboBoxInEmpty)
            {
                ResetSignComboBoxesStateTextBox();
            }
            else
            {
                var ok = !SignComboBoxesHaveSameValue;

                SignComboBoxesStateTextBox.Background = ok
                    ? Constants.OkBrush
                    : Constants.NotOkBrush;

                SignComboBoxesStateTextBox.Text = ok
                    ? "✓"
                    : "Значення не мають співпадати!";
            }
        }

        private void UpdateTwoSignsPlot()
        {
            if (FirstSignComboBox.SelectedIndex == Constants.NoElementComboBoxIndex) return;
            
            if (SecondSignComboBox.SelectedIndex == Constants.NoElementComboBoxIndex) return;
        
            if (FirstSignComboBox.SelectedIndex == SecondSignComboBox.SelectedIndex) return;

            ResetTwoSignsPlot();

            var firstIndex = FirstSelectedIndex;
            var secondIndex = SecondSelectedIndex;

            TwoSignsPlot.Plot.XLabel(FirstSelectedLabel);
            TwoSignsPlot.Plot.YLabel(SecondSelectedLabel);

            for (var i = 0; i < _currentData!.Count; i++)
            {
                var coordinates = _currentData[i].Coordinates!;
                var clusterCenterCoordinates = _clustersCenters?[_currentData[i].ClusterIndex!.Value];
                var color = IsClusterized
                    ? _clustersColors[clusterCenterCoordinates!]
                    : Constants.DefaultPlotColor;

                TwoSignsPlot.Plot.Add.Scatter(coordinates.Values[firstIndex], 
                    coordinates.Values[secondIndex], color: color);
            }

            TwoSignsPlot.Refresh();
        }

        private void UpdateParallelCoordinatesPlot()
        {
            ResetParallelCoordinatesPlot();

            var xs = Enumerable.Range(0, Dimensions!.Value).ToList();
            var isClusterized = IsClusterized;

            foreach (var dataRow in _currentData!)
            {
                var color = IsClusterized 
                    ? _clustersColors[_clustersCenters![dataRow.ClusterIndex!.Value]]
                    : Constants.DefaultPlotColor;

                DrawCurve(dataRow, color);
            }

            ParallelCoordinatesPlot.Refresh();

            void DrawCurve(DataRow row, ScottPlot.Color color)
            {
                var coordinates = xs
                    .Zip(row.Coordinates!.Values, (x, y) => new ScottPlot.Coordinates { X = x, Y = y })
                    .ToArray();

                ParallelCoordinatesPlot.Plot.Add
                    .ScatterPoints(coordinates, color: color);

                for (int i = 0; i < Dimensions - 1; i++)
                {
                    var line = ParallelCoordinatesPlot.Plot.Add.Line(coordinates[i], coordinates[i + 1]);
                    line.Color = color;
                }
            }
        }

        private void UpdateClustersCount()
        {
            var input = ClustersCountInputTextBox.Text;

            if (string.IsNullOrEmpty(input))
            {
                ClustersCountMessageTextBox.Background = Constants.DefaultTextBoxBrush;
                ClustersCountMessageTextBox.Text = string.Empty;
                return;
            }

            if (_currentData == null)
            {
                ClustersCountMessageTextBox.Background = Constants.DefaultTextBoxBrush;
                ClustersCountMessageTextBox.Text = "Очікуються дані!";
                return;
            }

            if (!int.TryParse(input, out var clustersCount))
            {
                ClustersCountMessageTextBox.Background = Constants.NotOkBrush;
                ClustersCountMessageTextBox.Text = "Не вдалося зчитати значення!";

                _clustersCount = null;

                return;
            }

            if (1 < clustersCount && clustersCount < _currentData.Count / 2)
            {
                ClustersCountMessageTextBox.Background = Constants.OkBrush;
                ClustersCountMessageTextBox.Text = "✓";
                _clustersCount = clustersCount;
            }
            else
            {
                ClustersCountMessageTextBox.Background = Constants.NotOkBrush;
                ClustersCountMessageTextBox.Text = $"Значення має бути [2, {_currentData.Count / 2}]!";

                _clustersCount = null;
            }
        }
    }
}
