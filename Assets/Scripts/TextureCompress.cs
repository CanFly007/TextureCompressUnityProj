using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextureCompress : MonoBehaviour 
{
    public Texture2D testTex2D;

    void Start()
    {
        var s1 = testTex2D;
        var s2 = new Texture2D(8, 8);
        var s3 = 1 + 1;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
