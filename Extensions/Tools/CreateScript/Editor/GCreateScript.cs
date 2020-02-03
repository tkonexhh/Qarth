using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Qarth
{
    public class GCreateScript : EditorWindow
    {
        private static GCreateScript m_Window;

        private GManagerVariable m_Manager = new GManagerVariable();

        [MenuItem("Tools/GFrame/CreateUIScript")]
        private static void ShowWindow()
        {
            m_Window = GetWindow<GCreateScript>();
            m_Window.Show();
        }

        private void Awake()
        {
            m_Manager = new GManagerVariable();
            if (GConfigure.selectTransform != null)
                m_Manager.Init();
        }

        [InitializeOnLoadMethod]
        private static void App()
        {
            Selection.selectionChanged += () =>
            {
                if (m_Window != null)
                {
                    m_Window.m_Manager.Clear();
                    m_Window.Repaint();
                }
            };
        }

        private void OnGUI()
        {
            if (GConfigure.selectTransform != Selection.activeTransform)
            {
                if (GConfigure.selectTransform != null)
                {
                    m_Manager.Clear();
                }
                GConfigure.selectTransform = Selection.activeTransform;
                if (GConfigure.selectTransform != null)
                {
                    m_Manager.Init();
                }
                if (Selection.gameObjects.Length == 1)
                {
                    GConfigure.selectTransform = Selection.gameObjects[0].transform;
                    if (GConfigure.selectTransform != null)
                    {
                        m_Manager.Init();
                    }
                }
            }

            EditorGUILayout.BeginHorizontal();
            {
                DrawLeft();
                DrawRight();
            }
            EditorGUILayout.EndHorizontal();

            if (!EditorApplication.isCompiling)
            {
                if (GConfigure.IsCompiling())
                {
                    m_Manager.Init();
                }
            }
        }


        private void DrawCreatePath()
        {
            GConfigure.referencePath = EditorPrefs.GetString("文件生成路径", GConfigureDefine.referencedefaultPath);
            GConfigure.referencePath = EditorGUILayout.TextField("脚本生成路径：", GConfigure.referencePath);
            EditorPrefs.SetString("文件生成路径", GConfigure.referencePath);

            GConfigure.prefabSavePath = EditorPrefs.GetString("prefab保存路径", GConfigureDefine.prefabdefaultPath);
            GConfigure.prefabSavePath = EditorGUILayout.TextField("预制体生成路径：", GConfigure.prefabSavePath);
            EditorPrefs.SetString("prefab保存路径", GConfigure.prefabSavePath);
        }

        private void DrawLeft()
        {
            EditorGUILayout.BeginVertical();
            {
                DrawCreatePath();
                EditorGUILayout.Space();
                GConfigure.Version = (ScriptVersion)EditorGUILayout.Popup("类型", (int)GConfigure.Version, GConfigure.versionStr);
                EditorGUILayout.Space();
                // EditorGUILayout.BeginHorizontal();
                // {
                //     if (GConfigure.Version == ScriptVersion.Mono)
                //     {
                //         GConfigure.isCreateModel = EditorPrefs.GetBool("是否生成Model", true);
                //         GConfigure.isCreateModel = EditorGUILayout.ToggleLeft("是否生成Model", GConfigure.isCreateModel, GUILayout.MaxWidth(100));
                //         EditorPrefs.SetBool("是否生成Model", GConfigure.isCreateModel);
                //     }

                //     GConfigure.isCreateController = EditorPrefs.GetBool("是否生成Controller", true);
                //     GConfigure.isCreateController = EditorGUILayout.ToggleLeft("是否生成Controller", GConfigure.isCreateController, GUILayout.MaxWidth(120));
                //     EditorPrefs.SetBool("是否生成Controller", GConfigure.isCreateController);

                // }
                // EditorGUILayout.EndHorizontal();
                EditorGUILayout.Space();
                EditorGUILayout.BeginHorizontal();
                {

                    if (GUILayout.Button("生成脚本")) m_Manager.CreateFile();
                    if (GUILayout.Button("更新脚本")) m_Manager.UpdateFile();
                }
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();
                {
                    if (GUILayout.Button("复制代码")) m_Manager.Copy();
                    if (GUILayout.Button("挂载脚本")) m_Manager.MountScript();
                    if (GUILayout.Button("绑定UI")) m_Manager.BindingUI();
                }
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();
                {
                    if (GUILayout.Button("生成/更新预制体")) m_Manager.CreatePrefab();
                }
                EditorGUILayout.EndHorizontal();

                DrawTable();
            }
            EditorGUILayout.EndVertical();
        }

        private Vector2 pos;
        private void DrawRight()
        {
            EditorGUILayout.BeginVertical();
            {
                var rect = EditorGUILayout.GetControlRect();
                rect.height = 35;
                GUI.Box(rect, "代码预览", "GroupBox");
                GUILayout.Space(20);

                {
                    pos = EditorGUILayout.BeginScrollView(pos, GUILayout.Width(position.width * 0.5f));
                    {
                        if (GConfigure.selectTransform != null)
                        {
                            var str = m_Manager.ToString();
                            var array = GGlobalFun.GetStringList(str);
                            EditorGUILayout.BeginVertical();
                            {
                                foreach (var item in array)
                                {
                                    GUILayout.Label(item);
                                }
                            }
                            EditorGUILayout.EndVertical();

                        }
                    }
                    EditorGUILayout.EndScrollView();
                }
            }
            EditorGUILayout.EndVertical();
        }

        private Vector2 tablePos;
        private void DrawTable()
        {
            EditorGUILayout.Space();
            EditorGUILayout.BeginHorizontal();
            {
                if (GUILayout.Button("全选变量")) m_Manager.TotalSelectVariable();
                if (GUILayout.Button("全选属性器")) m_Manager.TotalAttribute();
                if (GUILayout.Button("全选事件")) m_Manager.TotalEvent();
                if (GUILayout.Button("全折叠")) m_Manager.TotalFold(false);
                if (GUILayout.Button("查找赋值")) m_Manager.isFind = true;
            }
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            {
                if (GUILayout.Button("全取消变量")) m_Manager.TotalSelectVariable(false);
                if (GUILayout.Button("全取消属性器")) m_Manager.TotalAttribute(false);
                if (GUILayout.Button("全取消事件")) m_Manager.TotalEvent(false);
                if (GUILayout.Button("全展开")) m_Manager.TotalFold();
                if (GUILayout.Button("取消查找")) m_Manager.isFind = false;
            }
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.Space();
            tablePos = EditorGUILayout.BeginScrollView(tablePos);
            {
                if (GConfigure.selectTransform != null)
                {
                    DrawRow(GConfigure.selectTransform);
                }
            }
            EditorGUILayout.EndScrollView();
        }

        private void DrawRow(Transform tr, int depth = 0)
        {
            foreach (Transform t in tr)
            {
                if (m_Manager[t].state.Update(t, depth) && t.childCount > 0)
                {
                    DrawRow(t, depth + 1);
                }
            }
        }
    }
}