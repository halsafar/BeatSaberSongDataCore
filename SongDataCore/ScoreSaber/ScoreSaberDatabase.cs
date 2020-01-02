using SongDataCore.Downloader;
using System;
using System.Collections;
using UnityEngine.Networking;

namespace SongDataCore.ScoreSaber
{
    public class ScoreSaberDatabase : DatabaseDownloader, IDatabaseDownloadHandler
    {
        public const String SCRAPED_SCORE_SABER_ALL_JSON_URL = "https://cdn.wes.cloud/beatstar/bssb/v2-all.json";
        public const String SCRAPED_SCORE_SABER_RANKED_JSON_URL = "https://cdn.wes.cloud/beatstar/bssb/v2-ranked.json";

        public ScoreSaberDataFile Data = null;

        protected byte[] Buffer = new byte[2 * 1048576];

        /// <summary>
        /// Start downloading the BeatSaver database.
        /// </summary>
        public override void Load()
        {
            StartCoroutine(DownloadScoreSaberDatabases());            
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
                Plugin.Log.Debug($"BeatSaber Total Memory - Before ScoreSaber Unload: {GC.GetTotalMemory(false)}");
                Data = null;
                System.GC.Collect();
                Plugin.Log.Debug($"BeatSaber Total Memory - After  ScoreSaber Unload: {GC.GetTotalMemory(false)}");
            }
            else
            {
                Plugin.Log.Debug("ScoreSaber Database not loaded...");
            }
        }

        /// <summary>
        /// Helper to download both databases.  They will be stacked together, ranked is more updated.
        /// </summary>
        /// <returns></returns>
        private IEnumerator DownloadScoreSaberDatabases()
        {
            Data = null;
            _isDownloading = true;
            _cancelRequested = false;

            yield return DownloadDatabase(SCRAPED_SCORE_SABER_ALL_JSON_URL, this);

            if (_cancelRequested)
            {
                _isDownloading = false;
                yield break;
            }

            yield return DownloadDatabase(SCRAPED_SCORE_SABER_RANKED_JSON_URL, this);
            _isDownloading = false;
        }

        /// <summary>
        /// Return the appropriate handler for this database.
        /// </summary>
        /// <param name="www"></param>
        /// <returns></returns>
        public CacheableDownloadHandler GetDownloadHandler(UnityWebRequest www)
        {
            var cacheHandler = new CacheableScoreSaberDownloaderHandler(www, Buffer);
            www.SetCacheable(cacheHandler);
            return cacheHandler;
        }

        /// <summary>
        /// Acquire the results from the download handler.
        /// </summary>
        /// <param name="handler"></param>
        public void HandleDownloadResults(DownloadHandler handler)
        {
            if (Data == null)
            {
                Data = (handler as CacheableScoreSaberDownloaderHandler).DataFile;
            }
            else
            {
                // Second time, update.
                var newScoreSaberData = (handler as CacheableScoreSaberDownloaderHandler).DataFile;
                foreach (var pair in newScoreSaberData.Songs)
                {
                    if (Data.Songs.ContainsKey(pair.Key))
                    {
                        foreach (var diff in pair.Value.diffs)
                        {
                            var index = Data.Songs[pair.Key].diffs.FindIndex(x => x.diff == diff.diff);
                            if (index < 0)
                            {
                                Data.Songs[pair.Key].diffs.Add(diff);
                            }
                        }
                    }
                }
            }
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
        /// No need to interrupt ScoreSaber yet, it parses often faster than we can even interrupt it.
        /// </summary>
        public void CancelHandler(DownloadHandler handler)
        {
            return;
        }
    }
}
