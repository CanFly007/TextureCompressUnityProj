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
        string appData = Path.Combine(Application.dataPath, "Tools");
        switch (Application.platform)
        {
            case RuntimePlatform.WindowsPlayer:
            case RuntimePlatform.WindowsEditor:
                toolPath = Path.Combine(appData, "astcenc-sse2.exe");
                break;
            case RuntimePlatform.OSXPlayer:
            case RuntimePlatform.OSXEditor:
                toolPath = Path.Combine(appData, "astcenc-sse2-arm64");
                break;
        }
        if (!File.Exists(toolPath))
            throw new FileNotFoundException($"Cannot find astc encoder at {Path.GetFullPath(toolPath)}.");

        //图片
        pngPath = Path.Combine(Application.dataPath, "Images", "RGBA32.png");
        if (!File.Exists(pngPath))
        {
            throw new FileNotFoundException($"Cannot find image at {Path.GetFullPath(pngPath)}.");
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            AstcCompressAsync(pngPath).Forget();
        }
    }

    async UniTaskVoid AstcCompressAsync(string pngPath)
    {
        await AstcCompress(pngPath);
    }

    async UniTask AstcCompress(string path)
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
            }
            else
            {
                Debug.LogError("Texture compression failed.");
            }
        }
    }
}
