using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using UnityEngine.AddressableAssets;

namespace Qarth.Editor
{


    public class AddressableAssetAutoProcess : AssetPostprocessor
    {
        public static void OnPostprocessAllAssets(string[] importedAsset, string[] deleteAsset, string[] movedAssets, string[] movedFromAssetPaths)
        {
            ProcessImportedAssets(importedAsset);
            //ProcessMovedAsset(movedAssets, movedFromAssetPaths);
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
                    //ProcessAssetBundleTag(assetPath[i], true);
                }
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
    }
}
