using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace GFrame.Editor
{

    public class PathTools
    {
        [MenuItem("Tools/Path/Open Asset Path")]
        private static void OpenAssetPath()
        {
            Application.OpenURL("file:///" + Application.dataPath);
        }

        [MenuItem("Tools/Path/Open Peristent Path")]
        private static void OpenPeristentPath()
        {
            Debug.LogError(Application.persistentDataPath);
            Application.OpenURL("file:///" + Application.persistentDataPath);
        }
    }
}
