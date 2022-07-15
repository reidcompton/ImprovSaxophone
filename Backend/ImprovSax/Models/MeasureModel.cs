using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ImprovSaxophone.Models
{
    public class Song
    {
        [JsonPropertyName("bpm")]
        public int Bpm { get; set; }
        [JsonPropertyName("tsN")]
        public int TsN { get; set; }
        [JsonPropertyName("tsD")]
        public int TsD { get; set; }
        [JsonPropertyName("intro")]
        public double Intro { get; set; }
        [JsonPropertyName("measures")]
        public Chord[] Measures { get; set; }
    }

    public class Chord
    {
        [JsonPropertyName("root")]
        public string Root { get; set; }
        [JsonPropertyName("quality")]
        public string Quality { get; set; }
        [JsonPropertyName("auxiliary")]
        public string Auxiliary { get; set; }
        [JsonPropertyName("duration")]
        public double Duration { get; set; }
    }

    public class Measure
    {
        [JsonPropertyName("start")]
        public DateTime Start { get; set; }
        [JsonPropertyName("notes")]
        public Note[] Notes { get; set; }
    }

    public class Note
    {
        [JsonPropertyName("value")]
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
        [JsonPropertyName("duration")]
        public Duration Duration { get; set; }

    }

    public class Duration
    {
        [JsonPropertyName("stringDuration")]
        public string StringDuration { get; set; }
        [JsonPropertyName("doubleDuration")]
        public double DoubleDuration { get; set; }
    }

    public class Key
    {
        [JsonPropertyName("key")]
        public string KeyName { get; set; }
        [JsonPropertyName("notes")]
        public string[] Notes { get; set; }
    }

    public class Mode
    {
        [JsonPropertyName("mode")]
        public string ModeName { get; set; }
        [JsonPropertyName("transforms")]
        public int[] Transforms { get; set; }
    }
}