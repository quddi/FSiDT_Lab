using System.Windows.Media;

namespace FSiDT_Lab
{
    public static class Constants
    {
        public const int FontSize = 14;
        public const int NoElementComboBoxIndex = -1;

        public static readonly Color OkColor = Color.FromRgb(161, 255, 162);
        public static readonly Color NotOkColor = Color.FromRgb(255, 164, 161);
        public static readonly Color DefaultTextBoxColor = Color.FromRgb(200, 200, 200);
        public static readonly ScottPlot.Color DefaultPointsColor = new ScottPlot.Color(0, 0, 0);
        
        public static readonly Brush OkBrush = new SolidColorBrush(OkColor);
        public static readonly Brush NotOkBrush = new SolidColorBrush(NotOkColor);
        public static readonly Brush DefaultTextBoxBrush = new SolidColorBrush(DefaultTextBoxColor);
    }
}
