// using System.Collections;
// using System.Collections.Generic;
// using UnityEditor;
// using UnityEngine;
// using UnityEngine.UI;
// using System.Text.RegularExpressions;
// using System;
// using Qarth;

// public class EditorResourceSetting : AssetPostprocessor
// {
// #if ResourseSettingEnabled
// #region 模型处理
//     //模型导入之前调用  
//     public void OnPreprocessModel()
//     {
//         Log.i("模型导入之前调用=" + this.assetPath);
//         ModelImporter modelImporter = (ModelImporter)assetImporter;
//         //模型优化
//         //modelImporter.optimizeMesh = true;
//         modelImporter.optimizeGameObjects = true;
//         modelImporter.animationCompression = ModelImporterAnimationCompression.Optimal;
//         modelImporter.animationRotationError = 1.0f;
//         modelImporter.animationPositionError = 1.0f;
//         modelImporter.animationScaleError = 1.0f;
//     }
//     //模型导入之后调用  
//     public void OnPostprocessModel(GameObject go)
//     {

//         // for skeleton animations.
//         Log.i("模型导入之后调用  ");
//         List<AnimationClip> animationClipList = new List<AnimationClip>(AnimationUtility.GetAnimationClips(go));
//         if (animationClipList.Count == 0)
//         {
//             AnimationClip[] objectList = UnityEngine.Object.FindObjectsOfType(typeof(AnimationClip)) as AnimationClip[];
//             animationClipList.AddRange(objectList);
//         }

//         foreach (AnimationClip theAnimation in animationClipList)
//         {
//             try
//             {
//                 //  去除scale曲线
//                 //foreach (EditorCurveBinding theCurveBinding in AnimationUtility.GetCurveBindings(theAnimation))
//                 //{
//                 //    string name = theCurveBinding.propertyName.ToLower();
//                 //    if (name.Contains("scale"))
//                 //    {
//                 //        AnimationUtility.SetEditorCurve(theAnimation, theCurveBinding, null);
//                 //    }
//                 //}

//                 // 浮点数精度压缩到f3
//                 AnimationClipCurveData[] curves = null;
//                 curves = AnimationUtility.GetAllCurves(theAnimation);
//                 Keyframe key;
//                 Keyframe[] keyFrames;
//                 for (int ii = 0; ii < curves.Length; ++ii)
//                 {
//                     AnimationClipCurveData curveDate = curves[ii];
//                     if (curveDate.curve == null || curveDate.curve.keys == null)
//                     {
//                         //Debuger.LogWarning(string.Format("AnimationClipCurveData {0} don't have curve; Animation name {1} ", curveDate, animationPath));
//                         continue;
//                     }
//                     keyFrames = curveDate.curve.keys;
//                     for (int i = 0; i < keyFrames.Length; i++)
//                     {
//                         key = keyFrames[i];
//                         key.value = float.Parse(key.value.ToString("f3"));
//                         key.inTangent = float.Parse(key.inTangent.ToString("f3"));
//                         key.outTangent = float.Parse(key.outTangent.ToString("f3"));
//                         keyFrames[i] = key;
//                     }
//                     curveDate.curve.keys = keyFrames;
//                     theAnimation.SetCurve(curveDate.path, curveDate.type, curveDate.propertyName, curveDate.curve);
//                     Log.i("设定数值");
//                 }
//             }
//             catch (System.Exception e)
//             {
//                 Log.e(string.Format("CompressAnimationClip Failed !!! animationPath : {0} error: {1}", assetPath, e));
//             }
//         }
//     }
// #endregion

// #region 纹理处理

//     /// <summary>
//     /// 检索Texture的类型是否为Sprite
//     /// </summary>
//     private string RetrivalTextureType = @"[_][Uu][Ii]";
//     /// <summary>
//     /// 检索Texture的MaxSize大小
//     /// </summary>
//     private string RetrivalTextureMaxSize = @"[&]\d{2,4}";
//     /// <summary>
//     /// 检索Texture是否为带Alpha通道
//     /// </summary>
//     private string RetrivalTextureIsAlpha = @"[Aa][Ll][Pp][Hh][Aa]";

//     //纹理导入之前调用，针对入到的纹理进行设置
//     public void OnPreprocessTexture()
//     {
//         string dirName = System.IO.Path.GetDirectoryName(assetPath);
//         Log.i(dirName);//从Asset开始的目录信息
//         string folderStr = System.IO.Path.GetFileName(dirName);
//         Log.i(folderStr);//最近文件夹信息
//         TextureImporter textureImporter = (TextureImporter)assetImporter;
//         Log.i(assetPath);//从Asset开始全路径
//         string FileName = System.IO.Path.GetFileName(assetPath);
//         Log.i("导入文件名称：" + FileName);
//         string[] FileNameArray = FileName.Split(new string[] { "_" }, System.StringSplitOptions.RemoveEmptyEntries);

//         //含有UI指定字符，此图片是的类型为Sprite
//         if (Regex.IsMatch(FileName, RetrivalTextureType))
//         {
//             Log.i("含有指定字符");
//             textureImporter.textureType = TextureImporterType.Sprite;
//             textureImporter.mipmapEnabled = false;
//         }
//         else
//         {
//             textureImporter.textureType = TextureImporterType.Default;
//             //textureImporter.mipmapEnabled = true;
//         }

//         //判断是否使用Alpha通道
//         if (Regex.IsMatch(FileName, RetrivalTextureIsAlpha))
//         {
//             textureImporter.alphaIsTransparency = true;
//         }
//         else
//         {
//             textureImporter.alphaIsTransparency = false;
//         }

//         // 设置MaxSize尺寸
//         Regex tempRegex = new Regex(RetrivalTextureMaxSize);
//         if (tempRegex.IsMatch(FileName))
//         {
//             string MaxSizeStr = tempRegex.Match(FileName).Value.Replace("&", "");
//             int TempMaxSize = Convert.ToInt32(MaxSizeStr);
//             if (TempMaxSize < 50)
//             {
//                 TempMaxSize = 32;
//             }
//             else if (TempMaxSize < 100)
//             {
//                 TempMaxSize = 64;
//             }
//             else if (TempMaxSize < 150)
//             {
//                 TempMaxSize = 128;
//             }
//             else if (TempMaxSize < 300)
//             {
//                 TempMaxSize = 256;
//             }
//             else if (TempMaxSize < 600)
//             {
//                 TempMaxSize = 512;
//             }
//             else
//             {
//                 TempMaxSize = 1024;
//             }
//             textureImporter.maxTextureSize = TempMaxSize;
//             Log.i("设置的Texture尺寸为：" + TempMaxSize);
//         }



// #region         -------------------------根据平台分别设置-------------------------------------
//         // Debuger.Log("名称：" + textureImporter.GetDefaultPlatformTextureSettings().name);
//         // TextureImporterPlatformSettings TempTexture = new TextureImporterPlatformSettings();
//         // TempTexture.overridden = true;
//         // TempTexture.name = "Android";
//         // TempTexture.maxTextureSize = 512;
//         // TempTexture.format = TextureImporterFormat.ETC_RGB4;
//         // textureImporter.SetPlatformTextureSettings(TempTexture);
//         // textureImporter.wrapMode = TextureWrapMode.Clamp;


//         //textureImporter.maxTextureSize = 1024;
//         //textureImporter.textureCompression = TextureImporterCompression.Compressed;
//         //textureImporter.crunchedCompression = true;
//         //textureImporter.compressionQuality = 60;
// #endregion
//     }
//     public void OnPostprocessTexture(Texture2D tex)
//     {
//         Log.i("导入" + "tex" + "图片后处理");
//     }

//     void OnPostprocessSprites(Texture2D texture, Sprite[] sprites)
//     {
//         Log.i("Sprites: " + sprites.Length);
//         Log.i("Texture2D: " + texture.name);
//     }
// #endregion

// #region 音频处理
//     public void OnPreprocessAudio()
//     {
//         Log.i("音频导前预处理");

//         AudioImporterSampleSettings AudioSetting = new AudioImporterSampleSettings();
//         //加载方式选择
//         AudioSetting.loadType = AudioClipLoadType.CompressedInMemory;
//         //压缩方式选择
//         AudioSetting.compressionFormat = AudioCompressionFormat.Vorbis;
//         //设置播放质量
//         AudioSetting.quality = 0.1f;
//         //优化采样率
//         AudioSetting.sampleRateSetting = AudioSampleRateSetting.OptimizeSampleRate;


//         AudioImporter audio = assetImporter as AudioImporter;
//         //开启单声道 
//         audio.forceToMono = true;
//         audio.preloadAudioData = true;
//         audio.defaultSampleSettings = AudioSetting;

//     }
//     public void OnPostprocessAudio(AudioClip clip)
//     {
//         Log.i("音频导后处理");
//     }
// #endregion

// #region 其他处理
//     //所有的资源的导入，删除，移动，都会调用此方法，注意，这个方法是static的  （这个是在对应资源的导入前后函数执行后触发）
//     //public static void OnPostprocessAllAssets(string[] importedAsset, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
//     //{
//     //    Debuger.Log("EditorResourceSetting", "有文件处理");

//     //    foreach (string str in importedAsset)
//     //    {
//     //        Debuger.Log("导入文件 = " + str);

//     //    }
//     //    foreach (string str in deletedAssets)
//     //    {
//     //        Debuger.Log("删除文件 = " + str);
//     //    }
//     //    foreach (string str in movedAssets)
//     //    {
//     //        Debuger.Log("移动后文件 = " + str);
//     //    }
//     //    foreach (string str in movedFromAssetPaths)
//     //    {
//     //        Debuger.Log("移动前文件 = " + str);
//     //    }
//     //}
//     /// <summary>
//     /// 在对应的资源已经设置Assetbundle名称后更改其名称触发
//     /// </summary>
//     /// <param name="assetPath"></param>
//     /// <param name="previousAssetBundleName"></param>
//     /// <param name="newAssetBundleName"></param>
//     public void OnPostprocessAssetbundleNameChanged(string assetPath, string previousAssetBundleName, string newAssetBundleName)
//     {
//         Log.i("AssetBundle资源 " + assetPath + " 从名称： " + previousAssetBundleName + " 变更到： " + newAssetBundleName + ".");
//     }

//     /// <summary>
//     /// 模型 max、maya数值相关
//     /// </summary>
//     /// <param name="go"></param>
//     /// <param name="propNames"></param>
//     /// <param name="values"></param>
//     void OnPostprocessGameObjectWithUserProperties(GameObject go, string[] propNames, System.Object[] values)
//     {
//         //for (int i = 0; i < propNames.Length; i++)
//         //{
//         //    string propName = propNames[i];
//         //    System.Object value = (System.Object)values[i];

//         //    Debug.Log("Propname: " + propName + " value: " + values[i]);

//         //    if (value.GetType().ToString() == "System.Int32")
//         //    {
//         //        int myInt = (int)value;
//         //        // do something useful
//         //    }

//         //    // etc...
//         //}
//     }
//     /// <summary>
//     /// 模型 贴图相关
//     /// </summary>
//     /// <param name="material"></param>
//     /// <param name="renderer"></param>
//     public void OnAssignMaterialModel(Material material, Renderer renderer)
//     {
//         //Debug.Log("OnAssignMaterialModel");
//     }

//     //void OnPostprocessMaterial(Material material)
//     //{
//     //    material.color = Color.red;
//     //    Debuger.Log("更改材质球");
//     //}


//     //将该函数添加到子类中，以便在导入模型（.fbx，.mb文件等）的动画之前获取通知。    这使您可以通过代码控制导入设置。
//     void OnPreprocessAnimation()
//     {
//         //Debuger.Log("OnPreprocessAnimation");
//         //var modelImporter = assetImporter as ModelImporter;
//         //modelImporter.clipAnimations = modelImporter.defaultClipAnimations;
//     }

//     //private uint m_Version = 0;
//     //public override uint GetVersion() { return m_Version; }
// #endregion
// #endif
// }