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
        private int? _clustersCount;

        private bool _isInitialized;

        private int? Dimensions => _currentData?.FirstOrDefault()?.Values?.Count;

        public MainWindow()
        {
            InitializeComponent();

            _isInitialized = true;

            ResetAll();
        }
    }
}