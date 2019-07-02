# BeatSaberSongDataCore

A Beat Saber Plugin that manages scraped data from various sources.  

## Supports
- Cached Downloads (ETAG of resource)
- BeatSaver data dump.
- ScoreSaber data dump.
- Songs from each source are mapped by song hash.

## Example Usage:
Data Available:
```c#
SongDataCore.Plugin.BeatSaver.IsDataAvailable()
SongDataCore.Plugin.ScoreSaber.IsDataAvailable()
```

Get song:
```c#
BeatSaverSong beatSaverSong = SongDataCore.Plugin.BeatSaver.Data.Songs[hash];
ScoreSaberSong scoreSaberSong = SongDataCore.Plugin.ScoreSaber.Data.Songs[hash];
```
