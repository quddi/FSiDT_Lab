
using System.Reflection.Metadata;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace FSiDT_Lab
{
    public partial class MainWindow : Window
    {
        private List<string> _firstSignComboBoxItemsSource = new();
        private List<string> _secondSignComboBoxItemsSource = new();

        private void UpdateDataTable()
        {
            if (_currentData == null)
                return;
            
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
                var column = new DataGridTextColumn()
                {
                    Header = SignLabel.FromIndex(i),
                    FontSize = Constants.FontSize,
                    Binding = new Binding($"Values[{i}]"),
                    Width = 100
                };
                DataTable.Columns.Add(column);
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
            var anyIsEmpty = FirstSignComboBox.SelectedIndex == Constants.NoElementComboBoxIndex ||
                SecondSignComboBox.SelectedIndex == Constants.NoElementComboBoxIndex;

            if (anyIsEmpty)
            {
                ResetSignComboBoxesStateTextBox();
            }
            else
            {
                var ok = FirstSignComboBox.SelectedIndex != SecondSignComboBox.SelectedIndex;

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

        }
    }
}
