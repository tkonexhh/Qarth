using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using UnityEngine.EventSystems;
using System.Text;

namespace Qarth
{


    public class GVariableModel
    {
        private static Dictionary<string, Transform> nameDic = new Dictionary<string, Transform>();

        public Transform target;
        public string name;
        public string type;
        public string path;
        public string variableEvent;
        public string attributeName;
        public string eventName;
        public string eventType;
        public bool isUI = false;

        public QVariableState state = new QVariableState();

        private string[] parameters = new string[]
        {
        "Vector2","string","int","float","float","bool","","","",""
        };

        public GVariableModel(Transform t)
        {
            target = t;
            path = QGlobalFun.GetGameObjectPath(t, Selection.activeTransform);
            type = GetUIType();
            name = GetName(string.Format("{0}_{1}", type, t.name), t);
            state.SetIndex(type, t);
            OnTypeChanged(type);
            state.onTypeChanged = OnTypeChanged;
        }

        public void Reset()
        {
            name =
            path =
            type =
            eventType =
            variableEvent =
            attributeName =
            eventName = string.Empty;
            state.Reset();
        }

        private void OnTypeChanged(string value)
        {
            type = value;
            name = GetName(string.Format("{0}{1}", GConfigure.GetShortTypeName(type), target.name), target);
            // name = GetName(string.Format("{0}{1}", type, target.name), target);
            isUI = false;
            state.isSelectEvent = false;
            if (!IsUIType(type))
            {
                eventType =
                variableEvent =
                attributeName =
                eventName = string.Empty;
                return;
            }
            var uiType = (UIType)Enum.Parse(typeof(UIType), type);
            variableEvent = string.Empty;
            switch (uiType)
            {
                case UIType.Button:
                    variableEvent = "onClick";
                    break;
                case UIType.InputField:
                    variableEvent = "onEndEdit";
                    break;
                case UIType.ScrollRect:
                case UIType.Dropdown:
                case UIType.Scrollbar:
                case UIType.Slider:
                case UIType.Toggle:
                    variableEvent = "onValueChanged";
                    break;
            }

            if (variableEvent != string.Empty)
            {
                state.isSelectEvent = true;
                eventType = parameters[(int)uiType];
                attributeName = variableEvent.Insert(variableEvent.Length, name);
                eventName = QGlobalFun.GetFirstUpper(attributeName);
            }
        }

        private string GetName(string value, Transform tr)
        {
            var str = QGlobalFun.GetString(value);
            string name = str;
            int i = 1;
            while (true)
            {
                if (nameDic.ContainsKey(name))
                {
                    if (nameDic[name] == tr) break;
                    name = string.Format("{0}{1}", str, i++);
                }
                else
                {
                    break;
                }
            }
            nameDic[name] = tr;

            return name;
        }



        private string GetUIType()
        {
            var coms = target.GetComponents<UIBehaviour>();
            foreach (var v in Enum.GetNames(typeof(UIType)))
            {
                foreach (var com in coms)
                {
                    if (v == com.GetType().Name)
                    {
                        isUI = true;
                        return v;
                    }
                }
            }
            return "Transform";
        }


        public bool IsUIType(string type)
        {
            isUI = false;
            foreach (var v in Enum.GetNames(typeof(UIType)))
            {
                if (v == type)
                {
                    isUI = true;
                    break;
                }
            }

            return isUI;
        }

        public bool IsButton()
        {
            return type == "Button";
        }

        public enum UIType
        {
            ScrollRect,
            InputField,
            Dropdown,
            Scrollbar,
            Slider,
            Toggle,
            Button,
            RawImage,
            Image,
            Text,
        }
    }
}