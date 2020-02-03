using System;
using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using Qarth;
using System.IO;

namespace Qarth.Editor
{
    public class AssetAutoProcess : AssetPostprocessor
    {
        public static void OnPostprocessAllAssets(string[] importedAsset, string[] deleteAsset, string[] movedAssets, string[] movedFromAssetPaths)
        {
            ProcessImportedAssets(importedAsset);
            ProcessMovedAsset(movedAssets, movedFromAssetPaths);
        }

        private static void ProcessMovedAsset(string[] movedAssets, string[] movedFromAssets)
        {
            /*
            if (movedFromAssets != null && movedFromAssets.Length > 0)
            {
                for (int i = 0; i < movedFromAssets.Length; ++i)
                {
                    if (CheckIsRes4AssetBundle(movedFromAssets[i]))
                    {
                        ProcessAssetBundleTag(movedFromAssets[i], false);
                    }
                }
            }
            */
            if (movedAssets != null && movedAssets.Length > 0)
            {
                for (int i = 0; i < movedAssets.Length; ++i)
                {
                    if (CheckIsRes4AssetBundle(movedAssets[i]))
                    {
                        ProcessAssetBundleTag(movedAssets[i], true);
                    }
                    else if (CheckIsRes4Resources(movedAssets[i]))
                    {
                        ProcessAssetBundleTag(movedAssets[i], false);
                    }
                }
            }
        }

        private static void ProcessImportedAssets(string[] assetPath)
        {
            if (assetPath == null || assetPath.Length == 0)
            {
                return;
            }

            for (int i = 0; i < assetPath.Length; ++i)
            {
                if (CheckIsRes4AssetBundle(assetPath[i]))
                {
                    ProcessAssetBundleTag(assetPath[i], true);
                }
            }
        }

        private static void ProcessAssetBundleTag(string assetPath, bool tag)
        {
            AssetImporter ai = AssetImporter.GetAtPath(assetPath);
            if (ai == null)
            {
                Log.e("Not Find Asset:" + assetPath);
                return;
            }

            string fullPath = EditorUtils.AssetsPath2ABSPath(assetPath);
            if (Directory.Exists(fullPath))
            {
                return;
            }

            if (tag)
            {
                string dirName = Path.GetDirectoryName(assetPath);
                string assetBundleName = EditorUtils.AssetPath2ReltivePath(dirName).ToLower(); //EditUtils.GetReltivePath4AssetPath(folderPath).ToLower();
                assetBundleName = assetBundleName.Replace("resources/", "");

                if (assetPath.Contains("FolderMode"))
                {
                    ai.assetBundleName = assetBundleName + ".bundle";
                }
                else
                {
                    ai.assetBundleName = string.Format("{0}/{1}.bundle", assetBundleName, PathHelper.FileNameWithoutSuffix(Path.GetFileName(assetPath)));
                }
            }
            else
            {
                ai.assetBundleName = string.Empty;
            }
        }

        private static bool CheckIsRes4AssetBundle(string name)
        {
            if (name.StartsWith("Assets/") && name.Contains("/Res/"))
            {
                return true;
            }

            return false;
        }

        private static bool CheckIsRes4Resources(string name)
        {
            if (name.StartsWith("Assets/") && name.Contains("/Resources/"))
            {
                return true;
            }

            return false;
        }

        private static void LogStringArray(string name, string[] data)
        {
            if (data == null)
            {
                return;
            }

            for (int i = 0; i < data.Length; ++i)
            {
                Log.i(string.Format("{0}:{1}", name, data[i]));
            }
        }
    }
}
