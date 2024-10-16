﻿using System.Windows;

namespace FSiDT_Lab
{
    public partial class MainWindow : Window
    {
        private void ResetAll()
        {
            _context = null!;

            ResetDataTable();
            ResetClustersCentersTable();
            ResetClustersCountsEvaluationTable();

            ResetSignComboBoxes();
            ResetSignComboBoxesStateTextBox();
            ResetClustersCount();

            ResetTwoSignsPlot();
            ResetParallelCoordinatesPlot();
            ResetClustersCountsEvaluationPlot();
        }

        private void ResetDataTable()
        {
            DataTable.Columns.Clear();
            DataTable.Items.Clear();
        }

        private void ResetClustersCentersTable()
        {
            ClustersCentersTable.Columns.Clear();
            ClustersCentersTable.Items.Clear();
        }

        private void ResetClustersCountsEvaluationTable()
        {
            ClustersCountsEvaluationTable.Columns.Clear();
            ClustersCountsEvaluationTable.Items.Clear();
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

        private void ResetTwoSignsPlot()
        {
            _context?.PlotsContext.TwoSignsPlotPoints.Clear();
            _context?.PlotsContext.TwoSignsPlotClustersCenters.Clear();

            TwoSignsPlot.Reset();

            TwoSignsPlot.Plot.Title("Точковий графік двох ознак");

            TwoSignsPlot.Refresh();
        }

        private void ResetParallelCoordinatesPlot()
        {
            ParallelCoordinatesPlot.Reset();

            ParallelCoordinatesPlot.Plot.Title("Графік паралельних координат");

            ParallelCoordinatesPlot.Refresh();
        }

        private void ResetClustersCountsEvaluationPlot()
        {
            ClustersCountsEvaluationPlot.Reset();

            ClustersCountsEvaluationPlot.Plot.Title("Оцінка кількості кластерів");

            ClustersCountsEvaluationPlot.Refresh();
        }

        private void ResetClustersCount()
        {
            ClustersCountInputTextBox.Text = string.Empty;
        }
    }
}
