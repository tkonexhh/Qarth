// Copyright (c) 2015 - 2018 Doozy Entertainment / Marlink Trading SRL. All Rights Reserved.
// This code can only be used under the standard Unity Asset Store End User License Agreement
// A Copy of the EULA APPENDIX 1 is available at http://unity3d.com/company/legal/as_terms

using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.EventSystems;

namespace DoozyUI.Gestures
{
    public class TouchManager : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {

        #region Context Menu
#if UNITY_EDITOR

        public const int MENU_PRIORITY = 103;
        public const string TOOLS_MENU = "Tools/DoozyUI/Managers/TouchManager";
        public const string GAMEOBJECT_MENU = "GameObject/DoozyUI/Managers/TouchManager";

#if dUI_DoozyUI
        [UnityEditor.MenuItem(DUI.TOOLS_MENU_TOUCH_MANAGER, false, DUI.MENU_PRIORITY_TOUCH_MANAGER)]
        [UnityEditor.MenuItem(DUI.GAMEOBJECT_MENU_TOUCH_MANAGER, false, DUI.MENU_PRIORITY_TOUCH_MANAGER)]
#else
        [UnityEditor.MenuItem(TOOLS_MENU, false, MENU_PRIORITY)]
        [UnityEditor.MenuItem(GAMEOBJECT_MENU, false, MENU_PRIORITY)]
#endif
        static void CreateTouchManager(UnityEditor.MenuCommand menuCommand)
        {
            AddTouchManagerToScene();
        }
#endif

        public static TouchManager AddTouchManagerToScene()
        {
            if(FindObjectOfType<TouchManager>() != null)
            {
                Debug.Log("[TouchManager] Cannot add another TouchManager to this Scene because you don't need more than one.");
#if UNITY_EDITOR
                UnityEditor.Selection.activeObject = FindObjectOfType<TouchManager>();
#endif
                return null;
            }

            GameObject go = new GameObject("TouchManager", typeof(TouchManager));
#if UNITY_EDITOR
            UnityEditor.Undo.RegisterCreatedObjectUndo(go, "Added " + go.name);
            //UnityEditor.Selection.activeObject = go;
#endif
            return go.GetComponent<TouchManager>();
        }
        #endregion

        protected TouchManager() { }
        private static TouchManager _instance;
        public static TouchManager Instance
        {
            get
            {
                if(_instance == null)
                {
                    if(applicationIsQuitting)
                    {
                        Debug.LogWarning("[Singleton] Instance '" + typeof(TouchManager) + "' already destroyed on application quit. Won't create again - returning null.");
                        return null;
                    }

                    _instance = FindObjectOfType<TouchManager>();

                    if(_instance == null)
                    {
                        GameObject singleton = new GameObject("TouchManager");
                        _instance = singleton.AddComponent<TouchManager>();
                        DontDestroyOnLoad(singleton);
                    }
                }
                return _instance;
            }
        }

        public static bool applicationIsQuitting = false;
        private void OnApplicationQuit()
        {
            applicationIsQuitting = true;
        }

        public Action<TouchInfo> onSwipeAction;
        public Action<TouchInfo> onTapAction;
        public Action<TouchInfo> onLongTapAction;

        private bool touchInProgress = false;
        public bool TouchInProgress
        {
            get { return touchInProgress; }
            set { touchInProgress = value; }
        }

        /// <summary>
        /// Prints debug messages related to detected gestures at runtime.
        /// </summary>
        public bool debug = false;

        /// <summary>
        /// The minimum swipe distance to be considered a swipe.
        /// </summary>
        [Range(0f, 200f)]
        public float minSwipeLength = 10f;

        /// <summary>
        /// Time period for a finger to be touching the target device, in order for the tap to be considered a long tap (long press)
        /// </summary>
        public float longTapDuration = 0.5f;

        /// <summary>
        /// Sets if the TouchManager should detect swipes on eight or on four cardinal directions
        /// </summary>
        public bool useEightDirections = false;

        private Vector2 currentSwipe;
        private bool swipeEnded = false;
        private TouchInfo currentTouchInfo = new TouchInfo();
        public TouchInfo CurrentTouchInfo { get { return currentTouchInfo; } }

        public void SetDraggedObject(GameObject gameObject)
        {
            currentTouchInfo.draggedObject = gameObject;
        }

        private List<Touch> touches;
        private Touch touch;
        private PointerEventData pointerEventData;
        private List<RaycastResult> raycastResults;

        void HandleSwipe(TouchInfo touchInfo)
        {
            if(!debug) { return; }
            Debug.LogFormat("[TouchManager] HandleSwipe: {0}", touchInfo);
        }

        void HandleTap(TouchInfo touchInfo)
        {
            if(!debug) { return; }
            Debug.LogFormat("[TouchManager] HandleTap: {0}", touchInfo);
        }

        void HandleLongTap(TouchInfo touchInfo)
        {
            if(!debug) { return; }
            Debug.LogFormat("[TouchManager] HandleLongPress: {0}", touchInfo);
        }

        void Awake()
        {
            if(_instance != null && _instance != this)
            {
                Debug.Log("[DoozyUI] There cannot be two TouchManagers active at the same time. Destryoing this one!");
                Destroy(gameObject);
                return;
            }

            _instance = this;
            DontDestroyOnLoad(gameObject);

            Init();

            if(EventSystem.current == null)
            {
                new GameObject("EventSystem", typeof(EventSystem), typeof(StandaloneInputModule));
            }
        }

        void Init()
        {
            touches = new List<Touch>();
            raycastResults = new List<RaycastResult>();

            onSwipeAction += HandleSwipe;
            onLongTapAction += HandleLongTap;
            onTapAction += HandleTap;
        }

        void Update()
        {
            DetectTouch();
        }

        public void DetectTouch()
        {
            touches = InputHelper.GetTouches();
            if(touches.Count == 0 || swipeEnded)
            {
                TouchInProgress = false;
                return;
            }

            touch = touches[0];

            if(touch.phase == TouchPhase.Began)
            {
                pointerEventData = new PointerEventData(EventSystem.current)
                {
                    position = touch.position
                };

                raycastResults.Clear();
                EventSystem.current.RaycastAll(pointerEventData, raycastResults);

                currentTouchInfo.Reset(touch, raycastResults.Count > 0 ? raycastResults[0].gameObject : null); //reset

                pointerEventData = null;

                TouchInProgress = true;

                return;
            }

            UpdateCurrentTouchInfo(touch);

            if(touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary)
            {
                if(!currentTouchInfo.longTap && currentTouchInfo.duration > longTapDuration && currentTouchInfo.longestDistance < minSwipeLength) //LONG TAP
                {
                    currentTouchInfo.direction = Swipe.None; //invalidate current swipe action
                    currentTouchInfo.longTap = true;
                    if(onLongTapAction != null)
                    {
                        onLongTapAction(currentTouchInfo); //fire event - LONG TAP
                    }
                    return;
                }
            }

            if(touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
            {
                if(currentTouchInfo.duration < longTapDuration && currentTouchInfo.longestDistance < minSwipeLength) //TAP
                {
                    currentTouchInfo.direction = Swipe.None; //invalidate current swipe action
                    currentTouchInfo.tap = true;
                    if(onTapAction != null)
                    {
                        onTapAction(currentTouchInfo); //fire event - TAP
                    }
                    return;
                }

                if(currentTouchInfo.distance < minSwipeLength || currentTouchInfo.longTap) //didnt swipe enough or this is a long tap
                {
                    currentTouchInfo.direction = Swipe.None; //invalidate current swipe action
                    return;
                }

                //SWIPE
                if(onSwipeAction != null)
                {
                    onSwipeAction(currentTouchInfo); //fire event - SWIPE
                }
            }
        }

        void UpdateCurrentTouchInfo(Touch touch)
        {
            currentTouchInfo.touch = touch;

            currentTouchInfo.touchPreviousPosition = currentTouchInfo.touchCurrentPosition;
            currentTouchInfo.touchCurrentPosition = touch.position;
            currentTouchInfo.touchDeltaTime = touch.deltaTime;

            currentTouchInfo.endPosition = new Vector2(touch.position.x, touch.position.y);
            currentTouchInfo.endTime = Time.time;
            currentTouchInfo.duration = currentTouchInfo.endTime - currentTouchInfo.startTime;
            currentSwipe = currentTouchInfo.endPosition - currentTouchInfo.startPosition;
            currentTouchInfo.rawDirection = currentSwipe;
            currentTouchInfo.direction = currentTouchInfo.longestDistance < minSwipeLength ? Swipe.None : GetSwipeDirection(currentSwipe);
            currentTouchInfo.distance = Vector2.Distance(currentTouchInfo.startPosition, currentTouchInfo.endPosition);
            //currentTouchInfo.velocity = currentSwipe * (currentTouchInfo.endTime - currentTouchInfo.startTime);
            currentTouchInfo.velocity = (currentTouchInfo.endPosition - currentTouchInfo.startPosition) / (currentTouchInfo.endTime - currentTouchInfo.startTime);
            //velocity = (endPos - startPos) / (endTime  - startTime)
            //velocity = distance / duration
            if(currentTouchInfo.distance > currentTouchInfo.longestDistance) // If new distance is longer than previously longest
            {
                currentTouchInfo.longestDistance = currentTouchInfo.distance; // Update longest distance
            }
        }

        Swipe GetSwipeDirection(Vector2 direction)
        {
            var angle = Vector2.Angle(Vector2.up, direction.normalized); // Degrees
            var swipeDirection = Swipe.None;

            if(direction.x > 0) // Right
            {
                if(angle < 22.5f) // 0.0 - 22.5
                {
                    swipeDirection = Swipe.Up;
                }
                else if(angle < 67.5f) // 22.5 - 67.5
                {
                    swipeDirection = useEightDirections ? Swipe.UpRight : Swipe.Right;
                }
                else if(angle < 112.5f) // 67.5 - 112.5
                {
                    swipeDirection = Swipe.Right;
                }
                else if(angle < 157.5f) // 112.5 - 157.5
                {
                    swipeDirection = useEightDirections ? Swipe.DownRight : Swipe.Right;
                }
                else if(angle < 180.0f) // 157.5 - 180.0
                {
                    swipeDirection = Swipe.Down;
                }
            }
            else // Left
            {
                if(angle < 22.5f) // 0.0 - 22.5
                {
                    swipeDirection = Swipe.Up;
                }
                else if(angle < 67.5f) // 22.5 - 67.5
                {
                    swipeDirection = useEightDirections ? Swipe.UpLeft : Swipe.Left;
                }
                else if(angle < 112.5f) // 67.5 - 112.5
                {
                    swipeDirection = Swipe.Left;
                }
                else if(angle < 157.5f) // 112.5 - 157.5
                {
                    swipeDirection = useEightDirections ? Swipe.DownLeft : Swipe.Left;
                }
                else if(angle < 180.0f) // 157.5 - 180.0
                {
                    swipeDirection = Swipe.Down;
                }
            }

            return swipeDirection;
        }

        public static Vector2 GetCardinalDirection(Swipe swipe)
        {
            switch(swipe)
            {
                case Swipe.None: return CardinalDirection.None;
                case Swipe.UpLeft: return CardinalDirection.UpLeft;
                case Swipe.Up: return CardinalDirection.Up;
                case Swipe.UpRight: return CardinalDirection.UpRight;
                case Swipe.Left: return CardinalDirection.Left;
                case Swipe.Right: return CardinalDirection.Right;
                case Swipe.DownLeft: return CardinalDirection.DownLeft;
                case Swipe.Down: return CardinalDirection.Down;
                case Swipe.DownRight: return CardinalDirection.DownRight;
                default: return CardinalDirection.None;
            }
        }

        public static Swipe GetSwipe(SimpleSwipe simpleSwipe, bool reverse = false)
        {
            switch(simpleSwipe)
            {
                case SimpleSwipe.None: return Swipe.None;
                case SimpleSwipe.Left: return reverse ? Swipe.Right : Swipe.Left;
                case SimpleSwipe.Right: return reverse ? Swipe.Left : Swipe.Right;
                case SimpleSwipe.Up: return reverse ? Swipe.Down : Swipe.Up;
                case SimpleSwipe.Down: return reverse ? Swipe.Up : Swipe.Down;
                default: return Swipe.None;
            }
        }

        public static SimpleSwipe GetSimpleSwipe(Swipe swipe, bool reverse = false)
        {
            switch(swipe)
            {
                case Swipe.None: return SimpleSwipe.None;
                case Swipe.UpLeft: return reverse ? SimpleSwipe.Right : SimpleSwipe.Left;
                case Swipe.Up: return reverse ? SimpleSwipe.Down : SimpleSwipe.Up;
                case Swipe.UpRight: return reverse ? SimpleSwipe.Left : SimpleSwipe.Right;
                case Swipe.Left: return reverse ? SimpleSwipe.Right : SimpleSwipe.Left;
                case Swipe.Right: return reverse ? SimpleSwipe.Left : SimpleSwipe.Right;
                case Swipe.DownLeft: return reverse ? SimpleSwipe.Right : SimpleSwipe.Left;
                case Swipe.Down: return reverse ? SimpleSwipe.Up : SimpleSwipe.Down;
                case Swipe.DownRight: return reverse ? SimpleSwipe.Left : SimpleSwipe.Right;
                default: return SimpleSwipe.None;
            }
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            Debug.Log(gameObject.name + ": " + "OnBeginDrag");
        }

        public void OnDrag(PointerEventData eventData)
        {
            Debug.Log(gameObject.name + ": " + "OnDrag");
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            Debug.Log(gameObject.name + ": " + "OnEndDrag");
        }

    }

    public enum Swipe
    {
        None = 0,
        UpLeft = 1,
        Up = 2,
        UpRight = 3,
        Left = 4,
        Right = 5,
        DownLeft = 6,
        Down = 7,
        DownRight = 8
    }

    public enum SimpleSwipe
    {
        None = 0,
        Left = 1,
        Right = 2,
        Up = 3,
        Down = 4
    }

    public enum GestureType
    {
        Tap = 0,
        LongTap = 1,
        Swipe = 2
    }

    public class CardinalDirection
    {
        public static readonly Vector2 None = new Vector2(0, 0);
        public static readonly Vector2 Up = new Vector2(0, 1);
        public static readonly Vector2 Down = new Vector2(0, -1);
        public static readonly Vector2 Right = new Vector2(1, 0);
        public static readonly Vector2 Left = new Vector2(-1, 0);
        public static readonly Vector2 UpRight = new Vector2(1, 1);
        public static readonly Vector2 UpLeft = new Vector2(-1, 1);
        public static readonly Vector2 DownRight = new Vector2(1, -1);
        public static readonly Vector2 DownLeft = new Vector2(-1, -1);
    }

    public struct TouchInfo
    {
        public Touch touch;

        public Swipe direction;
        public Vector2 rawDirection;
        public Vector2 startPosition;
        public Vector2 endPosition;
        public Vector2 velocity;
        public float startTime;
        public float endTime;
        public float duration;
        public bool tap;
        public bool longTap;
        public float distance;
        public float longestDistance;
        public GameObject gameObject;

        public GameObject draggedObject;
        public bool IsDragging { get { return draggedObject != null; } }

        public Vector2 touchCurrentPosition;
        public Vector2 touchPreviousPosition;
        public float touchDeltaTime;
        public Vector2 TouchVelocity { get { return (touchCurrentPosition - touchPreviousPosition); } }

        public void Reset(Touch touch, GameObject gameObject = null)
        {
            this.touch = touch;

            this.direction = Swipe.None;
            this.startPosition = new Vector2(touch.position.x, touch.position.y);
            this.endPosition = this.startPosition;
            this.velocity = Vector2.zero;
            this.startTime = Time.time;
            this.endTime = this.startTime;
            this.duration = 0f;
            this.tap = false;
            this.longTap = false;
            this.distance = 0f;
            this.longestDistance = 0f;
            this.gameObject = gameObject;

            this.draggedObject = null;
        }

        public override string ToString()
        {
            return string.Format("[SwipeAction: {0}, From {1}, To {2}, Delta {3}, Time {4:0.00}s]", direction, rawDirection, startPosition, endPosition, duration);
        }
    }

    public static class InputHelper
    {
        static SimulatedTouch lastSimulatedTouch;

        static List<Touch> touches;

        public static List<Touch> GetTouches()
        {
            if(touches == null)
            {
                touches = new List<Touch>();
            }
            else
            {
                touches.Clear();
            }

            touches.AddRange(Input.touches);

            // Uncomment if you want it only to allow mouse swipes in the Unity Editor
            //#if UNITY_EDITOR
            if(lastSimulatedTouch == null)
            {
                lastSimulatedTouch = new SimulatedTouch();
            }

            if(Input.GetMouseButtonDown(0))
            {
                lastSimulatedTouch.Phase = TouchPhase.Began;
                lastSimulatedTouch.PositionDelta = new Vector2(0, 0);
                lastSimulatedTouch.Position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
                lastSimulatedTouch.FingerId = 0;
            }
            else if(Input.GetMouseButtonUp(0))
            {
                lastSimulatedTouch.Phase = TouchPhase.Ended;
                Vector2 newPosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
                lastSimulatedTouch.PositionDelta = newPosition - lastSimulatedTouch.Position;
                lastSimulatedTouch.Position = newPosition;
                lastSimulatedTouch.FingerId = 0;
            }
            else if(Input.GetMouseButton(0))
            {
                lastSimulatedTouch.Phase = TouchPhase.Moved;
                Vector2 newPosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
                lastSimulatedTouch.PositionDelta = newPosition - lastSimulatedTouch.Position;
                lastSimulatedTouch.Position = newPosition;
                lastSimulatedTouch.FingerId = 0;
            }
            else
            {
                lastSimulatedTouch = null;
            }
            if(lastSimulatedTouch != null)
            {
                touches.Add(lastSimulatedTouch.Create());
            }
            // Uncomment if you want it only to allow mouse swipes in the Unity Editor
            //#endif

            return touches;
        }
    }

    public class SimulatedTouch
    {
        static BindingFlags flags = BindingFlags.Instance | BindingFlags.NonPublic;
        static Dictionary<string, FieldInfo> fields;

        object touch;

        public SimulatedTouch()
        {
            touch = new Touch();
        }

        static SimulatedTouch()
        {
            fields = new Dictionary<string, FieldInfo>();
            foreach(var f in typeof(Touch).GetFields(flags))
            {
                fields.Add(f.Name, f);
                //Debug.Log("name: " + f.Name); // Use this to find the names of hidden private fields
            }
        }

        public Touch Create()
        {
            return (Touch)touch;
        }

        public float TimeDelta { get { return ((Touch)touch).deltaTime; } set { fields["m_TimeDelta"].SetValue(touch, value); } }
        public int TapCount { get { return ((Touch)touch).tapCount; } set { fields["m_TapCount"].SetValue(touch, value); } }
        public TouchPhase Phase { get { return ((Touch)touch).phase; } set { fields["m_Phase"].SetValue(touch, value); } }
        public Vector2 PositionDelta { get { return ((Touch)touch).deltaPosition; } set { fields["m_PositionDelta"].SetValue(touch, value); } }
        public int FingerId { get { return ((Touch)touch).fingerId; } set { fields["m_FingerId"].SetValue(touch, value); } }
        public Vector2 Position { get { return ((Touch)touch).position; } set { fields["m_Position"].SetValue(touch, value); } }
        public Vector2 RawPosition { get { return ((Touch)touch).rawPosition; } set { fields["m_RawPosition"].SetValue(touch, value); } }
    }
}
