using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

namespace SongDataCore.BeatSaver
{
    public class BeatSaverSong
    {
        public BeatSaverSongMetaData metadata { get; set; }
        public BeatSaverSongStats stats { get; set; }

        public string description { get; set; }
        public string deletedAt { get; set; }
        public string _id { get; set; }
        public string key { get; set; }
        public string name { get; set; }
        public BeatSaverSongUploader uploader { get; set; }
        public string hash { get; set; }
        public string uploaded { get; set; }
        public string downloadURL { get; set; }
        public string coverURL { get; set; }
    }

    public class BeatSaverSongMetaData
    {
        public string songName { get; set; }
        public string songSubName { get; set; }
        public string songAuthorName { get; set; }
        public string levelAuthorName { get; set; }
        public string bpm { get; set; }
        public Dictionary<string, bool> difficulties { get; set; }

        // read the list, deserialize process converts to map
        [JsonProperty("characteristics")]
        public List<BeatSaverSongCharacteristics> characteristicsList;

        // manually mapped during deserialization, JsonConverter as optmized as could be added 2x to the runtime (likely due to mass object creation)
        [JsonIgnore]
        public Dictionary<string, BeatSaverSongCharacteristics> characteristics { get; set; }
    }

    public class BeatSaverSongCharacteristics
    {
        public string name { get; set; }
        public Dictionary<string, BeatSaverSongCharacteristicData> difficulties { get; set; }
    }

    public class BeatSaverSongCharacteristicData
    {
        public string duration { get; set; }
        public string length { get; set; }
        public string bombs { get; set; }
        public string notes { get; set; }
        public string obstacles { get; set; }
        public string njs { get; set; }
        public string njsOffset { get; set; }
    }

    public class BeatSaverSongStats
    {
        public long downloads { get; set; }
        public long plays { get; set; }
        public long downVotes { get; set; }
        public long upVotes { get; set; }
        public double heat { get; set; }
        public double rating { get; set; }
    }

    public class BeatSaverSongUploader
    {
        public string _id { get; set; }
        public string username { get; set; }
    }
}
