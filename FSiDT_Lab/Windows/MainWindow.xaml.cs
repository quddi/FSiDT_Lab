using System.Windows;

namespace FSiDT_Lab
{
    public partial class MainWindow : Window
    {
        private bool _isInitialized;
        private Context? _context;

        public double PointsRadius => PlotsPointsSizeSlider.Value * Constants.PointsSizeMultiplier;
        public double ClustersCentersRadius => PlotsCentersSizeSlider.Value * Constants.ClustersCentersSizeMultiplier;
        public float ClustersCentersLineWidth => (float)PlotsCentersSizeSlider.Value * Constants.ClustersCentersLineWidthMultiplier;

        public MainWindow()
        {
            InitializeComponent();

            _isInitialized = true;

            ResetAll();
        }
    }
}