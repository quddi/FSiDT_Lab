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
        public List<double> Values { get; init; }

        public int Index { get; init; }

        public double this[int index]
        {
            get => Values[index];
            set => Values[index] = value;
        }
    }
}
