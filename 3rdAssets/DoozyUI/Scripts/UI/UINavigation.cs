// Copyright (c) 2015 - 2018 Doozy Entertainment / Marlink Trading SRL. All Rights Reserved.
// This code can only be used under the standard Unity Asset Store End User License Agreement
// A Copy of the EULA APPENDIX 1 is available at http://unity3d.com/company/legal/as_terms

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DoozyUI
{
    public class UINavigation
    {
#if dUI_NavigationDisabled
        /// <summary>
        /// Internal variable that determines if the UINavigation is enabled or not. Default is set to true.
        /// </summary>
        private static bool m_isNavigationEnabled = false;
#else
        /// <summary>
        /// Internal variable that determines if the UINavigation is enabled or not. Default is set to true.
        /// </summary>
        private static bool m_isNavigationEnabled = true;
#endif

        /// <summary>
        /// Returns true if the UINavigation is enabled and false otherwise. It is set to false if Scripting Define Symbols, for the current active platform, contain the 'dUI_NavigationDisabled' symbol.
        /// <para>In you want to handle the UINavigation yourself just disable it from the Control Panel.</para>
        /// </summary>
        public static bool IsNavigationEnabled { get { return m_isNavigationEnabled; } }
        /// <summary>
        /// Internal list of navigation pointer data used to keep track of the navigation history.
        /// </summary>
        private static List<NavigationPointerData> History;
        /// <summary>
        /// Internal list used to temporary store what UIElements need to be hidden. This variable is used to optimize the memory allocation by not creating a new list every time some UIElements need to be hidden by the UI Navigation.
        /// </summary>
        private static List<UIElement> visibleHideElementsList = new List<UIElement>();

        private static List<UIElement> tempUIElementsList;
        private static int count;

        /// <summary>
        /// Initializes the Navigation History by allocation a new Navigation Pointer Data list if needed.
        /// </summary>
        private static void InitNavigationHistory()
        {
            if(History != null) { return; }
            History = new List<NavigationPointerData>();
        }
        /// <summary>
        /// Clears the Navigation History.
        /// </summary>
        public static void ClearNavigationHistory()
        {
            if(!IsNavigationEnabled)
            {
                Debug.Log("[DoozyUI] You are trying to clear the Navigation History stack, but the system is disabled. Nothing happened.");
                return;
            }
            InitNavigationHistory();
            History.Clear();
        }
        /// <summary>
        /// Adds a navigation item to the end of Navigation History (FILO - First In Last Out).
        /// </summary>
        public static void AddItemToHistory(NavigationPointerData data)
        {
            if(!IsNavigationEnabled)
            {
                Debug.Log("[DoozyUI] You are trying to add a navigation item to the Navigation History, but the system is disabled. Nothing happened.");
                return;
            }
            InitNavigationHistory();
            History.Add(data);
        }
        /// <summary>
        /// Removes the last item from the Navigation History (FIFLO - First In Last Out).
        /// </summary>
        public static void RemoveLastItemFromHistory()
        {
            if(!IsNavigationEnabled)
            {
                Debug.Log("[DoozyUI] You are trying to remove a navigation item to the Navigation History, but the system is disabled. Nothing happened.");
                return;
            }
            InitNavigationHistory();
            count = History.Count;
            if(count == 0) { return; }
            History.RemoveAt(count - 1);
        }
        /// <summary>
        /// Returns the last item in the Navigation History. It removes the data from History by default.
        /// </summary>
        public static NavigationPointerData GetLastItemFromNavigationHistory(bool removeFromHistory = true)
        {
            if(!IsNavigationEnabled)
            {
                Debug.Log("[DoozyUI] You are trying to get a navigation item to the Navigation History, but the system is disabled. Nothing happened.");
                return null;
            }
            InitNavigationHistory();
            count = History.Count;
            if(count == 0) { return new NavigationPointerData(); }
            NavigationPointerData data = History[count - 1].Copy();
            if(removeFromHistory)
            {
                History.RemoveAt(count - 1);
            }
            return data;
        }

        /// <summary>
        /// Pops to specific UIElement.
        /// </summary>
        /// <param name="elementCategory">The element category.</param>
        /// <param name="elementName">The element name.</param>
        public static void PopToSpecificUIElement(string elementCategory, string elementName, bool showAndHideElements = false)
        {
            if(!IsNavigationEnabled) //check that the UINavigation is enabled
            {
                Debug.Log("[DoozyUI] You are trying to get a navigation item to the Navigation History, but the system is disabled. Nothing happened.");
                return;
            }
            InitNavigationHistory(); //initialize it if needed
            count = History.Count;
            if(count == 0) { return; } //check that the History is not empty
            bool found = false;
            for(int historyIndex = History.Count; historyIndex <= 0; historyIndex--) //iterate through the History
            {
                if(History[historyIndex].show == null || History[historyIndex].show.Count == 0) { continue; } //check that this item has items that need to be shown
                found = false; //mark that the UIElement category and name pair have not been found in this NavigationPointerData
                for(int i = 0; i < History[historyIndex].show.Count; i++)
                {
                    if(History[historyIndex].show[i].category.Equals(elementCategory)
                       && History[historyIndex].show[i].name.Equals(elementName))
                    {
                        found = true; //the category and name pair has been found
                        break; //break this loop on the show list
                    }
                }
                if(!found) //a pair has not been found in this entry --> pop it
                {
                    History.RemoveAt(historyIndex); //remove the last entry from the History as it does not contain the category and name pair we are looking for
                }
                else
                {
                    break; //break from the History loop as we found a pair and we no longer need to pop NavigationPointers
                }

                if(showAndHideElements)
                {
                    NavigationPointerData data = UINavigation.GetLastItemFromNavigationHistory(false);
                    UINavigation.Show(data.show);
                }
            }
        }

        /// <summary>
        /// Executes the Show for the given list of Navigation Pointers.
        /// </summary>
        public static void Show(List<NavigationPointer> show, bool instantAction = false)
        {
            count = show != null ? show.Count : 0;
            if(count == 0) { return; }
            for(int i = 0; i < count; i++)
            {
                if(show[i] == null) { continue; }
                UIManager.ShowUiElement(show[i].name, show[i].category, instantAction);
            }
        }
        /// <summary>
        /// Executes the Hide for the given list of Navigation Pointers.
        /// </summary>
        public static void Hide(List<NavigationPointer> hide, bool instantAction = false, bool disableWhenHidden = false)
        {
            count = hide != null ? hide.Count : 0;
            if(count == 0) { return; }
            for(int i = 0; i < count; i++)
            {
                if(hide[i] == null) { continue; }
                UIManager.HideUiElement(hide[i].name, hide[i].category, instantAction);
            }
        }
        /// <summary>
        /// Updates the Navigation History while showing and hiding the relevant UIElements. 
        /// </summary>
        public static void UpdateTheNavigationHistory(NavigationPointerData navData)
        {
            if(navData == null) { return; }
            for(int i = 0; i < navData.show.Count; i++) { if(navData.show[i].category.Equals(DUI.CUSTOM_NAME)) { navData.show[i].category = DUI.UNCATEGORIZED_CATEGORY_NAME; } }
            for(int j = 0; j < navData.hide.Count; j++) { if(navData.hide[j].category.Equals(DUI.CUSTOM_NAME)) { navData.hide[j].category = DUI.UNCATEGORIZED_CATEGORY_NAME; } }
            visibleHideElementsList.Clear();
            navData.hide = navData.hide.Where(navPointer => !navPointer.name.Equals(DUI.DEFAULT_ELEMENT_NAME)).ToList();
            navData.hide = navData.hide.Where(navPointer => UIManager.GetUiElements(navPointer.name, navPointer.category).Any(element => element.isVisible)).ToList();
            if(tempUIElementsList == null) { tempUIElementsList = new List<UIElement>(); }
            tempUIElementsList.Clear();
            for(int i = 0; i < navData.hide.Count; i++)
            {
                tempUIElementsList = UIManager.GetUiElements(navData.hide[i].name, navData.hide[i].category);
                for(int j = 0; j < tempUIElementsList.Count; j++)
                {
                    visibleHideElementsList.Add(tempUIElementsList[j]);
                }
            }

            Show(navData.show);
            Hide(navData.hide);

            if(navData.addToNavigationHistory)
            {
                NavigationPointerData historyNavData = new NavigationPointerData()
                {
                    show = new List<NavigationPointer>(),
                    hide = new List<NavigationPointer>()
                };

                count = visibleHideElementsList != null ? visibleHideElementsList.Count : 0;
                if(count > 0)
                {
                    for(int i = 0; i < count; i++)
                    {
                        historyNavData.show.Add(new NavigationPointer(visibleHideElementsList[i].elementCategory, visibleHideElementsList[i].elementName));
                    }
                }

                if(tempUIElementsList == null) { tempUIElementsList = new List<UIElement>(); }
                tempUIElementsList.Clear();
                for(int i = 0; i < navData.show.Count; i++)
                {
                    tempUIElementsList = UIManager.GetUiElements(navData.show[i].name, navData.show[i].category);
                    for(int j = 0; j < tempUIElementsList.Count; j++)
                    {
                        historyNavData.hide.Add(new NavigationPointer(tempUIElementsList[j].elementCategory, tempUIElementsList[j].elementName));
                    }
                }

                AddItemToHistory(historyNavData);
            }

        }
    }

    /// <summary>
    /// Helper class for the UI Navigation.
    /// </summary>
    [Serializable]
    public class NavigationPointer
    {
        /// <summary>
        /// Element Category
        /// </summary>
        public string category = DUI.UNCATEGORIZED_CATEGORY_NAME;
        /// <summary>
        /// Element Name
        /// </summary>
        public string name = DUI.DEFAULT_ELEMENT_NAME;

        public NavigationPointer()
        {
            category = DUI.UNCATEGORIZED_CATEGORY_NAME;
            name = DUI.DEFAULT_ELEMENT_NAME;
        }

        public NavigationPointer(string Category, string Name)
        {
            category = Category;
            name = Name;
        }

        public NavigationPointer Copy()
        {
            return new NavigationPointer(category, name);
        }
    }

    /// <summary>
    /// Helper class for the UINavigation.
    /// </summary>
    [Serializable]
    public class NavigationPointerData
    {
        /// <summary>
        /// Should the Navigation Pointers from the Show list be added to the UI Navigation History? Default is set to false.
        /// </summary>
        public bool addToNavigationHistory = false;
        /// <summary>
        /// A list of Navigation Pointers used for all the UIElements that need to be shown.
        /// </summary>
        public List<NavigationPointer> show = new List<NavigationPointer>();
        /// <summary>
        /// A list of Navigation Pointers used for all the UIElements that need to he hidden.
        /// </summary>
        public List<NavigationPointer> hide = new List<NavigationPointer>();

        public NavigationPointerData(bool AddToNavigationHitory)
        {
            addToNavigationHistory = AddToNavigationHitory;
            show = new List<NavigationPointer>();
            hide = new List<NavigationPointer>();
        }

        public NavigationPointerData()
        {
            addToNavigationHistory = false;
            show = new List<NavigationPointer>();
            hide = new List<NavigationPointer>();
        }

        public NavigationPointerData Copy()
        {
            NavigationPointerData copy = new NavigationPointerData();
            copy.addToNavigationHistory = addToNavigationHistory;
            for(int i = 0; i < show.Count; i++) { copy.show.Add(show[i].Copy()); }
            for(int j = 0; j < hide.Count; j++) { copy.hide.Add(hide[j].Copy()); }
            return copy;
        }

        /// <summary>
        /// Retruns true if there is at least one entry in the show or in the hide list.
        /// </summary>
        public bool Enabled
        {
            get
            {
                return show.Count > 0 || hide.Count > 0;
            }
        }
    }

#if UNITY_EDITOR
    [Serializable]
    public class EditorNavigationPointer
    {
        public int categoryIndex = 0;
        public int nameIndex = 0;

        public EditorNavigationPointer(int CategoryIndex, int NameIndex)
        {
            categoryIndex = CategoryIndex;
            nameIndex = NameIndex;
        }

        public EditorNavigationPointer Copy()
        {
            return new EditorNavigationPointer(categoryIndex, nameIndex);
        }
    }

    [Serializable]
    public class EditorNavigationPointerData
    {
        public List<EditorNavigationPointer> showIndex = new List<EditorNavigationPointer>();
        public List<EditorNavigationPointer> hideIndex = new List<EditorNavigationPointer>();

        public EditorNavigationPointerData()
        {
            showIndex = new List<EditorNavigationPointer>();
            hideIndex = new List<EditorNavigationPointer>();
        }

        public EditorNavigationPointerData Copy()
        {
            EditorNavigationPointerData copy = new EditorNavigationPointerData();
            for(int i = 0; i < showIndex.Count; i++) { copy.showIndex.Add(showIndex[i].Copy()); }
            for(int j = 0; j < hideIndex.Count; j++) { copy.hideIndex.Add(hideIndex[j].Copy()); }
            return copy;
        }
    }
#endif
}
