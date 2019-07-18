using UnityEngine.SceneManagement;
using System;
using UnityEngine;
using SongDataCore.BeatSaver;
using SongDataCore.ScoreSaber;
using IPA;

namespace SongDataCore
{
    public class Plugin : IBeatSaberPlugin
    {
        public const string VERSION_NUMBER = "1.1.1";
        public static Plugin Instance;
        public static IPA.Logging.Logger Log;

        public static BeatSaverDatabase BeatSaver;
        public static ScoreSaberDatabase ScoreSaber;

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

            BSEvents.OnLoad();
            BSEvents.menuSceneLoadedFresh += OnMenuSceneLoadedFresh;
        }

        public void OnApplicationQuit()
        {

        }

        private void OnMenuSceneLoadedFresh()
        {
            BeatSaver = new GameObject("SongDataCore_BeatSaver").AddComponent<BeatSaverDatabase>();
            ScoreSaber = new GameObject("SongDataCore_ScoreSaber").AddComponent<ScoreSaberDatabase>();
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
