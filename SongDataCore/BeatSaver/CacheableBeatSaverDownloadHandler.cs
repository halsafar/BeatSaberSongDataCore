using SongDataCore.Downloader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.Networking;

namespace SongDataCore.BeatSaver
{
    /// <summary>
	/// Cacheable download handler for beat savers dump.
	/// </summary>
	public class CacheableBeatSaverDownloadHandler : CacheableDownloadHandler
    {
        BeatSaverDataFile _dataFile;

        public CacheableBeatSaverDownloadHandler(UnityWebRequest www, byte[] preallocateBuffer)
            : base(www, preallocateBuffer)
        {
        }

        /// <summary>
        /// The first time we access this should be after we have confirmed it has downloaded.
        /// This will return null until we have data.
        /// </summary>
        public BeatSaverDataFile DataFile
        {
            get
            {
                if (_dataFile == null)
                {
                    _dataFile = new BeatSaverDataFile(GetData());
                }
                return _dataFile;
            }
        }
    }
}
