using System.IO;
using UnityEngine;
using UnityEditor;

namespace GFrame.Editor
{

    public class ScriptCreatorEditor : UnityEditor.AssetModificationProcessor
    {
        static string namespaceName = "GameWish.Game";
        //static string namespaceNameEditor = "GFrame.Editor";

        /// <summary>
        /// 将要创建资源时会调用这个函数
        /// </summary>
        static void OnWillCreateAsset(string path)
        {
            string str = path.Replace(".meta", "");

            if (str.EndsWith(".cs"))
            {
                string[] newSplitArgs = str.Split('/');
                bool isEditor = false;
                foreach (var item in newSplitArgs)
                {
                    if (item.Equals("Editor"))
                    {
                        isEditor = true;
                        break; ;
                    }
                }
                if (isEditor) return;
                ParseAndChangeScript(str.Substring(6, str.Length - 6));
            }
        }


        private static void ParseAndChangeScript(string path)
        {
            string str = File.ReadAllText(Application.dataPath + path);
            if (string.IsNullOrEmpty(str))
            {
                Debug.Log("读取出错了，Application.dataPath=" + Application.dataPath + "  path=" + path);
                return;
            }

            string newStr = "";
            newStr += "/************************\n";
            newStr += "\tFileName:" + path + "\n";
            newStr += "\tCreateAuthor:" + System.Environment.UserName + "\n";
            newStr += "\tCreateTime:" + System.DateTime.Now.ToString() + "\n";
            newStr += "************************/\n\n\n";

            //增加命名空间
            if (!str.Contains("namespace"))
            {
                if (!string.IsNullOrEmpty(namespaceName))
                {
                    int length = str.IndexOf("public");
                    newStr += str.Substring(0, length);
                    string extraStr = "";
                    string[] extraStrs = str.Substring(length, str.Length - length).Replace("\r\n", "\n").Split('\n');
                    foreach (var item in extraStrs)
                    {
                        extraStr += "\t" + item + "\r\n";
                    }

                    newStr += "\r\nnamespace " + namespaceName + "\r\n{\r\n" + extraStr + "}";

                }
                else
                {
                    newStr = str;
                }
                File.WriteAllText(Application.dataPath + path, newStr);
            }
        }
    }
}
