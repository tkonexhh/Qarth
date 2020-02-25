using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using System.Reflection;
using System.Reflection.Emit;

namespace Qarth
{

    public class QVariableState
    {
        public bool isVariable = false;
        public bool isAttribute = false;
        public bool isEvent = false;
        public string attributeName = "";


        public int index = 0;
        private int oldIndex = -1;
        public bool isOpen = true;
        private string[] comNames;

        public bool isSelectEvent = true;

        private QVariableState parentState = null;
        public QVariableState ParentState
        {
            set { parentState = value; }
        }
        private List<QVariableState> lstSubState = new List<QVariableState>();
        public List<QVariableState> LstSubState
        {
            get
            {
                return lstSubState;
            }
        }

        public GVariableModel Model
        {
            set;
            get;
        }
        public Action<QVariableState, string> onTypeChanged;
        public Action<QVariableState> onSubStateAdd;
        public Action<QVariableState> onSubStateDel;

        public void SetIndex(string name, Transform t)
        {
            comNames = GGlobalFun.GetComponentsName(t);
            int count = comNames.Length;
            oldIndex = -1;
            for (int i = 0; i < count; i++)
            {
                if (name == comNames[i])
                {
                    index = i;
                    break;
                }
            }
        }

        public void SetName(string name)
        {
            attributeName = name;
        }

        public void Reset()
        {
            comNames = null;
            lstSubState.Clear();
        }

        public bool Update(Transform t, int depth)
        {
            var rect = EditorGUILayout.BeginHorizontal();
            {
                if (isVariable) EditorGUI.DrawRect(rect, new Color(0, 0.5f, 0, 0.3f));

                if (parentState != null)
                {
                    EditorGUILayout.LabelField("----", GConfigureDefine.toggleMaxWidth);
                    if (GUILayout.Button("-", GConfigureDefine.plusMaxWidth))
                    {
                        parentState.lstSubState.Remove(this);
                        //删去一个sub
                        //parentState.lsts
                        parentState.onSubStateDel(this);
                    }
                }

                isVariable = EditorGUILayout.ToggleLeft("变量", isVariable, GConfigureDefine.toggleMaxWidth);

                if (!isVariable)
                {
                    isAttribute = false;
                    isEvent = false;
                    lstSubState.Clear();
                    // if (parentState != null)
                    //     parentState.onSubStateChanged();
                }

                {
                    GUI.enabled = isVariable;
                    isAttribute = EditorGUILayout.ToggleLeft("属性器", isAttribute, GConfigureDefine.toggleMaxWidth);

                    GUI.enabled = !isVariable ? false : isSelectEvent;
                    isEvent = EditorGUILayout.ToggleLeft("事件", isEvent, GConfigureDefine.toggleMaxWidth);

                    GUI.enabled = true;
                    if (parentState == null)
                    {
                        if (isVariable && GUILayout.Button("+", GConfigureDefine.plusMaxWidth))
                        {
                            var subState = new QVariableState();
                            subState.comNames = comNames;
                            subState.isVariable = true;
                            subState.parentState = this;
                            lstSubState.Add(subState);
                            onSubStateAdd(subState);
                            //添加一个sub
                        }
                    }
                }

                attributeName = EditorGUILayout.TextField(attributeName, GConfigureDefine.attriNameMaxWidth);

                oldIndex = index;
                comNames = GGlobalFun.GetComponentsName(t);

                index = EditorGUILayout.Popup(index, comNames, GConfigureDefine.popupMaxWidth);

                if (oldIndex != index)
                {
                    onTypeChanged(this, comNames[index]);
                }

                GUILayout.Space(depth * GConfigureDefine.space);

                if (t.childCount > 0)
                {
                    isOpen = EditorGUILayout.Foldout(isOpen, t.name, true);
                }
                else
                {
                    EditorGUILayout.LabelField(t.name);
                }
            }
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.Space();

            for (int i = 0; i < lstSubState.Count; i++)
            {
                EditorGUILayout.BeginVertical();
                lstSubState[i].Update(t, depth);
                EditorGUILayout.EndVertical();
            }


            return isOpen;
        }
    }
}