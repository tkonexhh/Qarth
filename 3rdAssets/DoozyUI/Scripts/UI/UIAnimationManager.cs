// Copyright (c) 2015 - 2018 Doozy Entertainment / Marlink Trading SRL. All Rights Reserved.
// This code can only be used under the standard Unity Asset Store End User License Agreement
// A Copy of the EULA APPENDIX 1 is available at http://unity3d.com/company/legal/as_terms

using UnityEngine;
using System.IO;

namespace DoozyUI
{
    [System.Serializable]
    [DisallowMultipleComponent]
    public class UIAnimationManager : MonoBehaviour
    {
#pragma warning disable 0612
#pragma warning disable 0219
        #region Enums - AnimationType, ButtonLoopType
        public enum AnimationType
        {
            IN,
            LOOP,
            OUT,
            OnClick,
            ButtonLoops
        }

        public enum ButtonLoopType
        {
            None,
            Normal,
            Highlighted
        }
        #endregion

        #region Constants
        public const string DEFAULT_PRESET_NAME = "DefaultPreset";
        #endregion

        #region Private Variables
        private UIElement uiElement = null;
        private UIButton uiButton = null;

        //Save and Load Settings
        private string uiAnimationsFolderPath = string.Empty;
        private string fileNameExtension = ".xml";
        #endregion

        #region Properties
        public UIElement GetUIElement
        {
            get
            {
                if (uiElement == null)
                {
                    uiElement = GetComponent<UIElement>();
                }
                return uiElement;
            }
        }

        public UIButton GetUIButton
        {
            get
            {
                if (uiButton == null)
                {
                    uiButton = GetComponent<UIButton>();
                }
                return uiButton;
            }
        }

        private string GetUIAnimationsFolderPath
        {
            get
            {
                if (string.IsNullOrEmpty(uiAnimationsFolderPath))
                {
                    uiAnimationsFolderPath = DUI.PATH + "/Presets/UIAnimations/";
                }
                return uiAnimationsFolderPath;
            }
        }
        #endregion

        #region Save Preset
        public void SavePreset(string presetName, AnimationType animationType, ButtonLoopType buttonLoopType = ButtonLoopType.None)
        {
            if (presetName.Equals(DEFAULT_PRESET_NAME))
            {
                Debug.Log("[DoozyUI] You cannot save a preset with the name '" + DEFAULT_PRESET_NAME + "'.");
                //return;
            }

            if (string.IsNullOrEmpty(presetName))
            {
                Debug.Log("[DoozyUI] You cannot save a preset with no name.");
                return;
            }

            string adjustedFolderPath = GetUIAnimationsFolderPath + animationType.ToString();
            string presetFileName = presetName + "_" + animationType.ToString() + fileNameExtension;
            string filePath = adjustedFolderPath + "/" + presetFileName;
            /*
            switch (animationType)
            {
                case AnimationType.IN:
                    InAnimations inAnimations = new InAnimations();
                    inAnimations = GetUIElement.GetInAnimations;
                    inAnimations.inAnimationsPresetName = presetName;
                    FileHelper.writeObjectToFile(filePath, inAnimations, FileHelper.SerializeXML);
                    //Debug.Log("[DoozyUI] [UIElement] the '" + presetName + "' IN Animations preset has been SAVED!");
                    break;

                case AnimationType.LOOP:
                    LoopAnimations loopAnimations = new LoopAnimations();
                    loopAnimations = GetUIElement.GetLoopAnimations;
                    loopAnimations.loopAnimationsPresetName = presetName;
                    FileHelper.writeObjectToFile(filePath, loopAnimations, FileHelper.SerializeXML);
                    //Debug.Log("[DoozyUI] [UIElement] the '" + presetName + "' LOOP Animations preset has been SAVED!");
                    break;

                case AnimationType.OUT:
                    OutAnimations outAnimations = new OutAnimations();
                    outAnimations = GetUIElement.GetOutAnimations;
                    outAnimations.outAnimationsPresetName = presetName;
                    FileHelper.writeObjectToFile(filePath, outAnimations, FileHelper.SerializeXML);
                    //Debug.Log("[DoozyUI] [UIElement] the '" + presetName + "' OUT Animations preset has been SAVED!");
                    break;

                case AnimationType.OnClick:
                    OnClickAnimations onClickAnimationSettings = new OnClickAnimations();
                    onClickAnimationSettings = GetUIButton.onClickAnimationSettings;
                    onClickAnimationSettings.onClickAnimationsPresetName = presetName;
                    FileHelper.writeObjectToFile(filePath, onClickAnimationSettings, FileHelper.SerializeXML);
                    //Debug.Log("[DoozyUI] [UIButton] the '" + presetName + "' OnClick Animations preset has been SAVED!");
                    break;

                case AnimationType.ButtonLoops:
                    switch (buttonLoopType)
                    {
                        case ButtonLoopType.Normal:
                            ButtonLoopsAnimations normalAnimationSettings = new ButtonLoopsAnimations();
                            normalAnimationSettings = GetUIButton.normalAnimationSettings;
                            normalAnimationSettings.animationsPresetName = presetName;
                            FileHelper.writeObjectToFile(filePath, normalAnimationSettings, FileHelper.SerializeXML);
                            //Debug.Log("[DoozyUI] [UIButton] the '" + presetName + "' Normal Animations preset has been SAVED!");
                            break;

                        case ButtonLoopType.Highlighted:
                            ButtonLoopsAnimations highlightedAnimationSettings = new ButtonLoopsAnimations();
                            highlightedAnimationSettings = GetUIButton.highlightedAnimationSettings;
                            highlightedAnimationSettings.animationsPresetName = presetName;
                            FileHelper.writeObjectToFile(filePath, highlightedAnimationSettings, FileHelper.SerializeXML);
                            //Debug.Log("[DoozyUI] [UIButton] the '" + presetName + "' Highlighted Animations preset has been SAVED!");
                            break;

                        case ButtonLoopType.None: //we should not be able to get here
                            Debug.Log("[DoozyUI] [UIButton] You are trying to save a preset named '" + presetName + "'. Something went wrong and the preset was not saved. This should not happen.");
                            break;

                        default:
                            Debug.Log("[DoozyUI] [UIButton] You are trying to save a preset named '" + presetName + "'. Something went wrong and the preset was not saved. This should not happen.");
                            break;
                    }
                    break;
            }
            */
            LoadPresetList(animationType);
            LoadPreset(presetName, animationType, buttonLoopType);
        }
        #endregion

        #region Load Preset
        public void LoadPreset(string presetName, AnimationType animationType, ButtonLoopType buttonLoopType = ButtonLoopType.None)
        {
            /*
            string adjustedFolderPath = GetUIAnimationsFolderPath + animationType.ToString();
            string presetFileName = presetName + "_" + animationType.ToString() + fileNameExtension;
            string filePath = adjustedFolderPath + "/" + presetFileName;

            switch (animationType)
            {
                case AnimationType.IN:
                    //UIElement.InAnimations inAnimations = new UIElement.InAnimations();
                    GetUIElement.SetInAnimations = FileHelper.readObjectFile<InAnimations>(filePath, FileHelper.DeserializeXML<InAnimations>);
                    for (int i = 0; i < GetUIElement.inAnimationsPresetNames.Length; i++)
                    {
                        if (GetUIElement.GetInAnimations.inAnimationsPresetName.Equals(GetUIElement.inAnimationsPresetNames[i]))
                        {
                            GetUIElement.activeInAnimationsPresetIndex = i;
                            break;
                        }
                    }
                    //Debug.Log("[DoozyUI] [UIElement] the '" + presetName + "' IN Animations preset has been LOADED!");
                    break;

                case AnimationType.LOOP:
                    //UIElement.LoopAnimations loopAnimations = new UIElement.LoopAnimations();
                    GetUIElement.SetLoopAnimations = FileHelper.readObjectFile<LoopAnimations>(filePath, FileHelper.DeserializeXML<LoopAnimations>);
                    for (int i = 0; i < GetUIElement.loopAnimationsPresetNames.Length; i++)
                    {
                        if (GetUIElement.GetLoopAnimations.loopAnimationsPresetName.Equals(GetUIElement.loopAnimationsPresetNames[i]))
                        {
                            GetUIElement.activeLoopAnimationsPresetIndex = i;
                            break;
                        }
                    }
                    //Debug.Log("[DoozyUI] [UIElement] the '" + presetName + "' LOOP Animations preset has been LOADED!");
                    break;

                case AnimationType.OUT:
                    //UIElement.OutAnimations outAnimations = new UIElement.OutAnimations();
                    GetUIElement.SetOutAnimations = FileHelper.readObjectFile<OutAnimations>(filePath, FileHelper.DeserializeXML<OutAnimations>);
                    for (int i = 0; i < GetUIElement.outAnimationsPresetNames.Length; i++)
                    {
                        if (GetUIElement.GetOutAnimations.outAnimationsPresetName.Equals(GetUIElement.outAnimationsPresetNames[i]))
                        {
                            GetUIElement.activeOutAnimationsPresetIndex = i;
                            break;
                        }
                    }
                    //Debug.Log("[DoozyUI] [UIElement] the '" + presetName + "' OUT Animations preset has been LOADED!");
                    break;

                case AnimationType.OnClick:
                    GetUIButton.onClickAnimationSettings = FileHelper.readObjectFile<OnClickAnimations>(filePath, FileHelper.DeserializeXML<OnClickAnimations>);
                    GetUIButton.onClickAnimationsPresetName = presetName;
                    for (int i = 0; i < GetUIButton.onClickAnimationsPresetNames.Length; i++)
                    {
                        if (GetUIButton.onClickAnimationsPresetName.Equals(GetUIButton.onClickAnimationsPresetNames[i]))
                        {
                            GetUIButton.activeOnclickAnimationsPresetIndex = i;
                            break;
                        }
                    }
                    //Debug.Log("[DoozyUI] [UIButton] the '" + presetName + "' OnClick Animations preset has been LOADED!");
                    break;

                case AnimationType.ButtonLoops:
                    switch (buttonLoopType)
                    {
                        case ButtonLoopType.Normal:
                            GetUIButton.normalAnimationSettings = FileHelper.readObjectFile<ButtonLoopsAnimations>(filePath, FileHelper.DeserializeXML<ButtonLoopsAnimations>);
                            GetUIButton.normalAnimationsPresetName = presetName;
                            for (int i = 0; i < GetUIButton.buttonLoopsAnimationsPresetNames.Length; i++)
                            {
                                if (GetUIButton.normalAnimationsPresetName.Equals(GetUIButton.buttonLoopsAnimationsPresetNames[i]))
                                {
                                    GetUIButton.activeNormalAnimationsPresetIndex = i;
                                    break;
                                }
                            }
                            //Debug.Log("[DoozyUI] [UIButton] the '" + presetName + "' Normal Animations preset has been LOADED!");
                            break;

                        case ButtonLoopType.Highlighted:
                            GetUIButton.highlightedAnimationSettings = FileHelper.readObjectFile<ButtonLoopsAnimations>(filePath, FileHelper.DeserializeXML<ButtonLoopsAnimations>);
                            GetUIButton.highlightedAnimationsPresetName = presetName;
                            for (int i = 0; i < GetUIButton.buttonLoopsAnimationsPresetNames.Length; i++)
                            {
                                if (GetUIButton.highlightedAnimationsPresetName.Equals(GetUIButton.buttonLoopsAnimationsPresetNames[i]))
                                {
                                    GetUIButton.activeHighlightedAnimationsPresetIndex = i;
                                    break;
                                }
                            }
                            //Debug.Log("[DoozyUI] [UIButton] the '" + presetName + "' Highlighted Animations preset has been LOADED!");
                            break;

                        case ButtonLoopType.None: //we should not be able to get here
                            Debug.Log("[DoozyUI] [UIButton] You are trying to load a preset named '" + presetName + "'. Something went wrong and the preset was not loaded. This should not happen.");
                            break;

                        default:
                            Debug.Log("[DoozyUI] [UIButton] You are trying to load a preset named '" + presetName + "'. Something went wrong and the preset was not loaded. This should not happen.");
                            break;
                    }
                    break;
            }
            */
        }
        #endregion

        #region Load PresetList
        public void LoadPresetList(AnimationType animationType)
        {
            /*
            string adjustedFolderPath = GetUIAnimationsFolderPath + animationType.ToString();
            string[] presetFilesInfo = Directory.GetFiles(adjustedFolderPath, "*" + fileNameExtension);
            int fileCount = presetFilesInfo.Length;

            switch (animationType)
            {
                case AnimationType.IN:
                    if (fileCount > 0)
                    {
                        GetUIElement.inAnimationsPresetNames = new string[fileCount];
                        fileCount = 0;
                        for (int i = 0; i < presetFilesInfo.Length; i++)
                        {
                            GetUIElement.inAnimationsPresetNames[fileCount] = FileHelper.readObjectFile<InAnimations>(presetFilesInfo[i], FileHelper.DeserializeXML<InAnimations>).inAnimationsPresetName;
                            fileCount++;
                        }
                    }
                    break;

                case AnimationType.LOOP:
                    if (fileCount > 0)
                    {
                        GetUIElement.loopAnimationsPresetNames = new string[fileCount];
                        fileCount = 0;
                        for (int i = 0; i < presetFilesInfo.Length; i++)
                        {
                            GetUIElement.loopAnimationsPresetNames[fileCount] = FileHelper.readObjectFile<LoopAnimations>(presetFilesInfo[i], FileHelper.DeserializeXML<LoopAnimations>).loopAnimationsPresetName;
                            fileCount++;
                        }
                    }
                    break;

                case AnimationType.OUT:
                    if (fileCount > 0)
                    {
                        GetUIElement.outAnimationsPresetNames = new string[fileCount];
                        fileCount = 0;
                        for (int i = 0; i < presetFilesInfo.Length; i++)
                        {
                            GetUIElement.outAnimationsPresetNames[fileCount] = FileHelper.readObjectFile<OutAnimations>(presetFilesInfo[i], FileHelper.DeserializeXML<OutAnimations>).outAnimationsPresetName;
                            fileCount++;
                        }
                    }
                    break;

                case AnimationType.OnClick:
                    {
                        if (fileCount > 0)
                        {
                            GetUIButton.onClickAnimationsPresetNames = new string[fileCount];
                            fileCount = 0;
                            for (int i = 0; i < presetFilesInfo.Length; i++)
                            {
                                GetUIButton.onClickAnimationsPresetNames[fileCount] = FileHelper.readObjectFile<OnClickAnimations>(presetFilesInfo[i], FileHelper.DeserializeXML<OnClickAnimations>).onClickAnimationsPresetName;
                                fileCount++;
                            }
                        }
                    }
                    break;

                case AnimationType.ButtonLoops:
                    {
                        if (fileCount > 0)
                        {
                            GetUIButton.buttonLoopsAnimationsPresetNames = new string[fileCount];
                            fileCount = 0;
                            for (int i = 0; i < presetFilesInfo.Length; i++)
                            {
                                GetUIButton.buttonLoopsAnimationsPresetNames[fileCount] = FileHelper.readObjectFile<ButtonLoopsAnimations>(presetFilesInfo[i], FileHelper.DeserializeXML<ButtonLoopsAnimations>).animationsPresetName;
                                fileCount++;
                            }
                        }
                    }
                    break;
            }
            */
        }
        #endregion

        #region Delete Preset
        public void DeletePreset(string presetName, AnimationType animationType, ButtonLoopType buttonLoopType = ButtonLoopType.None)
        {
            if (presetName.Equals(DEFAULT_PRESET_NAME))
            {
                Debug.Log("[DoozyUI] You cannot (and should not) delete the default preset '" + DEFAULT_PRESET_NAME + "'.");
                return;
            }

            //Get the full path to the file
            string adjustedFolderPath = GetUIAnimationsFolderPath + animationType.ToString();
            string presetFileName = presetName + "_" + animationType.ToString() + fileNameExtension;
            string filePath = adjustedFolderPath + "/" + presetFileName;

            //Load the default preset settings
            LoadPreset(DEFAULT_PRESET_NAME, animationType, buttonLoopType);

            //Delete the preset file
            QuickEngine.IO.File.Delete(filePath);

            //Debug.Log("[DoozyUI] the '" + presetName + "' preset has been DELETED!");

            //Reload the preset list for the animation patch
            LoadPresetList(animationType);
        }
        #endregion
#pragma warning restore 0612
#pragma warning restore 0219
    }
}

