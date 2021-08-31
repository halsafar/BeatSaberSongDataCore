using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;

namespace SongDataCore.BeatStar
{
    public class BeatStarDataFile
    {
        public Dictionary<string, BeatStarSong> Songs = null;

        public BeatStarDataFile(byte[] data)
        {
            Plugin.Log.Info("Constructing BeatStarDataFile");

            //System.Threading.Thread.Sleep(2000);
            //Plugin.Log.Debug($"BeatSaber Total Memory - Before BeatStar Load: {GC.GetTotalMemory(true)}");

            Stopwatch timer = new Stopwatch();
            timer.Start();

            try
            {
                string result;
                using (var compressedStream = new MemoryStream(data))
                using (var zipStream = new GZipStream(compressedStream, CompressionMode.Decompress))
                using (var resultStream = new MemoryStream())
                {
                    zipStream.CopyTo(resultStream);
                    result = System.Text.Encoding.UTF8.GetString(resultStream.ToArray());
                }

                var tmpSongs = JsonConvert.DeserializeObject<Dictionary<string, BeatStarSong>>(result);
                Songs = new Dictionary<string, BeatStarSong>(tmpSongs, StringComparer.OrdinalIgnoreCase);
                tmpSongs = null;

                HashSet<string> userSongKeys = SongCore.Loader.CustomLevels.Select(x => x.Value.levelID.Remove(0, 13)).ToHashSet();

                List<string> removeSongs = new List<string>();
                foreach (var pair in Songs)
                {
                    bool userHasSong = userSongKeys.Contains(pair.Key);
                    if (!userHasSong)
                    {
                        removeSongs.Add(pair.Key);
                        continue;
                    }

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

                Plugin.Log.Debug($"Removing {removeSongs.Count} songs from BeatStar database.");
                foreach (var key in removeSongs)
                {
                    Songs.Remove(key);
                }

                timer.Stop();
                Plugin.Log.Debug($"Processing BeatStar data took {timer.ElapsedMilliseconds}ms");

                //System.GC.Collect();
                //Plugin.Log.Debug($"BeatSaber Total Memory - After BeatStar Load: {GC.GetTotalMemory(true)}");
            }
            catch (Exception e)
            {
                Plugin.Log.Error($"BeatStar data corrupted, sometimes JSON dump returns from BeatSaver corrupted: {e.Message}");
                return;
            }
        }
    }
}
