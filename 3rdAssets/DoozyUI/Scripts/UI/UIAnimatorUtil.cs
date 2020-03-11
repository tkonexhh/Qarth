// Copyright (c) 2015 - 2018 Doozy Entertainment / Marlink Trading SRL. All Rights Reserved.
// This code can only be used under the standard Unity Asset Store End User License Agreement
// A Copy of the EULA APPENDIX 1 is available at http://unity3d.com/company/legal/as_terms

using System.Collections.Generic;
using UnityEngine;
using System;
using QuickEngine.Core;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace DoozyUI
{
    [Serializable]
    public class UIAnimatorUtil
    {
        public const string UNCATEGORIZED_CATEGORY_NAME = "Uncategorized";
        public const string DEFAULT_PRESET_NAME = "DefaultPreset";

        public const string FOLDER_NAME_IN = "In/";
        public const string FOLDER_NAME_OUT = "Out/";
        public const string FOLDER_NAME_STATE = "State/";
        public const string FOLDER_NAME_LOOP = "Loop/";
        public const string FOLDER_NAME_PUNCH = "Punch/";

        public const string RESOURCES_PATH_ANIMATIONS = "DUI/Animations/";
        public const string RESOURCES_PATH_IN_ANIM_DATA = RESOURCES_PATH_ANIMATIONS + FOLDER_NAME_IN;
        public const string RESOURCES_PATH_OUT_ANIM_DATA = RESOURCES_PATH_ANIMATIONS + FOLDER_NAME_OUT;
        public const string RESOURCES_PATH_STATE_ANIM_DATA = RESOURCES_PATH_ANIMATIONS + FOLDER_NAME_STATE;
        public const string RESOURCES_PATH_LOOP_DATA = RESOURCES_PATH_ANIMATIONS + FOLDER_NAME_LOOP;
        public const string RESOURCES_PATH_PUNCH_DATA = RESOURCES_PATH_ANIMATIONS + FOLDER_NAME_PUNCH;

        public static string RELATIVE_PATH_ANIMATIONS { get { return DUI.PATH + "/Resources/DUI/Animations/"; } }
        public static string RELATIVE_PATH_IN_ANIM_DATA { get { return RELATIVE_PATH_ANIMATIONS + FOLDER_NAME_IN; } }
        public static string RELATIVE_PATH_OUT_ANIM_DATA { get { return RELATIVE_PATH_ANIMATIONS + FOLDER_NAME_OUT; } }
        public static string RELATIVE_PATH_STATE_ANIM_DATA { get { return RELATIVE_PATH_ANIMATIONS + FOLDER_NAME_STATE; } }
        public static string RELATIVE_PATH_LOOP_DATA { get { return RELATIVE_PATH_ANIMATIONS + FOLDER_NAME_LOOP; } }
        public static string RELATIVE_PATH_PUNCH_DATA { get { return RELATIVE_PATH_ANIMATIONS + FOLDER_NAME_PUNCH; } }

        private static string[] GetInAnimPresetsDirectories { get { return QuickEngine.IO.File.GetDirectoriesNames(RELATIVE_PATH_IN_ANIM_DATA); } }
        private static string[] GetOutAnimPresetsDirectories { get { return QuickEngine.IO.File.GetDirectoriesNames(RELATIVE_PATH_OUT_ANIM_DATA); } }
        private static string[] GetLoopPresetsDirectories { get { return QuickEngine.IO.File.GetDirectoriesNames(RELATIVE_PATH_LOOP_DATA); } }
        private static string[] GetPunchPresetsDirectories { get { return QuickEngine.IO.File.GetDirectoriesNames(RELATIVE_PATH_PUNCH_DATA); } }

        private static string[] GetInAnimPresetsNamesForCategory(string presetCategory) { return QuickEngine.IO.File.GetFilesNames(RELATIVE_PATH_IN_ANIM_DATA + presetCategory + "/", "asset"); }
        private static string[] GetOutAnimPresetsNamesForCategory(string presetCategory) { return QuickEngine.IO.File.GetFilesNames(RELATIVE_PATH_OUT_ANIM_DATA + presetCategory + "/", "asset"); }
        private static string[] GetLoopPresetsNamesForCategory(string presetCategory) { return QuickEngine.IO.File.GetFilesNames(RELATIVE_PATH_LOOP_DATA + presetCategory + "/", "asset"); }
        private static string[] GetPunchPresetsNamesForCategory(string presetCategory) { return QuickEngine.IO.File.GetFilesNames(RELATIVE_PATH_PUNCH_DATA + presetCategory + "/", "asset"); }

        public static T GetResource<T>(string resourcesPath, string fileName) where T : ScriptableObject
        {
            return (T)Resources.Load(resourcesPath + fileName, typeof(T));
        }

        public static Anim GetInAnim(string presetCategory, string presetName)
        {
            return Q.GetResource<AnimData>(RESOURCES_PATH_IN_ANIM_DATA + presetCategory + "/", presetName).data.Copy();
        }
        public static Anim GetOutAnim(string presetCategory, string presetName)
        {
            return Q.GetResource<AnimData>(RESOURCES_PATH_OUT_ANIM_DATA + presetCategory + "/", presetName).data.Copy();
        }
        public static Anim GetStateAnim(string presetCategory, string presetName)
        {
            return Q.GetResource<AnimData>(RESOURCES_PATH_STATE_ANIM_DATA + presetCategory + "/", presetName).data.Copy();
        }
        public static Loop GetLoop(string presetCategory, string presetName)
        {
            return Q.GetResource<LoopData>(RESOURCES_PATH_LOOP_DATA + presetCategory + "/", presetName).data.Copy();
        }
        public static Punch GetPunch(string presetCategory, string presetName)
        {
            return Q.GetResource<PunchData>(RESOURCES_PATH_PUNCH_DATA + presetCategory + "/", presetName).data.Copy();
        }

#if UNITY_EDITOR
        public static Dictionary<string, List<AnimData>> InAnimDataPresetsDatabase;
        public static Dictionary<string, List<AnimData>> OutAnimDataPresetsDatabase;
        public static Dictionary<string, List<LoopData>> LoopDataPresetsDatabase;
        public static Dictionary<string, List<PunchData>> PunchDataPresetsDatabase;

        private static List<string> directories;
        private static string[] fileNames;

        private static int count;
        private static int len;


        public static void RefreshInAnimDataPresetsDatabase()
        {
            EditorUtility.DisplayProgressBar("Refreshing In Animations Database", "", 0f);
            if (InAnimDataPresetsDatabase == null) { InAnimDataPresetsDatabase = new Dictionary<string, List<AnimData>>(); }
            InAnimDataPresetsDatabase.Clear();
            if (directories == null) { directories = new List<string>(); }
            directories.Clear();
            directories = RemoveEmptyPresetFolders(RELATIVE_PATH_IN_ANIM_DATA, GetInAnimPresetsDirectories);
            count = directories != null ? directories.Count : 0;
            for (int directoryIndex = 0; directoryIndex < count; directoryIndex++)
            {
                EditorUtility.DisplayProgressBar("Refreshing In Animations Database", directories[directoryIndex], ((directoryIndex + 1) / (count + 2)));
                fileNames = GetInAnimPresetsNamesForCategory(directories[directoryIndex]);
                len = fileNames != null ? fileNames.Length : 0;
                if (len == 0) { continue; } //empty folder
                InAnimDataPresetsDatabase.Add(directories[directoryIndex], new List<AnimData>());
                for (int fileIndex = 0; fileIndex < len; fileIndex++)
                {
                    AnimData asset = GetResource<AnimData>(RESOURCES_PATH_IN_ANIM_DATA + directories[directoryIndex] + "/", fileNames[fileIndex]);
                    if (asset == null) { continue; }
                    EditorUtility.DisplayProgressBar("Refreshing In Animations Database", directories[directoryIndex] + " / " + asset.presetName, ((directoryIndex + 1) / (count + 2)));
                    InAnimDataPresetsDatabase[directories[directoryIndex]].Add(asset);
                }
            }
            EditorUtility.DisplayProgressBar("Refreshing In Animations Database", "Creating Categories List...", 0.9f);
            RefreshInAnimPresetCategories();
            EditorUtility.DisplayProgressBar("Refreshing In Animations Database", "Validating...", 1f);
            ValidateInAnimPresets();
            EditorUtility.ClearProgressBar();
        }
        public static void RefreshOutAnimDataPresetsDatabase()
        {
            EditorUtility.DisplayProgressBar("Refreshing Out Animations Database", "", 0f);
            if (OutAnimDataPresetsDatabase == null) { OutAnimDataPresetsDatabase = new Dictionary<string, List<AnimData>>(); }
            OutAnimDataPresetsDatabase.Clear();
            if (directories == null) { directories = new List<string>(); }
            directories.Clear();
            directories = RemoveEmptyPresetFolders(RELATIVE_PATH_OUT_ANIM_DATA, GetOutAnimPresetsDirectories);
            count = directories != null ? directories.Count : 0;
            for (int directoryIndex = 0; directoryIndex < count; directoryIndex++)
            {
                EditorUtility.DisplayProgressBar("Refreshing Out Animations Database", directories[directoryIndex], ((directoryIndex + 1) / (count + 2)));
                fileNames = GetOutAnimPresetsNamesForCategory(directories[directoryIndex]);
                len = fileNames != null ? fileNames.Length : 0;
                if (len == 0) { continue; } //empty folder
                OutAnimDataPresetsDatabase.Add(directories[directoryIndex], new List<AnimData>());
                for (int fileIndex = 0; fileIndex < len; fileIndex++)
                {
                    AnimData asset = GetResource<AnimData>(RESOURCES_PATH_OUT_ANIM_DATA + directories[directoryIndex] + "/", fileNames[fileIndex]);
                    if (asset == null) { continue; }
                    EditorUtility.DisplayProgressBar("Refreshing Out Animations Database", directories[directoryIndex] + " / " + asset.presetName, ((directoryIndex + 1) / (count + 2)));
                    OutAnimDataPresetsDatabase[directories[directoryIndex]].Add(asset);
                }
            }
            EditorUtility.DisplayProgressBar("Refreshing Out Animations Database", "Creating Categories List...", 0.9f);
            RefreshOutAnimPresetCategories();
            EditorUtility.DisplayProgressBar("Refreshing Out Animations Database", "Validating...", 1f);
            ValidateOutAnimPresets();
            EditorUtility.ClearProgressBar();
        }
        public static void RefreshLoopDataPresetsDatabase()
        {
            EditorUtility.DisplayProgressBar("Refreshing Loop Animations Database", "", 0f);
            if (LoopDataPresetsDatabase == null) { LoopDataPresetsDatabase = new Dictionary<string, List<LoopData>>(); }
            LoopDataPresetsDatabase.Clear();
            if (directories == null) { directories = new List<string>(); }
            directories.Clear();
            directories = RemoveEmptyPresetFolders(RELATIVE_PATH_LOOP_DATA, GetLoopPresetsDirectories);
            count = directories != null ? directories.Count : 0;
            for (int directoryIndex = 0; directoryIndex < count; directoryIndex++)
            {
                EditorUtility.DisplayProgressBar("Refreshing Loop Animations Database", directories[directoryIndex], ((directoryIndex + 1) / (count + 2)));
                fileNames = GetLoopPresetsNamesForCategory(directories[directoryIndex]);
                len = fileNames != null ? fileNames.Length : 0;
                if (len == 0) { continue; } //empty folder
                LoopDataPresetsDatabase.Add(directories[directoryIndex], new List<LoopData>());
                for (int fileIndex = 0; fileIndex < len; fileIndex++)
                {
                    LoopData asset = GetResource<LoopData>(RESOURCES_PATH_LOOP_DATA + directories[directoryIndex] + "/", fileNames[fileIndex]);
                    if (asset == null) { continue; }
                    EditorUtility.DisplayProgressBar("Refreshing Loop Animations Database", directories[directoryIndex] + " / " + asset.presetName, ((directoryIndex + 1) / (count + 2)));
                    LoopDataPresetsDatabase[directories[directoryIndex]].Add(asset);
                }
            }
            EditorUtility.DisplayProgressBar("Refreshing Loop Animations Database", "Creating Categories List...", 0.9f);
            RefreshLoopPresetCategories();
            EditorUtility.DisplayProgressBar("Refreshing Loop Animations Database", "Validating...", 1f);
            ValidateLoopPresets();
            EditorUtility.ClearProgressBar();
        }
        public static void RefreshPunchDataPresetsDatabase()
        {
            EditorUtility.DisplayProgressBar("Refreshing Punch Animations Database", "", 0f);
            if (PunchDataPresetsDatabase == null) { PunchDataPresetsDatabase = new Dictionary<string, List<PunchData>>(); }
            PunchDataPresetsDatabase.Clear();
            if (directories == null) { directories = new List<string>(); }
            directories.Clear();
            directories = RemoveEmptyPresetFolders(RELATIVE_PATH_PUNCH_DATA, GetPunchPresetsDirectories);
            count = directories != null ? directories.Count : 0;
            for (int directoryIndex = 0; directoryIndex < count; directoryIndex++)
            {
                EditorUtility.DisplayProgressBar("Refreshing Punch Animations Database", directories[directoryIndex], ((directoryIndex + 1) / (count + 2)));
                fileNames = GetPunchPresetsNamesForCategory(directories[directoryIndex]);
                len = fileNames != null ? fileNames.Length : 0;
                if (len == 0) { continue; } //empty folder
                PunchDataPresetsDatabase.Add(directories[directoryIndex], new List<PunchData>());
                for (int fileIndex = 0; fileIndex < len; fileIndex++)
                {
                    PunchData asset = GetResource<PunchData>(RESOURCES_PATH_PUNCH_DATA + directories[directoryIndex] + "/", fileNames[fileIndex]);
                    if (asset == null) { continue; }
                    EditorUtility.DisplayProgressBar("Refreshing Punch Animations Database", directories[directoryIndex] + " / " + asset.presetName, ((directoryIndex + 1) / (count + 2)));
                    PunchDataPresetsDatabase[directories[directoryIndex]].Add(asset);
                }
            }
            EditorUtility.DisplayProgressBar("Refreshing Punch Animations Database", "Creating Categories List...", 0.9f);
            RefreshPunchPresetCategories();
            EditorUtility.DisplayProgressBar("Refreshing Punch Animations Database", "Validating...", 1f);
            ValidatePunchPresets();
            EditorUtility.ClearProgressBar();
        }

        private static List<string> m_InAnimPresetCategories;
        public static List<string> InAnimPresetCategories { get { if (m_InAnimPresetCategories == null) { RefreshInAnimPresetCategories(); } return m_InAnimPresetCategories; } }
        private static void RefreshInAnimPresetCategories()
        {
            if (m_InAnimPresetCategories == null) { m_InAnimPresetCategories = new List<string>(); }
            m_InAnimPresetCategories.Clear();
            m_InAnimPresetCategories.AddRange(GetInAnimPresetCategories());
        }
        private static List<string> m_OutAnimPresetCategories;
        public static List<string> OutAnimPresetCategories { get { if (m_OutAnimPresetCategories == null) { RefreshOutAnimPresetCategories(); } return m_OutAnimPresetCategories; } }
        private static void RefreshOutAnimPresetCategories()
        {
            if (m_OutAnimPresetCategories == null) { m_OutAnimPresetCategories = new List<string>(); }
            m_OutAnimPresetCategories.Clear();
            m_OutAnimPresetCategories.AddRange(GetOutAnimPresetCategories());
        }
        private static List<string> m_LoopPresetCategories;
        public static List<string> LoopPresetCategories { get { if (m_LoopPresetCategories == null) { RefreshLoopPresetCategories(); } return m_LoopPresetCategories; } }
        private static void RefreshLoopPresetCategories()
        {
            if (m_LoopPresetCategories == null) { m_LoopPresetCategories = new List<string>(); }
            m_LoopPresetCategories.Clear();
            m_LoopPresetCategories.AddRange(GetLoopPresetCategories());
        }
        private static List<string> m_PunchPresetCategories;
        public static List<string> PunchPresetCategories { get { if (m_PunchPresetCategories == null) { RefreshPunchPresetCategories(); } return m_PunchPresetCategories; } }
        private static void RefreshPunchPresetCategories()
        {
            if (m_PunchPresetCategories == null) { m_PunchPresetCategories = new List<string>(); }
            m_PunchPresetCategories.Clear();
            m_PunchPresetCategories.AddRange(GetPunchPresetCategories());
        }

        private static List<string> GetInAnimPresetCategories()
        {
            if (InAnimDataPresetsDatabase == null) { RefreshInAnimDataPresetsDatabase(); }
            return new List<string>(InAnimDataPresetsDatabase.Keys);
        }
        private static List<string> GetOutAnimPresetCategories()
        {
            if (OutAnimDataPresetsDatabase == null) { RefreshOutAnimDataPresetsDatabase(); }
            return new List<string>(OutAnimDataPresetsDatabase.Keys);
        }
        private static List<string> GetLoopPresetCategories()
        {
            if (LoopDataPresetsDatabase == null) { RefreshLoopDataPresetsDatabase(); }
            return new List<string>(LoopDataPresetsDatabase.Keys);
        }
        private static List<string> GetPunchPresetCategories()
        {
            if (PunchDataPresetsDatabase == null) { RefreshPunchDataPresetsDatabase(); }
            return new List<string>(PunchDataPresetsDatabase.Keys);
        }

        public static List<string> GetInAnimPresetNames(string presetCategory)
        {
            if (InAnimDataPresetsDatabase == null) { RefreshInAnimDataPresetsDatabase(); }
            if (!InAnimPresetCategoryExists(presetCategory)) { return null; }
            List<string> presetNames = new List<string>();
            foreach (var presetData in InAnimDataPresetsDatabase[presetCategory]) { presetNames.Add(presetData.presetName); }
            return presetNames;
        }
        public static List<string> GetOutAnimPresetNames(string presetCategory)
        {
            if (OutAnimDataPresetsDatabase == null) { RefreshOutAnimDataPresetsDatabase(); }
            if (!OutAnimPresetCategoryExists(presetCategory)) { return null; }
            List<string> presetNames = new List<string>();
            foreach (var presetData in OutAnimDataPresetsDatabase[presetCategory]) { presetNames.Add(presetData.presetName); }
            return presetNames;
        }
        public static List<string> GetLoopPresetNames(string presetCategory)
        {
            if (LoopDataPresetsDatabase == null) { RefreshLoopDataPresetsDatabase(); }
            if (!LoopPresetCategoryExists(presetCategory)) { return null; }
            List<string> presetNames = new List<string>();
            foreach (var presetData in LoopDataPresetsDatabase[presetCategory]) { presetNames.Add(presetData.presetName); }
            return presetNames;
        }
        public static List<string> GetPunchPresetNames(string presetCategory)
        {
            if (PunchDataPresetsDatabase == null) { RefreshPunchDataPresetsDatabase(); }
            if (!PunchPresetCategoryExists(presetCategory)) { return null; }
            List<string> presetNames = new List<string>();
            foreach (var presetData in PunchDataPresetsDatabase[presetCategory]) { presetNames.Add(presetData.presetName); }
            return presetNames;
        }

        public static bool InAnimPresetCategoryExists(string presetCategory)
        {
            return InAnimPresetCategories.Contains(presetCategory);
        }
        public static bool OutAnimPresetCategoryExists(string presetCategory)
        {
            return OutAnimPresetCategories.Contains(presetCategory);
        }
        public static bool LoopPresetCategoryExists(string presetCategory)
        {
            return LoopPresetCategories.Contains(presetCategory);
        }
        public static bool PunchPresetCategoryExists(string presetCategory)
        {
            return PunchPresetCategories.Contains(presetCategory);
        }

        public static bool InAnimPresetExists(string presetCategory, string presetName)
        {
            if (!InAnimPresetCategoryExists(presetCategory)) { return false; }
            return GetInAnimPresetNames(presetCategory).Contains(presetName);
        }
        public static bool OutAnimPresetExists(string presetCategory, string presetName)
        {
            if (!OutAnimPresetCategoryExists(presetCategory)) { return false; }
            return GetOutAnimPresetNames(presetCategory).Contains(presetName);
        }
        public static bool LoopPresetExists(string presetCategory, string presetName)
        {
            if (!LoopPresetCategoryExists(presetCategory)) { return false; }
            return GetLoopPresetNames(presetCategory).Contains(presetName);
        }
        public static bool PunchPresetExists(string presetCategory, string presetName)
        {
            if (!PunchPresetCategoryExists(presetCategory)) { return false; }
            return GetPunchPresetNames(presetCategory).Contains(presetName);
        }

        public static AnimData GetInAnimData(string presetCategory, string presetName)
        {
            return GetResource<AnimData>(RESOURCES_PATH_IN_ANIM_DATA + presetCategory + "/", presetName);
        }
        public static AnimData GetOutAnimData(string presetCategory, string presetName)
        {
            return GetResource<AnimData>(RESOURCES_PATH_OUT_ANIM_DATA + presetCategory + "/", presetName);
        }
        public static LoopData GetLoopData(string presetCategory, string presetName)
        {
            return GetResource<LoopData>(RESOURCES_PATH_LOOP_DATA + presetCategory + "/", presetName);
        }
        public static PunchData GetPunchData(string presetCategory, string presetName)
        {
            return GetResource<PunchData>(RESOURCES_PATH_PUNCH_DATA + presetCategory + "/", presetName);
        }

        private static AnimData CreateAnimDataAsset(string relativePath, string presetCategory, string presetName, Anim anim)
        {
            AnimData asset = ScriptableObject.CreateInstance<AnimData>();
            asset.presetName = presetName;
            asset.presetCategory = presetCategory;
            asset.data = anim;
            if (!QuickEngine.IO.File.Exists(relativePath + presetCategory + "/"))
            {
                QuickEngine.IO.File.CreateDirectory(relativePath + presetCategory + "/");
            }
            AssetDatabase.CreateAsset(asset, relativePath + presetCategory + "/" + presetName + ".asset");
            EditorUtility.SetDirty(asset);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            return asset;
        }
        private static LoopData CreateLoopDataAsset(string relativePath, string presetCategory, string presetName, Loop loop)
        {
            LoopData asset = ScriptableObject.CreateInstance<LoopData>();
            asset.presetName = presetName;
            asset.presetCategory = presetCategory;
            asset.data = loop;
            if (!QuickEngine.IO.File.Exists(relativePath + presetCategory + "/"))
            {
                QuickEngine.IO.File.CreateDirectory(relativePath + presetCategory + "/");
            }
            AssetDatabase.CreateAsset(asset, relativePath + presetCategory + "/" + presetName + ".asset");
            EditorUtility.SetDirty(asset);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            return asset;
        }
        private static PunchData CreatePunchDataAsset(string relativePath, string presetCategory, string presetName, Punch punch)
        {
            PunchData asset = ScriptableObject.CreateInstance<PunchData>();
            asset.presetName = presetName;
            asset.presetCategory = presetCategory;
            asset.data = punch;
            if (!QuickEngine.IO.File.Exists(relativePath + presetCategory + "/"))
            {
                QuickEngine.IO.File.CreateDirectory(relativePath + presetCategory + "/");
            }
            AssetDatabase.CreateAsset(asset, relativePath + presetCategory + "/" + presetName + ".asset");
            EditorUtility.SetDirty(asset);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            return asset;
        }

        public static AnimData CreateInAnimPreset(string presetCategory, string presetName, Anim anim)
        {
            return CreateAnimDataAsset(RELATIVE_PATH_IN_ANIM_DATA, presetCategory, presetName, anim);
        }
        public static AnimData CreateOutAnimPreset(string presetCategory, string presetName, Anim anim)
        {
            return CreateAnimDataAsset(RELATIVE_PATH_OUT_ANIM_DATA, presetCategory, presetName, anim);
        }
        public static AnimData CreateStateAnimPreset(string presetCategory, string presetName, Anim anim)
        {
            return CreateAnimDataAsset(RELATIVE_PATH_STATE_ANIM_DATA, presetCategory, presetName, anim);
        }
        public static LoopData CreateLoopPreset(string presetCategory, string presetName, Loop loop)
        {
            return CreateLoopDataAsset(RELATIVE_PATH_LOOP_DATA, presetCategory, presetName, loop);
        }
        public static PunchData CreatePunchPreset(string presetCategory, string presetName, Punch punch)
        {
            return CreatePunchDataAsset(RELATIVE_PATH_PUNCH_DATA, presetCategory, presetName, punch);
        }

        public static void RenameInAnimPreset(string oldName, string newName, string presetCategory)
        {
            if (string.IsNullOrEmpty(oldName) || string.IsNullOrEmpty(newName) || oldName.Equals(newName)) { return; }
            if (!InAnimPresetExists(presetCategory, oldName)) { return; }
            if (InAnimPresetExists(presetCategory, newName)) { return; }
            AssetDatabase.RenameAsset(RELATIVE_PATH_IN_ANIM_DATA + presetCategory + "/" + oldName, newName);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            RefreshInAnimDataPresetsDatabase();
        }
        public static void RenameOutAnimPreset(string oldName, string newName, string presetCategory)
        {
            if (string.IsNullOrEmpty(oldName) || string.IsNullOrEmpty(newName) || oldName.Equals(newName)) { return; }
            if (!OutAnimPresetExists(presetCategory, oldName)) { return; }
            if (OutAnimPresetExists(presetCategory, newName)) { return; }
            AssetDatabase.RenameAsset(RELATIVE_PATH_OUT_ANIM_DATA + presetCategory + "/" + oldName, newName);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            RefreshOutAnimDataPresetsDatabase();
        }
        public static void RenameLoopPreset(string oldName, string newName, string presetCategory)
        {
            if (string.IsNullOrEmpty(oldName) || string.IsNullOrEmpty(newName) || oldName.Equals(newName)) { return; }
            if (!LoopPresetExists(presetCategory, oldName)) { return; }
            if (LoopPresetExists(presetCategory, newName)) { return; }
            AssetDatabase.RenameAsset(RELATIVE_PATH_LOOP_DATA + presetCategory + "/" + oldName, newName);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            RefreshLoopDataPresetsDatabase();
        }
        public static void RenamePunchPreset(string oldName, string newName, string presetCategory)
        {
            if (string.IsNullOrEmpty(oldName) || string.IsNullOrEmpty(newName) || oldName.Equals(newName)) { return; }
            if (!PunchPresetExists(presetCategory, oldName)) { return; }
            if (PunchPresetExists(presetCategory, newName)) { return; }
            AssetDatabase.RenameAsset(RELATIVE_PATH_PUNCH_DATA + presetCategory + "/" + oldName, newName);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            RefreshPunchDataPresetsDatabase();
        }

        public static void DeleteInAnimPreset(string presetCategory, string presetName)
        {
            if (string.IsNullOrEmpty(presetName)) { return; }
            if (!InAnimPresetExists(presetCategory, presetName)) { return; }
            if (AssetDatabase.MoveAssetToTrash(RELATIVE_PATH_IN_ANIM_DATA + presetCategory + "/" + presetName + ".asset"))
            {
                Debug.Log("[DoozyUI] The '" + presetName + "' In Animations preset asset file has been moved to trash.");
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
                RefreshInAnimDataPresetsDatabase();
            }
        }
        public static void DeleteOutAnimPreset(string presetCategory, string presetName)
        {
            if (string.IsNullOrEmpty(presetName)) { return; }
            if (!OutAnimPresetExists(presetCategory, presetName)) { return; }
            if (AssetDatabase.MoveAssetToTrash(RELATIVE_PATH_OUT_ANIM_DATA + presetCategory + "/" + presetName + ".asset"))
            {
                Debug.Log("[DoozyUI] The '" + presetName + "' Out Animations preset asset file has been moved to trash.");
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
                RefreshOutAnimDataPresetsDatabase();
            }
        }
        public static void DeleteLoopPreset(string presetCategory, string presetName)
        {
            if (string.IsNullOrEmpty(presetName)) { return; }
            if (!LoopPresetExists(presetCategory, presetName)) { return; }
            if (AssetDatabase.MoveAssetToTrash(RELATIVE_PATH_LOOP_DATA + presetCategory + "/" + presetName + ".asset"))
            {
                Debug.Log("[DoozyUI] The '" + presetName + "' Loop Animations preset asset file has been moved to trash.");
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
                RefreshLoopDataPresetsDatabase();
            }
        }
        public static void DeletePunchPreset(string presetCategory, string presetName)
        {
            if (string.IsNullOrEmpty(presetName)) { return; }
            if (!PunchPresetExists(presetCategory, presetName)) { return; }
            if (AssetDatabase.MoveAssetToTrash(RELATIVE_PATH_PUNCH_DATA + presetCategory + "/" + presetName + ".asset"))
            {
                Debug.Log("[DoozyUI] The '" + presetName + "' Punch Animations preset asset file has been moved to trash.");
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
                RefreshPunchDataPresetsDatabase();
            }
        }

        public static void DeleteInAnimCategory(string presetCategory)
        {
            if (string.IsNullOrEmpty(presetCategory)) { return; }
            if (!InAnimPresetCategoryExists(presetCategory)) { return; }
            foreach (var preset in InAnimDataPresetsDatabase[presetCategory])
            {
                AssetDatabase.MoveAssetToTrash(RELATIVE_PATH_IN_ANIM_DATA + presetCategory + "/" + preset.presetName + ".asset");
            }
            FileUtil.DeleteFileOrDirectory(RELATIVE_PATH_IN_ANIM_DATA + presetCategory);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            RefreshInAnimDataPresetsDatabase();
        }
        public static void DeleteOutAnimCategory(string presetCategory)
        {
            if (string.IsNullOrEmpty(presetCategory)) { return; }
            if (!OutAnimPresetCategoryExists(presetCategory)) { return; }
            foreach (var preset in OutAnimDataPresetsDatabase[presetCategory])
            {
                AssetDatabase.MoveAssetToTrash(RELATIVE_PATH_OUT_ANIM_DATA + presetCategory + "/" + preset.presetName + ".asset");
            }
            FileUtil.DeleteFileOrDirectory(RELATIVE_PATH_OUT_ANIM_DATA + presetCategory);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            RefreshOutAnimDataPresetsDatabase();
        }
        public static void DeleteLoopCategory(string presetCategory)
        {
            if (string.IsNullOrEmpty(presetCategory)) { return; }
            if (!LoopPresetCategoryExists(presetCategory)) { return; }
            foreach (var preset in LoopDataPresetsDatabase[presetCategory])
            {
                AssetDatabase.MoveAssetToTrash(RELATIVE_PATH_LOOP_DATA + presetCategory + "/" + preset.presetName + ".asset");
            }
            FileUtil.DeleteFileOrDirectory(RELATIVE_PATH_LOOP_DATA + presetCategory);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            RefreshLoopDataPresetsDatabase();
        }
        public static void DeletePunchCategory(string presetCategory)
        {
            if (string.IsNullOrEmpty(presetCategory)) { return; }
            if (!PunchPresetCategoryExists(presetCategory)) { return; }
            foreach (var preset in PunchDataPresetsDatabase[presetCategory])
            {
                AssetDatabase.MoveAssetToTrash(RELATIVE_PATH_PUNCH_DATA + presetCategory + "/" + preset.presetName + ".asset");
            }
            FileUtil.DeleteFileOrDirectory(RELATIVE_PATH_PUNCH_DATA + presetCategory);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            RefreshPunchDataPresetsDatabase();
        }

        private static List<string> RemoveEmptyPresetFolders(string relativePath, string[] directories)
        {
            List<string> list = new List<string>();
            list.AddRange(directories);
            bool refreshAssetsDatabase = false;
            for (int i = list.Count - 1; i >= 0; i--)
            {
                if (QuickEngine.IO.File.GetFilesNames(relativePath + list[i] + "/", "asset").Length == 0) //this is an empty folder -> delete it
                {
                    FileUtil.DeleteFileOrDirectory(relativePath + list[i]);
                    list.RemoveAt(i);
                    refreshAssetsDatabase = true;
                }
            }
            if (refreshAssetsDatabase) { AssetDatabase.Refresh(); }
            return list;
        }

        /// <summary>
        /// Checks that there are not null presets (animation data), that the preset category is matched to the directory structure and that the preset name is matched to the file name
        /// </summary>
        private static void ValidateInAnimPresets()
        {
            bool refreshDatabase = false;
            bool assetDatabaseSaveAssets = false;
            bool assetDatabaseRefresh = false;
            directories = new List<string>();
            directories = RemoveEmptyPresetFolders(RELATIVE_PATH_IN_ANIM_DATA, GetInAnimPresetsDirectories);
            if (!directories.Contains(UNCATEGORIZED_CATEGORY_NAME) || !InAnimPresetExists(UNCATEGORIZED_CATEGORY_NAME, DEFAULT_PRESET_NAME)) //the default preset category does not exist in the database or the default preset name does not exist in the database-> create it
            {
                refreshDatabase = true;
                CreateInAnimPreset(UNCATEGORIZED_CATEGORY_NAME, DEFAULT_PRESET_NAME, new Anim(Anim.AnimationType.In));
            }
            for (int directoryIndex = 0; directoryIndex < directories.Count; directoryIndex++)
            {
                fileNames = GetInAnimPresetsNamesForCategory(directories[directoryIndex]);
                if (fileNames.Length == 0) { continue; } //empty folder
                for (int fileIndex = 0; fileIndex < fileNames.Length; fileIndex++)
                {
                    AnimData asset = GetResource<AnimData>(RESOURCES_PATH_IN_ANIM_DATA + directories[directoryIndex] + "/", fileNames[fileIndex]);
                    if (asset == null) { continue; }
                    if (asset.data == null) //preset data is null (this is the animation data) -> this should not happen -> delete the preset to avoid corruption
                    {
                        refreshDatabase = true;
                        if (AssetDatabase.MoveAssetToTrash(RELATIVE_PATH_IN_ANIM_DATA + directories[directoryIndex] + "/" + fileNames[fileIndex] + ".asset")) //move asset file to trash
                        {
                            assetDatabaseSaveAssets = true;
                            assetDatabaseRefresh = true;
                        }
                    }
                    if (string.IsNullOrEmpty(asset.presetName) || !asset.presetName.Equals(fileNames[fileIndex])) //preset name is empty or preset name does not match the file name -> set the preset name as the file name
                    {
                        refreshDatabase = true;
                        asset.presetName = fileNames[fileIndex];
                        assetDatabaseSaveAssets = true;
                    }
                    if (string.IsNullOrEmpty(asset.presetCategory) || !asset.presetCategory.Equals(directories[directoryIndex])) //preset category is empty or preset category does not match the directory name-> set the preset category as the directory name 
                    {
                        refreshDatabase = true;
                        asset.presetCategory = directories[directoryIndex];
                        assetDatabaseSaveAssets = true;
                    }
                }
            }
            if (assetDatabaseSaveAssets) { AssetDatabase.SaveAssets(); }
            if (assetDatabaseRefresh) { AssetDatabase.Refresh(); }
            if (refreshDatabase) { RefreshInAnimDataPresetsDatabase(); }
        }
        /// <summary>
        /// Checks that there are not null presets (animation data), that the preset category is matched to the directory structure and that the preset name is matched to the file name
        /// </summary>
        private static void ValidateOutAnimPresets()
        {
            bool refreshDatabase = false;
            bool assetDatabaseSaveAssets = false;
            bool assetDatabaseRefresh = false;
            directories = new List<string>();
            directories = RemoveEmptyPresetFolders(RELATIVE_PATH_OUT_ANIM_DATA, GetOutAnimPresetsDirectories);
            if (!directories.Contains(UNCATEGORIZED_CATEGORY_NAME) || !OutAnimPresetExists(UNCATEGORIZED_CATEGORY_NAME, DEFAULT_PRESET_NAME)) //the default preset category does not exist in the database or the default preset name does not exist in the database-> create it
            {
                refreshDatabase = true;
                CreateOutAnimPreset(UNCATEGORIZED_CATEGORY_NAME, DEFAULT_PRESET_NAME, new Anim(Anim.AnimationType.Out));
            }
            for (int directoryIndex = 0; directoryIndex < directories.Count; directoryIndex++)
            {
                fileNames = GetOutAnimPresetsNamesForCategory(directories[directoryIndex]);
                if (fileNames.Length == 0) { continue; } //empty folder
                for (int fileIndex = 0; fileIndex < fileNames.Length; fileIndex++)
                {
                    AnimData asset = GetResource<AnimData>(RESOURCES_PATH_OUT_ANIM_DATA + directories[directoryIndex] + "/", fileNames[fileIndex]);
                    if (asset == null) { continue; }
                    if (asset.data == null) //preset data is null (this is the animation data) -> this should not happen -> delete the preset to avoid corruption
                    {
                        refreshDatabase = true;
                        if (AssetDatabase.MoveAssetToTrash(RELATIVE_PATH_OUT_ANIM_DATA + directories[directoryIndex] + "/" + fileNames[fileIndex] + ".asset")) //move asset file to trash
                        {
                            assetDatabaseSaveAssets = true;
                            assetDatabaseRefresh = true;
                        }
                    }
                    if (string.IsNullOrEmpty(asset.presetName) || !asset.presetName.Equals(fileNames[fileIndex])) //preset name is empty or preset name does not match the file name -> set the preset name as the file name
                    {
                        refreshDatabase = true;
                        asset.presetName = fileNames[fileIndex];
                        assetDatabaseSaveAssets = true;
                    }
                    if (string.IsNullOrEmpty(asset.presetCategory) || !asset.presetCategory.Equals(directories[directoryIndex])) //preset category is empty or preset category does not match the directory name-> set the preset category as the directory name 
                    {
                        refreshDatabase = true;
                        asset.presetCategory = directories[directoryIndex];
                        assetDatabaseSaveAssets = true;
                    }
                }
            }
            if (assetDatabaseSaveAssets) { AssetDatabase.SaveAssets(); }
            if (assetDatabaseRefresh) { AssetDatabase.Refresh(); }
            if (refreshDatabase) { RefreshOutAnimDataPresetsDatabase(); }
        }
        /// <summary>
        /// Checks that there are not null presets (animation data), that the preset category is matched to the directory structure and that the preset name is matched to the file name
        /// </summary>
        private static void ValidateLoopPresets()
        {
            bool refreshDatabase = false;
            bool assetDatabaseSaveAssets = false;
            bool assetDatabaseRefresh = false;
            directories = new List<string>();
            directories = RemoveEmptyPresetFolders(RELATIVE_PATH_LOOP_DATA, GetLoopPresetsDirectories);
            if (!directories.Contains(UNCATEGORIZED_CATEGORY_NAME) || !LoopPresetExists(UNCATEGORIZED_CATEGORY_NAME, DEFAULT_PRESET_NAME)) //the default preset category does not exist in the database or the default preset name does not exist in the database-> create it
            {
                refreshDatabase = true;
                CreateLoopPreset(UNCATEGORIZED_CATEGORY_NAME, DEFAULT_PRESET_NAME, new Loop());
            }
            for (int directoryIndex = 0; directoryIndex < directories.Count; directoryIndex++)
            {
                fileNames = GetLoopPresetsNamesForCategory(directories[directoryIndex]);
                if (fileNames.Length == 0) { continue; } //empty folder
                for (int fileIndex = 0; fileIndex < fileNames.Length; fileIndex++)
                {
                    AnimData asset = GetResource<AnimData>(RESOURCES_PATH_LOOP_DATA + directories[directoryIndex] + "/", fileNames[fileIndex]);
                    if (asset == null) { continue; }
                    if (asset.data == null) //preset data is null (this is the animation data) -> this should not happen -> delete the preset to avoid corruption
                    {
                        refreshDatabase = true;
                        if (AssetDatabase.MoveAssetToTrash(RELATIVE_PATH_LOOP_DATA + directories[directoryIndex] + "/" + fileNames[fileIndex] + ".asset")) //move asset file to trash
                        {
                            assetDatabaseSaveAssets = true;
                            assetDatabaseRefresh = true;
                        }
                    }
                    if (string.IsNullOrEmpty(asset.presetName) || !asset.presetName.Equals(fileNames[fileIndex])) //preset name is empty or preset name does not match the file name -> set the preset name as the file name
                    {
                        refreshDatabase = true;
                        asset.presetName = fileNames[fileIndex];
                        assetDatabaseSaveAssets = true;
                    }
                    if (string.IsNullOrEmpty(asset.presetCategory) || !asset.presetCategory.Equals(directories[directoryIndex])) //preset category is empty or preset category does not match the directory name-> set the preset category as the directory name 
                    {
                        refreshDatabase = true;
                        asset.presetCategory = directories[directoryIndex];
                        assetDatabaseSaveAssets = true;
                    }
                }
            }
            if (assetDatabaseSaveAssets) { AssetDatabase.SaveAssets(); }
            if (assetDatabaseRefresh) { AssetDatabase.Refresh(); }
            if (refreshDatabase) { RefreshLoopDataPresetsDatabase(); }
        }
        /// <summary>
        /// Checks that there are not null presets (animation data), that the preset category is matched to the directory structure and that the preset name is matched to the file name
        /// </summary>
        private static void ValidatePunchPresets()
        {
            bool refreshDatabase = false;
            bool assetDatabaseSaveAssets = false;
            bool assetDatabaseRefresh = false;
            directories = new List<string>();
            directories = RemoveEmptyPresetFolders(RELATIVE_PATH_PUNCH_DATA, GetPunchPresetsDirectories);
            if (!directories.Contains(UNCATEGORIZED_CATEGORY_NAME) || !PunchPresetExists(UNCATEGORIZED_CATEGORY_NAME, DEFAULT_PRESET_NAME)) //the default preset category does not exist in the database or the default preset name does not exist in the database-> create it
            {
                refreshDatabase = true;
                CreatePunchPreset(UNCATEGORIZED_CATEGORY_NAME, DEFAULT_PRESET_NAME, new Punch());
            }
            for (int directoryIndex = 0; directoryIndex < directories.Count; directoryIndex++)
            {
                fileNames = GetPunchPresetsNamesForCategory(directories[directoryIndex]);
                if (fileNames.Length == 0) { continue; } //empty folder
                for (int fileIndex = 0; fileIndex < fileNames.Length; fileIndex++)
                {
                    AnimData asset = GetResource<AnimData>(RESOURCES_PATH_PUNCH_DATA + directories[directoryIndex] + "/", fileNames[fileIndex]);
                    if (asset == null) { continue; }
                    if (asset.data == null) //preset data is null (this is the animation data) -> this should not happen -> delete the preset to avoid corruption
                    {
                        refreshDatabase = true;
                        if (AssetDatabase.MoveAssetToTrash(RELATIVE_PATH_PUNCH_DATA + directories[directoryIndex] + "/" + fileNames[fileIndex] + ".asset")) //move asset file to trash
                        {
                            assetDatabaseSaveAssets = true;
                            assetDatabaseRefresh = true;
                        }
                    }
                    if (string.IsNullOrEmpty(asset.presetName) || !asset.presetName.Equals(fileNames[fileIndex])) //preset name is empty or preset name does not match the file name -> set the preset name as the file name
                    {
                        refreshDatabase = true;
                        asset.presetName = fileNames[fileIndex];
                        assetDatabaseSaveAssets = true;
                    }
                    if (string.IsNullOrEmpty(asset.presetCategory) || !asset.presetCategory.Equals(directories[directoryIndex])) //preset category is empty or preset category does not match the directory name-> set the preset category as the directory name 
                    {
                        refreshDatabase = true;
                        asset.presetCategory = directories[directoryIndex];
                        assetDatabaseSaveAssets = true;
                    }
                }
            }
            if (assetDatabaseSaveAssets) { AssetDatabase.SaveAssets(); }
            if (assetDatabaseRefresh) { AssetDatabase.Refresh(); }
            if (refreshDatabase) { RefreshPunchDataPresetsDatabase(); }
        }
#endif
    }
}
