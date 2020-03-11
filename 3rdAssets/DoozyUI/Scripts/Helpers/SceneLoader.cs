// Copyright (c) 2015 - 2018 Doozy Entertainment / Marlink Trading SRL. All Rights Reserved.
// This code can only be used under the standard Unity Asset Store End User License Agreement
// A Copy of the EULA APPENDIX 1 is available at http://unity3d.com/company/legal/as_terms

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

#if dUI_EnergyBarToolkit
using EnergyBarToolkit;
#endif

namespace DoozyUI
{
    public class SceneLoader : MonoBehaviour
    {
        private static SceneLoader _instance;
        public static SceneLoader Instance
        {
            get
            {
                if(_instance == null)
                {
                    if(applicationIsQuitting)
                    {
                        Debug.LogWarning("[Singleton] Instance '" + typeof(SceneLoader) + "' already destroyed on application quit. Won't create again - returning null.");
                        return null;
                    }

                    _instance = FindObjectOfType<SceneLoader>();

                    if(_instance == null)
                    {
                        GameObject singleton = new GameObject("SceneLoader");
                        _instance = singleton.AddComponent<SceneLoader>();
                        DontDestroyOnLoad(singleton);
                    }
                }
                return _instance;
            }
        }

        private static bool applicationIsQuitting = false;
        private void OnApplicationQuit()
        {
            applicationIsQuitting = true;
        }

        #region Context Menu
#if UNITY_EDITOR
        [UnityEditor.MenuItem(DUI.TOOLS_MENU_SCENE_LOADER, false, DUI.MENU_PRIORITY_SCENE_LOADER)]
        [UnityEditor.MenuItem(DUI.GAMEOBJECT_MENU_SCENE_LOADER, false, DUI.MENU_PRIORITY_SCENE_LOADER)]
        static void CreateSceneLoader(UnityEditor.MenuCommand menuCommand)
        {
            AddSceneLoaderToScene();
        }
#endif

        public static SceneLoader AddSceneLoaderToScene()
        {
            if (FindObjectOfType<SceneLoader>() != null)
            {
                Debug.Log("[Scene Loader] Cannot add another Scene Loader to this Scene because you don't need more than one.");
#if UNITY_EDITOR
                if (!UnityEditor.EditorApplication.isPlaying)
                {
                    UnityEditor.Selection.activeObject = FindObjectOfType<SceneLoader>();
                }
#endif
                return null;
            }
            GameObject go = new GameObject("SceneLoader", typeof(SceneLoader));
#if UNITY_EDITOR
            if (!UnityEditor.EditorApplication.isPlaying)
            {
                UnityEditor.Undo.RegisterCreatedObjectUndo(go, "Added " + go.name);
                UnityEditor.Selection.activeObject = go;
            }
#endif
            return go.GetComponent<SceneLoader>();
        }

        #endregion

        public const string DEFAULT_LOAD_SCENE_ASYNC_SCENE_NAME = "LoadSceneAsync_Name_";
        public const string DEFAULT_LOAD_SCENE_ASYNC_SCENE_BUILD_INDEX = "LoadSceneAsync_ID_";
        public const string DEFAULT_LOAD_SCENE_ADDITIVE_ASYNC_SCENE_NAME = "LoadSceneAdditiveAsync_Name_";
        public const string DEFAULT_LOAD_SCENE_ADDITIVE_ASYNC_SCENE_BUILD_INDEX = "LoadSceneAdditiveAsync_ID_";
        public const string DEFAULT_UNLOAD_SCENE_SCENE_NAME = "UnloadScene_Name_";
        public const string DEFAULT_UNLOAD_SCENE_SCENE_BUILD_INDEX = "UnloadScene_ID_";
        public const string DEFAULT_UNLOAD_LEVEL = "UnloadLevel_";
        public const string DEFAULT_LOAD_LEVEL = "LoadLevel_";
        public const string DEFAULT_LEVEL_SCENE_NAME = "Level_";
        public const string DEFAULT_LEVEL_LOADED = "LevelLoaded";

#if dUI_EnergyBarToolkit
        public List<EnergyBar> energyBars = new List<EnergyBar>();
#endif

        public string command_LoadSceneAsync_SceneName = DEFAULT_LOAD_SCENE_ASYNC_SCENE_NAME;
        public string command_LoadSceneAsync_SceneBuildIndex = DEFAULT_LOAD_SCENE_ASYNC_SCENE_BUILD_INDEX;
        public string command_LoadSceneAdditiveAsync_SceneName = DEFAULT_LOAD_SCENE_ADDITIVE_ASYNC_SCENE_NAME;
        public string command_LoadSceneAdditiveAsync_SceneBuildIndex = DEFAULT_LOAD_SCENE_ADDITIVE_ASYNC_SCENE_BUILD_INDEX;
        public string command_UnloadScene_SceneName = DEFAULT_UNLOAD_SCENE_SCENE_NAME;
        public string command_UnloadScene_SceneBuildIndex = DEFAULT_UNLOAD_SCENE_SCENE_BUILD_INDEX;
        public string command_LoadLevel = DEFAULT_LOAD_LEVEL;
        public string command_UnloadLevel = DEFAULT_UNLOAD_LEVEL;
        public string levelSceneName = DEFAULT_LEVEL_SCENE_NAME;
        public string levelLoadedGameEvent = DEFAULT_LEVEL_LOADED;

        private AsyncOperation async = null; // When assigned, load is in progress.
        private int sceneBuildIndex = -1;
        private string sceneName = "";

        void Awake()
        {
            if(_instance != null && _instance != this)
            {
                Debug.Log("[DoozyUI] There cannot be two SceneLoaders active at the same time. Destryoing this one!");
                Destroy(gameObject);
                return;
            }

            _instance = this;
            DontDestroyOnLoad(gameObject);
        }

        void Update()
        {
            CheckIfLevelLoaded();
            UpdateEnergyBars();
        }

        void CheckIfLevelLoaded()
        {
            if (async != null)
            {
                if (async.isDone)
                {
                    UIManager.SendGameEvent(levelLoadedGameEvent);
                    async = null;
                }
            }
        }

        void UpdateEnergyBars()
        {
#if dUI_EnergyBarToolkit
            // If if we are loading a level and we have energyBars linkd then we update their values
            if (async != null && energyBars != null && energyBars.Count > 0)
            {
                for (int i = 0; i < energyBars.Count; i++)
                {
                    if (energyBars[i] != null)
                        energyBars[i].SetValueF(async.progress);
                }
            }
#endif
        }

        /// <summary>
        /// Method used by the UIManager to send the game events that trigger the loading and unloading of scenes.
        /// </summary>
        public void OnGameEvent(string gameEvent)
        {
            if (gameEvent.Contains(command_LoadSceneAsync_SceneName))
            {
                sceneName = gameEvent.Replace(command_LoadSceneAsync_SceneName, "");
                LoadSceneAsync(sceneName);
            }
            else if (gameEvent.Contains(command_LoadSceneAsync_SceneBuildIndex))
            {
                sceneBuildIndex = int.Parse(gameEvent.Replace(command_LoadSceneAsync_SceneBuildIndex, ""));
                LoadSceneAsync(sceneBuildIndex);
            }
            else if (gameEvent.Contains(command_LoadSceneAdditiveAsync_SceneName))
            {
                sceneName = gameEvent.Replace(command_LoadSceneAdditiveAsync_SceneName, "");
                LoadLevelAdditiveAsync(sceneName);
            }
            else if (gameEvent.Contains(command_LoadSceneAdditiveAsync_SceneBuildIndex))
            {
                sceneBuildIndex = int.Parse(gameEvent.Replace(command_LoadSceneAdditiveAsync_SceneBuildIndex, ""));
                LoadLevelAdditiveAsync(sceneBuildIndex);
            }
            else if (gameEvent.Contains(command_UnloadScene_SceneName))
            {
                sceneName = gameEvent.Replace(command_UnloadScene_SceneName, "");
                UnloadScene(sceneName);
            }
            else if (gameEvent.Contains(command_UnloadScene_SceneBuildIndex))
            {
                sceneBuildIndex = int.Parse(gameEvent.Replace(command_UnloadScene_SceneBuildIndex, ""));
                UnloadScene(sceneBuildIndex);
            }
            else if (gameEvent.Contains(command_LoadLevel))  //SHORTCUT VARIANT - we just call LoadLevel_{LevelNumber} and we load additive async the level data
            {
                sceneName = levelSceneName + gameEvent.Replace(command_LoadLevel, "");
                async = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
            }
            else if (gameEvent.Contains(command_UnloadLevel))    ///SHORTCUT VARIANT - we just call UnloadLevel_{LevelNumber} and we unload the level data
            {
                sceneName = levelSceneName + gameEvent.Replace(command_UnloadLevel, "");
#if UNITY_5_5_OR_NEWER
                SceneManager.UnloadSceneAsync(sceneName);
#else
                SceneManager.UnloadScene(sceneName);
#endif
            }
        }

        /// <summary>
        /// Loads the scene asynchronous.
        /// </summary>
        /// <param name="sceneName">Name of the scene.</param>
        public void LoadSceneAsync(string sceneName)
        {
            async = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);
        }

        /// <summary>
        /// Loads the scene asynchronous.
        /// </summary>
        /// <param name="sceneBuildIndex">Scene's Build Index.</param>
        public void LoadSceneAsync(int sceneBuildIndex)
        {
            async = SceneManager.LoadSceneAsync(sceneBuildIndex, LoadSceneMode.Single);
        }

        /// <summary>
        /// Loads the level additive asynchronous.
        /// </summary>
        /// <param name="sceneName">Name of the scene.</param>
        public void LoadLevelAdditiveAsync(string sceneName)
        {
            async = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        }

        /// <summary>
        /// Loads the level additive asynchronous.
        /// </summary>
        /// <param name="sceneBuildIndex">Scene's Build Index.</param>
        public void LoadLevelAdditiveAsync(int sceneBuildIndex)
        {
            async = SceneManager.LoadSceneAsync(sceneBuildIndex, LoadSceneMode.Additive);
        }

        /// <summary>
        /// Unloads the scene.
        /// </summary>
        /// <param name="sceneName">Name of the scene.</param>
        public void UnloadScene(string sceneName)
        {
#if UNITY_5_5_OR_NEWER
            SceneManager.UnloadSceneAsync(sceneName);
#else
            SceneManager.UnloadScene(sceneName);
#endif
        }

        /// <summary>
        /// Unloads the scene.
        /// </summary>
        /// <param name="sceneBuildIndex">Scene's Build Index.</param>
        public void UnloadScene(int sceneBuildIndex)
        {
#if UNITY_5_5_OR_NEWER
            SceneManager.UnloadSceneAsync(sceneBuildIndex);
#else
            SceneManager.UnloadScene(sceneBuildIndex);
#endif
        }

        /// <summary>
        /// Loads the level.
        /// </summary>
        /// <param name="levelNumber">The level number.</param>
        public void LoadLevel(int levelNumber)
        {
            sceneName = levelSceneName + levelNumber;
            async = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        }

        /// <summary>
        /// Unloads the level.
        /// </summary>
        /// <param name="levelNumber">The level number.</param>
        public void UnloadLevel(int levelNumber)
        {
            sceneName = levelSceneName + levelNumber;
#if UNITY_5_5_OR_NEWER
            SceneManager.UnloadSceneAsync(sceneName);
#else
            SceneManager.UnloadScene(sceneName);
#endif
        }
    }
}