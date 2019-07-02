using UnityEngine.Networking;

namespace SongDataCore.Downloader
{
    public interface IDatabaseDownloadHandler
    {
        CacheableDownloadHandler GetDownloadHandler(UnityWebRequest www);

        void HandleDownloadResults(DownloadHandler handler);

        bool IsDataAvailable();
    }
}
