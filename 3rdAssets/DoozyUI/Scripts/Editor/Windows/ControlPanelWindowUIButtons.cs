// Copyright (c) 2015 - 2018 Doozy Entertainment / Marlink Trading SRL. All Rights Reserved.
// This code can only be used under the standard Unity Asset Store End User License Agreement
// A Copy of the EULA APPENDIX 1 is available at http://unity3d.com/company/legal/as_terms

using QuickEditor;

namespace DoozyUI
{
    public partial class ControlPanelWindow : QWindow
    {
        void InitPageUIButtons()
        {
            DUIData.Instance.ScanForUIButtons(true);
            DUIData.Instance.ValidateUIButtons();
        }

        void DrawPageUIButtons()
        {
            DrawPageHeader("UIButtons", QColors.Blue, "Database", QUI.IsProSkin ? QColors.UnityLight : QColors.UnityMild, DUIResources.pageIconUIButtons);

            QUI.Space(6);

            DrawPageUIButtonsRefreshButton(WindowSettings.CurrentPageContentWidth);

            QUI.Space(SPACE_16);

            DrawDatabase(TargetDatabase.UIButtons, DUIData.Instance.DatabaseUIButtons, WindowSettings.CurrentPageContentWidth);
        }

        void DrawPageUIButtonsRefreshButton(float width)
        {
            QUI.BeginHorizontal(width);
            {
                QUI.Space(SPACE_8);
                if(QUI.SlicedButton("Refresh the UIButtons Database", QColors.Color.Gray, width - 16, 18))
                {
                    DUIData.Instance.ScanForUIButtons(true);
                }
                QUI.Space(SPACE_8);
            }
            QUI.EndHorizontal();
        }
    }
}
