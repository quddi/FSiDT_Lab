namespace FSiDT_Lab
{
    public class ClusterData
    {
        public int Index { get; set; }

        public ScottPlot.Color Color { get; set; }

        public Coordinates Coordinates { get; set; }

        public double this[int index]
        {
            get => Coordinates!.Values[index];
            set => Coordinates!.Values[index] = value;
        }
    }
}
