using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BabyNI.Data.Model
{
    public class HourlyAgg
    {
        public DateTime Time { get; set; }

        public double MaxRxLevel { get; set; }
    }
}
