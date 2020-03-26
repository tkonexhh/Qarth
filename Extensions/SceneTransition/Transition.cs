/************************
	FileName:/Qarth/Extensions/Transition/Transition.cs
	CreateAuthor:xuhonghua
	CreateTime:3/25/2020 4:30:08 PM
************************/


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Qarth;

namespace GFrame
{
    public class Transition : MonoBehaviour
    {
        private static GameObject canvasObject;

        private void Awake()
        {
            canvasObject = new GameObject("TransitionCanvas");
            var canvas = canvasObject.AddComponent<Canvas>();
            canvasObject.AddComponent<UnityEngine.UI.GraphicRaycaster>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvas.sortingOrder = 2000;//TODO  临时给个大值
            DontDestroyOnLoad(canvasObject);
        }

        public static Transition StartTransition()
        {
            var fade = new GameObject("Transition");
            var trans = fade.AddComponent<Transition>();
            fade.transform.SetParent(canvasObject.transform, false);
            return trans;
        }

        public void EndTransition()
        {
            Destroy(canvasObject);
        }
    }

}