
using System.Reflection.Metadata;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace FSiDT_Lab
{
    public partial class MainWindow : Window
    {
        private void UpdateDataTable()
        {
            if (_currentData == null)
                return;

            var dimensions = _currentData.First().Values.Count;

            DataTable.Columns.Clear();
            
            SetColumns(dimensions);

            foreach (var row in _currentData)
            {
                DataTable.Items.Add(row);
            }
        }

        private void SetColumns(int dimensions)
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

            for (int i = 0; i < dimensions; i++)
            {
                var column = new DataGridTextColumn()
                {
                    Header = i.ToString(),
                    FontSize = Constants.FontSize,
                    Binding = new Binding($"Values[{i}]"),
                    Width = 100
                };
                DataTable.Columns.Add(column);
            }
        }
    }
}
