using UnityEngine;
using UnityEditor;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;

namespace GFrame.Editor
{
    public class TextureProccess : AssetPostprocessor
    {
        /// <summary>
        /// 检索Texture的类型是否为Sprite
        /// </summary>
        private string RetrivalTextureType = @"[Uu][Ii][_]";
        /// <summary>
        /// 检索Texture的MaxSize大小
        /// </summary>
        private string RetrivalTextureMaxSize = @"[&]\d{2,4}";
        /// <summary>
        /// 检索Texture是否为带Alpha通道
        /// </summary>
        private string RetrivalTextureIsAlpha = @"[Aa][Ll][Pp][Hh][Aa]";


        void OnPreprocessTexture()
        {
            TextureImporter importer = assetImporter as TextureImporter;
            if (importer != null)
            {
                if (IsFirstImport(importer))
                {
                    importer.mipmapEnabled = false;
                    importer.isReadable = false;
                    importer.compressionQuality = (int)TextureImporterCompression.Uncompressed;

                    Debug.Log("图片导前处理");
                    string dirName = System.IO.Path.GetDirectoryName(assetPath);
                    string folderStr = System.IO.Path.GetFileName(dirName);
                    string FileName = System.IO.Path.GetFileName(assetPath);

                    if (Regex.IsMatch(FileName, RetrivalTextureType))
                    {
                        Debug.Log("导入UI图片 转为Sprite");
                        importer.textureType = TextureImporterType.Sprite;
                        importer.mipmapEnabled = false;
                    }
                    else
                    {
                        importer.textureType = TextureImporterType.Default;
                    }
                }
            }
        }

        public void OnPostprocessTexture(Texture tex)
        {
            //Debug.Log("图片导后处理");
        }


        //贴图不存在、meta文件不存在、图片尺寸发生修改需要重新导入
        private bool IsFirstImport(TextureImporter importer)
        {
            //(int width, int height) = GetTextureImporterSize(importer);
            Texture tex = AssetDatabase.LoadAssetAtPath<Texture2D>(assetPath);
            bool hasMeta = File.Exists(AssetDatabase.GetAssetPathFromTextMetaFilePath(assetPath));
            return tex == null || !hasMeta;//|| (tex.width != width && tex.height != height);
        }

        // private (int, int) GetTextureImporterSize(TextureImporter importer)
        // {
        //     if (importer != null)
        //     {
        //         object[] args = new object[2];
        //         MethodInfo mi = typeof(TextureImporter).GetMethod("GetWidthAndHeight", BindingFlags.NonPublic | BindingFlags.Instance);
        //         mi.Invoke(importer, args);
        //         return ((int)args[0], (int)args[1]);
        //     }
        //     return (0, 0);
        // }


    }

}