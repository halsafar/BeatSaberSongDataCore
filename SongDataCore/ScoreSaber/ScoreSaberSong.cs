using System.Collections.Generic;

namespace SongDataCore.ScoreSaber
{
    public class ScoreSaberSong
    {
        public string song { get; set; }
        public string mapper { get; set; }
        public List<ScoreSaberSongDifficultyStats> diffs { get; set; }
    }

    public class ScoreSaberSongDifficultyStats
    {
        public string diff { get; set; }
        public long scores { get; set; }
        public double star { get; set; }
        public double pp { get; set; }
    }
}
