using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

namespace SongDataCore.Downloader
{
    public class DatabaseDownloader : MonoBehaviour
    {
        public Action OnStartDownloading;
        public Action OnFinishDownloading;
        public Action OnDataFinishedProcessing;
        public Action OnFailedDownload;

        protected readonly byte[] Buffer = new byte[4 * 1048576];
        
        /// <summary>
        /// Download a database, invoke the appropriate actions to inform listerns.
        /// </summary>
        /// <returns></returns>
        protected IEnumerator DownloadDatabase(String url, IDatabaseDownloadHandler handler)
        {
            OnStartDownloading?.Invoke();

            Plugin.Log.Info($"Preparing to download: {url}");
            using (UnityWebRequest www = UnityWebRequest.Get(url))
            {
                var cacheHandler = handler.GetDownloadHandler(www);
                if (cacheHandler == null)
                {
                    OnFailedDownload?.Invoke();
                    Plugin.Log.Error($"Could not acquire a download handler for URL {url}");
                    yield break;
                }

                Plugin.Log.Info($"Sending Web Request: {url}");
                yield return www.SendWebRequest();

                Plugin.Log.Info($"Success downloading data!");

                OnFinishDownloading?.Invoke();

                try
                {
                    Plugin.Log.Info($"Processing data!");

                    handler.HandleDownloadResults(www.downloadHandler);

                    OnDataFinishedProcessing?.Invoke();

                    Plugin.Log.Info($"Success processing data!");
                }
                catch (System.InvalidOperationException)
                {
                    Plugin.Log.Error($"Failed to download data file...");
                    OnFailedDownload?.Invoke();
                }
                catch (Exception e)
                {
                    Plugin.Log.Critical($"Exception trying to download data file... {e}");
                    OnFailedDownload?.Invoke();
                }
            }
        }
    }
}
