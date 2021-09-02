# BeatSaberSongDataCore

A Beat Saber Plugin that manages scraped data from various sources.  

## Status
- Working with BeatSaber 1.17.0

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

## Building on Windows
To compile SongDataCore from source:

1. Install Beat Saber and Microsoft Visual Studio.
2. Download and extract the BeatSaberSongDataCore source code.
3. Create a new file `/SongDataCore/SongDataCore.csproj.user` with the following. (Make sure to replace BeatSaberDir with your real Beat Saber installation folder)
```
<?xml version="1.0" encoding="utf-8"?>
<Project ToolsVersion="15.0" xmlns="http://schemas.microsoft.com/developer/msbuild/2003">
  <PropertyGroup>
    <ProjectView>ProjectFiles</ProjectView>
    <BeatSaberDir>C:\Program Files (x86)\Steam\steamapps\common\Beat Saber</BeatSaberDir>
  </PropertyGroup>
</Project>
```
4. Open `/BeatSaberSongDataCore/SongDataCore.sln` in Microsoft Visual Studio.
5. Build the project with *Build -> Build Solution*.
