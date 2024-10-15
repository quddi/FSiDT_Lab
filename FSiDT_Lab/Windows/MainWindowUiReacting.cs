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
            ComputeClustersCountEvaluation();
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

        private void Clusterize()
        {
            if (_context == null || _context.CurrentData == null || _context.ClustersCount == null)
                return;

            _context.ClustersDatas = Compute.KMeansClusterization(_context.CurrentData, _context.ClustersCount!.Value);

            if (_context.SignComboBoxesOk) 
                UpdateTwoSignsPlot();

            UpdateParallelCoordinatesPlot();
            UpdateDataTable();
            UpdateClustersCentersTable();
        }

        private void PlotsElementsSizeSliderValueChangedHandler(object _, RoutedPropertyChangedEventArgs<double> __)
        {
            if (!_isInitialized)
                return;

            PlotsElementsSizeTextBox.Text = PlotsElementsSizeSlider.Value.ToString("#.##");

            _context?.PlotsContext.SetPointsRadius(PointsRadius);
            _context?.PlotsContext.SetClustersCenters(ClustersCentersRadius, ClustersCentersLineWidth);

            TwoSignsPlot.Refresh();
            ParallelCoordinatesPlot.Refresh();
        }
    }
}
