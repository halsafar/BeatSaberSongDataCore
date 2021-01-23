using UnityEngine.SceneManagement;
using UnityEngine;
using SongDataCore.BeatStar;
using IPA;
using IPA.Logging;
using BS_Utils.Utilities;

namespace SongDataCore
{
    [Plugin(RuntimeOptions.SingleStartInit)]
    public class Plugin
    {
        public const string VERSION_NUMBER = "1.3.6";
        public static Plugin Instance;
        public static IPA.Logging.Logger Log;

        public static BeatStarDatabase Songs;

        public bool DatabasesLoaded;

        public string Name
        {
            get { return "SongDataCore"; }
        }

        public string Version
        {
            get { return VERSION_NUMBER; }
        }

        [Init]
        public Plugin(IPA.Logging.Logger logger)
        {
            Log = logger;
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
            UnityEngine.Object.DontDestroyOnLoad(Songs.gameObject);

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

            Songs.Unload();

            DatabasesLoaded = false;
        }
    }
}
