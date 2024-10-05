using System.Windows;

namespace FSiDT_Lab
{
    public partial class MainWindow : Window
    {
        private void ResetAll()
        {
            ResetDataTable();
            ResetSignComboBoxes();
            ResetSignComboBoxesStateTextBox();
        }

        private void ResetDataTable()
        {
            DataTable.Columns.Clear();
            DataTable.Items.Clear();
        }

        private void ResetSignComboBoxes()
        {
            FirstSignComboBox.SelectedIndex = Constants.NoElementComboBoxIndex;
            SecondSignComboBox.SelectedIndex = Constants.NoElementComboBoxIndex;
        }

        private void ResetSignComboBoxesStateTextBox()
        {
            SignComboBoxesStateTextBox.Text = string.Empty;
            SignComboBoxesStateTextBox.Background = Constants.DefaultTextBoxBrush;
        }
    }
}
