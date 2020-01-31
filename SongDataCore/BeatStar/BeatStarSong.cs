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
    }

    public class BeatStarSongDifficultyStats
    {
        public string diff { get; set; }
        public long scores { get; set; }
        public double star { get; set; }
        public double pp { get; set; }

        public int length { get; set; }
        public int njs { get; set; }
        public int bombs { get; set; }
        public int notes { get; set; }
        public int obstacles { get; set; }
    }
}
