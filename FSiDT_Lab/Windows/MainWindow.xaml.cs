using System.Windows;

namespace FSiDT_Lab
{

    /*
     - таблиця
     - графіки (мінімум 2?)
    
     */
    public partial class MainWindow : Window
    {
        private List<DataRow>? _currentData;
        private int _firstSignIndex;
        private int _secondSignIndex;

        private int? Dimensions => _currentData?.FirstOrDefault()?.Values?.Count;

        public MainWindow()
        {
            InitializeComponent();

            ResetAll();
        }
    }
}