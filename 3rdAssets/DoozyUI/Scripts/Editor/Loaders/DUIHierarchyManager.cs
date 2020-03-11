using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using QuickEditor;

namespace DoozyUI
{
    [InitializeOnLoad]
    public class DUIHierarchyManager
    {
        static bool UseFixedUpdate = true;
        static float UpdateInterval = 5f;
        static double startTime;
        static double elapsedInterval = 0;

        static float iconWidth = 20;
        static float iconHeight = 20;

        static GameObject[] allTheGameObjectsInScene;
        static Dictionary<int, UIButton> uiButtonIDs;
        static Dictionary<int, UICanvas> uiCanvasIDs;
        static Dictionary<int, UIElement> uiElementIDs;
        static Dictionary<int, UITrigger> uiTriggerIDs;
        static Dictionary<int, UIEffect> uiEffectIDs;
        static Dictionary<int, UINotification> uiNotificationIDs;

        static int UIManagerID;
        static int SoundyID;
        static int UINotificationManagerID;
        static int OrientationManagerID;
        static int SceneLoaderID;

        static QLabel QLabel;
        static Rect rect;
        static UICanvas uic;
        static UIButton uib;
        static UIElement uie;
        static UIEffect uief;
        static UITrigger uit;
        static UINotification uin;

#if dUI_PlayMaker
        static Dictionary<int, PlaymakerEventDispatcher> playmakerEventDispatcherIDs;
        static PlaymakerEventDispatcher ped;
#endif

        static DUIHierarchyManager()
        {
            if(uiButtonIDs == null) { uiButtonIDs = new Dictionary<int, UIButton>(); } else { uiButtonIDs.Clear(); }
            if(uiCanvasIDs == null) { uiCanvasIDs = new Dictionary<int, UICanvas>(); } else { uiCanvasIDs.Clear(); }
            if(uiEffectIDs == null) { uiEffectIDs = new Dictionary<int, UIEffect>(); } else { uiEffectIDs.Clear(); }
            if(uiElementIDs == null) { uiElementIDs = new Dictionary<int, UIElement>(); } else { uiElementIDs.Clear(); }
            if(uiTriggerIDs == null) { uiTriggerIDs = new Dictionary<int, UITrigger>(); } else { uiTriggerIDs.Clear(); }
            if(uiNotificationIDs == null) { uiNotificationIDs = new Dictionary<int, UINotification>(); } else { uiNotificationIDs.Clear(); }

#if dUI_PlayMaker
            if(playmakerEventDispatcherIDs == null) { playmakerEventDispatcherIDs = new Dictionary<int, PlaymakerEventDispatcher>(); } else { playmakerEventDispatcherIDs.Clear(); }
#endif

            UpdateReferences();

            startTime = EditorApplication.timeSinceStartup;
            EditorApplication.update += EditorUpdate;

            EditorApplication.hierarchyWindowChanged += UpdateReferences;
            EditorApplication.hierarchyWindowItemOnGUI += HierarchyCustomizer;
        }

        static void EditorUpdate()
        {
            if(!UseFixedUpdate) { return; }
            if(EditorApplication.timeSinceStartup > (startTime + elapsedInterval + UpdateInterval))
            {
                elapsedInterval += UpdateInterval;
                EditorFixedUpdate();
            }
        }

        static void EditorFixedUpdate()
        {
            UpdateReferences();
        }

        public static void UpdateReferences()
        {
            if(DUI.DUISettings == null) { return; }
            if(!DUI.DUISettings.HierarchyManager_Enabled) { return; }

            allTheGameObjectsInScene = Object.FindObjectsOfType<GameObject>();
#if dUI_PlayMaker
            GetPlaymakerEventDispatchersReferences();
#endif
            GetUITriggersReferences();
            GetUIManagerReference();
            GetSoundyReference();
            GetUINotificationManagerReference();
            GetOrientationManagerReference();
            GetSceneLoaderReference();
            GetUIButtonsReferences();
            GetUICanvasesReferences();
            GetUINotificationsReferences();
            GetUIElementsReferences();
            GetUIEffectsReferences();
        }
        static void GetPlaymakerEventDispatchersReferences()
        {
#if dUI_PlayMaker
            if (!DUI.DUISettings.HierarchyManager_PlaymakerEventDispatcher_ShowIcon) { return; }
            playmakerEventDispatcherIDs.Clear();
            for (int i = 0; i < allTheGameObjectsInScene.Length; i++)
            {
                ped = null;
                ped = allTheGameObjectsInScene[i].GetComponent<PlaymakerEventDispatcher>();
                if (ped == null) { continue; }
                playmakerEventDispatcherIDs.Add(allTheGameObjectsInScene[i].GetInstanceID(), ped);
            }
#endif
        }
        static void GetUITriggersReferences()
        {
            if(!DUI.DUISettings.HierarchyManager_UITrigger_ShowIcon) { return; }
            uiTriggerIDs.Clear();
            for(int i = 0; i < allTheGameObjectsInScene.Length; i++)
            {
                uit = null;
                uit = allTheGameObjectsInScene[i].GetComponent<UITrigger>();
                if(uit == null) { continue; }
                uiTriggerIDs.Add(allTheGameObjectsInScene[i].GetInstanceID(), uit);
            }
        }
        static void GetUIManagerReference()
        {
            if(!DUI.DUISettings.HierarchyManager_UIManager_ShowIcon) { return; }
            UIManagerID = 0;
            for(int i = 0; i < allTheGameObjectsInScene.Length; i++)
            {
                if(allTheGameObjectsInScene[i].GetComponent<UIManager>() != null)
                {
                    UIManagerID = allTheGameObjectsInScene[i].GetInstanceID();
                    break;
                }
            }
        }
        static void GetSoundyReference()
        {
            if(!DUI.DUISettings.HierarchyManager_Soundy_ShowIcon) { return; }
            SoundyID = 0;
            for(int i = 0; i < allTheGameObjectsInScene.Length; i++)
            {
                if(allTheGameObjectsInScene[i].GetComponent<Soundy>() != null)
                {
                    SoundyID = allTheGameObjectsInScene[i].GetInstanceID();
                    break;
                }
            }
        }
        static void GetUINotificationManagerReference()
        {
            if(!DUI.DUISettings.HierarchyManager_UINotificationManager_ShowIcon) { return; }
            UINotificationManagerID = 0;
            for(int i = 0; i < allTheGameObjectsInScene.Length; i++)
            {
                if(allTheGameObjectsInScene[i].GetComponent<UINotificationManager>() != null)
                {
                    UINotificationManagerID = allTheGameObjectsInScene[i].GetInstanceID();
                    break;
                }
            }
        }
        static void GetOrientationManagerReference()
        {
            if(!DUI.DUISettings.HierarchyManager_OrientationManager_ShowIcon) { return; }
            OrientationManagerID = 0;
            for(int i = 0; i < allTheGameObjectsInScene.Length; i++)
            {
                if(allTheGameObjectsInScene[i].GetComponent<OrientationManager>() != null)
                {
                    OrientationManagerID = allTheGameObjectsInScene[i].GetInstanceID();
                    break;
                }
            }
        }
        static void GetSceneLoaderReference()
        {
            if(!DUI.DUISettings.HierarchyManager_SceneLoader_ShowIcon) { return; }
            SceneLoaderID = 0;
            for(int i = 0; i < allTheGameObjectsInScene.Length; i++)
            {
                if(allTheGameObjectsInScene[i].GetComponent<SceneLoader>() != null)
                {
                    SceneLoaderID = allTheGameObjectsInScene[i].GetInstanceID();
                    break;
                }
            }
        }
        static void GetUIButtonsReferences()
        {
            if(!DUI.DUISettings.HierarchyManager_UIButton_Enabled) { return; }
            uiButtonIDs.Clear();
            for(int i = 0; i < allTheGameObjectsInScene.Length; i++)
            {
                uib = null;
                uib = allTheGameObjectsInScene[i].GetComponent<UIButton>();
                if(uib == null) { continue; }
                uiButtonIDs.Add(allTheGameObjectsInScene[i].GetInstanceID(), uib);
            }
        }
        static void GetUICanvasesReferences()
        {
            if(!DUI.DUISettings.HierarchyManager_UICanvas_Enabled) { return; }
            uiCanvasIDs.Clear();
            for(int i = 0; i < allTheGameObjectsInScene.Length; i++)
            {
                uic = null;
                uic = allTheGameObjectsInScene[i].GetComponent<UICanvas>();
                if(uic == null) { continue; }
                uiCanvasIDs.Add(allTheGameObjectsInScene[i].GetInstanceID(), uic);
            }
        }
        static void GetUINotificationsReferences()
        {
            if(!DUI.DUISettings.HierarchyManager_UINotification_ShowIcon) { return; }
            uiNotificationIDs.Clear();
            for(int i = 0; i < allTheGameObjectsInScene.Length; i++)
            {
                uin = null;
                uin = allTheGameObjectsInScene[i].GetComponent<UINotification>();
                if(uin == null) { continue; }
                uiNotificationIDs.Add(allTheGameObjectsInScene[i].GetInstanceID(), uin);
            }
        }
        static void GetUIElementsReferences()
        {
            if(!DUI.DUISettings.HierarchyManager_UIElement_Enabled) { return; }
            uiElementIDs.Clear();
            for(int i = 0; i < allTheGameObjectsInScene.Length; i++)
            {
                uie = null;
                uie = allTheGameObjectsInScene[i].GetComponent<UIElement>();
                if(uie == null) { continue; }
                uiElementIDs.Add(allTheGameObjectsInScene[i].GetInstanceID(), uie);
            }
        }
        static void GetUIEffectsReferences()
        {
            if(!DUI.DUISettings.HierarchyManager_UIEffect_Enabled) { return; }
            uiEffectIDs.Clear();
            for(int i = 0; i < allTheGameObjectsInScene.Length; i++)
            {
                uief = null;
                uief = allTheGameObjectsInScene[i].GetComponent<UIEffect>();
                if(uief == null) { continue; }
                uiEffectIDs.Add(allTheGameObjectsInScene[i].GetInstanceID(), uief);
            }
        }

        static void HierarchyCustomizer(int instanceID, Rect selectionRect)
        {
            if(DUI.DUISettings == null || !DUI.DUISettings.HierarchyManager_Enabled) { return; }

            QLabel = new QLabel("", Style.Text.Small);

            rect = new Rect(selectionRect);
            rect.x = rect.xMax - iconWidth;
            rect.width = iconWidth;
            rect.height = iconHeight;

#if dUI_PlayMaker
            if (DUI.DUISettings.HierarchyManager_PlaymakerEventDispatcher_ShowIcon && playmakerEventDispatcherIDs.ContainsKey(instanceID) && playmakerEventDispatcherIDs[instanceID] != null) { GUI.Label(rect, DUIResources.iconPlayMakerEventDispatcher.texture); rect.x -= iconWidth; }
#endif
            if(DUI.DUISettings.HierarchyManager_UITrigger_ShowIcon && uiTriggerIDs.ContainsKey(instanceID) && uiTriggerIDs[instanceID] != null) { GUI.Label(rect, DUIResources.iconUITrigger.texture); rect.x -= iconWidth; }
            if(DUI.DUISettings.HierarchyManager_UIManager_ShowIcon && UIManagerID == instanceID) { GUI.Label(rect, DUIResources.iconUIManager.texture); rect.x -= iconWidth; }
            if(DUI.DUISettings.HierarchyManager_Soundy_ShowIcon && SoundyID == instanceID) { GUI.Label(rect, DUIResources.iconSoundy.texture); rect.x -= iconWidth; }
            if(DUI.DUISettings.HierarchyManager_UINotificationManager_ShowIcon && UINotificationManagerID == instanceID) { GUI.Label(rect, DUIResources.iconUINotificationManager.texture); rect.x -= iconWidth; }
            if(DUI.DUISettings.HierarchyManager_OrientationManager_ShowIcon && OrientationManagerID == instanceID) { GUI.Label(rect, DUIResources.iconOrientationManager.texture); rect.x -= iconWidth; }
            if(DUI.DUISettings.HierarchyManager_SceneLoader_ShowIcon && SceneLoaderID == instanceID) { GUI.Label(rect, DUIResources.iconSceneLoader.texture); rect.x -= iconWidth; }

            if(DUI.DUISettings.HierarchyManager_UICanvas_Enabled && uiCanvasIDs.ContainsKey(instanceID) && uiCanvasIDs[instanceID] != null)
            {
                if(DUI.DUISettings.HierarchyManager_UICanvas_ShowIcon) { GUI.Label(rect, DUIResources.iconUICanvas.texture); } else { rect.x += iconWidth; }
                uic = uiCanvasIDs[instanceID];
                QLabel.text = "";
                QLabel.style = Style.Text.Small;
                QLabel.text = (DUI.DUISettings.HierarchyManager_UICanvas_ShowCanvasName ? "[ " + uic.canvasName + " ]" : "")
                              + (DUI.DUISettings.HierarchyManager_UICanvas_ShowSortingLayerNameAndOrder ? " " + uic.Canvas.sortingLayerName + " " + uic.Canvas.sortingOrder : "");
                if(!string.IsNullOrEmpty(QLabel.text))
                {
                    rect.x -= QLabel.x;
                    rect.width = QLabel.x;
                    GUI.Label(rect, QLabel.text, QStyles.GetStyle(QStyles.GetStyleName(QLabel.style)));
                }
            }

            if(DUI.DUISettings.HierarchyManager_UIButton_Enabled && uiButtonIDs.ContainsKey(instanceID) && uiButtonIDs[instanceID] != null)
            {
                if(DUI.DUISettings.HierarchyManager_UIButton_ShowIcon) { GUI.Label(rect, DUIResources.iconUIButton.texture); } else { rect.x += iconWidth; }
                uib = uiButtonIDs[instanceID];
                QLabel.text = "";
                QLabel.style = Style.Text.Small;
                QLabel.text = (DUI.DUISettings.HierarchyManager_UIButton_ShowButtonCategory ? "[ " + uib.buttonCategory + " ]" : "")
                       + (DUI.DUISettings.HierarchyManager_UIButton_ShowButtonName ? "[ " + uib.buttonName + " ]" : "");
                if(!string.IsNullOrEmpty(QLabel.text))
                {
                    rect.x -= QLabel.x;
                    rect.width = QLabel.x;
                    GUI.Label(rect, QLabel.text, QStyles.GetStyle(QStyles.GetStyleName(QLabel.style)));
                }
            }

            if(DUI.DUISettings.HierarchyManager_UINotification_ShowIcon && uiNotificationIDs.ContainsKey(instanceID)) { GUI.Label(rect, DUIResources.iconUINotification.texture); rect.x -= iconWidth; }
            if(DUI.DUISettings.HierarchyManager_UIElement_Enabled && uiElementIDs.ContainsKey(instanceID) && uiElementIDs[instanceID] != null)
            {
                if(DUI.DUISettings.HierarchyManager_UIElement_ShowIcon) { GUI.Label(rect, DUIResources.iconUIElement.texture); } else { rect.x += iconWidth; }
                uie = uiElementIDs[instanceID];
                QLabel.text = "";
                QLabel.style = Style.Text.Small;
                if(uie.linkedToNotification)
                {
                    QLabel.text = "linked to notification";
                }
                else
                {
                    QLabel.text = (DUI.DUISettings.HierarchyManager_UIElement_ShowElementCategory ? "[ " + uie.elementCategory + " ]" : "")
                            + (DUI.DUISettings.HierarchyManager_UIElement_ShowElementName ? "[ " + uie.elementName + " ]" : "")
                            + (DUI.DUISettings.HierarchyManager_UIElement_ShowSortingLayerNameAndOrder ? " " + uie.Canvas.sortingLayerName + " " + uie.Canvas.sortingOrder : "");
                }
                if(!string.IsNullOrEmpty(QLabel.text))
                {
                    rect.x -= QLabel.x;
                    rect.width = QLabel.x;
                    GUI.Label(rect, QLabel.text, QStyles.GetStyle(QStyles.GetStyleName(QLabel.style)));
                }
            }

            if(DUI.DUISettings.HierarchyManager_UIEffect_Enabled && uiEffectIDs.ContainsKey(instanceID) && uiEffectIDs[instanceID] != null)
            {
                if(DUI.DUISettings.HierarchyManager_UIEffect_ShowIcon) { GUI.Label(rect, DUIResources.iconUIEffect.texture); } else { rect.x += iconWidth; }
                uief = uiEffectIDs[instanceID];
                if(DUI.DUISettings.HierarchyManager_UIEffect_ShowSortingLayerNameAndOrder)
                {
                    if(uief.targetUIElement != null)
                    {
                        if(uief.targetUIElement.linkedToNotification)
                        {
                            QLabel.text = "linked to notification";
                        }
                        else
                        {
                            QLabel.text = (uief.useCustomSortingLayerName
                                     ? uief.customSortingLayerName
                                     : uief.targetUIElement.Canvas.overrideSorting
                                       ? uief.targetUIElement.Canvas.sortingLayerName
                                       : uief.targetUIElement.Canvas.rootCanvas.sortingLayerName)
                                     + " " +
                                     (uief.useCustomOrderInLayer
                                     ? uief.customOrderInLayer
                                     : uief.targetUIElement.Canvas.overrideSorting
                                       ? uief.targetUIElement.Canvas.sortingOrder
                                       : uief.targetUIElement.Canvas.rootCanvas.sortingOrder);
                        }
                    }
                    else
                    {
                        QLabel.text = "DISABLED";
                    }
                    rect.x -= QLabel.x;
                    rect.width = QLabel.x;
                    GUI.Label(rect, QLabel.text, QStyles.GetStyle(QStyles.GetStyleName(QLabel.style)));
                }
            }

        }
    }
}
