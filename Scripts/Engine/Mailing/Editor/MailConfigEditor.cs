//  Desc:        Framework For Game Develop with Unity3d
//  Copyright:   Copyright (C) 2017 SnowCold. All rights reserved.
//  WebSite:     https://github.com/SnowCold/Qarth
//  Blog:        http://blog.csdn.net/snowcoldgame
//  Author:      SnowCold
//  E-mail:      snowcold.ouyang@gmail.com
using System;
using UnityEngine;

using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;

namespace Qarth.Editor
{
    public class MailConfigEditor
    {
        [MenuItem("Assets/Qarth/Config/Build MailConfig")]
        public static void BuildMailConfig()
        {
            MailConfig data = null;
            string folderPath = EditorUtils.GetSelectedDirAssetsPath();
            string dataPath = folderPath + "/MailConfig.asset";

            data = AssetDatabase.LoadAssetAtPath<MailConfig>(dataPath);
            if (data == null)
            {
                data = ScriptableObject.CreateInstance<MailConfig>();
                AssetDatabase.CreateAsset(data, dataPath);
            }
            Log.i("Create Mail Config In Folder:" + dataPath);
            EditorUtility.SetDirty(data);
            AssetDatabase.SaveAssets();
        }
    }
}
