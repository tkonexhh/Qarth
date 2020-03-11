// Copyright (c) 2015 - 2018 Doozy Entertainment / Marlink Trading SRL. All Rights Reserved.
// This code can only be used under the standard Unity Asset Store End User License Agreement
// A Copy of the EULA APPENDIX 1 is available at http://unity3d.com/company/legal/as_terms

using QuickEditor;
using QuickEngine.Extensions;
using UnityEditor;
using UnityEngine;

namespace DoozyUI
{
    public partial class ControlPanelWindow : QWindow
    {
        void InitPageUIElements()
        {
            DUIData.Instance.ScanForUIElements(true);
            DUIData.Instance.ValidateUIElements();
        }

        void DrawPageUIElements()
        {
            DrawPageHeader("UIElements", QColors.Blue, "Database", QUI.IsProSkin ? QColors.UnityLight : QColors.UnityMild, DUIResources.pageIconUIElements);

            QUI.Space(6);

            DrawPageUIElementsRefreshButton(WindowSettings.CurrentPageContentWidth);

            QUI.Space(SPACE_16);

            DrawDatabase(TargetDatabase.UIElements, DUIData.Instance.DatabaseUIElements, WindowSettings.CurrentPageContentWidth);
        }

        void DrawPageUIElementsRefreshButton(float width)
        {
            QUI.BeginHorizontal(width);
            {
                QUI.Space(SPACE_8);
                if(QUI.SlicedButton("Refresh the UIElements Database", QColors.Color.Gray, width - 16, 18))
                {
                    DUIData.Instance.ScanForUIElements(true);
                }
                QUI.Space(SPACE_8);
            }
            QUI.EndHorizontal();
        }

        public enum TargetDatabase
        {
            UIElements,
            UIButtons
        }

        public void DrawDatabase(TargetDatabase databaseType, DUIData.Database database, float width)
        {
            DrawNewCategoryAndSearch(width, databaseType);

            QUI.Space(SPACE_16);

            if(database.categories.Count == 0)
            {
                DrawInfoMessage(InfoMessageName.AddCategoryToStart.ToString(), width);
                return;
            }

            DrawExpandCollapseButtons(width, databaseType);

            QUI.Space(SPACE_8);

            foreach(string categoryName in database.categoryNames)
            {
                if(categoryName.Equals(DUI.CUSTOM_NAME)) { continue; }

                QUI.BeginHorizontal(width);
                {
                    #region Button Bar
                    if(RenameCategoryAnimBool.target && RenameCategoryTargetCategoryName.Equals(categoryName))
                    {
                        QLabel.text = "Rename category to";
                        QLabel.style = Style.Text.Normal;
                        QUI.Label(QLabel);
                        QUI.Space(SPACE_2);
                        QUI.SetNextControlName("RenameCategoryName");
                        RenameCategoryName = QUI.TextField(RenameCategoryName, width - QLabel.x - 46);
                        QUI.Space(1);
                        if(QUI.ButtonOk()
                           || (DetectKey_Return() && QUI.GetNameOfFocusedControl().Equals("RenameCategoryName")))
                        {
                            RenameCategoryName = RenameCategoryName.Trim();
                            if(string.IsNullOrEmpty(RenameCategoryName))
                            {
                                QUI.DisplayDialog("Action Required",
                                                  "Please enter a new category name in order to cotinue.",
                                                  "Ok");

                            }
                            else if(database.categoryNames.Contains(RenameCategoryName))
                            {
                                QUI.DisplayDialog("Action Required",
                                                  "There is another category with the name '" + RenameCategoryName + "' already in the database." +
                                                  "\n\n" +
                                                  "Enter another category name.",
                                                  "Ok");
                            }
                            else
                            {
                                database.RenameCategory(categoryName, RenameCategoryName);
                                RenameCategoryName = "";
                                RenameCategoryAnimBool.target = false;
                                RenameCategoryTargetCategoryName = "";
                                break;
                            }
                        }
                        QUI.Space(1);
                        if(QUI.ButtonCancel()
                           || QUI.DetectKeyDown(Event.current, KeyCode.Escape))
                        {
                            RenameCategoryName = "";
                            RenameCategoryAnimBool.target = false;
                            RenameCategoryTargetCategoryName = "";
                        }
                    }
                    else
                    {
                        if(QUI.GhostBar(categoryName, SearchPatternAnimBool.target ? QColors.Color.Orange : QColors.Color.Blue, database.GetCategory(categoryName).isExpanded, width - 70 * database.GetCategory(categoryName).isExpanded.faded * (1 - SearchPatternAnimBool.faded), BarHeight))
                        {
                            database.GetCategory(categoryName).isExpanded.target = !database.GetCategory(categoryName).isExpanded.target;
                        }

                        if(database.GetCategory(categoryName).isExpanded.faded > 0.7f && SearchPatternAnimBool.faded < 0.3f)
                        {
                            QUI.Space(1);

                            if(QUI.GhostButton("rename", QColors.Color.Gray, 52, BarHeight, database.GetCategory(categoryName).isExpanded.value))
                            {
                                if(QUI.DisplayDialog("Information",
                                                  "Note that after you rename this category, all the UI settings (and code references) that use the current category name, will not get automatically changed." +
                                                  "\n\n" +
                                                  "You are responsible to update your code and the UI settings.",
                                                  "Continue",
                                                  "Cancel"))
                                {
                                    RenameCategoryAnimBool.target = true;
                                    RenameCategoryName = categoryName;
                                    RenameCategoryTargetCategoryName = categoryName;
                                    QUI.FocusControl("RenameCategoryName");
                                    QUI.FocusTextInControl("RenameCategoryName");
                                }
                            }

                            QUI.Space(3);

                            if(QUI.ButtonCancel())
                            {
                                if(categoryName.Equals(DUI.UNCATEGORIZED_CATEGORY_NAME))
                                {
                                    QUI.DisplayDialog("Info",
                                                      "You cannot and should not try to delete the '" + categoryName + "' category.",
                                                      "Ok");
                                }
                                else if(QUI.DisplayDialog("Delete category?",
                                                     "Are you sure you want to delete the '" + categoryName + "'?",
                                                     "Yes",
                                                     "Cancel"))
                                {
                                    Undo.RecordObject(DUIData.Instance, "DeleteCategory");
                                    database.RemoveCategory(categoryName, true);
                                    QUI.EndHorizontal();
                                    break;
                                }
                            }
                        }
                    }
                    #endregion
                }
                QUI.EndHorizontal();
                if(QUI.BeginFadeGroup(database.GetCategory(categoryName).isExpanded.faded))
                {
                    DrawStringList(database.GetCategory(categoryName).itemNames, width, database.GetCategory(categoryName).isExpanded);
                    QUI.Space(SPACE_8 * database.GetCategory(categoryName).isExpanded.faded);
                }
                QUI.EndFadeGroup();
                QUI.Space(SPACE_2);
            }

        }

        void DrawNewCategoryAndSearch(float width, TargetDatabase targetDatabase)
        {
            QUI.BeginHorizontal(width);
            {
                #region New Category
                if(SearchPatternAnimBool.faded < 0.2f)
                {
                    if(QUI.GhostButton("New Category", QColors.Color.Green, 100 * (1 - SearchPatternAnimBool.faded), 20, NewCategoryAnimBool.value)
                       || DetectKeyCombo_Alt_N())
                    {
                        NewCategoryAnimBool.target = !NewCategoryAnimBool.target;
                        if(NewCategoryAnimBool.target)
                        {
                            NewCategoryName = "";
                            SearchPatternAnimBool.target = false;
                            ExpandOrCollapseAllDatabaseCategories(targetDatabase, false);
                        }
                    }
                }

                if(NewCategoryAnimBool.target)
                {
                    SearchPatternAnimBool.target = false;
                    QUI.SetGUIBackgroundColor(QColors.GreenLight.Color);
                    QUI.SetNextControlName("NewCategoryName" + targetDatabase.ToString());
                    NewCategoryName = EditorGUILayout.TextField(NewCategoryName, GUILayout.Width((width - 149) * NewCategoryAnimBool.faded));
                    QUI.ResetColors();

                    if(!NewCategoryAnimBool.value && Event.current.type == EventType.Layout) //if NewCategoryAnimBool.target is true and NewCategoryAnimBool.value is false -> in transition -> select the text in the control
                    {
                        QUI.FocusControl("NewCategoryName" + targetDatabase.ToString());
                        QUI.FocusTextInControl("NewCategoryName" + targetDatabase.ToString());
                    }

                    if(QUI.ButtonOk()
                       || (DetectKey_Return() && QUI.GetNameOfFocusedControl().Equals("NewCategoryName" + targetDatabase.ToString())))
                    {
                        if(NewCategoryName.IsNullOrEmpty())
                        {
                            EditorUtility.DisplayDialog("Info", "Cannot create an unnamed category. Try again.", "Ok");
                        }
                        else
                        {
                            switch(targetDatabase)
                            {
                                case TargetDatabase.UIElements:
                                    if(DUIData.Instance.DatabaseUIElements.ContainsCategoryName(NewCategoryName))
                                    {
                                        EditorUtility.DisplayDialog("Info", "A category named '" + NewCategoryName + "' already exists in the database. Try again.", "Ok");
                                    }
                                    else
                                    {
                                        DUIData.Instance.DatabaseUIElements.AddCategory(NewCategoryName, true);
                                        NewCategoryAnimBool.target = false;
                                    }
                                    break;
                                case TargetDatabase.UIButtons:
                                    if(DUIData.Instance.DatabaseUIButtons.ContainsCategoryName(NewCategoryName))
                                    {
                                        EditorUtility.DisplayDialog("Info", "A category named '" + NewCategoryName + "' already exists in the database. Try again.", "Ok");
                                    }
                                    else
                                    {
                                        DUIData.Instance.DatabaseUIButtons.AddCategory(NewCategoryName, true);
                                        NewCategoryAnimBool.target = false;
                                    }
                                    break;
                            }
                        }
                    }
                    QUI.Space(1);
                    if(QUI.ButtonCancel()
                       || QUI.DetectKeyDown(Event.current, KeyCode.Escape))
                    {
                        NewCategoryName = string.Empty;
                        NewCategoryAnimBool.target = false;
                    }
                }
                #endregion
                QUI.FlexibleSpace();
                #region Search
                if(SearchPatternAnimBool.value)
                {
                    NewCategoryAnimBool.target = false;
                    QUI.SetGUIBackgroundColor(QColors.OrangeLight.Color);
                    QUI.SetNextControlName("SearchPattern" + targetDatabase.ToString());
                    SearchPattern = EditorGUILayout.TextField(SearchPattern, GUILayout.Width((width - 104) * SearchPatternAnimBool.faded));
                    QUI.ResetColors();

                    if(SearchPatternAnimBool.target && Event.current.type == EventType.Layout) //if SearchPatternAnimBool.target is true and SearchPatternAnimBool.value is false -> in transition -> select the text in the control
                    {
                        QUI.FocusControl("SearchPattern" + targetDatabase.ToString());
                        QUI.FocusTextInControl("SearchPattern" + targetDatabase.ToString());
                    }
                }


                if(NewCategoryAnimBool.faded < 0.2f)
                {
                    if(QUI.GhostButton(SearchPatternAnimBool.value ? "Clear Search" : "Search", QColors.Color.Orange, 100 * (1 - NewCategoryAnimBool.faded), 20, SearchPatternAnimBool.value)
                       || DetectKeyCombo_Alt_S() //Toggle Search
                       || (DetectKey_Escape() && SearchPatternAnimBool.target)) //Clear Search
                    {
                        SearchPatternAnimBool.target = !SearchPatternAnimBool.target;
                        if(SearchPatternAnimBool.target)
                        {
                            SearchPattern = string.Empty;
                            NewCategoryAnimBool.target = false;
                            ExpandOrCollapseAllDatabaseCategories(targetDatabase, true);
                        }
                    }
                }
                #endregion
            }
            QUI.EndHorizontal();
        }

        void DrawExpandCollapseButtons(float width, TargetDatabase targetDatabase)
        {
            QUI.BeginHorizontal(width);
            {
                if(QUI.GhostButton("Expand", QColors.Color.Gray, 80)
                   || DetectKeyCombo_Alt_E()) // Alt + E: expand all categories)
                {
                    ExpandOrCollapseAllDatabaseCategories(targetDatabase, true);
                }
                QUI.FlexibleSpace();
                if(QUI.GhostButton("Collapse", QColors.Color.Gray, 80)
                      || DetectKeyCombo_Alt_C()) // Alt + C: collapse all categories)
                {
                    ExpandOrCollapseAllDatabaseCategories(targetDatabase, false);
                }
            }
            QUI.EndHorizontal();
        }

        void ExpandOrCollapseAllDatabaseCategories(TargetDatabase targetDatabase, bool expand)
        {

            switch(targetDatabase)
            {
                case TargetDatabase.UIElements: DUIData.Instance.DatabaseUIElements.ExpandOrCollapseAllCategories(expand); break;
                case TargetDatabase.UIButtons: DUIData.Instance.DatabaseUIButtons.ExpandOrCollapseAllCategories(expand); break;
            }
        }
    }
}
