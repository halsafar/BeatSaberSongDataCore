using UnityEngine.Networking;

namespace SongDataCore.Downloader
{
    public interface IDatabaseDownloadHandler
    {
        CacheableDownloadHandler GetDownloadHandler(UnityWebRequest www);

        void HandleDownloadResults(DownloadHandler handler);

        void CancelHandler(DownloadHandler handler);

        bool IsDataAvailable();
    }
}
