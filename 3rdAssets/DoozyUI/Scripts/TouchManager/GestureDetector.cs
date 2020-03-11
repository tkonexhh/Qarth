// Copyright (c) 2015 - 2018 Doozy Entertainment / Marlink Trading SRL. All Rights Reserved.
// This code can only be used under the standard Unity Asset Store End User License Agreement
// A Copy of the EULA APPENDIX 1 is available at http://unity3d.com/company/legal/as_terms

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace DoozyUI.Gestures
{
    public class GestureDetector : MonoBehaviour
    {
        /// <summary>
        /// Prints debug messages related to detected gestures at runtime.
        /// </summary>
        public bool debug = false;

        /// <summary>
        /// If true, it will trigger this Gesture Detector regardless of the targetGameObject.
        /// </summary>
        public bool isGlobalGestureDetector = false;

        /// <summary>
        /// Allows you to set another targetGameObject reference (in the Inspector); other than the gameObject this component is attached to
        /// </summary>
        public bool overrideTarget = false;
        /// <summary>
        /// Only gestures performed on the target game object will trigger this Gesture Detector.
        /// </summary>
        public GameObject targetGameObject;

        /// <summary>
        /// The gesture type this Gesture Detector will react to
        /// </summary>
        public GestureType gestureType = GestureType.Tap;
        /// <summary>
        /// The swipe direction this Gesture Detector will react to.
        /// This works only if the gestureType is set to GestureType.Swipe
        /// </summary>
        public Swipe swipeDirection = Swipe.None;

        /// <summary>
        /// UnityEvent invoked on tap
        /// </summary>
        public UnityEvent OnTap = new UnityEvent();
        /// <summary>
        /// UnityEvent invoked on long tap
        /// </summary>
        public UnityEvent OnLongTap = new UnityEvent();
        /// <summary>
        /// UnityEvent invoked on swipe (it has to be the proper swipe direction)
        /// </summary>
        public UnityEvent OnSwipe = new UnityEvent();

#if dUI_DoozyUI
        /// <summary>
        /// List of game events that are sent by the GestureDetector when it executes its actions.
        /// </summary>
        public List<string> gameEvents;
#endif

        /// <summary>
        /// Action invoked on tap
        /// </summary>
        public Action<TouchInfo> onTapAction;
        /// <summary>
        /// Action invoked on long tap
        /// </summary>
        public Action<TouchInfo> onLongTapAction;
        /// <summary>
        /// Action invoked on swipe (it has to be the proper swipe direction)
        /// </summary>
        public Action<TouchInfo> onSwipeAction;

#if dUI_DoozyUI
        /// <summary>
        /// UINavigation settings.
        /// </summary>
        public NavigationPointerData navigationPointerData = new NavigationPointerData();
#endif

        void Awake()
        {
            if(!overrideTarget || (overrideTarget && targetGameObject == null))
            {
                targetGameObject = gameObject;
            }
        }

        void OnEnable()
        {
            RegisterToTouchManager();
        }

        void OnDisable()
        {
            UnregisterFromTouchManager();
        }

        void OnDestroy()
        {
            UnregisterFromTouchManager();
        }

        void RegisterToTouchManager()
        {
            switch(gestureType)
            {
                case GestureType.Tap: TouchManager.Instance.onTapAction += HandleTap; break;
                case GestureType.LongTap: TouchManager.Instance.onLongTapAction += HandleLongTap; break;
                case GestureType.Swipe: TouchManager.Instance.onSwipeAction += HandleSwipe; break;
            }
        }

        void UnregisterFromTouchManager()
        {
            if(TouchManager.applicationIsQuitting
                || TouchManager.Instance == null)
            {
                return;
            }
            switch(gestureType)
            {
                case GestureType.Tap: TouchManager.Instance.onTapAction -= HandleTap; break;
                case GestureType.LongTap: TouchManager.Instance.onLongTapAction -= HandleLongTap; break;
                case GestureType.Swipe: TouchManager.Instance.onSwipeAction -= HandleSwipe; break;
            }
        }

        void HandleTap(TouchInfo touchInfo)
        {
            if(!isGlobalGestureDetector) //if this is not a global detector -> check that is a valid gameObject and the correct touchInfo target
            {
                if(targetGameObject == null || touchInfo.gameObject != targetGameObject)
                {
                    return;
                }
            }

            if(debug) { Debug.LogFormat("[GestureDetector] HandleTap on {0}: {1}", gameObject.name, touchInfo); }

            UpdateTheNavigationHistory();
            if(onSwipeAction != null) { onSwipeAction.Invoke(touchInfo); }
            OnTap.Invoke();

#if dUI_DoozyUI
            if(gameEvents != null && gameEvents.Count > 0)
            {
                StartCoroutine(SendGameEventsInTheNextFrame());
            }
#endif
        }

        void HandleLongTap(TouchInfo touchInfo)
        {
            if(!isGlobalGestureDetector) //if this is not a global detector -> check that is a valid gameObject and the correct touchInfo target
            {
                if(targetGameObject == null || touchInfo.gameObject != targetGameObject)
                {
                    return;
                }
            }

            if(debug) { Debug.LogFormat("[GestureDetector] HandleLongTap on {0}: {1}", gameObject.name, touchInfo); }

            UpdateTheNavigationHistory();
            if(onLongTapAction != null) { onLongTapAction.Invoke(touchInfo); }
            OnLongTap.Invoke();

#if dUI_DoozyUI
            if(gameEvents != null && gameEvents.Count > 0)
            {
                StartCoroutine(SendGameEventsInTheNextFrame());
            }
#endif
        }

        void HandleSwipe(TouchInfo touchInfo)
        {
            if(!isGlobalGestureDetector) //if this is not a global detector -> check that is a valid gameObject and the correct touchInfo target
            {
                if(targetGameObject == null || touchInfo.gameObject != targetGameObject)
                {
                    return;
                }
            }
            if(touchInfo.direction != swipeDirection) //check that the this is the swipe this gesture detector is set to react to
            {
                return;
            }

            if(debug) { Debug.LogFormat("[GestureDetector] HandleSwipe on {0}: {1}", gameObject.name, touchInfo); }

            UpdateTheNavigationHistory();
            if(onSwipeAction != null) { onSwipeAction.Invoke(touchInfo); }
            OnSwipe.Invoke();

#if dUI_DoozyUI
            if(gameEvents != null && gameEvents.Count > 0)
            {
                StartCoroutine(SendGameEventsInTheNextFrame());
            }
#endif
        }

        void UpdateTheNavigationHistory()
        {
#if dUI_DoozyUI
            if(!UIManager.IsNavigationEnabled) { return; }
            UINavigation.UpdateTheNavigationHistory(navigationPointerData.Copy());
#endif
        }

#if dUI_DoozyUI
        IEnumerator SendGameEventsInTheNextFrame()
        {

            yield return null;
            UIManager.SendGameEvents(gameEvents);
        }
#endif
    }
}
