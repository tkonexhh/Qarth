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
    [CustomEditor(typeof(UIEffect), true)]
    [CanEditMultipleObjects]
    public class UIEffectEditor : QEditor
    {
        private const string MISSING_UIELEMENT = " --- ";

        UIEffect uiEffect { get { return (UIEffect)target; } }

        DUISettings EditorSettings { get { return DUISettings.Instance; } }

        SerializedProperty
            targetParticleSystem, targetUIElement,
            startDelay, playOnAwake, stopInstantly,
            useCustomSortingLayerName, customSortingLayerName,
            useCustomOrderInLayer, customOrderInLayer,
            effectPosition, sortingOrderStep;

        Canvas m_targetCanvas;
        Canvas TargetCanvas
        {
            get
            {
                if(m_targetCanvas == null)
                {
                    m_targetCanvas = uiEffect.targetUIElement.GetComponent<Canvas>() == null
                                     ? uiEffect.targetUIElement.gameObject.AddComponent<Canvas>()
                                     : uiEffect.targetUIElement.GetComponent<Canvas>();
                }
                return m_targetCanvas;
            }
        }

        string targetSortingLayerName;
        int targetOrderInLayer;

        AnimBool useCustomOrderInLayerAnimBool;

        float GlobalWidth { get { return DUI.GLOBAL_EDITOR_WIDTH; } }
        int BarHeight { get { return DUI.BAR_HEIGHT; } }
        int MiniBarHeight { get { return DUI.MINI_BAR_HEIGHT; } }

        float tempFloat = 0;

        protected override void SerializedObjectFindProperties()
        {
            base.SerializedObjectFindProperties();

            targetParticleSystem = serializedObject.FindProperty("targetParticleSystem");
            targetUIElement = serializedObject.FindProperty("targetUIElement");
            startDelay = serializedObject.FindProperty("startDelay");
            playOnAwake = serializedObject.FindProperty("playOnAwake");
            stopInstantly = serializedObject.FindProperty("stopInstantly");
            useCustomSortingLayerName = serializedObject.FindProperty("useCustomSortingLayerName");
            customSortingLayerName = serializedObject.FindProperty("customSortingLayerName");
            useCustomOrderInLayer = serializedObject.FindProperty("useCustomOrderInLayer");
            customOrderInLayer = serializedObject.FindProperty("customOrderInLayer");
            effectPosition = serializedObject.FindProperty("effectPosition");
            sortingOrderStep = serializedObject.FindProperty("sortingOrderStep");
        }

        protected override void InitAnimBools()
        {
            base.InitAnimBools();

            useCustomOrderInLayerAnimBool = new AnimBool(!useCustomOrderInLayer.boolValue, Repaint);
        }

        protected override void GenerateInfoMessages()
        {
            base.GenerateInfoMessages();

            infoMessage.Add("ParticleSystemDisabled",
                            new InfoMessage()
                            {
                                title = "Missing ParticleSystem",
                                message = "Add a ParticleSystem to this gameObject or set the target ParticleSystem manually.",
                                type = InfoMessageType.Error,
                                show = new AnimBool(false, Repaint)
                            });

            infoMessage.Add("UIElementDisabled",
                            new InfoMessage()
                            {
                                title = "Missing UIElement",
                                message = "Set the target UIElement that manages this UIEffect.",
                                type = InfoMessageType.Error,
                                show = new AnimBool(false, Repaint)
                            });
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            requiresContantRepaint = true;

            if(uiEffect.targetParticleSystem == null)
            {
                uiEffect.targetParticleSystem = uiEffect.GetComponent<ParticleSystem>();
            }
        }

        public override void OnInspectorGUI()
        {
            DrawHeader(DUIResources.headerUIEffect.texture, WIDTH_420, HEIGHT_42);
            serializedObject.Update();

            if(!EditorSettings.UIEffect_Inspector_ShowButtonRenameGameObject
               || (uiEffect.targetUIElement != null && uiEffect.targetUIElement.linkedToNotification))
            {
            }
            else
            {
                DrawRenameGameObjectButton(GlobalWidth);
                QUI.Space(SPACE_4);
            }

            DrawAddParticleSystemButton(GlobalWidth);
            QUI.Space(SPACE_4);

            DrawTargetParticleSystem(GlobalWidth);
            QUI.Space(SPACE_4);

            DrawTargetUIElement(GlobalWidth);
            QUI.Space(SPACE_2);

            if(uiEffect.targetParticleSystem != null && uiEffect.targetUIElement != null)
            {
                DrawSortingLayerName(GlobalWidth);
                DrawSortingOrder(GlobalWidth);
                QUI.Space(SPACE_4);
                DrawSettings(GlobalWidth);
            }

            serializedObject.ApplyModifiedProperties();

            QUI.Space(SPACE_4);
        }

        void DrawRenameGameObjectButton(float width)
        {
            QUI.BeginHorizontal(width);
            {
                if(QUI.GhostButton("Rename GameObject", QColors.Color.Gray, width, 18))
                {
                    if(serializedObject.isEditingMultipleObjects)
                    {
                        Undo.RecordObjects(targets, "Rename");
                        for(int i = 0; i < targets.Length; i++)
                        {
                            UIEffect iTarget = (UIEffect)targets[i];
                            iTarget.gameObject.name = iTarget.targetUIElement != null
                                                      ? (DUI.DUISettings.UIEffect_Inspector_RenameGameObjectPrefix + iTarget.targetUIElement.elementName + DUI.DUISettings.UIEffect_Inspector_RenameGameObjectSuffix)
                                                      : "UIEffect DISABLED";
                        }
                    }
                    else
                    {
                        RenameGameObject();
                    }
                }
            }
            QUI.EndHorizontal();
        }
        void RenameGameObject()
        {
            Undo.RecordObject(target, "Rename");
            uiEffect.gameObject.name = uiEffect.targetUIElement != null
                          ? (DUI.DUISettings.UIEffect_Inspector_RenameGameObjectPrefix + uiEffect.targetUIElement.elementName + DUI.DUISettings.UIEffect_Inspector_RenameGameObjectSuffix)
                          : "UIEffect DISABLED";
        }

        void DrawAddParticleSystemButton(float width)
        {
            if(uiEffect.targetParticleSystem != null)
            {
                return;
            }

            QUI.BeginHorizontal(width);
            {
                if(QUI.GhostButton("Add a ParticleSystem to this gameObject", QColors.Color.Gray, width, 18))
                {
                    targetParticleSystem.objectReferenceValue = uiEffect.GetComponent<ParticleSystem>() == null ? uiEffect.gameObject.AddComponent<ParticleSystem>() : uiEffect.GetComponent<ParticleSystem>();
                    serializedObject.ApplyModifiedProperties();
                    uiEffect.targetParticleSystem.GetComponent<ParticleSystemRenderer>().material = AssetDatabase.GetBuiltinExtraResource<Material>("Default-Particle.mat");
                }
            }
            QUI.EndHorizontal();
        }

        void DrawTargetParticleSystem(float width)
        {
            QUI.Box(QStyles.GetBackgroundStyle(Style.BackgroundType.Low, uiEffect.targetParticleSystem == null ? QColors.Color.Red : QColors.Color.Blue), width, 20);
            QUI.Space(-20);

            QLabel.text = "Target ParticleSystem";
            QLabel.style = Style.Text.Normal;

            QUI.BeginHorizontal(width);
            {
                QUI.Space(SPACE_4);
                QUI.BeginVertical(QLabel.x, QUI.SingleLineHeight);
                {
                    QUI.Label(QLabel);
                    QUI.Space(1);
                }
                QUI.EndVertical();
                QUI.PropertyField(targetParticleSystem, width - QLabel.x - 20);
                QUI.FlexibleSpace();
                QUI.Space(SPACE_8);
            }
            QUI.EndHorizontal();

            infoMessage["ParticleSystemDisabled"].show.target = uiEffect.targetParticleSystem == null;
            DrawInfoMessage("ParticleSystemDisabled", GlobalWidth);

            QUI.Space(SPACE_4 * infoMessage["ParticleSystemDisabled"].show.faded);
        }

        void DrawTargetUIElement(float width)
        {
            QUI.Box(QStyles.GetBackgroundStyle(Style.BackgroundType.Low, uiEffect.targetUIElement == null ? QColors.Color.Red : QColors.Color.Blue), width, 54);
            QUI.Space(-54);

            QUI.BeginHorizontal(width);
            {
                QLabel.text = "Target UIElement";
                QLabel.style = Style.Text.Normal;
                tempFloat = QLabel.x; //save label width

                QUI.Space(SPACE_4);

                QUI.BeginVertical(QLabel.x, QUI.SingleLineHeight);
                {
                    QUI.Label(QLabel);
                    QUI.Space(1);
                }
                QUI.EndVertical();

                QUI.BeginChangeCheck();
                QUI.PropertyField(targetUIElement, width - QLabel.x - 20);
                if(QUI.EndChangeCheck())
                {
                    serializedObject.ApplyModifiedProperties();
                    RenameGameObject();
                }
                QUI.FlexibleSpace();
                QUI.Space(SPACE_8);
            }
            QUI.EndHorizontal();
            QUI.Space(-SPACE_2);
            tempFloat += SPACE_4 + 8; //calculate the indent for the next two rows
            QUI.BeginHorizontal(width);
            {
                QUI.Space(tempFloat);

                QLabel.text = "Element Category";
                QLabel.style = Style.Text.Small;

                QUI.BeginVertical(QLabel.x, QUI.SingleLineHeight);
                {
                    QUI.Label(QLabel);
                    QUI.Space(1);
                }
                QUI.EndVertical();

                QLabel.text = uiEffect.targetUIElement != null ? uiEffect.targetUIElement.elementCategory : MISSING_UIELEMENT;
                QLabel.style = Style.Text.Help;
                QUI.Label(QLabel);
                QUI.FlexibleSpace();
            }
            QUI.EndHorizontal();
            QUI.Space(-SPACE_4);
            QUI.BeginHorizontal(width);
            {
                QUI.Space(tempFloat);

                QLabel.text = "Element Name";
                QLabel.style = Style.Text.Small;

                QUI.BeginVertical(QLabel.x, QUI.SingleLineHeight);
                {
                    QUI.Label(QLabel);
                    QUI.Space(1);
                }
                QUI.EndVertical();

                QLabel.text = uiEffect.targetUIElement != null ? uiEffect.targetUIElement.elementName : MISSING_UIELEMENT;
                QLabel.style = Style.Text.Help;
                QUI.Label(QLabel);
                QUI.FlexibleSpace();
            }
            QUI.EndHorizontal();

            infoMessage["UIElementDisabled"].show.target = uiEffect.targetUIElement == null;
            DrawInfoMessage("UIElementDisabled", GlobalWidth);

            QUI.Space(SPACE_4 * infoMessage["UIElementDisabled"].show.faded);
        }

        void DrawSortingLayerName(float width)
        {
            QUI.Box(QStyles.GetBackgroundStyle(Style.BackgroundType.Low, useCustomSortingLayerName.boolValue ? QColors.Color.Blue : QColors.Color.Gray), width - 164, 20);
            QUI.Space(-20);

            QUI.BeginHorizontal(width);
            {
                QLabel.text = "Sorting Layer Name";
                QLabel.style = Style.Text.Normal;
                tempFloat = QLabel.x; //save label width

                QUI.Space(SPACE_4);

                QUI.BeginVertical(QLabel.x, QUI.SingleLineHeight);
                {
                    QUI.Space(1);
                    QUI.Label(QLabel);
                }
                QUI.EndVertical();

                if(!useCustomSortingLayerName.boolValue)
                {
                    targetSortingLayerName = TargetCanvas.overrideSorting
                                             ? TargetCanvas.sortingLayerName
                                             : TargetCanvas.rootCanvas.sortingLayerName;

                    QLabel.text = targetSortingLayerName;
                    QLabel.style = Style.Text.Help;

                    QUI.BeginVertical(width - tempFloat - 174, QUI.SingleLineHeight);
                    {
                        QUI.Space(2);
                        QUI.Label(QLabel);
                    }
                    QUI.EndVertical();

                    customSortingLayerName.stringValue = targetSortingLayerName;
                }
                else
                {
                    if(customSortingLayerName.stringValue == MISSING_UIELEMENT)
                    {
                        customSortingLayerName.stringValue = "Default";
                    }

                    QUI.PropertyField(customSortingLayerName, width - tempFloat - 184);
                    QUI.Space(2);
                }

                QUI.QToggle("use a custom layer name", useCustomSortingLayerName, 20);
            }
            QUI.EndHorizontal();
            QUI.Space(SPACE_2);
        }

        void DrawSortingOrder(float width)
        {
            QUI.Box(QStyles.GetBackgroundStyle(Style.BackgroundType.Low, useCustomOrderInLayer.boolValue ? QColors.Color.Blue : QColors.Color.Gray), width - 178, 20);
            QUI.Space(-20);

            QUI.BeginHorizontal(width);
            {
                QLabel.text = "Order in Layer";
                QLabel.style = Style.Text.Normal;
                tempFloat = QLabel.x; //save label width

                QUI.Space(SPACE_4);

                QUI.BeginVertical(QLabel.x, QUI.SingleLineHeight);
                {
                    QUI.Space(1);
                    QUI.Label(QLabel);
                }
                QUI.EndVertical();

                if(!useCustomOrderInLayer.boolValue)
                {
                    targetOrderInLayer = TargetCanvas.overrideSorting
                                         ? TargetCanvas.sortingOrder
                                         : TargetCanvas.rootCanvas.sortingOrder;

                    targetOrderInLayer = uiEffect.effectPosition == UIEffect.EffectPosition.InFrontOfTarget
                                         ? targetOrderInLayer + uiEffect.sortingOrderStep
                                         : targetOrderInLayer - uiEffect.sortingOrderStep;

                    QLabel.text = targetOrderInLayer.ToString();
                    QLabel.style = Style.Text.Help;

                    QUI.BeginVertical(width - tempFloat - 188, QUI.SingleLineHeight);
                    {
                        QUI.Space(2);
                        QUI.Label(QLabel);
                    }
                    QUI.EndVertical();

                    customOrderInLayer.intValue = targetOrderInLayer;
                }
                else
                {
                    QUI.PropertyField(customOrderInLayer, width - tempFloat - 198);
                    QUI.Space(2);
                }

                QUI.QToggle("use a custom order in layer", useCustomOrderInLayer, 20);
            }
            QUI.EndHorizontal();

            QUI.Space(SPACE_2);

            useCustomOrderInLayerAnimBool.target = !useCustomOrderInLayer.boolValue;
            QUI.Box(QStyles.GetBackgroundStyle(Style.BackgroundType.Low, QColors.Color.Gray), width, 24 + 20 * useCustomOrderInLayerAnimBool.faded);
            QUI.Space(-21 - 20 * useCustomOrderInLayerAnimBool.faded);

            if(QUI.BeginFadeGroup(useCustomOrderInLayerAnimBool.faded))
            {
                QUI.BeginHorizontal(width);
                {
                    QUI.Space(SPACE_4);

                    QLabel.text = "Set the effect";
                    QLabel.style = Style.Text.Normal;
                    QUI.BeginVertical(QLabel.x, QUI.SingleLineHeight);
                    {
                        QUI.Space(1);
                        QUI.Label(QLabel);
                    }
                    QUI.EndVertical();

                    QUI.PropertyField(effectPosition, width - QLabel.x - 132);

                    QLabel.text = "by";
                    QLabel.style = Style.Text.Normal;
                    QUI.BeginVertical(QLabel.x, QUI.SingleLineHeight);
                    {
                        QUI.Space(1);
                        QUI.Label(QLabel);
                    }
                    QUI.EndVertical();

                    QUI.PropertyField(sortingOrderStep, 40);

                    QLabel.text = "step";
                    QLabel.style = Style.Text.Normal;
                    QUI.BeginVertical(QLabel.x, QUI.SingleLineHeight);
                    {
                        QUI.Space(1);
                        QUI.Label(QLabel);
                    }
                    QUI.EndVertical();

                    QUI.FlexibleSpace();
                }
                QUI.EndHorizontal();
            }
            QUI.EndFadeGroup();

            QUI.Space(SPACE_2 * useCustomOrderInLayerAnimBool.faded);

            QUI.BeginHorizontal(width);
            {
                QUI.Space(SPACE_4);
                if(QUI.GhostButton("Update Sorting", QColors.Color.Gray, width - 8, 18))
                {
                    uiEffect.UpdateSorting();
                }
                QUI.Space(SPACE_4);
            }
            QUI.EndHorizontal();
        }

        void DrawSettings(float width)
        {
            QUI.Space(SPACE_2);
            QUI.BeginHorizontal(width);
            {
                QUI.QToggle("play on awake", playOnAwake, 20);
                QUI.FlexibleSpace();

                QUI.QToggle("stop instantly on hide", stopInstantly, 20);
                QUI.FlexibleSpace();

                QLabel.text = "start delay on show";
                QLabel.style = Style.Text.Normal;
                QUI.Box(QStyles.GetBackgroundStyle(Style.BackgroundType.Low, startDelay.floatValue != 0 ? QColors.Color.Blue : QColors.Color.Gray), 4 + QLabel.x + 40 + 16, 20);
                QUI.Space(-(QLabel.x + 40 + 16));
                QUI.BeginVertical(QLabel.x, QUI.SingleLineHeight);
                {
                    QUI.Space(1);
                    QUI.Label(QLabel);
                }
                QUI.EndVertical();
                QUI.PropertyField(startDelay, 40);
                QUI.Space(SPACE_4);
            }
            QUI.EndHorizontal();
        }
    }
}
