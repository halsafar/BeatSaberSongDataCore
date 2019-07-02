using System;
using UnityEngine.Networking;
using SongDataCore.Downloader;

namespace SongDataCore.BeatSaver
{
    public class BeatSaverDatabase : DatabaseDownloader, IDatabaseDownloadHandler
    {
        public const String BEAT_SAVER_DATA_DUMP_URL = "https://beatsaver.com/api/download/dump/maps";

        public BeatSaverDataFile Data = null;
        
        /// <summary>
        /// Start downloading the BeatSaver database.
        /// </summary>
        public void Start()
        {
            StartCoroutine(DownloadDatabase(BEAT_SAVER_DATA_DUMP_URL, this));
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
