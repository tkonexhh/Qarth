// Copyright (c) 2015 - 2018 Doozy Entertainment / Marlink Trading SRL. All Rights Reserved.
// This code can only be used under the standard Unity Asset Store End User License Agreement
// A Copy of the EULA APPENDIX 1 is available at http://unity3d.com/company/legal/as_terms

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DoozyUI
{
    [AddComponentMenu(DUI.COMPONENT_MENU_UIEFFECT, DUI.MENU_PRIORITY_UIEFFECT)]
    [DisallowMultipleComponent]
    public class UIEffect : MonoBehaviour
    {
        #region Context Menu Methods
#if UNITY_EDITOR
        [UnityEditor.MenuItem(DUI.GAMEOBJECT_MENU_UIEFFECT, false, DUI.MENU_PRIORITY_UIEFFECT)]
        static void CreateEffect(UnityEditor.MenuCommand menuCommand)
        {
            GameObject selectedGO = menuCommand.context as GameObject;
            GameObject go = new GameObject("UIEffect", typeof(UIEffect));
            UnityEditor.GameObjectUtility.SetParentAndAlign(go, menuCommand.context as GameObject);
            UnityEditor.Undo.RegisterCreatedObjectUndo(go, "Create " + go.name);
            if (selectedGO != null && selectedGO.GetComponent<RectTransform>() != null)
            {
                go.AddComponent<RectTransform>();
                go.GetComponent<RectTransform>().localScale = Vector3.one;
                go.GetComponent<UIEffect>().targetUIElement = selectedGO.GetComponent<UIElement>();
                if (go.GetComponent<UIEffect>().targetUIElement != null)
                {
                    go.name = DUI.DUISettings.UIEffect_Inspector_RenameGameObjectPrefix + go.GetComponent<UIEffect>().targetUIElement.elementName + DUI.DUISettings.UIEffect_Inspector_RenameGameObjectSuffix;
                }
            }
            UnityEditor.Selection.activeObject = go;
        }
#endif
        #endregion

        /// <summary>
        /// Default sorting layer name value.
        /// </summary>
        public const string DEFAULT_CUSTOM_SORTING_LAYER_NAME = "Default";
        /// <summary>
        /// Default order in layer value.
        /// </summary>
        public const int DEFAULT_CUSTOM_ORDER_IN_LAYER = 0;
        /// <summary>
        /// Default sorting order step value.
        /// </summary>
        public const int DEFAULT_DEFAULT_SORTING_ORDER_STEP = 1;

        /// <summary>
        /// Determines the sorting order.
        /// </summary>
        public enum EffectPosition { InFrontOfTarget, BehindTarget };

        /// <summary>
        /// The ParticleSystem that this UIEffect is controlling.
        /// </summary>
        public ParticleSystem targetParticleSystem;
        /// <summary>
        /// A reference to the main module of the target particle system.
        /// </summary>
        private ParticleSystem.MainModule targetParticleSystemMainModule;

        /// <summary>
        /// The UIElement that controls this UIEffect.
        /// </summary>
        public UIElement targetUIElement = null;

        /// <summary>
        /// Time interval to wait to play this effect, after the show command has been sent for the target UIElement.
        /// </summary>
        public float startDelay = 0f;

        /// <summary>
        /// Should this effect start playing on awake or not. Default is set to false.
        /// </summary>
        public bool playOnAwake = false;

        /// <summary>
        /// Should the effect stop instantly and clear, after the hide command has been sent, or should it stop and let the particles disappear after their set lifetime. Default is set to false.
        /// </summary>
        public bool stopInstantly = false;

        /// <summary>
        /// Used by the custom inspector to allow you to type a layer name instead of selecting it from the layers dropdown list. Use this only if you know what you are doing.
        /// </summary>
        public bool useCustomSortingLayerName = false;
        /// <summary>
        /// Used by the custom inspector to set your custom layer name. Use this only if you know what you are doing.
        /// </summary>
        public string customSortingLayerName = DEFAULT_CUSTOM_SORTING_LAYER_NAME;

        /// <summary>
        /// Used by the custom inspector to allow you to type a order in layer instead of getting it automatically set by this UIEffect. Use this only if you know what you are doing.
        /// </summary>
        public bool useCustomOrderInLayer = false;
        /// <summary>
        /// Used by the custom inspector to set your custom order in layer. Use this only if you know what you are doing.
        /// </summary>
        public int customOrderInLayer = DEFAULT_CUSTOM_ORDER_IN_LAYER;

        /// <summary>
        /// Determines the order in layer by adding (if InFrontOfTarget) or subtracting (if BehindTarget) the set number of sorting order steps to the order in layer value.
        /// </summary>
        public EffectPosition effectPosition = EffectPosition.InFrontOfTarget;
        /// <summary>
        /// Considering the target's [Canvas][Order in Layer][value] - we adjust the [ParticleSystem][Renderer][Order in Layer][value] with this sorting step (by adding, if set to InFrontOfTarget or subtracting, if set BehindTarget)
        /// </summary>
        public int sortingOrderStep = DEFAULT_DEFAULT_SORTING_ORDER_STEP;

        /// <summary>
        /// Used by the UINotification. If this effect is used by a notification, then the notification should handle it's registration process in order to use an auto generated name. Do not change this value yourself.
        /// </summary>
        public bool autoRegister = true;
        /// <summary>
        /// Keeps track if this UIEffect is visible or not. Do not change this value yourself.
        /// </summary>
        public bool isVisible = true;

        /// <summary>
        /// Internal variable used to hide this UIEffect after the last particle has dissapeared from the screen. This is used when stopInstantly is set to false.
        /// </summary>
        private float lifetime;
        /// <summary>
        /// Internal variable that holds a reference to the coroutine that resets this effect.
        /// </summary>
        private Coroutine cReset;
        /// <summary>
        /// Internal variable that holds a reference to the coroutine that plays this effect.
        /// </summary>
        private Coroutine cPlay;

        /// <summary>
        /// Internal array the holds all the ParticleSystems affected by this effect.
        /// </summary>
        private ParticleSystem[] allThePS;
        /// <summary>
        /// Internal variable that holds a reference to the target Canvas (found on the target UIElement).
        /// </summary>
        private Canvas m_targetCanvas;
        /// <summary>
        /// Returns the target Canvas (found on the target UIElement).
        /// </summary>
        private Canvas TargetCanvas
        {
            get
            {
                if (m_targetCanvas == null)
                {
                    if (targetUIElement != null)
                    {
                        m_targetCanvas = targetUIElement.GetComponent<Canvas>();
                        if (m_targetCanvas == null)
                        {
                            m_targetCanvas = targetUIElement.gameObject.AddComponent<Canvas>();
                        }
                    }
                }
                return m_targetCanvas;
            }
        }
        /// <summary>
        /// Internal variable that holds the the target physics layer id value. Used to be sure this effect is seen by the indended camera.
        /// </summary>
        private int targetPhysicsLayerID;
        /// <summary>
        /// Internal variable that holds the sorting layer name value that this effect is set to.
        /// </summary>
        private string targetSortingLayerName;
        /// <summary>
        /// Internal variable that holds order in layer value that this effect is set to.
        /// </summary>
        private int targetOrderInLayer;

        private void Reset()
        {
            if (DUI.DUISettings == null) { return; }
            playOnAwake = DUI.DUISettings.UIEffect_playOnAwake;
            stopInstantly = DUI.DUISettings.UIEffect_stopInstantly;
            startDelay = DUI.DUISettings.UIEffect_startDelay;

            useCustomSortingLayerName = DUI.DUISettings.UIEffect_useCustomSortingLayerName;
            customSortingLayerName = DUI.DUISettings.UIEffect_customSortingLayerName;

            useCustomOrderInLayer = DUI.DUISettings.UIEffect_useCustomOrderInLayer;
            customOrderInLayer = DUI.DUISettings.UIEffect_customOrderInLayer;

            effectPosition = DUI.DUISettings.UIEffect_effectPosition;
            sortingOrderStep = DUI.DUISettings.UIEffect_sortingOrderStep;
        }

        private void Awake()
        {
            if (targetParticleSystem == null) { targetParticleSystem = GetComponent<ParticleSystem>(); }
            targetParticleSystemMainModule = targetParticleSystem.main;
            targetParticleSystemMainModule.playOnAwake = playOnAwake;
            lifetime = targetParticleSystemMainModule.startLifetimeMultiplier;
            cReset = null;
            cPlay = null;
        }

        private void OnEnable()
        {
            if (targetParticleSystem == null) { targetParticleSystem = GetComponent<ParticleSystem>(); }

            if (targetUIElement == null)
            {
                Debug.Log("[DoozyUI] The UIEffect on [" + gameObject.name + "] gameObject is disabled. It will not work because it has no target ParticleSystem set.");
                return;
            }

            if (targetUIElement == null)
            {
                Debug.Log("[DoozyUI] The UIEffect on [" + gameObject.name + "] gameObject is disabled. It will not work because it has no target UIElement set.");
                playOnAwake = false;
                stopInstantly = true;
                ResetParticleSystem();
                return;
            }

            if (autoRegister) { RegisterToUIManager(); }

            targetParticleSystem.Stop(true);
            targetParticleSystem.Clear(true);
            isVisible = false;

            if (playOnAwake)
            {
                targetParticleSystem.Play(true);
                isVisible = true;
            }
        }

        private void OnDisable()
        {
            UnregisterFromUIManager();
        }

        private void OnDestroy()
        {
            UnregisterFromUIManager();
        }

        private void Start()
        {
            if (targetUIElement == null && targetParticleSystem == null) { return; }
            UpdateSorting();
        }

        #region RegisterToUIManager / UnregisterFromUIManager
        /// <summary>
        /// Registers this UIEffect to the UIManager.
        /// </summary>
        public void RegisterToUIManager()
        {
            if (targetUIElement == null)
            {
                Debug.Log("[DoozyUI] You cannot register the UIEffect on [" + gameObject.name + "] gameObject. That is not possible because it has no target UIElement set.");
                return;
            }
            if (UIManager.EffectDatabase.ContainsKey(targetUIElement.elementName))
            {
                if (UIManager.EffectDatabase[targetUIElement.elementName] == null) { UIManager.EffectDatabase[targetUIElement.elementName] = new List<UIEffect>(); }
                if (UIManager.EffectDatabase[targetUIElement.elementName].Contains(this)) { return; }
                UIManager.EffectDatabase[targetUIElement.elementName].Add(this);
            }
            else
            {
                UIManager.EffectDatabase.Add(targetUIElement.elementName, new List<UIEffect>() { this });
            }
        }
        /// <summary>
        /// Unregisteres this UIEffect from the UIManager.
        /// </summary>
        public void UnregisterFromUIManager()
        {
            if (targetUIElement == null)
            {
                Debug.Log("[DoozyUI] You cannot unregister the UIEffect on [" + gameObject.name + "] gameObject. That is not possible because it has no target UIElement set.");
                return;
            }
            if (UIManager.EffectDatabase == null) { return; }
            if (UIManager.EffectDatabase.ContainsKey(targetUIElement.elementName))
            {
                UIManager.EffectDatabase[targetUIElement.elementName].Remove(this);
                if (UIManager.EffectDatabase[targetUIElement.elementName].Count == 0) { UIManager.EffectDatabase.Remove(targetUIElement.elementName); }
            }
        }
        #endregion

        /// <summary>
        /// Updates the sorting of this effect to the set and calculated values.
        /// </summary>
        public void UpdateSorting()
        {
            if (targetUIElement == null) { return; }
            targetPhysicsLayerID = targetUIElement.transform.gameObject.layer;
            gameObject.layer = targetPhysicsLayerID; //Update the sorting layer (for the camera)
            foreach (Transform child in transform)
            {
                child.gameObject.layer = targetPhysicsLayerID; //Update the sorting layer in children (for the camera)
            }

            targetSortingLayerName = TargetCanvas.overrideSorting
                                     ? TargetCanvas.sortingLayerName
                                     : TargetCanvas.rootCanvas.sortingLayerName;

            targetOrderInLayer = TargetCanvas.overrideSorting
                                 ? TargetCanvas.sortingOrder
                                 : TargetCanvas.rootCanvas.sortingOrder;

            allThePS = GetComponentsInChildren<ParticleSystem>(true);
            for (int i = 0; i < allThePS.Length; i++)
            {
                allThePS[i].GetComponent<Renderer>().sortingLayerName = useCustomSortingLayerName ? customSortingLayerName : targetSortingLayerName;
                allThePS[i].GetComponent<Renderer>().sortingOrder = useCustomOrderInLayer
                                                                    ? customOrderInLayer
                                                                    : effectPosition == EffectPosition.InFrontOfTarget
                                                                      ? targetOrderInLayer + sortingOrderStep
                                                                      : targetOrderInLayer - sortingOrderStep;
            }
        }

        /// <summary>
        /// Shows this UIEffect (similar to the Show method of the UIElement).
        /// </summary>
        public void Show(bool forced = false)
        {
            if (!isVisible || forced)
            {
                if (cReset != null) { StopCoroutine(cReset); }
                if (cPlay != null) { StopCoroutine(cPlay); }
                cPlay = StartCoroutine(iPlay(startDelay));
            }
        }

        /// <summary>
        /// Hides this UIEffect (similar to the Hide method of the UIElement).
        /// </summary>
        /// <param name="forced"></param>
        public void Hide(bool forced = false)
        {
            if (isVisible || forced)
            {
                ResetParticleSystem();
            }
        }

        /// <summary>
        /// Resets this effect taking into accoutn all the settings.
        /// </summary>
        private void ResetParticleSystem()
        {
            isVisible = false;

            if (stopInstantly)
            {
                targetParticleSystem.Stop(true);
                targetParticleSystem.Clear(true);
                cReset = null;
            }
            else
            {
                cReset = StartCoroutine(iReset(lifetime));
            }
        }

        /// <summary>
        /// Executes the reset for this effect in realtime.
        /// </summary>
        /// <param name="resetDelay"></param>
        /// <returns></returns>
        IEnumerator iReset(float resetDelay)
        {
            targetParticleSystem.Stop(true);
            yield return new WaitForSecondsRealtime(resetDelay);
            targetParticleSystem.Clear(true);
            cReset = null;
        }

        /// <summary>
        /// Executes the play for this effect after the set startDelay in realtime.
        /// </summary>
        /// <param name="startDelay"></param>
        /// <returns></returns>
        IEnumerator iPlay(float startDelay = 0)
        {
            yield return new WaitForSecondsRealtime(startDelay);
            targetParticleSystem.Play(true);
            isVisible = true;
            cPlay = null;
        }
    }
}
