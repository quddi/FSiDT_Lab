namespace FSiDT_Lab
{
    public class Coordinates
    {
        public List<double> Values { get; set; }

        public static implicit operator List<double>(Coordinates coordinates)
        {
            return coordinates.Values;
        }

        public Coordinates()
        {
            Values = new();
        }

        public Coordinates(int dimensions)
        {
            Values = Enumerable.Repeat(0d, dimensions).ToList();
        }

        public Coordinates(List<double> values)
        {
            Values = values;
        }
    }
}
