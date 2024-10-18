using System.Net.Http.Headers;

namespace FSiDT_Lab
{
    public class ClusterData
    {
        private ScottPlot.Color _color;

        public int Index { get; set; }

        public ScottPlot.Color Color
        {
            get => _color;
            set
            {
                _color = value;

                CenterColor = new ScottPlot.Color
                (
                    MathF.Max(0, (1f * _color.R - Constants.CenterPointColorDifference) / Constants.MaxColorComponentValue),
                    MathF.Max(0, (1f * _color.G - Constants.CenterPointColorDifference) / Constants.MaxColorComponentValue),
                    MathF.Max(0, (1f * _color.B - Constants.CenterPointColorDifference) / Constants.MaxColorComponentValue)
                );
            }
        }

        public ScottPlot.Color CenterColor { get; private set; }

        public Coordinates? Coordinates { get; set; }

        public Coordinates? AverageCoordinates { get; set; }

        public double this[int index]
        {
            get => Coordinates!.Values[index];
            set => Coordinates!.Values[index] = value;
        }

        public override string ToString()
        {
            return $"Cluster [{Index}]: {Coordinates}";
        }
    }
}
