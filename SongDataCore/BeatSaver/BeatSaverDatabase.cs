using System;
using UnityEngine.Networking;
using SongDataCore.Downloader;
using System.Collections;

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
        public override void Load()
        {
            _isDownloading = true;
            _cancelRequested = false;
            StartCoroutine(DownloadBeatSaberDatabase());
        }

        /// <summary>
        /// Attempt to reduce memory usage.
        /// </summary>
        public override void Unload()
        {
            StartCoroutine(WaitAndUnload());
        }

        /// <summary>
        /// Cancel and wait for downloads to abort then unload.
        /// </summary>
        /// <returns></returns>
        private IEnumerator WaitAndUnload()
        {
            yield return StartCoroutine(CancelDownload());

            Plugin.Log.Debug($"BeatSaber Total Memory - Before BeatSaver Unload: {GC.GetTotalMemory(false)}");
            Data = null;
            System.GC.Collect();
            Plugin.Log.Debug($"BeatSaber Total Memory - After  BeatSaver Unload: {GC.GetTotalMemory(false)}");
        }

        /// <summary>
        /// Coroutine to manage downloading
        /// </summary>
        /// <returns></returns>
        private IEnumerator DownloadBeatSaberDatabase()
        {
            Data = null;

            yield return StartCoroutine(DownloadDatabase(BEAT_SAVER_DATA_DUMP_URL, this));

            _isDownloading = false;
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

        /// <summary>
        /// This database is interruptable due to how long it can take to parse.
        /// </summary>
        public void CancelHandler(DownloadHandler handler)
        {
            (handler as CacheableBeatSaverDownloadHandler).Cancel();
        }
    }
}
