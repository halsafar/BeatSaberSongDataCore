using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using System.Threading.Tasks;
using UnityEngine.SceneManagement;
using System.Threading;

namespace SongDataCore.Downloader
{
    public abstract class DatabaseDownloader : MonoBehaviour
    {
        public Action OnStartDownloading;
        public Action OnFinishDownloading;
        public Action OnDataFinishedProcessing;
        public Action OnFailedDownload;

        private bool _backgroundHandlerRunning;

        protected bool _cancelRequested = false;
        public bool CancelRequested { get => _cancelRequested; set => _cancelRequested = value; }

        protected bool _isDownloading = false;
        public bool IsDownloading { get => _isDownloading; }

        public abstract void Load();
        public abstract void Unload();

        /// <summary>
        /// Block and wait for download operation to cancel.
        /// </summary>
        /// <returns></returns>
        protected IEnumerator CancelDownload()
        {
            Plugin.Log.Debug("Attempting to cancel download...");

            _cancelRequested = true;

            while (_isDownloading)
            {
                yield return null;
            }
        }

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

                if (www.isNetworkError)
                {
                    Plugin.Log.Error($"Network error downloading: {url}");
                    yield break;
                }

                Plugin.Log.Info($"Success downloading data!");

                if (_cancelRequested)
                {
                    Plugin.Log.Debug("Cancel requested after download.");
                    yield break;
                }

                OnFinishDownloading?.Invoke();

                Task myTask = null;
                CancellationTokenSource tokenSource = new CancellationTokenSource();
                try
                {
                    Plugin.Log.Info($"Started data processing thread!");
                    _backgroundHandlerRunning = true;
                    myTask = Task.Run(() => BackgroundDownloadHandler(handler, www));
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
                bool cancelled = false;
                while (_backgroundHandlerRunning)
                {
                    if (!cancelled && _cancelRequested)
                    {
                        Plugin.Log.Debug("Cancel requested during parsing");
                        cancelled = true;
                        handler.CancelHandler(cacheHandler);
                    }

                    yield return null;
                }

                yield return new WaitForEndOfFrame();

                if (_cancelRequested)
                {
                    Plugin.Log.Debug("Cancel requested after parsing, aborting.");
                    yield break;
                }

                Scene scene = SceneManager.GetActiveScene();
                if (scene.name == "GameCore")
                {
                    Plugin.Log.Debug("SongDataCore cannot fire events outside of menu scene.");
                    yield break;
                }

                OnDataFinishedProcessing?.Invoke();
            }
        }

        private void BackgroundDownloadHandler(IDatabaseDownloadHandler handler, UnityWebRequest www)
        {
            try
            {
                if (_cancelRequested)
                {
                    Plugin.Log.Debug("Cancel requested mid-download, aborting.");
                    return;
                }

                handler.HandleDownloadResults(www.downloadHandler);
                Plugin.Log.Info($"Success processing data: {www.url}");
            }
            finally
            {
                _backgroundHandlerRunning = false;
            }
        }
    }
}
