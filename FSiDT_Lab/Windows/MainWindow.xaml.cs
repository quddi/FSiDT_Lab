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

        public double PointsRadius => PlotsElementsSizeSlider.Value * Constants.PointsSizeMultiplier;
        public double ClustersCentersRadius => PlotsElementsSizeSlider.Value * Constants.ClustersCentersSizeMultiplier;
        public float ClustersCentersLineWidth => (float)PlotsElementsSizeSlider.Value * Constants.ClustersCentersLineWidthMultiplier;

        public MainWindow()
        {
            InitializeComponent();

            _isInitialized = true;

            ResetAll();
        }
    }
}