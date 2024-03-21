using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering;

public class TestTextureFloat : MonoBehaviour
{
    public Texture2D texture;
    public float[] buffers = new float[] { 0, 100, 200, 0};
    // Start is called before the first frame update
    void Start()
    {
        CreateTexture();
        GetComponent<Renderer>().material.mainTexture = texture;
    }

    void CreateTexture()
    {
        Texture2D texture = new Texture2D(2, 2, GraphicsFormat.R32_SFloat, TextureCreationFlags.None);
        texture.filterMode = FilterMode.Point;
        texture.wrapMode = TextureWrapMode.Clamp;

        texture.SetPixelData<float>(buffers, 0);
        texture.Apply(updateMipmaps: false);
        this.texture = texture;
    }

    // Update is called once per frame
    void Update()
    {
        if (texture != null) {

            texture.SetPixelData<float>(buffers, 0);
            texture.Apply(updateMipmaps: false);
        }
    }
}
