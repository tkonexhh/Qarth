using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Qarth;
using System.IO;
using UnityEditor;

namespace Qarth.Editor
{
    public class AudioClipDataBuilder
    {
        [MenuItem("Assets/Qarth Builder/AudioClipData")]
        private static void BuildAudioClipsInFolder()
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
            string spriteDataPath = folderPath + "/" + folderName + "AudioClipData.asset";

            string workPath = EditorUtils.AssetsPath2ABSPath(folderPath);
            var filePaths = Directory.GetFiles(workPath);

            AudioClipData data = null;

            data = AssetDatabase.LoadAssetAtPath<AudioClipData>(spriteDataPath);

            bool needCreate = false;

            if (data == null)
            {
                data = ScriptableObject.CreateInstance<AudioClipData>();
                needCreate = true;
            }

            data.ClearAllData();

            for (int i = 0; i < filePaths.Length; ++i)
            {
                string relPath = EditorUtils.ABSPath2AssetsPath(filePaths[i]);
                UnityEngine.Object[] objs = AssetDatabase.LoadAllAssetsAtPath(relPath);

                if (objs != null)
                {
                    for (int j = 0; j < objs.Length; ++j)
                    {
                        Log.i(objs[j].GetType().Name);

                        if (objs[j] is AudioClip)
                        {
                            data.AddAudioClip(objs[j] as AudioClip);
                        }
                    }
                }
            }

            data.SortAsFileName();

            if (needCreate && !data.isEmpty)
            {
                AssetDatabase.CreateAsset(data, spriteDataPath);
            }

            EditorUtility.SetDirty(data);
            AssetDatabase.SaveAssets();
            Log.i("Success Process AudioClip Import:" + folderPath);
        }
    }
}
