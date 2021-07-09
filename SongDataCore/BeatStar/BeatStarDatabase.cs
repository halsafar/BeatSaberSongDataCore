using SongDataCore.Downloader;
using System;
using System.Collections;
using UnityEngine.Networking;

namespace SongDataCore.BeatStar
{
    public class BeatStarDatabase : DatabaseDownloader, IDatabaseDownloadHandler
    {
        public const String SCRAPED_SCORE_SABER_ALL_JSON_URL = "https://cdn.wes.cloud/beatstar/bssb/v2-all.json.gz";

        public BeatStarDataFile Data = null;

        protected byte[] Buffer = new byte[16 * 1048576];

        CacheableBeatStarDownloaderHandler _lastCacheHandler;

        /// <summary>
        /// Start downloading the BeatSaver database.
        /// </summary>
        public override void Load()
        {
            StartCoroutine(DownloadBeatStarDatabases());            
        }

        /// <summary>
        /// Attempt to reduce memory usage.
        /// </summary>
        public override void Unload()
        {
            StartCoroutine(WaitAndUnload());
        }

        /// <summary>
        /// Cancel and Wait for any existing operations to complete then clean up.
        /// </summary>
        /// <returns></returns>
        private IEnumerator WaitAndUnload()
        {
            yield return StartCoroutine(CancelDownload());

            if (Data != null)
            {
                //Plugin.Log.Debug($"BeatSaber Total Memory - Before BeatStar Unload: {GC.GetTotalMemory(false)}");
                Data = null;
                System.GC.Collect();
                //Plugin.Log.Debug($"BeatSaber Total Memory - After  BeatStar Unload: {GC.GetTotalMemory(false)}");
            }
            else
            {
                Plugin.Log.Debug("BeatStar Database not loaded...");
            }
        }

        /// <summary>
        /// Helper to download both databases.  They will be stacked together, ranked is more updated.
        /// </summary>
        /// <returns></returns>
        private IEnumerator DownloadBeatStarDatabases()
        {
            Data = null;
            _isDownloading = true;
            _cancelRequested = false;

            yield return DownloadDatabase(SCRAPED_SCORE_SABER_ALL_JSON_URL, this);

            _isDownloading = false;
        }

        /// <summary>
        /// Return the appropriate handler for this database.
        /// </summary>
        /// <param name="www"></param>
        /// <returns></returns>
        public CacheableDownloadHandler GetDownloadHandler(UnityWebRequest www)
        {
            if (!this._firstSuccess || _lastCacheHandler == null)
            {
                _lastCacheHandler = new CacheableBeatStarDownloaderHandler(www, Buffer);                
            }

            www.SetCacheable(_lastCacheHandler);
            return _lastCacheHandler;
        }

        /// <summary>
        /// Acquire the results from the download handler.
        /// </summary>
        /// <param name="handler"></param>
        public void HandleDownloadResults(DownloadHandler handler)
        {
            if (Data == null)
            {
                Data = (handler as CacheableBeatStarDownloaderHandler).DataFile;
            }

            _firstSuccess = true;
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
        /// No need to interrupt BeatStar yet, it parses often faster than we can even interrupt it.
        /// </summary>
        public void CancelHandler(DownloadHandler handler)
        {
            return;
        }
    }
}
