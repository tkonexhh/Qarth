using UnityEngine;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System;

public class GFileOperation
{
    /// <summary>
    /// 文件是否存在
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    public static bool IsExists(string path)
    {
        return File.Exists(path);
    }

    /// <summary>
    /// 检查文件夹是否存在
    /// </summary>
    /// <param name="path"></param>
    /// <param name="create"></param>
    /// <returns></returns>
    public static bool IsDirctoryName(string path, bool create = false)
    {
        if (string.IsNullOrEmpty(path))
            return false;

        var dir = Path.GetDirectoryName(path);
        var isExists = Directory.Exists(dir);
        if (create)
        {
            var info = Directory.CreateDirectory(dir);
            if (info != null) isExists = true;
        }
        return isExists;
    }

    /// <summary>
    /// 创建文件夹
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    public static bool CreateDirctory(string path)
    {
        return IsDirctoryName(path, true);
    }

    /// <summary>
    /// 写入文件
    /// </summary>
    /// <param name="filePath"></param>
    /// <param name="data"></param>
    public static void WriteBytes(string filePath, byte[] data)
    {
        CreateDirctory(filePath);
        File.WriteAllBytes(filePath, data);
    }

    /// <summary>
    /// 写入文本
    /// </summary>
    /// <param name="filePath"></param>
    /// <param name="data"></param>
    public static void WriteText(string filePath, string data, FileMode fileMode = FileMode.Create)
    {
        CreateDirctory(filePath);
        using (FileStream stream = new FileStream(filePath, fileMode))
        {
            using (StreamWriter sw = new StreamWriter(stream, Encoding.UTF8))
            {
                sw.Write(data);
                sw.Close();
            }
        }
    }

    /// <summary>
    /// 读取文件
    /// </summary>
    /// <param name="filePath"></param>
    /// <returns></returns>
    public static byte[] ReadBytes(string filePath)
    {
        return File.ReadAllBytes(filePath);
    }

    /// <summary>
    /// 读取文本
    /// </summary>
    /// <param name="filePath"></param>
    /// <returns></returns>
    public static string ReadText(string filePath)
    {
        StreamReader sr = null;
        sr = File.OpenText(filePath);

        string line = sr.ReadToEnd();

        sr.Close();
        sr.Dispose();
        return line;
    }

    /// <summary>
    /// 读取纹理
    /// </summary>
    /// <param name="filePath"></param>
    /// <returns></returns>
    public static Texture2D ReadTexture2D(string filePath)
    {
        Texture2D t = new Texture2D(2, 2, TextureFormat.PVRTC_RGB2, false);
        t.LoadImage(File.ReadAllBytes(filePath));
        return t;
    }

    /// <summary>
    /// 读取精灵
    /// </summary>
    /// <param name="filePath"></param>
    /// <returns></returns>
    public static Sprite ReadSprite(string filePath)
    {
        var t = ReadTexture2D(filePath);
        return Sprite.Create(t, new Rect(0, 0, t.width, t.height), new Vector2(0.5f, 0.5f));
    }

    /// <summary>
    /// 获得根据名字获得文件夹路径
    /// </summary>
    /// <param name="fileName"></param>
    /// <returns></returns>
    public static string GetAssetsDirectoryPath(string fileName)
    {
        DirectoryInfo dir = new DirectoryInfo(Application.dataPath);
        var infos = dir.GetDirectories("*", SearchOption.AllDirectories);
        foreach (var info in infos)
        {
            if (info.FullName.Contains(fileName))
            {
                return info.FullName;
            }
        }
        return string.Empty;
    }

    /// <summary>
    /// 获得文件夹下所有文件名
    /// </summary>
    /// <param name="dirName"></param>
    /// <param name="dirPattern"></param>
    /// <param name="filePattern"></param>
    /// <returns></returns>
    public static string[] GetFilesPath(string dirName, string dirPattern = "*", string filePattern = "*")
    {
        List<string> list = new List<string>();
        DirectoryInfo dir = new DirectoryInfo(dirName);
        string filter = ".meta";
        foreach (var child in dir.GetDirectories(dirPattern, SearchOption.AllDirectories))
        {
            var childDir = new DirectoryInfo(child.FullName);
            foreach (var file in childDir.GetFiles(filePattern, SearchOption.AllDirectories))
            {
                if (file.FullName.Contains(filter)) continue;
                list.Add(file.FullName);
            }
        }
        return list.ToArray();
    }

    /// <summary>
    /// 获得文件路径
    /// </summary>
    /// <param name="dirName"></param>
    /// <param name="suffix"></param>
    /// <param name="fileName"></param>
    /// <returns></returns>
    public static string GetFilePath(string dirName, string suffix, string fileName)
    {
        var dir = new DirectoryInfo(dirName);
        var infos = dir.GetFiles(string.Format("*.{0}", suffix), SearchOption.AllDirectories);
        foreach (var info in infos)
        {
            if (info.Name.Contains(fileName))
            {
                return info.FullName;
            }
        }
        return string.Empty;
    }

    /// <summary>
    /// 获取文件夹大小
    /// </summary>
    /// <param name="dirPath"></param>
    /// <returns></returns>
    public static long GetDirectoryLength(string dirPath)
    {
        //判断给定的路径是否存在,如果不存在则退出
        if (!Directory.Exists(dirPath))
            return 0;
        long len = 0;

        //定义一个DirectoryInfo对象
        DirectoryInfo di = new DirectoryInfo(dirPath);

        //通过GetFiles方法,获取di目录中的所有文件的大小
        foreach (FileInfo fi in di.GetFiles())
        {
            len += fi.Length;
        }

        //获取di中所有的文件夹,并存到一个新的对象数组中,以进行递归
        DirectoryInfo[] dis = di.GetDirectories();
        if (dis.Length > 0)
        {
            for (int i = 0; i < dis.Length; i++)
            {
                len += GetDirectoryLength(dis[i].FullName);
            }
        }
        return len;
    }

    /// <summary>
    /// 读取文件大小
    /// </summary>
    /// <param name="filePath"></param>
    /// <returns></returns>
    public static long FileSize(string filePath)
    {
        long temp = 0;

        //判断当前路径所指向的是否为文件
        if (File.Exists(filePath) == false)
        {
            string[] str1 = Directory.GetFileSystemEntries(filePath);
            foreach (string s1 in str1)
            {
                temp += FileSize(s1);
            }
        }
        else
        {
            FileInfo fileInfo = new FileInfo(filePath);
            return fileInfo.Length;
        }
        return temp;
    }


    /// <summary>
    /// 删除文件夹
    /// </summary>
    /// <param name="srcPath"></param>
    public static void DelectDir(string srcPath)
    {
        try
        {
            if (!Directory.Exists(srcPath))
            {
                DeleteFile(srcPath);
                return;
            }
            DirectoryInfo dir = new DirectoryInfo(srcPath);
            FileSystemInfo[] fileinfo = dir.GetFileSystemInfos();  //返回目录中所有文件和子目录
            foreach (FileSystemInfo i in fileinfo)
            {
                if (i is DirectoryInfo)            //判断是否文件夹
                {
                    DirectoryInfo subdir = new DirectoryInfo(i.FullName);
                    subdir.Delete(true);          //删除子目录和文件
                }
                else
                {
                    File.Delete(i.FullName);      //删除指定文件
                }
            }
        }
        catch (Exception e)
        {
            Debug.LogException(e);
        }
    }

    public static void DeleteFile(string path)
    {
        if (!File.Exists(path)) return;
        try
        {
            File.Delete(path);
        }
        catch (Exception e)
        {
            Debug.Log(e);
        }

    }

}
