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

        }
    }
}
