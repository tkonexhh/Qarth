using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Text;
using System.IO;
using System.Reflection;
using LitJson;

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
                v.state.isAttribute = fields[i].isAttr;
                v.state.isEvent = fields[i].isEvent;
            }
        }

        public string GetBuildUICode()
        {
            StringBuilder variable = new StringBuilder();
            StringBuilder controllerEvent = new StringBuilder();
            StringBuilder attributeVariable = new StringBuilder();
            StringBuilder attribute = new StringBuilder();
            StringBuilder find = new StringBuilder();
            StringBuilder newAttribute = new StringBuilder();
            StringBuilder register = new StringBuilder();
            StringBuilder function = new StringBuilder();

            newAttribute.Length =
            attributeVariable.Length =
            function.Length =
            register.Length =
            variable.Length =
            controllerEvent.Length =
            attribute.Length =
            find.Length = 0;

            foreach (var value in dic.Values)
            {
                if (!value.state.isVariable) continue;
                variable.AppendFormat(GConfigure.variableFormat, value.type, value.name);

                if (isFind)
                    find.AppendFormat(GConfigure.findFormat, value.name, value.path, value.type);

                if (value.state.isAttribute)
                {
                    if (value.isUI)
                    {
                        attribute.AppendFormat(GConfigure.attributeFormat, value.type, value.name);
                    }
                    else
                    {
                        attribute.AppendFormat(GConfigure.attribute2Format, value.type, value.name);
                    }
                }

                if (value.variableEvent != string.Empty && value.state.isEvent)
                {
                    register.AppendFormat(GConfigure.registerFormat, value.name, value.variableEvent, value.eventName);
                    controllerEvent.AppendFormat(GConfigure.controllerEventFormat, value.IsButton() ? string.Empty : string.Format("<{0}>", value.eventType), value.attributeName);
                    function.AppendFormat(GConfigure.functionFormat, value.eventName,
                        value.eventType + (!value.eventType.IsLengthZero() ? " value" : string.Empty), value.attributeName,
                        value.IsButton() ? string.Empty : "value");
                    //Debug.Log(value.IsButton());
                }
            }

            var tmp = string.Format(GConfigure.uiClassCode,
                variable, attributeVariable, controllerEvent, attribute, find, newAttribute, register, function);
            return string.Format(GConfigure.uiCode_BindUI, GGlobalFun.GetString(GConfigure.selectTransform.name), tmp);
        }

        // StringBuilder assignment = new StringBuilder();
        // StringBuilder declare = new StringBuilder();
        // StringBuilder fun = new StringBuilder();
        // public string GetControllerBuildCode()
        // {
        //     assignment.Length =
        //     declare.Length =
        //     fun.Length = 0;
        //     string type = string.Empty;
        //     foreach (var value in dic.Values)
        //     {
        //         if (value.variableEvent != string.Empty && value.state.isEvent)
        //         {
        //             type = value.IsButton() ? string.Empty : string.Format("{0} value", value.eventType);
        //             assignment.AppendFormat(GConfigure.assignmentFormat, value.attributeName, value.eventName);
        //             declare.AppendFormat(GConfigure.declareFormat, value.attributeName, type);
        //             fun.AppendFormat(GConfigure.funFormat, value.eventName, type,
        //                  value.attributeName, value.IsButton() ? string.Empty : "value");
        //         }
        //     }

        //     string code = string.Empty;
        //     if (GConfigure.isCreateModel)
        //     {
        //         code = GConfigure.controllerBuildCode;
        //     }
        //     else
        //     {
        //         code = GConfigure.controllerBuildCode2;
        //     }
        //     return string.Format(
        //         code,
        //         GGlobalFun.GetString(GConfigure.selectTransform.name),
        //         assignment,
        //         declare,
        //         fun);
        // }

        public string GetMainCode()
        {
            switch (GConfigure.Version)
            {
                case ScriptVersion.Mono:
                    return string.Format(GConfigure.uiCode_Mono, GGlobalFun.GetString(GConfigure.selectTransform.name), GConfigure.uicodeOnAwake);
                case ScriptVersion.Panel:
                    return string.Format(GConfigure.uiCode_Panel, GGlobalFun.GetString(GConfigure.selectTransform.name), GConfigure.uicodeQarthPanel);
                case ScriptVersion.AnimPanel:
                    return string.Format(GConfigure.uiCode_Panel, GGlobalFun.GetString(GConfigure.selectTransform.name), GConfigure.uicodeQarthAnimPanel);
                default:
                    return string.Format(GConfigure.uiCode_AnimPanel, GGlobalFun.GetString(GConfigure.selectTransform.name), GConfigure.uicodeOnAwake);
            }
        }

        // public string GetModelCode()
        // {
        //     return string.Format(GConfigure.modelCode, GGlobalFun.GetString(GConfigure.selectTransform.name));
        // }

        // public string GetControllerCode()
        // {
        //     return string.Format(GConfigure.controllerCode, GGlobalFun.GetString(GConfigure.selectTransform.name));
        // }

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

            GFileOperation.WriteText(GConfigure.FilePath(GConfigure.MainFileName), GetMainCode());
            // GFileOperation.WriteText(GConfigure.FilePath(GConfigure.UIFileName), GetUICode());
            GFileOperation.WriteText(GConfigure.FilePath(GConfigure.UIBuildFileName), GetBuildUICode());

            // if (GConfigure.isCreateModel)
            // {
            //     GFileOperation.WriteText(GConfigure.FilePath(GConfigure.ModelFileName), GetModelCode());
            // }

            // if (GConfigure.isCreateController)
            // {
            //     GFileOperation.WriteText(GConfigure.FilePath(GConfigure.ControllerFileName), GetControllerCode());
            //     GFileOperation.WriteText(GConfigure.FilePath(GConfigure.ControllerBuildFileName), GetControllerBuildCode());
            // }

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

            // if (GConfigure.isCreateController)
            // {
            //     GFileOperation.WriteText(GConfigure.FilePath(GConfigure.ControllerBuildFileName), GetControllerBuildCode(), FileMode.Create);
            // }

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

            //if (GConfigure.Version == ScriptVersion.Mono)
            //{
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
                if (string.IsNullOrEmpty(info.name)) continue;
                type.InvokeMember(info.name,
                                BindingFlags.SetField |
                                BindingFlags.Instance |
                                BindingFlags.NonPublic,
                                null, target, new object[] { root.Find(info.path).GetComponent(info.type) }, null, null, null);
            }
            //}

            // if (GConfigure.Version == ScriptVersion.Mono)
            // {
            //     if (!GFileOperation.IsExists(GConfigure.GetInfoPath()))
            //     {
            //         EditorUtility.DisplayDialog(GConfigure.msgTitle, GConfigure.plugCreate, GConfigure.ok);
            //         return;
            //     }
            //     var value = GFileOperation.ReadText(GConfigure.GetInfoPath());
            //     var jd = JsonMapper.ToObject(value);
            //     if (jd.IsArray)
            //     {
            //         for (int i = 0; i < jd.Count; i++)
            //         {
            //             VariableJson vj = JsonMapper.ToObject<VariableJson>(jd[i].ToJson());
            //             if (string.IsNullOrEmpty(vj.name)) continue;
            //             type.InvokeMember(vj.name,
            //                             BindingFlags.SetField |
            //                             BindingFlags.Instance |
            //                             BindingFlags.NonPublic,
            //                             null, target, new object[] { root.Find(vj.findPath).GetComponent(vj.type) }, null, null, null);
            //         }
            //     }
            // }

            // var obj = PrefabUtility.GetCorrespondingObjectFromOriginalSource(root.gameObject);
            // if (obj != null)
            // {
            //     PrefabUtility.SaveAsPrefabAssetAndConnect(root.gameObject, obj, ReplacePrefabOptions.ConnectToPrefab);
            //     AssetDatabase.Refresh();
            // }
        }


        // public void GetBindingInfoToJson()
        // {
        //     if (GConfigure.selectTransform == null) return;

        //     JsonData jd = new JsonData();
        //     foreach (var item in dic)
        //     {
        //         if (!item.Value.state.isVariable) continue;
        //         VariableJson vj = new VariableJson();
        //         var state = item.Value.state;
        //         vj.isOpen = state.isOpen;
        //         vj.isAttribute = state.isAttribute;
        //         vj.isEvent = state.isEvent;
        //         vj.isVariable = state.isVariable;
        //         vj.index = state.index;
        //         vj.name = item.Value.name;
        //         vj.type = item.Value.type;
        //         vj.findPath = GGlobalFun.GetGameObjectPath(item.Key, GConfigure.selectTransform);
        //         jd.Add(JsonMapper.ToObject(JsonMapper.ToJson(vj)));
        //     }
        //     GFileOperation.WriteText(GConfigure.GetInfoPath(), jd.ToJson());
        // }

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

            List<string> k = new List<string>(dic.Count);
            List<string> t = new List<string>(dic.Count);
            List<string> p = new List<string>(dic.Count);

            List<bool> lstEvent = new List<bool>(dic.Count);
            List<bool> lstAttr = new List<bool>(dic.Count);

            foreach (var key in dic.Keys)
            {
                var target = dic[key];
                if (target.state.isVariable)
                {

                    string name = string.Format("m_{0}", target.name);
                    k.Add(name);
                    t.Add(target.type.ToString());
                    p.Add(GGlobalFun.GetGameObjectPath(key, GConfigure.selectTransform));
                    lstEvent.Add(target.state.isEvent);
                    lstAttr.Add(target.state.isAttribute);
                }
            }

            int count = k.Count;
            var infos = new GScriptInfo.FieldInfo[count];
            for (int i = 0; i < count; i++)
            {
                infos[i] = new GScriptInfo.FieldInfo();
                infos[i].name = k[i];
                infos[i].type = t[i];
                infos[i].path = p[i];
                infos[i].isEvent = lstEvent[i];
                infos[i].isAttr = lstAttr[i];
            }

            so.SetClassInfo(GConfigure.MainFileName, infos);

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