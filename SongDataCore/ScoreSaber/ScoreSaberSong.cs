using System.Collections.Generic;

namespace SongDataCore.ScoreSaber
{
    public class ScoreSaberSong
    {
        public string key { get; set; }
        public string song { get; set; }
        public string mapper { get; set; }
        public List<ScoreSaberSongDifficultyStats> diffs { get; set; }

        public float bmp { get; set; }
        public int downloadCount { get; set; }
        public int upVotes { get; set; }
        public int downVotes { get; set; }
        public float heat { get; set; }
        public float rating { get; set; }
    }

    public class ScoreSaberSongDifficultyStats
    {
        public string diff { get; set; }
        public long scores { get; set; }
        public double star { get; set; }
        public double pp { get; set; }
    }
}
