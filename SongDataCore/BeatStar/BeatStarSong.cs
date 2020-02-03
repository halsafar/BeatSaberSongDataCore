using Newtonsoft.Json;
using System.Collections.Generic;

namespace SongDataCore.BeatStar
{
    public class BeatStarSong
    {
        public string key { get; set; }
        //public string song { get; set; }
        //public string mapper { get; set; }
        public List<BeatStarSongDifficultyStats> diffs { get; set; }

        public float bmp { get; set; }
        public int downloadCount { get; set; }
        public int upVotes { get; set; }
        public int downVotes { get; set; }
        public float heat { get; set; }
        public float rating { get; set; }

        [JsonIgnore]
        public Dictionary<BeatStarCharacteristics, Dictionary<string, BeatStarSongDifficultyStats>> characteristics { get; set; }
    }

    public class BeatStarSongDifficultyStats
    {
        public string diff { get; set; }
        public long scores { get; set; }
        public double star { get; set; }
        public double pp { get; set; }

        public int type { get; set; }
        public int len { get; set; }
        public int njs { get; set; }
        public int bmb { get; set; }
        public int nts { get; set; }
        public int obs{ get; set; }
    }

    public enum BeatStarCharacteristics
    {
        Unkown,
        Standard,
        OneSaber,
        NoArrows,
        Lightshow,
        Degree90,
        Degree360,
        Lawless,
    }
}
