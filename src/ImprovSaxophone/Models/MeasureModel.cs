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
        public Duration Duration { get; set; }

    }

    public class Duration
    {
        public string stringDuration { get; set; }
        public double doubleDuration { get; set; }
    }
    
    public class Key
    {
        public string key { get; set; }
        public string[] notes { get; set; }
    }

    public class Mode
    {
        public string mode { get; set; }
        public int[] transforms { get; set; }
    }
}
