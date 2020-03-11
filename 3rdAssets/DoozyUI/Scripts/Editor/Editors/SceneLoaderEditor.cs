// Copyright (c) 2015 - 2018 Doozy Entertainment / Marlink Trading SRL. All Rights Reserved.
// This code can only be used under the standard Unity Asset Store End User License Agreement
// A Copy of the EULA APPENDIX 1 is available at http://unity3d.com/company/legal/as_terms

using QuickEditor;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.AnimatedValues;
using UnityEngine;

namespace DoozyUI
{
    [CustomEditor(typeof(SceneLoader), true)]
    [DisallowMultipleComponent]
    public class SceneLoaderEditor : QEditor
    {
        SceneLoader sceneLoader { get { return (SceneLoader)target; } }

        Color AccentColorPurple { get { return QUI.IsProSkin ? QColors.Purple.Color : QColors.PurpleLight.Color; } }

        SerializedProperty
            command_LoadSceneAsync_SceneName,
            command_LoadSceneAsync_SceneBuildIndex,
            command_LoadSceneAdditiveAsync_SceneName,
            command_LoadSceneAdditiveAsync_SceneBuildIndex,
            command_UnloadScene_SceneName,
            command_UnloadScene_SceneBuildIndex,
            command_UnloadLevel,
            command_LoadLevel,
            levelSceneName,
            levelLoadedGameEvent;
#pragma warning disable 0414
        AnimBool showEnergyBars;
#pragma warning restore 0414

        float GlobalWidth { get { return DUI.GLOBAL_EDITOR_WIDTH; } }
        int BarHeight { get { return DUI.BAR_HEIGHT; } }
        int MiniBarHeight { get { return DUI.MINI_BAR_HEIGHT; } }

        float tempFloat;

        protected override void SerializedObjectFindProperties()
        {
            base.SerializedObjectFindProperties();

            command_LoadSceneAsync_SceneName = serializedObject.FindProperty("command_LoadSceneAsync_SceneName");
            command_LoadSceneAsync_SceneBuildIndex = serializedObject.FindProperty("command_LoadSceneAsync_SceneBuildIndex");
            command_LoadSceneAdditiveAsync_SceneName = serializedObject.FindProperty("command_LoadSceneAdditiveAsync_SceneName");
            command_LoadSceneAdditiveAsync_SceneBuildIndex = serializedObject.FindProperty("command_LoadSceneAdditiveAsync_SceneBuildIndex");
            command_UnloadScene_SceneName = serializedObject.FindProperty("command_UnloadScene_SceneName");
            command_UnloadScene_SceneBuildIndex = serializedObject.FindProperty("command_UnloadScene_SceneBuildIndex");
            command_UnloadLevel = serializedObject.FindProperty("command_UnloadLevel");
            command_LoadLevel = serializedObject.FindProperty("command_LoadLevel");
            levelSceneName = serializedObject.FindProperty("levelSceneName");
            levelLoadedGameEvent = serializedObject.FindProperty("levelLoadedGameEvent");
        }

        protected override void InitAnimBools()
        {
            base.InitAnimBools();

            showEnergyBars = new AnimBool(true, Repaint);
        }

        protected override void GenerateInfoMessages()
        {
            base.GenerateInfoMessages();

            infoMessage.Add("ComponentInfo",
                            new InfoMessage()
                            {
                                title = "",
                                message = "Here you can customize the game event commands that trigger different methods for scene loading.",
                                type = InfoMessageType.Help,
                                show = new AnimBool(true, Repaint)
                            });
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            requiresContantRepaint = true;
        }

        public override void OnInspectorGUI()
        {
            DrawHeader(DUIResources.headerSceneLoader.texture, WIDTH_420, HEIGHT_42);
            serializedObject.Update();

            DrawEnergyBars(GlobalWidth);

            DrawCustomGameEvents(GlobalWidth);

            serializedObject.ApplyModifiedProperties();

            QUI.Space(SPACE_4);
        }

        void DrawEnergyBars(float width)
        {
#if dUI_EnergyBarToolkit
            QUI.Space(SPACE_2);

            tempFloat = (20 + 2 + 18 * (sceneLoader.energyBars.Count + 1) + 2) * showEnergyBars.faded; //background height
            if(showEnergyBars.faded > 0.1f)
            {
                QUI.BeginHorizontal(width);
                {
                    QUI.Space(4 * showEnergyBars.faded);
                    QUI.Box(QStyles.GetBackgroundStyle(Style.BackgroundType.Low, sceneLoader.energyBars.Count > 0 ? QColors.Color.Purple : QColors.Color.Gray), width - 4, tempFloat);
                }
                QUI.EndHorizontal();
                QUI.Space(-tempFloat);
            }

            if(QUI.SlicedBar("Energy Bars", sceneLoader.energyBars.Count > 0 ? QColors.Color.Purple : QColors.Color.Gray, showEnergyBars, width, BarHeight))
            {
                showEnergyBars.target = !showEnergyBars.target;
            }

            QUI.BeginHorizontal(width);
            {
                QUI.Space(8 * showEnergyBars.faded);
                if(QUI.BeginFadeGroup(showEnergyBars.faded))
                {
                    QUI.BeginVertical(width - 8);
                    {

                        QUI.Space(2);

                        if(sceneLoader.energyBars.Count == 0)
                        {
                            QUI.BeginHorizontal(width - 8);
                            {
                                QLabel.text = "No Energy Bars referenced... Click [+] to start...";
                                QLabel.style = Style.Text.Help;
                                QUI.BeginVertical(QLabel.x, QUI.SingleLineHeight);
                                {
                                    QUI.Label(QLabel);
                                    QUI.Space(2);
                                }
                                QUI.EndVertical();

                                QUI.FlexibleSpace();

                                QUI.BeginVertical(16, QUI.SingleLineHeight);
                                {
                                    if(QUI.ButtonPlus())
                                    {
                                        Undo.RecordObject(sceneLoader, "Added Energy Bar");
                                        sceneLoader.energyBars = new List<EnergyBar> { null };
                                    }
                                    QUI.Space(1);
                                }
                                QUI.EndVertical();

                                QUI.Space(4);
                            }
                            QUI.EndHorizontal();
                        }
                        else
                        {
                            QUI.BeginVertical(width - 8);
                            {
                                QLabel.style = Style.Text.Help;
                                for(int i = 0; i < sceneLoader.energyBars.Count; i++)
                                {
                                    QUI.BeginHorizontal(width - 8, QUI.SingleLineHeight);
                                    {
                                        QLabel.text = i.ToString();
                                        QUI.Label(QLabel);

                                        QUI.Space(2);

                                        sceneLoader.energyBars[i] = (EnergyBar)QUI.ObjectField(sceneLoader.energyBars[i], typeof(EnergyBar), true, width - QLabel.x - 2 - 16 - 12 - 8);

                                        if(QUI.ButtonMinus())
                                        {
                                            Undo.RecordObject(sceneLoader, "Removed Energy Bar");
                                            sceneLoader.energyBars.RemoveAt(i);
                                        }

                                        QUI.Space(8);
                                    }
                                    QUI.EndHorizontal();
                                }

                                QUI.BeginHorizontal(width - 8);
                                {
                                    QUI.FlexibleSpace();

                                    QUI.BeginVertical(16, QUI.SingleLineHeight);
                                    {
                                        if(QUI.ButtonPlus())
                                        {
                                            Undo.RecordObject(sceneLoader, "Added Energy Bar");
                                            sceneLoader.energyBars.Add(null);
                                        }
                                        QUI.Space(1);
                                    }
                                    QUI.EndVertical();

                                    QUI.Space(4);
                                }
                                QUI.EndHorizontal();
                            }
                            QUI.EndVertical();

                            QUI.Space(2);

                            QUI.Space(4 * showEnergyBars.faded);
                        }
                    }
                    QUI.EndVertical();
                }
                QUI.EndFadeGroup();

            }
            QUI.EndHorizontal();

            QUI.Space(SPACE_8);
#endif
        }

        void DrawCustomGameEvents(float width)
        {
            DrawPair("LoadLevel Async",
                     "by name", command_LoadSceneAsync_SceneName, SceneLoader.DEFAULT_LOAD_SCENE_ASYNC_SCENE_NAME,
                     "by build index", command_LoadSceneAsync_SceneBuildIndex, SceneLoader.DEFAULT_LOAD_SCENE_ASYNC_SCENE_BUILD_INDEX,
                     width);

            QUI.Space(SPACE_8);

            DrawPair("LoadLevel Additive Async",
                     "by name", command_LoadSceneAdditiveAsync_SceneName, SceneLoader.DEFAULT_LOAD_SCENE_ADDITIVE_ASYNC_SCENE_NAME,
                     "by build index", command_LoadSceneAdditiveAsync_SceneBuildIndex, SceneLoader.DEFAULT_LOAD_SCENE_ADDITIVE_ASYNC_SCENE_BUILD_INDEX,
                     width);

            QUI.Space(SPACE_8);

            DrawPair("Unload Scene",
                     "by name", command_UnloadScene_SceneName, SceneLoader.DEFAULT_UNLOAD_SCENE_SCENE_NAME,
                     "by build index", command_UnloadScene_SceneBuildIndex, SceneLoader.DEFAULT_UNLOAD_SCENE_SCENE_BUILD_INDEX,
                     width);

            QUI.Space(SPACE_8);

            DrawPair("Load / Unload Level",
                     "load level", command_LoadLevel, SceneLoader.DEFAULT_LOAD_LEVEL,
                     "unload level", command_UnloadLevel, SceneLoader.DEFAULT_UNLOAD_LEVEL,
                     width);

            QUI.Space(SPACE_8);

            DrawPair("Level Scene Name (naming convention)",
                     "scene name", levelSceneName, SceneLoader.DEFAULT_LEVEL_SCENE_NAME,
                     "", null, "",
                     width,
                     true);

            QUI.Space(SPACE_8);

            DrawPair("Game Event sent after a scene was loaded",
                     "game event", levelLoadedGameEvent, SceneLoader.DEFAULT_LEVEL_LOADED,
                     "", null, "",
                     width,
                     true);

            QUI.Space(SPACE_8);
        }

        void DrawPair(string title,
                      string firstLabel, SerializedProperty firstProperty, string firstDefaultValue,
                      string secondLabel, SerializedProperty secondProperty, string secondDefaultValue,
                      float width, bool isSingle = false)
        {
            QUI.Box(QStyles.GetBackgroundStyle(Style.BackgroundType.Low, QColors.Color.Purple), width, 18);
            QUI.Space(-18);

            QUI.Box(QStyles.GetBackgroundStyle(Style.BackgroundType.Low, QColors.Color.Purple), width, isSingle ? 42 : 60);
            QUI.Space(isSingle ? -44 : -62);

            QLabel.text = title;
            QLabel.style = Style.Text.Normal;
            QUI.BeginHorizontal(width);
            {
                QUI.Space(6);
                QUI.Label(QLabel);
                QUI.FlexibleSpace();
            }
            QUI.EndHorizontal();

            QUI.Space(SPACE_2);

            QUI.BeginHorizontal(width);
            {
                QUI.Space(12);
                QLabel.text = firstLabel;
                QLabel.style = Style.Text.Small;
                QUI.Label(QLabel);
                QUI.PropertyField(firstProperty);
                if(QUI.ButtonReset())
                {
                    firstProperty.stringValue = firstDefaultValue;
                }
                QUI.Space(6);
            }
            QUI.EndHorizontal();

            if(isSingle)
            {
                return;
            }

            QUI.BeginHorizontal(width);
            {
                QUI.Space(12);
                QLabel.text = secondLabel;
                QLabel.style = Style.Text.Small;
                QUI.Label(QLabel);
                QUI.PropertyField(secondProperty);
                if(QUI.ButtonReset())
                {
                    secondProperty.stringValue = secondDefaultValue;
                }
                QUI.Space(6);
            }
            QUI.EndHorizontal();
        }
    }
}
