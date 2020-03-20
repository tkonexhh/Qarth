using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Text;
using System.IO;
using System.Reflection;
//using LitJson;

namespace Qarth
{
    public class GManagerVariable
    {
        public bool isFind = false;

        private Dictionary<Transform, GVariableModel> dic = new Dictionary<Transform, GVariableModel>();

        public GVariableModel this[Transform t]
        {
            get
            {
                if (!dic.ContainsKey(t))
                {
                    dic.Add(t, new GVariableModel(t));
                }
                return dic[t];
            }
        }


        public void Init()
        {
            GScriptInfo so = null;
            if (GFileOperation.IsExists(GConfigure.InfoPath))
            {
                so = AssetDatabase.LoadAssetAtPath<GScriptInfo>(GConfigure.InfoPath);
            }
            if (so == null) return;

            var fields = so.GetFieldInfos(GConfigure.selectTransform.name);
            for (int i = 0; i < fields.Length; i++)
            {
                var obj = GConfigure.selectTransform.Find(fields[i].path);
                if (obj == null) continue;
                var v = this[obj];
                v.state.isVariable = true;
                v.state.isAttribute = fields[i].mainAttrInfo.isAttr;
                v.state.attributeName = fields[i].mainAttrInfo.name;
                v.state.isEvent = fields[i].mainAttrInfo.isEvent;
                v.type = fields[i].mainAttrInfo.type;
                v.state.SetIndex(fields[i].mainAttrInfo.type, obj);
                v.LstSubModel.Clear();
                v.state.LstSubState.Clear();
                for (int j = 0; j < fields[i].subField.Count; j++)
                {
                    var subState = new QVariableState();
                    subState.isVariable = true;
                    subState.isAttribute = fields[i].subField[j].isAttr;

                    subState.isEvent = fields[i].subField[j].isEvent;
                    subState.SetIndex(fields[i].subField[j].type, obj);
                    v.AddSubState(subState);
                    subState.Model.type = fields[i].subField[j].type;
                    subState.attributeName = fields[i].subField[j].name;
                }
            }

            GConfigure.Version = so.GetVersion(GConfigure.selectTransform.name);
        }

        StringBuilder variable = new StringBuilder();
        StringBuilder assetNotNull = new StringBuilder();
        StringBuilder controllerEvent = new StringBuilder();
        StringBuilder attributeVariable = new StringBuilder();
        StringBuilder attribute = new StringBuilder();
        StringBuilder find = new StringBuilder();
        StringBuilder newAttribute = new StringBuilder();
        StringBuilder register = new StringBuilder();
        StringBuilder function = new StringBuilder();

        public string GetBuildUICode()
        {
            newAttribute.Length =
            attributeVariable.Length =
            function.Length =
            register.Length =
            variable.Length =
            assetNotNull.Length =
            controllerEvent.Length =
            attribute.Length =
            find.Length = 0;

            foreach (var value in dic.Values)
            {
                if (!value.state.isVariable) continue;
                variable.AppendFormat(GConfigure.variableFormat, value.type, value.state.attributeName);
                assetNotNull.AppendFormat(GConfigure.assetNotNull, value.state.attributeName);

                for (int i = 0; i < value.LstSubModel.Count; i++)
                {
                    if (value.LstSubModel[i].state.isVariable)
                    {
                        variable.AppendFormat(GConfigure.variableFormat, value.LstSubModel[i].type, value.LstSubModel[i].state.attributeName);
                        assetNotNull.AppendFormat(GConfigure.assetNotNull, value.LstSubModel[i].state.attributeName);
                    }

                }

                if (isFind)
                    find.AppendFormat(GConfigure.findFormat, value.name, value.path, value.type);

                // if (value.state.isAttribute)
                // {
                //     attribute.AppendFormat(GConfigure.attributeFormat, value.type, value.state.attributeName);
                //     assetNotNull.AppendFormat(GConfigure.assetNotNull, value.state.attributeName);
                // }

                // for (int i = 0; i < value.LstSubModel.Count; i++)
                // {
                //     if (value.LstSubModel[i].state.isAttribute)
                //     {
                //         attribute.AppendFormat(GConfigure.attributeFormat, value.LstSubModel[i].type, value.LstSubModel[i].state.attributeName);
                //         assetNotNull.AppendFormat(GConfigure.assetNotNull, value.state.attributeName);
                //     }
                // }

                if (value.variableEvent != string.Empty && value.state.isEvent)
                {
                    controllerEvent.AppendFormat(GConfigure.controllerEventFormat,
                        value.IsButton() ? string.Empty : string.Format("<{0}>", value.eventType),
                        value.GetActionMethodName());

                    register.AppendFormat(GConfigure.registerFormat, value.state.attributeName, value.variableEvent, value.GetEventMethodName());

                    function.AppendFormat(GConfigure.functionFormat, value.eventName,
                        value.eventType + (!value.eventType.IsLengthZero() ? " value" : string.Empty),
                        value.state.attributeName,
                        value.actionName,
                        value.IsButton() ? string.Empty : "value");
                }

                for (int i = 0; i < value.LstSubModel.Count; i++)
                {
                    if (value.LstSubModel[i].variableEvent != string.Empty && value.LstSubModel[i].state.isEvent)
                    {
                        controllerEvent.AppendFormat(GConfigure.controllerEventFormat,
                            value.LstSubModel[i].IsButton() ? string.Empty : string.Format("<{0}>", value.LstSubModel[i].eventType),
                            value.LstSubModel[i].GetActionMethodName());

                        register.AppendFormat(GConfigure.registerFormat, value.LstSubModel[i].state.attributeName, value.LstSubModel[i].variableEvent, value.LstSubModel[i].GetEventMethodName());

                        function.AppendFormat(GConfigure.functionFormat, value.LstSubModel[i].eventName,
                            value.LstSubModel[i].eventType + (!value.LstSubModel[i].eventType.IsLengthZero() ? " value" : string.Empty),
                            value.LstSubModel[i].state.attributeName,
                            value.LstSubModel[i].actionName,
                            value.LstSubModel[i].IsButton() ? string.Empty : "value");
                    }
                }
            }

            var tmp = string.Format(GConfigure.Version == ScriptVersion.Mono ? GConfigure.uiClassCode_Mono : GConfigure.uiClassCode,
                variable, attributeVariable, controllerEvent, attribute, assetNotNull, find, newAttribute, register, function);
            return string.Format(GConfigure.uiCode_BindUI, GGlobalFun.GetString(GConfigure.selectTransform.name), tmp);
        }


        public string GetMainCode()
        {
            switch (GConfigure.Version)
            {
                case ScriptVersion.Mono:
                    return string.Format(GConfigure.uiCode_Mono, GGlobalFun.GetString(GConfigure.selectTransform.name), GConfigure.uicodeOnAwake);
                case ScriptVersion.Panel:
                    return string.Format(GConfigure.uiCode_Panel, GGlobalFun.GetString(GConfigure.selectTransform.name), GConfigure.uicodeQarthPanel);
                case ScriptVersion.AnimPanel:
                    return string.Format(GConfigure.uiCode_AnimPanel, GGlobalFun.GetString(GConfigure.selectTransform.name), GConfigure.uicodeQarthAnimPanel);
                default:
                    return string.Format(GConfigure.uiCode_Mono, GGlobalFun.GetString(GConfigure.selectTransform.name), GConfigure.uicodeOnAwake);
            }
        }

        public override string ToString()
        {
            return GetBuildUICode();
        }

        public void Clear()
        {
            foreach (var value in dic.Values)
            {
                value.Reset();
            }
            dic.Clear();
        }

        public void TotalFold(bool isOn = true)
        {
            foreach (var value in dic.Values)
            {
                value.state.isOpen = isOn;
            }
        }

        public void TotalSelectVariable(bool isOn = true)
        {
            if (GConfigure.selectTransform != null)
                TotalSelect(GConfigure.selectTransform, isOn);
        }

        public void TotalAttribute(bool isOn = true)
        {
            foreach (var value in dic.Values)
            {
                if (!value.state.isVariable) continue;
                value.state.isAttribute = isOn;
            }
        }

        public void TotalEvent(bool isOn = true)
        {
            foreach (var value in dic.Values)
            {
                if (!value.state.isVariable || !value.state.isSelectEvent) continue;
                value.state.isEvent = isOn;
            }
        }

        private void TotalSelect(Transform tr, bool isOn)
        {
            foreach (Transform t in tr)
            {
                var tmp = dic[t];
                tmp.state.isVariable = isOn;
                if (tmp.state.isOpen && t.childCount > 0)
                {
                    TotalSelect(t, isOn);
                }
            }
        }

        public void CreatePrefab()
        {
            if (GConfigure.selectTransform == null)
            {
                EditorUtility.DisplayDialog(GConfigure.msgTitle, GConfigure.noSelect, GConfigure.ok);
                return;
            }
            if (EditorApplication.isCompiling)
            {
                EditorUtility.DisplayDialog(GConfigure.msgTitle, GConfigure.editorCompiling, GConfigure.ok);
                return;
            }

            var root = GConfigure.selectTransform;

            if (GFileOperation.IsDirctoryName("Assets/" + GConfigure.prefabSavePath, true))
            {
                PrefabUtility.SaveAsPrefabAssetAndConnect(root.gameObject, "Assets/" + GConfigure.prefabSavePath + root.name + ".prefab", InteractionMode.AutomatedAction);
                AssetDatabase.Refresh();

                EditorUtility.DisplayDialog(GConfigure.msgTitle, GConfigure.createPrefabSuccessTitle, GConfigure.ok);
            }

        }

        // 创建脚本
        public void CreateFile()
        {
            if (GConfigure.selectTransform == null)
            {
                EditorUtility.DisplayDialog(GConfigure.msgTitle, GConfigure.noSelect, GConfigure.ok);
                return;
            }
            if (EditorApplication.isCompiling)
            {
                EditorUtility.DisplayDialog(GConfigure.msgTitle, GConfigure.editorCompiling, GConfigure.ok);
                return;
            }
            if (GFileOperation.IsExists(GConfigure.FilePath(GConfigure.UIBuildFileName)))
            {
                EditorUtility.DisplayDialog(GConfigure.msgTitle, GConfigure.haveBeenCreated, GConfigure.ok);
                return;
            }

            if (!GFileOperation.IsExists(GConfigure.FilePath(GConfigure.MainFileName)))
            {
                GFileOperation.WriteText(GConfigure.FilePath(GConfigure.MainFileName), GetMainCode());
            }

            GFileOperation.WriteText(GConfigure.FilePath(GConfigure.UIBuildFileName), GetBuildUICode());

            GetBindingInfo();
            // if (GConfigure.Version == ScriptVersion.Mono)
            // {
            //     GetBindingInfo();
            // }
            // else
            // {
            //     GetBindingInfoToJson();
            // }
            GConfigure.Compiling();
            AssetDatabase.Refresh();
        }

        //更新脚本
        public void UpdateFile()
        {
            if (GConfigure.selectTransform == null) return;
            if (EditorApplication.isCompiling)
            {
                EditorUtility.DisplayDialog(GConfigure.msgTitle, GConfigure.editorCompiling, GConfigure.ok);
                return;
            }
            var fileName = GConfigure.FilePath(GConfigure.UIBuildFileName);
            if (!GFileOperation.IsExists(fileName))
            {
                EditorUtility.DisplayDialog(GConfigure.msgTitle, GConfigure.notCreate, GConfigure.ok);
                return;
            }
            //重新更新BindUI文件
            GFileOperation.WriteText(GConfigure.FilePath(GConfigure.UIBuildFileName), GetBuildUICode(), FileMode.Create);

            GetBindingInfo();
            // if (GConfigure.Version == ScriptVersion.Mono)
            // {
            //     GetBindingInfo();
            // }
            // else
            // {
            //     GetBindingInfoToJson();
            // }
            GConfigure.Compiling();
            AssetDatabase.Refresh();
        }

        public void Copy()
        {
            if (GConfigure.selectTransform == null) return;
            GUIUtility.systemCopyBuffer = GetBuildUICode();
            EditorUtility.DisplayDialog(GConfigure.msgTitle, GConfigure.copy, GConfigure.ok);
        }

        //挂载脚本
        public void MountScript()
        {
            if (GConfigure.selectTransform == null) return;

            if (EditorApplication.isCompiling)
            {
                EditorUtility.DisplayDialog(GConfigure.msgTitle, GConfigure.editorCompiling, GConfigure.ok);
                return;
            }

            var name = GConfigure.MainFileName;
            //一定要添加命名空间
            var scriptType = GGlobalFun.GetAssembly().GetType(GConfigureDefine.namespaceStr + "." + name);

            if (scriptType == null)
            {
                EditorUtility.DisplayDialog(GConfigure.msgTitle, GConfigure.notCreate, GConfigure.ok);
                return;
            }
            var root = GConfigure.selectTransform.gameObject;
            var target = root.GetComponent(scriptType);
            if (target == null)
            {
                target = root.AddComponent(scriptType);
                return;
            }

            EditorUtility.DisplayDialog(GConfigure.msgTitle, GConfigure.hasMount, GConfigure.ok);
        }

        public void BindingUI()
        {
            if (GConfigure.selectTransform == null) return;
            if (EditorApplication.isCompiling)
            {
                EditorUtility.DisplayDialog(GConfigure.msgTitle, GConfigure.editorCompiling, GConfigure.ok);
                return;
            }

            if (GConfigure.selectTransform.GetComponent(GConfigure.MainFileName) == null)
            {
                EditorUtility.DisplayDialog(GConfigure.msgTitle, GConfigure.noMountScript, GConfigure.ok);
                return;
            }

            var assembly = GGlobalFun.GetAssembly();
            var type = assembly.GetType(GConfigureDefine.namespaceStr + "." + GConfigure.MainFileName);

            if (type == null)
            {
                EditorUtility.DisplayDialog(GConfigure.msgTitle, GConfigure.notCreate, GConfigure.ok);
                return;
            }

            var root = GConfigure.selectTransform;
            var target = root.GetComponent(type);

            var so = AssetDatabase.LoadAssetAtPath<GScriptInfo>(GConfigure.InfoPath);
            if (so == null)
            {
                return;
            }
            var infos = so.GetFieldInfos(GConfigure.MainFileName);
            if (infos == null)
            {
                EditorUtility.DisplayDialog(GConfigure.msgTitle, GConfigure.plugCreate, GConfigure.ok);
                return;
            }
            foreach (var info in infos)
            {
                if (string.IsNullOrEmpty(info.mainAttrInfo.name)) continue;
                BindAttributeByInfo(type, root, target, info, info.mainAttrInfo);

                foreach (var subInfo in info.subField)
                {
                    if (string.IsNullOrEmpty(subInfo.name)) continue;
                    BindAttributeByInfo(type, root, target, info, subInfo);
                }
            }
        }

        private void BindAttributeByInfo(System.Type type, Transform root, Component target, GScriptInfo.FieldInfo fieldInfo, GScriptInfo.FieldAttrInfo attrInfo)
        {
            if (attrInfo.type == "GameObject")
            {
                type.InvokeMember("m_" + attrInfo.name,
                                              BindingFlags.SetField |
                                              BindingFlags.Instance |
                                              BindingFlags.NonPublic,
                                              null, target, new object[] { root.Find(fieldInfo.path).gameObject }, null, null, null);
            }
            else
            {
                type.InvokeMember("m_" + attrInfo.name,
                                              BindingFlags.SetField |
                                              BindingFlags.Instance |
                                              BindingFlags.NonPublic,
                                              null, target, new object[] { root.Find(fieldInfo.path).GetComponent(attrInfo.type) }, null, null, null);
            }
        }

        private void GetBindingInfo()
        {
            GScriptInfo so;
            if (GFileOperation.IsExists(GConfigure.InfoPath))
            {
                so = AssetDatabase.LoadAssetAtPath<GScriptInfo>(GConfigure.InfoPath);
            }
            else
            {
                so = ScriptableObject.CreateInstance<GScriptInfo>();
            }

            if (so == null) return;

            List<GScriptInfo.FieldInfo> fieldInfos = new List<GScriptInfo.FieldInfo>();

            foreach (var key in dic.Keys)
            {
                var target = dic[key];
                if (target.state.isVariable)
                {
                    GScriptInfo.FieldInfo info = new GScriptInfo.FieldInfo();
                    info.path = GGlobalFun.GetGameObjectPath(key, GConfigure.selectTransform);
                    info.mainAttrInfo.name = target.state.attributeName;
                    info.mainAttrInfo.type = target.type.ToString();
                    info.mainAttrInfo.isAttr = target.state.isAttribute;
                    info.mainAttrInfo.isEvent = target.state.isEvent;

                    var subStates = target.state.LstSubState;
                    info.subField = new List<GScriptInfo.FieldAttrInfo>();
                    for (int i = 0; i < subStates.Count; i++)
                    {
                        GScriptInfo.FieldAttrInfo subInfo = new GScriptInfo.FieldAttrInfo();
                        subInfo.name = subStates[i].attributeName;
                        subInfo.type = subStates[i].Model.type.ToString();
                        subInfo.isAttr = subStates[i].isAttribute;
                        subInfo.isEvent = subStates[i].isEvent;
                        info.subField.Add(subInfo);
                    }
                    fieldInfos.Add(info);
                }
            }

            var infos = fieldInfos.ToArray();

            so.SetClassInfo(GConfigure.MainFileName, GConfigure.Version, infos);

            if (GFileOperation.IsExists(GConfigure.InfoPath))
            {
                AssetDatabase.SaveAssets();
            }
            else
            {
                if (GFileOperation.IsDirctoryName(GConfigure.InfoPath, true))
                {
                    AssetDatabase.CreateAsset(so, GConfigure.InfoPath);
                }
            }
        }
    }
}