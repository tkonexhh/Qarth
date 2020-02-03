using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Qarth;
using System.IO;
using UnityEditor;

namespace Qarth.Editor
{
    public class MeshHolderBuilder
    {
        [MenuItem("Assets/Qarth Builder/MeshHolder")]
        private static void BuildSpritesDataInFolder()
        {
            string folderPath = EditorUtils.GetSelectedDirAssetsPath();
            DirectoryInfo dInfo = new DirectoryInfo(EditorUtils.AssetsPath2ABSPath(folderPath));
            DirectoryInfo[] subFolders = dInfo.GetDirectories();
            if (subFolders == null || subFolders.Length == 0)
            {
                BuildSpritesData(folderPath);
            }
            else
            {
                for (int i = 0; i < subFolders.Length; ++i)
                {
                    BuildSpritesData(EditorUtils.ABSPath2AssetsPath(subFolders[i].FullName));
                }
                BuildSpritesData(folderPath);
            }
        }

        public static void BuildSpritesData(string folderPath)
        {
            string folderName = PathHelper.FullAssetPath2Name(folderPath);
            string spriteDataPath = folderPath + "/" + folderName + "MeshHolder.asset";

            string workPath = EditorUtils.AssetsPath2ABSPath(folderPath);
            var filePaths = Directory.GetFiles(workPath);

            MeshHolder data = null;

            data = AssetDatabase.LoadAssetAtPath<MeshHolder>(spriteDataPath);

            bool needCreate = false;

            if (data == null)
            {
                data = ScriptableObject.CreateInstance<MeshHolder>();
                needCreate = true;
            }

            data.ClearAllMesh();

            for (int i = 0; i < filePaths.Length; ++i)
            {
                string relPath = EditorUtils.ABSPath2AssetsPath(filePaths[i]);
                UnityEngine.Object[] objs = AssetDatabase.LoadAllAssetsAtPath(relPath);

                if (objs != null)
                {
                    for (int j = 0; j < objs.Length; ++j)
                    {
                        Log.i(objs[j].GetType().Name);

                        if (objs[j] is Mesh)
                        {
                            data.AddMesh(objs[j] as Mesh);
                        }
                    }
                }
            }

            if (needCreate && !data.isEmpty)
            {
                AssetDatabase.CreateAsset(data, spriteDataPath);
            }

            EditorUtility.SetDirty(data);
            AssetDatabase.SaveAssets();
            Log.i("Success Process Mesh Import:" + folderPath);
        }
    }
}
