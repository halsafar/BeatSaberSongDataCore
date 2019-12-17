using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using System.Threading;
using System.Threading.Tasks;

namespace SongDataCore.Downloader
{
    public class DatabaseDownloader : MonoBehaviour
    {
        public Action OnStartDownloading;
        public Action OnFinishDownloading;
        public Action OnDataFinishedProcessing;
        public Action OnFailedDownload;

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

                bool running = true;
                try
                {
                    Plugin.Log.Info($"Started data processing thread!");
                    Task mytask = Task.Run(() =>
                    {
                        try
                        {
                            handler.HandleDownloadResults(www.downloadHandler);
                            Plugin.Log.Info($"Success processing data: {www.url}");
                        }
                        finally
                        {
                            running = false;
                        }
                    });
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

                // Wait for the thread to finish/die
                while (running)
                {
                    yield return null;
                }
                OnDataFinishedProcessing?.Invoke();
            }
        }
    }
}
