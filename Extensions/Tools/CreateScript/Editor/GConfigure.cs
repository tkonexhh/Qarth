using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

namespace Qarth
{
    public enum ScriptVersion
    {
        Mono,
        Panel,
        AnimPanel,
    }

    public class GConfigureDefine
    {
        public const string referencedefaultPath = "Scripts/Game/UIScripts/";
        public const string prefabdefaultPath = "Resources/UI/Panels/";
        public const string namespaceStr = "GameWish.Game";
    }

    public partial class GConfigure
    {
        private static ScriptVersion m_Version;
        public static string[] versionStr = new string[] { "Mono", "Panel", "AnimPanel" };
        public static Transform selectTransform;
        public static string referencePath;
        public static string prefabSavePath;
        // public static bool isCreateModel = false;
        // public static bool isCreateController = true;

        public static string InfoPath
        {
            get
            {
                string plugPath = GetSelectObjectRootPath() + string.Format("/BindInfo/{0}_Info.asset", selectTransform.name);
                plugPath = plugPath.Remove(0, plugPath.IndexOf("Assets"));
                plugPath = plugPath.Replace("//", "/");
                return plugPath;
            }
        }

        public static string msgTitle = "温馨提示";
        public static string ok = "知道了";
        public static string noSelect = "没有选对象呢";
        public static string haveBeenCreated = "脚本已创建了，点击更新哦~";
        public static string notCreate = "你还没生成脚本呢";
        public static string hasMount = "已经挂载脚本了";
        public static string editorCompiling = "编辑器傲娇中...";
        public static string plugCreate = "没有使用插件生成对应的脚本呢";
        public static string copy = "复制到剪贴板啦！";
        public static string noMountScript = "还没挂载脚本呢～";
        public static string createPrefabSuccessTitle = "预制体创建成功";


        public static string variableFormat = "\t\t[SerializeField] private {0} {1};\n";
        public static string findFormat = "\t\t\tm_{0} = transform.Find(\"{1}\").GetComponent<{2}>();\n";
        public static string attributeVariableFormat = "\t\tprivate {0,-45} m_{1};\n";
        public static string attributeFormat = "\t\tpublic {0} {1} {{ get {{ return m_{1}; }} }}\n";
        public static string newAttributeFormat = "\t\tq{0,-49} = new Q{1}({0});\n";
        public static string attribute2Format = "\t\tpublic {0} {1} {{ get {{ return m_{1}; }} }}\n";
        public static string registerFormat = "\t\t\tm_{0}.{1}.AddListener({2});\n";
        public static string controllerEventFormat = "\t\tpublic Action{0} {1};\n";
        public static string functionFormat = "\t\tprivate void {0}({1})\n\t\t{{\n\t\t\tif( {2} != null ){2}({3});\n\t\t}}\n";

        public static string assignmentFormat = "\t\tui.{0,-50} = {1};\n";
        public static string declareFormat = "\tpartial void {0}({1});\n";
        public static string funFormat = "\t\tprivate void {0}({1})\n\t\t{{\n\t\t{2}({3});\n\t\t}}\n";

        public static string uicodeOnAwake = "\t\tpartial void OnAwake()\n\t\t{\n\t\t}\n\n";
        public static string uicodeQarthPanel = "\t\tprotected override void OnUIInit()\n\t\t{\n\t\t}\n\n" +
                                                "\t\tprotected override void OnPanelOpen(params object[] args)\n\t\t{\n\t\t}\n\n" +
                                                "\t\tprotected override void OnOpen()\n\t\t{\n\t\t}\n\n" +
                                                "\t\tprotected override void OnClose()\n\t\t{\n\t\t}\n\n";

        public static string uicodeQarthAnimPanel = "\t\tprotected override void OnUIInit()\n\t\t{\n\t\t}\n\n" +
                                                    "\t\tprotected override void OnPanelOpen(params object[] args)\n\t\t{\n\t\t}\n\n" +
                                                    "\t\tprotected override void OnOpen()\n\t\t{\n\t\t}\n\n" +
                                                    "\t\tprotected override void OnPanelHideComplete()\n\t\t{\n\t\t}\n\n" +
                                                    "\t\tprotected override void OnClose()\n\t\t{\n\t\t}\n\n";

        public static string MainFileName { get { return GetMainFileName(); } }
        // /public static string UIFileName { get { return GetFileName("UI"); } }
        public static string UIBuildFileName { get { return GetFileName("BuildUI"); } }
        // public static string ModelFileName { get { return GetFileName("Model"); } }
        // public static string ControllerFileName { get { return GetFileName("Controller"); } }
        // public static string ControllerBuildFileName { get { return GetFileName("BuildController"); } }

        // public static string UIName { get { return GetClassName("UI"); } }
        // public static string UIBuildName { get { return GetClassName("BuildUI"); } }
        // public static string ModelName { get { return GetClassName("Model"); } }
        // public static string ControllerName { get { return GetClassName("Controller"); } }
        // public static string ControllerBuildName { get { return GetClassName("BuildController"); } }

        public static readonly string uiCode_Mono =
            "using UnityEngine;\n" +
            "using UnityEngine.UI;\n" +
            "using System;\n\n" +
            "namespace " + GConfigureDefine.namespaceStr + "\n{{\n" +
            "\tpublic partial class {0} : MonoBehaviour\n" +
            "\t{{\n{1}\n}}\n}" +
            "}";

        public static readonly string uiCode_Panel =
            "using UnityEngine;\n" +
            "using UnityEngine.UI;\n" +
            "using Qarth;\n" +
            "using System;\n\n" +
            "namespace " + GConfigureDefine.namespaceStr + "\n{{\n" +
            "\tpublic partial class {0} : AbstractPanel\n" +
            "\t{{\n{1}\n}}\n\t}" +
            "}";

        public static readonly string uiCode_AnimPanel =
            "using UnityEngine;\n" +
            "using UnityEngine.UI;\n" +
            "using Qarth;\n" +
            "using System;\n\n" +
            "namespace " + GConfigureDefine.namespaceStr + "\n{{\n" +
            "\tpublic partial class {0} : AbstractAnimPanel\n" +
            "\t{{\n{1}\n}}\n\t}" +
            "}";


        public static readonly string uiCode_BindUI =
            "using UnityEngine;\n" +
            "using UnityEngine.UI;\n" +
            "using System;\n\n" +
            "namespace " + GConfigureDefine.namespaceStr + "\n{{\n" +
            "\tpublic partial class {0} \n" +
            "\t{{\n{1}\n}}\n}" +
            "}";

        public static readonly string uiClassCode =
            "{0}\n{1}\n{2}\n{3}\n" +
            "\t\tpartial void OnAwake();\n" +
            "\t\tprivate void Awake()\n\t\t{{\n" +
            "{4}\n{5}\n{6}\n" +
            "\t\t\tOnAwake();\n\n" +
            "\t\t}}\n" +
            "{7}";

        //     public static readonly string modelCode =
        //         "\n\npublic class {0}_Model\n{{\n\tpublic {0}_Model()\n\t{{\n\n\t}}\n}}";

        //     public static readonly string controllerCode =
        //         "using UnityEngine;\n" +
        //         "using UnityEngine.EventSystems;\n\n" +
        //         "\n\npublic partial class {0}_Controller\n{{\n" +
        //         "\tpartial void OnAwake()\n\t{{\n\n\t}}\n}}";

        //     public static readonly string controllerBuildCode =
        //         "using UnityEngine;\n\n" +
        //         "\n\npublic partial class {0}_Controller:IController\n{{\n" +
        //         "\tprivate {0}_UI ui;\n" +
        //         "\tprivate {0}_Model model = new {0}_Model();\n\n" +
        //         "\tpublic {0}_Controller({0}_UI ui)\n\t{{\n" +
        //         "\t\tthis.ui = ui;\n" +
        //         "\t\tOnAwake();\n" +
        //         "{1}" +
        //         "\t}}\n" +
        //         "\tpartial void OnAwake();\n" +
        //         "{2}\n{3}\n" +
        //         "}}";

        //     public static readonly string controllerBuildCode2 =
        // "using UnityEngine;\n\n" +
        // "\n\npublic partial class {0}_Controller:IController\n{{\n" +
        // "\tprivate {0}_UI ui;\n" +
        // //"\tprivate {0}_Model model = new {0}_Model();\n\n" +
        // "\tpublic {0}_Controller({0}_UI ui)\n\t{{\n" +
        // "\t\tthis.ui = ui;\n" +
        // "\t\tOnAwake();\n" +
        // "{1}" +
        // "\t}}\n" +
        // "\tpartial void OnAwake();\n" +
        // "{2}\n{3}\n" +
        // "}}";

        public static ScriptVersion Version
        {
            get { return m_Version; }
            set
            {
                m_Version = value;
            }
        }

        public static string FilePath(string name)
        {
            var filePath = string.Format("{0}/{1}/{2}/{3}.cs", Application.dataPath, referencePath, GGlobalFun.GetString(Selection.activeTransform.name), name);
            if (!GFileOperation.IsDirctoryName(filePath, true))
            {
                EditorUtility.DisplayDialog(msgTitle, "文件夹无法创建", "OK");
                Debug.LogException(new Exception("文件夹无法创建"));
            }
            return filePath;
        }

        public static string GetSelectObjectRootPath()
        {
            var path = string.Format("{0}/{1}/{2}/", Application.dataPath, referencePath, GGlobalFun.GetString(Selection.activeTransform.name));
            return path;
        }

        // public static string GetClassName(string suffix)
        // {
        //     return string.Format("{1}_{0}", suffix, GGlobalFun.GetString(Selection.activeTransform.name));
        // }

        // public static string GetInfoPath()
        // {
        //     int id;
        //     var go = PrefabUtility.GetPrefabParent(selectTransform.gameObject);
        //     if (go != null)
        //     {
        //         id = go.GetInstanceID();
        //     }
        //     else
        //     {
        //         id = selectTransform.gameObject.GetInstanceID();
        //     }
        //     //Debug.LogError(InfoPath);
        //     return string.Format(InfoPath, id);
        // }

        public static void Compiling()
        {
            EditorPrefs.SetBool("QConfigureSelectCompiling", true);
        }

        public static bool IsCompiling()
        {
            var value = EditorPrefs.GetBool("QConfigureSelectCompiling", false);
            EditorPrefs.SetBool("QConfigureSelectCompiling", false);
            return value;
        }

        private static string GetFileName(string suffix)
        {
            return string.Format("{0}/{1}_{0}", suffix, GGlobalFun.GetString(Selection.activeTransform.name));
        }

        private static string GetMainFileName()
        {
            return string.Format("{0}", GGlobalFun.GetString(Selection.activeTransform.name));
        }


        public static string GetShortTypeName(string type)
        {
            switch (type)
            {
                case "Image":
                    return "Img";
                case "Button":
                    return "Btn";
                case "Text":
                    return "Txt";
                case "Transform":
                    return "Trans";
                case "GameObject":
                    return "Obj";
                case "InputField":
                    return "Input";
                default:
                    return type;
            }
        }

        private static string[] s_FrontStr = new string[] { "Btn", "Button", "Image", "Img", "Transform", "Trans", "GameObject", "Obj", "Text", "Txt" };
        public static string RemoveFrontTypeName(string name)
        {
            for (int i = 0; i < s_FrontStr.Length; i++)
            {
                if (name.StartsWith(s_FrontStr[i]))
                {
                    name = name.Remove(0, s_FrontStr[i].Length);
                    return name;
                }
                else if (name.EndsWith(s_FrontStr[i]))
                {
                    name = name.Remove(name.Length - s_FrontStr[i].Length, s_FrontStr[i].Length);
                    return name;
                }
            }


            return name;
        }
    }
}