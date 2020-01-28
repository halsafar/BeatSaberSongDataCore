using UnityEngine.SceneManagement;
using System;
using UnityEngine;
using SongDataCore.BeatSaver;
using SongDataCore.ScoreSaber;
using IPA;
using BS_Utils.Utilities;

namespace SongDataCore
{
    public class Plugin : IBeatSaberPlugin
    {
        public const string VERSION_NUMBER = "1.3.0";
        public static Plugin Instance;
        public static IPA.Logging.Logger Log;

        //public static BeatSaverDatabase BeatSaver;
        public static ScoreSaberDatabase ScoreSaber;

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

            //BeatSaver = new GameObject("SongDataCore_BeatSaver").AddComponent<BeatSaverDatabase>();
            //UnityEngine.Object.DontDestroyOnLoad(BeatSaver.gameObject);

            ScoreSaber = new GameObject("SongDataCore_ScoreSaber").AddComponent<ScoreSaberDatabase>();
            UnityEngine.Object.DontDestroyOnLoad(ScoreSaber.gameObject);

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

            //BeatSaver.Load();
            ScoreSaber.Load();

            DatabasesLoaded = true;
        }

        private void UnloadDatabases()
        {
            if (!DatabasesLoaded) return;

            //BeatSaver.Unload();
            ScoreSaber.Unload();

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
