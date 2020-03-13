using SongDataCore.Downloader;
using UnityEngine.Networking;

namespace SongDataCore.BeatStar
{
    /// <summary>
	/// Cacheable download handler for score saber v2 json.
	/// </summary>
	public class CacheableBeatStarDownloaderHandler : CacheableDownloadHandler
    {
        BeatStarDataFile _dataFile;

        public CacheableBeatStarDownloaderHandler(UnityWebRequest www, byte[] preallocateBuffer)
            : base(www, preallocateBuffer)
        {
        }

        /// <summary>
        /// Returns the downloaded score saber data file or null.
        /// </summary>
        public BeatStarDataFile DataFile
        {           
            get
            {
                if (_dataFile == null)
                {
                    _dataFile = new BeatStarDataFile(GetData());
                }
                return _dataFile;
            }
        }
    }
}
