// Copyright (c) 2015 - 2018 Doozy Entertainment / Marlink Trading SRL. All Rights Reserved.
// This code can only be used under the standard Unity Asset Store End User License Agreement
// A Copy of the EULA APPENDIX 1 is available at http://unity3d.com/company/legal/as_terms

using QuickEditor;
using UnityEditor.AnimatedValues;

namespace DoozyUI
{
    public partial class ControlPanelWindow : QWindow
    {
        AnimBool _UICanvasesAnimBool;
        AnimBool UICanvasesAnimBool { get { if(_UICanvasesAnimBool == null) { _UICanvasesAnimBool = new AnimBool(true, Repaint); } return _UICanvasesAnimBool; } }

        void InitPageUICanvases()
        {
            DUIData.Instance.ScanForUICanvases(true);
            DUIData.Instance.ValidateUICanvases();
        }

        void DrawPageUICanvases()
        {
            DrawPageHeader("UICanvases", QColors.Blue, "Database", QUI.IsProSkin ? QColors.UnityLight : QColors.UnityMild, DUIResources.pageIconUICanvases);

            QUI.Space(6);

            DrawPageUICanvasesRefreshButton(WindowSettings.CurrentPageContentWidth);

            QUI.Space(SPACE_16);

            QUI.BeginHorizontal(WindowSettings.CurrentPageContentWidth);
            {
                QUI.BeginVertical(WindowSettings.CurrentPageContentWidth - SPACE_8);
                {
                    DrawStringList(DUIData.Instance.DatabaseUICanvases, WindowSettings.CurrentPageContentWidth - SPACE_8, UICanvasesAnimBool);
                }
                QUI.EndVertical();
            }
            QUI.EndHorizontal();

            QUI.Space(SPACE_16);
        }

        void DrawPageUICanvasesRefreshButton(float width)
        {
            QUI.BeginHorizontal(width);
            {
                QUI.Space(SPACE_8);
                if(QUI.SlicedButton("Refresh the UICanvases Database", QColors.Color.Gray, width - 16, 18))
                {
                    DUIData.Instance.ScanForUICanvases(true);
                }
                QUI.Space(SPACE_8);
            }
            QUI.EndHorizontal();
        }
    }
}