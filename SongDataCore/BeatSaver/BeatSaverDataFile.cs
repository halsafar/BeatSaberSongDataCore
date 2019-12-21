using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;


namespace SongDataCore.BeatSaver
{
    public class BeatSaverDataFile
    {
        public Dictionary<String, BeatSaverSong> Songs = null;

        public BeatSaverDataFile(byte[] data)
        {
            Plugin.Log.Info("Constructing BeatSaver Database.");

            Stopwatch timer = new Stopwatch();
            timer.Start();

            try
            {
                string result = System.Text.Encoding.UTF8.GetString(data);

                List<BeatSaverSong> songs = JsonConvert.DeserializeObject<List<BeatSaverSong>>(result);
                Songs = songs.ToDictionary(x => x.hash, x => x, StringComparer.OrdinalIgnoreCase);
            }
            catch (Exception e)
            {
                Plugin.Log.Error($"BeatSaver data corrupted, sometimes JSON dump returns from BeatSaver corrupted: {e.Message}");
            }
            timer.Stop();

            Plugin.Log.Debug($"Processing BeatSaver data took {timer.ElapsedMilliseconds}ms");
        }
    }
}
