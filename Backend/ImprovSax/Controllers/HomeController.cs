using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using System.Threading.Tasks;
using ImprovSaxophone.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace ImprovSax.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        private IWebHostEnvironment _env;
        public HomeController(IWebHostEnvironment env)
        {
            _env = env;
        }

        private T LoadJson<T>(string path)
        {
            var contentRoot = _env.ContentRootPath;
            var file = System.IO.Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), path);;
            using (StreamReader r = System.IO.File.OpenText(file))
            {
                string json = r.ReadToEnd();
                T items = JsonSerializer.Deserialize<T>(json);
                return items;
            }
        }

        private string RandomNote(Random r1, Key transformedKey, List<string> prefNotes, Note lastNote = null)
        {
            List<string> localPrefNotes = new List<string>(prefNotes);
            string lastNoteValue = ""; string lastNoteOctave = ""; int lastNoteIndex = 1;
            if (lastNote != null)
            {
                lastNoteValue = lastNote.Value.Split('_')[0];
                lastNoteOctave = lastNote.Value.Split('_')[1];
                lastNoteIndex = Array.IndexOf(transformedKey.Notes, lastNoteValue);
                if (lastNoteIndex > 0)
                    localPrefNotes.Add((lastNoteIndex + (r1.Next(-1, 1))).ToString());
            }

            if (lastNoteOctave == "4")
                Console.WriteLine("hi");
            string octave = !string.IsNullOrEmpty(lastNoteOctave) ?
                                ((lastNoteIndex >= 5 && lastNoteOctave != "4") ? (Int32.Parse(lastNoteOctave) + r1.Next(2)).ToString() :
                                ((lastNoteIndex < 2 && lastNoteOctave != "2") ? (Int32.Parse(lastNoteOctave) + r1.Next(-1, 1)).ToString() : lastNoteOctave)) :
                                "2";



            int randNote = Int32.Parse(localPrefNotes[r1.Next(localPrefNotes.Count())]);
            if (randNote >= transformedKey.Notes.Count())
                randNote = transformedKey.Notes.Count() - 1;
            string name = string.Format("{0}_{1}", transformedKey.Notes[randNote], octave);
            return name;
        }



        private string ModePicker(string quality, string auxiliary)
        {
            string mode = "";
            string[] modes = new string[0];
            Random r = new Random();
            switch (quality)
            {
                case "major":
                    switch (auxiliary)
                    {
                        case "":
                            modes = new string[] { "ionian", "phygrian", "blues", "major pentatonic" };
                            mode = modes[r.Next(0, modes.Length)];
                            break;
                        case "6":
                            modes = new string[] { "ionian", "major pentatonic" };
                            mode = modes[r.Next(0, modes.Length)];
                            break;
                        case "7":
                            modes = new string[] { "ionian", "lydian", "harmonic minor", "melodic minor" };
                            mode = modes[r.Next(0, modes.Length)];
                            break;
                    }
                    break;
                case "minor":
                    switch (auxiliary)
                    {
                        case "":
                            modes = new string[] { "dorian", "phygrian", "aeolian", "harmonic minor", "melodic minor", "blues", "minor pentatonic" };
                            mode = modes[r.Next(0, modes.Length)];
                            break;
                        case "7":
                            modes = new string[] { "lydian", "aeolian", "blues", "minor pentatonic", "diminished" };
                            mode = modes[r.Next(0, modes.Length)];
                            break;
                        case "7b5":
                            modes = new string[] { "locrian", "diminished" };
                            mode = modes[r.Next(0, modes.Length)];
                            break;
                    }
                    break;
                case "dominant":
                    switch (auxiliary)
                    {
                        case "7":
                            modes = new string[] { "mixolydian", "blues", "major pentatonic" };
                            mode = modes[r.Next(0, modes.Length)];
                            break;
                        case "7b9":
                            mode = "diminished";
                            break;
                        case "9#11":
                            mode = "melodic minor";
                            break;
                    }
                    break;
                case "diminished":
                    mode = "diminished";
                    break;
                case "augmented":
                    mode = "whole-tone";
                    break;
            }
            return mode;
        }

        private Key TransformKey(Key chromatic, Key key, Mode mode)
        {
            Key transformedKey = new Key() { KeyName = string.Format("{0}_{1}", key.KeyName, mode.ModeName) };
            List<string> transformedNotes = new List<string>();
            for (int i = 0; i < key.Notes.Count(); i++)
            {
                string note = TransformNote(chromatic.Notes, key.Notes[i], mode.Transforms[0]);
                if (string.IsNullOrEmpty(note))
                    continue;
                transformedNotes.Add(note);
            }
            transformedKey.Notes = transformedNotes.ToArray();
            return transformedKey;
        }

        private string TransformNote(string[] chromatic, string note, int transform)
        {
            string newNote = "";
            if (transform == -10)
                return newNote;
            else
                newNote = chromatic[Array.IndexOf(chromatic, note) + transform];
            return newNote;
        }

        private Duration RandomDuration(Random r, int tsD, Duration lastDuration, double measureDuration, double measureLeft, List<Duration> recursivelyExcludedDurations = null)
        {
            List<Duration> excludedDurations = new List<Duration>();
            if (recursivelyExcludedDurations != null)
                excludedDurations = recursivelyExcludedDurations;
            // todo add lastDuration and measureDuration to algorithm
            List<Duration> durations = new List<Duration>() {
                new Duration() { StringDuration = string.Format("n_{0}_over_{1}", "1", (tsD*8).ToString()), DoubleDuration = ((measureDuration)/(tsD*8)) },
                new Duration() { StringDuration = string.Format("n_{0}_over_{1}", "1", (tsD*4).ToString()), DoubleDuration = ((measureDuration)/(tsD*4)) },
                new Duration() { StringDuration = string.Format("n_{0}_over_{1}", "1", (tsD*4).ToString()), DoubleDuration = ((measureDuration)/(tsD*4)) },
                new Duration() { StringDuration = string.Format("n_{0}_over_{1}", "1", (tsD*4).ToString()), DoubleDuration = ((measureDuration)/(tsD*4)) },
                new Duration() { StringDuration = string.Format("n_{0}_over_{1}", "1", (tsD*2).ToString()), DoubleDuration = ((measureDuration)/(tsD*2)) },
                new Duration() { StringDuration = string.Format("n_{0}_over_{1}", "1", (tsD*2).ToString()), DoubleDuration = ((measureDuration)/(tsD*2)) },
                new Duration() { StringDuration = string.Format("n_{0}_over_{1}", "1", (tsD*2).ToString()), DoubleDuration = ((measureDuration)/(tsD*2)) },
                new Duration() { StringDuration = string.Format("n_{0}_over_{1}", "1", (tsD*1).ToString()), DoubleDuration = ((measureDuration)/(tsD*1)) }
            };
            IEnumerable<Duration> durationList = durations.Where(x => excludedDurations.Find(y => y.DoubleDuration == x.DoubleDuration) == null);
            Duration duration = durationList.ElementAt(r.Next(0, durationList.Count()));
            if (duration.DoubleDuration > measureLeft)
            {
                //if (recursivelyExcludedDurations == null)
                //    recursivelyExcludedDurations = durationsHolder;
                excludedDurations.Add(duration);
                RandomDuration(r, tsD, lastDuration, measureDuration, measureLeft, excludedDurations);
            }
            return duration;
        }

        public List<Note> GetNotes(DateTime? start, string songName)
        {
            DateTime startTime = start ?? DateTime.UtcNow;
            if (string.IsNullOrEmpty(songName))
                songName = "a_train";
            Measure m = new Measure() { Start = start ?? DateTime.UtcNow };
            List<Note> notes = new List<Note>();
            Song song = LoadJson<Song>(string.Format("{0}{1}{2}", "Files/", songName, ".json"));
            Random r1 = new Random();
            foreach (Chord measure in song.Measures)
            {
                List<Note> note = Measure(startTime, song.Bpm, song.TsN, song.TsD, r1, measure.Duration, measure.Root, measure.Quality, measure.Auxiliary);
                foreach (Note n in note)
                {
                    notes.Add(n);
                }
            }
            //m.Notes = notes;
            return notes;
        }

        private List<Note> Measure(DateTime start, int bpm, int tsN, int tsD, Random r1, double measureFraction = 1.0, string root = "c", string quality = "major", string auxiliary = "")
        {
            double measureDuration = ((480 / bpm) * 1000) * measureFraction;
            double measureDurationLeft = measureDuration;
            int numberOfNotes = r1.Next(1, tsN * 3);

            List<Note> notes = new List<Note>();
            List<Key> keys = LoadJson<List<Key>>("Files/keys.json");
            int auxMod;
            if (Int32.TryParse(auxiliary, out auxMod))
                auxMod = Int32.Parse(auxiliary);

            List<string> prefNotes = new List<string>() { "0", "2", "4", (auxMod - 1).ToString() };
            Key chromatic = keys.Where(x => x.KeyName == "chromatic").FirstOrDefault();
            Key key = keys.Where(x => x.KeyName == root).FirstOrDefault();
            string modeName = ModePicker(quality, auxiliary);
            Mode mode = LoadJson<List<Mode>>("Files/modes.json").Where(x => x.ModeName == modeName).FirstOrDefault();
            Key transformedKey = TransformKey(chromatic, key, mode);
            while (measureDurationLeft > 0)
            {
                Duration lastNote = notes.Count() > 0 ? notes.Last().Duration : new Duration();
                Duration duration = RandomDuration(r1, tsD, lastNote, measureDuration, measureDurationLeft);
                notes.Add(new Note
                {
                    Duration = duration,
                    Value = RandomNote(r1, transformedKey, prefNotes, notes.Count() > 0 ? notes.Last() : null)
                });
                measureDurationLeft = measureDurationLeft - duration.DoubleDuration;
            }

            return notes;
        }
    }
}
