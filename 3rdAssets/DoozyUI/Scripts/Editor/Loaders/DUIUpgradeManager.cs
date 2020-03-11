// Copyright (c) 2015 - 2018 Doozy Entertainment / Marlink Trading SRL. All Rights Reserved.
// This code can only be used under the standard Unity Asset Store End User License Agreement
// A Copy of the EULA APPENDIX 1 is available at http://unity3d.com/company/legal/as_terms

using DoozyUI.Internal;
using QuickEditor;
using UnityEditor;

namespace DoozyUI
{
    [InitializeOnLoad]
    public class DUIUpgradeManager
    {
        static DUIUpgradeManager()
        {
            EditorApplication.update += RunOnce;
        }

        static void RunOnce()
        {
            EditorApplication.update -= RunOnce;
            UpgradeDatabases();
        }

        static void UpgradeDatabases()
        {
#if dUI_SOURCE
            return;
#endif

#pragma warning disable CS0162 // Unreachable code detected
            if(DUIVersion.Instance.performDatabaseUpgrade == false)
            {
                return;
            }

            DUIData.Instance.ScanForUICanvases();
            DUIData.Instance.ScanForUIElements();
            DUIData.Instance.ScanForUIButtons();
            DUIData.Instance.ScanForUISounds();

            DUIData.Instance.ScanForInAnimations();
            DUIData.Instance.ScanForOutAnimations();
            DUIData.Instance.ScanForLoopAnimations();
            DUIData.Instance.ScanForPunchAnimations();
            DUIData.Instance.ScanForStateAnimations();

            DUIVersion.Instance.performDatabaseUpgrade = false;

            QUI.SetDirty(DUIVersion.Instance);
            AssetDatabase.SaveAssets();
#pragma warning restore CS0162 // Unreachable code detected
        }
    }
}
