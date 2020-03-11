// Copyright (c) 2015 - 2018 Doozy Entertainment / Marlink Trading SRL. All Rights Reserved.
// This code can only be used under the standard Unity Asset Store End User License Agreement
// A Copy of the EULA APPENDIX 1 is available at http://unity3d.com/company/legal/as_terms

using QuickEditor;
using QuickEngine.Core;
using QuickEngine.Extensions;
using QuickEngine.IO;
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.AnimatedValues;
using UnityEngine;

namespace DoozyUI
{
    [Serializable]
    public class DUIData : ScriptableObject
    {
        private static DUIData _instance;
        public static DUIData Instance
        {
            get
            {
                if(_instance == null)
                {
                    _instance = Q.GetResource<DUIData>(DUI.RESOURCES_PATH_DUIDATA, "DUIData");

                    if(_instance == null)
                    {
                        _instance = Q.CreateAsset<DUIData>(DUI.RELATIVE_PATH_DUIDATA, "DUIData");
                    }
                }
                return _instance;
            }
        }

        public Database DatabaseUIElements;
        public Database DatabaseUIButtons;
        public List<string> DatabaseUICanvases;
        public SoundsDatabase DatabaseUISounds;
        public AnimDatabase DatabaseInAnimations;
        public AnimDatabase DatabaseOutAnimations;
        public LoopDatabase DatabaseLoopAnimations;
        public PunchDatabase DatabasePunchAnimations;
        public AnimDatabase DatabaseStateAnimations;

        #region UIElements
        /// <summary>
        /// Checks that the database is not null and that it contains the minumum required items.
        /// </summary>
        public void ValidateUIElements()
        {
            bool needsSave = false;
            if(DatabaseUIElements == null) //database was null -> initialize it
            {
                DatabaseUIElements = new Database();
                needsSave = true;
            }

            if(!DatabaseUIElements.ContainsCategoryName(DUI.UNCATEGORIZED_CATEGORY_NAME)) //database did not contain a required category -> add it
            {
                DatabaseUIElements.AddCategory(DUI.UNCATEGORIZED_CATEGORY_NAME, false);
                needsSave = true;
            }

            if(!DatabaseUIElements.Contains(DUI.UNCATEGORIZED_CATEGORY_NAME, DUI.DEFAULT_ELEMENT_NAME)) //category did not contain a required item -> add it
            {
                DatabaseUIElements.GetCategory(DUI.UNCATEGORIZED_CATEGORY_NAME).AddItemName(DUI.DEFAULT_ELEMENT_NAME, false);
                needsSave = true;
            }

            if(!DatabaseUIElements.ContainsCategoryName(DUI.CUSTOM_NAME))
            {
                DatabaseUIElements.UpdateCategoriesNamesList();
                needsSave = true;
            }

            DatabaseUIElements.RemoveCategoriesWithNoName(); //remove all categories that have no name set
            DatabaseUIElements.RemoveEmptyItemsFromCategories(); //remove all empty items with no name set
            DatabaseUIElements.SortCategoriesByCategoryName(); //sort categories by category name
            DatabaseUIElements.SortItemNamesInCategories(); //sort the items list in each category
            if(needsSave)
            {
                QUI.SetDirty(Instance);
                AssetDatabase.SaveAssets();
            }
        }

        /// <summary>
        /// Checks if there are any UIElement database files. If yes, then copies their entries to the proper database and then deletes the files.
        /// </summary>
        public void ScanForUIElements(bool saveAssets = false)
        {
            string[] fileNames = null; //initialize a string array to save file names
            fileNames = File.GetFilesNames(DUI.RELATIVE_PATH_UIELEMENTS, "asset"); //get all the file names in the target folder
            int fileNamesLength = fileNames != null ? fileNames.Length : 0;
            if(fileNamesLength == 0) //there are no files in the folder -> return
            {
                return;
            }

            string progressBarTitle = "Scanning UIElements Database"; //set a progress bar title
            EditorUtility.DisplayProgressBar(progressBarTitle, "", 0f); //display a progress bar to let the developer know that something is happening
            Dictionary<string, NamesDatabase> foundDatabase = new Dictionary<string, NamesDatabase>(); //initilize a container for the found database

            for(int fileIndex = 0; fileIndex < fileNamesLength; fileIndex++)
            {
                EditorUtility.DisplayProgressBar(progressBarTitle, fileNames[fileIndex], ((fileIndex + 1) / (fileNamesLength + 2))); //display the progress bar with the file name that is being processed
                foundDatabase.Add(fileNames[fileIndex], Q.GetResource<NamesDatabase>(DUI.RESOURCES_PATH_UIELEMENTS, fileNames[fileIndex])); //add a names database by loading it from the resources folder
                Q.MoveAssetToTrash(DUI.RELATIVE_PATH_UIELEMENTS, fileNames[fileIndex]);
            }

            EditorUtility.DisplayProgressBar(progressBarTitle, "", 0.9f); //display a progress bar to let the developer know that something is happening
            if(DatabaseUIElements == null) { DatabaseUIElements = new Database(); } //database is null -> initialize it
            Category c = null;
            foreach(string categoryName in foundDatabase.Keys) //iterate through the found database
            {
                if(foundDatabase[categoryName].data == null) { continue; } //this category in the found database is empty -> do not add it
                c = DatabaseUIElements.GetCategory(categoryName); //get a reference to the category in the database
                if(c == null) { c = DatabaseUIElements.AddCategory(categoryName); } //the category name does not exist in the database -> add it
                for(int i = 0; i < foundDatabase[categoryName].data.Count; i++)
                {
                    if(foundDatabase[categoryName].data[i].Trim().IsNullOrEmpty()) { continue; } //item name in found database category is empty -> skip adding it
                    if(c.Contains(foundDatabase[categoryName].data[i])) { continue; } //the database category already contains this item name -> skip adding it
                    c.AddItemName(foundDatabase[categoryName].data[i]); //add the item name from the found database category to the database category
                }
                c.SortItemNames(); //sort the item names list
            }

            EditorUtility.DisplayProgressBar(progressBarTitle, "", 0.95f); //display a progress bar to let the developer know that something is happening
            DatabaseUIElements.UpdateCategoriesNamesList();

            EditorUtility.DisplayProgressBar(progressBarTitle, "", 1f); //update the progress bar
            QUI.SetDirty(Instance); //mark this scriptable objct as dirty
            if(saveAssets) { AssetDatabase.SaveAssets(); } //save the asset database

            EditorUtility.ClearProgressBar(); //clear the progress bar
        }

        public void CreateUIElementsDatabaseAssetFile(string categoryName, List<string> names, bool saveAssets = false, bool refreshAssets = false)
        {
            if(string.IsNullOrEmpty(categoryName)) { return; } //category name is empty -> return
            NamesDatabase newCategory = Q.GetResource<NamesDatabase>(DUI.RESOURCES_PATH_UIELEMENTS, categoryName);
            if(newCategory == null) //asset file does not exist
            {
                newCategory = Q.CreateAsset<NamesDatabase>(DUI.RELATIVE_PATH_UIELEMENTS, categoryName);
                newCategory.Init();
            }

            if(names != null && names.Count > 0)
            {
                if(newCategory.data == null)
                {
                    newCategory.data = new List<string>();
                }

                for(int i = 0; i < names.Count; i++)
                {
                    if(newCategory.data.Contains(names[i]))
                    {
                        continue;
                    }
                    newCategory.data.Add(names[i]);
                }

                newCategory.data.Sort();
            }

            EditorUtility.SetDirty(newCategory);
            if(saveAssets) { AssetDatabase.SaveAssets(); }
            if(refreshAssets) { AssetDatabase.Refresh(); }
        }
        #endregion

        #region UIButtons
        /// <summary>
        /// Checks that the database is not null and that it contains the minumum required items.
        /// </summary>
        public void ValidateUIButtons()
        {
            bool needsSave = false;
            if(DatabaseUIButtons == null) { DatabaseUIButtons = new Database(); needsSave = true; } //database was null -> initialize it
            if(!DatabaseUIButtons.ContainsCategoryName(DUI.UNCATEGORIZED_CATEGORY_NAME)) { DatabaseUIButtons.AddCategory(DUI.UNCATEGORIZED_CATEGORY_NAME); needsSave = true; } //database did not contain a required category -> add it
            if(!DatabaseUIButtons.GetCategory(DUI.UNCATEGORIZED_CATEGORY_NAME).Contains(DUI.DEFAULT_BUTTON_NAME)) { DatabaseUIButtons.GetCategory(DUI.UNCATEGORIZED_CATEGORY_NAME).AddItemName(DUI.DEFAULT_BUTTON_NAME); needsSave = true; } //category did not contain a required item -> add it
            if(!DatabaseUIButtons.categoryNames.Contains(DUI.CUSTOM_NAME)) { DatabaseUIButtons.UpdateCategoriesNamesList(); }
            DatabaseUIButtons.RemoveCategoriesWithNoName(); //remove all categories that have no name set
            DatabaseUIButtons.RemoveEmptyItemsFromCategories(); //remove all empty items with no name set
            DatabaseUIButtons.SortCategoriesByCategoryName(); //sort categories by category name
            DatabaseUIButtons.SortItemNamesInCategories(); //sort the items list in each category
            if(needsSave)
            {
                QUI.SetDirty(Instance);
                AssetDatabase.SaveAssets();
            }
        }

        /// <summary>
        /// Checks if there are any UIButtons database files. If yes, then copies their entries to the proper database and then deletes the files.
        /// </summary>
        public void ScanForUIButtons(bool saveAssets = false)
        {
            string[] fileNames = null; //initialize a string array to save file names
            fileNames = QuickEngine.IO.File.GetFilesNames(DUI.RELATIVE_PATH_UIBUTTONS, "asset"); //get all the file names in the target folder
            int fileNamesLength = fileNames != null ? fileNames.Length : 0;
            if(fileNamesLength == 0) //there are no files in the folder -> return
            {
                return;
            }

            string progressBarTitle = "Scanning UIButtons Database"; //set a progress bar title
            EditorUtility.DisplayProgressBar(progressBarTitle, "", 0f); //display a progress bar to let the developer know that something is happening
            Dictionary<string, NamesDatabase> foundDatabase = new Dictionary<string, NamesDatabase>(); //initilize a container for the found database

            for(int fileIndex = 0; fileIndex < fileNamesLength; fileIndex++)
            {
                EditorUtility.DisplayProgressBar(progressBarTitle, fileNames[fileIndex], ((fileIndex + 1) / (fileNamesLength + 2))); //display the progress bar with the file name that is being processed
                foundDatabase.Add(fileNames[fileIndex], Q.GetResource<NamesDatabase>(DUI.RESOURCES_PATH_UIBUTTONS, fileNames[fileIndex])); //add a names database by loading it from the resources folder
                Q.MoveAssetToTrash(DUI.RELATIVE_PATH_UIBUTTONS, fileNames[fileIndex]);
            }

            EditorUtility.DisplayProgressBar(progressBarTitle, "", 0.9f); //display a progress bar to let the developer know that something is happening
            if(DatabaseUIButtons == null) { DatabaseUIButtons = new Database(); } //database is null -> initialize it
            Category c = null;
            foreach(string categoryName in foundDatabase.Keys) //iterate through the found database
            {
                if(foundDatabase[categoryName].data == null) { continue; } //this category in the found database is empty -> do not add it
                c = DatabaseUIButtons.GetCategory(categoryName); //get a reference to the category in the database
                if(c == null) { c = DatabaseUIButtons.AddCategory(categoryName); } //the category name does not exist in the database -> add it
                for(int i = 0; i < foundDatabase[categoryName].data.Count; i++)
                {
                    if(foundDatabase[categoryName].data[i].Trim().IsNullOrEmpty()) { continue; } //item name in found database category is empty -> skip adding it
                    if(c.Contains(foundDatabase[categoryName].data[i])) { continue; } //the database category already contains this item name -> skip adding it
                    c.AddItemName(foundDatabase[categoryName].data[i]); //add the item name from the found database category to the database category
                }
                c.SortItemNames(); //sort the item names list
            }

            EditorUtility.DisplayProgressBar(progressBarTitle, "", 0.95f); //display a progress bar to let the developer know that something is happening
            DatabaseUIElements.UpdateCategoriesNamesList();

            EditorUtility.DisplayProgressBar(progressBarTitle, "", 1f); //update the progress bar
            QUI.SetDirty(Instance); //mark this scriptable objct as dirty
            if(saveAssets) { AssetDatabase.SaveAssets(); } //save the asset database

            EditorUtility.ClearProgressBar(); //clear the progress bar
        }

        public void CreateUIButtonsDatabaseAssetFile(string categoryName, List<string> names, bool saveAssets = false, bool refreshAssets = false)
        {
            if(string.IsNullOrEmpty(categoryName)) { return; } //category name is empty -> return
            NamesDatabase newCategory = Q.GetResource<NamesDatabase>(DUI.RESOURCES_PATH_UIBUTTONS, categoryName);
            if(newCategory == null) //asset file does not exist
            {
                newCategory = Q.CreateAsset<NamesDatabase>(DUI.RELATIVE_PATH_UIBUTTONS, categoryName);
                newCategory.Init();
            }

            if(names != null && names.Count > 0)
            {
                if(newCategory.data == null)
                {
                    newCategory.data = new List<string>();
                }

                for(int i = 0; i < names.Count; i++)
                {
                    if(newCategory.data.Contains(names[i]))
                    {
                        continue;
                    }
                    newCategory.data.Add(names[i]);
                }

                newCategory.data.Sort();
            }

            EditorUtility.SetDirty(newCategory);
            if(saveAssets) { AssetDatabase.SaveAssets(); }
            if(refreshAssets) { AssetDatabase.Refresh(); }
        }
        #endregion

        #region UICanvases
        /// <summary>
        /// Checks that the database is not null and that it contains the minumum required items.
        /// </summary>
        public void ValidateUICanvases()
        {
            bool needsSave = false;
            if(DatabaseUICanvases == null) { DatabaseUICanvases = new List<string>(); needsSave = true; } //database was null -> initialize it
            if(!DatabaseUICanvases.Contains(DUI.MASTER_CANVAS_NAME)) { DatabaseUICanvases.Add(DUI.MASTER_CANVAS_NAME); needsSave = true; } //database did not contain a required item -> add it
            for(int i = DatabaseUICanvases.Count - 1; i >= 0; i--) { if(DatabaseUICanvases[i].IsNullOrEmpty()) { DatabaseUICanvases.RemoveAt(i); needsSave = true; } } //remove all the empty entries
            DatabaseUICanvases.Sort(); //sort database entries for usability reasons
            if(needsSave)
            {
                QUI.SetDirty(Instance);
                AssetDatabase.SaveAssets();
            }
        }

        /// <summary>
        /// Checks if there are any UICanvases files. If yes, then copies their entries to the proper database and then deletes the files
        /// </summary>
        public void ScanForUICanvases(bool saveAssets = false)
        {
            NamesDatabase foundDatabase = Q.GetResource<NamesDatabase>(DUI.RESOURCES_PATH_CANVAS_DATABASE, DUI.CANVAS_DATABASE_FILENAME); //try to find a database in the target folder
            if(foundDatabase == null) //did not find a database -> return
            {
                return;
            }

            string progressBarTitle = "Scanning UICanvases Database"; //set a progress bar title
            EditorUtility.DisplayProgressBar(progressBarTitle, "", 0f); //display a progress bar to let the developer know that something is happening
            ValidateUICanvases(); //initialize and validate the database

            EditorUtility.DisplayProgressBar(progressBarTitle, "", 0.4f); //update the progress bar
            for(int i = 0; i < foundDatabase.data.Count; i++)
            {
                if(foundDatabase.data[i].IsNullOrEmpty()) { continue; } //this was an empty entry in the found databse -> continue
                if(DatabaseUICanvases.Contains(foundDatabase.data[i])) { continue; } //the database already contains this entry -> continue
                DatabaseUICanvases.Add(foundDatabase.data[i]); //the database does not contain this endty -> add it
            }

            EditorUtility.DisplayProgressBar(progressBarTitle, "", 0.6f); //update the progress bar
            Q.MoveAssetToTrash(DUI.RELATIVE_PATH_CANVAS_DATABASE, DUI.CANVAS_DATABASE_FILENAME); //delete the found database file

            EditorUtility.DisplayProgressBar(progressBarTitle, "", 0.8f); //update the progress bar
            QUI.SetDirty(Instance); //mark this scriptable objct as dirty
            if(saveAssets) { AssetDatabase.SaveAssets(); } //save the asset database

            EditorUtility.ClearProgressBar(); //clear the progress bar
        }

        public void AddUICanvas(string canvasName, bool saveAssets = true)
        {
            if(DatabaseUICanvases == null) { DatabaseUICanvases = new List<string>(); }
            if(DatabaseUICanvases.Contains(canvasName)) { return; }
            DatabaseUICanvases.Add(canvasName);
            QUI.SetDirty(Instance);
            if(saveAssets)
            {
                AssetDatabase.SaveAssets();
            }
        }

        public void RemoveUICanvas(string canvasName, bool saveAssets = true)
        {
            if(DatabaseUICanvases == null) { DatabaseUICanvases = new List<string>(); }
            if(canvasName == DUI.MASTER_CANVAS_NAME) { return; }
            if(!DatabaseUICanvases.Contains(canvasName)) { return; }
            DatabaseUICanvases.Remove(canvasName);
            QUI.SetDirty(Instance);
            if(saveAssets)
            {
                AssetDatabase.SaveAssets();
            }
        }
        #endregion

        #region UISounds
        /// <summary>
        /// Performs an automated scan for UISounds files if the database is null or empty.
        /// </summary>
        public void AutomatedScanForUISounds()
        {
            if(DatabaseUISounds == null
               || DatabaseUISounds.sounds == null
               || DatabaseUISounds.sounds.Count == 0)
            {
                ScanForUISounds(true);
            }
        }

        /// <summary>
        /// Checks if there are any UISounds files. If yes, then copies their entries to the proper database.
        /// </summary>
        public void ScanForUISounds(bool saveAssets = false)
        {
            string[] fileNames = null; //initialize a string array to save file names
            fileNames = QuickEngine.IO.File.GetFilesNames(DUI.RELATIVE_PATH_UISOUNDS, "asset"); //get all the file names in the target folder
            int fileNamesLength = fileNames != null ? fileNames.Length : 0;
            if(fileNamesLength == 0) //there are no files in the folder -> return
            {
                return;
            }

            string progressBarTitle = "Scanning UISounds Database"; //set a progress bar title
            EditorUtility.DisplayProgressBar(progressBarTitle, "", 0f); //display a progress bar to let the developer know that something is happening
            List<UISound> foundDatabase = new List<UISound>(); //initilize a container for the found database

            for(int fileIndex = 0; fileIndex < fileNamesLength; fileIndex++)
            {
                EditorUtility.DisplayProgressBar(progressBarTitle, fileNames[fileIndex], ((fileIndex + 1) / (fileNamesLength + 2))); //display the progress bar with the file name that is being processed
                UISound temp = Q.GetResource<UISound>(DUI.RESOURCES_PATH_UISOUNDS, fileNames[fileIndex]); //get the asset file reference
                if(temp.soundName.IsNullOrEmpty() || temp.soundName != fileNames[fileIndex]) //soundName is empty or does not match the file name
                {
                    temp.soundName = fileNames[fileIndex]; //set the soundName the same as the fileName
                    QUI.SetDirty(temp); //mark this file as dirty
                    saveAssets = true; //mark saveAssets as true to ensure that this UISound asset file gets saved with the new soundName
                }
                foundDatabase.Add(temp); //add the UISound reference to the found database
            }

            EditorUtility.DisplayProgressBar(progressBarTitle, "", 0.9f); //display a progress bar to let the developer know that something is happening
            if(DatabaseUISounds == null) { DatabaseUISounds = new SoundsDatabase(); } //database is null -> initialize it
            foreach(UISound uis in foundDatabase) //iterate through the found database
            {
                if(uis == null) { continue; } //this item in the found database is empty or null -> do not add it (sanity check)
                if(DatabaseUISounds.Contains(uis.soundName)) { continue; } //this soundName already exists in the databse -> continue to avoid duplicates and issues
                DatabaseUISounds.AddUISound(uis, false);
            }

            EditorUtility.DisplayProgressBar(progressBarTitle, "", 0.95f); //display a progress bar to let the developer know that something is happening
            DatabaseUISounds.UpdateSoundsNamesList(); //sorts the database and updates the soundNames list

            EditorUtility.DisplayProgressBar(progressBarTitle, "", 1f); //update the progress bar
            QUI.SetDirty(Instance); //mark this scriptable objct as dirty
            if(saveAssets) { AssetDatabase.SaveAssets(); } //save the asset database

            EditorUtility.ClearProgressBar(); //clear the progress bar
        }

        /// <summary>
        /// Checks that the database is not null and initializes it if needed.
        /// </summary>
        public void ValidateUISounds()
        {
            bool needsSave = false;
            if(DatabaseUISounds == null) //database was null -> initialize it and save the file
            {
                DatabaseUISounds = new SoundsDatabase();
                needsSave = true;
            }

            if(!DatabaseUISounds.Contains(DUI.DEFAULT_SOUND_NAME))
            {
                DatabaseUISounds.CreateUISound(DUI.DEFAULT_SOUND_NAME, SoundType.All, null);
                needsSave = true;
            }

            DatabaseUISounds.UpdateSoundsNamesList(); //update the soundNames list (this also sorts the database by soundName)

            if(needsSave)
            {
                QUI.SetDirty(Instance);
                AssetDatabase.SaveAssets();
            }
        }
        #endregion

        #region Animator Presets

        /// <summary>
        /// Checks if there are any relevant asset files. If yes, then copies their entries to the proper database.
        /// </summary>
        public void ScanForInAnimations(bool saveAssets = false)
        {
            List<string> directories = new List<string>(); //initialize a string list to store the names of the folders
            directories.AddRange(File.GetDirectoriesNames(UIAnimatorUtil.RELATIVE_PATH_IN_ANIM_DATA)); //add all the folder names to the list (these are the categories names)
            RemoveEmptyFolders(directories, UIAnimatorUtil.RELATIVE_PATH_IN_ANIM_DATA); //removes all the empty folders (categories with no presets in them) and updates the given directories list
            if(DatabaseInAnimations == null) { DatabaseInAnimations = new AnimDatabase(); }

            string progressBarTitle = "Scanning for In Animations"; //set a progress bar title
            EditorUtility.DisplayProgressBar(progressBarTitle, "", 0f); //display a progress bar to let the developer know that something is happening

            string[] fileNames = null; //initialize a string array to save file names
            for(int directoryIndex = directories.Count - 1; directoryIndex >= 0; directoryIndex--)
            {
                EditorUtility.DisplayProgressBar(progressBarTitle, directories[directoryIndex], ((directoryIndex + 1) / (directories.Count + 2)));
                fileNames = GetAnimatorPresetsFilesNames(UIAnimatorUtil.RELATIVE_PATH_IN_ANIM_DATA, directories[directoryIndex]); //get all the preset asset files names
                if(fileNames == null || fileNames.Length == 0) { continue; } //sanity check - this should not happen as an empty folder check has already been performed
                DatabaseInAnimations.AddCategory(directories[directoryIndex], UIAnimatorUtil.RELATIVE_PATH_IN_ANIM_DATA); //add this category to the database
                for(int fileIndex = 0; fileIndex < fileNames.Length; fileIndex++)
                {
                    AnimData asset = Q.GetResource<AnimData>(UIAnimatorUtil.RESOURCES_PATH_IN_ANIM_DATA + directories[directoryIndex] + "/", fileNames[fileIndex]); //get the preset asset file
                    if(asset == null) { continue; }
                    EditorUtility.DisplayProgressBar(progressBarTitle, directories[directoryIndex] + " / " + asset.presetName, ((directoryIndex + 1) / (directories.Count + 2)));
                    DatabaseInAnimations.GetCategory(directories[directoryIndex]).AddAnimData(asset); //add this preset to the database
                }
            }
            EditorUtility.DisplayProgressBar(progressBarTitle, "", 0.95f); //display a progress bar to let the developer know that something is happening
            DatabaseInAnimations.SortCategoriesByCategoryName(); //sort categories
            DatabaseInAnimations.SortPresetNamesInCategories(); //sort preset names

            EditorUtility.DisplayProgressBar(progressBarTitle, "", 1f); //update the progress bar
            QUI.SetDirty(Instance); //mark this scriptable objct as dirty
            if(saveAssets) { AssetDatabase.SaveAssets(); } //save the asset database
            EditorUtility.ClearProgressBar(); //clear the progress bar
        }

        /// <summary>
        /// Checks that the database is not null and initializes it if needed.
        /// </summary>
        public void ValidateInAnimations()
        {
            bool needsSave = false;
            if(DatabaseInAnimations == null || DatabaseInAnimations.categories == null || DatabaseInAnimations.categories.Count == 0) //database is null or empty -> initialize it and save the file
            {
                ScanForInAnimations(false);
                needsSave = true;
            }

            if(!DatabaseInAnimations.ContainsCategory(UIAnimatorUtil.UNCATEGORIZED_CATEGORY_NAME)) //the default category is missing -> add it
            {
                DatabaseInAnimations.AddCategory(UIAnimatorUtil.UNCATEGORIZED_CATEGORY_NAME, UIAnimatorUtil.RELATIVE_PATH_IN_ANIM_DATA, false);
                needsSave = true;
            }

            if(!DatabaseInAnimations.Contains(UIAnimatorUtil.UNCATEGORIZED_CATEGORY_NAME, UIAnimatorUtil.DEFAULT_PRESET_NAME)) //the default preset is missing -> create and add it
            {
                DatabaseInAnimations.GetCategory(UIAnimatorUtil.UNCATEGORIZED_CATEGORY_NAME).CreateAnimData(new AnimData(Anim.AnimationType.In), UIAnimatorUtil.RESOURCES_PATH_IN_ANIM_DATA, UIAnimatorUtil.RELATIVE_PATH_IN_ANIM_DATA, true, true).data.move.enabled = true; ;
                needsSave = true;
            }

            if(needsSave)
            {
                QUI.SetDirty(Instance);
                AssetDatabase.SaveAssets();
            }
        }

        /// <summary>
        /// Checks if there are any relevant asset files. If yes, then copies their entries to the proper database.
        /// </summary>
        public void ScanForOutAnimations(bool saveAssets = false)
        {
            List<string> directories = new List<string>(); //initialize a string list to store the names of the folders
            directories.AddRange(File.GetDirectoriesNames(UIAnimatorUtil.RELATIVE_PATH_OUT_ANIM_DATA)); //add all the folder names to the list (these are the categories names)
            RemoveEmptyFolders(directories, UIAnimatorUtil.RELATIVE_PATH_OUT_ANIM_DATA); //removes all the empty folders (categories with no presets in them) and updates the given directories list
            if(DatabaseOutAnimations == null) { DatabaseOutAnimations = new AnimDatabase(); }

            string progressBarTitle = "Scanning for Out Animations"; //set a progress bar title
            EditorUtility.DisplayProgressBar(progressBarTitle, "", 0f); //display a progress bar to let the developer know that something is happening

            string[] fileNames = null; //initialize a string array to save file names
            for(int directoryIndex = directories.Count - 1; directoryIndex >= 0; directoryIndex--)
            {
                EditorUtility.DisplayProgressBar(progressBarTitle, directories[directoryIndex], ((directoryIndex + 1) / (directories.Count + 2)));
                fileNames = GetAnimatorPresetsFilesNames(UIAnimatorUtil.RELATIVE_PATH_OUT_ANIM_DATA, directories[directoryIndex]); //get all the preset asset files names
                if(fileNames == null || fileNames.Length == 0) { continue; } //sanity check - this should not happen as an empty folder check has already been performed
                DatabaseOutAnimations.AddCategory(directories[directoryIndex], UIAnimatorUtil.RELATIVE_PATH_OUT_ANIM_DATA); //add this category to the database
                for(int fileIndex = 0; fileIndex < fileNames.Length; fileIndex++)
                {
                    AnimData asset = Q.GetResource<AnimData>(UIAnimatorUtil.RESOURCES_PATH_OUT_ANIM_DATA + directories[directoryIndex] + "/", fileNames[fileIndex]); //get the preset asset file
                    if(asset == null) { continue; }
                    EditorUtility.DisplayProgressBar(progressBarTitle, directories[directoryIndex] + " / " + asset.presetName, ((directoryIndex + 1) / (directories.Count + 2)));
                    DatabaseOutAnimations.GetCategory(directories[directoryIndex]).AddAnimData(asset); //add this preset to the database
                }
            }
            EditorUtility.DisplayProgressBar(progressBarTitle, "", 0.95f); //display a progress bar to let the developer know that something is happening
            DatabaseOutAnimations.SortCategoriesByCategoryName(); //sort categories
            DatabaseOutAnimations.SortPresetNamesInCategories(); //sort preset names

            EditorUtility.DisplayProgressBar(progressBarTitle, "", 1f); //update the progress bar
            QUI.SetDirty(Instance); //mark this scriptable objct as dirty
            if(saveAssets) { AssetDatabase.SaveAssets(); } //save the asset database
            EditorUtility.ClearProgressBar(); //clear the progress bar
        }

        /// <summary>
        /// Checks that the database is not null and initializes it if needed.
        /// </summary>
        public void ValidateOutAnimations()
        {
            bool needsSave = false;
            if(DatabaseOutAnimations == null || DatabaseOutAnimations.categories == null || DatabaseOutAnimations.categories.Count == 0) //database is null or empty -> initialize it and save the file
            {
                ScanForOutAnimations(false);
                needsSave = true;
            }

            if(!DatabaseOutAnimations.ContainsCategory(UIAnimatorUtil.UNCATEGORIZED_CATEGORY_NAME)) //the default category is missing -> add it
            {
                DatabaseOutAnimations.AddCategory(UIAnimatorUtil.UNCATEGORIZED_CATEGORY_NAME, UIAnimatorUtil.RELATIVE_PATH_OUT_ANIM_DATA, false);
                needsSave = true;
            }

            if(!DatabaseOutAnimations.Contains(UIAnimatorUtil.UNCATEGORIZED_CATEGORY_NAME, UIAnimatorUtil.DEFAULT_PRESET_NAME)) //the default preset is missing -> create and add it
            {
                DatabaseOutAnimations.GetCategory(UIAnimatorUtil.UNCATEGORIZED_CATEGORY_NAME).CreateAnimData(new AnimData(Anim.AnimationType.Out), UIAnimatorUtil.RESOURCES_PATH_OUT_ANIM_DATA, UIAnimatorUtil.RELATIVE_PATH_OUT_ANIM_DATA, true, true).data.move.enabled = true; ;
                needsSave = true;
            }

            if(needsSave)
            {
                QUI.SetDirty(Instance);
                AssetDatabase.SaveAssets();
            }
        }

        /// <summary>
        /// Checks if there are any relevant asset files. If yes, then copies their entries to the proper database.
        /// </summary>
        public void ScanForLoopAnimations(bool saveAssets = false)
        {
            List<string> directories = new List<string>(); //initialize a string list to store the names of the folders
            directories.AddRange(File.GetDirectoriesNames(UIAnimatorUtil.RELATIVE_PATH_LOOP_DATA)); //add all the folder names to the list (these are the categories names)
            RemoveEmptyFolders(directories, UIAnimatorUtil.RELATIVE_PATH_LOOP_DATA); //removes all the empty folders (categories with no presets in them) and updates the given directories list
            if(DatabaseLoopAnimations == null) { DatabaseLoopAnimations = new LoopDatabase(); }

            string progressBarTitle = "Scanning for Loop Animations"; //set a progress bar title
            EditorUtility.DisplayProgressBar(progressBarTitle, "", 0f); //display a progress bar to let the developer know that something is happening

            string[] fileNames = null; //initialize a string array to save file names
            for(int directoryIndex = directories.Count - 1; directoryIndex >= 0; directoryIndex--)
            {
                EditorUtility.DisplayProgressBar(progressBarTitle, directories[directoryIndex], ((directoryIndex + 1) / (directories.Count + 2)));
                fileNames = GetAnimatorPresetsFilesNames(UIAnimatorUtil.RELATIVE_PATH_LOOP_DATA, directories[directoryIndex]); //get all the preset asset files names
                if(fileNames == null || fileNames.Length == 0) { continue; } //sanity check - this should not happen as an empty folder check has already been performed
                DatabaseLoopAnimations.AddCategory(directories[directoryIndex], UIAnimatorUtil.RELATIVE_PATH_LOOP_DATA); //add this category to the database
                for(int fileIndex = 0; fileIndex < fileNames.Length; fileIndex++)
                {
                    LoopData asset = Q.GetResource<LoopData>(UIAnimatorUtil.RESOURCES_PATH_LOOP_DATA + directories[directoryIndex] + "/", fileNames[fileIndex]); //get the preset asset file
                    if(asset == null) { continue; }
                    EditorUtility.DisplayProgressBar(progressBarTitle, directories[directoryIndex] + " / " + asset.presetName, ((directoryIndex + 1) / (directories.Count + 2)));
                    DatabaseLoopAnimations.GetCategory(directories[directoryIndex]).AddLoopData(asset); //add this preset to the database
                }
            }
            EditorUtility.DisplayProgressBar(progressBarTitle, "", 0.95f); //display a progress bar to let the developer know that something is happening
            DatabaseLoopAnimations.SortCategoriesByCategoryName(); //sort categories
            DatabaseLoopAnimations.SortPresetNamesInCategories(); //sort preset names

            EditorUtility.DisplayProgressBar(progressBarTitle, "", 1f); //update the progress bar
            QUI.SetDirty(Instance); //mark this scriptable objct as dirty
            if(saveAssets) { AssetDatabase.SaveAssets(); } //save the asset database
            EditorUtility.ClearProgressBar(); //clear the progress bar
        }

        /// <summary>
        /// Checks that the database is not null and initializes it if needed.
        /// </summary>
        public void ValidateLoopAnimations()
        {
            bool needsSave = false;
            if(DatabaseLoopAnimations == null || DatabaseLoopAnimations.categories == null || DatabaseLoopAnimations.categories.Count == 0) //database is null or empty -> initialize it and save the file
            {
                ScanForLoopAnimations(false);
                needsSave = true;
            }

            if(!DatabaseLoopAnimations.ContainsCategory(UIAnimatorUtil.UNCATEGORIZED_CATEGORY_NAME)) //the default category is missing -> add it
            {
                DatabaseLoopAnimations.AddCategory(UIAnimatorUtil.UNCATEGORIZED_CATEGORY_NAME, UIAnimatorUtil.RELATIVE_PATH_LOOP_DATA, false);
                needsSave = true;
            }

            if(!DatabaseLoopAnimations.Contains(UIAnimatorUtil.UNCATEGORIZED_CATEGORY_NAME, UIAnimatorUtil.DEFAULT_PRESET_NAME)) //the default preset is missing -> create and add it
            {
                DatabaseLoopAnimations.GetCategory(UIAnimatorUtil.UNCATEGORIZED_CATEGORY_NAME).CreateLoopData(new LoopData(), UIAnimatorUtil.RESOURCES_PATH_LOOP_DATA, UIAnimatorUtil.RELATIVE_PATH_LOOP_DATA, true, true).data.move.enabled = true; ;
                needsSave = true;
            }

            if(needsSave)
            {
                QUI.SetDirty(Instance);
                AssetDatabase.SaveAssets();
            }
        }

        /// <summary>
        /// Checks if there are any relevant asset files. If yes, then copies their entries to the proper database.
        /// </summary>
        public void ScanForPunchAnimations(bool saveAssets = false)
        {
            List<string> directories = new List<string>(); //initialize a string list to store the names of the folders
            directories.AddRange(File.GetDirectoriesNames(UIAnimatorUtil.RELATIVE_PATH_PUNCH_DATA)); //add all the folder names to the list (these are the categories names)
            RemoveEmptyFolders(directories, UIAnimatorUtil.RELATIVE_PATH_PUNCH_DATA); //removes all the empty folders (categories with no presets in them) and updates the given directories list
            if(DatabasePunchAnimations == null) { DatabasePunchAnimations = new PunchDatabase(); }

            string progressBarTitle = "Scanning for Punch Animations"; //set a progress bar title
            EditorUtility.DisplayProgressBar(progressBarTitle, "", 0f); //display a progress bar to let the developer know that something is happening

            string[] fileNames = null; //initialize a string array to save file names
            for(int directoryIndex = directories.Count - 1; directoryIndex >= 0; directoryIndex--)
            {
                EditorUtility.DisplayProgressBar(progressBarTitle, directories[directoryIndex], ((directoryIndex + 1) / (directories.Count + 2)));
                fileNames = GetAnimatorPresetsFilesNames(UIAnimatorUtil.RELATIVE_PATH_PUNCH_DATA, directories[directoryIndex]); //get all the preset asset files names
                if(fileNames == null || fileNames.Length == 0) { continue; } //sanity check - this should not happen as an empty folder check has already been performed
                DatabasePunchAnimations.AddCategory(directories[directoryIndex], UIAnimatorUtil.RELATIVE_PATH_PUNCH_DATA); //add this category to the database
                for(int fileIndex = 0; fileIndex < fileNames.Length; fileIndex++)
                {
                    PunchData asset = Q.GetResource<PunchData>(UIAnimatorUtil.RESOURCES_PATH_PUNCH_DATA + directories[directoryIndex] + "/", fileNames[fileIndex]); //get the preset asset file
                    if(asset == null) { continue; }
                    EditorUtility.DisplayProgressBar(progressBarTitle, directories[directoryIndex] + " / " + asset.presetName, ((directoryIndex + 1) / (directories.Count + 2)));
                    DatabasePunchAnimations.GetCategory(directories[directoryIndex]).AddPunchData(asset); //add this preset to the database
                }
            }
            EditorUtility.DisplayProgressBar(progressBarTitle, "", 0.95f); //display a progress bar to let the developer know that something is happening
            DatabasePunchAnimations.SortCategoriesByCategoryName(); //sort categories
            DatabasePunchAnimations.SortPresetNamesInCategories(); //sort preset names

            EditorUtility.DisplayProgressBar(progressBarTitle, "", 1f); //update the progress bar
            QUI.SetDirty(Instance); //mark this scriptable objct as dirty
            if(saveAssets) { AssetDatabase.SaveAssets(); } //save the asset database
            EditorUtility.ClearProgressBar(); //clear the progress bar
        }

        /// <summary>
        /// Checks that the database is not null and initializes it if needed.
        /// </summary>
        public void ValidatePunchAnimations()
        {
            bool needsSave = false;
            if(DatabasePunchAnimations == null || DatabasePunchAnimations.categories == null || DatabasePunchAnimations.categories.Count == 0) //database is null or empty -> initialize it and save the file
            {
                ScanForPunchAnimations(false);
                needsSave = true;
            }

            if(!DatabasePunchAnimations.ContainsCategory(UIAnimatorUtil.UNCATEGORIZED_CATEGORY_NAME)) //the default category is missing -> add it
            {
                DatabasePunchAnimations.AddCategory(UIAnimatorUtil.UNCATEGORIZED_CATEGORY_NAME, UIAnimatorUtil.RELATIVE_PATH_PUNCH_DATA, false);
                needsSave = true;
            }

            if(!DatabasePunchAnimations.Contains(UIAnimatorUtil.UNCATEGORIZED_CATEGORY_NAME, UIAnimatorUtil.DEFAULT_PRESET_NAME)) //the default preset is missing -> create and add it
            {
                DatabasePunchAnimations.GetCategory(UIAnimatorUtil.UNCATEGORIZED_CATEGORY_NAME).CreatePunchData(new PunchData(), UIAnimatorUtil.RESOURCES_PATH_PUNCH_DATA, UIAnimatorUtil.RELATIVE_PATH_PUNCH_DATA, true, true).data.move.enabled = true; ;
                needsSave = true;
            }

            if(needsSave)
            {
                QUI.SetDirty(Instance);
                AssetDatabase.SaveAssets();
            }
        }

        /// <summary>
        /// Checks if there are any relevant asset files. If yes, then copies their entries to the proper database.
        /// </summary>
        public void ScanForStateAnimations(bool saveAssets = false)
        {
            List<string> directories = new List<string>(); //initialize a string list to store the names of the folders
            directories.AddRange(File.GetDirectoriesNames(UIAnimatorUtil.RELATIVE_PATH_STATE_ANIM_DATA)); //add all the folder names to the list (these are the categories names)
            RemoveEmptyFolders(directories, UIAnimatorUtil.RELATIVE_PATH_STATE_ANIM_DATA); //removes all the empty folders (categories with no presets in them) and updates the given directories list
            if(DatabaseStateAnimations == null) { DatabaseStateAnimations = new AnimDatabase(); }

            string progressBarTitle = "Scanning for State Animations"; //set a progress bar title
            EditorUtility.DisplayProgressBar(progressBarTitle, "", 0f); //display a progress bar to let the developer know that something is happening

            string[] fileNames = null; //initialize a string array to save file names
            for(int directoryIndex = directories.Count - 1; directoryIndex >= 0; directoryIndex--)
            {
                EditorUtility.DisplayProgressBar(progressBarTitle, directories[directoryIndex], ((directoryIndex + 1) / (directories.Count + 2)));
                fileNames = GetAnimatorPresetsFilesNames(UIAnimatorUtil.RELATIVE_PATH_STATE_ANIM_DATA, directories[directoryIndex]); //get all the preset asset files names
                if(fileNames == null || fileNames.Length == 0) { continue; } //sanity check - this should not happen as an empty folder check has already been performed
                DatabaseStateAnimations.AddCategory(directories[directoryIndex], UIAnimatorUtil.RELATIVE_PATH_STATE_ANIM_DATA); //add this category to the database
                for(int fileIndex = 0; fileIndex < fileNames.Length; fileIndex++)
                {
                    AnimData asset = Q.GetResource<AnimData>(UIAnimatorUtil.RESOURCES_PATH_STATE_ANIM_DATA + directories[directoryIndex] + "/", fileNames[fileIndex]); //get the preset asset file
                    if(asset == null) { continue; }
                    EditorUtility.DisplayProgressBar(progressBarTitle, directories[directoryIndex] + " / " + asset.presetName, ((directoryIndex + 1) / (directories.Count + 2)));
                    DatabaseStateAnimations.GetCategory(directories[directoryIndex]).AddAnimData(asset); //add this preset to the database
                }
            }
            EditorUtility.DisplayProgressBar(progressBarTitle, "", 0.95f); //display a progress bar to let the developer know that something is happening
            DatabaseStateAnimations.SortCategoriesByCategoryName(); //sort categories
            DatabaseStateAnimations.SortPresetNamesInCategories(); //sort preset names

            EditorUtility.DisplayProgressBar(progressBarTitle, "", 1f); //update the progress bar
            QUI.SetDirty(Instance); //mark this scriptable objct as dirty
            if(saveAssets) { AssetDatabase.SaveAssets(); } //save the asset database
            EditorUtility.ClearProgressBar(); //clear the progress bar
        }

        /// <summary>
        /// Checks that the database is not null and initializes it if needed.
        /// </summary>
        public void ValidateStateAnimations()
        {
            bool needsSave = false;
            if(DatabaseStateAnimations == null || DatabaseStateAnimations.categories == null || DatabaseStateAnimations.categories.Count == 0) //database is null or empty -> initialize it and save the file
            {
                ScanForStateAnimations(false);
                needsSave = true;
            }

            if(!DatabaseStateAnimations.ContainsCategory(UIAnimatorUtil.UNCATEGORIZED_CATEGORY_NAME)) //the default category is missing -> add it
            {
                DatabaseStateAnimations.AddCategory(UIAnimatorUtil.UNCATEGORIZED_CATEGORY_NAME, UIAnimatorUtil.RELATIVE_PATH_STATE_ANIM_DATA, false);
                needsSave = true;
            }

            if(!DatabaseStateAnimations.Contains(UIAnimatorUtil.UNCATEGORIZED_CATEGORY_NAME, UIAnimatorUtil.DEFAULT_PRESET_NAME)) //the default preset is missing -> create and add it
            {
                DatabaseStateAnimations.GetCategory(UIAnimatorUtil.UNCATEGORIZED_CATEGORY_NAME).CreateAnimData(new AnimData(Anim.AnimationType.State), UIAnimatorUtil.RESOURCES_PATH_STATE_ANIM_DATA, UIAnimatorUtil.RELATIVE_PATH_STATE_ANIM_DATA, true, true).data.move.enabled = true; ;
                needsSave = true;
            }

            if(needsSave)
            {
                QUI.SetDirty(Instance);
                AssetDatabase.SaveAssets();
            }
        }

        private string[] GetAnimatorPresetsFilesNames(string relativePath, string presetCategory)
        {
            return File.GetFilesNames(relativePath + presetCategory + "/", "asset");
        }
        private void RemoveEmptyFolders(List<string> directories, string relativePath)
        {
            if(directories == null || directories.Count == 0 || relativePath.IsNullOrEmpty()) { return; }
            for(int i = directories.Count - 1; i >= 0; i--)
            {
                if(GetAnimatorPresetsFilesNames(relativePath, directories[i]).Length == 0) //if this folder does not contain any asset files (presets) -> delete it and remove it from the list
                {
                    FileUtil.DeleteFileOrDirectory(relativePath + directories[i]);
                    directories.RemoveAt(i);
                }
            }
        }
        #endregion

        [Serializable]
        public class Database
        {
            public List<Category> categories;
            public List<string> categoryNames;

            public Category AddCategory(string categoryName, bool saveAssets = false)
            {
                categoryName = categoryName.Trim();
                if(categories == null)
                {
                    categories = new List<Category>();
                }
                if(ContainsCategoryName(categoryName))
                {
                    return categories[CategoryIndex(categoryName)];
                }
                categories.Add(new Category(categoryName));
                UpdateCategoriesNamesList();
                QUI.SetDirty(Instance);
                if(saveAssets)
                {
                    AssetDatabase.SaveAssets();
                }
                return categories[CategoryIndex(categoryName)];
            }
            public Category AddCategory(string categoryName, List<string> itemNames)
            {
                Category newCategory = AddCategory(categoryName, false);
                newCategory.AddItemNames(itemNames, false);
                QUI.SetDirty(Instance);
                AssetDatabase.SaveAssets();
                return newCategory;
            }
            public void RemoveCategory(string categoryName, bool saveAssets = false)
            {
                if(ContainsCategoryName(categoryName))
                {
                    categories.RemoveAt(CategoryIndex(categoryName));
                }
                UpdateCategoriesNamesList();
                QUI.SetDirty(Instance);
                if(saveAssets)
                {
                    AssetDatabase.SaveAssets();
                }
            }

            public void RenameCategory(string oldName, string newName)
            {
                newName = newName.Trim();
                if(string.IsNullOrEmpty(oldName) || string.IsNullOrEmpty(newName))
                {
                    return;
                }
                if(ContainsCategoryName(oldName))
                {
                    categories[CategoryIndex(oldName)].categoryName = newName;
                    categoryNames[CategoryNameIndex(oldName)] = newName;
                }
                UpdateCategoriesNamesList();
                QUI.SetDirty(Instance);
                AssetDatabase.SaveAssets();
            }

            public Category GetCategory(string categoryName)
            {
                if(categories == null)
                {
                    categories = new List<Category>();
                }

                int index = CategoryIndex(categoryName); //check to see if the Database contains a category with the given category name

                if(index < 0) //database does not contain a category with the given category name
                {
                    return null;
                }

                return categories[index]; //the category was found -> return a reference to it
            }

            public bool ContainsCategoryName(string categoryName)
            {
                if(categoryNames == null || categoryNames.Count == 0)
                {
                    return false;
                }
                for(int i = 0; i < categoryNames.Count; i++)
                {
                    if(categoryNames[i].Equals(categoryName))
                    {
                        return true;
                    }
                }
                return false;
            }
            public bool ContainsCategory(string categoryName)
            {
                if(categories == null || categories.Count == 0)
                {
                    return false;
                }
                for(int i = 0; i < categories.Count; i++)
                {
                    if(categories[i].categoryName.Equals(categoryName))
                    {
                        return true;
                    }
                }
                return false;
            }
            public bool Contains(string categoryName, string itemName)
            {
                if(!ContainsCategoryName(categoryName)) { return false; }
                return GetCategory(categoryName).Contains(itemName);
            }

            /// <summary>
            /// Returns the index of the categoryName from the categoryNames list or -1
            /// </summary>
            public int CategoryNameIndex(string categoryName)
            {
                for(int i = 0; i < categoryNames.Count; i++)
                {
                    if(categoryNames[i].Equals(categoryName))
                    {
                        return i;
                    }
                }
                return -1;
            }
            /// <summary>
            /// Returns the index of the categoryName from the categories list or -1
            /// </summary>
            public int CategoryIndex(string categoryName)
            {
                for(int i = 0; i < categories.Count; i++)
                {
                    if(categories[i].categoryName.Equals(categoryName))
                    {
                        return i;
                    }
                }
                return -1;
            }
            /// <summary>
            /// Returns the index of the itemName from the given categoryName or -1
            /// </summary>
            public int ItemNameIndex(string categoryName, string itemName)
            {
                if(!ContainsCategory(categoryName)) { return -1; } //category does not exist -> return -1
                return GetCategory(categoryName).IndexOf(itemName);
            }

            public void RemoveEmptyItemsFromCategories()
            {
                if(categories == null) { categories = new List<Category>(); }
                for(int i = categories.Count - 1; i >= 0; i--)
                {
                    categories[i].RemoveEmptyItems();
                }

            }
            public void Clear()
            {
                if(categories == null) { categories = new List<Category>(); }
                categories.Clear();
                if(categoryNames == null) { categoryNames = new List<string>(); }
                categoryNames.Clear();
            }
            public void UpdateCategoriesNamesList()
            {
                if(categories == null) //database is null -> initialize it
                {
                    categories = new List<Category>();
                }

                if(categoryNames == null)
                {
                    categoryNames = new List<string>(); //categoryNames list is null -> initialize it
                }
                else
                {
                    categoryNames.Clear(); //categoryNames list is not null -> clear it
                }

                categoryNames.Add(DUI.CUSTOM_NAME); //add the custom name category name

                SortCategoriesByCategoryName(); //sort the categories list by categoryName before adding it to the names list
                for(int i = 0; i < categories.Count; i++) //populate the categoryNames array with the sorted values
                {
                    categoryNames.Add(categories[i].categoryName);
                }
            }
            public void SortCategoriesByCategoryName()
            {
                categories.Sort((a, b) => a.categoryName.CompareTo(b.categoryName));
            }
            public void SortItemNamesInCategories()
            {
                for(int i = 0; i < categories.Count; i++)
                {
                    categories[i].SortItemNames();
                }
            }
            public bool RemoveCategoriesWithNoName()
            {
                bool itemsWereRemoved = false;
                if(categories == null) { categories = new List<Category>(); }
                for(int i = categories.Count - 1; i >= 0; i--)
                {
                    if(categories[i].categoryName.Trim().IsNullOrEmpty())
                    {
                        categories.RemoveAt(i);
                        itemsWereRemoved = true;
                    }
                }
                return itemsWereRemoved;
            }
            public bool IsEmpty(string categoryName)
            {
                if(categories == null || categories.Count == 0 || !ContainsCategoryName(categoryName))
                {
                    return true;
                }
                else
                {
                    return GetCategory(categoryName).IsEmpty();
                }
            }

            public void ExpandOrCollapseAllCategories(bool expand)
            {
                for(int i = 0; i < categories.Count; i++) { categories[i].isExpanded.target = expand; }
            }

            internal int CategoryNameIndex(object buttonCategory)
            {
                throw new NotImplementedException();
            }
        }

        [Serializable]
        public class Category
        {
            public string categoryName = string.Empty;
            public List<string> itemNames = new List<string>();
            public AnimBool isExpanded = new AnimBool(false);

            public Category(string categoryName)
            {
                this.categoryName = categoryName;
                this.itemNames = new List<string>();
                this.isExpanded = new AnimBool(false);
            }
            public Category(string categoryName, string itemName)
            {
                this.categoryName = categoryName;
                this.itemNames = new List<string>() { itemName };
                this.isExpanded = new AnimBool(false);
            }
            public Category(string categoryName, List<string> itemNames)
            {
                this.categoryName = categoryName;
                this.itemNames = new List<string>(itemNames);
                this.isExpanded = new AnimBool(false);
            }

            public void AddItemName(string itemName, bool saveAssets = false)
            {
                itemName = itemName.Trim();
                if(string.IsNullOrEmpty(itemName)) { return; }
                if(itemNames == null) { itemNames = new List<string>(); }
                if(itemNames.Contains(itemName)) { return; }
                itemNames.Add(itemName);
                SortItemNames();
                QUI.SetDirty(Instance);
                if(saveAssets) { AssetDatabase.SaveAssets(); }
            }
            public void AddItemNames(List<string> itemNames, bool saveAssets = false)
            {
                if(itemNames == null || itemNames.Count == 0) { return; }
                for(int i = 0; i < itemNames.Count; i++)
                {
                    AddItemName(itemNames[i], false);
                }
                SortItemNames();
                QUI.SetDirty(Instance);
                if(saveAssets) { AssetDatabase.SaveAssets(); }
            }

            /// <summary>
            /// Returns the index of the given itemName or -1
            /// </summary>
            public bool Contains(string itemName)
            {
                if(itemNames == null || itemNames.Count == 0) { return false; }
                for(int i = 0; i < itemNames.Count; i++) { if(itemNames[i] == itemName) { return true; } }
                return false;
            }
            public int IndexOf(string itemName)
            {
                if(itemNames == null || itemNames.Count == 0 || !Contains(itemName)) { return -1; }
                return itemNames.IndexOf(itemName);
            }
            public void RemoveEmptyItems()
            {
                if(itemNames == null) { itemNames = new List<string>(); }
                for(int i = itemNames.Count - 1; i >= 0; i--)
                {
                    if(itemNames[i].Trim().IsNullOrEmpty())
                    {
                        itemNames.RemoveAt(i);
                    }

                }
            }
            public void SortItemNames()
            {
                if(itemNames == null) { itemNames = new List<string>(); }
                itemNames.Sort();
            }
            public bool IsEmpty()
            {
                if(itemNames == null)
                {
                    itemNames = new List<string>();
                }
                return itemNames.Count == 0;
            }
        }

        [Serializable]
        public class SoundsDatabase
        {
            public List<UISound> sounds;
            public List<string> soundNames;

            public UISound CreateUISound(string soundName, SoundType soundType, AudioClip audioClip = null)
            {
                soundName = soundName.Trim(); //trim the soundName - just in case
                if(string.IsNullOrEmpty(soundName)) { return null; } //sanity check
                if(Contains(soundName)) //found another UISound with this sound name -> ask the dev how to proceed
                {
                    UISound uis = Q.GetResource<UISound>(DUI.RESOURCES_PATH_UISOUNDS, soundName); //get the asset file

                    if(uis.soundType == soundType) //check the sound type
                    {
                        return uis;
                    }

                    if((uis.soundType == SoundType.UIButtons && soundType == SoundType.UIElements) //check if the soundType can be changed to ALL
                        || (uis.soundType == SoundType.UIElements && soundType == SoundType.UIButtons)
                        || soundType == SoundType.All)
                    {
                        Undo.RecordObjects(new UnityEngine.Object[] { uis, Instance }, "UpdateUISound"); //add an undo for this change
                        uis.soundType = SoundType.All; //set the ALL soundType
                        QUI.SetDirty(uis); //mark the scriptable object as dirty
                        sounds[IndexOf(soundName)] = uis; //set the reference to the database
                        QUI.SetDirty(Instance); //set the database scriptable object as dirty
                        AssetDatabase.SaveAssets(); //save assets
                        return uis; //return a reference to the updated UISound
                    }
                }

                UISound newUIS = Q.CreateAsset<UISound>(DUI.RELATIVE_PATH_UISOUNDS, soundName); //create a new asset file (scriptable object)
                newUIS.soundName = soundName; //set the soundName value
                newUIS.soundType = soundType; //set the soundType value
                newUIS.audioClip = audioClip; //set the audioClip reference (can be null)
                QUI.SetDirty(newUIS); //mark the newly created asset file (scriptable object) as dirty

                AddUISound(newUIS, false); //add a reference to the newly created scriptable object to the database (but do not save the assets database, yet)
                QUI.SetDirty(Instance); //mark the database as dirty
                AssetDatabase.SaveAssets(); //save the assets database
                AssetDatabase.Refresh(); //refresh the assets database (in order for the file to become visible in the ProjectView)

                return newUIS; //return a reference to the newly created UISound
            }

            public void DeleteUISound(string soundName)
            {
                if(sounds == null || sounds.Count == 0 || !Contains(soundName)) { return; } //database is null or empty or does not contain the name -> return
                sounds.RemoveAt(IndexOf(soundName)); //remove the entry from the database
                UpdateSoundsNamesList(); //update the names list
                Q.MoveAssetToTrash(DUI.RELATIVE_PATH_UISOUNDS, soundName, true, true, true); //move the asset file to trash
            }

            public UISound AddUISound(UISound uis, bool saveAssets = false)
            {
                if(uis == null) { return null; }
                if(sounds == null) { sounds = new List<UISound>(); }
                if(Contains(uis.soundName)) { return sounds[IndexOf(uis.soundName)]; }
                sounds.Add(uis);
                UpdateSoundsNamesList();
                QUI.SetDirty(Instance);
                if(saveAssets) { AssetDatabase.SaveAssets(); }
                return sounds[IndexOf(uis.soundName)];
            }

            public UISound GetUISound(string soundName)
            {
                if(sounds == null) { sounds = new List<UISound>(); } //database is null -> initialize it
                if(Contains(soundName)) { return sounds[IndexOf(soundName)]; } //found a match -> return the reference
                return null; //the given soundName does not exist in the database -> return null
            }

            public UISound GetUISound(AudioClip audioClip)
            {
                if(sounds == null) { sounds = new List<UISound>(); } //database is null -> initialize it
                if(audioClip == null) { return null; } //check that the audioClip reference is not null
                if(Contains(audioClip)) { return sounds[IndexOf(audioClip)]; } //found a match -> return the reference
                return null; //the given audioClip does not exist in the database -> return null
            }

            public void SortSoundsBySoundName()
            {
                sounds.Sort((a, b) => a.soundName.CompareTo(b.soundName));
            }

            public void UpdateSoundsNamesList()
            {
                if(sounds == null) { sounds = new List<UISound>(); } //database is null -> initialize it

                if(soundNames == null)
                {
                    soundNames = new List<string>(); //soundNames list is null -> initialize it
                }
                else
                {
                    soundNames.Clear(); //soundNames list is not null -> clear it
                }

                SortSoundsBySoundName(); //sort the categories list by soundName

                for(int i = 0; i < sounds.Count; i++) //populate the soundNames array with the sorted values
                {
                    soundNames.Add(sounds[i].soundName);
                }
            }

            public bool Contains(string soundName)
            {
                if(sounds == null || sounds.Count == 0) { return false; }
                for(int i = 0; i < sounds.Count; i++)
                {
                    if(sounds[i].soundName.Equals(soundName))
                    {
                        return true;
                    }
                }
                return false;
            }
            public bool Contains(string soundName, SoundType soundType)
            {
                if(sounds == null || sounds.Count == 0) { return false; }
                for(int i = 0; i < sounds.Count; i++)
                {
                    if(sounds[i].soundName.Equals(soundName)
                       && (sounds[i].soundType == soundType || sounds[i].soundType == SoundType.All))
                    {
                        return true;
                    }
                }
                return false;
            }
            public bool Contains(AudioClip audioClip)
            {
                if(sounds == null || sounds.Count == 0) { return false; }
                for(int i = 0; i < sounds.Count; i++)
                {
                    if(sounds[i].audioClip == audioClip) { return true; }
                }
                return false;
            }
            public bool Contains(AudioClip audioClip, SoundType soundType)
            {
                if(sounds == null || sounds.Count == 0) { return false; }
                for(int i = 0; i < sounds.Count; i++)
                {
                    if(sounds[i].audioClip == audioClip
                       && (sounds[i].soundType == soundType || sounds[i].soundType == SoundType.All))
                    {
                        return true;
                    }
                }
                return false;
            }

            public int IndexOf(string soundName)
            {
                if(sounds == null || sounds.Count == 0) { return -1; }
                for(int i = 0; i < sounds.Count; i++)
                {
                    if(sounds[i].soundName.Equals(soundName)) { return i; }
                }
                return -1;
            }
            public int IndexOf(AudioClip audioClip)
            {
                if(sounds == null || sounds.Count == 0) { return -1; }
                for(int i = 0; i < sounds.Count; i++)
                {
                    if(sounds[i].audioClip == audioClip) { return i; }
                }
                return -1;
            }

            public SoundType GetSoundType(string soundName)
            {
                return sounds[IndexOf(soundName)].soundType;
            }
            public SoundType GetSoundType(AudioClip audioClip)
            {
                return sounds[IndexOf(audioClip)].soundType;
            }
        }

        [Serializable]
        public class AnimDatabase
        {
            public List<AnimCategory> categories;
            public List<string> categoryNames;

            public AnimCategory AddCategory(string categoryName, string relativePath, bool saveAssets = false)
            {
                categoryName = categoryName.Trim();
                if(categories == null) { categories = new List<AnimCategory>(); }
                if(ContainsCategory(categoryName)) { return categories[CategoryIndex(categoryName)]; }

                if(!AssetDatabase.IsValidFolder(relativePath + categoryName)) //check that the folder exists -> if not, create it
                {
                    AssetDatabase.CreateFolder(relativePath.Remove(relativePath.Length - 1), categoryName);
                }

                categories.Add(new AnimCategory(categoryName));
                UpdateCategoriesNamesList();
                QUI.SetDirty(Instance);
                if(saveAssets) { AssetDatabase.SaveAssets(); }
                return categories[CategoryIndex(categoryName)];
            }
            public void RemoveCategory(string categoryName, string relativePath, bool saveAssets = false)
            {
                if(categories == null) { categories = new List<AnimCategory>(); }
                if(!ContainsCategory(categoryName)) { return; }
                categoryNames.RemoveAt(CategoryNameIndex(categoryName)); //remove categoryName from the names list
                categories.RemoveAt(CategoryIndex(categoryName)); //remove category info from the database
                if(saveAssets) { AssetDatabase.SaveAssets(); }
                FileUtil.DeleteFileOrDirectory(relativePath + categoryName); //delete the folder
                AssetDatabase.Refresh();
            }

            public AnimCategory GetCategory(string categoryName)

            {
                if(categories == null) { categories = new List<AnimCategory>(); }
                int index = CategoryIndex(categoryName); //check to see if the Database contains a category with the given category name
                if(index < 0) //database does not contain a category with the given category name
                {
                    return null;
                }
                return categories[index]; //the category was found -> return a reference to it
            }

            public bool ContainsCategoryName(string categoryName)
            {
                if(categoryNames == null || categoryNames.Count == 0)
                {
                    return false;
                }
                for(int i = 0; i < categoryNames.Count; i++)
                {
                    if(categoryNames[i].Equals(categoryName))
                    {
                        return true;
                    }
                }
                return false;
            }
            public bool ContainsCategory(string categoryName)
            {
                if(categories == null || categories.Count == 0)
                {
                    return false;
                }
                for(int i = 0; i < categories.Count; i++)
                {
                    if(categories[i].categoryName.Equals(categoryName))
                    {
                        return true;
                    }
                }
                return false;
            }
            public bool Contains(string categoryName, string itemName)
            {
                if(!ContainsCategoryName(categoryName)) { return false; }
                return GetCategory(categoryName).Contains(itemName);
            }

            /// <summary>
            /// Returns the index of the categoryName from the categoryNames list or -1
            /// </summary>
            public int CategoryNameIndex(string categoryName)
            {
                for(int i = 0; i < categoryNames.Count; i++)
                {
                    if(categoryNames[i].Equals(categoryName))
                    {
                        return i;
                    }
                }
                return -1;
            }
            /// <summary>
            /// Returns the index of the categoryName from the categories list or -1
            /// </summary>
            public int CategoryIndex(string categoryName)
            {
                for(int i = 0; i < categories.Count; i++)
                {
                    if(categories[i].categoryName.Equals(categoryName))
                    {
                        return i;
                    }
                }
                return -1;
            }
            /// <summary>
            /// Returns the index of the presetName from the given categoryName or -1
            /// </summary>
            public int ItemNameIndex(string categoryName, string presetName)
            {
                if(!ContainsCategory(categoryName)) { return -1; } //category does not exist -> return -1
                return GetCategory(categoryName).IndexOf(presetName);
            }

            public void Clear()
            {
                if(categories == null) { categories = new List<AnimCategory>(); }
                categories.Clear();
                if(categoryNames == null) { categoryNames = new List<string>(); }
                categoryNames.Clear();
            }
            public bool IsEmpty(string categoryName)
            {
                if(categories == null || categories.Count == 0 || !ContainsCategoryName(categoryName))
                {
                    return true;
                }
                else
                {
                    return GetCategory(categoryName).IsEmpty();
                }
            }

            public void UpdateCategoriesNamesList()
            {
                if(categories == null) { categories = new List<AnimCategory>(); } //database is null -> initialize it

                if(categoryNames == null)
                {
                    categoryNames = new List<string>(); //categoryNames list is null -> initialize it
                }
                else
                {
                    categoryNames.Clear(); //categoryNames list is not null -> clear it
                }

                SortCategoriesByCategoryName(); //sort the categories list by categoryName

                for(int i = 0; i < categories.Count; i++) //populate the categoryNames array with the sorted values
                {
                    categoryNames.Add(categories[i].categoryName);
                }
            }
            public void SortCategoriesByCategoryName()
            {
                if(categories == null) { categories = new List<AnimCategory>(); }
                categories.Sort((a, b) => a.categoryName.CompareTo(b.categoryName));
            }
            public void SortPresetNamesInCategories()
            {
                for(int i = 0; i < categories.Count; i++)
                {
                    categories[i].SortPresetsByPresetName();
                }
            }
            public bool RemoveCategoriesWithNoName()
            {
                bool itemsWereRemoved = false;
                if(categories == null) { categories = new List<AnimCategory>(); }
                for(int i = categories.Count - 1; i >= 0; i--)
                {
                    if(categories[i].categoryName.Trim().IsNullOrEmpty())
                    {
                        categories.RemoveAt(i);
                        itemsWereRemoved = true;
                    }
                }
                return itemsWereRemoved;
            }

            public void ExpandOrCollapseAllCategories(bool expand)
            {
                for(int i = 0; i < categories.Count; i++)
                {
                    categories[i].isExpanded.target = expand;
                    categories[i].ExpandOrCollapseAllAnimData(false);
                }
            }
        }

        [Serializable]
        public class AnimCategory
        {
            public string categoryName = string.Empty;
            public List<AnimData> presets = new List<AnimData>();
            public List<string> presetNames = new List<string>();
            public AnimBool isExpanded = new AnimBool(false);

            public AnimCategory(string categoryName)
            {
                this.categoryName = categoryName;
                this.presets = new List<AnimData>();
                this.presetNames = new List<string>();
                this.isExpanded = new AnimBool(false);
            }

            public AnimData CreateAnimData(AnimData ad, string resourcesPath, string relativePath, bool saveAssets = true, bool refreshAssets = true)
            {
                ad.presetName = ad.presetName.Trim(); //trim the name - just in case
                if(string.IsNullOrEmpty(ad.presetName)) { return null; } //sanity check
                if(Contains(ad.presetName)) //found another preset with this name -> ask the dev how to proceed
                {
                    AnimData asset = Q.GetResource<AnimData>(resourcesPath, ad.presetName); //get the asset file

                    if(asset != null)
                    {
                        return asset;
                    }
                }

                AnimData newAsset = Q.CreateAsset<AnimData>(relativePath + categoryName + "/", ad.presetName); //create a new asset file (scriptable object)
                newAsset.presetCategory = ad.presetCategory;
                newAsset.presetName = ad.presetName;
                newAsset.data = ad.data.Copy();
                QUI.SetDirty(newAsset); //mark the newly created asset file (scriptable object) as dirty

                AddAnimData(newAsset, false); //add a reference to the newly created scriptable object to the database (but do not save the assets database, yet)
                QUI.SetDirty(Instance); //mark the database as dirty

                if(saveAssets)
                {
                    AssetDatabase.SaveAssets(); //save the assets database
                }
                if(refreshAssets)
                {
                    AssetDatabase.Refresh(); //refresh the assets database (in order for the file to become visible in the ProjectView)
                }

                return newAsset; //return a reference to the newly created asset
            }
            public void DeleteAnimData(string presetName, string relativePath)
            {
                if(presets == null || presets.Count == 0 || !Contains(presetName)) { return; } //database is null or empty or it does not contain the name -> return
                if(categoryName.Equals(UIAnimatorUtil.UNCATEGORIZED_CATEGORY_NAME) && presetName.Equals(UIAnimatorUtil.DEFAULT_PRESET_NAME))
                {
                    QUI.DisplayDialog("Delete Preset", "You cannot and should not delete the '" + UIAnimatorUtil.DEFAULT_PRESET_NAME + "' preset.", "OK");
                    return;
                }
                Q.MoveAssetToTrash(relativePath + categoryName + "/", presetName, false, false, true); //move the asset file to trash
                presets.RemoveAt(IndexOf(presetName)); //remove the entry from the database
                UpdatePresetNamesList(); //update the names list
                QUI.SetDirty(Instance);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
            }

            public AnimData AddAnimData(AnimData ad, bool saveAssets = false)
            {
                if(ad == null) { return null; }
                if(presets == null) { presets = new List<AnimData>(); }
                if(Contains(ad.presetName)) { return presets[IndexOf(ad.presetName)]; }
                presets.Add(ad);
                UpdatePresetNamesList();
                QUI.SetDirty(Instance);
                if(saveAssets) { AssetDatabase.SaveAssets(); }
                return presets[IndexOf(ad.presetName)];
            }
            public AnimData GetAnimData(string presetName)
            {
                if(presets == null) { presets = new List<AnimData>(); } //database is null -> initialize it
                if(Contains(presetName)) { return presets[IndexOf(presetName)]; } //found a match -> return the reference
                return null; //the given name does not exist in the database -> return null
            }

            public void SortPresetsByPresetName()
            {
                presets.Sort((a, b) => a.presetName.CompareTo(b.presetName));
            }
            public void UpdatePresetNamesList()
            {
                if(presets == null) { presets = new List<AnimData>(); } //database is null -> initialize it

                if(presetNames == null)
                {
                    presetNames = new List<string>(); //list is null -> initialize it
                }
                else
                {
                    presetNames.Clear(); //list is not null -> clear it
                }

                SortPresetsByPresetName(); //sort the list by name

                for(int i = 0; i < presets.Count; i++) //populate the array with the sorted values
                {
                    presetNames.Add(presets[i].presetName);
                }
            }
            public bool Contains(string presetName)
            {
                if(presets == null || presets.Count == 0) { return false; }
                for(int i = 0; i < presets.Count; i++)
                {
                    if(presets[i].presetName.Equals(presetName))
                    {
                        return true;
                    }
                }
                return false;
            }
            public int IndexOf(string presetName)
            {
                if(presets == null || presets.Count == 0) { return -1; }
                for(int i = 0; i < presets.Count; i++)
                {
                    if(presets[i].presetName.Equals(presetName))
                    {
                        return i;
                    }
                }
                return -1;
            }
            public void ExpandOrCollapseAllAnimData(bool expand)
            {
                for(int i = 0; i < presets.Count; i++)
                {
                    presets[i].isExpanded.target = expand;
                }
            }
            public bool IsEmpty()
            {
                if(presets == null)
                {
                    presets = new List<AnimData>();
                }
                return presets.Count == 0;
            }
        }

        [Serializable]
        public class LoopDatabase
        {
            public List<LoopCategory> categories;
            public List<string> categoryNames;

            public LoopCategory AddCategory(string categoryName, string relativePath, bool saveAssets = false)
            {
                categoryName = categoryName.Trim();
                if(categories == null) { categories = new List<LoopCategory>(); }
                if(ContainsCategory(categoryName)) { return categories[CategoryIndex(categoryName)]; }

                if(!AssetDatabase.IsValidFolder(relativePath + categoryName)) //check that the folder exists -> if not, create it
                {
                    AssetDatabase.CreateFolder(relativePath.Remove(relativePath.Length - 1), categoryName);
                }

                categories.Add(new LoopCategory(categoryName));
                UpdateCategoriesNamesList();
                QUI.SetDirty(Instance);
                if(saveAssets) { AssetDatabase.SaveAssets(); }
                return categories[CategoryIndex(categoryName)];
            }
            public void RemoveCategory(string categoryName, string relativePath, bool saveAssets = false)
            {
                if(categories == null) { categories = new List<LoopCategory>(); }
                if(!ContainsCategory(categoryName)) { return; }
                categoryNames.RemoveAt(CategoryNameIndex(categoryName)); //remove categoryName from the names list
                categories.RemoveAt(CategoryIndex(categoryName)); //remove category info from the database
                if(saveAssets) { AssetDatabase.SaveAssets(); }
                FileUtil.DeleteFileOrDirectory(relativePath + categoryName); //delete the folder
                AssetDatabase.Refresh();
            }

            public LoopCategory GetCategory(string categoryName)
            {
                if(categories == null) { categories = new List<LoopCategory>(); }
                int index = CategoryIndex(categoryName); //check to see if the Database contains a category with the given category name
                if(index < 0) //database does not contain a category with the given category name
                {
                    return null;
                }
                return categories[index]; //the category was found -> return a reference to it
            }

            public bool ContainsCategoryName(string categoryName)
            {
                if(categoryNames == null || categoryNames.Count == 0)
                {
                    return false;
                }
                for(int i = 0; i < categoryNames.Count; i++)
                {
                    if(categoryNames[i].Equals(categoryName))
                    {
                        return true;
                    }
                }
                return false;
            }
            public bool ContainsCategory(string categoryName)
            {
                if(categories == null || categories.Count == 0)
                {
                    return false;
                }
                for(int i = 0; i < categories.Count; i++)
                {
                    if(categories[i].categoryName.Equals(categoryName))
                    {
                        return true;
                    }
                }
                return false;
            }
            public bool Contains(string categoryName, string itemName)
            {
                if(!ContainsCategoryName(categoryName)) { return false; }
                return GetCategory(categoryName).Contains(itemName);
            }

            /// <summary>
            /// Returns the index of the categoryName from the categoryNames list or -1
            /// </summary>
            public int CategoryNameIndex(string categoryName)
            {
                for(int i = 0; i < categoryNames.Count; i++)
                {
                    if(categoryNames[i].Equals(categoryName))
                    {
                        return i;
                    }
                }
                return -1;
            }
            /// <summary>
            /// Returns the index of the categoryName from the categories list or -1
            /// </summary>
            public int CategoryIndex(string categoryName)
            {
                for(int i = 0; i < categories.Count; i++)
                {
                    if(categories[i].categoryName.Equals(categoryName))
                    {
                        return i;
                    }
                }
                return -1;
            }
            /// <summary>
            /// Returns the index of the presetName from the given categoryName or -1
            /// </summary>
            public int ItemNameIndex(string categoryName, string presetName)
            {
                if(!ContainsCategory(categoryName)) { return -1; } //category does not exist -> return -1
                return GetCategory(categoryName).IndexOf(presetName);
            }

            public void Clear()
            {
                if(categories == null) { categories = new List<LoopCategory>(); }
                categories.Clear();
                if(categoryNames == null) { categoryNames = new List<string>(); }
                categoryNames.Clear();
            }
            public bool IsEmpty(string categoryName)
            {
                if(categories == null || categories.Count == 0 || !ContainsCategoryName(categoryName))
                {
                    return true;
                }
                else
                {
                    return GetCategory(categoryName).IsEmpty();
                }
            }

            public void UpdateCategoriesNamesList()
            {
                if(categories == null) { categories = new List<LoopCategory>(); } //database is null -> initialize it

                if(categoryNames == null)
                {
                    categoryNames = new List<string>(); //categoryNames list is null -> initialize it
                }
                else
                {
                    categoryNames.Clear(); //categoryNames list is not null -> clear it
                }

                SortCategoriesByCategoryName(); //sort the categories list by categoryName

                for(int i = 0; i < categories.Count; i++) //populate the categoryNames array with the sorted values
                {
                    categoryNames.Add(categories[i].categoryName);
                }
            }
            public void SortCategoriesByCategoryName()
            {
                categories.Sort((a, b) => a.categoryName.CompareTo(b.categoryName));
            }
            public void SortPresetNamesInCategories()
            {
                for(int i = 0; i < categories.Count; i++)
                {
                    categories[i].SortPresetsByPresetName();
                }
            }
            public bool RemoveCategoriesWithNoName()
            {
                bool itemsWereRemoved = false;
                if(categories == null) { categories = new List<LoopCategory>(); }
                for(int i = categories.Count - 1; i >= 0; i--)
                {
                    if(categories[i].categoryName.Trim().IsNullOrEmpty())
                    {
                        categories.RemoveAt(i);
                        itemsWereRemoved = true;
                    }
                }
                return itemsWereRemoved;
            }

            public void ExpandOrCollapseAllCategories(bool expand)
            {
                for(int i = 0; i < categories.Count; i++)
                {
                    categories[i].isExpanded.target = expand;
                    categories[i].ExpandOrCollapseAllAnimData(false);
                }
            }
        }

        [Serializable]
        public class LoopCategory
        {
            public string categoryName = string.Empty;
            public List<LoopData> presets = new List<LoopData>();
            public List<string> presetNames = new List<string>();
            public AnimBool isExpanded = new AnimBool(false);

            public LoopCategory(string categoryName)
            {
                this.categoryName = categoryName;
                this.presets = new List<LoopData>();
                this.presetNames = new List<string>();
                this.isExpanded = new AnimBool(false);
            }

            public LoopData CreateLoopData(LoopData ld, string resourcesPath, string relativePath, bool saveAssets = true, bool refreshAssets = true)
            {
                ld.presetName = ld.presetName.Trim(); //trim the name - just in case
                if(string.IsNullOrEmpty(ld.presetName)) { return null; } //sanity check
                if(Contains(ld.presetName)) //found another preset with this name -> ask the dev how to proceed
                {
                    LoopData asset = Q.GetResource<LoopData>(resourcesPath, ld.presetName); //get the asset file

                    if(asset != null)
                    {
                        return asset;
                    }
                }

                LoopData newAsset = Q.CreateAsset<LoopData>(relativePath + categoryName + "/", ld.presetName); //create a new asset file (scriptable object)
                newAsset.presetCategory = ld.presetCategory;
                newAsset.presetName = ld.presetName;
                newAsset.data = ld.data.Copy();
                QUI.SetDirty(newAsset); //mark the newly created asset file (scriptable object) as dirty

                AddLoopData(newAsset, false); //add a reference to the newly created scriptable object to the database (but do not save the assets database, yet)
                QUI.SetDirty(Instance); //mark the database as dirty

                if(saveAssets)
                {
                    AssetDatabase.SaveAssets(); //save the assets database
                }
                if(refreshAssets)
                {
                    AssetDatabase.Refresh(); //refresh the assets database (in order for the file to become visible in the ProjectView)
                }

                return newAsset; //return a reference to the newly created asset
            }
            public void DeleteLoopData(string presetName, string relativePath)
            {
                if(presets == null || presets.Count == 0 || !Contains(presetName)) { return; } //database is null or empty or it does not contain the name -> return
                if(categoryName.Equals(UIAnimatorUtil.UNCATEGORIZED_CATEGORY_NAME) && presetName.Equals(UIAnimatorUtil.DEFAULT_PRESET_NAME))
                {
                    QUI.DisplayDialog("Delete Preset", "You cannot and should not delete the '" + UIAnimatorUtil.DEFAULT_PRESET_NAME + "' preset.", "OK");
                    return;
                }
                Q.MoveAssetToTrash(relativePath, presetName, false, false, true); //move the asset file to trash
                presets.RemoveAt(IndexOf(presetName)); //remove the entry from the database
                UpdatePresetNamesList(); //update the names list
                QUI.SetDirty(Instance);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
            }

            public LoopData AddLoopData(LoopData ld, bool saveAssets = false)
            {
                if(ld == null) { return null; }
                if(presets == null) { presets = new List<LoopData>(); }
                if(Contains(ld.presetName)) { return presets[IndexOf(ld.presetName)]; }
                presets.Add(ld);
                UpdatePresetNamesList();
                QUI.SetDirty(Instance);
                if(saveAssets) { AssetDatabase.SaveAssets(); }
                return presets[IndexOf(ld.presetName)];
            }
            public LoopData GetLoopData(string presetName)
            {
                if(presets == null) { presets = new List<LoopData>(); } //database is null -> initialize it
                if(Contains(presetName)) { return presets[IndexOf(presetName)]; } //found a match -> return the reference
                return null; //the given name does not exist in the database -> return null
            }

            public void SortPresetsByPresetName()
            {
                presets.Sort((a, b) => a.presetName.CompareTo(b.presetName));
            }
            public void UpdatePresetNamesList()
            {
                if(presets == null) { presets = new List<LoopData>(); } //database is null -> initialize it

                if(presetNames == null)
                {
                    presetNames = new List<string>(); //list is null -> initialize it
                }
                else
                {
                    presetNames.Clear(); //list is not null -> clear it
                }

                SortPresetsByPresetName(); //sort the list by name

                for(int i = 0; i < presets.Count; i++) //populate the array with the sorted values
                {
                    presetNames.Add(presets[i].presetName);
                }
            }
            public bool Contains(string presetName)
            {
                if(presets == null || presets.Count == 0) { return false; }
                for(int i = 0; i < presets.Count; i++)
                {
                    if(presets[i].presetName.Equals(presetName))
                    {
                        return true;
                    }
                }
                return false;
            }
            public int IndexOf(string presetName)
            {
                if(presets == null || presets.Count == 0) { return -1; }
                for(int i = 0; i < presets.Count; i++)
                {
                    if(presets[i].presetName.Equals(presetName))
                    {
                        return i;
                    }
                }
                return -1;
            }
            public void ExpandOrCollapseAllAnimData(bool expand)
            {
                for(int i = 0; i < presets.Count; i++)
                {
                    presets[i].isExpanded.target = expand;
                }
            }
            public bool IsEmpty()
            {
                if(presets == null)
                {
                    presets = new List<LoopData>();
                }
                return presets.Count == 0;
            }
        }

        [Serializable]
        public class PunchDatabase
        {
            public List<PunchCategory> categories;
            public List<string> categoryNames;

            public PunchCategory AddCategory(string categoryName, string relativePath, bool saveAssets = false)
            {
                categoryName = categoryName.Trim();
                if(categories == null) { categories = new List<PunchCategory>(); }
                if(ContainsCategory(categoryName)) { return categories[CategoryIndex(categoryName)]; }

                if(!AssetDatabase.IsValidFolder(relativePath + categoryName)) //check that the folder exists -> if not, create it
                {
                    AssetDatabase.CreateFolder(relativePath.Remove(relativePath.Length - 1), categoryName);
                }

                categories.Add(new PunchCategory(categoryName));
                UpdateCategoriesNamesList();
                QUI.SetDirty(Instance);
                if(saveAssets) { AssetDatabase.SaveAssets(); }
                return categories[CategoryIndex(categoryName)];
            }
            public void RemoveCategory(string categoryName, string relativePath, bool saveAssets = false)
            {
                if(categories == null) { categories = new List<PunchCategory>(); }
                if(!ContainsCategory(categoryName)) { return; }
                categoryNames.RemoveAt(CategoryNameIndex(categoryName)); //remove categoryName from the names list
                categories.RemoveAt(CategoryIndex(categoryName)); //remove category info from the database
                if(saveAssets) { AssetDatabase.SaveAssets(); }
                FileUtil.DeleteFileOrDirectory(relativePath + categoryName); //delete the folder
                AssetDatabase.Refresh();
            }

            public PunchCategory GetCategory(string categoryName)
            {
                if(categories == null) { categories = new List<PunchCategory>(); }
                int index = CategoryIndex(categoryName); //check to see if the Database contains a category with the given category name
                if(index < 0) //database does not contain a category with the given category name
                {
                    return null;
                }
                return categories[index]; //the category was found -> return a reference to it
            }

            public bool ContainsCategoryName(string categoryName)
            {
                if(categoryNames == null || categoryNames.Count == 0)
                {
                    return false;
                }
                for(int i = 0; i < categoryNames.Count; i++)
                {
                    if(categoryNames[i].Equals(categoryName))
                    {
                        return true;
                    }
                }
                return false;
            }
            public bool ContainsCategory(string categoryName)
            {
                if(categories == null || categories.Count == 0)
                {
                    return false;
                }
                for(int i = 0; i < categories.Count; i++)
                {
                    if(categories[i].categoryName.Equals(categoryName))
                    {
                        return true;
                    }
                }
                return false;
            }
            public bool Contains(string categoryName, string itemName)
            {
                if(!ContainsCategoryName(categoryName)) { return false; }
                return GetCategory(categoryName).Contains(itemName);
            }

            /// <summary>
            /// Returns the index of the categoryName from the categoryNames list or -1
            /// </summary>
            public int CategoryNameIndex(string categoryName)
            {
                for(int i = 0; i < categoryNames.Count; i++)
                {
                    if(categoryNames[i].Equals(categoryName))
                    {
                        return i;
                    }
                }
                return -1;
            }
            /// <summary>
            /// Returns the index of the categoryName from the categories list or -1
            /// </summary>
            public int CategoryIndex(string categoryName)
            {
                for(int i = 0; i < categories.Count; i++)
                {
                    if(categories[i].categoryName.Equals(categoryName))
                    {
                        return i;
                    }
                }
                return -1;
            }
            /// <summary>
            /// Returns the index of the presetName from the given categoryName or -1
            /// </summary>
            public int ItemNameIndex(string categoryName, string presetName)
            {
                if(!ContainsCategory(categoryName)) { return -1; } //category does not exist -> return -1
                return GetCategory(categoryName).IndexOf(presetName);
            }

            public void Clear()
            {
                if(categories == null) { categories = new List<PunchCategory>(); }
                categories.Clear();
                if(categoryNames == null) { categoryNames = new List<string>(); }
                categoryNames.Clear();
            }
            public bool IsEmpty(string categoryName)
            {
                if(categories == null || categories.Count == 0 || !ContainsCategoryName(categoryName))
                {
                    return true;
                }
                else
                {
                    return GetCategory(categoryName).IsEmpty();
                }
            }

            public void UpdateCategoriesNamesList()
            {
                if(categories == null) { categories = new List<PunchCategory>(); } //database is null -> initialize it

                if(categoryNames == null)
                {
                    categoryNames = new List<string>(); //categoryNames list is null -> initialize it
                }
                else
                {
                    categoryNames.Clear(); //categoryNames list is not null -> clear it
                }

                SortCategoriesByCategoryName(); //sort the categories list by categoryName

                for(int i = 0; i < categories.Count; i++) //populate the categoryNames array with the sorted values
                {
                    categoryNames.Add(categories[i].categoryName);
                }
            }
            public void SortCategoriesByCategoryName()
            {
                categories.Sort((a, b) => a.categoryName.CompareTo(b.categoryName));
            }
            public void SortPresetNamesInCategories()
            {
                for(int i = 0; i < categories.Count; i++)
                {
                    categories[i].SortPresetsByPresetName();
                }
            }
            public bool RemoveCategoriesWithNoName()
            {
                bool itemsWereRemoved = false;
                if(categories == null) { categories = new List<PunchCategory>(); }
                for(int i = categories.Count - 1; i >= 0; i--)
                {
                    if(categories[i].categoryName.Trim().IsNullOrEmpty())
                    {
                        categories.RemoveAt(i);
                        itemsWereRemoved = true;
                    }
                }
                return itemsWereRemoved;
            }

            public void ExpandOrCollapseAllCategories(bool expand)
            {
                for(int i = 0; i < categories.Count; i++)
                {
                    categories[i].isExpanded.target = expand;
                    categories[i].ExpandOrCollapseAllAnimData(false);
                }
            }
        }

        [Serializable]
        public class PunchCategory
        {
            public string categoryName = string.Empty;
            public List<PunchData> presets = new List<PunchData>();
            public List<string> presetNames = new List<string>();
            public AnimBool isExpanded = new AnimBool(false);

            public PunchCategory(string categoryName)
            {
                this.categoryName = categoryName;
                this.presets = new List<PunchData>();
                this.presetNames = new List<string>();
                this.isExpanded = new AnimBool(false);
            }

            public PunchData CreatePunchData(PunchData pd, string resourcesPath, string relativePath, bool saveAssets = true, bool refreshAssets = true)
            {
                pd.presetName = pd.presetName.Trim(); //trim the name - just in case
                if(string.IsNullOrEmpty(pd.presetName)) { return null; } //sanity check
                if(Contains(pd.presetName)) //found another preset with this name -> ask the dev how to proceed
                {
                    PunchData asset = Q.GetResource<PunchData>(resourcesPath, pd.presetName); //get the asset file

                    if(asset != null)
                    {
                        return asset;
                    }
                }

                PunchData newAsset = Q.CreateAsset<PunchData>(relativePath + categoryName + "/", pd.presetName); //create a new asset file (scriptable object)
                newAsset.presetCategory = pd.presetCategory;
                newAsset.presetName = pd.presetName;
                newAsset.data = pd.data.Copy();
                QUI.SetDirty(newAsset); //mark the newly created asset file (scriptable object) as dirty

                AddPunchData(newAsset, false); //add a reference to the newly created scriptable object to the database (but do not save the assets database, yet)
                QUI.SetDirty(Instance); //mark the database as dirty

                if(saveAssets)
                {
                    AssetDatabase.SaveAssets(); //save the assets database
                }
                if(refreshAssets)
                {
                    AssetDatabase.Refresh(); //refresh the assets database (in order for the file to become visible in the ProjectView)
                }

                return newAsset; //return a reference to the newly created asset
            }
            public void DeletePunchData(string presetName, string relativePath)
            {
                if(presets == null || presets.Count == 0 || !Contains(presetName)) { return; } //database is null or empty or it does not contain the name -> return
                if(categoryName.Equals(UIAnimatorUtil.UNCATEGORIZED_CATEGORY_NAME) && presetName.Equals(UIAnimatorUtil.DEFAULT_PRESET_NAME))
                {
                    QUI.DisplayDialog("Delete Preset", "You cannot and should not delete the '" + UIAnimatorUtil.DEFAULT_PRESET_NAME + "' preset.", "OK");
                    return;
                }
                Q.MoveAssetToTrash(relativePath, presetName, false, false, true); //move the asset file to trash
                presets.RemoveAt(IndexOf(presetName)); //remove the entry from the database
                UpdatePresetNamesList(); //update the names list
                QUI.SetDirty(Instance);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
            }

            public PunchData AddPunchData(PunchData pd, bool saveAssets = false)
            {
                if(pd == null) { return null; }
                if(presets == null) { presets = new List<PunchData>(); }
                if(Contains(pd.presetName)) { return presets[IndexOf(pd.presetName)]; }
                presets.Add(pd);
                UpdatePresetNamesList();
                QUI.SetDirty(Instance);
                if(saveAssets) { AssetDatabase.SaveAssets(); }
                return presets[IndexOf(pd.presetName)];
            }
            public PunchData GetPunchData(string presetName)
            {
                if(presets == null) { presets = new List<PunchData>(); } //database is null -> initialize it
                if(Contains(presetName)) { return presets[IndexOf(presetName)]; } //found a match -> return the reference
                return null; //the given name does not exist in the database -> return null
            }

            public void SortPresetsByPresetName()
            {
                presets.Sort((a, b) => a.presetName.CompareTo(b.presetName));
            }
            public void UpdatePresetNamesList()
            {
                if(presets == null) { presets = new List<PunchData>(); } //database is null -> initialize it

                if(presetNames == null)
                {
                    presetNames = new List<string>(); //list is null -> initialize it
                }
                else
                {
                    presetNames.Clear(); //list is not null -> clear it
                }

                SortPresetsByPresetName(); //sort the list by name

                for(int i = 0; i < presets.Count; i++) //populate the array with the sorted values
                {
                    presetNames.Add(presets[i].presetName);
                }
            }
            public bool Contains(string presetName)
            {
                if(presets == null || presets.Count == 0) { return false; }
                for(int i = 0; i < presets.Count; i++)
                {
                    if(presets[i].presetName.Equals(presetName))
                    {
                        return true;
                    }
                }
                return false;
            }
            public int IndexOf(string presetName)
            {
                if(presets == null || presets.Count == 0) { return -1; }
                for(int i = 0; i < presets.Count; i++)
                {
                    if(presets[i].presetName.Equals(presetName))
                    {
                        return i;
                    }
                }
                return -1;
            }
            public void ExpandOrCollapseAllAnimData(bool expand)
            {
                for(int i = 0; i < presets.Count; i++)
                {
                    presets[i].isExpanded.target = expand;
                }
            }
            public bool IsEmpty()
            {
                if(presets == null)
                {
                    presets = new List<PunchData>();
                }
                return presets.Count == 0;
            }
        }
    }
}
