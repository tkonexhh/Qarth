using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using Qarth;

namespace GFrame.Drame
{

    public class DialoguePreviewPlayer : EditorWindow
    {
        public static DialogueBluePrint dramaGraph;
        private DialoguePlayer m_Player;
        private GUIStatus m_CurStatus = GUIStatus.select;



        private string m_Title;
        private string m_Content;
        private Button[] m_Btns;
        private GUIStyle m_TitleStyle = new GUIStyle();
        private GUIStyle m_ContentStyle = new GUIStyle();

        [MenuItem("Tools/GFrame/Drama System/Drama Preview", false, 1)]
        public static void OpenWindow()
        {
            GetWindow<DialoguePreviewPlayer>().Show();
        }

        private void OnEnable()
        {
            this.titleContent = new GUIContent("对话预览");

            m_Player = new DialoguePlayer();
            m_Player.OnDoChoose = (choose) =>
            {
                m_CurStatus = GUIStatus.choose;
                m_Title = "Title";
                m_Content = choose.Content;

                var chooses = choose.Chooses;
                List<Button> buttons = new List<Button>();
                for (int i = 0; i < chooses.Count; i++)
                {
                    int index = i;
                    buttons.Add(new Button()
                    {
                        Title = chooses[i].title,
                        callback = () => { m_Player.NextWithParam(index); }
                    });
                }
                m_Btns = buttons.ToArray();
            };


            m_Player.OnDoContent = (content) =>
            {
                m_CurStatus = GUIStatus.content;
                m_Title = "Title";
                m_Content = content.Content;
            };

            m_Player.OnDoFinish = (finish) =>
            {
                m_Title = "";
                m_Content = "";
                m_CurStatus = GUIStatus.finish;
            };
        }


        private void OnGUI()
        {
            switch (m_CurStatus)
            {
                case GUIStatus.select:
                    Draw_select();
                    break;
                case GUIStatus.content:
                    Draw_Content();
                    break;
                case GUIStatus.choose:
                    Draw_Choose();
                    break;
                case GUIStatus.finish:
                    Draw_Finish();
                    break;
            }
        }

        private string m_SelectPath;

        private void Draw_select()
        {
            GUILayout.Label("输入路径：");
            m_SelectPath = GUILayout.TextField(m_SelectPath);
            if (GUILayout.Button("选择"))
            {
                var path = EditorUtility.OpenFilePanel("选择对话蓝图", System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), "Assets"), "asset");
                var cur_path = System.IO.Directory.GetCurrentDirectory();
                cur_path = cur_path.Replace("\\", "/");
                cur_path = cur_path + "/";
                path = path.Replace(cur_path, "");
                //path = path.Replace("\\", "/");
                m_SelectPath = path;
            }

            GUILayout.Space(10);
            if (!m_SelectPath.IsNullOrEmpty())
            {
                if (GUILayout.Button("播放"))
                {
                    AssetDatabase.Refresh();
                    AssetDatabase.ReleaseCachedFileHandles();

                    var asset = AssetDatabase.LoadAssetAtPath<DialogueBluePrint>(m_SelectPath);
                    if (asset == null)
                    {
                        EditorUtility.DisplayDialog("失败", "载入文件失败", "嗯");
                        return;
                    }
                    m_Player.PlayDialogue(asset);

                }
            }
        }

        private void Draw_Content()
        {
            GUILayout.BeginVertical();

            if (!m_Title.IsNullOrEmpty())
            {
                GUILayout.Label(m_Title, m_TitleStyle);
            }
            if (!m_Content.IsNullOrEmpty())
            {
                GUILayout.Label(m_Content, m_ContentStyle);
            }


            if (GUILayout.Button("下一步"))
            {
                m_Player.Next();
            }

            GUILayout.EndVertical();
        }

        private void Draw_Choose()
        {
            GUILayout.BeginVertical();

            if (!m_Title.IsNullOrEmpty())
            {
                GUILayout.Label(m_Title, m_TitleStyle);
            }
            if (!m_Content.IsNullOrEmpty())
            {
                GUILayout.Label(m_Content, m_ContentStyle);
            }

            //绘制按钮组
            foreach (var item in m_Btns)
            {
                if (GUILayout.Button(item.Title))
                {
                    item.callback();
                }
            }

            GUILayout.EndVertical();
        }

        private void Draw_Finish()
        {
            GUILayout.BeginVertical();

            if (!m_Title.IsNullOrEmpty())
            {
                GUILayout.Label(m_Title, m_TitleStyle);
            }
            if (!m_Content.IsNullOrEmpty())
            {
                GUILayout.Label(m_Content, m_ContentStyle);
            }

            if (GUILayout.Button("结束"))
            {
                m_Player.Next();
            }

            GUILayout.EndVertical();
        }


        private enum GUIStatus
        {
            /// <summary>
            /// 选择播放的蓝图
            /// </summary>
            select,
            /// <summary>
            /// 显示内容对话
            /// </summary>
            content,
            /// <summary>
            /// 显示选项
            /// </summary>
            choose,
            /// <summary>
            /// 播放结束
            /// </summary>
            finish,
        }

        private struct Button
        {
            public string Title;
            public System.Action callback;
        }
    }
}
