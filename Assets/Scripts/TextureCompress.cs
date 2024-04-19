using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class TextureCompress : MonoBehaviour 
{
    private string toolPath = "Path/To/astcenc-sse2.exe";
    public Texture2D testTex2D;
    private string pngPath = "Path/To/astcenc-sse2.exe";

    void Start()
    {
        //Debug.Log(Application.streamingAssetsPath);
        //Debug.Log(Application.persistentDataPath);
        //Debug.Log(Application.dataPath);
        //Debug.Log(Application.temporaryCachePath);

        //pngPath = Path.Combine(Application.streamingAssetsPath, "RGBA32.png");
        pngPath = "E:\\202404\\TextureCompressUnityProj\\Assets\\Textures\\ASTC\\98_OriRGBA32.png";


        string appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        toolPath = Path.Combine(appData, "MyTools/astcenc-sse2.exe");

        if (!File.Exists(toolPath))
            throw new FileNotFoundException($"Cannot find astc encoder at {Path.GetFullPath(toolPath)}.");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            CompressTextureAsync(pngPath).Forget();
        }
    }

    async UniTaskVoid CompressTextureAsync(string pngPath)
    {
        await CompressTexture(pngPath);
    }

    async UniTask CompressTexture(string path)
    {
        if (!File.Exists(path))
        {
            Debug.LogError("File is not exists: " + path);
            return;
        }

        int extIndex = path.LastIndexOf('.');
        string outputPath = path.Substring(0, extIndex) + "_astc" + path.Substring(extIndex);

        ProcessStartInfo startInfo = new ProcessStartInfo
        {
            FileName = toolPath,
            Arguments = $"-cl \"{path}\" \"{outputPath}\" 6x6 -medium",
            UseShellExecute = false,
            RedirectStandardOutput = true,
            CreateNoWindow = true
        };

        using (Process process = new Process { StartInfo = startInfo })
        {
            process.Start();
            await UniTask.WaitUntil(() => process.HasExited); 

            if (process.ExitCode == 0)
            {
                Debug.Log("Texture compressed successfully.");
                // 你可以在这里添加代码来处理输出文件
            }
            else
            {
                Debug.LogError("Texture compression failed.");
            }
        }
    }

    //private void ASTC_Compress(string path)
    //{
    //    int extIndex = path.LastIndexOf('.');
    //    string outputPath = path.Substring(0, extIndex) + "_astc" + path.Substring(extIndex);

    //    ProcessStartInfo startInfo = new ProcessStartInfo
    //    {
    //        FileName = toolPath,
    //        Arguments = $"-cl \"{path}\" \"{outputPath}\" 6x6 -medium",
    //        UseShellExecute = false,
    //        RedirectStandardOutput = true,
    //        CreateNoWindow = true
    //    };
    //    Process process = new Process { StartInfo = startInfo };
    //    process.Start();
    //    process.WaitForExit();
    //    if (process.ExitCode == 0)
    //    {
    //        Debug.Log("Texture compressed successfully.");
            
    //    }
    //    else
    //    {
    //        Debug.LogError("Texture compression failed.");
    //    }

    //    process.Close();
    //}
}
