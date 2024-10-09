using System.Windows;

namespace FSiDT_Lab
{

    /*
     - таблиця
     - графіки (мінімум 2?)
    
     */
    public partial class MainWindow : Window
    {
        private bool _isInitialized;
        private Context _context;

        public MainWindow()
        {
            InitializeComponent();

            _isInitialized = true;

            ResetAll();
        }
    }
}