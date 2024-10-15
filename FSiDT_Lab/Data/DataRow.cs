using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSiDT_Lab
{
    public class DataRow
    {
        public Coordinates? Coordinates { get; init; }

        public int Index { get; init; }

        public int? ClusterIndex { get; set; }

        public bool IsAverageData { get; set; }

        public string IndexString => IsAverageData ? "Середнє" : Index.ToString();

        public string ClusterIndexString => IsAverageData ? string.Empty : ClusterIndex.ToString()!;

        public double this[int index]
        {
            get => Coordinates!.Values[index];
            set => Coordinates!.Values[index] = value;
        }
    }
}
