using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace SongDataCore.ScoreSaber
{
    public class ScoreSaberDataFile
    {
        public Dictionary<string, ScoreSaberSong> Songs = null;

        public ScoreSaberDataFile(byte[] data)
        {
            Plugin.Log.Info("Constructing ScoreSaberDataFile");

            Stopwatch timer = new Stopwatch();
            timer.Start();

            string result = System.Text.Encoding.UTF8.GetString(data);

            var tmpSongs = JsonConvert.DeserializeObject<Dictionary<string, ScoreSaberSong>>(result);
            Songs = new Dictionary<string, ScoreSaberSong>(tmpSongs, StringComparer.OrdinalIgnoreCase);

            timer.Stop();
            Plugin.Log.Debug($"Processing ScoreSaber data took {timer.ElapsedMilliseconds}ms");
        }
    }
}
