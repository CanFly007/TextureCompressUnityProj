using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class TextureCompressTools
{
    [MenuItem("Assets/Texture Compress", false, 0)]
    static void TextureCompress()
    {
        string folderPath = Path.Combine(Application.streamingAssetsPath, "TextureCompress");
        if (!Directory.Exists(folderPath))
        {
            Directory.CreateDirectory(folderPath);
        }
        else
        {
            DirectoryInfo di = new DirectoryInfo(folderPath);
            foreach (FileInfo file in di.GetFiles())
            {
                file.Delete();
            }
        }

        foreach (var selectedObj in Selection.objects)
        {
            if (selectedObj is Texture2D)
            {
                Texture2D tex2D = selectedObj as Texture2D;
                byte[] rawData = tex2D.GetRawTextureData();
                tex2D.Compress(true);
                byte[] rawData2 = tex2D.GetRawTextureData();

            }
            else
            {
                Debug.LogError("Selected object " + selectedObj.name + " is not a Texture2D.");
                continue;
            }
        }
    }
}
