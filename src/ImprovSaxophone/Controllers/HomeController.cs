using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using ImprovSaxophone.Models;
using System.IO;
using Newtonsoft.Json;
using System.Text;
using Microsoft.Dnx.Runtime;

namespace ImprovSaxophone.Controllers
{
    public class HomeController : Controller
    {
        private readonly IApplicationEnvironment _appEnvironment;

        public HomeController(IApplicationEnvironment appEnvironment)
        {
            _appEnvironment = appEnvironment;
        }

        public IActionResult Index()
        {
            return View();
        }

        public string RandomNote(Random r1, Key transformedKey)
        {
            string[] notes = transformedKey.notes;
            string[] octaves = { "2", "3"};
            int randNote = r1.Next(notes.Count());
            int randOctave = r1.Next(octaves.Count());
            string name = string.Format("{0}_{1}", notes[randNote], octaves[randOctave]);
            return name;
        }

        public List<T> LoadJson<T>(string path)
        {
            using (StreamReader r = System.IO.File.OpenText(Path.Combine(_appEnvironment.ApplicationBasePath, path)))
            {
                string json = r.ReadToEnd();
                List<T> items = JsonConvert.DeserializeObject<List<T>>(json);
                return items;
            }
        }

        public string ModePicker(string quality, string auxiliary)
        {
            string mode = "";
            string[] modes = new string[0];
            Random r = new Random();
            switch(quality)
            {
                case "major":
                    switch (auxiliary)
                    {
                        case "":
                            modes = new string[] { "ionian", "phygrian", "blues", "major pentatonic" };
                            mode = modes[r.Next(0, modes.Length)];
                            break;
                        case "6":
                            modes = new string[] { "ionian", "major pentatonic", "minor pentatonic" };
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
                case "dominiant":
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

        public Key TransformKey(Key chromatic, Key key, Mode mode)
        {
            Key transformedKey = new Key() { key = string.Format("{0}_{1}", key.key, mode.mode) };
            List<string> transformedNotes = new List<string>();
            for (int i = 0; i < key.notes.Count(); i++)
            {
                string note = TransformNote(chromatic.notes, key.notes[i], mode.transforms[0]);
                if (string.IsNullOrEmpty(note))
                    continue;
                transformedNotes.Add(note);
            }
            transformedKey.notes = transformedNotes.ToArray();
            return transformedKey;
        }

        public string TransformNote(string[] chromatic, string note, int transform)
        {
            string newNote = "";
            if (transform == -10)
                return newNote;
            else
                newNote = chromatic[Array.IndexOf(chromatic, note) + transform];
            return newNote;
        }

        public Duration RandomDuration(Random r, int tsD, Duration lastDuration, double measureDuration, List<Duration> recursivelyExcludedDurations = null)
        {
            List<Duration> excludedDurations = new List<Duration>();
            if (recursivelyExcludedDurations != null)
                excludedDurations = recursivelyExcludedDurations;
            // todo add lastDuration and measureDuration to algorithm
            List<Duration> durations = new List<Duration>() {
                new Duration() { stringDuration = string.Format("{0}_over_{1}", "1", (tsD*4).ToString()), doubleDuration = ((measureDuration)/(tsD*4)) },
                new Duration() { stringDuration = string.Format("{0}_over_{1}", "1", (tsD*2).ToString()), doubleDuration = ((measureDuration)/(tsD*2)) },
                new Duration() { stringDuration = string.Format("{0}_over_{1}", "1", (tsD*1).ToString()), doubleDuration = ((measureDuration)/(tsD*0.5)) },
                new Duration() { stringDuration = string.Format("{0}_over_{1}", "1", (tsD*0.375).ToString()), doubleDuration = ((measureDuration)/(tsD*.375)) },
                new Duration() { stringDuration = string.Format("{0}_over_{1}", "1", (tsD*0.25).ToString()), doubleDuration = ((measureDuration)/(tsD*0.25)) },
            };
            Duration duration = durations.Where(x => !excludedDurations.Contains(x)).ElementAt(r.Next(0, durations.Count()));
            if (duration.doubleDuration < measureDuration)
            {
                //if (recursivelyExcludedDurations == null)
                //    recursivelyExcludedDurations = durationsHolder;
                excludedDurations.Add(duration);
                RandomDuration(r, tsD, lastDuration, measureDuration, excludedDurations);
            }
            return duration;
        }

        public JsonResult Measure(DateTime start, int bpm, int tsN, int tsD, double measureFraction = 1.0, string root = "c", string quality = "major", string auxiliary = "")
        {
            double measureDuration = ((480 / bpm) * 1000) * measureFraction;
            Random r1 = new Random();
            int numberOfNotes = r1.Next(1, tsN * 3);

            Measure measure = new Measure()
            {
                Start = start
            };
            
            List<Note> notes = new List<Note>();
            List<Key> keys = LoadJson<Key>("wwwroot/Assets/json/keys.json");
            Key chromatic = keys.Where(x => x.key == "chromatic").FirstOrDefault();
            Key key = keys.Where(x => x.key == root).FirstOrDefault();
            string modeName = ModePicker(quality, auxiliary);
            Mode mode = LoadJson<Mode>("wwwroot/Assets/json/modes.json").Where(x => x.mode == modeName).FirstOrDefault();
            Key transformedKey = TransformKey(chromatic, key, mode);
            while (measureDuration > 0)
            {
                Duration lastNote = notes.Count() > 0 ? notes.Last().Duration : null;
                Duration duration = RandomDuration(r1, tsD, lastNote, measureDuration);
                notes.Add(new Note
                {
                    Duration = duration,
                    Value = RandomNote(r1, transformedKey)
                });
                measureDuration = measureDuration - duration.doubleDuration;
            }
            measure.Notes = notes.ToArray();
            return Json(measure);
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View("~/Views/Shared/Error.cshtml");
        }
    }
}
