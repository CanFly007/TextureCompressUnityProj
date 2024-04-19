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
    private string toolPath;
    private string pngPath;

    void Start()
    {
        //Debug.Log(Application.streamingAssetsPath);
        //Debug.Log(Application.persistentDataPath);
        //Debug.Log(Application.dataPath);
        //Debug.Log(Application.temporaryCachePath);

        string appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        toolPath = Path.Combine(appData, "MyTools/astcenc-sse2.exe");
        Debug.Log("toolPath: " + toolPath);
        //win: C:\Users\56399\AppData\Roaming\MyTools/astcenc-sse2.exe
        //mac: 
        if (!File.Exists(toolPath))
            throw new FileNotFoundException($"Cannot find astc encoder at {Path.GetFullPath(toolPath)}.");

        pngPath = Path.Combine(Application.streamingAssetsPath, "RGBA32.png");
        //pngPath = "E:\\202404\\TextureCompressUnityProj\\Assets\\Textures\\ASTC\\98_OriRGBA32.png";
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
        string outputPath = (extIndex != -1) ? path.Substring(0, extIndex) + ".astc" : path + ".astc";

        ProcessStartInfo startInfo = new ProcessStartInfo
        {
            FileName = toolPath,
            Arguments = $" -cl \"{path}\" \"{outputPath}\" 6x6 -medium -yflip",
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
}
