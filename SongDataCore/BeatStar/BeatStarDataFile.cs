using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace SongDataCore.BeatStar
{
    public class BeatStarDataFile
    {
        public Dictionary<string, BeatStarSong> Songs = null;

        public BeatStarDataFile(byte[] data)
        {
            Plugin.Log.Info("Constructing BeatStarDataFile");

            Stopwatch timer = new Stopwatch();
            timer.Start();

            try
            {
                string result = System.Text.Encoding.UTF8.GetString(data);

                var tmpSongs = JsonConvert.DeserializeObject<Dictionary<string, BeatStarSong>>(result);
                Songs = new Dictionary<string, BeatStarSong>(tmpSongs, StringComparer.OrdinalIgnoreCase);

                foreach (var pair in Songs)
                {
                    pair.Value.characteristics = new Dictionary<BeatStarCharacteristics, Dictionary<string, BeatStarSongDifficultyStats>>();
                    foreach (var diff in pair.Value.diffs)
                    {
                        var characteristic = (BeatStarCharacteristics)diff.type;
                        if (!pair.Value.characteristics.ContainsKey(characteristic))
                        {
                            pair.Value.characteristics.Add(characteristic, new Dictionary<string, BeatStarSongDifficultyStats>());
                        }

                        // TODO - REMOVE when the scrape isnt duplicating diffs.
                        if (pair.Value.characteristics[characteristic].ContainsKey(diff.diff))
                        {
                            continue;
                        }

                        //Plugin.Log.Info($"Adding {characteristic}, {diff.diff}, {diff}");
                        pair.Value.characteristics[characteristic].Add(diff.diff, diff);
                    }
                }

                timer.Stop();
                Plugin.Log.Debug($"Processing BeatStar data took {timer.ElapsedMilliseconds}ms");
            }
            catch (Exception e)
            {
                Plugin.Log.Error($"BeatStar data corrupted, sometimes JSON dump returns from BeatSaver corrupted: {e.Message}");
                return;
            }
        }
    }
}
