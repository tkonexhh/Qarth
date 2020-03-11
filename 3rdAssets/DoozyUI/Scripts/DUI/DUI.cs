// Copyright (c) 2015 - 2018 Doozy Entertainment / Marlink Trading SRL. All Rights Reserved.
// This code can only be used under the standard Unity Asset Store End User License Agreement
// A Copy of the EULA APPENDIX 1 is available at http://unity3d.com/company/legal/as_terms

using UnityEngine;
using System.Collections.Generic;
using System;
using QuickEngine.Extensions;
using QuickEngine.Core;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace DoozyUI
{
    [Serializable]
    public partial class DUI
    {
        #region Context Menu Settings

        public const int MENU_PRIORITY_UICANVAS = 0;
        public const string COMPONENT_MENU_UICANVAS = "DoozyUI/UICanvas";
        public const string GAMEOBJECT_MENU_UICANVAS = "GameObject/DoozyUI/UICanvas";

        public const int MENU_PRIORITY_UIELEMENT = 1;
        public const string COMPONENT_MENU_UIELEMENT = "DoozyUI/UIElement";
        public const string GAMEOBJECT_MENU_UIELEMENT = "GameObject/DoozyUI/UIElement";

        public const int MENU_PRIORITY_UIBUTTON = 2;
        public const string COMPONENT_MENU_UIBUTTON = "DoozyUI/UIButton";
        public const string GAMEOBJECT_MENU_UIBUTTON = "GameObject/DoozyUI/UIButton";

        public const int MENU_PRIORITY_UITOGGLE = 3;
        public const string COMPONENT_MENU_UITOGGLE = "DoozyUI/UIToggle";
        public const string GAMEOBJECT_MENU_UITOGGLE = "GameObject/DoozyUI/UIToggle";

        public const int MENU_PRIORITY_UIEFFECT = 4;
        public const string COMPONENT_MENU_UIEFFECT = "DoozyUI/UIEffect";
        public const string GAMEOBJECT_MENU_UIEFFECT = "GameObject/DoozyUI/UIEffect";

        public const int MENU_PRIORITY_UITRIGGER = 5;
        public const string COMPONENT_MENU_UITRIGGER = "DoozyUI/UITrigger";
        public const string GAMEOBJECT_MENU_UITRIGGER = "GameObject/DoozyUI/UITrigger";

        public const int MENU_PRIORITY_UINOTIFICATION = 6;
        public const string COMPONENT_MENU_UINOTIFICATION = "DoozyUI/UINotification";
        public const string GAMEOBJECT_MENU_UINOTIFICATION = "GameObject/DoozyUI/UINotification";


        public const int MENU_PRIORITY_PLAYMAKER_EVENT_DISPATCHER = 50;
        public const string COMPONENT_MENU_PLAYMAKER_EVENT_DISPATCHER = "DoozyUI/Playmaker/EventDispatcher";
        public const string GAMEOBJECT_MENU_PLAYMAKER_EVENT_DISPATCHER = "GameObject/DoozyUI/Playmaker/EventDispatcher";


        public const int MENU_PRIORITY_UIMANAGER = 100;
        public const string TOOLS_MENU_UIMANAGER = "Tools/DoozyUI/Managers/UIManager";
        public const string GAMEOBJECT_MENU_UIMANAGER = "GameObject/DoozyUI/Managers/UIManager";

        public const int MENU_PRIORITY_ORIENTATION_MANAGER = 101;
        public const string TOOLS_MENU_ORIENTATION_MANAGER = "Tools/DoozyUI/Managers/OrientationManager";
        public const string GAMEOBJECT_MENU_ORIENTATION_MANAGER = "GameObject/DoozyUI/Managers/OrientationManager";

        public const int MENU_PRIORITY_SCENE_LOADER = 102;
        public const string TOOLS_MENU_SCENE_LOADER = "Tools/DoozyUI/Managers/SceneLoader";
        public const string GAMEOBJECT_MENU_SCENE_LOADER = "GameObject/DoozyUI/Managers/SceneLoader";

        public const int MENU_PRIORITY_TOUCH_MANAGER = 103;
        public const string TOOLS_MENU_TOUCH_MANAGER = "Tools/DoozyUI/Managers/TouchManager";
        public const string GAMEOBJECT_MENU_TOUCH_MANAGER = "GameObject/DoozyUI/Managers/TouchManager";
        #endregion


#if dUI_PlayMaker
        public static bool PlayMakerEnabled = true;
#else
        public static bool PlayMakerEnabled = false;
#endif

#if dUI_MasterAudio
        public static bool MasterAudioEnabled = true;
#else
        public static bool MasterAudioEnabled = false;
#endif

#if dUI_EnergyBarToolkit
        public static bool EnergyBarToolkitEnabled = true;
#else
        public static bool EnergyBarToolkitEnabled = false;
#endif

#if dUI_TextMeshPro
        public static bool TextMeshProEnabled = true;
#else
        public static bool TextMeshProEnabled = false;
#endif

#if dUI_NavigationDisabled
        public static bool UINavigationEnabled = false;
#else
        public static bool UINavigationEnabled = true;
#endif

#if dUI_UseOrientationManager
        public static bool OrientationManagerEnabled = true;
#else
        public static bool OrientationManagerEnabled = false;
#endif

#if dUI_SceneLoaderDisabled
        public static bool SceneLoaderEnabled = false;
#else
        public static bool SceneLoaderEnabled = true;
#endif

#if dUI_UIDrawer
        public static bool UIDrawerEnabled = true;
#else
        public static bool UIDrawerEnabled = false;
#endif

        public const string SYMBOL_DOOZYUI = "dUI_DoozyUI";
        public const string SYMBOL_PLAYMAKER = "dUI_PlayMaker";
        public const string SYMBOL_MASTER_AUDIO = "dUI_MasterAudio";
        public const string SYMBOL_ENERGY_BAR_TOOLKIT = "dUI_EnergyBarToolkit";
        public const string SYMBOL_TEXT_MESH_PRO = "dUI_TextMeshPro";
        public const string SYMBOL_NAVIGATION_SYSTEM = "dUI_NavigationDisabled";
        public const string SYMBOL_ORIENTATION_MANAGER = "dUI_UseOrientationManager";
        public const string SYMBOL_SCENE_LOADER = "dUI_SceneLoaderDisabled";
        public const string SYMBOL_UIDRAWER = "dUI_UIDrawer";

        public const string UNCATEGORIZED_CATEGORY_NAME = "Uncategorized";
        public const string DEFAULT_ELEMENT_NAME = "~Element Name~";
        public const string DEFAULT_BUTTON_NAME = "~Button Name~";
        public const string DEFAULT_SOUND_NAME = "~No Sound~";
        public const string MASTER_CANVAS_NAME = UICanvas.MASTER_CANVAS_NAME;

        public const string DISPATCH_ALL = "~Dispatch All~";
        public const string CUSTOM_NAME = "~Custom Name~";
        public const string BACK_BUTTON_NAME = "Back";

        public const int GLOBAL_EDITOR_WIDTH = 420;
        public const int BIG_BAR_HEIGHT = 24;
        public const int BAR_HEIGHT = 20;
        public const int MINI_BAR_HEIGHT = 18;

        public enum EventType { GameEvent, ButtonClick, ButtonDoubleClick, ButtonLongClick }

        public const string CANVAS_DATABASE_FILENAME = "CanvasDatabase";
        public const string SETTINGS_FILENAME = "DUISettings";

        private static string _DOOZYUI_PATH = "";
        public static string PATH
        {
            get
            {
                if(_DOOZYUI_PATH.IsNullOrEmpty())
                {
                    _DOOZYUI_PATH = QuickEngine.IO.File.GetRelativeDirectoryPath("DoozyUI");
                }
                return _DOOZYUI_PATH;
            }
        }

        public const string RESOURCES_PATH_DUIDATA = "DUI/Data/";
        public const string RESOURCES_PATH_CONTROL_PANEL_WINDOW_SETTINGS = "DUI/Data/";
        public const string RESOURCES_PATH_BUTTON_LINKS = "DUI/Data/";
        public const string RESOURCES_PATH_NEWS_ARTICLES = "DUI/Data/";
        public const string RESOURCES_PATH_VERSION = "DUI/Data/";

        public static string RELATIVE_PATH_DUIDATA { get { return PATH + "/Editor/Resources/" + RESOURCES_PATH_DUIDATA; } }
        public static string RELATIVE_PATH_CONTROL_PANEL_WINDOW_SETTINGS { get { return PATH + "/Editor/Resources/" + RESOURCES_PATH_CONTROL_PANEL_WINDOW_SETTINGS; } }
        public static string RELATIVE_PATH_BUTTON_LINKS { get { return PATH + "/Editor/Resources/" + RESOURCES_PATH_BUTTON_LINKS; } }
        public static string RELATIVE_PATH_NEWS_ARTICLES { get { return PATH + "/Editor/Resources/" + RESOURCES_PATH_NEWS_ARTICLES; } }
        public static string RELATIVE_PATH_VERSION { get { return PATH + "/Editor/Resources/" + RESOURCES_PATH_VERSION; } }


        public const string RESOURCES_PATH_UIELEMENTS = "DUI/UIElements/";
        public const string RESOURCES_PATH_UIBUTTONS = "DUI/UIButtons/";
        public const string RESOURCES_PATH_UISOUNDS = "DUI/UISounds/";
        public const string RESOURCES_PATH_CANVAS_DATABASE = "DUI/Canvases/";
        public const string RESOURCES_PATH_SETTINGS = "DUI/Settings/";

        public static string RELATIVE_PATH_UIELEMENTS { get { return PATH + "/Resources/" + RESOURCES_PATH_UIELEMENTS; } }
        public static string RELATIVE_PATH_UIBUTTONS { get { return PATH + "/Resources/" + RESOURCES_PATH_UIBUTTONS; } }
        public static string RELATIVE_PATH_UISOUNDS { get { return PATH + "/Resources/" + RESOURCES_PATH_UISOUNDS; } }
        public static string RELATIVE_PATH_CANVAS_DATABASE { get { return PATH + "/Resources/" + RESOURCES_PATH_CANVAS_DATABASE; } }
        public static string RELATIVE_PATH_SETTINGS { get { return PATH + "/Resources/" + RESOURCES_PATH_SETTINGS; } }

        private static string[] GetUIElementCategoriesFileNames { get { return QuickEngine.IO.File.GetFilesNames(RELATIVE_PATH_UIELEMENTS, "asset"); } }
        private static string[] GetUIButtonCategoriesFileNames { get { return QuickEngine.IO.File.GetFilesNames(RELATIVE_PATH_UIBUTTONS, "asset"); } }
        private static string[] GetUISoundsFileNames { get { return QuickEngine.IO.File.GetFilesNames(RELATIVE_PATH_UISOUNDS, "asset"); } }

        public static T GetResource<T>(string resourcesPath, string fileName) where T : ScriptableObject
        {
            return (T)Resources.Load(resourcesPath + fileName, typeof(T));
        }

        public static UISound GetUISound(string soundName)
        {
            return GetResource<UISound>(RESOURCES_PATH_UISOUNDS, soundName);
        }

        private static DUISettings _DUISettings;
        public static DUISettings DUISettings
        {
            get
            {
                if(_DUISettings == null)
                {
#if UNITY_EDITOR
                    if(!AssetDatabase.IsValidFolder(DUI.PATH + "/Resources")) { AssetDatabase.CreateFolder(DUI.PATH, "Resources"); }
                    if(!AssetDatabase.IsValidFolder(DUI.PATH + "/Resources/DUI")) { AssetDatabase.CreateFolder(DUI.PATH + "/Resources", "DUI"); }
                    if(!AssetDatabase.IsValidFolder(DUI.PATH + "/Resources/DUI/Settings")) { AssetDatabase.CreateFolder(DUI.PATH + "/Resources/DUI", "Settings"); }
#endif
                    _DUISettings = Q.GetResource<DUISettings>(RESOURCES_PATH_SETTINGS, SETTINGS_FILENAME);
                }

#if UNITY_EDITOR && !dUI_SOURCE
                if(_DUISettings == null)
                {
                    _DUISettings = Q.CreateAsset<DUISettings>(RELATIVE_PATH_SETTINGS, SETTINGS_FILENAME);
                }
#endif
                return _DUISettings;
            }
        }

#if UNITY_EDITOR
        public static T CreateAsset<T>(string relativePath, string fileName, string extension = ".asset") where T : ScriptableObject
        {
            T asset = ScriptableObject.CreateInstance<T>();
            AssetDatabase.CreateAsset(asset, relativePath + fileName + extension);
            EditorUtility.SetDirty(asset);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            return asset;
        }

        public static Dictionary<string, NamesDatabase> UIButtonsDatabase;
        public static List<UISound> UISoundsDatabase;
        public static NamesDatabase CanvasNamesDatabase;

        public static NamesDatabase GetUIButtonsDatabase(string category)
        {
            return GetResource<NamesDatabase>(RESOURCES_PATH_UIBUTTONS, category);
        }
        public static NamesDatabase GetCanvasNamesDatabase()
        {
            return GetResource<NamesDatabase>(RESOURCES_PATH_CANVAS_DATABASE, CANVAS_DATABASE_FILENAME);
        }

        private static string[] fileNames;

        public static void RefreshUIButtonsDatabase()
        {
            EditorUtility.DisplayProgressBar("Refreshing UIButtons Database", "", 0f);
            if(UIButtonsDatabase == null) { UIButtonsDatabase = new Dictionary<string, NamesDatabase>(); }
            UIButtonsDatabase.Clear();
            fileNames = null;
            fileNames = GetUIButtonCategoriesFileNames;
            int fileNamesLength = fileNames != null ? fileNames.Length : 0;
            for(int fileIndex = 0; fileIndex < fileNamesLength; fileIndex++)
            {
                EditorUtility.DisplayProgressBar("Refreshing UIButtons Database", fileNames[fileIndex], ((fileIndex + 1) / (fileNamesLength + 2)));
                UIButtonsDatabase.Add(fileNames[fileIndex], GetUIButtonsDatabase(fileNames[fileIndex]));
            }
            EditorUtility.DisplayProgressBar("Refreshing UIButtons Database", "Creating Categories List...", 0.9f);
            RefreshUIButtonCategories();
            EditorUtility.DisplayProgressBar("Refreshing UIButtons Database", "Validating...", 1f);
            ValidateUIButtonsDatabase();
            EditorUtility.ClearProgressBar();
        }
        public static void RefreshUISoundsDatabase()
        {
            EditorUtility.DisplayProgressBar("Refreshing UISounds Database", "", 0f);
            if(UISoundsDatabase == null) { UISoundsDatabase = new List<UISound>(); }
            UISoundsDatabase.Clear();
            fileNames = null;
            fileNames = GetUISoundsFileNames;
            int fileNamesLength = fileNames != null ? fileNames.Length : 0;
            for(int fileIndex = 0; fileIndex < fileNamesLength; fileIndex++)
            {
                UISound asset = GetResource<UISound>(RESOURCES_PATH_UISOUNDS, fileNames[fileIndex]);
                if(asset == null) { continue; }
                EditorUtility.DisplayProgressBar("Refreshing UISounds Database", fileNames[fileIndex], ((fileIndex + 1) / (fileNamesLength + 2)));
                UISoundsDatabase.Add(asset);
            }
            EditorUtility.DisplayProgressBar("Refreshing UISounds Database", "Creating Sound Names List...", 0.9f);
            RefreshUISoundNames();
            EditorUtility.DisplayProgressBar("Refreshing UISounds Database", "Validating...", 1f);
            ValidateUISoundsDatabase();
            EditorUtility.ClearProgressBar();
        }
        public static void RefreshCanvasNamesDatabase()
        {
            EditorUtility.DisplayProgressBar("Refreshing Canvas Names Database", "", 0f);
            CanvasNamesDatabase = GetCanvasNamesDatabase();
            if(CanvasNamesDatabase == null) { CreateCanvasDatabase(); } //the canvas names database asset file does not exist -> create it
            EditorUtility.DisplayProgressBar("Refreshing Canvas Names Database", "Creating Canvas Names List...", 0.9f);
            RefreshCanvasNames();
            EditorUtility.DisplayProgressBar("Refreshing Canvas Names Database", "Validating...", 1f);
            ValidateCanvasNamesDatabase();
            EditorUtility.ClearProgressBar();
        }


        private static List<string> m_UIButtonCategories;
        public static List<string> UIButtonCategories { get { if(m_UIButtonCategories == null) { RefreshUIButtonCategories(); } return m_UIButtonCategories; } }
        public static void RefreshUIButtonCategories()
        {

            if(m_UIButtonCategories == null) { m_UIButtonCategories = new List<string>(); }
            m_UIButtonCategories.Clear();
            m_UIButtonCategories.Add(CUSTOM_NAME);
            m_UIButtonCategories.AddRange(GetUIButtonCategories());
        }

        private static List<string> m_UISoundNamesAll;
        public static List<string> UISoundNamesAll { get { if(m_UISoundNamesAll == null) { RefreshUISoundNames(); } return m_UISoundNamesAll; } }
        private static List<string> m_UISoundNamesUIButtons;
        public static List<string> UISoundNamesUIButtons { get { if(m_UISoundNamesUIButtons == null) { RefreshUISoundNames(); } return m_UISoundNamesUIButtons; } }
        private static List<string> m_UISoundNamesUIElements;
        public static List<string> UISoundNamesUIElements { get { if(m_UISoundNamesUIElements == null) { RefreshUISoundNames(); } return m_UISoundNamesUIElements; } }
        public static void RefreshUISoundNames()
        {
            if(m_UISoundNamesAll == null) { m_UISoundNamesAll = new List<string>(); }
            m_UISoundNamesAll.Clear();
            m_UISoundNamesAll.Add(DEFAULT_SOUND_NAME);
            m_UISoundNamesAll.AddRange(GetUISoundNames(SoundType.All));

            if(m_UISoundNamesUIButtons == null) { m_UISoundNamesUIButtons = new List<string>(); }
            m_UISoundNamesUIButtons.Clear();
            m_UISoundNamesUIButtons.Add(DEFAULT_SOUND_NAME);
            m_UISoundNamesUIButtons.AddRange(GetUISoundNames(SoundType.UIButtons));

            if(m_UISoundNamesUIElements == null) { m_UISoundNamesUIElements = new List<string>(); }
            m_UISoundNamesUIElements.Clear();
            m_UISoundNamesUIElements.Add(DEFAULT_SOUND_NAME);
            m_UISoundNamesUIElements.AddRange(GetUISoundNames(SoundType.UIElements));
        }

        private static List<string> m_CanvasNames;
        public static List<string> CanvasNames { get { if(m_CanvasNames == null) { RefreshCanvasNames(); } return m_CanvasNames; } }
        public static void RefreshCanvasNames()
        {
            if(m_CanvasNames == null) { m_CanvasNames = new List<string>(); }
            m_CanvasNames.Clear();
            m_CanvasNames.AddRange(GetCanvasNames());
        }

        public static bool UIButtonCategoryExists(string categoryName)
        {
            return UIButtonCategories.Contains(categoryName);
        }
        public static bool UIButtonNameExists(string categoryName, string elementName)
        {
            if(!UIButtonCategoryExists(categoryName)) { return false; }
            return GetUIButtonNames(categoryName).Contains(elementName);
        }
        public static bool UISoundNameExists(string soundName, SoundType soundType = SoundType.All)
        {
            return GetUISoundNames(soundType).Contains(soundName);
        }

        private static NamesDatabase UIButtonCategoryDatabase(string categoryName)
        {
            NamesDatabase db = GetResource<NamesDatabase>(RESOURCES_PATH_UIBUTTONS, categoryName);
            if(db != null)
            {
                return db;
            }
            return CreateAsset<NamesDatabase>(RELATIVE_PATH_UIBUTTONS, categoryName);
        }

        private static string[] GetUIButtonCategories()
        {
            if(UIButtonsDatabase == null) { RefreshUIButtonsDatabase(); }
            return new List<string>(UIButtonsDatabase.Keys).ToArray();
        }
        private static string[] GetCanvasNames()
        {
            if(CanvasNamesDatabase == null) { RefreshCanvasNames(); }
            return CanvasNamesDatabase.ToArray();
        }

        public static List<string> GetUIButtonNames(string categoryName)
        {
            if(UIButtonsDatabase == null) { RefreshUIButtonsDatabase(); }
            if(!UIButtonsDatabase.ContainsKey(categoryName)) { return null; }
            return UIButtonsDatabase[categoryName].data;
        }
        private static List<string> GetUISoundNames(SoundType soundType = SoundType.All)
        {
            if(UISoundsDatabase == null) { RefreshUISoundsDatabase(); }
            List<string> soundNames = new List<string>();
            foreach(var sound in UISoundsDatabase)
            {
                switch(soundType)
                {
                    case SoundType.All:
                        soundNames.Add(sound.soundName);
                        break;
                    case SoundType.UIButtons:
                        if(sound.soundType == SoundType.All || sound.soundType == SoundType.UIButtons)
                        {
                            soundNames.Add(sound.soundName);
                        }
                        break;
                    case SoundType.UIElements:
                        if(sound.soundType == SoundType.All || sound.soundType == SoundType.UIElements)
                        {
                            soundNames.Add(sound.soundName);
                        }
                        break;
                }
            }
            return soundNames;
        }

        private static void AddName(NamesDatabase targetDatabase, string name)
        {
            if(targetDatabase == null) { return; }
            targetDatabase.Add(name);
            EditorUtility.SetDirty(targetDatabase);
            AssetDatabase.SaveAssets();
        }
        public static void AddUIButtonName(string categoryName, string elementName)
        {
            if(UIButtonsDatabase == null) { RefreshUIButtonsDatabase(); }
            if(!UIButtonCategoryExists(categoryName))
            {
                CreateUIButtonsCategory(categoryName);
            }
            UIButtonCategoryDatabase(categoryName).Add(elementName);
            EditorUtility.SetDirty(UIButtonCategoryDatabase(categoryName));
            AssetDatabase.SaveAssets();
            RefreshUIButtonsDatabase();
        }
        public static void AddCanvasName(string canvasName)
        {
            if(CanvasNamesDatabase == null) { RefreshCanvasNamesDatabase(); }
            if(CanvasNamesDatabase.Contains(canvasName)) { return; }
            CanvasNamesDatabase.Add(canvasName);
            EditorUtility.SetDirty(CanvasNamesDatabase);
            AssetDatabase.SaveAssets();
            RefreshCanvasNamesDatabase();
        }

        private static void RemoveName(NamesDatabase targetDatabase, string name)
        {
            if(targetDatabase == null) { return; }
            targetDatabase.Remove(name);
            EditorUtility.SetDirty(targetDatabase);
            AssetDatabase.SaveAssets();
        }
        public static void RemoveUIButtonName(string categoryName, string elementName)
        {
            if(UIButtonsDatabase == null) { RefreshUIButtonsDatabase(); }
            if(!UIButtonCategoryExists(categoryName)) { return; }
            if(!UIButtonNameExists(categoryName, elementName)) { return; }
            UIButtonCategoryDatabase(categoryName).Remove(elementName);
            EditorUtility.SetDirty(UIButtonCategoryDatabase(categoryName));
            AssetDatabase.SaveAssets();
            RefreshUIButtonsDatabase();
        }

        public static void RenameName(NamesDatabase targetDatabase, string oldName, string newName)
        {
            if(oldName.Equals(newName) || string.IsNullOrEmpty(oldName) || string.IsNullOrEmpty(newName)) { return; }
            if(targetDatabase == null || !targetDatabase.Contains(oldName)) { return; }
            if(targetDatabase.Contains(newName)) { targetDatabase.Remove(newName); }
            targetDatabase.data[targetDatabase.IndexOf(oldName)] = newName;
            targetDatabase.Sort();
            EditorUtility.SetDirty(targetDatabase);
            AssetDatabase.SaveAssets();
        }

        public static void CreateUIButtonsCategory(string categoryName)
        {
            if(string.IsNullOrEmpty(categoryName)) { return; }
            if(UIButtonsDatabase.ContainsKey(categoryName)) { return; }
            NamesDatabase newCategory = CreateAsset<NamesDatabase>(RELATIVE_PATH_UIBUTTONS, categoryName);
            newCategory.Init();
            EditorUtility.SetDirty(newCategory);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            RefreshUIButtonsDatabase();
        }
        public static void CreateUISound(string soundName, SoundType soundType, AudioClip audioClip = null)
        {
            if(string.IsNullOrEmpty(soundName)) { return; }
            if(UISoundNameExists(soundName, soundType)) { return; }
            if(UISoundNameExists(soundName, SoundType.All))
            {
                UpdateUISoundSoundType(soundName, SoundType.All);
                return;
            }
            UISound newUISound = CreateAsset<UISound>(RELATIVE_PATH_UISOUNDS, soundName);
            newUISound.soundType = soundType;
            newUISound.soundName = soundName;
            newUISound.audioClip = audioClip;
            EditorUtility.SetDirty(newUISound);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            RefreshUISoundsDatabase();
        }
        private static void CreateCanvasDatabase()
        {
            NamesDatabase canvasDatabase = CreateAsset<NamesDatabase>(RELATIVE_PATH_CANVAS_DATABASE, CANVAS_DATABASE_FILENAME);
            canvasDatabase.Init();
            EditorUtility.SetDirty(canvasDatabase);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            RefreshCanvasNamesDatabase();
        }
        public static void CreateDUISettings()
        {
            DUISettings asset = CreateAsset<DUISettings>(RELATIVE_PATH_SETTINGS, SETTINGS_FILENAME);
            EditorUtility.SetDirty(asset);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            _DUISettings = GetResource<DUISettings>(RESOURCES_PATH_SETTINGS, SETTINGS_FILENAME);
        }

        private static void UpdateUISoundSoundType(string soundName, SoundType newSoundType = SoundType.All)
        {
            if(UISoundsDatabase == null) { RefreshUISoundsDatabase(); }
            for(int i = 0; i < UISoundsDatabase.Count; i++)
            {
                if(!UISoundsDatabase[i].soundName.Equals(soundName)) { continue; }
                UISoundsDatabase[i].soundType = newSoundType;
                EditorUtility.SetDirty(UISoundsDatabase[i]);
                AssetDatabase.SaveAssets();
                RefreshUISoundNames();
                break;
            }
        }

        public static void RenameUIButtonsCategory(string oldName, string newName)
        {
            if(string.IsNullOrEmpty(oldName) || string.IsNullOrEmpty(newName) || oldName.Equals(newName)) { return; }
            if(!UIButtonsDatabase.ContainsKey(oldName)) { return; }
            if(UIButtonsDatabase.ContainsKey(newName)) { return; }
            AssetDatabase.RenameAsset(RELATIVE_PATH_UIBUTTONS + oldName, newName);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            RefreshUIButtonsDatabase();
        }

        public static void SortUIButtonsCategories(string categoryName)
        {
            if(string.IsNullOrEmpty(categoryName)) { return; }
            if(UIButtonsDatabase.ContainsKey(categoryName)) { return; }
            UIButtonsDatabase[categoryName].Sort();
            EditorUtility.SetDirty(UIButtonsDatabase[categoryName]);
            AssetDatabase.SaveAssets();
        }

        public static void DeleteUIButtonsCategory(string categoryName)
        {
            if(string.IsNullOrEmpty(categoryName)) { return; }
            if(!UIButtonsDatabase.ContainsKey(categoryName)) { return; }
            if(AssetDatabase.MoveAssetToTrash(RELATIVE_PATH_UIBUTTONS + categoryName + ".asset"))
            {
                Debug.Log("[DoozyUI] The '" + categoryName + "' UIButtons category database asset has been moved to trash.");
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
                RefreshUIButtonsDatabase();
            }
        }
        public static void DeleteUISound(string soundName)
        {
            if(string.IsNullOrEmpty(soundName)) { return; }
            if(!UISoundNameExists(soundName)) { return; }
            if(AssetDatabase.MoveAssetToTrash(RELATIVE_PATH_UISOUNDS + soundName + ".asset"))
            {
                Debug.Log("[DoozyUI] The '" + soundName + "' UISound asset has been moved to trash.");
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
                RefreshUISoundsDatabase();
            }
        }

        private static void ValidateUIButtonsDatabase()
        {
            bool refreshDatabase = false;
            if(UIButtonsDatabase == null) { RefreshUIButtonsDatabase(); return; }
            if(!UIButtonCategoryExists(UNCATEGORIZED_CATEGORY_NAME))
            {
                refreshDatabase = true;
                NamesDatabase newCategory = CreateAsset<NamesDatabase>(RELATIVE_PATH_UIBUTTONS, UNCATEGORIZED_CATEGORY_NAME);
                newCategory.Init();
                EditorUtility.SetDirty(newCategory);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
            }
            else if(!UIButtonNameExists(UNCATEGORIZED_CATEGORY_NAME, DEFAULT_BUTTON_NAME))
            {
                refreshDatabase = true;
            }
            if(refreshDatabase)
            {
                AddName(UIButtonCategoryDatabase(UNCATEGORIZED_CATEGORY_NAME), DEFAULT_BUTTON_NAME);
                RefreshUIButtonsDatabase();
            }
        }
        private static void ValidateUISoundsDatabase()
        {
            bool refreshDatabase = false;
            string[] fileNames = GetUISoundsFileNames;
            for(int fileIndex = 0; fileIndex < fileNames.Length; fileIndex++)
            {
                UISound asset = GetResource<UISound>(RESOURCES_PATH_UISOUNDS, fileNames[fileIndex]);
                if(asset == null) { continue; }
                if(string.IsNullOrEmpty(asset.soundName)) //the UISound .asset file does not have a soundName -> deletion is in order
                {
                    refreshDatabase = true;
                    if(AssetDatabase.MoveAssetToTrash(RELATIVE_PATH_UISOUNDS + fileNames[fileIndex] + ".asset")) //move asset file to trash
                    {
                        AssetDatabase.SaveAssets();
                    }
                    continue;
                }
                if(!fileNames[fileIndex].Equals(asset.soundName)) //the .asset fileName does not match the soundName
                {
                    refreshDatabase = true;
                    if(GetResource<UISound>(RESOURCES_PATH_UISOUNDS, asset.soundName) != null) //there is an .asset fileName that is the same with the soundName
                    {
                        if(AssetDatabase.MoveAssetToTrash(RELATIVE_PATH_UISOUNDS + fileNames[fileIndex] + ".asset")) //move asset file to trash (just in case)
                        {
                            Debug.Log("[DoozyUI] UISoundsDatabase automated validation system - The " + fileNames[fileIndex] + ".asset file has been moved to trash. This happened because there is another UISound asset file that has the '" + asset.soundName + "' soundName. The system does not allow for duplicate soundNames in the database, thus it executed this fail safe operation.");
                            AssetDatabase.SaveAssets();
                        }
                        continue;
                    }
                    CreateUISound(asset.soundName, asset.soundType, asset.audioClip);
                    if(AssetDatabase.MoveAssetToTrash(RELATIVE_PATH_UISOUNDS + fileNames[fileIndex] + ".asset")) //move asset file to trash to avoid duplicates
                    {
                        AssetDatabase.SaveAssets();
                    }
                }
            }
            if(refreshDatabase) { RefreshUISoundsDatabase(); }
        }
        private static void ValidateCanvasNamesDatabase()
        {
            bool refreshDatabase = false;
            if(CanvasNamesDatabase == null) { RefreshCanvasNamesDatabase(); return; }
            CanvasNamesDatabase.RemoveEmpty();
            CanvasNamesDatabase.Sort();
            if(!CanvasNamesDatabase.Contains(MASTER_CANVAS_NAME)) //the default canvas name is missing -> add it
            {
                refreshDatabase = true;
                AddName(CanvasNamesDatabase, MASTER_CANVAS_NAME);
            }
            if(refreshDatabase) { RefreshCanvasNamesDatabase(); }
        }

#region UI built-in sprites
        private const string kStandardSpritePath = "UI/Skin/UISprite.psd";
        private const string kBackgroundSpriteResourcePath = "UI/Skin/Background.psd";
        private const string kInputFieldBackgroundPath = "UI/Skin/InputFieldBackground.psd";
        private const string kKnobPath = "UI/Skin/Knob.psd";
        private const string kCheckmarkPath = "UI/Skin/Checkmark.psd";

        public static Sprite UISprite { get { return AssetDatabase.GetBuiltinExtraResource<Sprite>(kStandardSpritePath); ; } }
        public static Sprite Background { get { return AssetDatabase.GetBuiltinExtraResource<Sprite>(kBackgroundSpriteResourcePath); ; } }
        public static Sprite FieldBackground { get { return AssetDatabase.GetBuiltinExtraResource<Sprite>(kInputFieldBackgroundPath); ; } }
        public static Sprite Knob { get { return AssetDatabase.GetBuiltinExtraResource<Sprite>(kKnobPath); ; } }
        public static Sprite Checkmark { get { return AssetDatabase.GetBuiltinExtraResource<Sprite>(kCheckmarkPath); ; } }
#endregion
#endif
    }
}
