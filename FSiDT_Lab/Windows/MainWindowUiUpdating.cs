
using ScottPlot;
using System.Diagnostics.Metrics;
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
        private void UpdateDataTable()
        {
            if (_context?.CurrentData == null)
                return;

            SetDataTableColumns();

            var averageCoordinates = _context.CurrentData
                .Select(data => data.Coordinates)!
                .Average();

            DataTable.Items.Add(new DataRow
            {
                Coordinates = averageCoordinates,
                IsAverageData = true
            });

            foreach (var row in _context.CurrentData)
            {
                DataTable.Items.Add(row);
            }
        } 

        private void SetDataTableColumns()
        {
            ResetDataTable();

            DataTable.Columns.Add
            (
                new DataGridTextColumn()
                {
                    Header = "№",
                    FontSize = Constants.FontSize,
                    Binding = new Binding("IndexString"),
                    Width = 70
                }
            );

            for (int i = 0; i < _context!.Dimensions; i++)
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

            if (_context.IsClusterized)
            {
                DataTable.Columns.Add
                (
                    new DataGridTextColumn
                    {
                        Header = "Кластер",
                        FontSize = Constants.FontSize,
                        Binding = new Binding($"ClusterIndexString"),
                        Width = 100
                    }
                );
            }
        }

        private void UpdateClustersCentersTable()
        {
            if (_context?.CurrentData == null || !_context.IsClusterized)
                return;

            SetClustersCentersTableColumns();

            foreach (var row in _context.ClustersDatas!)
            {
                ClustersCentersTable.Items.Add(row);
            }
        }

        private void SetClustersCentersTableColumns()
        {
            ResetClustersCentersTable();
            
            ClustersCentersTable.Columns.Add
            (
                new DataGridTextColumn()
                {
                    Header = "№",
                    FontSize = Constants.FontSize,
                    Binding = new Binding("Index"),
                    Width = 30
                }
            );

            for (int i = 0; i < _context!.Dimensions; i++)
            {
                ClustersCentersTable.Columns.Add
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
        } 

        private void UpdateClustersCountsEvaluationTable()
        {
            if (_context?.CurrentData == null)
                return;

            SetClustersCountsEvaluationTableColumns();

            foreach (var evaluation in _context.ClustersCountsEvaluation!)
            {
                ClustersCountsEvaluationTable.Items.Add(evaluation);
            }
        }

        private void SetClustersCountsEvaluationTableColumns()
        {
            ResetClustersCountsEvaluationTable();

            ClustersCountsEvaluationTable.Columns.Add
            (
                new DataGridTextColumn
                {
                    Header = "Кількість",
                    FontSize = Constants.FontSize,
                    Binding = new Binding("ClustersCount"),
                }
            );

            ClustersCountsEvaluationTable.Columns.Add
            (
                new DataGridTextColumn
                {
                    Header = "Оцінка",
                    FontSize = Constants.FontSize,
                    Binding = new Binding("ValueString"),
                }
            );

            ClustersCountsEvaluationTable.Columns.Add
            (
                new DataGridCheckBoxColumn
                {
                    Header = "Висновок",
                    Binding = new Binding("Result"),
                }
            );
        }

        private void UpdateSignComboBoxesItems()
        {
            if (_context.CurrentData == null) return;

            _context.FirstSignComboBoxItemsSource.Clear();
            _context.SecondSignComboBoxItemsSource.Clear();

            FirstSignComboBox.SelectedIndex = Constants.NoElementComboBoxIndex;
            SecondSignComboBox.SelectedIndex = Constants.NoElementComboBoxIndex;

            _context.FirstSignComboBoxItemsSource = Enumerable.Range(0, _context.Dimensions!.Value)
                .Select(SignLabel.FromIndex)
                .ToList()!;

            _context.SecondSignComboBoxItemsSource = Enumerable.Range(0, _context.Dimensions!.Value)
                .Select(SignLabel.FromIndex)
                .ToList()!;

            FirstSignComboBox.ItemsSource = _context.FirstSignComboBoxItemsSource;
            SecondSignComboBox.ItemsSource = _context.SecondSignComboBoxItemsSource;
        }

        private void UpdateSignComboBoxesState()
        {
            if (_context == null)
            {
                SignComboBoxesStateTextBox.Background = Constants.DefaultTextBoxBrush;
                SignComboBoxesStateTextBox.Text = string.Empty;

                return;
            }

            if (_context.AnySignComboBoxInEmpty)
            {
                ResetSignComboBoxesStateTextBox();
            }
            else
            {
                var ok = !_context.SignComboBoxesHaveSameValue;

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

            var firstIndex = _context.FirstSelectedIndex;
            var secondIndex = _context.SecondSelectedIndex;

            TwoSignsPlot.Plot.XLabel(_context.FirstSelectedLabel);
            TwoSignsPlot.Plot.YLabel(_context.SecondSelectedLabel);

            for (var i = 0; i < _context.CurrentData!.Count; i++)
            {
                var coordinates = _context.CurrentData[i].Coordinates!;
                var color = _context.IsClusterized
                    ? _context.ClustersDatas?[_context.CurrentData[i].ClusterIndex!.Value].Color
                    : Constants.DefaultPlotColor;

                var circle = TwoSignsPlot.Plot.Add.Circle(coordinates.Values[firstIndex], 
                    coordinates.Values[secondIndex], PointsRadius);

                circle.FillColor = color!.Value;
                circle.LineColor = color!.Value;

                _context.PlotsContext.TwoSignsPlotPoints.Add(circle);
            }

            for (int i = 0; i < _context.ClustersDatas?.Count; i++)
            {
                var data = _context.ClustersDatas[i];
                var coordinates = data.Coordinates!;

                var circle = TwoSignsPlot.Plot.Add.Circle(coordinates.Values[firstIndex],
                    coordinates.Values[secondIndex], ClustersCentersRadius);

                circle.FillColor = data.CenterColor;
                circle.LineColor = Constants.ClustersCentersLineColor;
                circle.LineWidth = ClustersCentersLineWidth;

                _context.PlotsContext.TwoSignsPlotClustersCenters.Add(circle);
            }

            TwoSignsPlot.Plot.Axes.SquareUnits();
            TwoSignsPlot.Refresh();
        }

        private void UpdateParallelCoordinatesPlot()
        {
            ResetParallelCoordinatesPlot();

            var xs = Enumerable.Range(0, _context!.Dimensions!.Value).ToList();
            var isClusterized = _context.IsClusterized;

            foreach (var dataRow in _context.CurrentData!)
            {
                var color = _context.IsClusterized 
                    ? _context.ClustersDatas![dataRow.ClusterIndex!.Value].Color
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

                for (int i = 0; i < _context.Dimensions - 1; i++)
                {
                    var line = ParallelCoordinatesPlot.Plot.Add.Line(coordinates[i], coordinates[i + 1]);
                    line.Color = color;
                }
            }
        }

        private void UpdateClustersCountsEvaluationPlot()
        {
            if (_context?.ClustersCountsEvaluation == null)
                return;

            ResetClustersCountsEvaluationPlot();

            var xs = _context.ClustersCountsEvaluation.Select(evaluation => evaluation.ClustersCount).ToArray();
            var ys = _context.ClustersCountsEvaluation.Select(evaluation => evaluation.Value).ToArray();

            ClustersCountsEvaluationPlot.Plot.Add.Scatter(xs, ys, color: Constants.DefaultPlotColor);
            ClustersCountsEvaluationPlot.Refresh();
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

            if (_context.CurrentData == null)
            {
                ClustersCountMessageTextBox.Background = Constants.DefaultTextBoxBrush;
                ClustersCountMessageTextBox.Text = "Очікуються дані!";
                return;
            }

            if (!int.TryParse(input, out var clustersCount))
            {
                ClustersCountMessageTextBox.Background = Constants.NotOkBrush;
                ClustersCountMessageTextBox.Text = "Не вдалося зчитати значення!";

                _context.ClustersCount = null;

                return;
            }

            if (1 < clustersCount && clustersCount < _context.CurrentData.Count / 2)
            {
                ClustersCountMessageTextBox.Background = Constants.OkBrush;
                ClustersCountMessageTextBox.Text = "✓";
                _context.ClustersCount = clustersCount;
            }
            else
            {
                ClustersCountMessageTextBox.Background = Constants.NotOkBrush;
                ClustersCountMessageTextBox.Text = $"Значення має бути [2, {_context.CurrentData.Count / 2}]!";

                _context.ClustersCount = null;
            }
        }

        private void ComputeClustersCountEvaluation()
        {
            _context!.ClustersCountsEvaluation = new();

            for (int i = Constants.EvaluationMinClustersCount; i <= Constants.EvaluationMaxClustersCount; i++)
            {
                var index = Compute.CHIndex(_context!.CurrentData!, i);

                if (index == null) continue;

                _context!.ClustersCountsEvaluation.Add
                (
                    new ClustersCountEvaluationData
                    {
                        ClustersCount = i,
                        Value = index!.Value
                    }
                );
            }

            _context!.ClustersCountsEvaluation.MaxBy(evaluation => evaluation.Value)!.Result = true;

            UpdateClustersCountsEvaluationTable();
            UpdateClustersCountsEvaluationPlot();
        }
    }
}
