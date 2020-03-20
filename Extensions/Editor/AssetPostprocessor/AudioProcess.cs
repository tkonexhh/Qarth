using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

public class AudioProcess : AssetPostprocessor
{
    /*
    .AIFF 适用于较短的音乐文件可用作游戏打斗音效
    .WAV 适用于较短的音乐文件可用作游戏打斗音效
    .MP3 适用于较长的音乐文件可用作游戏背景音乐
    .OGG 适用于较长的音乐文件可用作游戏背景音乐

    音频格式(Audio Format)： 在运行时被应用到声音上的特定格式。

    原生的(Native):较大文件尺寸，质量较高。最适用于很短的音效。
    压缩的(Compressed):较小文件尺寸，质量较低/不稳定。最适用于中等长度音效与音乐。
    三维声音(3D Sound)：如果启用，音乐将在3D空间中播放。

    强制单声道(Force to mono)：如果启用，该音频剪辑将向下混合到单通道声音。

    加载类型(Load Type)： 运行时Unity加载音频的方法。

    加载时解压缩(Decompress on load): 加载后解压缩声音。使用于较小的压缩声音，以避免运行时解压缩的性能开销。(将使用比在它们在内存中压缩的多10倍或更多内存，因此大文件不要使用这个。)
    在内存中压缩(Compressed in memory): 保持声音在内存中（压缩的）在播放时解压缩。这有轻微的性能开销（尤其是OGG / Vorbis格式的压缩文件），因此大文件使用这个。
    从磁盘流(Stream from disc): 直接从磁盘流读取音频数据。这只使用了原始声音占内存大小的很小一部分。使用这个用于很长的音乐。取决于硬件，一般建议1-2线程同时流。
    压缩(Compression)： 压缩量应用于压缩的剪辑,保持在一个足够小的尺寸，以满足您的文件大小/分配需要。可以拖动滑条来调整大小，播放声音来判断音质，滑动条后面可以看到文件大小的统计。

    硬件解码(Hardware Decoding)：（仅iOS）用于iOS设备压缩的音频。使用苹果的硬件解码来减少CPU的密集解压缩。（同一时间只能有一个硬件音频流解压缩，包括背景的iPod音频。）

    无缝循环(Gapless looping)：（仅Android/iOS）压缩一个完美的循环音频源文件（非压缩的PCM格式）保持循环。标准的MPEG编码器插入silence循环点，播放时有小的"click" 或 "pop" Unity为你顺畅处理这个问题。

         */


    #region 音频处理
    public void OnPreprocessAudio()
    {
        AudioImporter importer = assetImporter as AudioImporter;
        if (importer != null)
        {
            if (IsFirstImport(importer))
            {
                AudioImporterSampleSettings AudioSetting = new AudioImporterSampleSettings();
                // //加载方式选择
                AudioSetting.loadType = AudioClipLoadType.CompressedInMemory;
                // //压缩方式选择
                AudioSetting.compressionFormat = AudioCompressionFormat.Vorbis;
                // //设置播放质量
                AudioSetting.quality = 0.1f;
                // //优化采样率
                AudioSetting.sampleRateSetting = AudioSampleRateSetting.OptimizeSampleRate;

                // //开启单声道 
                importer.forceToMono = true;
                importer.loadInBackground = false;
                importer.preloadAudioData = true;
                importer.defaultSampleSettings = AudioSetting;
                Debug.Log("音频导前预处理");
            }
        }



    }
    public void OnPostprocessAudio(AudioClip clip)
    {
        //Debug.Log("音频导后处理");
    }


    private bool IsFirstImport(AudioImporter importer)
    {
        //(int width, int height) = GetTextureImporterSize(importer);
        AudioClip asset = AssetDatabase.LoadAssetAtPath<AudioClip>(assetPath);
        bool hasMeta = File.Exists(AssetDatabase.GetAssetPathFromTextMetaFilePath(assetPath));
        return asset == null || !hasMeta;//|| (tex.width != width && tex.height != height);
    }
    #endregion
}
