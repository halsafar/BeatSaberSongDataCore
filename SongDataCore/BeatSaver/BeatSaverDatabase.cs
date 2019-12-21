using System;
using UnityEngine.Networking;
using SongDataCore.Downloader;

namespace SongDataCore.BeatSaver
{
    public class BeatSaverDatabase : DatabaseDownloader, IDatabaseDownloadHandler
    {
        public const String BEAT_SAVER_DATA_DUMP_URL = "https://beatsaver.com/api/download/dump/maps";

        public BeatSaverDataFile Data = null;

        protected byte[] Buffer = new byte[64 * 1048576];

        /// <summary>
        /// Start downloading the BeatSaver database.
        /// </summary>
        public void Load()
        {
            StartCoroutine(DownloadDatabase(BEAT_SAVER_DATA_DUMP_URL, this));
        }

        /// <summary>
        /// Attempt to reduce memory usage.
        /// </summary>
        public void Unload()
        {
            Data = null;
        }

        /// <summary>
        /// Return the appropriate handler for this database.
        /// </summary>
        /// <param name="www"></param>
        /// <returns></returns>
        public CacheableDownloadHandler GetDownloadHandler(UnityWebRequest www)
        {
            var cacheHandler = new CacheableBeatSaverDownloadHandler(www, Buffer);
            www.SetCacheable(cacheHandler);
            return cacheHandler;
        }

        /// <summary>
        /// Acquire the results from the download handler.
        /// </summary>
        /// <param name="handler"></param>
        public void HandleDownloadResults(DownloadHandler handler)
        {
            Data = (handler as CacheableBeatSaverDownloadHandler).DataFile;
        }

        /// <summary>
        /// Do we have data.
        /// </summary>
        /// <returns></returns>
        public bool IsDataAvailable()
        {
            return Data != null && Data.Songs != null;
        }
    }
}
