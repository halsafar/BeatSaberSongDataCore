using UnityEngine.SceneManagement;
using UnityEngine;
using SongDataCore.BeatStar;
using IPA;
using BS_Utils.Utilities;

namespace SongDataCore
{
    public class Plugin : IBeatSaberPlugin
    {
        public const string VERSION_NUMBER = "1.3.2";
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

        public void Init(object nullObject, IPA.Logging.Logger logger)
        {
            Log = logger;
        }

        public void OnApplicationStart()
        {
            Instance = this;
            DatabasesLoaded = false;

            BSEvents.OnLoad();

            BSEvents.menuSceneLoadedFresh += OnMenuSceneLoadedFresh;
            BSEvents.menuSceneLoaded += OnMenuSceneLoaded;
            BSEvents.gameSceneLoaded += OnGameSceneLoaded;
        }

        public void OnApplicationQuit()
        {

        }

        private void OnMenuSceneLoadedFresh()
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

        public void OnSceneLoaded(Scene scene, LoadSceneMode sceneMode)
        {
        }

        public void OnSceneUnloaded(Scene scene)
        {
        }

        public void OnActiveSceneChanged(Scene prevScene, Scene nextScene)
        {
        }

        public void OnUpdate()
        {

        }

        public void OnFixedUpdate()
        {
        }
    }
}
