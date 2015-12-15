using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImprovSaxophone.Models
{
    public class Song
    {
        public int bpm { get; set; }
        public int tsN { get; set; }
        public int tsD { get; set; }
        public double intro { get; set; }
        public Chord[] measures { get; set; }
    }

    public class Chord
    {
        public string Root { get; set; }
        public string Quality { get; set; }
        public string Auxiliary { get; set; }
        public double Duration { get; set; }
    }

    public class Measure
    {
        public DateTime Start { get; set; }
        public Note[] Notes { get; set; }
    }

    public class Note
    {
        public string Value { get; set; }
        public string SoloValue
        {
            get
            {
                return this.Value.Split('_')[0];
            }
        }
        public string SoloOctave
        {
            get
            {
                return this.Value.Split('_')[1];
            }
        }
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
