using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using SongDataCore.Internal;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace SongDataCore.BeatSaver
{
    public class BeatSaverDataFile
    {
        public Dictionary<String, BeatSaverSong> Songs = null;

        public bool CancelParsing = false;

        public void Parse(byte[] data)
        {
            Plugin.Log.Info("Constructing BeatSaver Database.");

            Stopwatch timer = new Stopwatch();
            timer.Start();

            try
            {
                using (var stream = new MemoryStream(data))
                {
                    using (var streamReader = new StreamReader(stream, Encoding.UTF8))
                    {
                        List<BeatSaverSong> songs = new List<BeatSaverSong>();
                        IEnumerable<BeatSaverSong> songsGenerator = JsonExtensions.DeserializeValues<BeatSaverSong>(streamReader);
                        foreach (var song in songsGenerator)
                        {
                            songs.Add(song);

                            if (CancelParsing)
                            {
                                Plugin.Log.Debug("BeatSaver parsing cancelled...");
                                return;
                            }
                        }

                        // manually map charactertistics from list to dictionary for plugins to do quick lookups
                        // doing this manually vs using a JsonConverter cut the parse time in half.
                        foreach (BeatSaverSong song in songs)
                        {
                            song.metadata.characteristics = new Dictionary<string, BeatSaverSongCharacteristics>();
                            song.metadata.characteristicsList.ForEach(x => song.metadata.characteristics.Add(x.name, x));
                        }

                        Songs = songs.ToDictionary(x => x.hash, x => x, StringComparer.OrdinalIgnoreCase);
                    }
                }
            }
            catch (Exception e)
            {
                Plugin.Log.Error($"BeatSaver data corrupted, sometimes JSON dump returns from BeatSaver corrupted: {e.Message}");
                return;
            }
            timer.Stop();

            Plugin.Log.Debug($"Processing BeatSaver data took {timer.ElapsedMilliseconds}ms");

            return;
        }
    }
}
