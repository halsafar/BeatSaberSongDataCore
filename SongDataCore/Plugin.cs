using UnityEngine.SceneManagement;
using UnityEngine;
using SongDataCore.BeatStar;
using IPA;
using IPA.Logging;
using BS_Utils.Utilities;
using IPA.Loader;
using System.Reflection;

namespace SongDataCore
{
    [Plugin(RuntimeOptions.SingleStartInit)]
    public class Plugin
    {
        public static string VersionNumber { get; private set; }

        public static Plugin Instance;
        public static IPA.Logging.Logger Log;

        public static BeatStarDatabase Songs;

        public bool DatabasesLoaded;

        [Init]
        public void Init(IPA.Logging.Logger logger, PluginMetadata metadata)
        {
            Log = logger;
            VersionNumber = metadata.Version?.ToString() ?? Assembly.GetExecutingAssembly().GetName().Version.ToString(3);
        }

        [OnStart]
        public void OnStart()
        {
            Instance = this;
            DatabasesLoaded = false;

            BSEvents.OnLoad();

            BSEvents.lateMenuSceneLoadedFresh += OnMenuSceneLoadedFresh;
            BSEvents.menuSceneLoaded += OnMenuSceneLoaded;
            BSEvents.gameSceneLoaded += OnGameSceneLoaded;
        }

        [OnExit]
        public void OnExit()
        {

        }

        private void OnMenuSceneLoadedFresh(ScenesTransitionSetupDataSO data)
        {
            Log.Info("OnMenuSceneLoadedFresh()");

            Songs = new GameObject("SongDataCore_BeatStar").AddComponent<BeatStarDatabase>();

            // Force false, always load the database here.
            DatabasesLoaded = false;
            LoadDatabases();
        }

        private void OnMenuSceneLoaded()
        {
            Log.Info("OnMenuSceneLoaded()");

            LoadDatabases();
        }

        private void OnGameSceneLoaded()
        {
            Log.Info("OnGameSceneLoaded()");

            UnloadDatabases();
        }

        private void LoadDatabases()
        {
            if (DatabasesLoaded) return;

            Songs.Load();

            DatabasesLoaded = true;
        }

        private void UnloadDatabases()
        {
            if (!DatabasesLoaded) return;

            if (Songs.isActiveAndEnabled)
            {
                Songs.Unload();
            } else
            {
                Songs.UnloadNow();
            }

            DatabasesLoaded = false;
        }
    }
}
