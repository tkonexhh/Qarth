using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using UnityEngine.EventSystems;
using System.Text;

namespace Qarth
{
    // public class GVariableModelAttribute
    // {
    //     public string name;
    //     public string type;
    //     public string variableEvent;
    //     public string attributeName;
    //     public string eventName;
    //     public string eventType;
    //     public bool isUI = false;
    // }

    public class GVariableModel
    {
        private static Dictionary<string, Transform> nameDic = new Dictionary<string, Transform>();

        private Transform target;
        public string path;
        //public GVariableModelAttribute mainStateModel;
        public string name;
        public string type;
        public string variableEvent;
        public string attributeName;
        public string eventName;
        public string eventType;
        public bool isUI = false;


        public QVariableState state = new QVariableState();
        private List<GVariableModel> lstSubModel = new List<GVariableModel>();
        public List<GVariableModel> LstSubModel
        {
            get { return lstSubModel; }
        }

        private string[] parameters = new string[]
        {
        "Vector2","string","int","float","float","bool","","","",""
        };

        public GVariableModel(Transform t)
        {
            target = t;
            path = GGlobalFun.GetGameObjectPath(t, Selection.activeTransform);
            type = GetUIType();
            name = GetName(string.Format("{0}_{1}", type, t.name), t);

            state.SetIndex(type, t);
            Init(state, this);
        }

        private void Init(QVariableState state, GVariableModel model)
        {
            state.Model = model;
            OnTypeChanged(state, type);
            state.onTypeChanged = OnTypeChanged;
            state.onSubStateAdd = OnSubStateAdd;
            state.onSubStateDel = OnSubStateDel;
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
            lstSubModel.Clear();
        }

        private void OnTypeChanged(QVariableState state, string value)
        {
            state.Model.type = value;
            state.Model.name = GetName(string.Format("{0}{1}", GConfigure.GetShortTypeName(type), GConfigure.RemoveFrontTypeName(target.name)), target);
            state.SetName(state.Model.name);
            state.Model.isUI = false;
            state.isSelectEvent = false;
            if (!IsUIType(state.Model.type))
            {
                state.Model.eventType =
                state.Model.variableEvent =
                state.Model.attributeName =
                state.Model.eventName = string.Empty;
                return;
            }
            var uiType = (UIType)Enum.Parse(typeof(UIType), type);
            state.Model.variableEvent = string.Empty;
            switch (uiType)
            {
                case UIType.Button:
                    state.Model.variableEvent = "onClick";
                    break;
                case UIType.InputField:
                    state.Model.variableEvent = "onEndEdit";
                    break;
                case UIType.ScrollRect:
                case UIType.Dropdown:
                case UIType.Scrollbar:
                case UIType.Slider:
                case UIType.Toggle:
                    state.Model.variableEvent = "onValueChanged";
                    break;
            }

            if (state.Model.variableEvent != string.Empty)
            {
                state.isSelectEvent = true;
                state.Model.eventType = parameters[(int)uiType];
                state.Model.attributeName = state.Model.variableEvent.Insert(state.Model.variableEvent.Length, state.Model.name);
                state.Model.eventName = GGlobalFun.GetFirstUpper(state.Model.attributeName);
            }
        }

        private void OnSubStateAdd(QVariableState state)
        {
            GVariableModel model = new GVariableModel(target);
            model.state = state;
            Init(model.state, model);
            lstSubModel.Add(model);
        }

        private void OnSubStateDel(QVariableState state)
        {
            lstSubModel.Remove(state.Model);
        }

        private string GetName(string value, Transform tr)
        {
            var str = GGlobalFun.GetString(value);
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