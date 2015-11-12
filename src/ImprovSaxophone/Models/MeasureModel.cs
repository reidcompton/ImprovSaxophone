using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImprovSaxophone.Models
{
    public class Measure
    {
        public DateTime Start { get; set; }
        public Note[] Notes { get; set; }
    }

    public class Note
    {
        public string Value { get; set; }
        public double Duration { get; set; }

    }
}
