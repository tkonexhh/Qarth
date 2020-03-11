// Copyright (c) 2015 - 2018 Doozy Entertainment / Marlink Trading SRL. All Rights Reserved.
// This code can only be used under the standard Unity Asset Store End User License Agreement
// A Copy of the EULA APPENDIX 1 is available at http://unity3d.com/company/legal/as_terms

using UnityEngine;
using UnityEngine.EventSystems;

namespace DoozyUI
{
    [AddComponentMenu(DUI.COMPONENT_MENU_UICANVAS, DUI.MENU_PRIORITY_UICANVAS)]
    [RequireComponent(typeof(Canvas))]
    [DisallowMultipleComponent]
    public class UICanvas : MonoBehaviour
    {

#if UNITY_EDITOR
        [UnityEditor.MenuItem(DUI.GAMEOBJECT_MENU_UICANVAS, false, DUI.MENU_PRIORITY_UICANVAS)]
        static void CreateCanvas(UnityEditor.MenuCommand menuCommand)
        {
            string canvasName = MASTER_CANVAS_NAME;
            UICanvas[] searchResults = FindObjectsOfType<UICanvas>();
            if (searchResults != null && searchResults.Length > 0)
            {
                bool renameRequired = true;
                int canvasCount = 0;
                while (renameRequired)
                {
                    renameRequired = false;
                    for (int i = 0; i < searchResults.Length; i++)
                    {
                        if (canvasName.Equals(searchResults[i].canvasName))
                        {
                            canvasCount++;
                            canvasName = "UICanvas " + canvasCount;
                            renameRequired = true;
                            break;
                        }
                    }
                }
            }
            UICanvas canvas = UIManager.CreateCanvas(canvasName);
            UnityEditor.Undo.RegisterCreatedObjectUndo(canvas.gameObject, "Create " + canvas.gameObject.name);
            UnityEditor.Selection.activeObject = canvas.gameObject;
        }
#endif

        /// <summary>
        /// Default name given to a new canvas. The name is 'MasterCanvas' and you should have ONLY ONE per scene as this is considered your main/default canvas.
        /// </summary>
        public const string MASTER_CANVAS_NAME = "MasterCanvas";

        /// <summary>
        /// The name of this canvas.
        /// </summary>
        public string canvasName = MASTER_CANVAS_NAME;
        /// <summary>
        /// Used by the custom inspector to allow you to type a canvas name instead of selecting it from the Canvas Names Database.
        /// </summary>
        public bool customCanvasName = false;

        /// <summary>
        /// Makes this UICanvas gameObject not get destroyed automatically when loading a new scene.
        /// </summary>
        public bool dontDestroyOnLoad = false;

        /// <summary>
        /// Returns true if this canvas name is 'MasterCanvas' and if it has been registered to the UIManager as the MasterCanvas
        /// </summary>
        public bool IsMasterCanvas { get { return canvasName == MASTER_CANVAS_NAME && UIManager.GetMasterCanvas() == this; } }

        /// <summary>
        /// Internal variable that holds a reference to the RectTransform component.
        /// </summary>
        private RectTransform m_rectTransform;
        /// <summary>
        /// Returns the RectTransform component.
        /// </summary>
        public RectTransform RectTransform { get { if (m_rectTransform == null) { m_rectTransform = GetComponent<RectTransform>() == null ? gameObject.AddComponent<RectTransform>() : GetComponent<RectTransform>(); } return m_rectTransform; } }

        /// <summary>
        /// Internal variable that holds a reference to the Canvas component.
        /// </summary>
        private Canvas m_canvas;
        /// <summary>
        /// Returns the Canvas component.
        /// </summary>
        public Canvas Canvas { get { if (m_canvas == null) { m_canvas = GetComponent<Canvas>(); } return m_canvas; } }

        /// <summary>
        /// Internal variable that is set to true if this canvas has been registered to the UIManager.
        /// </summary>
        private bool registeredToUIManager = false;

        private void Awake()
        {
            if (Canvas == null)
            {
                Debug.LogError("[DoozyUI] The UICanvas, attached to the " + name + " gameObject, does not have a Canvas component attached. Fix this by adding a Canvas component.");
                Destroy(this);
                return;
            }
            if (!Canvas.isRootCanvas)
            {
                Debug.LogError("[DoozyUI] The Canvas, attached to the " + name + " gameObject, is to a root canvas. The UICanvas component must be attached to a top (root) canvas in the Hierarchy.");
                return;
            }
            if(dontDestroyOnLoad)
            {
                DontDestroyOnLoad(gameObject);
            }
        }

        private void OnEnable()
        {
            RegisterToUIManager();
        }

        private void OnDisable()
        {
            UnregisterFromUIManager();
        }

        private void OnDestroy()
        {
            UnregisterFromUIManager();
        }

        /// <summary>
        /// Registers this UICanvas to the UIManager.
        /// </summary>
        public void RegisterToUIManager()
        {
            if (Canvas == null || !Canvas.isRootCanvas) { return; }
            if (registeredToUIManager) { return; }
            if (UIManager.CanvasDatabase.ContainsKey(canvasName))
            {
                Debug.LogError("[DoozyUI] Error duplicate UICanvas found. " +
                               "You cannot have multiple UICanvases with the same canvas name. " +
                               "This error orginated from the UICanvas component attached to the " + name + " gameObject. " +
                               "The duplicate canvas name is '" + canvasName + "'.");
                return;
            }

            UIManager.CanvasDatabase.Add(canvasName, this);
            registeredToUIManager = true;
        }

        /// <summary>
        /// Unregisteres this UICanvas from the UIManager.
        /// </summary>
        public void UnregisterFromUIManager()
        {
            if (Canvas == null || !Canvas.isRootCanvas) { return; }
            if (!registeredToUIManager) { return; }
            if (UIManager.CanvasDatabase.ContainsKey(canvasName))
            {
                UIManager.CanvasDatabase.Remove(canvasName);
                registeredToUIManager = false;
            }
        }
    }
}
