using SongDataCore.Downloader;
using UnityEngine.Networking;

namespace SongDataCore.ScoreSaber
{
    /// <summary>
	/// Cacheable download handler for score saber v2 json.
	/// </summary>
	public class CacheableScoreSaberDownloaderHandler : CacheableDownloadHandler
    {
        ScoreSaberDataFile _dataFile;

        public CacheableScoreSaberDownloaderHandler(UnityWebRequest www, byte[] preallocateBuffer)
            : base(www, preallocateBuffer)
        {
        }

        /// <summary>
        /// Returns the downloaded score saber data file or null.
        /// </summary>
        public ScoreSaberDataFile DataFile
        {
            get
            {
                if (_dataFile == null)
                {
                    _dataFile = new ScoreSaberDataFile(GetData());
                }
                return _dataFile;
            }
        }
    }
}
