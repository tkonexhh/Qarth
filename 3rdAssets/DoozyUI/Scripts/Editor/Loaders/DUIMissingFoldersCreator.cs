// Copyright (c) 2015 - 2018 Doozy Entertainment / Marlink Trading SRL. All Rights Reserved.
// This code can only be used under the standard Unity Asset Store End User License Agreement
// A Copy of the EULA APPENDIX 1 is available at http://unity3d.com/company/legal/as_terms

using UnityEditor;

namespace DoozyUI.Internal
{
    [InitializeOnLoad]
    public class DUIMissingFoldersCreator
    {
        static DUIMissingFoldersCreator()
        {
            if(EditorApplication.isPlayingOrWillChangePlaymode)
            {
                return;
            }

            EditorApplication.update += RunOnce;
        }

        static void RunOnce()
        {
            EditorApplication.update -= RunOnce;
            CreateMissingFolders();
        }

        static void CreateMissingFolders()
        {
            if(!IsValidFolder(DUI.PATH + "/Editor")) { CreateFolder(DUI.PATH, "Editor"); }
            if(!IsValidFolder(DUI.PATH + "/Editor/Resources")) { CreateFolder(DUI.PATH + "/Editor", "Resources"); }
            if(!IsValidFolder(DUI.PATH + "/Editor/Resources/DUI")) { CreateFolder(DUI.PATH + "/Editor/Resources", "DUI"); }
            if(!IsValidFolder(DUI.PATH + "/Editor/Resources/DUI/Data")) { CreateFolder(DUI.PATH + "/Editor/Resources/DUI", "Data"); }

            if(!IsValidFolder(DUI.PATH + "/Resources")) { CreateFolder(DUI.PATH, "Resources"); }
            if(!IsValidFolder(DUI.PATH + "/Resources/DUI")) { CreateFolder(DUI.PATH + "/Resources", "DUI"); }
            if(!IsValidFolder(DUI.PATH + "/Resources/DUI/Animations")) { CreateFolder(DUI.PATH + "/Resources/DUI", "Animations"); }
            if(!IsValidFolder(DUI.PATH + "/Resources/DUI/Animations/In")) { CreateFolder(DUI.PATH + "/Resources/DUI/Animations", "In"); }
            if(!IsValidFolder(DUI.PATH + "/Resources/DUI/Animations/Loop")) { CreateFolder(DUI.PATH + "/Resources/DUI/Animations", "Loop"); }
            if(!IsValidFolder(DUI.PATH + "/Resources/DUI/Animations/Out")) { CreateFolder(DUI.PATH + "/Resources/DUI/Animations", "Out"); }
            if(!IsValidFolder(DUI.PATH + "/Resources/DUI/Animations/Punch")) { CreateFolder(DUI.PATH + "/Resources/DUI/Animations", "Punch"); }
            if(!IsValidFolder(DUI.PATH + "/Resources/DUI/Animations/State")) { CreateFolder(DUI.PATH + "/Resources/DUI/Animations", "State"); }
            if(!IsValidFolder(DUI.PATH + "/Resources/DUI/Settings")) { CreateFolder(DUI.PATH + "/Resources/DUI", "Settings"); }
            if(!IsValidFolder(DUI.PATH + "/Resources/DUI/UIButtons")) { CreateFolder(DUI.PATH + "/Resources/DUI", "UIButtons"); }
            if(!IsValidFolder(DUI.PATH + "/Resources/DUI/UIElements")) { CreateFolder(DUI.PATH + "/Resources/DUI", "UIElements"); }
            if(!IsValidFolder(DUI.PATH + "/Resources/DUI/UISounds")) { CreateFolder(DUI.PATH + "/Resources/DUI", "UISounds"); }
            if(!IsValidFolder(DUI.PATH + "/Resources/DUI/Canvases")) { CreateFolder(DUI.PATH + "/Resources/DUI", "Canvases"); }
        }

        public static bool IsValidFolder(string relativePath)
        {
            return AssetDatabase.IsValidFolder(relativePath);
        }

        public static void CreateFolder(string parentFolder, string newFolderName)
        {
            AssetDatabase.CreateFolder(parentFolder, newFolderName);
        }
    }
}
