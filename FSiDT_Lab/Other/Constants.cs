using System.Windows.Media;

namespace FSiDT_Lab
{
    public static class Constants
    {
        public const int FontSize = 14;
        public const int NoElementComboBoxIndex = -1;
        public const int FirstElementIndex = 0;
        public const int MaxClasterizationIterations = 50;
        public const int MaxColorComponent= 255;
        public const double MaxRelativeChance = 1d;
        public const double DoubleComparsionTolerance = 0.00001d;
        public const double PointsSizeMultiplier = 1.0;
        public const double ClustersCentersSizeMultiplier = 1.8;
        public const float ClustersCentersLineWidthMultiplier = 1.4f;

        public static readonly Color OkColor = Color.FromRgb(161, 255, 162);
        public static readonly Color NotOkColor = Color.FromRgb(255, 164, 161);
        public static readonly Color DefaultTextBoxColor = Color.FromRgb(200, 200, 200);
        public static readonly Color InputTextBoxColor = Color.FromRgb(250, 250, 250);
        public static readonly ScottPlot.Color DefaultPlotColor = new ScottPlot.Color(0, 0, 0);
        public static readonly ScottPlot.Color ClustersCentersLineColor = new ScottPlot.Color(0, 0, 0);
        
        public static readonly Brush OkBrush = new SolidColorBrush(OkColor);
        public static readonly Brush NotOkBrush = new SolidColorBrush(NotOkColor);
        public static readonly Brush DefaultTextBoxBrush = new SolidColorBrush(DefaultTextBoxColor);
        public static readonly Brush InputTextBoxBrush = new SolidColorBrush(InputTextBoxColor);
    }
}
